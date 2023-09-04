using BN_Project.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BN_Project.Domain.Entities.Authentication
{
    public class Permission : BaseEntity
    {
        public string UniqeName { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public Permission? MotherPermission { get; set; }
        public ICollection<RolesPermissions>? RolesPermissions { get; set; }
        public ICollection<Permission>? Permissions { get; set; }
    }
}
