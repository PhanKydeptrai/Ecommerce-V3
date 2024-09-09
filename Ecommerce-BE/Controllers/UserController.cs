using AutoMapper;
using Domain.DTO_Models;
using Domain.Models;
using Ecommerce_BE.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace Ecommerce_BE.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userService;
        private readonly IToken _token;
        private readonly IMapper _mapper;
        public UserController(IUserServices userService, IToken token, IMapper mapper)
        {
            _userService = userService;
            _token = token;
            _mapper = mapper;
        }
        /// <summary>
        /// Lấy danh sách User dành cho role admin
        /// </summary>
        /// <remarks>
        /// Sample response:
        /// 
        ///     GET
        ///     [
        ///         {
        ///             "userId": "2b64cdbe-7fa9-4cff-b7ce-75676439be3b",
        ///             "email": "user@example.com",
        ///             "password": "$2b$10$V.y5mBjCUWuNME65rqxQsucE8877tBuHydBXDhxD40RC8/57w9iRS",
        ///             "gender": "string",
        ///             "phoneNumber": "0588234946",
        ///             "dateOfBirth": "2024-07-14T08:54:47.627",
        ///             "firstName": "string",
        ///             "lastName": "string",
        ///             "address": "string",
        ///             "image": "string",
        ///             "status": "hd",
        ///             "role": "admin",
        ///             "orders": null
        ///         }
        ///     ]        
        /// </remarks>
        /// <response code="200">Trả về danh sách user</response>
        /// <response code="404">Không tìm thấy user nào</response>
        /// <response code="401">Chưa xác thực</response>
        /// <response code="403">Khi người dùng không phải admin</response>
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Users()
        {
            IEnumerable<User> users = await _userService.GetUsers();
            if (users == null)
            {
                return NotFound("Không tìm thấy User");
            }
            return Ok(users);
        }

        //get user by email
        /// <summary>
        /// Trả về user dựa trên email (Vai trò được sử dụng: admin)
        /// </summary>
        /// <remarks>
        /// Sample response:
        /// 
        ///     GET
        ///         {
        ///             "userId": "2b64cdbe-7fa9-4cff-b7ce-75676439be3b",
        ///             "email": "user@example.com",
        ///             "password": "$2b$10$V.y5mBjCUWuNME65rqxQsucE8877tBuHydBXDhxD40RC8/57w9iRS",
        ///             "gender": "string",
        ///             "phoneNumber": "0588234946",
        ///             "dateOfBirth": "2024-07-14T08:54:47.627",
        ///             "firstName": "string",
        ///             "lastName": "string",
        ///             "address": "string",
        ///             "image": "string",
        ///             "status": "hd",
        ///             "role": "admin",
        ///             "orders": null
        ///         }        
        /// </remarks>
        /// <response code="200">Tìm thấy user</response>
        /// <response code="404">Không tìm thấy user</response>
        [HttpGet("{email}")]
        public async Task<IActionResult> User(string email)
        {
            var user = await _userService.GetUserByEmail(email);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound("Không tìm thấy User");
        }

        //Forget password(User auth)
        /// <summary>
        /// Gửi mã đăng nhập cho việc quên mật khẩu (Sử dụng bởi cả 2 role)
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UserAuthForForgetPassword(string email)
        {
            User userCheck = await _userService.GetUserByEmail(email);
            if(userCheck != null && userCheck.Status == "hd")
            {
                //Gửi mã xác nhận qua email
                //Random mã xác nhận
                string code = await _userService.RandomString();
                _userService.SendEmail(email, "Mã xác nhận khôi phục tài khoản ","Mã của bạn là:" + code);
                
                HttpContext.Session.SetString("MailConfirm", email);
                
                HttpContext.Session.SetString("code", code);
                return Ok("Mã xác nhận đã được gửi qua email");
            }
            return BadRequest("Email chưa được đăng ký");
        }

        /// <summary>
        /// Sử dụng để kiểm tra mã xác thực (Sử dụng bởi cả 2 role) 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        //CheckRandomCode
        [HttpGet]
        public async Task<IActionResult> CheckRandomCode(string code)
        {
            string codeCheck = HttpContext.Session.GetString("code");
            string mailConfirm = HttpContext.Session.GetString("MailConfirm");

            if (code == codeCheck)
            {
                return Ok(true);
            }
            return BadRequest("Mã xác nhận không đúng");
        }
        
        

        //create user by admin
        /// <summary>
        /// Thêm một User (Vai trò được sử dụng: admin)
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST
        ///     {
        ///         "email": "user@example.com",
        ///         "password": "strongpass",
        ///         "gender": "Male" or "Female"
        ///         "phoneNumber": "0932712427",
        ///         "dateOfBirth": "2004-08-8T07:47:28.328Z",
        ///         "firstName": "Ky",
        ///         "lastName": "Phan The",
        ///         "address": "abc street",
        ///         "image": "user.png",
        ///         "status": "hd",
        ///         "role": "user" or "admin"
        ///     }
        /// </remarks>
        /// <response code="200">Thêm thành công</response>
        /// <response code="400">Dữ liệu nhập vào không đầy đủ hoặc null</response>
        /// <response code="401">Chưa xác thực</response>
        /// <response code="403">Khi người dùng không phải admin</response>
        /// <param name="user"></param>
        [HttpPost]
        public async Task<IActionResult> User(UserCreationDto user)
        {
            if (ModelState.IsValid && user != null) //kiểm tra kiểu dữ liệu
            {
                var userExist = await _userService.GetUserByEmail(user.Email);
                if (userExist == null)
                {
                    user.UserId = Guid.NewGuid(); //Tạo mới id
                    user.Status = "hd"; //Trạng thái hoạt động   
                    user.Password = await _userService.EncryptPassword(user.Password); //Mã hóa mật khẩu
                    if (await _userService.CreateUser(user) == true)
                    {
                        return Ok("Thêm thành công");
                    }
                    //return thông báo khi nhập sai thông tin
                    var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return BadRequest(new { message });
                }
            }
            return BadRequest("Thêm không thành công");
        }

        //login
        /// <summary>
        /// Đăng nhập (Vai trò được sử dụng: user hoặc admin)
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST
        ///     {
        ///         "email": "user@example.com",
        ///         "password": "strongpass"
        ///     }
        /// </remarks>
        /// <response code="200">Đăng nhập thành công và trả về token</response>
        /// <response code="404">Tài khoản không tồn tại</response>
        /// <response code="400">Nhập thiếu dữ liệu hoặc input bị null</response>
        [HttpPost]
        public async Task<IActionResult> Login(UserLogin user)
        {
            if (ModelState.IsValid && user != null)
            {
                User userExist = await _userService.GetUserByEmail(user.Email);
                
                if (userExist != null)
                {
                    bool isverify = BCrypt.Net.BCrypt.Verify(user.Password, userExist.Password);
                    if (userExist != null && isverify == true && userExist.Status == "hd")
                    {
                        var token = await _token.CreateToken(userExist);
                        //Tạo response trả về
                        LoginResponse res = new LoginResponse
                        {
                            UserId = userExist.UserId.ToString(),
                            Token = token,
                            Role = userExist.Role,
                            UserName = userExist.FirstName + " " + userExist.LastName,
                            Email = userExist.Email
                        };
                        //Tạo session
                        HttpContext.Session.SetString("LoginResponse", JsonConvert.SerializeObject(res));
                        return Ok(res);
                    }
                    return BadRequest("Sai tài khoản hoặc mật khẩu");
                }
                return NotFound("Tài khoản không tồn tại");
            }
            var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { message });
        }


        //Check login status
        //Why: Lấy thông tin user đang đăng nhập

        /// <summary>
        /// Lấy thông tin của user đang đăng nhập
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> CheckLoginStatus()
        {
            //string session chứa dữ liệu trong Session LoginResponse
            string session = HttpContext.Session.GetString("LoginResponse");
            //Nếu chưa đăng nhập
            if (session == null)
            {
                return BadRequest("Vui lòng đăng nhập");
            }
            //Deserialize session thành LoginResponse
            LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(session);
            return Ok(loginResponse);

        }


        //register user
        /// <summary>
        /// Đăng ký tài khoản (Vai trò được sử dụng: guest)
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST
        ///     {
        ///         "email": "user@example.com",
        ///         "password": "strongpass",
        ///         "gender": "Male" or "Female"
        ///         "phoneNumber": "0932712427",
        ///         "dateOfBirth": "2004-08-8T07:47:28.328Z",
        ///         "firstName": "Ky",
        ///         "lastName": "Phan The",
        ///         "address": "abc street",
        ///         "image": "user.png",
        ///         "status": "hd",
        ///         "role": "user" or "admin"
        ///     }
        /// </remarks>
        /// <response code="200">Đăng ký thành công</response>
        /// <response code="400">Email đã được sử dụng</response>
        [HttpPost]
        public async Task<IActionResult> Register(UserCreationDto user)
        {
            if (ModelState.IsValid && user != null)
            {
                var userExist = await _userService.GetUserByEmail(user.Email);

                if (userExist == null)
                {
                    user.UserId = Guid.NewGuid();
                    user.Status = "hd";
                    user.Role = "user";
                    user.Password = await _userService.EncryptPassword(user.Password); //Mã hóa mật khẩu
                    if (await _userService.CreateUser(user) == true)
                    {
                        return Ok("Đăng ký thành công");
                    }
                }
                return BadRequest("Email đã được sử dụng");
            }
            var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { message });
        }

        //Remove user by id
        /// <summary>
        /// Xoá user dựa trên id (Vai trò được sử dụng: admin)
        /// </summary>
        /// <remarks>
        /// Sample response: Một thông báo trả về sau khi xoá user
        ///     
        ///           
        /// </remarks>
        /// <response code="200">Xoá thành công</response>
        /// <response code="404">User đã ngừng hoạt động</response>
        /// <response code="400">Xoá không thành công</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> User(Guid id)
        {
            var user = await _userService.GetUserById(id);
            if (user.Status != "nhd")
            {
                if (await _userService.RemoveUser(id) == true)
                {
                    return Ok("Xóa thành công");
                }
                return BadRequest("Xóa không thành công");
            }
            return NotFound("User đã ngừng hoạt động");
        }

        //update user
        /// <summary>
        /// Cập nhật thông tin tài khoản cá nhân (Vai trò được sử dụng: user hoặc admin)
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT
        ///     {
        ///         "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "email": "user@example.com",
        ///         "password": "strongpass",
        ///         "gender": "Male" or "Female"
        ///         "phoneNumber": "0932712427",
        ///         "dateOfBirth": "2004-08-8T07:47:28.328Z",
        ///         "firstName": "Ky",
        ///         "lastName": "Phan The",
        ///         "address": "abc street",
        ///         "image": "user.png",
        ///         "status": "hd",
        ///         "role": "user" or "admin"
        ///     }
        /// </remarks>
        /// <response code="200">Cập nhật thành công</response>
        /// <response code="400">Dữ liệu nhập vào không đầy đủ hoặc null</response>
        /// <response code="401">Chưa xác thực</response>
        /// <response code="403">Khi người dùng không phải user</response>

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserCreationDto user)
        {

            if (ModelState.IsValid && user != null) //Validate
            {
                //Thông tin user trước update
                User userBeforeUpdate = await _userService.GetUserById(user.UserId ?? Guid.Empty);
                if (userBeforeUpdate.Email == user.Email || await _userService.GetUserByEmail(user.Email) == null)
                {
                    //Cho update
                    if (await _userService.UpdateUser(user) == true)
                    {
                        return Ok("Cập nhật thành công");
                    }
                    return BadRequest("Cập nhật không thành công");
                }
                return BadRequest("Email đã được sử dụng");
            }
            var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(new { message });
        }

        /// <summary>
        /// Khôi phục tài khoản bị ngưng hoạt động (Vai trò được sử dụng: admin)    
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> AccountRestore(Guid id)
        {
            if (await _userService.AccountRestore(id) == true)
            {
                return Ok("Khôi phục tài khoản thành công");
            }
            return BadRequest("Khôi phục tài khoản không thành công");
        }

        
    }
}
