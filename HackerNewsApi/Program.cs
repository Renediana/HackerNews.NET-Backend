using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using LettuceEncrypt;
namespace HackerNewsApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHost(builder => builder.UseKestrel(k =>
                {
                    k.Listen(IPAddress.Any, 5000);
                    k.Listen(IPAddress.Any, 5001, o => o.UseHttps(h => h.UseLettuceEncrypt(k.ApplicationServices)));
                })
               .UseStartup<Startup>());
    }
}
