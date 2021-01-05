using LM.DAL.Implement;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Database
{
    public class LibraryManagementDbContext : IdentityDbContext<ApplicationUser>
    {
        public LibraryManagementDbContext(DbContextOptions<LibraryManagementDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
