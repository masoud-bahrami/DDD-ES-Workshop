using Microsoft.EntityFrameworkCore;

namespace Zero.DataBase
{
    public class DbContextInterceptor : IDbContextInterceptor
    {
        private readonly ZeroDbContext _zeroDbContext;

        public DbContextInterceptor(ZeroDbContext zeroDbContext) 
            => _zeroDbContext = zeroDbContext;

        public Task Start()
        {
            //
            return Task.CompletedTask;
        }

        public async Task Commit()
            => await _zeroDbContext.SaveChangesAsync();

        public async Task RoleBack()
        {
            var context = _zeroDbContext;
            var changedEntries = context.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Detached:
                    case EntityState.Unchanged:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}