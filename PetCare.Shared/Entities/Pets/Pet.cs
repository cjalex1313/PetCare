namespace PetCare.Shared.Entities.Pets;

public class Pet
{
    public Guid Id { get; set; }
    public required string UserId { get; set; }
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
}
