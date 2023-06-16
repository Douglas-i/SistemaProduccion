using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaProduccion.Presentacion.Paneles.Planeacion_agregada
{
    public partial class Pnl_planeacion_agregada : UserControl
    {
         
        public Pnl_planeacion_agregada()
        {
            InitializeComponent();
            this.dataMetodos.Columns.Add("columna1","Meses");
            String[] fila= new String[1];
            fila[0]="Demanda";
            this.dataMetodos.Rows.Add(fila);
            fila[0] = "Días";
            this.dataMetodos.Rows.Add(fila);
        }

        /*........ METODO DE FUERZA NIVELADA CON HORAS EXTRAS .......*/

        static double[,] getNExtraHrs(double[,] demLab, double inveIni, double[] ss, double hrsReq, int trabAct, double Ccont, double Cdesp, double hrsNorm, double hrsExtra, double mant)
        {
            double[,] mat = new double[14, (demLab.Length / 2)];

            double tDem = 0, tReqUn = 0;
            for (int i = 0; i < (demLab.Length / 2); i++)
            {
                mat[0, i] = demLab[0, i]; //Demanda
                mat[1, i] = demLab[1, i]; //Periodo laboral
                mat[3, i] = ss[i]; //ss
                if (i == 0)  //Inventario Inicial
                {
                    mat[2, i] = inveIni;
                }
                else
                {
                    mat[2, i] = mat[3, (i - 1)];
                }
                mat[4, i] = mat[0, i] + mat[3, i] - mat[2, i]; //Requerimiento unidades
                tDem += demLab[0, i];
                tReqUn += mat[4, i];
            }
            int trab = (int)Math.Ceiling((tReqUn * hrsReq) / (tDem / 8));
            for (int i = 0; i < (demLab.Length / 2); i++)
            {
                mat[5, i] = trab; // Trabajadores requeridos
                mat[6, i] = (mat[1, i] * 8 * mat[5, i]) / hrsReq; // Produccion maxima
                if (i == 0) // Inventario final
                {
                    mat[7, i] = mat[6, i] - mat[4, i];
                }
                else
                {
                    if (mat[7, (i - 1)] < 0)
                    {
                        mat[7, i] = mat[6, i] - mat[4, i];
                    }
                    else
                    {
                        mat[7, i] = mat[6, i] - mat[4, i] + mat[7, (i - 1)];
                    }
                }
                if (mat[7, i] < 0)
                {
                    mat[8, i] = mat[7, i] * hrsReq * -1; //Horas extras requeridas
                }
                if ((mat[5, 0] - trabAct) < 0)
                {
                    mat[10, 0] = Cdesp * (trabAct - mat[5, 0]); // Costos de despidos
                }
                else
                {
                    mat[9, 0] = (mat[5, 0] - trabAct) * Ccont; // Costos de contratacion
                }
                mat[11, i] = mat[1, i] * mat[5, i] * 8 * hrsNorm; // Costos de horas normales
                mat[12, i] = mat[8, i] * hrsExtra; // Costos de horas extras
                if (mat[7, i] < 0) //Costos de mantenimiento
                {
                    mat[13, i] = mat[3, i] * mant;
                }
                else
                {
                    mat[13, i] = (mat[3, i] * mant) + (mat[7, i] * mant);
                }
            }
            return mat;
        }

        /*........ METODO DE FUERZA NIVELADA .......*/
        static double[,] getNivelada(double[,] demLab, double inveIni, double[] ss, double hrsReq, int trabAct, double Ccont, double Cdesp, double hrsNorm, double mant, double Cfalt)
        {
            double[,] mat = new double[13, (demLab.Length / 2)];
            double tDem = 0, tReqUn = 0;
            for (int i = 0; i < (demLab.Length / 2); i++)
            {
                mat[0, i] = demLab[0, i]; //Demanda
                mat[1, i] = demLab[1, i]; //Periodo laboral
                mat[3, i] = ss[i]; //ss
                if (i == 0)  //Inventario Inicial
                {
                    mat[2, i] = inveIni;
                }
                else
                {
                    mat[2, i] = mat[3, (i - 1)];
                }
                mat[4, i] = mat[0, i] + mat[3, i] - mat[2, i]; //Requerimiento unidades
                tDem += demLab[0, i];
                tReqUn += mat[4, i];
            }
            int trab = (int)Math.Ceiling((tReqUn * hrsReq) / (tDem / 8));
            for (int i = 0; i < (demLab.Length / 2); i++)
            {
                mat[5, i] = trab; // Trabajadores requeridos
                mat[6, i] = (mat[1, i] * 8 * mat[5, i]) / hrsReq; // Produccion maxima
                if (i == 0) // Inventario final
                {
                    mat[7, i] = mat[6, i] - mat[4, i];
                }
                else
                {
                    if (mat[7, (i - 1)] < 0)
                    {
                        mat[7, i] = mat[6, i] - mat[4, i];
                    }
                    else
                    {
                        mat[7, i] = mat[6, i] - mat[4, i] + mat[7, (i - 1)];
                    }
                }
                mat[10, i] = mat[1, i] * mat[5, i] * 8 * hrsNorm; // Costos de horas normales
                if (mat[7, i] < 0) //Costos de mantenimiento
                {
                    mat[11, i] = mat[3, i] * mant;
                }
                else
                {
                    mat[11, i] = (mat[3, i] * mant) + (mat[7, i] * mant);
                }
                if (mat[7, i] < 0)
                {
                    mat[12, i] = (mat[7, i] * -1) * Cfalt; //Costo del faltante
                }
            }

            if ((mat[5, 0] - trabAct) < 0)
            {
                mat[9, 0] = Cdesp * (trabAct - mat[5, 0]); // Costos de despidos
            }
            else
            {
                mat[8, 0] = (mat[5, 0] - trabAct) * Ccont; // Costos de contratacion
            }
            return mat;
        }

        /*........ METODO DE PERSECUCION .......*/
        private double[,] getPer(double[,] demLab, double inveIni, double[] ss, double hrsReq, int trabAct, double Ccont, double Cdesp, double hrsNorm, double mant)
        {
            double[,] mat = new double[12, (demLab.Length / 2)];
            for (int i = 0; i < (demLab.Length / 2); i++)
            {
                mat[0, i] = demLab[0, i]; //Demanda
                mat[1, i] = demLab[1, i]; //Periodo laboral
                mat[3, i] = ss[i]; //ss
                if (i == 0)  //Inventario Inicial
                {
                    mat[2, i] = inveIni;
                }
                else
                {
                    mat[2, i] = mat[3, (i - 1)];
                }
                mat[4, i] = mat[0, i] + mat[3, i] - mat[2, i]; //Requerimiento unidades
                mat[5, i] = mat[4, i] * hrsReq; //Requerimiento tiempo
                mat[6, i] = mat[1, i] * 8; //Horas disponibles :v ese ocho no se si hacerlo variable
                mat[7, i] = Math.Ceiling(mat[5, i] / mat[6, i]); //Trabajadores requeridos
                if (i == 0) //Costos de Contratacion y Despido
                {
                    if (mat[7, i] > trabAct)
                    {
                        mat[8, i] = (mat[7, i] - trabAct) * Ccont;
                    }
                    else
                    {
                        mat[9, i] = (trabAct - mat[7, i]) * Cdesp;
                    }
                }
                else
                {
                    if ((mat[7, i - 1] - mat[7, i]) < 0)
                    {
                        mat[8, i] = (mat[7, i] - mat[7, i - 1]) * Ccont;
                    }
                    else
                    {
                        mat[9, i] = (mat[7, i - 1] - mat[7, i]) * Cdesp;
                    }
                }
                mat[10, i] = mat[6, i] * mat[7, i] * hrsNorm;
                mat[11, i] = mat[3, i] * mant;
            }
            return mat;
        }

        private void comboMetodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboMetodo.SelectedIndex == 0)
            {

            }
        }

        private void numericMeses_ValueChanged(object sender, EventArgs e)
        {
            
            if (!flag) {
                dataMetodos.Columns.Clear();
                this.dataMetodos.Columns.Add("columna1", "Meses");
                String[] fila = new String[1];
                fila[0] = "Demanda";
                this.dataMetodos.Rows.Add(fila);
                fila[0] = "Días";
                this.dataMetodos.Rows.Add(fila);
                numericMeses.Value = 0;
                flag = true;
                btnCalcular.Enabled = true;
            }

            int x = this.dataMetodos.Columns.Count - 1;
            
            if (x<numericMeses.Value)
            {
                this.dataMetodos.Columns.Add("", Convert.ToString(x+1));
                
            }
            else if (x > numericMeses.Value)
            {
                this.dataMetodos.Columns.RemoveAt(x);
               
            }
        }

        static double[,] tmp;

        public int columna { get; set; }
        Boolean flag = true;

        private void calcularPer() {

            double[,] data;
            if (flag)
            {
                data = obtenerDemanda();
                tmp = data;
            }
            else {
                data = tmp;
            }
            flag = false;

            double[,] Plan = getPer(data, Convert.ToDouble(txtInventarioInicial.Text), ss(Convert.ToDouble(this.numericSS.Value)),
                Convert.ToDouble(this.numeriHorasREqueridas.Value), Convert.ToInt32(this.numericFuerzaLaboral.Value),
                Convert.ToDouble(this.txtCostoContratacion.Text), Convert.ToDouble(this.txtcostoDespido.Text),
                Convert.ToDouble(this.txtCostoHorasNormales.Text), Convert.ToDouble(this.txtCostoMantenimiento.Text));

            dataMetodos.Columns.Clear();
            int count = 0;
            int count2 = 0;
            int rowCount = Plan.GetLength(0);
            int rowLength = Plan.GetLength(1);
            this.dataMetodos.ColumnCount = rowLength + 1;
            String[] Descrip = { "Demanda", "Dia Laboral", "Invent. inicial", "SS", "Requerimiento de unidad", "Requerimiento por hora", "Hora Disponible", "Requerimieto de trabajadores", "Costos de contratacion", "Costos de despido", "Costos de horas normales", "Costo de mantenimiento" };
            for (int i = 0; i < rowCount+1; i++)
            {
                var row = new DataGridViewRow();

                for (int j = 0; j < rowLength + 1; j++)
                {
                    
                    if (i==0) {
                        row.Cells.Add(new DataGridViewTextBoxCell() { Value = count2 });
                        count2++;
                    }
                    else
                    {
                        if (j == 0)
                        {
                            row.Cells.Add(new DataGridViewTextBoxCell() { Value = Descrip[count] });
                            count++;
                        }
                        else {
                            row.Cells.Add(new DataGridViewTextBoxCell() { Value = Plan[i - 1, j - 1] });
                        }
                    }

                }
                dataMetodos.Rows.Add(row);
            }
            dataMetodos[0, 0].Value = "Meses";

            double totalC = 0;

            for (int i = 8; i < 12; i++)
            {
                for (int j = 0; j < (data.Length/2); j++)
                {
                    totalC += Plan[i, j];
                }
            }
            this.lblPersecucion.LabelText = "C$ " + Convert.ToString(totalC);
        }

        private void calcularFuerzaNivelada() {
            double[,] data;
            if (flag)
            {
                data = obtenerDemanda();
                tmp = data;
            }
            else
            {
                data = tmp;
            }
            flag = false;
            double[,] Plan = getNivelada(data, Convert.ToDouble(txtInventarioInicial.Text),ss(Convert.ToDouble(this.numericSS.Value)),
                Convert.ToDouble(this.numeriHorasREqueridas.Value), Convert.ToInt32(this.numericFuerzaLaboral.Value),
                Convert.ToDouble(this.txtCostoContratacion.Text), Convert.ToDouble(this.txtcostoDespido.Text),
                Convert.ToDouble(this.txtCostoHorasNormales.Text), Convert.ToDouble(this.txtCostoMantenimiento.Text),
                Convert.ToDouble(this.txtCostoFaltante.Text));
            dataMetodos.Columns.Clear();

            int count = 0;
            int count2 = 0;
            int rowCount = Plan.GetLength(0);
            int rowLength = Plan.GetLength(1);
            this.dataMetodos.ColumnCount = rowLength + 1;
            String[] Descrip = { "Demanda", "Dia Laboral", "Invent. inicial", "SS", "Requerimiento de unidad", "Requerimiento de trabajadores", "Produccion", "Inventario final", "Costos de contratacion", "Costos de despido", "Costos de horas normales", "Costo de mantenimiento","Costo faltante" };
            for (int i = 0; i < rowCount + 1; i++)
            {
                var row = new DataGridViewRow();

                for (int j = 0; j < rowLength + 1; j++)
                {

                    if (i == 0)
                    {
                        row.Cells.Add(new DataGridViewTextBoxCell() { Value = count2 });
                        count2++;
                    }
                    else
                    {
                        if (j == 0)
                        {
                            row.Cells.Add(new DataGridViewTextBoxCell() { Value = Descrip[count] });
                            count++;
                        }
                        else
                        {
                            row.Cells.Add(new DataGridViewTextBoxCell() { Value = Plan[i - 1, j - 1] });
                        }
                    }

                }
                dataMetodos.Rows.Add(row);
            }
            dataMetodos[0, 0].Value = "Meses";

            double totalC = 0;

            for (int i = 8; i < 13; i++)
            {
                for (int j = 0; j < (data.Length / 2); j++)
                {
                    totalC += Plan[i, j];
                }
            }
            this.lblNivelado.LabelText = "C$ " + Convert.ToString(totalC);
        }

        private void calculoHoraExtra() {
            double[,] data;
            if (flag)
            {
                data = obtenerDemanda();
                tmp = data;
            }
            else
            {
                data = tmp;
            }
            flag = false;
            double[,] Plan = getNExtraHrs(data, Convert.ToDouble(txtInventarioInicial.Text), ss(Convert.ToDouble(this.numericSS.Value)),
                Convert.ToDouble(this.numeriHorasREqueridas.Value), Convert.ToInt32(this.numericFuerzaLaboral.Value),
                Convert.ToDouble(this.txtCostoContratacion.Text), Convert.ToDouble(this.txtcostoDespido.Text),
                Convert.ToDouble(this.txtCostoHorasNormales.Text), Convert.ToDouble(this.txtHorasExtras.Text),
                Convert.ToDouble(this.txtCostoMantenimiento.Text));
            dataMetodos.Columns.Clear();

            int count = 0;
            int count2 = 0;
            int rowCount = Plan.GetLength(0);
            int rowLength = Plan.GetLength(1);
            this.dataMetodos.ColumnCount = rowLength + 1;
            String[] Descrip = { "Demanda", "Dia Laboral", "Invent. inicial", "SS", "Requerimiento de unidad", "Requerimiento de trabajadores", "Produccion", "Inventario final", "Horas Faltantes", "Costos de contratacion", "Costos de despido", "Costos de horas normales", "Costo de horas extras" ,"Costo de mantenimiento"};
            for (int i = 0; i < rowCount + 1; i++)
            {
                var row = new DataGridViewRow();

                for (int j = 0; j < rowLength + 1; j++)
                {

                    if (i == 0)
                    {
                        row.Cells.Add(new DataGridViewTextBoxCell() { Value = count2 });
                        count2++;
                    }
                    else
                    {
                        if (j == 0)
                        {
                            row.Cells.Add(new DataGridViewTextBoxCell() { Value = Descrip[count] });
                            count++;
                        }
                        else
                        {
                            row.Cells.Add(new DataGridViewTextBoxCell() { Value = Plan[i - 1, j - 1] });
                        }
                    }

                }
                dataMetodos.Rows.Add(row);
            }
            dataMetodos[0, 0].Value = "Meses";

            double totalC = 0;

            for (int i = 9; i < 14; i++)
            {
                for (int j = 0; j < (data.Length / 2); j++)
                {
                    totalC += Plan[i, j];
                }
            }
            this.lblExtras.LabelText = "C$ " + Convert.ToString(totalC);
        }

        private void calcularOut() {

            double[,] data;
            if (flag)
            {
                data = obtenerDemanda();
                tmp = data;
            }
            else
            {
                data = tmp;
            }
            flag = false;
            double[,] Plan = getNivelada(data, Convert.ToDouble(txtInventarioInicial.Text), ss(Convert.ToDouble(this.numericSS.Value)),
                Convert.ToDouble(this.numeriHorasREqueridas.Value), Convert.ToInt32(this.numericFuerzaLaboral.Value),
                Convert.ToDouble(this.txtCostoContratacion.Text), Convert.ToDouble(this.txtcostoDespido.Text),
                Convert.ToDouble(this.txtCostoHorasNormales.Text), Convert.ToDouble(this.txtCostoMantenimiento.Text),
                Convert.ToDouble(this.txtOutsourcing.Text));
            dataMetodos.Columns.Clear();

            int count = 0;
            int count2 = 0;
            int rowCount = Plan.GetLength(0);
            int rowLength = Plan.GetLength(1);
            this.dataMetodos.ColumnCount = rowLength + 1;
            String[] Descrip = { "Demanda", "Dia Laboral", "Invent. inicial", "SS", "Requerimiento de unidad", "Requerimiento de trabajadores", "Produccion", "Inventario final", "Costos de contratacion", "Costos de despido", "Costos de horas normales", "Costo de mantenimiento", "Costo de Out-Sourcing" };
            for (int i = 0; i < rowCount + 1; i++)
            {
                var row = new DataGridViewRow();

                for (int j = 0; j < rowLength + 1; j++)
                {

                    if (i == 0)
                    {
                        row.Cells.Add(new DataGridViewTextBoxCell() { Value = count2 });
                        count2++;
                    }
                    else
                    {
                        if (j == 0)
                        {
                            row.Cells.Add(new DataGridViewTextBoxCell() { Value = Descrip[count] });
                            count++;
                        }
                        else
                        {
                            row.Cells.Add(new DataGridViewTextBoxCell() { Value = Plan[i - 1, j - 1] });
                        }
                    }

                }
                dataMetodos.Rows.Add(row);
            }
            dataMetodos[0, 0].Value = "Meses";

            double totalC = 0;

            for (int i = 8; i < 13; i++)
            {
                for (int j = 0; j < (data.Length / 2); j++)
                {
                    totalC += Plan[i, j];
                }
            }
            this.lblOut.LabelText = "C$ " + Convert.ToString(totalC);

        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {

            switch (comboMetodo.SelectedIndex) {
                case 0:
                    calcularPer();
                    break;
                case 1:
                    calcularFuerzaNivelada();
                    break;
                case 2:
                    calculoHoraExtra();
                    break;
                case 3:
                    calcularOut();
                    break;
            }

        }
        private double[] ss(double por) {
            double[,] data = tmp;
            double[] ss = new double[data.Length/2];
            for (int i = 0; i < (data.Length/2); i++)
            {
                ss[i] = data[0,i]*(por/100);
            }
            return ss;
        }
        private double[,] obtenerDemanda(){

           double[,] demanda = new double[this.dataMetodos.RowCount,this.dataMetodos.ColumnCount-1];
            
            for (int i = 0; i < dataMetodos.RowCount; i++)
            {

                for (int j = 1; j < dataMetodos.ColumnCount; j++)
                {
                    demanda[i,j-1] = Convert.ToDouble(dataMetodos.Rows[i].Cells[j].Value.ToString());
                    Console.Write(demanda[i,j-1] + " ");
                }
                Console.WriteLine("");
            }

            return demanda;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            btnLimpiarCampos.Enabled = true;
            btnCancelar.Enabled = true;
        }
    }
}
