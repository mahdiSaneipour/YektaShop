using BN_Project.Domain.Entities.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BN_Project.Data.Context
{
    public class UsersRolesSeed : IEntityTypeConfiguration<UsersRoles>
    {
        public void Configure(EntityTypeBuilder<UsersRoles> builder)
        {
            builder.HasData(new UsersRoles
            {
                Id = 1,
                Create = DateTime.Now,
                IsDelete = false,
                RoleId = 1,
                UserId = 1
            });
        }
    }
}
