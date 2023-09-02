using BN_Project.Core.Tools;
using BN_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BN_Project.Data.Context
{
    public class UserSeed : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasData(
                new UserEntity
                {
                    Id = 1,
                    ActivationCode = Tools.GenerateUniqCode(),
                    Create = DateTime.Now,
                    Name = "admin",
                    Password = "admin1383".EncodePasswordMd5(),
                    Email = "admin@gmail.com",
                    IsActive = true,
                    IsDelete = false
                });
        }
    }
}