using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCare.Shared.Entities.Auth;
using PetCare.DataAccess.DbMappings;
using PetCare.Shared.Entities.Pets;
using PetCare.Shared.Entities;

namespace PetCare.DataAccess
{
    public class PetDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public DbSet<FacebookUser> FacebookUsers { get; set; }
        public DbSet<GoogleUser> GoogleUsers { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Cat> Cats { get; set; }
        public DbSet<Dog> Dogs { get; set; }
        public DbSet<Vaccine> Vaccines { get; set; }
        public DbSet<UpcomingVaccine> UpcomingVaccines { get; set; }

        public PetDbContext(DbContextOptions<PetDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            EntityMappings.MapUserRefreshToken(builder.Entity<UserRefreshToken>());
            EntityMappings.MapPets(builder.Entity<Pet>());
            EntityMappings.MapCats(builder.Entity<Cat>());
            EntityMappings.MapDogs(builder.Entity<Dog>());
            EntityMappings.MapVaccines(builder.Entity<Vaccine>());
            EntityMappings.MapUpcomingVaccines(builder.Entity<UpcomingVaccine>());
            EntityMappings.MapFacebookUser(builder.Entity<FacebookUser>());
            EntityMappings.MapGoogleUser(builder.Entity<GoogleUser>());
            EntityMappings.MapUserProfile(builder.Entity<UserProfile>());
        }
    }
}
