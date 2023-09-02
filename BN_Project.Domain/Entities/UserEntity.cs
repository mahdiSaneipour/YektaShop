using BN_Project.Domain.Entities.Authentication;
using BN_Project.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BN_Project.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public string? Name { get; set; }

        public string Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Avatar { get; set; }

        public string Password { get; set; }

        public string ActivationCode { get; set; }

        public bool IsActive { get; set; }

        [InverseProperty("User")]
        public List<Order> Orders { get; set; }
        public ICollection<Comment.Comment> Comments { get; set; }
        public ICollection<UsersRoles> UsersRoles { get; set; }
    }
}
