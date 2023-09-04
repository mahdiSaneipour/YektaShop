using BN_Project.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BN_Project.Domain.Entities.Authentication
{
    public class UsersRoles : BaseEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}
