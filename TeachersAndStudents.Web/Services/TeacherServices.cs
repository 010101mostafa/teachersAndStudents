using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TeachersAndStudents.models;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using System;
using System.Net.Http.Headers;
using System.Net;

namespace TeachersAndStudents.Web.Services
{
    public interface ITeacherServices
    {
        public Task<List<StudentView>> getStudent();
        public Task AddToAClass(Student student);
        public Task addClass(Class _class);
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

        public async Task addClass(Class _class)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToke());

            var res = await httpClient.PostAsJsonAsync("/Teacher/addClass", _class);
            if (res.StatusCode == HttpStatusCode.BadRequest)
                throw new Exception(await res.Content.ReadAsStringAsync());
            if (!res.IsSuccessStatusCode)
                throw new Exception("StatusCode :" + res.StatusCode);
        }

        public async Task AddToAClass(Student student)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToke());
            var res = await httpClient.PostAsJsonAsync("/Teacher/AddToAClass", student);
            if (res.StatusCode == HttpStatusCode.BadRequest)
                throw new Exception(await res.Content.ReadAsStringAsync());
            if (!res.IsSuccessStatusCode)
                throw new Exception("StatusCode :" + res.StatusCode);
        }

        public async Task<IEnumerable<Class>> getClass()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToke());
            var res = await httpClient.GetAsync("/Teacher/getClass");
            if (res.StatusCode == HttpStatusCode.BadRequest)
                throw new Exception(await res.Content.ReadAsStringAsync());
            if (!res.IsSuccessStatusCode)
                throw new Exception("StatusCode :" + res.StatusCode);
            return await res.Content.ReadFromJsonAsync<IEnumerable<Class>>();
        }

        public async Task<List<StudentView>> getStudent()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToke());
            var res = await httpClient.GetAsync("/Teacher/getStudent");
            if (res.StatusCode == HttpStatusCode.BadRequest)
                throw new Exception(await res.Content.ReadAsStringAsync());
            if (!res.IsSuccessStatusCode)
                throw new Exception("StatusCode :" + res.StatusCode);
            var ans= (await res.Content.ReadFromJsonAsync<List<Student>>());
            return ans.ConvertAll(x => new StudentView { 
                FullName= x.FullName,
                Class= x.Class,
                user= x.user,});
        }
    }
}
