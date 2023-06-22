using ApplicationCore.Entities.Concrete;
using Infrastructure.Context;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Concrete
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }
        public async Task<bool> AnyCategoryName(string name)
        {
            return await _context.Categories.Where(x => x.Status != ApplicationCore.Entities.Abstract.Status.Passive).AnyAsync(x => x.Name == name);
        }
    }
}
