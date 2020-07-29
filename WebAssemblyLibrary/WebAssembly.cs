using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebAssemblyLibrary.Server
{
    public class WebAssembly
    {
        public static IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder().UseStartup<Startup>(); 
        }
    }
}
