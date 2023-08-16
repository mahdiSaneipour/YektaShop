using BN_Project.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BN_Project.Domain.Entities
{
    public class TicketMessages : BaseEntity
    {
        public string Message { get; set; }
        public bool IsAdminRead { get; set; }
        public bool IsCustomerRead { get; set; }
        public int SenderId { get; set; }
        public int TicketId { get; set; }

        [ForeignKey("SenderId")]
        public UserEntity UserEntity { get; set; }
        [ForeignKey("TicketId")]
        public Ticket Ticket { get; set; }
    }
}
