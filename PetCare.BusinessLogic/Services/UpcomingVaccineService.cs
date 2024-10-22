using Microsoft.EntityFrameworkCore;
using PetCare.DataAccess;
using PetCare.Email;
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
    public interface IUpcomingVaccinesService
    {
        void AddUpcomingVaccine(UpcomingVaccine vaccine);
        void DeleteUpcomingVaccine(UpcomingVaccine vaccine);
        UpcomingVaccine GetUpcomingVaccine(Guid id);
        IEnumerable<UpcomingVaccine> GetUpcomingVaccinesForPet(Guid petId);
        UpcomingVaccine UpdateUpcomingVaccine(UpcomingVaccineDTO vaccineDTO);
        void SendVaccineReminder();
    }

    internal class UpcomingVaccinesService : IUpcomingVaccinesService
    {
        private readonly PetDbContext _dbContext;
        private readonly IEmailService _emailService;

        public UpcomingVaccinesService(PetDbContext dbContext, IEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }

        public void AddUpcomingVaccine(UpcomingVaccine vaccine)
        {
            _dbContext.UpcomingVaccines.Add(vaccine);
            _dbContext.SaveChanges();
        }

        public void DeleteUpcomingVaccine(UpcomingVaccine vaccine)
        {
            _dbContext.UpcomingVaccines.Remove(vaccine);
            _dbContext.SaveChanges();
        }

        public UpcomingVaccine GetUpcomingVaccine(Guid id)
        {
            var vaccine = _dbContext.UpcomingVaccines.FirstOrDefault(v => v.Id == id);
            if (vaccine == null)
            {
                throw new VaccineNotFoundException(id);
            }
            return vaccine;
        }

        public IEnumerable<UpcomingVaccine> GetUpcomingVaccinesForPet(Guid petId)
        {
            var vaccines = _dbContext.UpcomingVaccines.Where(v => v.PetId == petId).ToList();
            return vaccines;
        }

        public void SendVaccineReminder()
        {
            var vaccines = _dbContext.UpcomingVaccines.Include(v => v.Pet).Where(v => v.Date > DateTime.UtcNow.Date && v.Date <= DateTime.UtcNow.AddDays(1).Date).ToList();
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
                    Just a friendly reminder that {pet.Name}'s vaccine is due on {vaccine.Date:MMMM dd, yyyy}.
                    <br><br>
                    Don't forget to schedule an appointment with your vet!
                    <br><br>
                    Best,
                    <br>
                    PetCare"
                }, MimeKit.Text.TextFormat.Html);
            }
        }

        public UpcomingVaccine UpdateUpcomingVaccine(UpcomingVaccineDTO vaccineDTO)
        {
            var vaccine = _dbContext.UpcomingVaccines.FirstOrDefault(v => v.Id == vaccineDTO.Id);
            if (vaccine == null)
            {
                throw new VaccineNotFoundException(vaccineDTO.Id);
            }
            vaccine.Name = vaccineDTO.Name;
            vaccine.Notes = vaccineDTO.Notes;
            vaccine.Date = vaccineDTO.Date;
            _dbContext.SaveChanges();
            return vaccine;
        }
    }
}
