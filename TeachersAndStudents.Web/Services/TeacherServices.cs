using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TeachersAndStudents.models;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using System;
using System.Net.Http.Headers;

namespace TeachersAndStudents.Web.Services
{
    public interface ITeacherServices
    {
        public Task<IEnumerable<StudentView>> getStudent();
        public Task<bool> AddToAClass(Student student);
        public Task<bool> addClass(Class _class);
        public Task<IEnumerable<Class>> getClass();
    }
    public class TeacherServices : ITeacherServices
    {
        private readonly HttpClient httpClient;
        private readonly ISessionStorageService session;
        private readonly ILocalStorageService local;

        public TeacherServices(HttpClient httpClient ,ISessionStorageService session,ILocalStorageService local)
        {
            this.httpClient = httpClient;
            this.session = session;
            this.local = local;
        }

        private async Task<string> GetToke() {
            try
            {
                return await session.GetItemAsync<string>("Token");
            }
            catch (Exception)
            {
                try
                {
                    return await local.GetItemAsync<string>("Token");
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        public async Task<bool> addClass(Class _class)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToke());

            var res = await httpClient.PostAsJsonAsync<Class>("/Teacher/addClass", _class);
            if (res.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<bool> AddToAClass(Student student)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToke());
            var res = await httpClient.PostAsJsonAsync("/Teacher/AddToAClass", student);
            if (res.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<IEnumerable<Class>> getClass()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToke());
            try
            {
                return await httpClient.GetFromJsonAsync<IEnumerable<Class>>("/Teacher/getClass");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<StudentView>> getStudent()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToke());
            try
            {
                var res=await httpClient.GetFromJsonAsync<List<Student>>("/Teacher/getStudent");
                return res.ConvertAll(x => (StudentView)x);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
