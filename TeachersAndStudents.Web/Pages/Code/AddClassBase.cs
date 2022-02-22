using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using TeachersAndStudents.models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using TeachersAndStudents.Web.Services;
using System;

namespace TeachersAndStudents.Web.Pages
{
    public class AddClassBase : ComponentBase
    {
        protected Class Model = new Class();
        [Inject]protected ITeacherServices services { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authState { get; set; }
        protected ClaimsPrincipal user { get; set; }

        protected ERROR Error { get; set; }=new ERROR();
        protected override async void OnParametersSet()
        {
            base.OnParametersSet();
            if (authState is not null)
                user = (await authState).User;
        }
        public async Task onSubmitAsync()
        {
            Model.TeacherId= user.FindFirst("uid").Value;
            try
            {
                await services.addClass(Model);
            }
            catch (Exception e)
            {
                Error.HaveError = true;
                Error.Message = e.Message;
            }
            Model=new Class();
        }
    }
}
