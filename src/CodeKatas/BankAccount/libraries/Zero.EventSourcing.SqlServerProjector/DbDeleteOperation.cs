using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zero.DataBase;

namespace Zero.EventSourcing.SqlServerProjector;

public class DbDeleteOperation<T> : DbOperationCommand
    where T : class
{
    private readonly ZeroDbContext _zeroDbContext;
    private readonly Expression<Func<T, bool>> _prediction;

    public DbDeleteOperation(ZeroDbContext ZeroDbContext, Expression<Func<T, bool>> prediction)
    {
        _zeroDbContext = ZeroDbContext;
        _prediction = prediction;
    }

    public override async Task Execute()
    {
        var dbSet = _zeroDbContext.Set<T>();
        var entity = await dbSet.Where(_prediction).ToListAsync();

        if (entity == null || !entity.Any())
            return;

        dbSet.RemoveRange(entity);

        await _zeroDbContext.SaveChangesAsync();
            
    }
}