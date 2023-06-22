using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities.DTO_s.ProductDTO
{
    public class CreateProductDTO
    {
        [Required(ErrorMessage = "Lütfen bir isim giriniz!!")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Lütfen bir açıklama giriniz!!")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Lütfen bir fiyat giriniz!!")]
        [Column(TypeName = "decimal(8,2)")]
        public decimal UnitPrice { get; set; }


        public string? Image { get; set; }

        public IFormFile UploadImage { get; set; }


        [Required(ErrorMessage = "Lütfen bir kategori seçiniz!!")]
        public int CategoryId { get; set; }
    }
}
