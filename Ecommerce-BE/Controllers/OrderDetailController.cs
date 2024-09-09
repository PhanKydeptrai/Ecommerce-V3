using AutoMapper;
using Domain.DTO_Models;
using Domain.Models;
using Ecommerce_BE.IServices;
using Ecommerce_BE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_BE.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailServices _orderDetailServices;
        private readonly IOrderServices _orderServices;
        private readonly IMapper _mapper;
        private readonly IProductServices _productServices;
        private readonly IItemServices _itemServices;
        private readonly IComboServices _comboServices;
        public OrderDetailController(IOrderServices orderServices, IOrderDetailServices orderDetailServices, IMapper mapper, IProductServices productServices, IItemServices items, IComboServices comboServices)
        {
            _orderDetailServices = orderDetailServices;
            _orderServices = orderServices;
            _mapper = mapper;
            _productServices = productServices;
            _itemServices = items;
            _comboServices = comboServices;
        }

        //Get list of order details 
        /// <summary>
        /// Lấy danh sách order detail
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Trả về danh sách order detail</response>
        [HttpGet]
        public async Task<IActionResult> OrderDetails()
        {
            return Ok(await _orderDetailServices.GetOrderDetails());
        }

        //Get order detail by id
        /// <summary>
        /// Lấy order detail theo id order id
        /// </summary>
        /// 
        /// <returns></returns>
        /// <response code="200">Trả về danh sách order detail</response>
        /// <response code="400">Trả về danh sách order detail</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> OrderDetail(Guid id)
        {
            OrderDetail orderDetail = await _orderDetailServices.GetOrderDetailByID(id);
            if (orderDetail == null)
            {
                return Ok(orderDetail);
            }
            return NotFound("Không tìm thấy order detail");

        }

        //Get order details by order id
        /// <summary>
        /// Lấy order detail theo id
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Trả về danh sách order detail</response>
        /// <response code="400">Trả về danh sách order detail</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> OrderdetailsByOrderID(Guid id)
        {
            IEnumerable<OrderDetail> orderDetails = await _orderDetailServices.GetOrderDetailsByOrderID(id);
            if(orderDetails != null)
            {
                return Ok(orderDetails);
            }
            return NotFound("Không tìm thấy Order");
        }

        /// <summary>
        /// Thêm order detail
        /// </summary>
        /// <remarks>
        /// Sample response:
        ///     
        ///     User Id: 8e11d2d7-39c7-49c2-a68e-2c87bb409d66
        ///     Item Id: 1c3eedfb-6e38-4f98-84dd-74390857ba39 
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
        /// <response code="200">Thêm thành công</response>
        /// <response code="400">Thêm không thành công</response>
        //Create new order detail
        [HttpPost]
        public async Task<IActionResult> Orderdetail(Guid userId, Guid itemId, int quantity)
        {
            if (userId != null && itemId != null && quantity != null)
            {
                //kiểm tra order của user có đang ở trạng thái waiting không
                IEnumerable<Order> orderCheck = await _orderServices.GetOrdersByUserID(userId);
                Order order = orderCheck.FirstOrDefault(a => a.Status == "waiting");
                //kiểm tra loại item và lấy giá (combo hay product)
                decimal price = new decimal();
                Item item = await _itemServices.GetItemById(itemId);
                if(item == null) //Khong tìm thấy item
                {
                    return NotFound("Không tìm thấy item");
                }
                //Nếu item tồn tại và không phải combo
                if (item.ItemType != "combo")
                {
                    Product product = await _productServices.GetProductByItemId(itemId);
                    price = product.Price;
                }
                else
                {
                    Combo combo = await _comboServices.GetComboById(itemId);
                    price = combo.Price;
                }
                //Kiểm tra có order nào đang đợi không
                if (order != null) //Nếu có
                {
                    //Kiểm tra orderdetail đã tồn hại hay không?
                    List<OrderDetail> orderDetails = order.OrderDetails.ToList();
                    OrderDetail orderdetail = orderDetails.FirstOrDefault(a => a.ItemId == itemId);
                    if(orderdetail != null) //Nếu có
                    {
                        //Cập nhật giá và số lượng của orderdetail
                        orderdetail.Quanlity += quantity; //Cập nhật số lượng
                        orderdetail.Price += price * quantity; //Cập nhật giá
                        await _orderDetailServices.UpdateOrderDetail(orderdetail);
                    }
                    else //Nếu không
                    {
                        //Tạo orderdetail mới
                        await _orderDetailServices.CreateNewOrderDetail(new OrderDetailCreationDto
                        {
                            OrderDetailId = Guid.NewGuid(),
                            OrderId = order.OrderId,
                            ItemId = itemId,
                            Price = price*quantity,
                            Quanlity = quantity
                        });
                    }
                    //Luôn phải câp nhật giá của order nên để cập nhật bên ngoài
                    //Cập nhật giá của order
                    order.UnitPrice += price * quantity;
                    await _orderServices.UpdateOrderPrice(order);
                }
                else //Nếu không
                {
                    //Tạo order mới
                    OrderCreationDto orderDto = new OrderCreationDto
                    {
                        OrderId = Guid.NewGuid(),
                        Status = "waiting",
                        UserId = userId,
                        CreatedOfDate = DateTime.Now,
                        UnitPrice = price * quantity
                    };
                    await _orderServices.CreateNewOrder(orderDto);
                    //Tạo orderdetail mới
                    await _orderDetailServices.CreateNewOrderDetail(new OrderDetailCreationDto
                    {
                        OrderDetailId = Guid.NewGuid(),
                        OrderId = orderDto.OrderId ?? Guid.Empty,
                        ItemId = itemId,
                        Price = price * quantity,
                        Quanlity = quantity
                    });
                    return Ok("Tạo order và orderdetail thành công");
                }
                
            }
            return BadRequest("Dữ liệu không hợp lệ");
        }



        //public async Task<IActionResult> OrderDetail(Guid userid, Guid itemid)
        //{
        //    if(userid != null && itemid != null)
        //    {
        //        IEnumerable<Order> orderCheck = await _orderServices.GetOrdersByUserID(userid);
        //        //order ở trạng thái waiting
        //        Order order = orderCheck.FirstOrDefault(a => a.Status == "waiting");
        //        //lấy giá sản phẩm
        //        Item item = await _itemServices.GetItemById(itemid);
        //        decimal productPrice = new decimal();
        //        if (item.ItemType != "combo") //Item không phải combo
        //        {
        //            //Lấy giá trong bảng product
        //            Product pro = await _productServices.GetProductByItemId(itemid);
        //            productPrice = pro.Price;
        //        }
        //        else
        //        {
        //            Combo combo = await _comboServices.GetComboById(itemid);
        //            productPrice = combo.Price;
        //            //lấy giá trong combo
        //            Console.WriteLine("Item là combo");
        //        }

        //        //Nếu không có order nào đang đợi
        //        if (order == null)
        //        {
        //            //tạo order mới
        //            OrderCreationDto orderCreationDto = new OrderCreationDto
        //            {
        //                OrderId = Guid.NewGuid(),
        //                Status = "waiting",
        //                UserId = userid,
        //                CreatedOfDate = DateTime.Now,
        //                UnitPrice = 0
        //            };

        //            if (await _orderServices.CreateNewOrder(orderCreationDto))
        //            {
        //                //Tạo orderdetail
        //                _orderDetailServices.CreateNewOrderDetail(new OrderDetailCreationDto
        //                {
        //                    OrderDetailId = Guid.NewGuid(),
        //                    OrderId = orderCreationDto.OrderId ?? Guid.Empty,
        //                    ItemId = itemid,
        //                    Price = productPrice,
        //                    Quanlity = 1
        //                });
        //                return Ok("Tạo order và orderdetail thành công");
        //            }
        //            return BadRequest("Tạo order và orderdetail không thành công");
        //        }
        //        //Nếu có order nào đang đợi
        //        else
        //        {
        //            //List orderdetail theo order Id
        //            IEnumerable<OrderDetail> orderDetails = await _orderDetailServices.GetOrderDetailsByOrderID(order.OrderId);
        //            //Lấy itemid của orderdetail
        //            OrderDetail orderdetail = orderDetails.FirstOrDefault(a => a.ItemId == itemid);
        //            if(orderdetail != null) //Nếu tìm thấy orderdetail
        //            {
        //                //Tiến hành cập nhật số lượng order detail
        //                orderdetail.Quanlity += 1;
        //                orderdetail.Price += productPrice;
        //                order.UnitPrice += productPrice;
        //                //Cập nhật order 
        //                await _orderServices.UpdateOrderPrice(order);
        //                if (await _orderDetailServices.UpdateOrderDetail(orderdetail))
        //                {
        //                    return Ok("Cập nhật orderdetail thành công");
        //                }
        //            }
        //            else //Nếu không tìm thấy orderdetail
        //            {
        //                //Tạo orderdetail mới
        //                //Tạo orderdetail
        //                _orderDetailServices.CreateNewOrderDetail(new OrderDetailCreationDto
        //                {
        //                    OrderDetailId = Guid.NewGuid(),
        //                    OrderId = order.OrderId,
        //                    ItemId = itemid,
        //                    Price = productPrice,
        //                    Quanlity = 1
        //                });
        //            }
        //            return BadRequest("Cập nhật orderdetail không thành công");
        //        }
        //    }
        //    return BadRequest("Dữ liệu không hợp lệ");
        //}
        
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveOrderDetail(Guid id)
        {
            //Cập nhật lại giá order
            OrderDetail orderDetail = await _orderDetailServices.GetOrderDetailByID(id);
            //Lấy order
            Order order = orderDetail.Order;
            //Cập nhật giá order
            order.UnitPrice -= orderDetail.Price;
            await _orderServices.UpdateOrderPrice(order);

            if (await _orderDetailServices.RemoveOrderDetail(id) == true)
            {
                return Ok("Xoá thành công");
            }
            return BadRequest("Xoá không thành công");
        }
    }
}
