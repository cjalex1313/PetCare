namespace PetCare.Shared.DTOs.Pets;

public class PetDTO
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public PetType PetType { get; set; }
}
