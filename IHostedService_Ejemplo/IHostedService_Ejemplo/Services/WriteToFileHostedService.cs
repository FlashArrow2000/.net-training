using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IHostedService_Ejemplo.Services
{
    public class WriteToFileHostedService : IHostedService, IDisposable
    {
        private readonly IHostingEnvironment environment;
        private readonly string fileName = "File 1.txt";
        private Timer timer;

        public WriteToFileHostedService(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        //Este método se va a ejecutar cuando empiece o se monte la aplicacion
        public Task StartAsync(CancellationToken cancellationToken)
        {
            WriteToFile("WriteToFileHostedService: Process Started!");
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            throw new NotImplementedException();
        }

        private void DoWork(object state)
        {
            WriteToFile("WriteToFileHostedService: Doing some work at " + DateTime.Now.ToString("dd/MM/yyyyy hh:mm:ss"));
        }

        //Este método se va a ejecutar cuando finalice o se desmonte la aplicacion
        public Task StopAsync(CancellationToken cancellationToken)
        {
            WriteToFile("WriteToFileHostedService: Process Stopped!");
            timer?.Change(Timeout.Infinite, 0);
            throw new NotImplementedException();
        }

        private void WriteToFile(string message)
        {
            var path = $@"{environment.ContentRootPath}\wwwroot\{fileName}";
            using (StreamWriter writer = new StreamWriter(path, append: true))
            {
                writer.WriteLine(message);
            }
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
