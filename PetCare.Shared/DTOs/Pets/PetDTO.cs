using PetCare.Shared.Entities.Pets;

namespace PetCare.Shared.DTOs.Pets;

public class PetDTO
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public PetType PetType { get; set; }

    public static PetDTO GetDTO(Pet pet)
    {
        return new PetDTO
        {
            Id = pet.Id,
            Name = pet.Name,
            DateOfBirth = pet.DateOfBirth,
            PetType = MapPetType(pet.GetType())
        };
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
