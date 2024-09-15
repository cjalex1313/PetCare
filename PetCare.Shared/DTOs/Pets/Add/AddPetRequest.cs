namespace PetCare.Shared.DTOs.Pets.Add;

public class AddPetRequest : BaseRequest
{
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
}
