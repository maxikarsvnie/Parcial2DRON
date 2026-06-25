using Interfaces;
using Models;
using Services;
using Repository;

class Program
{
    static void Main(string[] args)
    {
        // 1. Configuración: leer cadena de conexión desde appsettings.json
        IServicioConfiguracion configService = new ConfiguracionService();
        string connectionString = configService.ObtenerCadenaConexion();

        // 2. Solicitar datos al usuario
        Console.Write("Ingrese el tamaño del terreno (N x N): ");
        int n = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("Ingrese la coordenada inicial X (Fila): ");
        int xInicial = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("Ingrese la coordenada inicial Y (Columna): ");
        int yInicial = int.Parse(Console.ReadLine() ?? "0");

        Console.WriteLine("\nProcesando simulación de ruta...\n");

        // Validación dinámica
        if (n < 1 || xInicial < 0 || xInicial >= n || yInicial < 0 || yInicial >= n)
        {
            Console.WriteLine("Error: N o coordenadas fuera de rango.");
            return;
        }

        // 3. Resolver recorrido con backtracking
        IMovimientoDron movimientoService = new MovimientoDronService();
        ISolucionadorDron solver = new SolucionadoRecursivo(movimientoService);

        bool exito = solver.Resolver(n, xInicial, yInicial);

        if (!exito)
        {
            Console.WriteLine("Sin solución: no existe ruta que cubra todas las parcelas alcanzables.");
            return;
        }

        // 4. Mostrar matriz en consola
        Console.WriteLine("MAPA RESULTANTE DE LA EXPLORACIÓN:");
        string[,] matriz = new string[n, n];
        foreach (var mov in solver.Movimientos)
        {
            matriz[mov.X, mov.Y] = mov.paso.ToString();
        }

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write((matriz[i, j] ?? ".").PadLeft(4));
            }
            Console.WriteLine();
        }

        Console.WriteLine("\n¡ÉXITO! El dron cubrió de manera óptima las parcelas alcanzables.");

        // 5. Guardar en PostgreSQL
        IServicioPersistencia persistencia = new ServicioPersistencia(connectionString);
        var cabecera = new ControlMaestro
        {
            FechaHora = DateTime.Now,
            N = n,
            XInicial = xInicial,
            YInicial = yInicial
        };

        int idCabecera = persistencia.GuardarCabecera(cabecera);
        persistencia.GuardarDetalle(idCabecera, solver.Movimientos);

        Console.WriteLine($"Simulación guardada en la base de datos con el ID Master Control: {idCabecera}\n");

        // 6. Reporte inverso: últimos 5 pasos reconstruidos
        Console.WriteLine("REPORTE INVERSO: ÚLTIMOS 5 PASOS RECONSTRUIDOS");
        Console.WriteLine("------------------------------------------------");
        Console.WriteLine("ID Registro | Paso Real | Coordenadas (X, Y)");

        IServicioReporte reporte = new ServicioReporte(connectionString);
        reporte.MostrarUltimos5(idCabecera);
    }
}
