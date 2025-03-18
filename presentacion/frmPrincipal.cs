using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using negocio;
using dominio;
using System.Security.Policy;

namespace presentacion
{
    public partial class frmPrincipal: Form
    {
        private List<Articulo> listaArticulos;
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            cargarDatos();            
            cargarCboxes();
        }

        private void cargarDatos()
        {
            inicializarLista();
            cargarDataGridView(listaArticulos);
        }

        private void inicializarLista()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            listaArticulos = negocio.listar();
        }

        private void cargarDataGridView(List<Articulo> lista)
        {
            try
            {                
                dgvArticulos.DataSource = lista;
                ocultarColumnas();
                if (listaArticulos.Count > 0)    //Chequea que la lista no esté vacía si se filtró antes
                {
                    cargarImagen(listaArticulos[0].UrlImagen);
                    lblDescripcion.Text = (listaArticulos[0].Descripcion);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo cargar los datos. Intente nuevamente más tarde");
            }
            
        }

        private void ocultarColumnas()
        {
            dgvArticulos.Columns["Id"].Visible = false;
            dgvArticulos.Columns["UrlImagen"].Visible = false;
        }

        private void cargarCboxes()
        {
            HashSet<String> listaMarcas = new HashSet<String>();
            foreach (Articulo articulo in listaArticulos)
            {
                if (articulo.Marca.Descripcion != "")
                {
                    listaMarcas.Add(articulo.Marca.Descripcion);
                }
            }

            cbMarca.Items.Add("Todas");
            cbMarca.SelectedIndex = 0;

            foreach (String marca in listaMarcas)
            {
                cbMarca.Items.Add(marca);
            }

            HashSet<String> listaCategorias = new HashSet<String>();
            foreach (Articulo articulo in listaArticulos)
            {
                if (articulo.Categoria.Descripcion != "")
                {
                    listaCategorias.Add(articulo.Categoria.Descripcion);
                }
            }

            cbCategoria.Items.Add("Todas");
            cbCategoria.SelectedIndex = 0;

            foreach (String categoria in listaCategorias)
            {
                cbCategoria.Items.Add(categoria);
            }

        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                cargarImagen(((Articulo)dgvArticulos.CurrentRow.DataBoundItem).UrlImagen);
                cargarDescripcion(((Articulo)dgvArticulos.CurrentRow.DataBoundItem).Descripcion);
            }
            else
            {
                cargarImagen("https://developers.elementor.com/docs/assets/img/elementor-placeholder-image.png");
                cargarDescripcion("-");
            }
                
        }

        public void cargarImagen(string url)
        {
            try
            {
                pbArticulo.Load(url);
            }
            catch (Exception)
            {
                pbArticulo.Load("https://developers.elementor.com/docs/assets/img/elementor-placeholder-image.png");
            }
        }

        public void cargarDescripcion(string descripcion)
        {
            lblDescripcion.Text = descripcion;
        }

        private void txtBuscador_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtBuscador.Text;

            if (filtro.Length >= 2)
            {
                listaFiltrada = listaArticulos.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.CodigoArticulo.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaArticulos;
            }

            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void txtPrecioMinimo_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Validamos que solamente se puedan ingresar números y una coma
            if (!(Char.IsNumber(e.KeyChar) || e.KeyChar == ',' || e.KeyChar == (char)Keys.Back))
                e.Handled = true;
            if (e.KeyChar == ',' && txtPrecioMinimo.Text.Contains(','))
                e.Handled = true;
          
        }

        private void txtPrecioMaximo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || e.KeyChar == ',' || e.KeyChar == (char)Keys.Back))
                e.Handled = true;
            if (e.KeyChar == ',' && txtPrecioMaximo.Text.Contains(','))
                e.Handled = true;
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            try
            {
                ArticuloNegocio negocio = new ArticuloNegocio();
                //List<Articulo> listaFiltrada = negocio.listarConFiltro(cbMarca.Text,cbCategoria.Text, txtPrecioMinimo.Text, txtPrecioMaximo.Text);
                //cargarDataGridView(listaFiltrada);    Viejo, sin modificar la lista original
                listaArticulos = negocio.listarConFiltro(cbMarca.Text,cbCategoria.Text, txtPrecioMinimo.Text, txtPrecioMaximo.Text);
                cargarDataGridView(listaArticulos);
            }
            catch (Exception)
            {
                throw;
            }


            txtBuscador.Text = "";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArticulo ventanaAlta = new frmAltaArticulo();
            ventanaAlta.ShowDialog();

            cargarDatos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        private void eliminar()
        {

            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                DialogResult respuesta = MessageBox.Show("Al eliminar el registro no se podrá recuperar. ¿Está seguro que desea continuar con la eliminación?", "Eliminar artículo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    negocio.eliminar((Articulo)dgvArticulos.CurrentRow.DataBoundItem);                  
                    cargarDatos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                frmAltaArticulo ventanaModificar = new frmAltaArticulo((Articulo)dgvArticulos.CurrentRow.DataBoundItem);
                ventanaModificar.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Debe seleccionar un artículo para modificar");
            }
            finally
            {
                reiniciar();
            }
        }

        private void btnReiniciar_Click(object sender, EventArgs e)
        {
            reiniciar();
        }

        private void reiniciar()
        {
            cargarDatos();
            cbMarca.SelectedIndex = 0;
            cbCategoria.SelectedIndex = 0;
            txtPrecioMinimo.Text = "";
            txtPrecioMaximo.Text = "";
            txtBuscador.Text = "";
        }
    }
}
