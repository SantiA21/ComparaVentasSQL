using ComparaVentasExcel.Services.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComparaVentasExcel.Forms.Usuarios
{
    public partial class FormResultadosBackOffice : Form
    {
        private DataTable dtUsuarios;
        public FormResultadosBackOffice(DataTable datos)
        {
            InitializeComponent();
            dgvResultados.DataSource = datos;
            if (datos == null)
                throw new ArgumentNullException(nameof(datos));

            dtUsuarios = datos;

            dgvResultados.DataSource = dtUsuarios;
            dgvResultados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResultados.ReadOnly = true;
            dgvResultados.AllowUserToAddRows = false;
        }

        private void CargarCombosFiltros()
        {
            if (dtUsuarios == null || dtUsuarios.Rows.Count == 0)
                return;

            // Categoría
            var categorias = dtUsuarios.AsEnumerable()
                .Select(r => r["Categoria"]?.ToString())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct()
                .OrderBy(s => s)
                .ToArray();

            cbCategoria.Items.Clear();
            cbCategoria.Items.Add("Todas");
            cbCategoria.Items.AddRange(categorias);
            cbCategoria.SelectedIndex = 0;

            // Estado
            var estados = dtUsuarios.AsEnumerable()
                .Select(r => r["Estado"]?.ToString())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct()
                .OrderBy(s => s)
                .ToArray();

            cbEstado.Items.Clear();
            cbEstado.Items.Add("Todos");
            cbEstado.Items.AddRange(estados);
            cbEstado.SelectedIndex = 0;
        }

        private void AplicarFiltros()
        {
            if (dtUsuarios == null) return;

            string filtro = "";

            if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                string texto = txtBuscar.Text.Replace("'", "''");

                filtro +=
                    $"(Convert(DNI, 'System.String') LIKE '%{texto}%' " +
                    $"OR Nombre LIKE '%{texto}%' " +
                    $"OR Apellido LIKE '%{texto}%' " +
                    $"OR NombreCinet LIKE '%{texto}%')";
            }

            if (cbCategoria.SelectedItem != null && cbCategoria.Text != "Todas")
            {
                if (filtro != "") filtro += " AND ";
                filtro += $"Categoria = '{cbCategoria.Text.Replace("'", "''")}'";
            }

            if (cbEstado.SelectedItem != null && cbEstado.Text != "Todos")
            {
                if (filtro != "") filtro += " AND ";
                filtro += $"Estado = '{cbEstado.Text.Replace("'", "''")}'";
            }

            DataView dv = dtUsuarios.DefaultView;
            dv.RowFilter = filtro;
            dgvResultados.DataSource = dv;
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void cbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void cbEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormResultadosBackOffice_Load(object sender, EventArgs e)
        {
            CargarCombosFiltros();
            AplicarFiltros();
        }
    }
}
