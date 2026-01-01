using Microsoft.EntityFrameworkCore;

namespace Ordering.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    
}