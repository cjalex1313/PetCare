using PetCare.DataAccess;
using PetCare.Shared.DTOs.Pets.Cats;
using PetCare.Shared.Entities.Pets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCare.BusinessLogic.Services
{
    public interface ICatsService
    {
        CatDto AddCat(CatDto catDto, string userId);
    }

    internal class CatsService : ICatsService
    {
        private readonly PetDbContext _dbContext;

        public CatsService(PetDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public CatDto AddCat(CatDto catDto, string userId)
        {
            var catEntity = MapCatEntity(catDto, userId);
            _dbContext.Cats.Add(catEntity);
            _dbContext.SaveChanges();
            return MapCatDto(catEntity);
        }

        private Cat MapCatEntity(CatDto catDto, string userId)
        {
            return new Cat { Id = catDto.Id, Name = catDto.Name, DateOfBirth = catDto.DateOfBirth, UserId = userId, Sex = catDto.Sex };
        }

        private static CatDto MapCatDto(Cat cat)
        {
            return new CatDto { Id = cat.Id, Name = cat.Name, DateOfBirth = cat.DateOfBirth, PetType = Shared.DTOs.Pets.PetType.Cat, Sex = cat.Sex };
        }
    }
}
