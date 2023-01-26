namespace API.Services.Services
{
    public class EscribirEnArchivo : IHostedService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string nombreArchivo = "Archivo1.txt";
        private Timer? _timer;

        public EscribirEnArchivo(IWebHostEnvironment env)
        {
            _env = env;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new (DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Escribir("Proceso Iniciado");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer!.Dispose();
            Escribir("Proceso Finalizado");
            return Task.CompletedTask;
        }

        private void DoWork(Object? state)
        {
            Escribir("Proceso en ejecucion " + DateTime.Now.ToString("dd/MM/yyy hh:mm:ss")); 
        }

        private void Escribir(string mensaje)
        {
            var ruta = $@"{_env.ContentRootPath}\wwwroot\{nombreArchivo}";
            using (StreamWriter writer = new (ruta, append: true))
            {
                writer.WriteLine(mensaje);
            }
        }
    }
}
