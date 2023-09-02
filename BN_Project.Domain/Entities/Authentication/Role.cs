using BN_Project.Domain.Entities.Common;

namespace BN_Project.Domain.Entities.Authentication
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<UsersRoles> UsersRoles { get; set; }
        public ICollection<RolesPermissions> RolesPermissions { get; set; }
    }
}
