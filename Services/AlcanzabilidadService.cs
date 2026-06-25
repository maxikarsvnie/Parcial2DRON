using Interfaces;

namespace Services
{
    public class AlcanzabilidadService
    {
        private readonly IMovimientoDron _movimientoDron;
        private bool[,] _visitado;
        private int _n;

        public AlcanzabilidadService(IMovimientoDron movimientoDron)
        { 
            _movimientoDron = movimientoDron;
            _visitado = new bool[1, 1];
        }

        public int Calcular(int n, int xInicial, int yInicial)
        {
            _n = n;
            _visitado = new bool[n, n];
            return DFS(xInicial, yInicial);
        }

        private int DFS(int x, int y)
        {
            if (_visitado[x, y]) return 0;
            _visitado[x, y] = true;
            int count = 1;

            foreach (var (nx, ny) in _movimientoDron.ObtenerSaltos(x, y, _n))
            {
                count += DFS(nx, ny);
            }

            return count;
        }
    }
}