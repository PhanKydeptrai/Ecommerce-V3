using Domain.DTO_Models;
using Domain.Models;

namespace Ecommerce_BE.IServices
{
    public interface IProductServices
    {
        //Get all product
        Task<IEnumerable<Product>> GetProducts();
        //Get prodct by id
        Task<Product> GetProductById(Guid id);
        //get product by name
        Task<Product> GetProductByName(string name);
        //Add product
        Task<bool> AddProduct(ProductCreationDto product);
        //Update product
        Task<bool> UpdateProduct(Guid id, ProductUpdateDto productDto);
        //Remove product
        Task<bool> RemoveProductById(Guid id);
        //Get product by category id
        Task<IEnumerable<Product>> GetProductByCategoryId(Guid categoryid);
        //Get product by item id
        Task<Product> GetProductByItemId(Guid itemid);
        //Get product price by item id
        Task<decimal> GetPriceByItemId(Guid itemid);
    }
}
