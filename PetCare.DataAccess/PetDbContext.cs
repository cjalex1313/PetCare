using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PetCare.DataAccess
{
    public class PetDbContext : IdentityDbContext<IdentityUser>
    {
        public PetDbContext(DbContextOptions<PetDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
