using PetCare.Shared.Entities.Pets;

namespace PetCare.Shared.DTOs.Pets.Add;

public class AddPetRequest : BaseRequest
{
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Sex Sex { get; set; }
}
