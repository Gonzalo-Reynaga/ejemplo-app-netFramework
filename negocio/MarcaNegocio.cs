using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class MarcaNegocio
    {
        public List<Marca> listar()
        {
            List<Marca> lista = new List<Marca>();
            AccesoDatos datos = new AccesoDatos();
            datos.setearConsulta("select Id, Descripcion from MARCAS");
            datos.ejecutarLectura();

            try
            {
                while (datos.Lector.Read())
                {
                    Marca aux = new Marca();
                    
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    lista.Add(aux);
                }
            }
            catch (Exception)
            {

                throw;
            }
            
            return lista;
        }
    }
}
