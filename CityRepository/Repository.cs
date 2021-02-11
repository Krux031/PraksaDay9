using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Data;
using CityRepositoryCommon;
using CityModelCommon;
using CityModel;

namespace CityRepository
{
    public class Repository : IRepository
    {
        SqlCommand command = null;
        SqlTransaction transaction;
        public static SqlConnection connection = new SqlConnection(@"Server=tcp:kruninserver.database.windows.net,1433;Initial Catalog=kruninabaza;Persist Security Info=False;User ID=krux031;Password=sifra;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        public async Task<ICity> GetCityRep(int id)
        {
            command = new SqlCommand("select * from grad where pbr=@id", connection);
            command.Parameters.AddWithValue("@id", id);
            ICity grad = new City();
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                SqlDataReader reader = command.ExecuteReader();

                while (await reader.ReadAsync())
                {
                    grad.Id = reader.GetInt32(0);
                    grad.Name = reader.GetString(1);
                }

            }
            catch (SqlException Ex)
            {
                transaction.Rollback();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return grad;
        }


        public async Task<List<ICity>> GetAllCityRep(int size, int page, string filter, string sort)
        {
            command = new SqlCommand("select * from grad where naziv = '" + filter + "' order by naziv " + sort + " OFFSET " + (page-1)*size + " ROWS FETCH NEXT " + size + " ROWS ONLY", connection);
            List <ICity> gradovi = new List<ICity>();
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                SqlDataReader reader = command.ExecuteReader();

                while (await reader.ReadAsync())
                {
                    ICity grad = new City();
                    grad.Id = reader.GetInt32(0);
                    grad.Name = reader.GetString(1);
                    gradovi.Add(grad);
                }

            }
            catch (SqlException Ex)
            {
                transaction.Rollback();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return gradovi;
        }

        public async Task<bool> DeleteresidentRep(int id)
        {
            command = new SqlCommand("delete from stanovnici where jmbg=@id", connection);
            command.Parameters.AddWithValue("@id", id);
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                if(await command.ExecuteNonQueryAsync()==0)
                {
                    return false;
                }
            }
            catch (SqlException Ex)
            {
                transaction.Rollback();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return true;
        }

        public async Task<bool> PostResidentRep(IResidents res, int id)
        {
            command = new SqlCommand("insert into stanovnici values (@id, @ime, @prezime, @pbr, @spol)", connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@ime", res.Name);
            command.Parameters.AddWithValue("@prezime", res.Surname);
            command.Parameters.AddWithValue("@pbr", res.Pbr);
            command.Parameters.AddWithValue("@spol", res.Gender);
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException Ex)
            {
                return false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            
            return true;
        }
    }
}
