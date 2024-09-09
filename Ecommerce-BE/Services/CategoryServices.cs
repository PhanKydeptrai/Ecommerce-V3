using AutoMapper;
using Data;
using Domain.DTO_Models;
using Domain.Models;
using Ecommerce_BE.IServices;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_BE.Services
{
    public class CategoryServices : ICategoryServices
    {
        public CategoryServices() { }
        private readonly EcommerceDbContext _context;
        private readonly IMapper _mapper;
        public CategoryServices(EcommerceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // add category
        public async Task<bool> Add(CategoryCreationDto category)
        {
            // auto mapper
            _context.Categories.AddAsync(_mapper.Map<Category>(category));
            // kiem tra
            int check = await _context.SaveChangesAsync();
            if(check > 0) 
                return true;
            return false;
        }
        // delete category - update status
        public async Task<bool> Delete(Guid id)
        {
            Category category = await GetCategoryId(id);
            if(category == null) 
                return false;
            category.Status = "nhd";

            int check = await _context.SaveChangesAsync();
            if(check > 0)
                return true;
            return false;
        }
        // get all category
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }
        // get category by id
        public async Task<Category> GetCategoryId(Guid id)
        {
            return await _context.Categories.FindAsync(id);
        }
        // get category by name
        public async Task<Category> GetCategoryName(string categoryname)
        {
            return await _context.Categories.FirstOrDefaultAsync(a => a.CategoryName == categoryname);
        }
        // update info category
        public async Task<bool> Update(Guid id, CategoryUpdateDto categoryDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            
            category.CategoryName = categoryDto.CategoryName;
            category.Status = categoryDto.Status;
            category.Image = categoryDto.Image;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
