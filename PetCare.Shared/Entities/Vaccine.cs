using PetCare.Shared.Entities.Pets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCare.Shared.Entities
{
    public class Vaccine
    {
        public Guid Id { get; set; }
        public Guid PetId { get; set; }
        public required string Name { get; set; }
        public DateTime AdministrationDate { get; set; }
        public DateTime? NextDueDate { get; set; }
        public string? Notes { get; set; }
        public virtual Pet? Pet { get; set; }
    }
}
