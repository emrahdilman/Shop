using ApplicationCore.Entities.Concrete;
using Infrastructure.Context;
using Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Concrete
{
    public class ProductRepository : BaseRepository<Product>, IProductsRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {

        }
    }
}
