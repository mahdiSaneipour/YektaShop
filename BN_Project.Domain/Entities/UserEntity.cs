using BN_Project.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
