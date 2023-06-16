using SistemaProduccion.Presentacion.Paneles;
using SistemaProduccion.Presentacion.Paneles.Analisis_Inventario;
using SistemaProduccion.Presentacion.Paneles.Gestion_Inventario;
using SistemaProduccion.Presentacion.Paneles.Planeacion_agregada;
using SistemaProduccion.Presentacion.Paneles.TmpPanelAlex;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemaProduccion.Presentacion.Paneles.Inicio;


namespace SistemaProduccion.Presentacion
{
    public partial class Main : Form
    {
        private pnl_gestion_inventario pgi;
        private pnl_analisis_de_inventario pai;
        private Pnl_planeacion_agregada ppa;
        private pnlGestionOperaciones pnlOperaciones;
        frmInicio ini = new frmInicio();


        public Main()
        {
            InitializeComponent();
            // demostramos y mostramos primero la bienvenida
            this.Hide();
            Bienvenida n = new Bienvenida();
            n.ShowDialog();
            ///////////////////////////////////
            pnlOperaciones = new pnlGestionOperaciones();
            abrir(new frmInicio());

            pgi = new pnl_gestion_inventario();
            pgi.Visible = true;
            panelContenedor.Controls.Add(pgi);

            /*int height = Screen.PrimaryScreen.WorkingArea.Height;
            int width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Size = new Size(width, height);
            this.Location = new Point(0, 0);*/
        }

        private void btnCerrar_MouseMove(object sender, MouseEventArgs e)
        {
            this.btnCerrar.BackColor = Color.FromArgb(76, 109, 139);
        }

        private void btnCerrar_MouseLeave(object sender, EventArgs e)
        {
            this.btnCerrar.BackColor = Color.Transparent;
        }

        private void btnGestionInventario_Click(object sender, EventArgs e)
        {

            
            if (panelContenedor.Controls.Count != 0)
            {
                panelContenedor.Controls.RemoveAt(0);
            }
            pgi = new pnl_gestion_inventario();
            pgi.Visible = true;
            panelContenedor.Controls.Add(pgi);
            this.labelInfoMain.Text = "Sistema de producción - Gestión de inventario";
         


        }

      

        private void btnGEstionOperaciones_Click(object sender, EventArgs e)
        {


        }

        private void btnAnalisisInventario_Click(object sender, EventArgs e)
        {
           
            
        }

        private void btnPlaneacionAgregada_Click(object sender, EventArgs e)
        {
            
           
        }

        private void abrir (object Inicioxd)
        {
            if (this.panelContenedor.Controls.Count > 0)
                this.panelContenedor.Controls.RemoveAt(0);
            frmInicio ini = Inicioxd as frmInicio;
            ini.TopLevel = false;
            ini.Dock = DockStyle.Fill;
            this.panelContenedor.Controls.Add(ini);
            this.panelContenedor.Tag = ini;
            ini.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            abrir(new frmInicio());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (panelContenedor.Controls.Count != 0)
            {
                panelContenedor.Controls.RemoveAt(0);
            }
            pgi = new pnl_gestion_inventario();
            pgi.Visible = true;
            panelContenedor.Controls.Add(pgi);

            this.labelInfoMain.Text = "Sistema de producción - Gestión de inventario";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pnlOperaciones.Size = this.panelContenedor.Size;
            if (panelContenedor.Controls.Count != 0)
            {
                panelContenedor.Controls.RemoveAt(0);
            }
            panelContenedor.Controls.Add(pnlOperaciones);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (panelContenedor.Controls.Count != 0)
            {
                panelContenedor.Controls.RemoveAt(0);
            }
            pai = new pnl_analisis_de_inventario();
            pai.Visible = true;
            panelContenedor.Controls.Add(pai);
            this.labelInfoMain.Text = "Sistema de producción - Análisis de inventario";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (panelContenedor.Controls.Count != 0)
            {
                panelContenedor.Controls.RemoveAt(0);
            }
            ppa = new Pnl_planeacion_agregada();
            ppa.Visible = true;
            panelContenedor.Controls.Add(ppa);
            this.labelInfoMain.Text = "Sistema de producción - Planeación agregada";
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
