using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemaProduccion.Presentacion.Paneles.Gestion_Inventario;

namespace SistemaProduccion.Presentacion.Paneles
{
    public partial class pnl_gestion_inventario : UserControl
    {
     
        private Modelo_P modP;
        public Especiales espe;

        public pnl_gestion_inventario()
        {
            InitializeComponent();
            this.cargarPanelModeloP();
        }

        private void btnModelo_P_Click(object sender, EventArgs e)
        {
            this.cargarPanelModeloP();
        }


        public void cargarPanelModeloP()
        {
            if (this.panelContainer.Controls.Count != 0)
            {
                this.panelContainer.Controls.RemoveAt(0);
            }
            this.modP = new Modelo_P();
            this.modP.Visible = true;
            this.panelContainer.Controls.Add(modP);
        }

        public void cargarPanelEspecial()
        {
            if (this.panelContainer.Controls.Count != 0)
            {
                this.panelContainer.Controls.RemoveAt(0);
            }
            this.espe = new Especiales();
            this.espe.Visible = true;
            this.panelContainer.Controls.Add(espe);
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            cargarPanelEspecial();
        }

    }
}
