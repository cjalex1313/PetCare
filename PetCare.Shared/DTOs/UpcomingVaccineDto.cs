namespace PetCare.Shared.DTOs
{
    public class UpcomingVaccineDto
    {
        public Guid Id { get; set; }
        public Guid PetId { get; set; }
        public required string Name { get; set; }
        public DateTime Date { get; set; }
        public string? Notes { get; set; }
    }
}
