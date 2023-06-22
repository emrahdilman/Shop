using ApplicationCore.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities.DTO_s.RoleDTO
{
    public class AssignedRoleDTO
    {
        public IdentityRole Role { get; set; }

        public IEnumerable<AppUser>? HasRole { get; set; }
        public IEnumerable<AppUser>? HasNotRole { get; set; }

        public string RoleName { get; set; }

        public string[]? AddIds { get; set; }
        public string[]? DeleteIds { get; set; }
    }
}
