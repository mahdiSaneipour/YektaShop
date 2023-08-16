using BN_Project.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BN_Project.Domain.Entities
{
    public class Section : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
