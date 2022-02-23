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
                Model =await services.getStudent();
                var _Class = await services.getClass();

                if (_Class is not null)
                    foreach (var c in _Class)
                        _class.Add(c.Name);
                else
                    throw new Exception("you don't have any classes please add the Class first and then add its students.");
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
                Class s_Class = _Class.FirstOrDefault(x => x.Name == selectedClass);
                var students = Model.FindAll(s => s.selected).ConvertAll(s => (Student)s);
                if (students is not null)
                    foreach (var s in students)
                    {
                        s.ClassId = s_Class.Id;
                        await services.AddToAClass(s);
                    }
            }
            catch (Exception e)
            {
                Error.HaveError = true;
                Error.Message=e.Message;
            }
        }

        public void onSelect(int i) {
            if (!Model[i].selected && Model[i].ClassId is not null)
            {
                Error.HaveError = true;
                Error.Message = $"this student :{Model[i].FullName} have a Class already!!";
                Model[i].selected = false;
            }
            else if (Model[i].selected) 
                selectedNum--;
            else
                selectedNum++;
        }
    }
}
