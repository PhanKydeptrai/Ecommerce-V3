using Domain.DTO_Models;
using Domain.Models;
using Ecommerce_BE.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace Ecommerce_BE.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productServices;
        private readonly ICategoryServices _categoryServices;
        public ProductController(IProductServices productServices, ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
            _productServices = productServices;
        }

        // lấy danh sách sản phẩm
        /// <summary>
        /// Lấy danh sách product
        /// </summary>
        /// <remarks>
        /// Sample response:
        /// 
        ///     GET
        ///     [
        ///         {
        ///             "productId": "6520ff63-d211-465c-bd66-e547989b33df",
        ///             "productName": "string",
        ///             "description": "string",
        ///             "quanlity": 0,
        ///             "unit": "string",
        ///             "size": "string",
        ///             "price": 0,
        ///             "categoryId": "732b01cb-4020-4d5b-98aa-dee530853d7d",
        ///             "image": "string",
        ///             "itemId": "a33f6242-b462-41ff-918e-18feaca0998a"
        ///         }
        ///     ]        
        /// </remarks>
        /// <response code="200">Trả về danh sách product</response>
        /// <response code="404">Không lấy được danh sách product</response>
        
        [HttpGet]
        public async Task<ActionResult> Products()
        {
            IEnumerable<Product> list = await _productServices.GetProducts();
            if (list == null)
                return NotFound("Danh sách không tồn tại");
            return Ok(list);
        }

        // lấy sản phẩm theo id
        /// <summary>
        /// Lấy product theo id
        /// </summary>
        /// <remarks>
        /// Sample response:
        /// 
        ///     GET
        ///     {
        ///         "productId": "6520ff63-d211-465c-bd66-e547989b33df",
        ///         "productName": "string",
        ///         "description": "string",
        ///         "quanlity": 0,
        ///         "unit": "string",
        ///         "size": "string",
        ///         "price": 0,
        ///         "categoryId": "732b01cb-4020-4d5b-98aa-dee530853d7d",
        ///         "image": "string",
        ///         "itemId": "a33f6242-b462-41ff-918e-18feaca0998a"
        ///     }
        ///             
        /// </remarks>
        /// <response code="200">Trả product theo id</response>
        /// <response code="404">Product không tồn tại</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> ProductId(Guid id)
        {
            Product productid = await _productServices.GetProductById(id);
            if (productid == null)
                return NotFound("Mã sản phẩm không tồn tại");
            return Ok(productid);
        }



        // Lấy sản phẩm theo tên
        /// <summary>
        /// Lấy product theo tên 
        /// </summary>
        /// <remarks>
        /// Sample response:
        /// 
        ///     GET
        ///     {
        ///         "productId": "6520ff63-d211-465c-bd66-e547989b33df",
        ///         "productName": "string",
        ///         "description": "string",
        ///         "quanlity": 0,
        ///         "unit": "string",
        ///         "size": "string",
        ///         "price": 0,
        ///         "categoryId": "732b01cb-4020-4d5b-98aa-dee530853d7d",
        ///         "image": "string",
        ///         "itemId": "a33f6242-b462-41ff-918e-18feaca0998a"
        ///     }
        ///             
        /// </remarks>
        /// <response code="200">Trả product theo tên</response>
        /// <response code="404">Product không tồn tại</response>
        [HttpGet("{name}")]
        public async Task<ActionResult<Product>> ProductName(string name)
        {
            Product productname = await _productServices.GetProductByName(name);
            if (productname == null)
                return NotFound("Tên sản phẩm không tồn tại");
            return Ok(productname);
        }


        // Thêm sản phẩm

        /// <summary>
        /// Thêm product mới 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST
        ///     {
        ///         "productName": "string",
        ///         "description": "string",
        ///         "quanlity": 0,
        ///         "unit": "string",
        ///         "size": "string",
        ///         "price": 0,
        ///         "categoryId": "732b01cb-4020-4d5b-98aa-dee530853d7d",
        ///         "image": "string"
        ///     }
        ///             
        /// </remarks>
        /// <response code="200">Thêm sản phẩm thành công</response>
        /// <response code="400">Product đã tồn tại</response>
        [HttpPost]
        public async Task<ActionResult<bool>> Product(ProductCreationDto product)
        {
            if (product != null)
            {
                //Kiểm tra trùng tên sản phẩm
                product.ProductId = Guid.NewGuid();
                Product nameCheck = await _productServices.GetProductByName(product.ProductName);
                if (nameCheck != null)
                {
                    return BadRequest("Tên sản phẩm đã tồn tại");
                }
                if (ModelState.IsValid)
                {
                    if (await _productServices.AddProduct(product) == true)
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
            return BadRequest("Chưa nhập thông tin sản phẩm");
        }

        // Cập nhật sản phẩm
        /// <summary>
        /// Cập nhật thông tin product theo id 
        /// </summary>
        /// <remarks>
        /// Sample resquest:
        ///     
        ///     PUT
        /// 
        ///     Id: 6520ff63-d211-465c-bd66-e547989b33df
        /// 
        ///     {
        ///         "productId": "6520ff63-d211-465c-bd66-e547989b33df",
        ///         "productName": "string",
        ///         "description": "string",
        ///         "quanlity": 0,
        ///         "unit": "string",
        ///         "size": "string",
        ///         "price": 0,
        ///         "categoryId": "732b01cb-4020-4d5b-98aa-dee530853d7d",
        ///         "image": "string",
        ///         "itemId": "a33f6242-b462-41ff-918e-18feaca0998a"
        ///     }
        ///             
        /// </remarks>
        /// <response code="200">Trả product theo tên</response>
        /// <response code="400">Product không tồn tại hoặc tên bị trùng</response>
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<ActionResult<bool>> UpdateProduct(Guid id, ProductUpdateDto productDto)
        {
            var product = await _productServices.GetProductById(id);
            Category category = await _categoryServices.GetCategoryId(productDto.CategoryId);

            if (product == null || category == null)
            {
                return BadRequest("Id  không tồn tại");
            }

            string nameBeforeUpdate = product.ProductName; 
            if (productDto.ProductName == nameBeforeUpdate || _productServices.GetProductByName(productDto.ProductName) == null)
            {

                var result = await _productServices.UpdateProduct(id, productDto);
                return result ? Ok("Cập nhật thành công") : BadRequest("Không thể cập nhật sản phẩm");
            }
            return BadRequest("Tên sản phẩm đã tồn tại");
        }

        // Xóa sản phẩm

        /// <summary>
        /// Xoá thông tin product theo id 
        /// </summary>
        /// <remarks>
        /// Sample resquest:
        ///     
        ///     PUT
        /// 
        ///     Id: 6520ff63-d211-465c-bd66-e547989b33df
        /// 
        ///     {
        ///         "productId": "6520ff63-d211-465c-bd66-e547989b33df",
        ///         "productName": "string",
        ///         "description": "string",
        ///         "quanlity": 0,
        ///         "unit": "string",
        ///         "size": "string",
        ///         "price": 0,
        ///         "categoryId": "732b01cb-4020-4d5b-98aa-dee530853d7d",
        ///         "image": "string",
        ///         "itemId": "a33f6242-b462-41ff-918e-18feaca0998a"
        ///     }
        ///             
        /// </remarks>
        /// <response code="200">Thay đổi trạng thái của item</response>
        /// <response code="400">Product không tồn tại</response>
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<ActionResult<bool>> DeleteProduct(Guid id)
        {
            var product = await _productServices.GetProductById(id);
            if (product == null)
                return BadRequest("Id khong ton tai");
            var delete = await _productServices.RemoveProductById(id);
            if (delete)
                return Ok("Cap nhat trang thai thanh cong");
            return BadRequest("Yeu cau khong hop le");
        }
    }
}
