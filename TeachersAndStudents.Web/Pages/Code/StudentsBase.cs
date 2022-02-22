using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TeachersAndStudents.models;
using TeachersAndStudents.Web.Services;
using System.Linq;
using System;
namespace TeachersAndStudents.Web.Pages
{
    public class StudentsBase:ComponentBase
    {
        protected List<StudentView> Model = new List<StudentView>();
        protected List<string> _class = new List<string>();
        protected string selectedClass = "";
        protected int selectedNum = 0;
        [Inject] protected ITeacherServices services { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authState { get; set; }
        protected ClaimsPrincipal user{ get; set; }

        public Empty empty { get; set; }

        protected ERROR Error { get; set; }= new ERROR();
        protected override async void OnParametersSet()
        {
            base.OnParametersSet();
            try
            {
                Model = (List<StudentView>)await services.getStudent();
                var _Class = await services.getClass();

                if (_Class is not null)
                    foreach (var c in _Class)
                        _class.Add(c.Name);
            }
            catch (Exception e) { 
                Error.HaveError = true;
                Error.Message = e.Message; 
            }
            if (authState is not null)
                user = (await authState).User;
        }
        public async Task onSubmitAsync()
        {
            try
            {
                var _Class = (List<Class>)await services.getClass();
                var s_Class = _Class.FirstOrDefault(x => x.Name == selectedClass);
                var students = Model.FindAll(s => s.selected).ConvertAll(s => (Student)s);
                if (students is not null)
                    foreach (var s in students)
                    {
                        s.Class = s_Class;
                        await services.AddToAClass(s);
                    }
            }
            catch (Exception e)
            {
                Error.HaveError = true;
                Error.Message=e.Message;
            }
        }
    }
}
