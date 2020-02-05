using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BankOfDotNet.ConsoleClient
{
    class Program
    {
        static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        [Obsolete]
        private static async Task MainAsync()
        {
            //dicover all the endpoints using metadata od identity server
            var disco = await IdentityModel.Client.DiscoveryClient.GetAsync("http://localhost:50000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            //grab a bearer token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("bankOfDotNetApi");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            //cosumer our Customer API
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var customerInfo = new StringContent(
                JsonConvert.SerializeObject(
                    new {Id = 10, FirstName = "Manish", LastName = "Narayan"}),
                    Encoding.UTF8, "application/json");

            var createCustomerResponse = await client.PostAsync("http://localhost:63504/api/customers", customerInfo);
            if (!createCustomerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(createCustomerResponse.StatusCode);
            }

            var getCustomerResponse = await client.GetAsync("http://localhost:63504/api/customers");
            if (!getCustomerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(getCustomerResponse.StatusCode);
            }
            else
            {
                var content = await getCustomerResponse.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }

            Console.Read();
        }
    }
}
