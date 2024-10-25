using PetCare.Shared.Entities.Pets;

namespace PetCare.Shared.DTOs.Pets;

public class PetDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public PetType PetType { get; set; }
    public Sex Sex { get; set; }

    public static PetDto GetDTO(Pet pet)
    {
        return new PetDto
        {
            Id = pet.Id,
            Name = pet.Name,
            DateOfBirth = pet.DateOfBirth,
            Sex = pet.Sex,
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
