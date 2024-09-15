using PetCare.DataAccess;
using PetCare.Shared.DTOs.Pets;
using PetCare.Shared.Entities.Pets;
using PetCare.Shared.Exceptions.Pets;

namespace PetCare.BusinessLogic.Services;

public interface IPetService
{
    void DeletePet(Guid id, string userId);
    Pet GetPet(Guid id);
    IEnumerable<PetDTO> GetUserPets(string userId);
}

public class PetService : IPetService
{
    private readonly PetDbContext _dbContext;

    public PetService(PetDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public IEnumerable<PetDTO> GetUserPets(string userId)
    {
        var result = _dbContext.Pets.Where(p => p.UserId == userId).Select(p => PetDTO.GetDTO(p)).ToList();
        return result ?? new List<PetDTO>();
    }

    public void DeletePet(Guid id, string userId)
    {
        var dbPet = _dbContext.Pets.FirstOrDefault(p => p.Id == id);
        if (dbPet == null)
        {
            throw new PetNotFoundExpcetion(id);
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
            throw new PetNotFoundExpcetion(id);
        }
        return dbPet;
    }
}