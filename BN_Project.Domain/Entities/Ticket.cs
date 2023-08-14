using BN_Project.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BN_Project.Domain.Entities
{
    public class Ticket : BaseEntity
    {
        public Ticket()
        {
            TicketMessages = new List<TicketMessages>();
        }

        public string Subject { get; set; }
        public int SectionId { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public DateTime LastUpadate { get; set; }
        public int OwnerId { get; set; }

        #region Relations
        [ForeignKey("SectionId")]
        public Section Section { get; set; }
        [ForeignKey("OwnerId")]
        public UserEntity UserEntity { get; set; }
        public ICollection<TicketMessages> TicketMessages { get; set; }
        #endregion
    }
}
