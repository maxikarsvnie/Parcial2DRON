using Models;

namespace Interfaces
{
    public interface IServicioPersistencia
    {
        int GuardarCabecera(ControlMaestro cabecera);
        void GuardarDetalle(int idCabecera, List<Movimiento> movimientos);
    }
}
