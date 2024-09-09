using Domain.DTO_Models;
using Domain.Models;

namespace Ecommerce_BE.IServices
{
    public interface ICategoryServices
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> GetCategoryId(Guid id);
        Task<Category> GetCategoryName(string categoryname);

        Task<bool> Add(CategoryCreationDto category);
        Task<bool> Update(Guid id, CategoryUpdateDto categoryDto);
        Task<bool> Delete(Guid id);
    }
}
