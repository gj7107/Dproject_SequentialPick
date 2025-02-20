using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ButtonTest
{
    public partial class Draw : Form
    {
        bool bColorClick = false;
        bool bTextBoxClick = false;
        int iColorValue = 0;
        double dColorValue = 0;
        int[] iaColorArr = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public double[,] dataVar;
        public string result = "";
        public int dataSize = L_textbox;

        public Draw()
        {
            InitializeComponent();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox tb = (TextBox)sender;
            tb.ForeColor = Color.Black;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            int BoxSize = 30;
            int BoxSpacing = 5;
            this.groupBox1.Size = new Size(6 + (BoxSize + BoxSpacing) * L_textbox, 20 + (BoxSize + BoxSpacing) * L_textbox);
            for (int i = 0; i < L_textbox; i++)
            {
                for (int j = 0; j < L_textbox; j++)
                {
                    this.textBoxs[i, j] = new System.Windows.Forms.TextBox();
                }
            }

           
            // 
            // groupBox1
            // 
            for (int i = 0; i < L_textbox; i++)
            {
                for (int j = 0; j < L_textbox; j++)
                {
                    this.groupBox1.Controls.Add(this.textBoxs[i, j]);
                }
            }


            // 
            // textBox1
            // 
            for (int i = 0; i < L_textbox; i++)
            {
                for (int j = 0; j < L_textbox; j++)
                {
                    this.textBoxs[i, j].BackColor = Color.FromArgb(0, 0, 0);
                    this.textBoxs[i, j].Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                    this.textBoxs[i, j].Location = new System.Drawing.Point(6 + (BoxSize + BoxSpacing) * i, 20 + (BoxSize + BoxSpacing) * j);
                    this.textBoxs[i, j].MaximumSize = new System.Drawing.Size(BoxSize, BoxSize);
                    this.textBoxs[i, j].MinimumSize = new System.Drawing.Size(BoxSize, BoxSize);
                    this.textBoxs[i, j].Name = "textBox" + (i * L_textbox + j).ToString();
                    this.textBoxs[i, j].Size = new System.Drawing.Size(BoxSize, BoxSize);
                    this.textBoxs[i, j].AutoSize = false;
                    this.textBoxs[i, j].TextAlign = HorizontalAlignment.Center;
                    this.textBoxs[i, j].ForeColor = Color.Transparent;
                    this.textBoxs[i, j].Text = "";
                    this.textBoxs[i, j].TabIndex = 2;
                    this.textBoxs[i, j].TextChanged += new System.EventHandler(this.textBox_TextChanged);
                    this.textBoxs[i, j].MouseDown += new MouseEventHandler(this.textBox_MouseDown);
                    this.textBoxs[i, j].MouseUp += new MouseEventHandler(this.textBox_MouseUp);
                    this.textBoxs[i, j].LostFocus += new EventHandler(this.textBox_LostFocus);
                    this.textBoxs[i, j].MouseEnter += new EventHandler(this.textBox_MouseEnter);
                }
            }
            dataVar = new double[L_textbox, L_textbox];
        }

        private Button btt_selectcsv;
        private Button btt_Loadcsv;
        private TextBox txt_Load;

        string filePath;
       
        private void btt_selectcsv_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv| jpg files (*.jpg)|*.jpg";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
                txt_Load.Text = filePath;
            }
        }

        
        private void btt_Loadcsv_Click(object sender, EventArgs e)
        {
            List<List<double>> data = new List<List<double>>();

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] values = line.Split(',');
                    List<double> rowlist = new List<double>();

                    for (int col = 0; col < values.Length; col++)
                    {
                        if (double.TryParse(values[col], out double value))
                        {
                            rowlist.Add(value);
                        }

                    }
                    data.Add(rowlist);
                }
            }

            int rowCount = data.Count;
            int colCount = data.Count > 0 ? data[0].Count : 0;
            arrcsv = new double[rowCount, colCount];
            arrcsvtext = new string[rowCount, colCount];

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    arrcsv[row, col] = data[col][row];
                }
            }
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    arrcsvtext[row, col] = Convert.ToString(arrcsv[row, col]);
                }
            }



            for (int i = 0; i < L_textbox; i++)
            {
                for (int j = 0; j < L_textbox; j++)
                {
                    textBoxs[i, j].Text = arrcsvtext[i, j];
                    dColorValue = arrcsv[i, j];
                    iColorValue = (int)(arrcsv[i, j] * 255);
                    int colorval = Convert.ToInt16(iColorValue);
                    textBoxs[i, j].BackColor = Color.FromArgb(colorval, colorval, colorval);
                    textBoxs[i, j].ForeColor = Color.FromArgb(colorval, colorval, colorval);
                }
            }
        }
        double[,] arrcsv;
        string[,] arrcsvtext;

        private void textBox_MouseEnter(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox tb = (TextBox)sender;

            if (bColorClick)
            {
                int colorval = Convert.ToInt16(iColorValue);
                tb.Text = dColorValue.ToString();
                tb.BackColor = Color.FromArgb(colorval, colorval, colorval);
                tb.ForeColor = Color.FromArgb(colorval, colorval, colorval);
            }
        }

        private void textBox_MouseDown(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.TextBox tb = (TextBox)sender;
            if (e.Button == MouseButtons.Right && bColorClick)
            {
                bColorClick = false;

                button1.FlatAppearance.BorderColor = Color.Black;
                button2.FlatAppearance.BorderColor = Color.Black;
                button3.FlatAppearance.BorderColor = Color.Black;
                button4.FlatAppearance.BorderColor = Color.Black;
                button5.FlatAppearance.BorderColor = Color.Black;
                button6.FlatAppearance.BorderColor = Color.Black;
                button7.FlatAppearance.BorderColor = Color.Black;
                button8.FlatAppearance.BorderColor = Color.Black;
                button9.FlatAppearance.BorderColor = Color.Black;
                button10.FlatAppearance.BorderColor = Color.Black;
                button11.FlatAppearance.BorderColor = Color.Black;
                button12.FlatAppearance.BorderColor = Color.Black;

            } else if(e.Button == MouseButtons.Left)
            {
                tb.ForeColor = Color.Red;
                bTextBoxClick = true;
            }

        }
        private void textBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                bTextBoxClick = false;
            }
        }

        private void textBox_LostFocus(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox tb = (TextBox)sender;
            String txt = tb.Text;
            if (double.TryParse(txt, out double val) && val <= 10.0 && val >= 0.0)
            {
                if(val > 1.0 && val <=10.0)
                {
                    val = val / 10;
                }
                tb.Text = Convert.ToString(val);
                int colorval = Convert.ToInt16((val) * 255);
                tb.BackColor = Color.FromArgb(colorval, colorval, colorval);
                tb.ForeColor = Color.FromArgb(colorval, colorval, colorval);
            }
            else
            {
                tb.Text = "";
                tb.BackColor = Color.FromArgb(255, 255, 255);
                tb.ForeColor = Color.FromArgb(255, 255, 255);
            }
          
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonImgClear = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.btt_selectcsv = new System.Windows.Forms.Button();
            this.btt_Loadcsv = new System.Windows.Forms.Button();
            this.txt_Load = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(531, 545);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            this.groupBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.onFormClick);
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter_1);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.button1.FlatAppearance.BorderSize = 2;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("굴림", 6F);
            this.button1.Location = new System.Drawing.Point(568, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "1.0";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.button2.FlatAppearance.BorderSize = 2;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("굴림", 6F);
            this.button2.Location = new System.Drawing.Point(568, 60);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(30, 30);
            this.button2.TabIndex = 2;
            this.button2.Text = "0.9";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.button3.FlatAppearance.BorderSize = 2;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("굴림", 6F);
            this.button3.Location = new System.Drawing.Point(568, 96);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(30, 30);
            this.button3.TabIndex = 3;
            this.button3.Text = "0.8";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(178)))), ((int)(((byte)(178)))));
            this.button4.FlatAppearance.BorderSize = 2;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("굴림", 6F);
            this.button4.Location = new System.Drawing.Point(568, 132);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(30, 30);
            this.button4.TabIndex = 4;
            this.button4.Text = "0.7";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.button5.FlatAppearance.BorderSize = 2;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("굴림", 6F);
            this.button5.Location = new System.Drawing.Point(568, 168);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(30, 30);
            this.button5.TabIndex = 5;
            this.button5.Text = "0.6";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.button6.FlatAppearance.BorderSize = 2;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("굴림", 6F);
            this.button6.Location = new System.Drawing.Point(568, 204);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(30, 30);
            this.button6.TabIndex = 6;
            this.button6.Text = "0.5";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button_Click);
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.button7.FlatAppearance.BorderSize = 2;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Font = new System.Drawing.Font("굴림", 6F);
            this.button7.Location = new System.Drawing.Point(568, 240);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(30, 30);
            this.button7.TabIndex = 7;
            this.button7.Text = "0.4";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button_Click);
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.button8.FlatAppearance.BorderSize = 2;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Font = new System.Drawing.Font("굴림", 6F);
            this.button8.Location = new System.Drawing.Point(568, 276);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(30, 30);
            this.button8.TabIndex = 8;
            this.button8.Text = "0.3";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button_Click);
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.button9.FlatAppearance.BorderSize = 2;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Font = new System.Drawing.Font("굴림", 6F);
            this.button9.Location = new System.Drawing.Point(568, 312);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(30, 30);
            this.button9.TabIndex = 9;
            this.button9.Text = "0.2";
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.button_Click);
            // 
            // button10
            // 
            this.button10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.button10.FlatAppearance.BorderSize = 2;
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button10.Font = new System.Drawing.Font("굴림", 6F);
            this.button10.Location = new System.Drawing.Point(568, 348);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(30, 30);
            this.button10.TabIndex = 10;
            this.button10.Text = "0.1";
            this.button10.UseVisualStyleBackColor = false;
            this.button10.Click += new System.EventHandler(this.button_Click);
            // 
            // button11
            // 
            this.button11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button11.FlatAppearance.BorderSize = 2;
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11.Font = new System.Drawing.Font("굴림", 6F);
            this.button11.Location = new System.Drawing.Point(568, 384);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(30, 30);
            this.button11.TabIndex = 11;
            this.button11.Text = "0.0";
            this.button11.UseVisualStyleBackColor = false;
            this.button11.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(568, 456);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(66, 23);
            this.buttonClear.TabIndex = 12;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(604, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "1.0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(604, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "0.9";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(604, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "0.8";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(604, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "0.7";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(604, 177);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "0.6";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(604, 213);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "0.5";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(604, 249);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(21, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "0.4";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(604, 285);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(21, 12);
            this.label8.TabIndex = 13;
            this.label8.Text = "0.3";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(604, 321);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 12);
            this.label9.TabIndex = 13;
            this.label9.Text = "0.2";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(604, 357);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(21, 12);
            this.label10.TabIndex = 13;
            this.label10.Text = "0.1";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(604, 393);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(21, 12);
            this.label11.TabIndex = 13;
            this.label11.Text = "0.0";
            // 
            // button12
            // 
            this.button12.BackColor = System.Drawing.Color.Black;
            this.button12.FlatAppearance.BorderSize = 2;
            this.button12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button12.Font = new System.Drawing.Font("굴림", 6F);
            this.button12.Location = new System.Drawing.Point(568, 420);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(30, 30);
            this.button12.TabIndex = 14;
            this.button12.Text = "1.0";
            this.button12.UseVisualStyleBackColor = false;
            this.button12.Click += new System.EventHandler(this.button_Click);
            // 
            // button13
            // 
            this.button13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.button13.Location = new System.Drawing.Point(641, 420);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(58, 30);
            this.button13.TabIndex = 14;
            this.button13.Text = "Set";
            this.button13.UseVisualStyleBackColor = false;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(604, 426);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(30, 21);
            this.textBox1.TabIndex = 15;
            this.textBox1.Text = "0.0";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // buttonImgClear
            // 
            this.buttonImgClear.Location = new System.Drawing.Point(568, 484);
            this.buttonImgClear.Name = "buttonImgClear";
            this.buttonImgClear.Size = new System.Drawing.Size(92, 37);
            this.buttonImgClear.TabIndex = 16;
            this.buttonImgClear.Text = "Image Clear";
            this.buttonImgClear.UseVisualStyleBackColor = true;
            this.buttonImgClear.Click += new System.EventHandler(this.buttonImgClear_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(568, 527);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(92, 37);
            this.buttonSave.TabIndex = 17;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // btt_selectcsv
            // 
            this.btt_selectcsv.Location = new System.Drawing.Point(641, 126);
            this.btt_selectcsv.Name = "btt_selectcsv";
            this.btt_selectcsv.Size = new System.Drawing.Size(77, 27);
            this.btt_selectcsv.TabIndex = 18;
            this.btt_selectcsv.Text = "Select File";
            this.btt_selectcsv.UseVisualStyleBackColor = true;
            this.btt_selectcsv.Click += new System.EventHandler(this.btt_selectcsv_Click);
            // 
            // btt_Loadcsv
            // 
            this.btt_Loadcsv.Location = new System.Drawing.Point(724, 126);
            this.btt_Loadcsv.Name = "btt_Loadcsv";
            this.btt_Loadcsv.Size = new System.Drawing.Size(64, 27);
            this.btt_Loadcsv.TabIndex = 19;
            this.btt_Loadcsv.Text = "Load";
            this.btt_Loadcsv.UseVisualStyleBackColor = true;
            this.btt_Loadcsv.Click += new System.EventHandler(this.btt_Loadcsv_Click);
            // 
            // txt_Load
            // 
            this.txt_Load.Location = new System.Drawing.Point(641, 71);
            this.txt_Load.Multiline = true;
            this.txt_Load.Name = "txt_Load";
            this.txt_Load.Size = new System.Drawing.Size(147, 46);
            this.txt_Load.TabIndex = 20;
            // 
            // Draw
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 572);
            this.Controls.Add(this.txt_Load);
            this.Controls.Add(this.btt_Loadcsv);
            this.Controls.Add(this.btt_selectcsv);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonImgClear);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Draw";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.onFormClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void onFormClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right && bColorClick)
            {
                bColorClick = false;

                button1.FlatAppearance.BorderColor = Color.Black;
                button2.FlatAppearance.BorderColor = Color.Black;
                button3.FlatAppearance.BorderColor = Color.Black;
                button4.FlatAppearance.BorderColor = Color.Black;
                button5.FlatAppearance.BorderColor = Color.Black;
                button6.FlatAppearance.BorderColor = Color.Black;
                button7.FlatAppearance.BorderColor = Color.Black;
                button8.FlatAppearance.BorderColor = Color.Black;
                button9.FlatAppearance.BorderColor = Color.Black;
                button10.FlatAppearance.BorderColor = Color.Black;
                button11.FlatAppearance.BorderColor = Color.Black;
                button12.FlatAppearance.BorderColor = Color.Black;

            }

        }

        static int L_textbox = 15;

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox[,] textBoxs = new System.Windows.Forms.TextBox[L_textbox, L_textbox];
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;
        private Button button9;
        private Button button10;
        private Button button11;

        private void button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (!bColorClick)
            {
                button1.FlatAppearance.BorderColor = Color.Black;
                button2.FlatAppearance.BorderColor = Color.Black;
                button3.FlatAppearance.BorderColor = Color.Black;
                button4.FlatAppearance.BorderColor = Color.Black;
                button5.FlatAppearance.BorderColor = Color.Black;
                button6.FlatAppearance.BorderColor = Color.Black;
                button7.FlatAppearance.BorderColor = Color.Black;
                button8.FlatAppearance.BorderColor = Color.Black;
                button9.FlatAppearance.BorderColor = Color.Black;
                button10.FlatAppearance.BorderColor = Color.Black;
                button11.FlatAppearance.BorderColor = Color.Black;
                button12.FlatAppearance.BorderColor = Color.Black;

                btn.FlatAppearance.BorderColor = Color.Red;
                iColorValue = btn.BackColor.R;
                dColorValue = Convert.ToDouble(btn.Text);
                bColorClick = true;
                //for(int i = 0; i < L_textbox; i++)
                //{
                //    for(int j = 0; j < L_textbox; j++)
                //    {
                //        textBoxs[i, j].Enabled = false;
                //    }
                //}
            }
            else
            {

                button1.FlatAppearance.BorderColor = Color.Black;
                button2.FlatAppearance.BorderColor = Color.Black;
                button3.FlatAppearance.BorderColor = Color.Black;
                button4.FlatAppearance.BorderColor = Color.Black;
                button5.FlatAppearance.BorderColor = Color.Black;
                button6.FlatAppearance.BorderColor = Color.Black;
                button7.FlatAppearance.BorderColor = Color.Black;
                button8.FlatAppearance.BorderColor = Color.Black;
                button9.FlatAppearance.BorderColor = Color.Black;
                button10.FlatAppearance.BorderColor = Color.Black;
                button11.FlatAppearance.BorderColor = Color.Black;
                button12.FlatAppearance.BorderColor = Color.Black;

                btn.FlatAppearance.BorderColor = Color.Black;
                iColorValue = btn.BackColor.R;
                bColorClick = false;
                //for (int i = 0; i < L_textbox; i++)
                //{
                //    for (int j = 0; j < L_textbox; j++)
                //    {
                //        textBoxs[i, j].Enabled = true;
                //    }
                //}
            }
        }

        private Button buttonClear;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private Button button12;

        private void button13_Click(object sender, EventArgs e)
        {
            String txt = textBox1.Text;
            if (double.TryParse(txt, out double val) && val <= 1.0 && val >= 0.0)
            {
                int colorval = Convert.ToInt16((val) * 255);
                button12.BackColor = Color.FromArgb(colorval, colorval, colorval);
                button12.Text = val.ToString();
            }
            else
            {
                textBox1.Text = "1.0";
                button12.Text = "1.0";
                button12.BackColor = Color.FromArgb(0,0,0);
            }
        }

        private Button button13;
        private TextBox textBox1;

        private void buttonClear_Click(object sender, EventArgs e)
        {
            button1.FlatAppearance.BorderColor = Color.Black;
            button2.FlatAppearance.BorderColor = Color.Black;
            button3.FlatAppearance.BorderColor = Color.Black;
            button4.FlatAppearance.BorderColor = Color.Black;
            button5.FlatAppearance.BorderColor = Color.Black;
            button6.FlatAppearance.BorderColor = Color.Black;
            button7.FlatAppearance.BorderColor = Color.Black;
            button8.FlatAppearance.BorderColor = Color.Black;
            button9.FlatAppearance.BorderColor = Color.Black;
            button10.FlatAppearance.BorderColor = Color.Black;
            button11.FlatAppearance.BorderColor = Color.Black;
            button12.FlatAppearance.BorderColor = Color.Black;

            bColorClick = false;
        }



        private Button buttonImgClear;

        private void buttonImgClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < L_textbox; i++)
            {
                for (int j = 0; j < L_textbox; j++)
                {
                    textBoxs[i, j].Text = "0.0";
                    textBoxs[i, j].BackColor = Color.FromArgb(0, 0, 0);
                }
            }
        }




        private Button buttonSave;

        private void buttonSave_Click(object sender, EventArgs e)
        { 
           for (int i = 0; i < L_textbox; i++)
           {
               for (int j = 0; j < L_textbox; j++)
               {
                    if (textBoxs[j, i].Text == "")
                    {
                        dataVar[i, j] = 0.0;
                        Console.Write($"{dataVar[i,j]}");
                    }
                    else
                    {
                        dataVar[i, j] = Convert.ToDouble(textBoxs[j, i].Text);
                        Console.Write($"{dataVar[i, j]}");
                    }
               }
               Console.WriteLine();
           }
            Console.WriteLine();
            
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        

        protected override void OnFormClosing(FormClosingEventArgs e)
        {        
            for (int i = 0; i < L_textbox; i++)
            {
               for (int j = 0; j < L_textbox; j++)
               {
                   if (textBoxs[i, j].Text == "")
                       dataVar[i, j] = 0;
                   else
                       dataVar[i, j] = Convert.ToDouble(textBoxs[i, j].Text);
               }
            }
                base.OnFormClosing(e);           
        }

        
     
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter_1(object sender, EventArgs e)
        {

        }

        private void btt_Testcsv_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < arrcsvtext.GetLength(0); i++)
            {
                for (int j = 0; j < arrcsvtext.GetLength(1); j++)
                {
                    Console.Write($"{arrcsvtext[i, j]}  ");
                }
                Console.WriteLine();
            }
        }

       
    }

}
