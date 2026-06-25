using Interfaces;
using Models;

namespace Services
{
    public class SolucionadoRecursivo : ISolucionadorDron
    {
        private readonly IMovimientoDron _movimientoDron;
        private bool[,] _visitado;
        private int _n;
        private int _totalAlcanzables;

        public List<Movimiento> Movimientos { get; private set; }

        public SolucionadoRecursivo(IMovimientoDron movimientoDron)
        {
            _movimientoDron = movimientoDron;
            Movimientos = new List<Movimiento>();
            _visitado = new bool[1, 1];
        }

        public bool Resolver(int n, int xInicial, int yInicial)
        {
            _n = n;
            _visitado = new bool[n, n];
            Movimientos.Clear();

            _visitado[xInicial, yInicial] = true;
            Movimientos.Add(new Movimiento { paso = 0, X = xInicial, Y = yInicial });

            _totalAlcanzables = new AlcanzabilidadService(_movimientoDron).Calcular(n, xInicial, yInicial);

            return Backtrack(xInicial, yInicial, 1);
        }

        private bool Backtrack(int x, int y, int paso)
        {
            if (paso == _totalAlcanzables)
                return true;

            var candidatos = _movimientoDron.ObtenerSaltos(x, y, _n)
                .Where(c => !_visitado[c.x, c.y])
                .OrderBy(c => CalcularGrado(c.x, c.y))
                .ToList();

            foreach (var (nx, ny) in candidatos)
            {
                _visitado[nx, ny] = true;
                Movimientos.Add(new Movimiento { paso = paso, X = nx, Y = ny });

                if (Backtrack(nx, ny, paso + 1))
                    return true;

                _visitado[nx, ny] = false;
                Movimientos.RemoveAt(Movimientos.Count - 1);
            }

            return false;
        }

        private int CalcularGrado(int x, int y)
        {
            return _movimientoDron.ObtenerSaltos(x, y, _n)
                .Count(c => !_visitado[c.x, c.y]);
        }
    }
}