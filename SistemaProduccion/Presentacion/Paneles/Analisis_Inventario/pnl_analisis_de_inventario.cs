using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaProduccion.Presentacion.Paneles.Analisis_Inventario
{
    public partial class pnl_analisis_de_inventario : UserControl
    {
        private double costo_pedir = 0, costo_mantener = 0, costo_unidad=0, H=0;
        private double[] demanda;
        double count = 0;
      

        public pnl_analisis_de_inventario()
        {
            InitializeComponent();
            this.comboMetodo.SelectedIndex = 0;
            this.Deshabilitar();
           
        }

       

        private void comboMetodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboMetodo.SelectedIndex == 0)
            {
                this.comboMantenimiento.Enabled = false;
            }
            else if(this.comboMetodo.SelectedIndex == 1)
            {
                this.comboMantenimiento.Enabled = true;
            }
            else
            {   
                this.comboMantenimiento.Enabled = true;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int filas = Convert.ToInt32(this.numericUpDown1.Value);
            int filasActuales = this.dataMetodos.Rows.Count;
            int diferencia = filas - filasActuales;
            if (diferencia > 0)
            {
                for (int i = 0; i < diferencia; i++)
                {
                    String[] fila = new String[] {Convert.ToString((filasActuales + (i + 1))), "0"};
                    this.dataMetodos.Rows.Add(fila);
                }
            }
            else
            {
                diferencia *= -1;
                for (int i = filasActuales - 1; i >= (filasActuales - diferencia); i--)
                {
                    this.dataMetodos.Rows.RemoveAt(i);
                }
            }
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            switch(this.comboMetodo.SelectedIndex){

                case 0:                  
                      this.MetodoL4L();                    
                    break;
                case 1:
                    if (this.comboMantenimiento.SelectedIndex == 0)
                    {  
                        costo_pedir = Convert.ToDouble(this.txtCosto_pedir.Text);
                        costo_mantener = Convert.ToDouble(this.numericTasa.Value);
                        costo_unidad = Convert.ToDouble(this.txtCostoUnidad.Text);
                        costo_mantener /= 100;
                        H = (costo_mantener/costo_unidad)*costo_unidad*52;
                        double Qoptimo = getQEOQ(obtenerDemanda(), costo_pedir, H);
                        Console.WriteLine(Qoptimo);
                        this.MRP(dataMetodos, costo_pedir, H, Qoptimo);
                        this.lblCostoMetodoEOQ.LabelText = "C$ " + Convert.ToString(count);
                        count = 0;
                    }
                    else
                    {
                      
                    }
                    break;
                case 2:
                    if (this.comboMantenimiento.SelectedIndex == 0)
                    {
                        costo_pedir = Convert.ToDouble(this.txtCosto_pedir.Text);
                        costo_mantener = Convert.ToDouble(this.numericTasa.Value);
                        costo_mantener /= 100;
                        H = (costo_mantener / costo_unidad) * costo_unidad * 52;
                        double[,] LTCM = getLTCM(obtenerDemanda(), costo_mantener, costo_pedir); 
                        double Q2 = getQLTC(LTCM, dataMetodos.RowCount);
                        Console.WriteLine(Q2);
                        this.MRP(dataMetodos, costo_pedir, costo_mantener, Q2);
                        this.tileMetodoLTC.LabelText = "C$ " + Convert.ToString(count);
                        count = 0;
                    }
                    else
                    {

                    }
                    break;
                case 3:
                    if (this.comboMantenimiento.SelectedIndex == 0)
                    {
                        costo_pedir = Convert.ToDouble(this.txtCosto_pedir.Text);
                        costo_mantener = Convert.ToDouble(this.numericTasa.Value);
                        costo_mantener /= 100;
                        H = (costo_mantener / costo_unidad) * costo_unidad * 52;
                        double[,] LTUCM = getLUCM(obtenerDemanda(), costo_mantener, costo_pedir); 
                        double Q2 = getLUCQ(LTUCM, dataMetodos.RowCount); 
                        Console.WriteLine(Q2);
                        this.MRP(dataMetodos, costo_pedir, costo_mantener, Q2);
                        this.TileMetodoLUC.LabelText = "C$ " + Convert.ToString(count);
                        count = 0;
                    }
                    else
                    {

                    }
                    break;
            }
        }
           
        private void btnLimpiarCampos_Click(object sender, EventArgs e)
        {

            this.Limpiar_Campos();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.btnLimpiarCampos_Click(sender, e);
            this.Deshabilitar();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Habilitar();
        }



        private void Deshabilitar()
        {
            this.btnCalcular.Enabled = false;
            this.btnCancelar.Enabled = false;
            this.btnLimpiarCampos.Enabled = false;
            this.dataMetodos.Enabled = false;
            this.comboMetodo.Enabled = false;
            this.comboMantenimiento.Enabled = false;
            this.numericUpDown1.Enabled = false;
            this.numericTasa.Enabled = false;
            this.txtCosto_pedir.Enabled = false;
            this.txtCostoUnidad.Enabled = false;
            this.btnAgregar.Enabled = true;
            this.txtCostoGeneralMnto.Enabled = false;
        }

        private void Habilitar()
        {
            this.btnCalcular.Enabled = true;
            this.btnCancelar.Enabled = true;
            this.btnAgregar.Enabled = false;
            this.btnLimpiarCampos.Enabled = true;
            this.dataMetodos.Enabled = true;
            this.comboMetodo.Enabled = true;
            this.comboMantenimiento.Enabled = true;
            this.numericUpDown1.Enabled = true;
            this.txtCosto_pedir.Enabled = true;
        }


        private void Limpiar_Campos()
        {
            this.txtCosto_pedir.Text = string.Empty;
            this.txtCostoUnidad.Text = string.Empty;
            this.txtCostoGeneralMnto.Text = string.Empty;
            this.comboMantenimiento.SelectedIndex = 0;
            this.comboMetodo.SelectedIndex = 0;
            this.numericUpDown1.Value = 0;
            this.numericTasa.Value = 0;
            this.lblCostoMetodoEOQ.LabelText = "C$ 0.00";
            this.tileMetodoL4L.LabelText = "C$ 0.00";
            this.tileMetodoLTC.LabelText = "C$ 0.00";
            this.TileMetodoLUC.LabelText = "C$ 0.00"; ;

            if (dataMetodos.RowCount != -1)
            {
                for (int i = dataMetodos.RowCount - 1; i >= 0; i--)
                {
                    dataMetodos.Rows.RemoveAt(i);
                }
            }
        }


        private void comboMantenimiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboMantenimiento.SelectedIndex == 0)
            {
                this.txtCostoGeneralMnto.Enabled = false;
                this.txtCostoUnidad.Enabled = true;
                this.numericTasa.Enabled = true;

            }
            else
            {
                this.txtCostoGeneralMnto.Enabled = true;
                this.txtCostoUnidad.Enabled = false;
                this.numericTasa.Enabled = false;
            }
        }


        private double[] obtenerDemanda(){
            demanda = new double[dataMetodos.RowCount];
            for (int i = 0; i < demanda.Length; i++)
            {
                demanda[i] = Convert.ToDouble(dataMetodos.Rows[i].Cells[1].Value.ToString());
            }

            return demanda;
        }

        /*........ MÉTODO L4L .......*/

        private void MetodoL4L()
        {
            this.costo_pedir = Convert.ToDouble(this.txtCosto_pedir.Text);
            this.demanda = new double[this.dataMetodos.RowCount];
            double count = 0;
            for (int i = 0; i < demanda.Length; i++ )
            {
                dataMetodos.Rows[i].Cells[2].Value = dataMetodos.Rows[i].Cells[1].Value;
                dataMetodos.Rows[i].Cells[3].Value = 0;
                dataMetodos.Rows[i].Cells[4].Value = 0;
                this.dataMetodos.Rows[i].Cells[5].Value = this.costo_pedir;
                count += costo_pedir;
                dataMetodos.Rows[i].Cells[6].Value = count;
                this.tileMetodoL4L.LabelText = "C$ " + count;
            }
        }

        /*........ MÉTODO PARA OBTENER LA DEMANDA OPTIMA EOQ ......*/
        private double getQEOQ(double[] demanda, double ordenar, double mantenimineto)
        {
            double pro = 0;
            for (int i = 0; i < demanda.Length; i++)
            {
                pro += demanda[i];

            }
            pro = pro / demanda.Length;
            pro *= 52;
            double q;
            q = Math.Pow((2 * pro * ordenar) / mantenimineto, 0.5);
            int cant = (int)q;
            return cant;
        }


        /*.......... LTC ........*/

        private double[,] getLTCM(double[] demanda, double mantenimiento, double ordenar)
        {

            double[,] LTC = new double[demanda.Length, 4];
            double demTMP = 0, demA = 0, demB = 0;

            for (int i = 0; i < demanda.Length; i++)
            {
                LTC[i, 0] = (i + 1); // Semanas
                demTMP += demanda[i]; // Produccion
                LTC[i, 1] = demTMP;
                demA = LTC[i, 1];
                demA = LTC[i, 1]; // Mantenimiento
                for (int j = 0; j < (i + 1); j++)
                {
                    demA -= demanda[j];
                    demB += demA;
                }
                LTC[i, 2] = demB * mantenimiento;
                demB = 0;
                LTC[i, 3] = ordenar; // Pedir

            }

            return LTC;
        }

        static double getQLTC(double[,] LTCM, int length)
        {
            double[] tmp = new double[length];
            for (int i = 0; i < length; i++)
            {
                if ((LTCM[i, 2] - LTCM[i, 3]) < 0)
                {
                    tmp[i] = LTCM[i, 3] - LTCM[i, 2];
                }
                else
                {
                    tmp[i] = LTCM[i, 2] - LTCM[i, 3];
                }

            }
            double min = tmp[0];
            int pos = 0;

            for (int i = 0; i < length; i++)
            {
                if (tmp[i] < min)
                {
                    min = tmp[i];
                    pos = i;
                }
            }
            return LTCM[pos, 1];
        }


        /*..........LUC .........*/
        private double[,] getLUCM(double[] demanda, double mantenimiento, double ordenar)
        {

            double[,] LUC = new double[demanda.Length, 6];
            double demTMP = 0, demA = 0, demB = 0;

            for (int i = 0; i < demanda.Length; i++)
            {
                LUC[i, 0] = (i + 1); // Semanas
                demTMP += demanda[i]; // Produccion
                LUC[i, 1] = demTMP;
                demA = LUC[i, 1];
                demA = LUC[i, 1]; // Mantenimiento
                for (int j = 0; j < (i + 1); j++)
                {
                    demA -= demanda[j];
                    demB += demA;
                }
                LUC[i, 2] = demB * mantenimiento;
                demB = 0;
                LUC[i, 3] = ordenar; // Pedir
                LUC[i, 4] = LUC[i, 2] + LUC[i, 3]; //Costo T
                LUC[i, 5] = LUC[i, 4] / LUC[i, 1]; // UC

            }
            return LUC;
        }

        private double getLUCQ(double[,] LUCM, int length)
        {
            double min = LUCM[0, 5];
            int pos = 0;
            for (int i = 0; i < length; i++)
            {
                if (LUCM[i, 5] < min)
                {
                    min = LUCM[i, 5];
                    pos = i;
                }
            }
            return LUCM[pos, 1]; ;
        }

        /*.......MÉTODO PARA OBTENER LA TABLA DEL MRP.......*/
        private void MRP(DataGridView demanda, double ordenar, double mantenimiento, double Qoptimo)
        {
         
            for (int i = 0; i < demanda.RowCount; i++ )
            {
                demanda.Rows[i].Cells[2].Value = 0;
                demanda.Rows[0].Cells[2].Value = Qoptimo;
                if (i == 0)
                {
                    demanda.Rows[i].Cells[3].Value = (Convert.ToDouble(demanda.Rows[0].Cells[2].Value) - Convert.ToDouble(demanda.Rows[0].Cells[1].Value));
                }
                else
                {
                    if ((Convert.ToDouble(demanda.Rows[i - 1].Cells[3].Value) - Convert.ToDouble(demanda.Rows[i].Cells[1].Value)) < 0)
                    {
                        demanda.Rows[i].Cells[2].Value = Qoptimo;
                        demanda.Rows[i].Cells[3].Value = Qoptimo + (Convert.ToDouble(demanda.Rows[i - 1].Cells[3].Value) - Convert.ToDouble(demanda.Rows[i].Cells[1].Value));
                    }
                    else
                    {
                        demanda.Rows[i].Cells[3].Value = (Convert.ToDouble(demanda.Rows[i - 1].Cells[3].Value) - Convert.ToDouble(demanda.Rows[i].Cells[1].Value));
                    }
                }

                demanda.Rows[i].Cells[4].Value = Convert.ToDouble(demanda.Rows[i].Cells[3].Value) * costo_mantener;

                if (Convert.ToDouble(demanda.Rows[i].Cells[2].Value) == 0) 
                {
                    demanda.Rows[i].Cells[5].Value = 0;
                }
                else
                {
                    demanda.Rows[i].Cells[5].Value = costo_pedir;

                }
                count += Convert.ToDouble(demanda.Rows[i].Cells[5].Value) + Convert.ToDouble(demanda.Rows[i].Cells[4].Value);
                demanda.Rows[i].Cells[6].Value = count;
                
            }
        }
        

    }
}
