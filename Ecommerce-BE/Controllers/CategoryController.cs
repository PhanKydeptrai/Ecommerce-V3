using Data;
using Domain.DTO_Models;
using Domain.Models;
using Ecommerce_BE.IServices;
using Ecommerce_BE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_BE.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase

    {
        private readonly ICategoryServices _categoryServices;
        private readonly IProductServices _productServices;
        private readonly EcommerceDbContext _context;
        private readonly IItemServices _itemServices;
        public CategoryController(ICategoryServices categoryServices, EcommerceDbContext context, IProductServices productServices, IItemServices itemServices)
        {
            _categoryServices = categoryServices;
            _context = context;
            _productServices = productServices;
            _itemServices = itemServices;
        }
        /// <summary>
        /// Lấy thông tin tất cả Category
        /// </summary>
        /// <remarks>
        /// Sample response:
        /// 
        ///     GET
        ///     
        ///     [
        ///        {
        ///             "categoryId": "6dc42c61-45c5-47da-a497-218df9fd2441",
        ///             "categoryName": "Category 1",
        ///             "image": "category1.png",
        ///             "status": "hd" or "nhd",
        ///             "products": null    
        ///        }
        ///     ]    
        ///     
        /// </remarks>
        /// <response code="200">Lấy thành công</response>
        /// <response code="404">Không tìm thấy dữ liệu</response>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Categories()
        {
                IEnumerable<Category> list = await _categoryServices.GetCategories();
                if (list == null)
                    return NotFound("Danh sách không tồn tại");
                return Ok(list);
        }
        // Loại hàng theo id

        /// <summary>
        /// Lấy thông tin Category theo Id
        /// </summary>
        /// <remarks>
        /// Sample response:
        /// 
        ///     GET
        ///     
        ///         {
        ///             "categoryId": "6dc42c61-45c5-47da-a497-218df9fd2441",
        ///             "categoryName": "Category 1",
        ///             "image": "category1.png",
        ///             "status": "hd" or "nhd",
        ///             "products": null    
        ///         }
        ///     
        /// </remarks>
        /// <response code="200">Lấy thành công</response>
        /// <response code="404">Không tìm thấy dữ liệu</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> CategoryId(Guid id)
        {
            Category categoryid = await _categoryServices.GetCategoryId(id);
            if(categoryid == null)
                 return NotFound("Mã loại hàng không tồn tại");
            return Ok(categoryid);
        }


        // loại hàng theo tên

        /// <summary>
        /// Lấy thông tin Category theo tên
        /// </summary>
        /// <remarks>
        /// Sample response:
        /// 
        ///     GET
        ///     
        ///         {
        ///             "categoryId": "6dc42c61-45c5-47da-a497-218df9fd2441",
        ///             "categoryName": "Category 1",
        ///             "image": "category1.png",
        ///             "status": "hd" or "nhd",
        ///             "products": null    
        ///         }
        ///     
        /// </remarks>
        /// <response code="200">Lấy thành công</response>
        /// <response code="404">Không tìm thấy dữ liệu</response>
        [HttpGet("{name}")]
        public async Task<ActionResult<Category>> CategoryName(string name)
        {
            Category names = await _categoryServices.GetCategoryName(name);
            if (names == null)
                return NotFound("Tên loại hàng không tồn tại");
            return Ok(name);
        }
        // thêm loại hàng
        /// <summary>
        /// Thêm thông tin Category
        /// </summary>
        /// <remarks>
        /// Sample response:
        /// 
        ///     POST
        ///     
        ///         {
        ///             "categoryId": "6dc42c61-45c5-47da-a497-218df9fd2441",
        ///             "categoryName": "Category 1",
        ///             "image": "category1.png",
        ///             "status": "hd" or "nhd",
        ///             "products": null    
        ///         }
        ///     
        /// </remarks>
        /// <response code="200">Lấy thành công</response>
        /// <response code="404">Không tìm thấy dữ liệu</response>
        [HttpPost]
        public async Task<ActionResult<bool>> AddCategory(CategoryCreationDto category)
        {
            if (category != null)
            {
                category.CategoryId = Guid.NewGuid();
                if (ModelState.IsValid && category != null)
                {
                    if(await _categoryServices.GetCategoryName(category.CategoryName) != null)
                        return BadRequest("Tên loại hàng đã tồn tại");
                    category.Status = "hd";
                    if (await _categoryServices.Add(category) == true)
                    {
                        return Ok("Thêm thành công");
                    }
                }
                else 
                {
                    var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return BadRequest(new { message });
                }
            }
            return BadRequest("Chưa nhập thông tin loại hàng");
        }

        /// <summary>
        /// Thêm thông tin Category
        /// </summary>
        /// <remarks>
        /// Sample response:
        /// 
        ///     POST
        ///         Id: 6dc42c61-45c5-47da-a497-218df9fd2441 Example
        ///         
        ///         {
        ///             "categoryName": "Category 1",
        ///             "image": "category1.png",
        ///             "status": "hd" or "nhd"
        ///         }
        ///     
        /// </remarks>
        /// <response code="200">Cập nhật thành công</response>
        /// <response code="400">Dữ liệu không đầy đủ hoặc category không tồn tại</response>
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<ActionResult<bool>> UpdateCategory(Guid id, CategoryUpdateDto categoryDto)
        {
            
            var category = await _categoryServices.GetCategoryId(id);
            if (category == null)
                return BadRequest("Id không tồn tại");

            // Cập nhật danh mục
            var result = await _categoryServices.Update(id, categoryDto);
            if (!result)
                return BadRequest("Không thể cập nhật danh mục");

            return Ok("Cập nhật thành công");
        }



        /// <summary>
        /// Xoá thông tin Category(Thay đổi trạng thái)
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT
        ///         Id: 6dc42c61-45c5-47da-a497-218df9fd2441 Example
        ///         
        ///     
        /// </remarks>
        /// <response code="200">Cập nhật thành công</response>
        /// <response code="400">Dữ liệu không đầy đủ hoặc Id không tồn tại</response>
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<ActionResult<bool>> DeleteCategory(Guid id)
        {
            var category = await _categoryServices.GetCategoryId(id);
            if (category == null)
                return BadRequest("Id không tồn tại");
            // cap nhat trang thai product
            var updateProduct = await _productServices.GetProductByCategoryId(id);
            foreach(var item in updateProduct)
            {
                await _itemServices.RemoveItemById(item.ItemId);
            }
            var delete = await _categoryServices.Delete(id);
            if (!delete)
                return Ok("Cập nhật trạng thái thành công");
            return Ok("Yêu cầu không hợp lệ");
            
        }



    }
}
