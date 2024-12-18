using PetCare.DataAccess;
using PetCare.Shared.DTOs.Pets;
using PetCare.Shared.Entities.Pets;
using PetCare.Shared.Exceptions.Pets;

namespace PetCare.BusinessLogic.Services;

public interface IPetService
{
    void VerifyUserCanAccessPet(string userId, Guid petId);
    void DeletePet(Guid id, string userId);
    Pet GetPet(Guid id);
    IEnumerable<PetDto> GetUserPets(string userId);
    void UpdatePet(PetDto pet);
}

public class PetService : IPetService
{
    private readonly PetDbContext _dbContext;

    public PetService(PetDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public IEnumerable<PetDto> GetUserPets(string userId)
    {
        var result = _dbContext.Pets.Where(p => p.UserId == userId).Select(p => PetDto.GetDTO(p)).ToList();
        return result;
    }

    public void VerifyUserCanAccessPet(string userId, Guid petId)
    {
        var pet = _dbContext.Pets.FirstOrDefault(p => p.Id == petId);
        if (pet == null)
        {
            throw new PetNotFoundException(petId);
        }
        if (pet.UserId != userId)
        {
            throw new PetOwnershipException();
        }
    }

    public void DeletePet(Guid id, string userId)
    {
        var dbPet = _dbContext.Pets.FirstOrDefault(p => p.Id == id);
        if (dbPet == null)
        {
            throw new PetNotFoundException(id);
        }
        if (dbPet.UserId != userId)
        {
            throw new PetOwnershipException();
        }
        _dbContext.Pets.Remove(dbPet);
        _dbContext.SaveChanges();
    }


    public Pet GetPet(Guid id)
    {
        var dbPet = _dbContext.Pets.FirstOrDefault(p => p.Id == id);
        if (dbPet == null)
        {
            throw new PetNotFoundException(id);
        }
        return dbPet;
    }

    public void UpdatePet(PetDto pet)
    {
        var dbPet = this.GetPet(pet.Id);
        dbPet.Name = pet.Name;
        dbPet.DateOfBirth = pet.DateOfBirth;
        dbPet.Sex = pet.Sex;
        _dbContext.SaveChanges();
    }
}