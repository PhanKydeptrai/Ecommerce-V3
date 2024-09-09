using Domain.DTO_Models;
using Domain.Models;
using Ecommerce_BE.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_BE.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ComboDetailController : ControllerBase
    {
        private readonly IComboDetailServices _comboDetailServices;
        public ComboDetailController(IComboDetailServices comboDetailServices)
        {
            _comboDetailServices = comboDetailServices;
        }


        [HttpGet]        
        public async Task<IActionResult> ComboDetails()
        {
            var result = await _comboDetailServices.GetComboDetails();
            return Ok(result);
        }

        [HttpGet("{id}")] //combodetail id
        public async Task<IActionResult> ComboDetail(Guid id)
        { 
            var comboDetail = await _comboDetailServices.GetComboDetailById(id);
            if(comboDetail != null)
            {
                return Ok(comboDetail);
            }
            
            return NotFound("Không tìm thấy combodetail");
        }

        //trả về danh sách combodetail theo comboId 
        [HttpGet("{id}")] //Combo Id
        public async Task<IActionResult> ComboDetailByComboId(Guid id)
        {
            IEnumerable<ComboDetail> comboDetail = await _comboDetailServices.GetComboDetailByComboId(id);
            if(comboDetail == null)
            {
                return NotFound("Không tìm thấy combodetail");
            }
            return Ok(comboDetail);
        }

        [HttpPost]
        public async Task<IActionResult> AddComboDetail(ComboDetailCreationDto comboDetailCreation)
        {
            comboDetailCreation.ComboDetailId = Guid.NewGuid();
            var result = await _comboDetailServices.AddComboDetail(comboDetailCreation);
            if(result)
            {
                return Ok("Thêm combodetail thành công");
            }
            return BadRequest("Thêm combodetail thất bại");
        }

        [HttpDelete("{id}")] //combodetail id
        public async Task<IActionResult> RemoveComboDetail(Guid id)
        {
            var result = await _comboDetailServices.RemoveComboDetail(id);
            if(result)
            {
                return Ok("Xóa combodetail thành công");
            }
            return BadRequest("Xóa combodetail thất bại");
        }
    }
}
