using Domain.DTO_Models;
using Ecommerce_BE.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ecommerce_BE.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        private readonly IProductServices _productServices;
        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }


        /// <summary>
        /// Lấy danh sách order
        /// </summary>
        /// <remarks>
        /// Sample response:
        /// 
        ///     GET
        ///     [
        ///         {
        ///             "orderId": "8e11d2d7-39c7-49c2-a68e-2c87bb409d66",
        ///             "userId": "1c3eedfb-6e38-4f98-84dd-74390857ba39",
        ///             "createdOfDate": "2024-07-25T07:44:27.993",
        ///             "unitPrice": 0,
        ///             "status": "waiting",
        ///             "user": null,
        ///             "recomment": null,
        ///             "orderDetails": null
        ///         }
        ///     ]        
        /// </remarks>
        /// <response code="200">Trả về danh sách order</response>
        /// <response code="404">Không lấy được danh sách order</response>
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderServices.GetAllOrders();
            return Ok(orders);
        }



        [HttpGet("{status}")]
        public async Task<IActionResult> GetOrderByStatus(string status)
        {
            var orders = await _orderServices.GetOrderByStatus(status);
            //Serialize object to json
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            return Ok(JsonSerializer.Serialize(orders, options));
        }


        /// <summary>
        /// Lấy danh sách order theo user id
        /// </summary>
        /// <remarks>
        /// Sample response:
        /// 
        ///     GET
        ///     
        ///     User Id: 1c3eedfb-6e38-4f98-84dd-74390857ba39
        /// 
        ///     [
        ///         {
        ///             "orderId": "8e11d2d7-39c7-49c2-a68e-2c87bb409d66",
        ///             "userId": "1c3eedfb-6e38-4f98-84dd-74390857ba39",
        ///             "createdOfDate": "2024-07-25T07:44:27.993",
        ///             "unitPrice": 0,
        ///             "status": "waiting",
        ///             "user": null,
        ///             "recomment": null,
        ///             "orderDetails": null
        ///         }
        ///     ]        
        /// </remarks>
        /// <response code="200">Trả về danh sách order</response>
        /// <response code="404">Không tìm thấy kết quả</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrdersByUserID(Guid id)
        {
            var orders = await _orderServices.GetOrdersByUserID(id);
            
            //Serialize object to json
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            return Ok(JsonSerializer.Serialize(orders, options));
            
        }


        /// <summary>
        /// Lấy danh sách order theo id
        /// </summary>
        /// <remarks>
        /// Sample response:
        ///     
        ///     Order Id: 8e11d2d7-39c7-49c2-a68e-2c87bb409d66
        ///     
        ///     GET
        ///     [
        ///         {
        ///             "orderId": "8e11d2d7-39c7-49c2-a68e-2c87bb409d66",
        ///             "userId": "1c3eedfb-6e38-4f98-84dd-74390857ba39",
        ///             "createdOfDate": "2024-07-25T07:44:27.993",
        ///             "unitPrice": 0,
        ///             "status": "waiting",
        ///             "user": null,
        ///             "recomment": null,
        ///             "orderDetails": null
        ///         }
        ///     ]        
        /// </remarks>
        /// <response code="200">Trả về danh sách order</response>
        /// <response code="404">Không lấy được danh sách order</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderByID(Guid id)
        {
            var order = await _orderServices.GetOrderByID(id);
            return Ok(order);
        }


        //Thêm order mới
        /// <summary>
        /// Tạo order mới
        /// </summary>
        /// <remarks>
        /// Sample resquest:
        ///     
        /// 
        ///     POST
        ///     {
        ///         "orderId": "8e11d2d7-39c7-49c2-a68e-2c87bb409d66",
        ///         "userId": "1c3eedfb-6e38-4f98-84dd-74390857ba39",
        ///         "createdOfDate": "2024-07-25T07:44:27.993",
        ///         "unitPrice": 0,
        ///         "status": "waiting",
        ///         "user": null,
        ///         "recomment": null,
        ///         "orderDetails": null
        ///     }
        ///         
        ///             
        /// </remarks>
        /// <response code="200">Trả về danh sách order</response>
        /// <response code="400">Tạo order</response>
        [HttpPost]
        public async Task<IActionResult> CreateNewOrder(OrderCreationDto order)
        {
            if(ModelState.IsValid && order != null)
            {
                order.OrderId = Guid.NewGuid();
                order.CreatedOfDate = DateTime.Now;
                var result = await _orderServices.CreateNewOrder(order);
                if (result)
                {
                    return Ok("Tạo thành công!");
                }
                return BadRequest("Tạo không thành công");
            }
            var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { message });
        }


        /// <summary>
        /// Cập nhật thông tin order
        /// </summary>
        /// <remarks>
        /// Sample resquest:
        ///     
        /// 
        ///     PUT
        ///     {
        ///         "orderId": "8e11d2d7-39c7-49c2-a68e-2c87bb409d66",
        ///         "userId": "1c3eedfb-6e38-4f98-84dd-74390857ba39",
        ///         "createdOfDate": "2024-07-25T07:44:27.993",
        ///         "unitPrice": 0,
        ///         "status": "waiting",
        ///         "user": null,
        ///         "recomment": null,
        ///         "orderDetails": null
        ///     }
        ///         
        ///             
        /// </remarks>
        /// <response code="200">Cập nhật thành công</response>
        /// <response code="400">Cập nhật không thành công</response>
        [HttpPut]
        public async Task<IActionResult> UpdateStatus(OrderCreationDto order)
        {
            var result = await _orderServices.UpdateStatus(order);
            if (result)
            {
                return Ok("Cập nhật thành công");
            }
            return BadRequest("Cập nhật thất bại");
        }




    }
}
