namespace Zero.DataBase
{
    public interface IDbContextInterceptor
    {
        Task Start();
        Task Commit();
        Task RoleBack();
    }
}