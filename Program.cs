using Curl.CommandLine.Parser;
using Curl.HttpClient.Converter;
using System.Net;

namespace LittleAPIStresser
{
    internal class Program
    {
        public static async Task PeticionAsync(string peticionCURL)
        {
            var input = @"curl https://sentry.io/api/0/projects/1/groups/?status=unresolved -d '{""status"": ""resolved""}' -H 'Content-Type: application/json' -u 'username:password' -H 'Accept: application/json' -H 'User-Agent: curl/7.60.0'";
            var curlOption = new CurlParser().Parse(input);
            var output = new CurlHttpClientConverter().ToCsharp(curlOption.Data);

            var proxy = new WebProxy
            {
                Address = new Uri("socks4://127.0.0.1:9050")
            };

            var handler = new HttpClientHandler() { Proxy = proxy };
            var httpClient = new HttpClient(handler);
            var ip = await httpClient.GetStringAsync("https://api.ipify.org");
            Console.WriteLine($"My public IP address is: {ip}");
        }

        static void Main(string[] args)
        {
            string peticionCURL = "";

            PeticionAsync(peticionCURL).Wait();
            Console.ReadLine();
        }
    }
}