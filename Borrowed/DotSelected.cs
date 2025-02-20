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
    public partial class DotSelected: UserControl
    {
        public DotSelected()
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
            double x0 = Convert.ToDouble(getVaraible(numericUpDown1));
            double y0 = Convert.ToDouble(getVaraible(numericUpDown2));
            double rad = Convert.ToDouble(getVaraible(numericUpDown3));
            Mat mat;
            if (checkBox1.Checked)
            {
                mat = Mat.Ones(h, w, MatType.CV_32FC1)*255;
                Cv2.Circle(mat, new OpenCvSharp.Point(x0, y0), (int)rad, Scalar.Black, Cv2.FILLED);
            } else
            {
                mat = Mat.Zeros(h, w, MatType.CV_32FC1);
                Cv2.Circle(mat, new OpenCvSharp.Point(x0, y0), (int)rad, Scalar.White, Cv2.FILLED);
            }
            return mat;
        }
        


    }
}
