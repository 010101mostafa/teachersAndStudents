using System.Text;

namespace teachersAndStudents.API.Helpers
{
    public class Jwt
    {
        public string key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int DurationInDays { get; set; }
        public byte[] Key() { return Encoding.ASCII.GetBytes(key); }

    }
}
