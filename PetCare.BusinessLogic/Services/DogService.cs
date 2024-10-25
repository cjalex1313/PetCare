using Microsoft.EntityFrameworkCore;
using PetCare.DataAccess;
using PetCare.Shared.DTOs.Pets.Cats;
using PetCare.Shared.DTOs.Pets.Dogs;
using PetCare.Shared.Entities.Pets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCare.BusinessLogic.Services
{
    public interface IDogService
    {
        DogDto AddDog(DogDto dogDTO, string userID);
    }

    internal class DogService : IDogService
    {
        private readonly PetDbContext _dbContext;

        public DogService(PetDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public DogDto AddDog(DogDto dogDTO, string userID)
        {
            var dogEntity = MapDogEntity(dogDTO, userID);
            _dbContext.Dogs.Add(dogEntity);
            _dbContext.SaveChanges();
            return MapDogDTO(dogEntity);
        }

        private Dog MapDogEntity(DogDto dto, string userID) {
            return new Dog { Id = dto.Id, Name = dto.Name, DateOfBirth = dto.DateOfBirth, UserId = userID, Sex = dto.Sex };
        }

        private DogDto MapDogDTO(Dog dog)
        {
            return new DogDto { Id = dog.Id, Name = dog.Name, DateOfBirth = dog.DateOfBirth, PetType = Shared.DTOs.Pets.PetType.Dog, Sex = dog.Sex };
        }
    }
}
