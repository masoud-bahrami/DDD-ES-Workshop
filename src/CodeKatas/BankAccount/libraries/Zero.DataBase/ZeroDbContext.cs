using Microsoft.EntityFrameworkCore;

namespace Zero.DataBase;

public class ZeroDbContext : DbContext
{
    public ZeroDbContext(DbContextOptions<ZeroDbContext> options):base(options)
    {
    }
}