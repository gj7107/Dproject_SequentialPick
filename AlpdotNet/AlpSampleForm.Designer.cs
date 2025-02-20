namespace ALP_CSharp_DotNet
{
    partial class AlpSampleForm
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
            this.bnInit = new System.Windows.Forms.Button();
            this.bnSeq1 = new System.Windows.Forms.Button();
            this.bnSeq2 = new System.Windows.Forms.Button();
            this.bnHalt = new System.Windows.Forms.Button();
            this.bnCleanUp = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txResult = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // bnInit
            // 
            this.bnInit.Location = new System.Drawing.Point(18, 21);
            this.bnInit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bnInit.Name = "bnInit";
            this.bnInit.Size = new System.Drawing.Size(120, 28);
            this.bnInit.TabIndex = 0;
            this.bnInit.Text = "Init";
            this.bnInit.UseVisualStyleBackColor = true;
            this.bnInit.Click += new System.EventHandler(this.bnInit_Click);
            // 
            // bnSeq1
            // 
            this.bnSeq1.Location = new System.Drawing.Point(18, 54);
            this.bnSeq1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bnSeq1.Name = "bnSeq1";
            this.bnSeq1.Size = new System.Drawing.Size(120, 28);
            this.bnSeq1.TabIndex = 1;
            this.bnSeq1.Text = "Proj Seq1";
            this.bnSeq1.UseVisualStyleBackColor = true;
            this.bnSeq1.Click += new System.EventHandler(this.bnSeq1_Click);
            // 
            // bnSeq2
            // 
            this.bnSeq2.Location = new System.Drawing.Point(14, 88);
            this.bnSeq2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bnSeq2.Name = "bnSeq2";
            this.bnSeq2.Size = new System.Drawing.Size(120, 28);
            this.bnSeq2.TabIndex = 2;
            this.bnSeq2.Text = "Proj Seq2";
            this.bnSeq2.UseVisualStyleBackColor = true;
            this.bnSeq2.Click += new System.EventHandler(this.bnSeq2_Click);
            // 
            // bnHalt
            // 
            this.bnHalt.Location = new System.Drawing.Point(14, 121);
            this.bnHalt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bnHalt.Name = "bnHalt";
            this.bnHalt.Size = new System.Drawing.Size(120, 28);
            this.bnHalt.TabIndex = 3;
            this.bnHalt.Text = "Halt";
            this.bnHalt.UseVisualStyleBackColor = true;
            this.bnHalt.Click += new System.EventHandler(this.bnHalt_Click);
            // 
            // bnCleanUp
            // 
            this.bnCleanUp.Location = new System.Drawing.Point(145, 21);
            this.bnCleanUp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bnCleanUp.Name = "bnCleanUp";
            this.bnCleanUp.Size = new System.Drawing.Size(120, 28);
            this.bnCleanUp.TabIndex = 4;
            this.bnCleanUp.Text = "Clean Up";
            this.bnCleanUp.UseVisualStyleBackColor = true;
            this.bnCleanUp.Click += new System.EventHandler(this.bnCleanUp_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 161);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Result:";
            // 
            // txResult
            // 
            this.txResult.Location = new System.Drawing.Point(71, 154);
            this.txResult.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txResult.Name = "txResult";
            this.txResult.ReadOnly = true;
            this.txResult.Size = new System.Drawing.Size(193, 21);
            this.txResult.TabIndex = 6;
            // 
            // AlpSampleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 190);
            this.Controls.Add(this.txResult);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bnCleanUp);
            this.Controls.Add(this.bnHalt);
            this.Controls.Add(this.bnSeq2);
            this.Controls.Add(this.bnSeq1);
            this.Controls.Add(this.bnInit);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "AlpSampleForm";
            this.Text = "AlpTest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bnInit;
        private System.Windows.Forms.Button bnSeq1;
        private System.Windows.Forms.Button bnSeq2;
        private System.Windows.Forms.Button bnHalt;
        private System.Windows.Forms.Button bnCleanUp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txResult;
    }
}

