using ApplicationCore.Entities.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities.Concrete
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int UnitPrice { get; set; }
        public string Image { get; set; }

        [NotMapped]
        public IFormFile UploadImage { get; set; }

        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}
