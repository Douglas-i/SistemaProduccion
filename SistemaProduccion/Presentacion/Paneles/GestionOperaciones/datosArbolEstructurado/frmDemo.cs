using System;
using System.Drawing;
using System.Windows.Forms;

using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace SistemaProduccion.Presentacion.Paneles.TmpPanelAlex.datosArbolEstructurado
{
    partial class frmDemo : Form
    {
        //Make a borderless form movable
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        //Make a borderless form movable

        public frmDemo(TreeNode<PictureNode> root)
        {
            InitializeComponent();
            this.root = root;
            this.label2.Text = this.root.Data.Description;
        }
        // The root node.
        private TreeNode<PictureNode> root =
            new TreeNode<PictureNode>(
                new PictureNode("(1) Producto A",
                    Properties.Resources.Product));

        // Make a tree.
        private void Form1_Load(object sender, EventArgs e)
        {
            TreeNode<PictureNode> productoX =
                new TreeNode<PictureNode>(
                    new PictureNode("(1) Producto X",
                        Properties.Resources.Product));
            TreeNode<PictureNode> productoY =
                new TreeNode<PictureNode>(
                    new PictureNode("(1) Producto Y",
                        Properties.Resources.Product));
            //TreeNode<PictureNode> andrew =
            //    new TreeNode<PictureNode>(
            //        new PictureNode("Prince Andrew, Duke of York",
            //            Properties.Resources.Product));
            //TreeNode<PictureNode> edward =
            //    new TreeNode<PictureNode>(
            //        new PictureNode("Prince Edward, Earl of Wessex",
            //            Properties.Resources.Product));
            //TreeNode<PictureNode> william =
            //    new TreeNode<PictureNode>(
            //        new PictureNode("Prince William",
            //            Properties.Resources.Product));
            //TreeNode<PictureNode> harry =
            //    new TreeNode<PictureNode>(
            //        new PictureNode("Prince Henry (Harry)",
            //            Properties.Resources.Product));
            TreeNode<PictureNode> peter =
                new TreeNode<PictureNode>(
                    new PictureNode("(1) Producto X",
                        Properties.Resources.Product));
            TreeNode<PictureNode> zara =
                new TreeNode<PictureNode>(
                    new PictureNode("(1) Producto ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ",
                        Properties.Resources.Product));
            //TreeNode<PictureNode> beatrice =
            //    new TreeNode<PictureNode>(
            //        new PictureNode("Princess Beatrice",
            //            Properties.Resources.Product));
            //TreeNode<PictureNode> eugenie =
            //    new TreeNode<PictureNode>(
            //        new PictureNode("Princess Eugenie",
            //            Properties.Resources.Product));
            //TreeNode<PictureNode> louise =
            //    new TreeNode<PictureNode>(
            //        new PictureNode("Lady Louise",
            //            Properties.Resources.Product));
            //TreeNode<PictureNode> severn =
            //    new TreeNode<PictureNode>(
            //        new PictureNode("Viscount Severn",
            //            Properties.Resources.Product));

            //root.AddChild(productoX);
            //charles.AddChild(william);
            //charles.AddChild(harry);
            //root.AddChild(productoY);
            //productoY.AddChild(peter);
            //productoY.AddChild(zara);
            //root.AddChild(andrew);
            //andrew.AddChild(beatrice);
            //andrew.AddChild(eugenie);
            //root.AddChild(edward);
            //edward.AddChild(louise);
            //edward.AddChild(severn);

            // Arrange the tree.
            //ArrangeTree();
            
            // Draw the tree into a Bitmap and
            // display the result in the PictureBox.
            picTree.Location = new Point(0, 0);
            picTree.SizeMode = PictureBoxSizeMode.AutoSize;
            picTree.Image = DrawTree(root, 5);
        }        

        private Bitmap DrawTree(TreeNode<PictureNode> root, int margin)
        {
            float xmin = margin, ymin = margin;

            // Make a small bitmap so we can use its graphics handle.
            using (Bitmap bm = new Bitmap(10, 10))
            {
                using (Graphics gr = Graphics.FromImage(bm))
                {
                    // Arrange the tree to see how big it is.
                    root.Arrange(gr, ref xmin, ref ymin);
                }
            }

            // Make the result bitmap.
            int wid = (int)xmin + margin;
            int hgt = (int)ymin + margin;
            Bitmap result_bm = new Bitmap(wid, hgt);
            using (Graphics gr = Graphics.FromImage(result_bm))
            {
                // Draw the tree.
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                root.DrawTree(gr);
            }

            return result_bm;
        }

        // Draw the tree.
        private void picTree_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            root.DrawTree(e.Graphics);
        }

        // Center the tree on the form.
        private void picTree_Resize(object sender, EventArgs e)
        {
            //ArrangeTree();
        }
        private void ArrangeTree()
        {
            using (Graphics gr = picTree.CreateGraphics())
            {
                // Arrange the tree once to see how big it is.
                float xmin = 0, ymin = 0;
                root.Arrange(gr, ref xmin, ref ymin);

                // Arrange the tree again to center it horizontally.
                xmin = (this.ClientSize.Width - xmin) / 2;
                ymin = 10;
                root.Arrange(gr, ref xmin, ref ymin);
            }

            picTree.Refresh();
        }

        // The currently selected node.
        private TreeNode<PictureNode> SelectedNode;

        private void picTree_MouseClick(object sender, MouseEventArgs e)
        {
            FindNodeUnderMouse(e.Location);
        }

        // Set SelectedNode to the node under the mouse.
        private void FindNodeUnderMouse(PointF pt)
        {
            // Deselect the previously selected node.
            if (SelectedNode != null)
            {
                SelectedNode.Data.Selected = false;
                //lblNodeText.Text = "";
            }

            // Find the node at this position (if any).
            using (Graphics gr = picTree.CreateGraphics())
            {
                SelectedNode = root.NodeAtPoint(gr, pt);
            }

            // Select the node.
            if (SelectedNode != null)
            {
                SelectedNode.Data.Selected = true;
                //lblNodeText.Text = SelectedNode.Data.Description;
            }

            // Redraw.
            picTree.Refresh();
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String nombreArchivo=Environment.CurrentDirectory + "/arbol-estructurado.png";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "arbol-estructurado.png";
            saveFileDialog.Filter = "Archivos de imágenes (*.png)|*.png|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                nombreArchivo = saveFileDialog.FileName;
                this.picTree.Image.Save(nombreArchivo);
                MessageBox.Show("Guardado en " + nombreArchivo);
                System.Diagnostics.Process.Start(@nombreArchivo);
            }            
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            this.Location = e.Location;
        }

        private void btnCerrar_MouseLeave(object sender, EventArgs e)
        {
            this.btnCerrar.BackColor = Color.Transparent;
        }

        private void btnCerrar_MouseMove(object sender, MouseEventArgs e)
        {
            this.btnCerrar.BackColor = Color.FromArgb(76, 109, 139);
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            int height = Screen.PrimaryScreen.WorkingArea.Height;
            int width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Size = new Size(width, height);
            this.Location = new Point(0, 0);
            this.bunifuImageButton1.Visible = false;
        }
    }    
}
