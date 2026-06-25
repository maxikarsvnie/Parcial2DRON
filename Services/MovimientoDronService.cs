using Interfaces;

namespace Services
{
    public class MovimientoDronService : IMovimientoDron
    {
        public List<(int x, int y)> ObtenerSaltos(int x, int y, int n)
        {
            var saltos = new List<(int, int)>
            {
                (x - 2, y - 1), (x - 2, y + 1),
                (x + 2, y - 1), (x + 2, y + 1),
                (x - 1, y - 2), (x - 1, y + 2),
                (x + 1, y - 2), (x + 1, y + 2)
            };

            return saltos.Where(s => s.Item1 >= 0 && s.Item1 < n && s.Item2 >= 0 && s.Item2 < n).ToList();
        }
    }
}