using PetCare.Shared.Entities.Pets;

namespace PetCare.Shared.Entities
{
    public class UpcomingVaccine
    {
        public Guid Id { get; set; }
        public Guid PetId { get; set; }
        public required string Name { get; set; }
        public DateTime Date { get; set; }
        public string? Notes { get; set; }
        public virtual Pet? Pet { get; set; }
    }
}
