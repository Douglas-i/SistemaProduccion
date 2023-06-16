using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SistemaProduccion.Presentacion.Paneles.TmpPanelAlex.datosArbolEstructurado;

namespace SistemaInventarioProduccion.Presentacion.ArbolEstructurado
{
    public partial class pnlItemArbolEstruct : UserControl
    {
        private bool selected;

        private interfeProductChanged interfaz;
        private int posicion;
        public pnlItemArbolEstruct(interfeProductChanged interfaz, int posicion)
        {
            InitializeComponent();
            this.interfaz = interfaz;
            this.posicion = posicion;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (interfaz!=null)
            {
                interfaz.nuevoProducto(this.textBox1.Text,this.posicion);
            }
        }

        public void setProductos(String[] productos)
        {
            int posicion = this.comboBox1.SelectedIndex;
            this.comboBox1.Items.Clear();
            //String[] filtrado = filtrarLista(productos);
            //this.comboBox1.Items.AddRange(filtrado);
            this.comboBox1.Items.AddRange(productos);
            this.comboBox1.SelectedIndex = posicion;
            this.comboBox1.Update();

        }

        public bool esHijoDe(String elemento)
        {
            Object elementoSeleccionado = this.comboBox1.SelectedItem;
            if (elementoSeleccionado != null)
            {
                return elementoSeleccionado.ToString().Equals(elemento);               
            }
            return false;
        }

        public String getNombreProducto()
        {
            return this.textBox1.Text;
        }

        public int getCantidad()
        {
            return Convert.ToInt32(this.numericUpDown1.Value);
        }

        private String[] filtrarLista(String[] productos)
        {
            String[] nuevaLista = new String[productos.Length-1];
            int contador = 0;            
            for (int i = 0; i < nuevaLista.Length; i++)
            {
                if (i != (posicion+1))
                {
                    nuevaLista[contador] = productos[i];
                    contador++;
                }
            }
            return nuevaLista;
        }

        private void tableLayoutPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (selected)
            {
                this.BackColor = UserControl.DefaultBackColor;
            }
            else
            {
                this.BackColor = Color.CadetBlue;
            }
            selected = !selected;
        }
    }
}
