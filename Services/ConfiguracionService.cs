using Microsoft.Extensions.Configuration;
using Interfaces;

namespace Services
{
    public class ConfiguracionService : IServicioConfiguracion
    {
        private readonly IConfiguration _configuration;

        public ConfiguracionService()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public string ObtenerCadenaConexion()
        {
            return _configuration.GetConnectionString("PostgresConnection") ?? "";
        }
    }
}