namespace PetCare.Shared.Entities.Pets;

public class Pet
{
    public Guid Id { get; set; }
    public required string UserId { get; set; }
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Sex Sex { get; set; }
    public virtual ICollection<Vaccine>? Vaccines { get; set; }
    public virtual ICollection<UpcomingVaccine>? UpcomingVaccines { get; set; }
}

public enum Sex
{
    Male = 0,
    Female = 1
}
