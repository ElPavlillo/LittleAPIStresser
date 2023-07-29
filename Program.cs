using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace LittleAPIStresser
{
    internal class Program
    {
        public static async Task PeticionAsync()
        {
            var proxy = new WebProxy
            {
                Address = new Uri("socks4://127.0.0.1:9050")
            };

            var handler = new HttpClientHandler() { Proxy = proxy };
            var httpClient = new HttpClient(handler);
            var ip = await httpClient.GetStringAsync("https://api.ipify.org");
            Console.WriteLine($"My public IP address is: {ip}");
        }

        public static async Task<bool> DinamicCode(string code)
        {
            bool result = false;
            try
            {
                // Create a list of references to assemblies that the code might depend on.
                var references = new[]
                {
                    typeof(object).Assembly,
                    typeof(Console).Assembly,
                    typeof(HttpClient).Assembly,
                    typeof(Task).Assembly,
                    typeof(DecompressionMethods).Assembly,
                    // Add other assemblies if needed
                };

                // Create options with the necessary references.
                var options = ScriptOptions.Default
                    .AddReferences(references)
                    .AddImports("System", "System.Net", "System.Net.Http", "System.Threading.Tasks");

                // Evaluate the code and get the result.
                var response = await CSharpScript.EvaluateAsync(code, options);

                // Do something with the result (if required).
                Console.WriteLine("Result: " + response);
                result = true;
            }
            catch (CompilationErrorException compilationError)
            {
                // Handle compilation errors if any.
                foreach (var diagnostic in compilationError.Diagnostics)
                {
                    Console.WriteLine(diagnostic.ToString());
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions if any.
                Console.WriteLine("Error: " + ex.Message);
            }
            return result;
        }

        public static string LeerArchivo(string filePath)
        {
            string code = File.ReadAllText(filePath);

            return code;
        }

        public static void ThreadLauncher(int ThreadsNumber, string code)
        {
            Task[] tasks = new Task[ThreadsNumber];

            for (int i = 0; i < ThreadsNumber; i++)
            {
                tasks[i] = DinamicCode(code);
            }

        }

        static async Task Main(string[] args)
        {
            string code = LeerArchivo(args[0]);
            //Task<bool> task = Task.Run(() => DinamicCode(LeerArchivo(args[0])));
            //Console.WriteLine(await task);

            var resul = int.TryParse(args[1],out int ThreadsNumber);

            if (resul)
            {
                ThreadLauncher(ThreadsNumber, code);
            }
            else
            {
                Console.WriteLine("Invalid params ThreadsNumber");
            }

            Console.ReadLine();
        }
    }
}