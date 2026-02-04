namespace ComparaVentasExcel
{
    partial class FormInicio
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInicio));
            menuStrip = new MenuStrip();
            ventasToolStripMenuItem = new ToolStripMenuItem();
            importarExcelToolStripMenuItem = new ToolStripMenuItem();
            consultarVentaToolStripMenuItem = new ToolStripMenuItem();
            ventasConCAEAToolStripMenuItem = new ToolStripMenuItem();
            modificarImporteToolStripMenuItem = new ToolStripMenuItem();
            sucursalesToolStripMenuItem = new ToolStripMenuItem();
            verSucursalesToolStripMenuItem = new ToolStripMenuItem();
            usuariosToolStripMenuItem = new ToolStripMenuItem();
            usuariosMOSTAZAERPToolStripMenuItem = new ToolStripMenuItem();
            usuariosGMGERPToolStripMenuItem = new ToolStripMenuItem();
            usuariosBackofficeToolStripMenuItem = new ToolStripMenuItem();
            ayudaToolStripMenuItem = new ToolStripMenuItem();
            novToolStripMenuItem = new ToolStripMenuItem();
            lblVersion = new Label();
            insertarSucursalToolStripMenuItem = new ToolStripMenuItem();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { ventasToolStripMenuItem, sucursalesToolStripMenuItem, usuariosToolStripMenuItem, ayudaToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(345, 24);
            menuStrip.TabIndex = 15;
            // 
            // ventasToolStripMenuItem
            // 
            ventasToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { importarExcelToolStripMenuItem, consultarVentaToolStripMenuItem, ventasConCAEAToolStripMenuItem, modificarImporteToolStripMenuItem });
            ventasToolStripMenuItem.Name = "ventasToolStripMenuItem";
            ventasToolStripMenuItem.Size = new Size(53, 20);
            ventasToolStripMenuItem.Text = "Ventas";
            // 
            // importarExcelToolStripMenuItem
            // 
            importarExcelToolStripMenuItem.Name = "importarExcelToolStripMenuItem";
            importarExcelToolStripMenuItem.Size = new Size(170, 22);
            importarExcelToolStripMenuItem.Text = "Importar excel";
            importarExcelToolStripMenuItem.Click += importarExcelToolStripMenuItem_Click;
            // 
            // consultarVentaToolStripMenuItem
            // 
            consultarVentaToolStripMenuItem.Name = "consultarVentaToolStripMenuItem";
            consultarVentaToolStripMenuItem.Size = new Size(170, 22);
            consultarVentaToolStripMenuItem.Text = "Consultar venta";
            consultarVentaToolStripMenuItem.Click += consultarVentaToolStripMenuItem_Click;
            // 
            // ventasConCAEAToolStripMenuItem
            // 
            ventasConCAEAToolStripMenuItem.Name = "ventasConCAEAToolStripMenuItem";
            ventasConCAEAToolStripMenuItem.Size = new Size(170, 22);
            ventasConCAEAToolStripMenuItem.Text = "Ventas con CAEA";
            ventasConCAEAToolStripMenuItem.Click += ventasConCAEAToolStripMenuItem_Click;
            // 
            // modificarImporteToolStripMenuItem
            // 
            modificarImporteToolStripMenuItem.Name = "modificarImporteToolStripMenuItem";
            modificarImporteToolStripMenuItem.Size = new Size(170, 22);
            modificarImporteToolStripMenuItem.Text = "Modificar importe";
            modificarImporteToolStripMenuItem.Click += modificarImporteToolStripMenuItem_Click;
            // 
            // sucursalesToolStripMenuItem
            // 
            sucursalesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { verSucursalesToolStripMenuItem, insertarSucursalToolStripMenuItem });
            sucursalesToolStripMenuItem.Name = "sucursalesToolStripMenuItem";
            sucursalesToolStripMenuItem.Size = new Size(74, 20);
            sucursalesToolStripMenuItem.Text = "Sucursales";
            // 
            // verSucursalesToolStripMenuItem
            // 
            verSucursalesToolStripMenuItem.Name = "verSucursalesToolStripMenuItem";
            verSucursalesToolStripMenuItem.Size = new Size(180, 22);
            verSucursalesToolStripMenuItem.Text = "Ver sucursales";
            verSucursalesToolStripMenuItem.Click += verSucursalesToolStripMenuItem_Click;
            // 
            // usuariosToolStripMenuItem
            // 
            usuariosToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { usuariosMOSTAZAERPToolStripMenuItem, usuariosGMGERPToolStripMenuItem, usuariosBackofficeToolStripMenuItem });
            usuariosToolStripMenuItem.Name = "usuariosToolStripMenuItem";
            usuariosToolStripMenuItem.Size = new Size(64, 20);
            usuariosToolStripMenuItem.Text = "Usuarios";
            // 
            // usuariosMOSTAZAERPToolStripMenuItem
            // 
            usuariosMOSTAZAERPToolStripMenuItem.Name = "usuariosMOSTAZAERPToolStripMenuItem";
            usuariosMOSTAZAERPToolStripMenuItem.Size = new Size(201, 22);
            usuariosMOSTAZAERPToolStripMenuItem.Text = "Usuarios MOSTAZA_ERP";
            usuariosMOSTAZAERPToolStripMenuItem.Click += usuariosMOSTAZAERPToolStripMenuItem_Click;
            // 
            // usuariosGMGERPToolStripMenuItem
            // 
            usuariosGMGERPToolStripMenuItem.Name = "usuariosGMGERPToolStripMenuItem";
            usuariosGMGERPToolStripMenuItem.Size = new Size(201, 22);
            usuariosGMGERPToolStripMenuItem.Text = "Usuarios GMG_ERP";
            usuariosGMGERPToolStripMenuItem.Click += usuariosGMGERPToolStripMenuItem_Click;
            // 
            // usuariosBackofficeToolStripMenuItem
            // 
            usuariosBackofficeToolStripMenuItem.Name = "usuariosBackofficeToolStripMenuItem";
            usuariosBackofficeToolStripMenuItem.Size = new Size(201, 22);
            usuariosBackofficeToolStripMenuItem.Text = "Usuarios backoffice";
            usuariosBackofficeToolStripMenuItem.Click += usuariosBackofficeToolStripMenuItem_Click;
            // 
            // ayudaToolStripMenuItem
            // 
            ayudaToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { novToolStripMenuItem });
            ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            ayudaToolStripMenuItem.Size = new Size(53, 20);
            ayudaToolStripMenuItem.Text = "Ayuda";
            // 
            // novToolStripMenuItem
            // 
            novToolStripMenuItem.Name = "novToolStripMenuItem";
            novToolStripMenuItem.Size = new Size(133, 22);
            novToolStripMenuItem.Text = "Novedades";
            novToolStripMenuItem.Click += novToolStripMenuItem_Click;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.FlatStyle = FlatStyle.Popup;
            lblVersion.Location = new Point(223, 281);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(38, 15);
            lblVersion.TabIndex = 14;
            lblVersion.Text = "label3";
            // 
            // insertarSucursalToolStripMenuItem
            // 
            insertarSucursalToolStripMenuItem.Name = "insertarSucursalToolStripMenuItem";
            insertarSucursalToolStripMenuItem.Size = new Size(180, 22);
            insertarSucursalToolStripMenuItem.Text = "Insertar sucursal FE";
            insertarSucursalToolStripMenuItem.Click += insertarSucursalToolStripMenuItem_Click;
            // 
            // FormInicio
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(345, 305);
            Controls.Add(lblVersion);
            Controls.Add(menuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            Name = "FormInicio";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cinet";
            Load += FormInicio_Load;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private Label lblVersion;
        private ToolStripMenuItem ventasToolStripMenuItem;
        private ToolStripMenuItem importarExcelToolStripMenuItem;
        private ToolStripMenuItem consultarVentaToolStripMenuItem;
        private ToolStripMenuItem ventasConCAEAToolStripMenuItem;
        private ToolStripMenuItem modificarImporteToolStripMenuItem;
        private ToolStripMenuItem sucursalesToolStripMenuItem;
        private ToolStripMenuItem verSucursalesToolStripMenuItem;
        private ToolStripMenuItem usuariosToolStripMenuItem;
        private ToolStripMenuItem usuariosMOSTAZAERPToolStripMenuItem;
        private ToolStripMenuItem usuariosGMGERPToolStripMenuItem;
        private ToolStripMenuItem ayudaToolStripMenuItem;
        private ToolStripMenuItem novToolStripMenuItem;
        private ToolStripMenuItem usuariosBackofficeToolStripMenuItem;
        private ToolStripMenuItem insertarSucursalToolStripMenuItem;
    }
}