using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemaInventarioProduccion.Presentacion.ArbolEstructurado;
using SistemaProduccion.Presentacion.Paneles.TmpPanelAlex.datosArbolEstructurado;

namespace SistemaProduccion.Presentacion.Paneles.TmpPanelAlex
{
    public partial class pnlGestionOperaciones : UserControl, interfeProductChanged
    {
        private String[] productos;
        private pnlItemArbolEstruct[] paneles;
        public pnlGestionOperaciones()
        {
            InitializeComponent();
            paneles = new pnlItemArbolEstruct[0];
            this.productos = new String[1];
            this.productos[0] = "";
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            pnlItemArbolEstruct pnl = new pnlItemArbolEstruct(this, this.panelInterno.Controls.Count);
            pnl.Size = new Size(this.panelInterno.Size.Width - 10, pnl.Size.Height);
            this.paneles = agregarElemento(pnl, this.paneles);
            pnl.setProductos(this.productos);
            this.productos = agregarElemento("", productos);
            this.panelInterno.Controls.Add(pnl);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            nuevoProducto(this.txtNombreProducto.Text, -1);
        }

        public void nuevoProducto(string nombre, int position)
        {
            //productos[0] es el producto principal
            this.productos[position + 1] = nombre;
            for (int i = 0; i < paneles.Length; i++)
            {
                this.paneles[i].setProductos(this.productos);
            }
        }

        private pnlItemArbolEstruct[] agregarElemento(pnlItemArbolEstruct elemento, pnlItemArbolEstruct[] arreglo)
        {
            pnlItemArbolEstruct[] tmp = new pnlItemArbolEstruct[arreglo.Length + 1];
            Array.Copy(arreglo, tmp, arreglo.Length);
            tmp[arreglo.Length] = elemento;
            return tmp;
        }

        private TreeNode<PictureNode>[] agregarElemento(TreeNode<PictureNode> elemento, TreeNode<PictureNode>[] arreglo)
        {
            TreeNode<PictureNode>[] tmp = new TreeNode<PictureNode>[arreglo.Length + 1];
            Array.Copy(arreglo, tmp, arreglo.Length);
            tmp[arreglo.Length] = elemento;
            return tmp;
        }

        private String[] agregarElemento(String elemento, String[] arreglo)
        {
            foreach (String item in arreglo)
            {
                Console.Write(item + ",");
            }
            Console.WriteLine(arreglo.Length);
            String[] tmp = new String[arreglo.Length + 1];
            for (int i = 0; i < arreglo.Length; i++)
            {
                tmp[i] = arreglo[i];
            }
            //Array.Copy(arreglo, tmp, arreglo.Length+1);
            tmp[arreglo.Length] = elemento;
            foreach (String item in tmp)
            {
                Console.Write(item + ",");
            }
            Console.WriteLine(tmp.Length);
            return tmp;
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            pnlItemArbolEstruct pnl = new pnlItemArbolEstruct(this, this.panelInterno.Controls.Count);
            pnl.Size = new Size(this.panelInterno.Size.Width - 10, pnl.Size.Height);
            this.paneles = agregarElemento(pnl, this.paneles);
            pnl.setProductos(this.productos);
            this.productos = agregarElemento("", productos);
            this.panelInterno.Controls.Add(pnl);
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            TreeNode<PictureNode> root = new TreeNode<PictureNode>(new PictureNode(productos[0], 1));
            TreeNode<PictureNode>[] nodosHijo = new TreeNode<PictureNode>[0];
            TreeNode<PictureNode> nodoHijo;
            foreach (pnlItemArbolEstruct pnl in paneles)
            {
                if (pnl.esHijoDe(productos[0]))
                {
                    nodoHijo = new TreeNode<PictureNode>(new PictureNode(pnl.getNombreProducto(), pnl.getCantidad()));
                    nodosHijo = agregarElemento(nodoHijo, nodosHijo);
                    root.AddChild(nodoHijo);
                }
            }
            foreach (TreeNode<PictureNode> item in nodosHijo)
            {
                foreach (pnlItemArbolEstruct pnl in paneles)
                {
                    if (pnl.esHijoDe(item.Data.Description))
                    {
                        nodoHijo = new TreeNode<PictureNode>(new PictureNode(pnl.getNombreProducto(), pnl.getCantidad()));
                        item.AddChild(nodoHijo);
                    }
                }
            }
            frmDemo demo = new frmDemo(root);
            demo.ShowDialog();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int filas = Convert.ToInt32(this.numericUpDown1.Value);
            int filasActuales = this.dataGridView1.Rows.Count;
            int diferencia = filas - filasActuales;
            if (diferencia > 0)
            {
                for (int i = 0; i <diferencia; i++)
                {
                  String[] fila=new String[]{"# "+(filasActuales+(i+1)),"0","0"};
                    this.dataGridView1.Rows.Add(fila);
                }
            }
            else
            {
                diferencia *= -1;
                for (int i = filasActuales-1; i >= (filasActuales-diferencia); i--)
                {
                    this.dataGridView1.Rows.RemoveAt(i);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboBox1.SelectedIndex)
            {
                case 0:
                    label12.Visible = true;
                    txtQOptimo.Visible = true;
                    break;
                case 1:
                    label12.Visible = false;
                    txtQOptimo.Visible = false;
                    break;
            }
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            String producto;
            int tiempoEspera;
            double Qopt, SS, inventarioInicial;
            double[] demanda, entradasProgramadas;
            double[][] explosion;

            producto = this.txtNombreProducto.Text;
            try
            {
                tiempoEspera = Convert.ToInt32(this.txtTiempoEspera.Text);
                SS = Convert.ToDouble(this.txtSS.Text);
                inventarioInicial = Convert.ToInt32(this.txtInventInicial.Text);
                demanda = new double[this.dataGridView1.RowCount];
                entradasProgramadas = new double[this.dataGridView1.RowCount];
                int contador = 0;
                foreach (DataGridViewRow item in this.dataGridView1.Rows)
                {
                    demanda[contador] = Convert.ToDouble(item.Cells[1].Value);
                    entradasProgramadas[contador] = Convert.ToDouble(item.Cells[2].Value);
                    contador++;
                }
            }
            catch (Exception excep)
            {
                MessageBox.Show("Hubo al error al leer los datos, por favor revise");
                Console.WriteLine(excep.ToString());
                return;
            }
            if (this.comboBox1.SelectedIndex == 0)//Q optimo
            {
                Qopt = Convert.ToDouble(this.txtQOptimo.Text);
                explosion = obtenerExplosionMRP(demanda, entradasProgramadas, inventarioInicial, Qopt, tiempoEspera, SS);
            }
            else
            {
                explosion = obtenerExplosionMRPL4L(demanda, entradasProgramadas, inventarioInicial, tiempoEspera, SS);
            }
            rellenarTabla(explosion,tiempoEspera);
        }

        private void rellenarTabla(double[][] explosion,int plazoEntrega)
        {
            this.dataGridViewMRP.Columns.Clear();
            this.dataGridViewMRP.Columns.Add("columnInicial","");
            for (int i = 0; i < plazoEntrega; i++)
            {
                this.dataGridViewMRP.Columns.Add("semanaCero" + i, "Semana #" + 0);
            }
            for (int i = 1; i < explosion[0].Length; i++)
            {
                this.dataGridViewMRP.Columns.Add("semana"+i,"Semana #" + i);
            }
            String[] columnas = new String[] {
                "Necesidades brutas",
                "Entradas Programadas",
                "Inventario",
                "Necesidades netas",
                "Entrega de pedidos",
                "Planeación de pedidos"
            };
            String[] fila;
            for (int i = 0; i < explosion.Length; i++)
            {
                fila = new String[explosion[i].Length + 1];
                fila[0] = columnas[i];
                for (int j = 0; j < explosion[i].Length; j++)
                {
                    fila[j + 1] = Convert.ToString(explosion[i][j]);
                }
                this.dataGridViewMRP.Rows.Add(fila);
            }
        }

        private double[][] obtenerExplosionMRP(double[] demanda, double[] entradasProgramadas, double inventarioInicial, double Qopt, int plazoEntrega, double SS)
        {
            double[][] explosion = new double[6][];
            int columnas = plazoEntrega + demanda.Length;
            for (int i = 0; i < explosion.Length; i++)
            {
                explosion[i] = new double[columnas]; //inicializar tamaños (plazoEntrega + cant semanas)
            }
            explosion[2][plazoEntrega] = inventarioInicial - demanda[0] + entradasProgramadas[0] - SS;
            for (int i = plazoEntrega; i < columnas; i++)
            {
                explosion[0][i] = demanda[i - plazoEntrega];   //necesidades brutas
                explosion[1][i] = entradasProgramadas[i - plazoEntrega];       //entradas programadas
                double inventarioFinal = explosion[1][i] + explosion[2][i - 1] - explosion[0][i]; //inventario
                if (i == plazoEntrega)
                {
                    inventarioFinal += inventarioInicial;
                    inventarioFinal -= SS;
                }
                if (inventarioFinal >= SS)
                {
                    explosion[2][i] = inventarioFinal;
                }
                else
                {
                    explosion[3][i] = (inventarioFinal * -1);//nec netas
                    explosion[4][i] = Qopt;
                    if ((i - plazoEntrega) >= 0)
                    {
                        explosion[5][i - plazoEntrega] = Qopt;
                    }
                    inventarioFinal = explosion[4][i] - explosion[3][i];
                    explosion[2][i] = inventarioFinal;
                }

            }
            return explosion;
        }

        private double[][] obtenerExplosionMRPL4L(double[] demanda, double[] entradasProgramadas, double inventarioInicial, int plazoEntrega, double SS)
        {
            double[][] explosion = new double[6][];
            int columnas = plazoEntrega + demanda.Length;
            for (int i = 0; i < explosion.Length; i++)
            {
                explosion[i] = new double[columnas]; //inicializar tamaños
            }
            explosion[2][0] = inventarioInicial;
            for (int i = plazoEntrega; i < columnas; i++)
            {
                explosion[0][i] = demanda[i - plazoEntrega];
                explosion[1][i] = entradasProgramadas[i - plazoEntrega];
                //Inventario = EntradasProgramas + Existencias - SS
                double existenciaAnterior = 0;
                if ((i - 1) < 0)
                {
                    existenciaAnterior = 0;
                }
                else
                {
                    existenciaAnterior = explosion[2][i - 1];
                }
                double inventarioActual = entradasProgramadas[i - plazoEntrega] + existenciaAnterior - SS - explosion[0][i];
                double necNetas = 0;
                if (inventarioActual < 0)
                {
                    necNetas = inventarioActual * -1;
                    inventarioActual = 0;
                }
                explosion[2][i] = inventarioActual;
                //Necesidades netas = Inventario - demanda
                explosion[3][i] = 0;

                explosion[4][i] = necNetas;
                explosion[5][i - plazoEntrega] = necNetas;
            }
            return explosion;
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
