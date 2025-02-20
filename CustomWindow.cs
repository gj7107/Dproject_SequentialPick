using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dproject;
using System.IO;
using OpenCvSharp.Extensions;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Dproject
{
    public partial class CustomWindow : Form
    {
        public Mat mat = null;
        public string str_imgtype = "No Image";
        int w = 1024, h = 768;
        public CustomWindow()
        {
            InitializeComponent();
        }

     

        private void buttonDotImage_Click(object sender, EventArgs e)
        {
            mat = dotSelected1.getImage(w, h);
            str_imgtype = "Dot";
       
        }

        private void buttonFlattopImage_Click(object sender, EventArgs e)
        {
            mat = flattopSelected1.getImage(w, h);
            str_imgtype = "Flattop";
        }

        private void buttonLineImage_Click(object sender, EventArgs e)
        {
            mat = lineSelected1.getImage(w, h);
            str_imgtype = "Line";
            
        }


       

        private void dotSelected1_Load(object sender, EventArgs e)
        {

        }

   
        

    }
}
