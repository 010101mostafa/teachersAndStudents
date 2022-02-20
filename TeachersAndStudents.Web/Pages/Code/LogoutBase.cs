using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;

namespace TeachersAndStudents.Web.Pages
{
    public class LogoutBase : ComponentBase
    {
        [Inject]
        protected ILocalStorageService local { get; set; }
        [Inject]
        protected ISessionStorageService session { get; set; }
        [Inject]
        protected NavigationManager navigationManager { get; set; }

        public async Task logout() {
            await local.ClearAsync();
            await session.ClearAsync();
            navigationManager.NavigateTo($"login/{navigationManager.ToBaseRelativePath(navigationManager.Uri)}",
                true);
        }

    }
}
