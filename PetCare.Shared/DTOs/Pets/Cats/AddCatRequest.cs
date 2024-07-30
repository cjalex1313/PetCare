namespace PetCare.Shared.DTOs.Pets.Cats;

public class AddCatRequest : BaseRequest
{
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
}
