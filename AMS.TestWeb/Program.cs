using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Jerrisoft.Platform.TestWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            CreateWebHostBuilder(args).Build().Run();
        }

        private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var container = Assembly.GetExecutingAssembly();
            var path = new AssemblyName(args.Name).Name + ".dll";

            if (path.StartsWith("AMS"))
            {
                return Assembly.LoadFile($"{AppDomain.CurrentDomain.BaseDirectory}\\Controller\\{path}");
            }

            using (var stream = container.GetManifestResourceStream(path))
            {
                if (stream == null)
                {
                    return null;
                }

                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                return Assembly.Load(bytes);
            }


        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
