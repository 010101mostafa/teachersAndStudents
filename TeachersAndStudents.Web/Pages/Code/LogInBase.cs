using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using System;
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

        protected ERROR Error { get; set; }= new ERROR();
        public async Task onSubmitAsync() {
            try {
                var token = await account.LogIn(Model);
                if (token is not "" or null)
                {
                    if (Model.RememberMe)
                    {
                        await local.SetItemAsync("Token", token);
                    }
                    else {
                        await session.SetItemAsync("Token", token);
                    }
                    if (returnUri is null)
                        navigationManager.NavigateTo("/", true);
                    else
                        navigationManager.NavigateTo(returnUri, true);
                }

            }
            catch (Exception e) 
            {
                Error.HaveError = true;
                Error.Message = e.Message;
            }
        }
    }
}
