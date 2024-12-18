﻿using PetCare.DataAccess;
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
        CatDto AddCat(CatDto catDTO, string userId);
    }

    internal class CatsService : ICatsService
    {
        private readonly PetDbContext _dbContext;

        public CatsService(PetDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public CatDto AddCat(CatDto catDTO, string userId)
        {
            var catEntity = MapCatEntity(catDTO, userId);
            _dbContext.Cats.Add(catEntity);
            _dbContext.SaveChanges();
            return MapCatDTO(catEntity);
        }

        private Cat MapCatEntity(CatDto catDTO, string userId)
        {
            if (catDTO == null)
            {
                return null;
            }
            return new Cat { Id = catDTO.Id, Name = catDTO.Name, DateOfBirth = catDTO.DateOfBirth, UserId = userId, Sex = catDTO.Sex };
        }

        private CatDto MapCatDTO(Cat cat)
        {
            if (cat == null)
            {
                return null;
            }
            return new CatDto { Id = cat.Id, Name = cat.Name, DateOfBirth = cat.DateOfBirth, PetType = Shared.DTOs.Pets.PetType.Cat, Sex = cat.Sex };
        }
    }
}
