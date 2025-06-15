using System;
using System.Drawing;
using System.Windows.Forms;

namespace OtobusOtomasyonSistemi
{
    public class TransparentLabel : Label
    {
        private byte alpha = 250;

        public byte Alpha
        {
            get { return alpha; }
            set
            {
                alpha = value;
                this.Invalidate(); // Label'i yeniden boyamak için
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (Brush brush = new SolidBrush(Color.FromArgb(alpha, this.ForeColor)))
            {
                e.Graphics.DrawString(this.Text, this.Font, brush, new PointF(0, 0));
            }
        }
    }
}