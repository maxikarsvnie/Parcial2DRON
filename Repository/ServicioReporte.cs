using Interfaces;
using Npgsql;

namespace Repository
{
    public class ServicioReporte : IServicioReporte
    {
        private readonly string _connectionString;

        public ServicioReporte(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void MostrarUltimos5(int idCabecera)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "SELECT id, paso, x, y FROM tb_det_log WHERE id_control_maestro = @id ORDER BY id DESC LIMIT 5",
                conn);

            cmd.Parameters.AddWithValue("id", idCabecera);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int idRegistro = reader.GetInt32(0);   // id
                int pasoGuardado = reader.GetInt32(1); // paso
                int x = reader.GetInt32(2);            // x
                int y = reader.GetInt32(3);            // y

                // reconstruir paso real
                int pasoReal = pasoGuardado < 0 ? -pasoGuardado : pasoGuardado / 2;

                Console.WriteLine($"{idRegistro.ToString().PadRight(12)} | {pasoReal.ToString().PadRight(9)} | ({x}, {y})");
            }
        }
    }
}