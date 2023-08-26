using BN_Project.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BN_Project.Domain.Entities
{
    public class Address : BaseEntity
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalCode { get; set; }
        public string CompleteAddress { get; set; }
        public int UserId { get; set; }
        public bool IsDefalut { get; set; } = false;
        [ForeignKey("UserId")]
        public UserEntity UserEntity { get; set; }
    }
}
