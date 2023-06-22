using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities.DTO_s.CategoryDTO
{
    public class CreateCategoryDTO
    {
        [DisplayName("Kategori Adı")]
        [Required(ErrorMessage = "Bu alan zorunludur!")]
        [MinLength(3, ErrorMessage = "En az 3 karakter girmelisiniz!")]
        public string Name { get; set; }
    }
}
