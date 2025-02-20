namespace Dproject
{
    partial class CustomWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonLineImage = new System.Windows.Forms.Button();
            this.buttonFlattopImage = new System.Windows.Forms.Button();
            this.buttonDotImage = new System.Windows.Forms.Button();
            this.lineSelected1 = new WordGenerator.Controls.DMD.LineSelected();
            this.flattopSelected1 = new WordGenerator.Controls.DMD.FlattopSelected();
            this.dotSelected1 = new WordGenerator.Controls.DMD.DotSelected();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dotSelected1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(262, 222);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dot";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.flattopSelected1);
            this.groupBox2.Location = new System.Drawing.Point(280, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(260, 222);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Flattop";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lineSelected1);
            this.groupBox3.Location = new System.Drawing.Point(546, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(266, 222);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Line";
            // 
            // buttonLineImage
            // 
            this.buttonLineImage.Location = new System.Drawing.Point(821, 181);
            this.buttonLineImage.Name = "buttonLineImage";
            this.buttonLineImage.Size = new System.Drawing.Size(75, 53);
            this.buttonLineImage.TabIndex = 5;
            this.buttonLineImage.Text = "Line Image";
            this.buttonLineImage.UseVisualStyleBackColor = true;
            this.buttonLineImage.Click += new System.EventHandler(this.buttonLineImage_Click);
            // 
            // buttonFlattopImage
            // 
            this.buttonFlattopImage.Location = new System.Drawing.Point(821, 101);
            this.buttonFlattopImage.Name = "buttonFlattopImage";
            this.buttonFlattopImage.Size = new System.Drawing.Size(75, 53);
            this.buttonFlattopImage.TabIndex = 6;
            this.buttonFlattopImage.Text = "Flattop Image";
            this.buttonFlattopImage.UseVisualStyleBackColor = true;
            this.buttonFlattopImage.Click += new System.EventHandler(this.buttonFlattopImage_Click);
            // 
            // buttonDotImage
            // 
            this.buttonDotImage.Location = new System.Drawing.Point(821, 19);
            this.buttonDotImage.Name = "buttonDotImage";
            this.buttonDotImage.Size = new System.Drawing.Size(75, 53);
            this.buttonDotImage.TabIndex = 7;
            this.buttonDotImage.Text = "Dot Image";
            this.buttonDotImage.UseVisualStyleBackColor = true;
            this.buttonDotImage.Click += new System.EventHandler(this.buttonDotImage_Click);
            // 
            // lineSelected1
            // 
            this.lineSelected1.Location = new System.Drawing.Point(6, 20);
            this.lineSelected1.Name = "lineSelected1";
            this.lineSelected1.Size = new System.Drawing.Size(252, 180);
            this.lineSelected1.TabIndex = 0;
            // 
            // flattopSelected1
            // 
            this.flattopSelected1.Location = new System.Drawing.Point(6, 20);
            this.flattopSelected1.Name = "flattopSelected1";
            this.flattopSelected1.Size = new System.Drawing.Size(250, 110);
            this.flattopSelected1.TabIndex = 0;
            // 
            // dotSelected1
            // 
            this.dotSelected1.Location = new System.Drawing.Point(6, 20);
            this.dotSelected1.Name = "dotSelected1";
            this.dotSelected1.Size = new System.Drawing.Size(250, 122);
            this.dotSelected1.TabIndex = 0;
            this.dotSelected1.Load += new System.EventHandler(this.dotSelected1_Load);
            // 
            // CustomWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 491);
            this.Controls.Add(this.buttonDotImage);
            this.Controls.Add(this.buttonFlattopImage);
            this.Controls.Add(this.buttonLineImage);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "CustomWindow";
            this.Text = "CustomWindow";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private WordGenerator.Controls.DMD.DotSelected dotSelected1;
        private System.Windows.Forms.GroupBox groupBox2;
        private WordGenerator.Controls.DMD.FlattopSelected flattopSelected1;
        private System.Windows.Forms.GroupBox groupBox3;
        private WordGenerator.Controls.DMD.LineSelected lineSelected1;
        private System.Windows.Forms.Button buttonLineImage;
        private System.Windows.Forms.Button buttonFlattopImage;
        private System.Windows.Forms.Button buttonDotImage;
    }
}