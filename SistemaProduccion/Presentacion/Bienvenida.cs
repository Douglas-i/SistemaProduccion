using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaProduccion
{
    public partial class Bienvenida : Form
    {
        public Bienvenida()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Value += 1;
            if(Opacity<1) 
            {
                Opacity += 0.05;
            }
            if(progressBar1.Value==100)
            {
                timer1.Stop();
                timer2.Start();

            }
        }

        private void Bienvenida_Load(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            Opacity = 0;
            timer1.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Opacity -= 0.1;
            if (Opacity==0)
            {
                timer2.Stop();
                Close();
            }
        }
    }
}
