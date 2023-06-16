using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaProduccion.Presentacion.Paneles.Gestion_Inventario
{
    public partial class Especiales : UserControl
    {
        public Especiales()
        {
            InitializeComponent();
            this.checkBox1.Enabled = false;
            this.checkBox2.Enabled = false;
            this.btnAgregar.Enabled = true;
            this.btnCalcular.Enabled = false;
            this.btnCancelar.Enabled = false;
            this.btnLimpiarCampos.Enabled = false;
            this.txtcostoProd.Enabled = false;
            this.txtDemandaEspe.Enabled = false;
            this.txtDesviacion.Enabled = false;
            this.txtpagoRema.Enabled = false;
            this.txtventaProd.Enabled = false;
            this.txtdemanda2.Enabled = false;
            this.txtCu.Enabled = false;
            this.txtCo.Enabled = false;
            this.txtdesviacion2.Enabled = false;
        }

        private void btnLimpiarCampos_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            this.btnCalcular.Enabled = true;
            this.btnCancelar.Enabled = true;
            this.btnLimpiarCampos.Enabled = true;
            this.btnAgregar.Enabled = false;
            this.checkBox1.Enabled = true;
            this.checkBox2.Enabled = true;
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                CuantoPedir();

            else if (checkBox2.Checked)
                CuantoPedir2();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.checkBox1.Checked = false;
            this.checkBox2.Checked = false;
            this.checkBox1.Enabled = false;
            this.checkBox2.Enabled = false;
            this.btnAgregar.Enabled = true;
            this.btnCalcular.Enabled = false;
            this.btnCancelar.Enabled = false;
            this.btnLimpiarCampos.Enabled = false;
            this.txtcostoProd.Enabled = false;
            this.txtDemandaEspe.Enabled = false;
            this.txtDesviacion.Enabled = false;
            this.txtpagoRema.Enabled = false;
            this.txtventaProd.Enabled = false;
            this.txtdemanda2.Enabled = false;
            this.txtCu.Enabled = false;
            this.txtCo.Enabled = false;
            this.txtdesviacion2.Enabled = false;

            limpiar();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            seleccion();
            limpiar();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            seleccion2();
            limpiar();
        }

        //METODOS
        private void limpiar()
        {
            this.txtcostoProd.Text = "";
            this.txtDemandaEspe.Text = "";
            this.txtpagoRema.Text = "";
            this.txtventaProd.Text = "";
            this.lblQespe.Text = "0.00";
            this.txtDesviacion.Text = "";
            this.txtdemanda2.Text = "";
            this.txtCu.Text = "";
            this.txtCo.Text = "";
            this.txtdesviacion2.Text = "";
        }

        public void CuantoPedir() 
        {
            double Cu;
            double Co;
            double div;
            double P;
            double venta;
            double compra;
            double remanente;
            double Q;
            double demanda;
            double desviacion;
            double z;
            double dato;

            try
            {
                venta = double.Parse(txtventaProd.Text);
                compra = double.Parse(txtcostoProd.Text);
                remanente = double.Parse(txtpagoRema.Text);

                Cu = venta - compra;
                Co = compra - remanente;
                div = Co + Cu;
                P = Cu/div;

                demanda = double.Parse(txtDemandaEspe.Text);
                desviacion = double.Parse(txtDesviacion.Text);
                z = MathNet.Numerics.Distributions.Normal.InvCDF(0.0, 1.0, P);
                dato = z * desviacion;
                Q = demanda + dato;
                lblQespe.Text = Q.ToString();
            }
            catch (Exception) { MessageBox.Show("Error"); }
        }

        public void CuantoPedir2() 
        {
            double Cu;
            double Co;
            double div;
            double P;
            double Q;
            double demanda;
            double desviacion;
            double z;
            double dato;

            try
            {
                Cu = double.Parse(txtCu.Text);
                Co = double.Parse(txtCo.Text);
                div = Co + Cu;
                P = Cu / div;

                demanda = double.Parse(txtdemanda2.Text);
                desviacion = double.Parse(txtdesviacion2.Text);
                z = MathNet.Numerics.Distributions.Normal.InvCDF(0.0, 1.0, P);
                dato = z * desviacion;
                Q = demanda + dato;
                lblQespe.Text = Q.ToString();
            }
            catch (Exception) { MessageBox.Show("Error"); }
        }

        private void seleccion() 
        {
            this.checkBox2.Checked = false;
            this.txtcostoProd.Enabled = true;
            this.txtDemandaEspe.Enabled = true;
            this.txtDesviacion.Enabled = true;
            this.txtpagoRema.Enabled = true;
            this.txtventaProd.Enabled = true;
            this.txtdemanda2.Enabled = false;
            this.txtCu.Enabled = false;
            this.txtCo.Enabled = false;
            this.txtdesviacion2.Enabled = false;
        }

        private void seleccion2()
        {
            this.checkBox1.Checked = false;
            this.txtcostoProd.Enabled = false;
            this.txtDemandaEspe.Enabled = false;
            this.txtDesviacion.Enabled = false;
            this.txtpagoRema.Enabled = false;
            this.txtventaProd.Enabled = false;
            this.txtdemanda2.Enabled = true;
            this.txtCu.Enabled = true;
            this.txtCo.Enabled = true;
            this.txtdesviacion2.Enabled = true;
        }
    }
}
