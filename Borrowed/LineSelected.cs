using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenCvSharp;
using System.IO;

namespace WordGenerator.Controls.DMD
{
    public partial class LineSelected: UserControl
    {
        public LineSelected()
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

                double wid = Convert.ToDouble(getVaraible(numericUpDown1));
                double dx = Convert.ToDouble(getVaraible(numericUpDown2));
                double x0 = Convert.ToDouble(getVaraible(numericUpDown3));
                double angle = Convert.ToDouble(getVaraible(numericUpDown4));
                double rad = Convert.ToDouble(getVaraible(numericUpDown5));
                double phx = x0;
                double phy = 0;
                int intensity = Convert.ToInt32(getVaraible(numericUpDown6));
                if ((int)wid < 1 || (int)dx < 1 || wid > dx) return null;
                Mat mat = Mat.Zeros(1, 4 * w, MatType.CV_32FC1);
                for (int row = 0; row < 4 * w; row++)
                {
                    for (int col = 0; col < 1; col++)
                    {
                        mat.Set<float>(col, row, (float)Math.Cos(2 * Math.PI * row / dx));
                    }
                }


                mat = Cv2.Repeat(mat, 4 * h, 1);
                //        Rect rect = new Rect(0, 0, (int)w/2, h);
                //   Cv2.Rectangle(mat, rect, Scalar.White, Cv2.FILLED);
                //          mat = Cv2.Repeat(mat, 4, 4*(int)(w / dx) + 1);

                float c = (float)Math.Cos(angle / 180 * Math.PI);
                float s = (float)Math.Sin(angle / 180 * Math.PI);
                Console.WriteLine("phases");
                Console.WriteLine(phx);
                Console.WriteLine(phy);

                float[,] farr2 = { { c, s, (-c) * h - w * s + c * (float)phx + s * (float)phy }, { -s, c, s * h + (-c) * w - s * (float)phx + c * (float)phy } };
                Console.WriteLine(c * (float)phx - s * (float)phy);
                InputArray affinemat2 = InputArray.Create<float>(farr2);
                Cv2.WarpAffine(mat, mat, affinemat2, new OpenCvSharp.Size(w, h));

                double th = Math.Cos(2 * Math.PI * wid / 2.0 / dx);
                for (int row = 0; row < w; row++)
                {
                    for (int col = 0; col < h; col++)
                    {
                        var v = mat.Get<float>(col, row);
                        if (v < th)
                            mat.Set<float>(col, row, (float)0);
                        if (v >= th)
                            mat.Set<float>(col, row, (float)intensity);
                    }
                }
                Cv2.Circle(mat, new OpenCvSharp.Point(w / 2, h / 2), (int)rad, Scalar.White, 20);

            return mat;
            
        }
    }
}
