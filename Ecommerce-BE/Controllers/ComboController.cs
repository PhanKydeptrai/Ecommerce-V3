using Domain.DTO_Models;
using Domain.Models;
using Ecommerce_BE.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_BE.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ComboController : ControllerBase
    {
        private readonly IComboServices _comboServices;

        
        public ComboController(IComboServices comboServices)
        {
            _comboServices = comboServices;
        }

        /// <summary>
        /// Lấy danh sách combo
        /// </summary>
        /// <remarks>
        /// Sample response:
        /// 
        ///     GET
        ///         [
        ///             {
        ///                 "comboId": "3f336220-7650-4945-86b4-685c47a944b6",
        ///                 "comboName": "string",
        ///                 "description": "string",
        ///                 "image": "string",  
        ///                 "itemId": "055586cb-b196-47fb-a8f7-543224dfe972"
        ///             }   
        ///         [
        /// </remarks>
        /// <response code="200">Lấy dữ liệu thành công</response>
        /// <response code="404">không tìm thấy dữ liệu</response>
        
        [HttpGet]
        public async Task<IActionResult> GetCombos()
        {
            var result = await _comboServices.GetCombos();
            return Ok(result);
        }

        /// <summary>
        /// Lấy danh sách combo theo id
        /// </summary>
        /// <remarks>
        /// Sample response:
        /// 
        ///     GET
        ///     {
        ///         "comboId": "3f336220-7650-4945-86b4-685c47a944b6",
        ///         "comboName": "string",
        ///         "description": "string",
        ///         "image": "string",  
        ///         "itemId": "055586cb-b196-47fb-a8f7-543224dfe972"
        ///     }  
        /// </remarks>
        /// <response code="200">Lấy dữ liệu thành công</response>
        /// <response code="404">không tìm thấy dữ liệu</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComboById(Guid id)
        {
            var combo = await _comboServices.GetComboById(id);
            if(combo == null)
            {
                return NotFound("Không tìm thấy combo");
            }
            return Ok(combo);
        }

        /// <summary>
        /// Lấy danh sách combo theo tên
        /// </summary>
        /// <remarks>
        /// Sample response:
        /// 
        ///     GET
        ///     {
        ///         "comboId": "3f336220-7650-4945-86b4-685c47a944b6",
        ///         "comboName": "string",
        ///         "description": "string",
        ///         "image": "string",  
        ///         "itemId": "055586cb-b196-47fb-a8f7-543224dfe972"
        ///     }  
        /// </remarks>
        /// <response code="200">Lấy dữ liệu thành công</response>
        /// <response code="404">không tìm thấy dữ liệu</response>
        [HttpGet("{name}")]
        public async Task<IActionResult> GetComboByName(string name)
        {
            var combo = await _comboServices.GetComboByName(name);
            if(combo == null)
            {
                return NotFound("Không tìm thấy combo");
            }
            return Ok(combo);
        }


        /// <summary>
        /// Thêm combo mới
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST
        ///     {
        ///         "comboId": "3f336220-7650-4945-86b4-685c47a944b6",
        ///         "comboName": "string",
        ///         "description": "string",
        ///         "image": "string",  
        ///         "itemId": "055586cb-b196-47fb-a8f7-543224dfe972"
        ///     }  
        /// </remarks>
        /// <response code="200">Thêm thành công</response>
        /// <response code="400">Dữ liệu không đầy đủ hoặc tên bị trùng</response>
        [HttpPost]
        public async Task<IActionResult> AddCombo(ComboCreationDto combo)
        {
            if (ModelState.IsValid && combo != null)
            {
                var checkCombo = await _comboServices.GetComboByName(combo.ComboName);
                if(checkCombo == null)
                {
                    combo.ComboId = Guid.NewGuid();
                    var result = await _comboServices.AddCombo(combo);
                    if (result)
                    {
                        return Ok("Thêm thành công combo mới");
                    }
                }
                return BadRequest("Tên combo đã tồn tại");
            }
            var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { message });
            
        }


        /// <summary>
        /// Thêm combo mới
        /// </summary>
        /// <remarks>
        /// Sample resquest:
        ///     
        ///     POST
        /// 
        ///     ID: 3f336220-7650-4945-86b4-685c47a944b6 Example    
        /// 
        ///     {
        ///         "comboName": "string",
        ///         "description": "string",
        ///         "image": "string",  
        ///         "itemId": "055586cb-b196-47fb-a8f7-543224dfe972"
        ///     }  
        /// </remarks>
        /// <response code="200">Thêm thành công</response>
        /// <response code="400">Dữ liệu không đầy đủ hoặc tên bị trùng</response>
        [HttpPost]
        public async Task<IActionResult> UpdateCombo(Guid id ,ComboCreationDto combo)
        {
            if(ModelState.IsValid && combo != null)
            {
                var comboBeforeupdate = await _comboServices.GetComboById(id) ?? new Combo();
                if(comboBeforeupdate.ComboName == combo.ComboName || await _comboServices.GetComboByName(combo.ComboName) == null)
                {
                    var result = await _comboServices.UpdateCombo(combo);
                    if (result)
                    {
                        return Ok("Cập nhật combo thành công");
                    }
                }
                return BadRequest("Tên combo đã tồn tại");
                
            }
            var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { message });
        }
    }
}
