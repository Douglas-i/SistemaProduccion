using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

namespace SistemaProduccion.Presentacion.Paneles.TmpPanelAlex.datosArbolEstructurado
{
    class PictureNode : IDrawable
    {
        // Constructor.
        public Image Picture = null;
        public string Description;
        public int Cantidad;
        public bool Selected = false;
        public PictureNode(string description,int cantidad)
        {            
            this.Description = description;
            this.Cantidad = cantidad;
            Picture = SistemaProduccion.Properties.Resources.Product;
            String text = "(" + Cantidad + ") " + Description;
            int ancho = Picture.Width;
            ancho += (text.Length * 10);
            NodeSize = new SizeF(ancho, 50);
        }

        public PictureNode(string description, Image picture)
        {
            Description = description;
            Picture = picture;
            int ancho = Picture.Width;
            ancho += (Description.Length * 10);
            NodeSize = new SizeF(ancho, 50);
        }

        // The size of the drawn rectangles.
        //public SizeF NodeSize = new SizeF(150, 50);
        public SizeF NodeSize;

        // For testing.
        //private static Random Rand = new Random();

        // Return the size needed by this node.
        public SizeF GetSize(Graphics gr, Font font)
        {
            return NodeSize;
        }

        // Return a RectangleF giving the node's location.
        private RectangleF Location(PointF center)
        {
            return new RectangleF(
                center.X - NodeSize.Width / 2,
                center.Y - NodeSize.Height / 2,
                NodeSize.Width, NodeSize.Height);
        }

        // Return True if the target is under this node.
        public bool IsAtPoint(Graphics gr, Font font, PointF center_pt, PointF target_pt)
        {
            RectangleF rect = Location(center_pt);
            return rect.Contains(target_pt);
        }

        // Draw the person.
        public void Draw(float x, float y, Graphics gr, Pen pen, Brush bg_brush, Brush text_brush, Font font)
        {
            // Draw a border.
            RectangleF rectf = Location(new PointF(x, y));
            Rectangle rect = Rectangle.Round(rectf);
            if (Selected)
            {                
                gr.FillRectangle(new SolidBrush(Color.FromArgb(76, 109, 139)), rect);
                ControlPaint.DrawBorder3D(gr, rect,Border3DStyle.Sunken);
            }
            else
            {
                gr.FillRectangle(new SolidBrush(Color.FromArgb(223, 235, 247)), rect);
                ControlPaint.DrawBorder3D(gr, rect,Border3DStyle.Raised);
            }

            // Draw the picture.
            rectf.Inflate(-5, -5);
            rectf = PositionImage(Picture, rectf);
            gr.DrawImage(Picture, rectf);
            Font arialFont = new Font("Arial", 10);
            float ancho=x;
            ancho+=Picture.Width;
            String text = "(" + Cantidad + ") " + Description;
            ancho -= (text.Length * 4);
            PointF location = new PointF(ancho, y-10);
            if (Selected)
                gr.DrawString(text, arialFont, Brushes.White, location);
            else
                gr.DrawString(text, arialFont, Brushes.Black, location);
        }

        // Find a rectangle to draw the image centered in the
        // rectangle as large as possible without stretching.
        private RectangleF PositionImage(Image picture, RectangleF rect)
        {
            // Get the X and Y scales.
            float pic_wid = picture.Width;
            float pic_hgt = picture.Height;
            float pic_aspect = pic_wid / pic_hgt;
            float rect_aspect = rect.Width / rect.Height;
            float scale = 1;
            if (pic_aspect > rect_aspect)
            {
                scale = rect.Width / pic_wid;
            }
            else
            {
                scale = rect.Height / pic_hgt;
            }

            // See where we need to draw.
            pic_wid *= scale;
            pic_hgt *= scale;
            RectangleF drawing_rect = new RectangleF(
                //rect.X + (rect.Width - pic_wid) / 2,
                //rect.Y + (rect.Height - pic_hgt) / 2,
                rect.X,
                rect.Y,
                pic_wid, pic_hgt);
            return drawing_rect;
        }
    }
}
