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

namespace presentacion
{
    public partial class frmAltaArticulo: Form
    {
        private Articulo articulo = null;
        public frmAltaArticulo()
        {
            InitializeComponent();
        }

        public frmAltaArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar artículo";
        }

        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            try
            {
                cargarCbs();
                pbImagen.Load("https://developers.elementor.com/docs/assets/img/elementor-placeholder-image.png");

                if(articulo != null)
                {
                txtCodigoArticulo.Text = articulo.CodigoArticulo;
                txtNombre.Text = articulo.Nombre;
                txtDescripcion.Text = articulo.Descripcion;
                cbMarca.SelectedValue = articulo.Marca.Id;
                cbCategoria.SelectedValue = articulo.Categoria.Id;
                txtUrlImagen.Text = articulo.UrlImagen;
                txtPrecio.Text = articulo.Precio.ToString();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void cargarCbs()
        {

            MarcaNegocio negocioMarca = new MarcaNegocio();
            List<Marca> listaMarcas = negocioMarca.listar();
            cbMarca.ValueMember = "Id";
            cbMarca.DisplayMember = "Descripcion";
            cbMarca.DataSource = listaMarcas;

            CategoriaNegocio negocioCategoria = new CategoriaNegocio();
            List<Categoria> listaCategorias = negocioCategoria.listar();
            cbCategoria.ValueMember = "Id";
            cbCategoria.DisplayMember = "Descripcion";
            cbCategoria.DataSource = listaCategorias;

        }
        private void btnAceptar_Click(object sender, EventArgs e)
        {            
            ArticuloNegocio negocio = new ArticuloNegocio();
            
            try
            {
                if (articulo == null)
                    articulo = new Articulo();

                articulo.CodigoArticulo = txtCodigoArticulo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Marca = (Marca)cbMarca.SelectedItem;
                articulo.Categoria = (Categoria)cbCategoria.SelectedItem;
                articulo.UrlImagen = txtUrlImagen.Text;
                articulo.Precio = decimal.Parse(txtPrecio.Text);

                if(articulo.Id != 0) 
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("El artículo fue modificado exitosamente");
                }
                else
                {
                    negocio.agregar(articulo);
                    MessageBox.Show("El artículo fue agregado exitosamente");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Close();
            }

        }

        private void txtCodigoArticulo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back))
                e.Handled = true;           
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }

        private void cargarImagen(string url)
        {
            try
            {
                pbImagen.Load(url);
            }
            catch (Exception)
            {
                pbImagen.Load("https://developers.elementor.com/docs/assets/img/elementor-placeholder-image.png");
            }
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || e.KeyChar == ',' || e.KeyChar == (char)Keys.Back))
                e.Handled = true;
            if (e.KeyChar == ',' && txtPrecio.Text.Contains(','))
                e.Handled = true;
        }
    }
}
