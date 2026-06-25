namespace Interfaces
{
    public interface IMovimientoDron
    {
        List<(int x, int y)> ObtenerSaltos(int x, int y, int n);
    }
}