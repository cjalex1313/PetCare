using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PetCare.Shared.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PetCare.DataAccess.DbMappings;
public static class EntityMappings
{
    public static void MapUserRefreshToken(EntityTypeBuilder<UserRefreshToken> entity)
    {
        entity.HasKey(e => e.UserId);
        entity.Property(e => e.RefreshToken).IsRequired();
        entity.Property(e => e.ExpieryDate).IsRequired();
    }
}