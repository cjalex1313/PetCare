using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PetCare.Shared.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using PetCare.Shared.Entities.Pets;
using Microsoft.AspNetCore.Identity;
using PetCare.Shared.Entities;

namespace PetCare.DataAccess.DbMappings;
public static class EntityMappings
{
    public static void MapUserRefreshToken(EntityTypeBuilder<UserRefreshToken> entity)
    {
        entity.HasKey(e => e.UserId);
        entity.Property(e => e.RefreshToken).IsRequired();
        entity.Property(e => e.ExpieryDate).IsRequired();
    }

    public static void MapPets(EntityTypeBuilder<Pet> entity)
    {
        entity.ToTable("Pets");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
        entity.Property(e => e.DateOfBirth).HasColumnType("date");
        entity.HasOne<IdentityUser>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
        entity.HasIndex(e => e.UserId);
    }

    public static void MapCats(EntityTypeBuilder<Cat> entity)
    {
        entity.ToTable("Cats");
    }

    public static void MapDogs(EntityTypeBuilder<Dog> entity)
    {
        entity.ToTable("Dogs");
    }

    public static void MapVaccines(EntityTypeBuilder<Vaccine> entity)
    {
        entity.ToTable("Vaccines");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).HasMaxLength(128);
        entity.Property(e => e.AdministrationDate).HasColumnType("date");
        entity.Property(e => e.NextDueDate).HasColumnType("date");
        entity.HasIndex(e => e.NextDueDate);
        entity.HasOne(e => e.Pet).WithMany(p => p.Vaccines).HasForeignKey(e => e.PetId).OnDelete(DeleteBehavior.Cascade);
    }
}