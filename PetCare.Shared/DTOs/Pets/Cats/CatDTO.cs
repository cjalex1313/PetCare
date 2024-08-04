namespace PetCare.Shared.DTOs.Pets.Cats;

public class CatDTO 
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
}
