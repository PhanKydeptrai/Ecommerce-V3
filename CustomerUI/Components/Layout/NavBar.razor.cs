using Blazored.SessionStorage;
using Domain.DTO_Models;
using Domain.Models;
using Microsoft.AspNetCore.Components;

namespace CustomerUI.Components.Layout
{
    public partial class NavBar
    {
        [Inject]
        public ISessionStorageService session { get; set; }

        //OnAfterRenderAsync được gọi khi component NavBar đã được render xong
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //Lấy thông tin user từ session
                var check = await session.GetItemAsync<LoginResponse>("UserInfo");
                //Thông báo cho Blazor rằng trạng thái của component đã thay đổi và cần được render lại.
                StateHasChanged();
            }
        }


    }
}
