using System;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaProduccion.Presentacion.Paneles.Gestion_Inventario
{
    public partial class Modelo_P : UserControl
    {
        public Modelo_P()
        {
            InitializeComponent();
            this.DeshabilitarTexto();
        }

        public void Cuantopedir()
        {
            double Q, demanda, costo_pedir, costo_mantenimiento;
            try
            {
                demanda = double.Parse(txtDemanda.Text);
                costo_pedir = double.Parse(txtS.Text);
                costo_mantenimiento = double.Parse(txtH.Text);
                Q = Math.Sqrt((2 * demanda * costo_pedir) / costo_mantenimiento);

                labelQoptimo.Text = Q.ToString();
            } catch (Exception) { MessageBox.Show("No se mostrará Q ya que\nno se ingresaron los datos\nnecesarios "); }
        }

        public void Cuando_Pedir() {
            double ROP,dias,demanda=0, probabilidad, demanda_promedio, pEntrega, pRevision, sigma, Z, inventario;
            try
            {
                dias = double.Parse(txtDiasLaborales.Text);
                demanda_promedio = double.Parse(txtDemandaDiaria.Text);
                pEntrega = double.Parse(txtPlazo_de_entrega.Text);
                probabilidad = double.Parse(txtProbabilidad.Text);
                pRevision = double.Parse(txtPeriodoRevision.Text);
                sigma = double.Parse(txtDesviacion.Text);
                inventario = double.Parse(txtInventarioExistente.Text);
                probabilidad = probabilidad / 100;



                switch (comboPeriodo.SelectedIndex)
                {
                    case 0:
                        demanda = demanda_promedio;
                        break;

                    case 1:
                        demanda = demanda_promedio / dias;
                        break;

                    case 2:
                        demanda = demanda_promedio / dias;
                        break;

                    default:
                        break;
                }
              
                Z = MathNet.Numerics.Distributions.Normal.InvCDF(0.0, 1.0, probabilidad);
                sigma = Math.Sqrt((Math.Pow(sigma, 2)) * (pRevision + pEntrega));

                ROP = (demanda * (pRevision + pEntrega)) + ((Z * sigma) - inventario);
                labelResultadoROP.Text = ROP.ToString();

            }
            catch (Exception) { MessageBox.Show("No se mostrará ROP ya que\nno se ingresaron los datos\nnecesarios"); }
        
        
        
        }
        

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            Cuantopedir();
            Cuando_Pedir();
           
        }
        
        private void btnLimpiarCampos_Click(object sender, EventArgs e)
        {
            this.limpiarCampos();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DeshabilitarTexto();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            this.HabilitarTexto();
        }

        private void HabilitarTexto()
        {
            this.txtDemandaDiaria.Enabled = true;
            this.txtProbabilidad.Enabled = true;
            this.txtPeriodoRevision.Enabled = true;
            this.txtPlazo_de_entrega.Enabled = true;
            this.txtDesviacion.Enabled = true;
            this.txtInventarioExistente.Enabled = true;
            this.txtDemanda.Enabled = true;
            this.txtS.Enabled = true;
            this.txtH.Enabled = true;
            this.comboPeriodo.Enabled = true;
            this.txtDiasLaborales.Enabled = true;

            
          
            this.btnCalcular.Enabled = true;
            this.btnLimpiarCampos.Enabled = true;
            this.btnCancelar.Enabled = true;
            this.btnAgregar.Enabled = false;
        }

        private void DeshabilitarTexto()
        {
            this.txtDemandaDiaria.Enabled = false;
            this.txtProbabilidad.Enabled = false;
            this.txtPeriodoRevision.Enabled = false;
            this.txtPlazo_de_entrega.Enabled = false;
            this.txtDesviacion.Enabled = false;
            this.txtInventarioExistente.Enabled = false;
            this.txtDemanda.Enabled = false;
            this.txtS.Enabled = false;
            this.txtH.Enabled = false;
            this.comboPeriodo.Enabled = false;
            this.txtDiasLaborales.Enabled = false;


            this.btnCalcular.Enabled = false;
            this.btnLimpiarCampos.Enabled = false;
            this.btnCancelar.Enabled = false;
            this.btnAgregar.Enabled = true;
            this.limpiarCampos();

        }

        private void limpiarCampos()
        {
            this.txtDemandaDiaria.Text = string.Empty;
            this.txtProbabilidad.Text = string.Empty;
            this.txtPeriodoRevision.Text = string.Empty;
            this.txtPlazo_de_entrega.Text = string.Empty;
            this.txtDesviacion.Text = string.Empty;
            this.txtInventarioExistente.Text = string.Empty;
            this.txtDemanda.Text="";
            this.txtS.Text="";
            this.txtH.Text = "";
            this.txtDiasLaborales.Text = "";
            this.txtPlazo_de_entrega.Text = string.Empty;
            this.comboPeriodo.SelectedIndex = 0;
            this.labelQoptimo.Text = "0,00";
            this.labelResultadoROP.Text = "0,00";
          
        }


    }
}
