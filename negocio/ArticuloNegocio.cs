using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            datos.setearConsulta("select A.Id as Id, Codigo, Nombre, A.Descripcion, IdMarca,  M.Descripcion as Marca, IdCategoria, C.Descripcion as Categoria, ImagenUrl as UrlImagen, Precio from ARTICULOS A, CATEGORIAS C, MARCAS M  where A.IdMarca = M.Id and A.IdCategoria = C.Id");
            datos.ejecutarLectura();

            try
            {
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.CodigoArticulo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.UrlImagen = (string)datos.Lector["UrlImagen"];
                    //aux.Precio = (decimal)datos.Lector["Precio"];
                    aux.Precio = Math.Round((decimal)datos.Lector["Precio"], 2);
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

        }

        public List<Articulo> listarConFiltro(string marca, string categoria, string precioMin, string precioMax)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            string consulta = "select A.Id as Id, Codigo, Nombre, A.Descripcion, IdMarca,  M.Descripcion as Marca, IdCategoria, C.Descripcion as Categoria, ImagenUrl as UrlImagen, Precio from ARTICULOS A, CATEGORIAS C, MARCAS M  where A.IdMarca = M.Id and A.IdCategoria = C.Id";
            if (marca != "Todas")
                consulta = consulta + " and M.Descripcion = '" + marca + "'";
            if (categoria != "Todas")
                consulta = consulta + " and C.Descripcion = '" + categoria + "'";
            if (precioMin != "")             
                consulta = consulta + " and Precio >" + precioMin;            
            if(precioMax != "")
                consulta = consulta + " and Precio <" + precioMax;
            

            datos.setearConsulta(consulta);
            datos.ejecutarLectura();

            try
            {
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.CodigoArticulo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.UrlImagen = (string)datos.Lector["UrlImagen"];
                    aux.Precio = Math.Round((decimal)datos.Lector["Precio"], 2);
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
