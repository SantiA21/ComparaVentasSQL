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
            menuStrip1 = new MenuStrip();
            ventasToolStripMenuItem = new ToolStripMenuItem();
            importarExcelToolStripMenuItem = new ToolStripMenuItem();
            consultarVentaToolStripMenuItem = new ToolStripMenuItem();
            ventasConCAEAToolStripMenuItem = new ToolStripMenuItem();
            modificarImporteToolStripMenuItem = new ToolStripMenuItem();
            sucursalesToolStripMenuItem = new ToolStripMenuItem();
            verSucursalesToolStripMenuItem = new ToolStripMenuItem();
            ayudaToolStripMenuItem = new ToolStripMenuItem();
            novedadesToolStripMenuItem = new ToolStripMenuItem();
            lblVersion = new Label();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { ventasToolStripMenuItem, sucursalesToolStripMenuItem, ayudaToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(302, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
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
            modificarImporteToolStripMenuItem.Text = "Modificar Importe";
            modificarImporteToolStripMenuItem.Click += modificarImporteToolStripMenuItem_Click;
            // 
            // sucursalesToolStripMenuItem
            // 
            sucursalesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { verSucursalesToolStripMenuItem });
            sucursalesToolStripMenuItem.Name = "sucursalesToolStripMenuItem";
            sucursalesToolStripMenuItem.Size = new Size(74, 20);
            sucursalesToolStripMenuItem.Text = "Sucursales";
            // 
            // verSucursalesToolStripMenuItem
            // 
            verSucursalesToolStripMenuItem.Name = "verSucursalesToolStripMenuItem";
            verSucursalesToolStripMenuItem.Size = new Size(147, 22);
            verSucursalesToolStripMenuItem.Text = "Ver sucursales";
            verSucursalesToolStripMenuItem.Click += verSucursalesToolStripMenuItem_Click;
            // 
            // ayudaToolStripMenuItem
            // 
            ayudaToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { novedadesToolStripMenuItem });
            ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            ayudaToolStripMenuItem.Size = new Size(53, 20);
            ayudaToolStripMenuItem.Text = "Ayuda";
            // 
            // novedadesToolStripMenuItem
            // 
            novedadesToolStripMenuItem.Name = "novedadesToolStripMenuItem";
            novedadesToolStripMenuItem.Size = new Size(180, 22);
            novedadesToolStripMenuItem.Text = "Novedades";
            novedadesToolStripMenuItem.Click += novedadesToolStripMenuItem_Click;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.FlatStyle = FlatStyle.Popup;
            lblVersion.Location = new Point(215, 281);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(38, 15);
            lblVersion.TabIndex = 14;
            lblVersion.Text = "label3";
            // 
            // FormInicio
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(302, 305);
            Controls.Add(lblVersion);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "FormInicio";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ComparaVentas";
            Load += FormInicio_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem ventasToolStripMenuItem;
        private ToolStripMenuItem importarExcelToolStripMenuItem;
        private ToolStripMenuItem consultarVentaToolStripMenuItem;
        private Label lblVersion;
        private ToolStripMenuItem sucursalesToolStripMenuItem;
        private ToolStripMenuItem verSucursalesToolStripMenuItem;
        private ToolStripMenuItem ventasConCAEAToolStripMenuItem;
        private ToolStripMenuItem modificarImporteToolStripMenuItem;
        private ToolStripMenuItem ayudaToolStripMenuItem;
        private ToolStripMenuItem novedadesToolStripMenuItem;
    }
}