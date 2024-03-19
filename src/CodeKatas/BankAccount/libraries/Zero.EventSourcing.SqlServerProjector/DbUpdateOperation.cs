using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zero.DataBase;

namespace Zero.EventSourcing.SqlServerProjector;

public class DbUpdateOperation<T> : DbOperationCommand
    where T : class
{
    private readonly ZeroDbContext _zeroDbContext;

    private Action<T> _action { get; set; }

    private Expression<Func<T, bool>> _prediction { get; }

    public DbUpdateOperation(ZeroDbContext zeroDbContext, Expression<Func<T, bool>> prediction)
    {
        _zeroDbContext = zeroDbContext;
        _prediction = prediction;
    }


    public DbUpdateOperation<T> With(Action<T> action)
    {
        _action = action;
        return this;
    }

    public override async Task Execute()
    {
        var dbSet = _zeroDbContext.Set<T>();

        var entity = await dbSet.FirstOrDefaultAsync(_prediction);

        if (entity is not null)
        {
            _action.Invoke(entity);
            dbSet.Update(entity);
        }
        else
        {
            // give it another try !
            // TODO danger zone!

            var entityBeingTracked = _zeroDbContext.ChangeTracker.Entries().FirstOrDefault(e => e.Entity.GetType()
                                                                      == typeof(T));

            entity = entityBeingTracked as T ?? throw new EntityNotFoundException(typeof(T));
            
            _action.Invoke(entity);
        }
        
        await _zeroDbContext.SaveChangesAsync();
    }
        
}