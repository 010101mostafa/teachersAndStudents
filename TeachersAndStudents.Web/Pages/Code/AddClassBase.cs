using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using TeachersAndStudents.models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using TeachersAndStudents.Web.Services;

namespace TeachersAndStudents.Web.Pages
{
    public class AddClassBase : ComponentBase
    {
        protected Class Model = new Class();
        [Inject]protected ITeacherServices services { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authState { get; set; }
        protected ClaimsPrincipal user { get; set; }

        protected bool hasError { get; set; }
        protected override async void OnParametersSet()
        {
            base.OnParametersSet();
            user = (await authState).User;
        }
        public async Task onSubmitAsync()
        {
            Model.TeacherId= user.FindFirst("uid").Value;
            hasError = !(await services.addClass(Model));

            Model=new Class();
        }
    }
}
