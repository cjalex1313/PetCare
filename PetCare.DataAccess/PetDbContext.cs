using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCare.Shared.Entities.Auth;
using PetCare.DataAccess.DbMappings;
using PetCare.Shared.Entities.Pets;

namespace PetCare.DataAccess
{
    public class PetDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public DbSet<Pet> Pets { get; set; }

        public PetDbContext(DbContextOptions<PetDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            EntityMappings.MapUserRefreshToken(builder.Entity<UserRefreshToken>());
            EntityMappings.MapPets(builder.Entity<Pet>());
            EntityMappings.MapCats(builder.Entity<Cat>());
            EntityMappings.MapDogs(builder.Entity<Dog>());
        }
    }
}
