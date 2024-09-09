using Blazored.SessionStorage;
using Domain.DTO_Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Text;

namespace CustomerUI.Components.Pages
{
    public partial class Login
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService session { get; set; }
        UserLogin userLogin = new UserLogin();

        protected async Task UserLogin()
        {
            using (HttpClient client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "token");
                StringContent content = new StringContent(JsonConvert.SerializeObject(userLogin), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync($"https://localhost:7206/api/User/Login", content))
                {
                    //IsSuccessStatusCode: kiểm tra xem request có thành công không
                    if(response.IsSuccessStatusCode) //Đăng nhập thành công 
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(apiResponse);
                        if (loginResponse.Role == "admin") //Nếu là admin thì chuyển hướng đến trang admin(Web assembly)
                        {
                            NavigationManager.NavigateTo("https://example/");
                        }
                        else //Nếu là user thì chuyển hướng đến trang chính
                        {
                            //tạo session storage
                            await session.SetItemAsync("UserInfo", loginResponse);
                            NavigationManager.NavigateTo("/");
                        }
                    }
                    else
                    {
                        NavigationManager.NavigateTo("/Login");
                    }   
                }
            }

            ////Gọi API để kiểm tra trạng thái đăng nhập
            //using (HttpClient client = new HttpClient())
            //{
            //    using (var response = await client.GetAsync("https://localhost:7206/api/User/CheckLoginStatus"))
            //    {
            //        string apiResponse = await response.Content.ReadAsStringAsync();
            //        //Xử lý thông tin trả về từ api
            //        try
            //        {
            //            LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(apiResponse);
            //        }
            //        catch (HttpRequestException)
            //        {
            //            NavigationManager.NavigateTo("/Login");
            //        }
            //    }
            //}
        }
    }

   

}
