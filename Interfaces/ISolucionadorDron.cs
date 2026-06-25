using Models;

namespace Interfaces
{
    public interface ISolucionadorDron
    {
        bool Resolver(int n, int xInicial, int yInicial);
        List<Movimiento> Movimientos { get; }
    }
}
