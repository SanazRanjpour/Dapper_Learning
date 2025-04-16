using Microsoft.EntityFrameworkCore;

namespace Dapper_Learn.Models
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) 
        {
            
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().Ignore(t => t.Employees);
            modelBuilder.Entity<Employee>()
                .HasOne(t => t.Company)
                .WithMany(t => t.Employees)
                .HasForeignKey(t => t.CompanyId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
