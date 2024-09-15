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
        var result = _dbContext.Pets.Where(p => p.UserId == userId).Select(p => new PetDTO
        {
            Id = p.Id,
            Name = p.Name,
            DateOfBirth = p.DateOfBirth,
            PetType = MapPetType(p.GetType())
        }).ToList();
        return result ?? new List<PetDTO>();
    }

    public void DeletePet(Guid id, string userId)
    {
        var dbPet = _dbContext.Pets.FirstOrDefault(p => p.Id == id);
        if(dbPet == null)
        {
            throw new PetNotFoundExpcetion(id);
        }
        if (dbPet.UserId != userId) {
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

    private static PetType MapPetType(Type type)
    {
        if (type == typeof(Dog))
        {
            return PetType.Dog;
        }
        if (type == typeof(Cat))
        {
            return PetType.Cat;
        }
        return PetType.Unknown;
    }
}