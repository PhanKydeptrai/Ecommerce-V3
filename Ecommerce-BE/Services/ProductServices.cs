using AutoMapper;
using Data;
using Domain.DTO_Models;
using Domain.Models;
using Ecommerce_BE.IServices;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_BE.Services
{
    public class ProductServices : IProductServices
    {
        private readonly EcommerceDbContext _context;
        private readonly IItemServices _itemServices;
        private readonly IComboDetailServices _comboDetailServices;
        private readonly IComboServices _comboServices;
        private readonly IMapper _mapper;
        public ProductServices() { }
        public ProductServices(EcommerceDbContext context, IMapper mapper, IItemServices itemServices, IComboServices comboServices, IComboDetailServices comboDetailServices)
        {
            _comboDetailServices = comboDetailServices;
            _comboServices = comboServices;
            _itemServices = itemServices;
            _mapper = mapper;
            _context = context;
        }

        //Add product
        public async Task<bool> AddProduct(ProductCreationDto product)
        {
            //Create new item
            var item = await _itemServices.CreateItem("product");
            //get new item id
            product.ItemId = item.ItemId;
            
            _context.Products.AddAsync(_mapper.Map<Product>(product));
            int check = await _context.SaveChangesAsync();
            if (check > 0)
            {
                return true;
            }
            return false;
        }

        //Gets product by id
        public async Task<Product> GetProductById(Guid id)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
        }

        //get product by name   
        public async Task<Product> GetProductByName(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.ProductName == name);
        }

        //get all product
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        //remove product by id (Chuyển trạng thái của bảng item)
        public async Task<bool> RemoveProductById(Guid id)
        {
            Product product = await GetProductById(id);
            if (product == null)
            {
                return false;
            }
            IEnumerable<ComboDetail> comboDetails = await _comboDetailServices.GetComboDetailsByProductId(id);
            //Lấy productId => lấy ra ComboDetail có productId đó => lấy ra Combo có ComboId => lấy ra ItemId của Combo => lấy ra Item có ItemId đó
            //Chuyển trạng thái của những Item trong Combo 
            if(comboDetails == null)
            {
                return false;
            }
            IEnumerable<Guid> comboIds = comboDetails.Select(x => x.ComboId).Distinct();
            foreach(Guid x in comboIds)
            {
                var combo = await _comboServices.GetComboById(x);
                await _itemServices.RemoveItemById(combo.ItemId);
            }    
            
            // return bool
            return await _itemServices.RemoveItemById(product.ItemId);
        }

        // get product by category id
        public async Task<IEnumerable<Product>> GetProductByCategoryId(Guid categoryid)
        {
            var product = await _context.Products.Where(p => p.CategoryId == categoryid).ToListAsync();
            return product;
        }
        //update product    
        public async Task<bool> UpdateProduct(Guid id, ProductUpdateDto productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null)
                return false;

            product.ProductName = productDto.ProductName;
            product.Description = productDto.Description;
            product.Quanlity = productDto.Quanlity;
            product.Unit = productDto.Unit;
            product.Size = productDto.Size;
            product.Price = productDto.Price;
            product.Image = productDto.Image;
            product.CategoryId = productDto.CategoryId;

            await _context.SaveChangesAsync();
            return true;

        }

        //Get product by item id
        public async Task<Product> GetProductByItemId(Guid itemid)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.ItemId == itemid);
        }

        //Get product price by item id
        public async Task<decimal> GetPriceByItemId(Guid itemid)
        {
            decimal price = (await _context.Products.FirstOrDefaultAsync(a => a.ItemId == itemid)).Price;
            if(price == null)
                return price;
            return 0;
        }

    }
}
