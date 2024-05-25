using Dapper;
using Microsoft.SqlServer.Management.IntegrationServices;
using Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly MySqlConfiguration _connectionString;
        private readonly SqlConfiguration _sqlconnectionString;
      
        public CarRepository(MySqlConfiguration connectionString, SqlConfiguration sqlConnectionString)
        {
            _connectionString = connectionString;
            _sqlconnectionString = sqlConnectionString;

        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }


        protected SqlConnection sqlConnection()
        {
            return new SqlConnection(_sqlconnectionString.ConnectionString);
        }



       public async Task<IEnumerable<Car>> GetAllCars()
        {
            var db = sqlConnection();
            var sql = @"SELECT id, make , model, color, year, doors
                        FROM cars";
            return await db.QueryAsync<Car>(sql, new { });

        }

        public async Task<bool> DeleteCar(Car car)
        {
            var db = sqlConnection();
            var sql = @"DELETE FROM cars WHERE id = @Id";
            var result = await db.ExecuteAsync(sql, new { Id = car.Id });


            return result > 0;
        }


        public async Task<Car> GetDetails(int id)
        {
            var db = sqlConnection();
            var sql = @"SELECT id, make , model, color, year, doors
                        FROM cars
                        WHERE id = @Id";

            return await db.QueryFirstOrDefaultAsync<Car>(sql, new { Id = id });
        }

        public async Task<string> InsertCar(Car car)
        {
            var db = sqlConnection();

            string storedProcName = "sp_insert_car";

            var parameters = new
            {
                Make = car.Make,
                Model = car.Model,
                Color = car.Color,
                Year = car.Year,
                Doors = car.Doors

            };

            //esto va si devolviera un boolean, como un 1 de respuesta, considerar cambiar <string> por <bool> en el Task
            //var result = await db.ExecuteAsync(storedProcName, parameters, commandType: CommandType.StoredProcedure);

            var result = await db.ExecuteScalarAsync<string>(storedProcName, parameters, commandType: CommandType.StoredProcedure);

            // ACA SE REALIZA UN INSERT MANUAL, SIN STORED PROCEDURE 
            //var sql = @"INSERT INTO cars(make , model, color, year, doors)
            //            VALUES (@Make , @Model, @Color, @Year, @Doors)";

            //var result = await db.ExecuteAsync(sql, new
            //{ car.Make, car.Model, car.Color, car.Year, car.Doors });


            try
            {
                return result;
                //return result > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al insertar el registro: " + ex.Message);
                return null; // O manejar el error según tus necesidades
            }
        }

       public async Task<bool> UpdateCar(Car car)
        {
            var db = sqlConnection();
            var sql = @"UPDATE cars
                         SET make = @Make,
                           model = @Model,
                           color = @Color,
                           year = @Year,
                           doors = @Doors
                         WHERE id = @Id";

            var result = await db.ExecuteAsync(sql, new
            { car.Make, car.Model, car.Color, car.Year, car.Doors, car.Id });

            return result > 0;
        }
    }
}
