using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PetCare.DataAccess;
using PetCare.Shared.DTOs;
using PetCare.Shared.Entities;
using PetCare.Shared.Exceptions.Vaccines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCare.BusinessLogic.Services
{
    public interface IVaccinesService
    {
        void AddVaccine(Vaccine vaccine);
        void DeleteVaccine(Vaccine vaccine);
        Vaccine GetVaccine(Guid id);
        IEnumerable<Vaccine> GetVaccinesForPet(Guid petId);
        Vaccine UpdateVaccine(VaccineDTO vaccineDTO);
        void SendVaccineReminder();
    }

    internal class VaccinesService : IVaccinesService
    {
        private readonly PetDbContext _dbContext;

        public VaccinesService(PetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddVaccine(Vaccine vaccine)
        {
            _dbContext.Vaccines.Add(vaccine);
            _dbContext.SaveChanges();
        }

        public void DeleteVaccine(Vaccine vaccine)
        {
            _dbContext.Vaccines.Remove(vaccine);
            _dbContext.SaveChanges();
        }

        public Vaccine GetVaccine(Guid id)
        {
            var vaccine = _dbContext.Vaccines.FirstOrDefault(v => v.Id == id);
            if (vaccine == null)
            {
                throw new VaccineNotFoundException(id);
            }
            return vaccine;
        }

        public IEnumerable<Vaccine> GetVaccinesForPet(Guid petId)
        {
            var vaccines = _dbContext.Vaccines.Where(v => v.PetId == petId).ToList();
            return vaccines;
        }

        public void SendVaccineReminder()
        {
            var allVaccines = _dbContext.Vaccines.ToList();
            var vaccines = _dbContext.Vaccines.Where(v => v.NextDueDate != null && v.NextDueDate > DateTime.UtcNow.Date && v.NextDueDate <= DateTime.UtcNow.AddDays(1).Date).ToList();
            var filteredTest = allVaccines.Where(v => v.NextDueDate == DateTime.UtcNow.Date.AddDays(1)).ToList();
            var x = vaccines;
        }

        public Vaccine UpdateVaccine(VaccineDTO vaccineDTO)
        {
            var vaccine = _dbContext.Vaccines.FirstOrDefault(v => v.Id ==  vaccineDTO.Id);
            if (vaccine == null)
            {
                throw new VaccineNotFoundException(vaccineDTO.Id);
            }
            vaccine.Name = vaccineDTO.Name;
            vaccine.Notes = vaccineDTO.Notes;
            vaccine.NextDueDate = vaccineDTO.NextDueDate;
            vaccine.AdministrationDate = vaccineDTO.AdministrationDate;
            _dbContext.SaveChanges();
            return vaccine;
        }
    }
}
