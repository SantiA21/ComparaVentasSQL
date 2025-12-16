using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ComparaVentasExcel
{
    public partial class FormVerSucursalesV2 : Form
    {
        private readonly string _connectionString;

        public FormVerSucursalesV2(string connectionString)
        {
            InitializeComponent();
            _connectionString = connectionString;
        }

        private void FormVerSucursalesV2_Load(object sender, EventArgs e)
        {
            CargarSucursales();
        }

        private void CargarSucursales()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    // 🔥 X QUERY (ejemplo)
                    string query = @"
use backoffice
declare @infoCaja table([caja] varchar(20), [equipo] varchar(259), [version] varchar(200) );

insert into @infoCaja
select distinct caja,EQUIPO,valor from (
select RANK() OVER (
    PARTITION BY caja, parametro
    ORDER BY fechatrans desc) rango, *
from hparamloc
where parametro = 'VERSION') subQuery
where rango = 1
order by equipo

SELECT vene_caja As NumCaja, suc_codigo As Sucursal, equipo As Hostname
FROM (
    SELECT 
        ROW_NUMBER() OVER (PARTITION BY v.vene_caja ORDER BY v.vene_fecha DESC) AS rn,
        v.vene_caja,
        v.suc_codigo,
        i.equipo
    FROM VENTAS_E v
    INNER JOIN @infoCaja i ON v.vene_caja = i.caja COLLATE SQL_Latin1_General_CP1_CI_AS
    WHERE v.vene_caja != ''
) AS subquery
WHERE rn = 1
ORDER BY equipo;
";

                    DataTable dt = new DataTable();
                    using (var cmd = new SqlCommand(query, conn))
                    using (var da = new SqlDataAdapter(cmd))
                        da.Fill(dt);

                    dgvSucursales.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando sucursales: " + ex.Message);
            }
        }
    }
}
