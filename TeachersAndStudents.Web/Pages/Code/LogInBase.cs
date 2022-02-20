using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using TeachersAndStudents.models;
using TeachersAndStudents.Web.Services;

namespace TeachersAndStudents.Web.Pages
{
    public class LogInBase:ComponentBase
    {
        protected Login Model = new Login();
        [Inject]
        protected ILocalStorageService local { get; set; }
        [Inject]
        protected ISessionStorageService session { get; set; }
        [Inject]
        protected IAccountServices account { get; set; }
        [Inject]
        protected NavigationManager navigationManager { get; set; }
        [Parameter] public string returnUri { get; set; }

        protected bool hasError { get; set; }
        public async Task onSubmitAsync() {
            var token=await account.LogIn(Model);
            if (token is "" or null)
            {
                hasError = true;
            }
            else
            {
                if (Model.RememberMe)
                {
                    await local.SetItemAsync("Token", token);
                }
                else {
                    await session.SetItemAsync("Token", token);
                }
                if (returnUri is null)
                    navigationManager.NavigateTo("/",true);
                else
                    navigationManager.NavigateTo(returnUri,true);

        }
        }
    }
}
