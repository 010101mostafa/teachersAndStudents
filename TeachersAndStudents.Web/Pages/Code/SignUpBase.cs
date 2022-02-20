using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using TeachersAndStudents.models;
using TeachersAndStudents.Web.Services;

namespace TeachersAndStudents.Web.Pages
{
    public class SignUpBase:LogInBase
    {
        protected new SignUp Model = new SignUp();
        public new async Task onSubmitAsync()
        {
            var token = await account.SinUp(Model);
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
                else
                {
                    await session.SetItemAsync("Token", token);
                }
                navigationManager.NavigateTo("/", true);
            }
        }
    }
}
