using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenCvSharp;
namespace WordGenerator.Controls.DMD
{
    public partial class FlattopSelected : UserControl
    {
        public FlattopSelected()
        {
            InitializeComponent();
        }

        private object getVaraible(NumericUpDown upDown)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => { getVaraible(upDown); }));
                return 0;
            }
            else
            {
                return (object)upDown.Value;

            }
        }
        public Mat getImage(int w, int h)
        {
            double wrat = Convert.ToDouble(getVaraible(numericUpDown1)) /100;
            double hrat = Convert.ToDouble(getVaraible(numericUpDown2)) /100;
            double inten = Convert.ToDouble(getVaraible(numericUpDown3));
            int dw = (int)(wrat * w);
            int dh = (int)(hrat * h);
            int offsetw = (int)((1.0 - wrat) / 2.0 * w);
            int offseth = (int)((1.0 - hrat) / 2.0 * h);
            Mat mat;
            mat = Mat.Zeros(h, w, MatType.CV_32FC1);
            Cv2.Rectangle(mat, new Rect(offsetw, offseth, (int)dw, (int)dh), new Scalar(inten, inten, inten), Cv2.FILLED);
            if (checkBoxReverse.Checked)
                // mat = 1 - mat;
                Cv2.Subtract(mat, new Scalar(1), mat);

            return mat;
        }
    }
}
