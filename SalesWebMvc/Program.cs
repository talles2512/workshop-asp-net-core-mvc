using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SalesWebMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run(); //Buildando o site e iniciando
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => //Definindo o método para a inicialização do site a partir da classe Startup
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>(); 
    }
}
