using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PetCare.DataAccess;
using PetCare.Email;
using PetCare.Shared.Config;
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
        private readonly IEmailService _emailService;

        public VaccinesService(PetDbContext dbContext, IEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
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
            var vaccines = _dbContext.Vaccines.Include(v => v.Pet).Where(v => v.NextDueDate != null && v.NextDueDate > DateTime.UtcNow.Date && v.NextDueDate <= DateTime.UtcNow.AddDays(1).Date).ToList();
            foreach (var vaccine in vaccines)
            {
                var pet = vaccine.Pet;
                if (pet == null)
                {
                    continue;
                }
                var user = _dbContext.Users.FirstOrDefault(u => u.Id == pet.UserId);
                if (user == null || user.Email == null || user.UserName == null)
                {
                    continue;
                }
                _emailService.SendEmail(new Email.Models.MailData
                {
                    Email = user.Email,
                    Name = user.UserName,
                    Subject = $"{pet.Name}'s Vaccine is Due Soon!",
                    Body = $@"Hi {user.UserName},
                    <br><br>
                    Just a friendly reminder that {pet.Name}'s vaccine is due on {vaccine.NextDueDate:MMMM dd, yyyy}.
                    <br><br>
                    Don't forget to schedule an appointment with your vet!
                    <br><br>
                    Best,
                    <br>
                    PetCare"
                }, MimeKit.Text.TextFormat.Html);
            }
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
