namespace PetCare.Shared.DTOs
{
    public class VaccineDTO
    {
        public Guid Id { get; set; }
        public Guid PetId { get; set; }
        public required string Name { get; set; }
        public DateTime AdministrationDate { get; set; }
        public DateTime? NextDueDate { get; set; }
        public string? Notes { get; set; }
    }
}
