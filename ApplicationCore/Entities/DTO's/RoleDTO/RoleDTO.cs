using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities.DTO_s.RoleDTO
{
    public class RoleDTO
    {
        [Required] 
        [MinLength(3, ErrorMessage = "En az 3 karakter giriniz!!")]
        public string Name { get; set; }
    }
}
