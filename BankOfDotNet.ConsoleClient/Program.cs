using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

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


        }
    }
}
