// Repository/ServicioPersistencia.cs
using Interfaces;
using Models;
using Npgsql;


namespace Repository
{
    public class ServicioPersistencia : IServicioPersistencia
    {
        private readonly string _connectionString;

        public ServicioPersistencia(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int GuardarCabecera(ControlMaestro cabecera)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "INSERT INTO tb_master_control (fecha_hora, n, x_inicial, y_inicial) VALUES (@fecha, @n, @x, @y) RETURNING id",
                conn);

            cmd.Parameters.AddWithValue("fecha", cabecera.FechaHora);
            cmd.Parameters.AddWithValue("n", cabecera.N);
            cmd.Parameters.AddWithValue("x", cabecera.XInicial);
            cmd.Parameters.AddWithValue("y", cabecera.YInicial);

            return (int)(cmd.ExecuteScalar() ?? 0);
        }

        public void GuardarDetalle(int idCabecera, List<Movimiento> movimientos)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            using var tx = conn.BeginTransaction();

            int i = 0;
            while (i < movimientos.Count)
            {
                var mov = movimientos[i];
                int pasoOfuscado = mov.paso % 2 == 0 ? mov.paso * 2 : -mov.paso;

                using var cmd = new NpgsqlCommand(
                    "INSERT INTO tb_det_log (id_control_maestro, paso, x, y) VALUES (@id, @paso, @x, @y)",
                    conn, tx);

                cmd.Parameters.AddWithValue("id", idCabecera);
                cmd.Parameters.AddWithValue("paso", pasoOfuscado);
                cmd.Parameters.AddWithValue("x", mov.X);
                cmd.Parameters.AddWithValue("y", mov.Y);

                cmd.ExecuteNonQuery();
                i++;
            }

            tx.Commit();
        }
    }
}