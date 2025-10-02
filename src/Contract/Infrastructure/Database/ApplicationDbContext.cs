using Microsoft.EntityFrameworkCore;

namespace Contract.Infrastructure.Database;

public class ApplicationDbContext<T>(DbContextOptions<T> options) : DbContext(options) where T : DbContext
{

}
