using System;
using System.Threading.Tasks;
using Zero.DataBase;

namespace Zero.EventSourcing.SqlServerProjector;

public class DbAddOperation<T> : DbOperationCommand
    where T : class
{
    private static Action<T> _action;
    private readonly ZeroDbContext _zeroDbContext;
    private readonly T _initialState;

    public DbAddOperation(ZeroDbContext zeroDbContext, T initialState)
    {
        _zeroDbContext = zeroDbContext;
        _initialState = initialState;
    }

    public DbAddOperation<T> Add(Action<T> action)
    {
        _action = action;
        return this;
    }

    public override async Task Execute()
    {
        _action.Invoke(_initialState);

        var dbSet = _zeroDbContext.Set<T>();

        await dbSet.AddAsync(_initialState);
        await _zeroDbContext.SaveChangesAsync();
    }
}