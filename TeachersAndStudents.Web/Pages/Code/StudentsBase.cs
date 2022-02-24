using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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
        protected List<Class> _classes = new List<Class>();
        protected string selectedClass = "";
        protected int selectedNum = 0;
        [Inject] protected ITeacherServices services { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authState { get; set; }
        protected ClaimsPrincipal user{ get; set; }
        protected ERROR Error { get; set; }= new ERROR();

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                Model =await services.getStudent();
                _classes = (List<Class>)await services.getClass();
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
                if(_classes.Count==0)
                    throw new Exception("you don't have any classes please add a Class first and then add its students.");
                if(selectedClass=="")
                    throw new Exception("Please select a class");
                Class s_Class = _classes.FirstOrDefault(x => x.Name == selectedClass);
                var students = Model.FindAll(s => s.selected).ConvertAll(s => (Student)s);
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

        public void onSelect(StudentView s) {
            if (s.selected && s.ClassId is not null)
            {
                Error.HaveError = true;
                Error.Message = $"this student :{s.FullName} have a Class already!!";
                s.selected = false;
            }
            else if (s.selected) 
                selectedNum--;
            else
                selectedNum++;
        }
    }
}
