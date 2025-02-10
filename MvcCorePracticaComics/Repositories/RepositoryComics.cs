using System.Collections.Generic;
using System.Data;
using System.Numerics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using MvcCorePracticaComics.Models;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MvcCorePracticaComics.Repositories
{
    #region
//    create procedure SP_INSERTAR_COMIC
//    (@nombre nvarchar(50), @imagen nvarchar(50), @descripcion nvarchar(50))
//as
//declare @nextId int
//select @nextId = MAX(IDCOMIC) + 1 from COMICS
//insert into COMICS values(@nextId, @nombre, @imagen, @descripcion)
//go
    #endregion
    public class RepositoryComics
    {
        private DataTable tablaComics;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryComics()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=PRACTICACOMICS;Persist Security Info=True;User ID=SA;Trust Server Certificate=True";
            this.tablaComics = new DataTable();
            string sql = "select * from COMICS";
            SqlDataAdapter ad = new SqlDataAdapter(sql, connectionString);
            ad.Fill(this.tablaComics);
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;
            List<Comic> comics = new List<Comic>();
            foreach (var row in consulta)
            {
                Comic c = new Comic
                {
                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION"),
                };
                comics.Add(c);
            }
            return comics;
        }

        public async Task InsertComicAsync(string nombre, string imagen, string descripcion)
        {
            int maxId = this.tablaComics.AsEnumerable().Max(row => row.Field<int>("IDCOMIC")) + 1;
            string sql = "INSERT INTO COMICS (IDCOMIC, NOMBRE, IMAGEN, DESCRIPCION) VALUES (@id, @nombre, @imagen, @descripcion)";
            this.com.Parameters.AddWithValue("@id", maxId);
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@imagen", imagen);
            this.com.Parameters.AddWithValue("@descripcion", descripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public List<string> GetNombresComics()
        {
            var consulta = (from datos in this.tablaComics.AsEnumerable()
                            select datos.Field<string>("NOMBRE")).Distinct();
            return consulta.ToList();
        }

        public Comic GetComicsNombre(string nombre)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           where datos.Field<string>("NOMBRE") == nombre
                           select datos;

            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                var row = consulta.First();
                Comic c = new Comic
                {
                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION")

                };
                return c;
            }
        }
    }
}
