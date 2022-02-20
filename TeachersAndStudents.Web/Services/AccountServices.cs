
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TeachersAndStudents.models;

namespace TeachersAndStudents.Web.Services
{
    public interface IAccountServices { 
        public Task<string> LogIn(Login model);
        public Task<string> SinUp(SignUp model);
    }
    public class AccountServices: IAccountServices
    {
        private readonly HttpClient httpClient;

        public AccountServices(HttpClient httpClient) {
            this.httpClient = httpClient;
        }
        public async Task<string> LogIn(Login model) {
            var res = await httpClient.PostAsJsonAsync<Login>("/Account", model);
            if (!res.IsSuccessStatusCode)
                return null;
            return await res.Content.ReadAsStringAsync();
           
        }

        public async Task<string> SinUp(SignUp model)
        {
            var res = await httpClient.PostAsJsonAsync<Login>("/Account/SignUp", model);
            if (!res.IsSuccessStatusCode)
                return null;
            return await res.Content.ReadAsStringAsync();
        }
    }
}
