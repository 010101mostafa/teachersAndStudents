using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using System;
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
            try
            {
                var token = await account.SinUp(Model);
                if (token is not "" or null)
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
            catch (Exception e) { 
                Error.HaveError=true;
                Error.Message=e.Message;
            }
        }
    }
}
