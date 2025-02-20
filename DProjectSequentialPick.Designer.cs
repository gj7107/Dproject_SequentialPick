using System.Windows.Forms;

namespace DProject
{
    partial class DProjectForm240905
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DProjectForm240905));
            this.bnInit = new System.Windows.Forms.Button();
            this.bnSeqLoad = new System.Windows.Forms.Button();
            this.bnHalt = new System.Windows.Forms.Button();
            this.bnCleanUp = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txResult = new System.Windows.Forms.TextBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.buttonProjSeqs = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBoxTF = new System.Windows.Forms.GroupBox();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.textBoxTFread = new System.Windows.Forms.TextBox();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonWatch = new System.Windows.Forms.Button();
            this.buttonLoadTF = new System.Windows.Forms.Button();
            this.textBoxTF = new System.Windows.Forms.TextBox();
            this.groupBoxMask = new System.Windows.Forms.GroupBox();
            this.pictureBoxMask = new System.Windows.Forms.PictureBox();
            this.buttonMaskLoad = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxMulti = new System.Windows.Forms.TextBox();
            this.textBoxBinMode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxBinUnint = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxIll = new System.Windows.Forms.TextBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.buttonToBot = new System.Windows.Forms.Button();
            this.buttonImgs = new System.Windows.Forms.Button();
            this.labelImageType = new System.Windows.Forms.Label();
            this.txtbox_loadseq = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.labelmaxnum = new System.Windows.Forms.Label();
            this.textBoxMaxRandom = new System.Windows.Forms.TextBox();
            this.buttonResetPick = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDownReplaceAt = new System.Windows.Forms.NumericUpDown();
            this.buttonRandomPick = new System.Windows.Forms.Button();
            this.textBoxPickFileName = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.debounceTimer = new System.Timers.Timer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxPicker = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBoxTF.SuspendLayout();
            this.groupBoxMask.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMask)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReplaceAt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.debounceTimer)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bnInit
            // 
            this.bnInit.Location = new System.Drawing.Point(17, 21);
            this.bnInit.Name = "bnInit";
            this.bnInit.Size = new System.Drawing.Size(120, 28);
            this.bnInit.TabIndex = 0;
            this.bnInit.Text = "Init";
            this.bnInit.UseVisualStyleBackColor = true;
            this.bnInit.Click += new System.EventHandler(this.bnInit_Click);
            // 
            // bnSeqLoad
            // 
            this.bnSeqLoad.Location = new System.Drawing.Point(15, 24);
            this.bnSeqLoad.Name = "bnSeqLoad";
            this.bnSeqLoad.Size = new System.Drawing.Size(86, 28);
            this.bnSeqLoad.TabIndex = 1;
            this.bnSeqLoad.Text = "Load Seq";
            this.bnSeqLoad.UseVisualStyleBackColor = true;
            this.bnSeqLoad.Click += new System.EventHandler(this.bnSeq1_Click);
            // 
            // bnHalt
            // 
            this.bnHalt.Location = new System.Drawing.Point(17, 55);
            this.bnHalt.Name = "bnHalt";
            this.bnHalt.Size = new System.Drawing.Size(120, 28);
            this.bnHalt.TabIndex = 3;
            this.bnHalt.Text = "Halt";
            this.bnHalt.UseVisualStyleBackColor = true;
            this.bnHalt.Click += new System.EventHandler(this.bnHalt_Click);
            // 
            // bnCleanUp
            // 
            this.bnCleanUp.Location = new System.Drawing.Point(17, 89);
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
            this.label1.Location = new System.Drawing.Point(18, 544);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Result:";
            // 
            // txResult
            // 
            this.txResult.Location = new System.Drawing.Point(68, 540);
            this.txResult.Name = "txResult";
            this.txResult.ReadOnly = true;
            this.txResult.Size = new System.Drawing.Size(531, 21);
            this.txResult.TabIndex = 6;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(20, 123);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(65, 16);
            this.radioButton1.TabIndex = 7;
            this.radioButton1.Text = "Trig On";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(21, 145);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(64, 16);
            this.radioButton2.TabIndex = 8;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Trig Off";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // buttonProjSeqs
            // 
            this.buttonProjSeqs.Location = new System.Drawing.Point(17, 248);
            this.buttonProjSeqs.Name = "buttonProjSeqs";
            this.buttonProjSeqs.Size = new System.Drawing.Size(120, 28);
            this.buttonProjSeqs.TabIndex = 9;
            this.buttonProjSeqs.Text = "Proj Seqs";
            this.buttonProjSeqs.UseVisualStyleBackColor = true;
            this.buttonProjSeqs.Click += new System.EventHandler(this.buttonProjSeqs_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DisplayMember = "0";
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "EDGE_RISING",
            "EDGE_FALLING"});
            this.comboBox1.Location = new System.Drawing.Point(20, 167);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(117, 20);
            this.comboBox1.TabIndex = 10;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(164, 123);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(432, 327);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(72, 219);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(65, 21);
            this.textBox1.TabIndex = 12;
            this.textBox1.Text = "3334";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 222);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "Pic Timing";
            // 
            // groupBoxTF
            // 
            this.groupBoxTF.Controls.Add(this.buttonSelect);
            this.groupBoxTF.Controls.Add(this.textBoxTFread);
            this.groupBoxTF.Controls.Add(this.buttonStop);
            this.groupBoxTF.Controls.Add(this.buttonWatch);
            this.groupBoxTF.Controls.Add(this.buttonLoadTF);
            this.groupBoxTF.Controls.Add(this.textBoxTF);
            this.groupBoxTF.Location = new System.Drawing.Point(20, 455);
            this.groupBoxTF.Name = "groupBoxTF";
            this.groupBoxTF.Size = new System.Drawing.Size(445, 82);
            this.groupBoxTF.TabIndex = 14;
            this.groupBoxTF.TabStop = false;
            this.groupBoxTF.Text = "Transform";
            // 
            // buttonSelect
            // 
            this.buttonSelect.Location = new System.Drawing.Point(6, 49);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(65, 23);
            this.buttonSelect.TabIndex = 6;
            this.buttonSelect.Text = "Select";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // textBoxTFread
            // 
            this.textBoxTFread.Font = new System.Drawing.Font("±¼¸²", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBoxTFread.Location = new System.Drawing.Point(284, 30);
            this.textBoxTFread.Name = "textBoxTFread";
            this.textBoxTFread.ReadOnly = true;
            this.textBoxTFread.Size = new System.Drawing.Size(151, 35);
            this.textBoxTFread.TabIndex = 5;
            this.textBoxTFread.Text = "(0,0)";
            this.textBoxTFread.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(213, 49);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(65, 23);
            this.buttonStop.TabIndex = 4;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonWatch
            // 
            this.buttonWatch.Location = new System.Drawing.Point(144, 49);
            this.buttonWatch.Name = "buttonWatch";
            this.buttonWatch.Size = new System.Drawing.Size(65, 23);
            this.buttonWatch.TabIndex = 3;
            this.buttonWatch.Text = "Watch";
            this.buttonWatch.UseVisualStyleBackColor = true;
            this.buttonWatch.Click += new System.EventHandler(this.buttonWatch_Click);
            // 
            // buttonLoadTF
            // 
            this.buttonLoadTF.Location = new System.Drawing.Point(75, 49);
            this.buttonLoadTF.Name = "buttonLoadTF";
            this.buttonLoadTF.Size = new System.Drawing.Size(65, 23);
            this.buttonLoadTF.TabIndex = 2;
            this.buttonLoadTF.Text = "Load";
            this.buttonLoadTF.UseVisualStyleBackColor = true;
            this.buttonLoadTF.Click += new System.EventHandler(this.buttonLoadTF_Click);
            // 
            // textBoxTF
            // 
            this.textBoxTF.Location = new System.Drawing.Point(6, 20);
            this.textBoxTF.Name = "textBoxTF";
            this.textBoxTF.Size = new System.Drawing.Size(271, 21);
            this.textBoxTF.TabIndex = 0;
            this.textBoxTF.Text = "Z:\\LatticePhaseLock\\LatticePhaseTest.txt";
            // 
            // groupBoxMask
            // 
            this.groupBoxMask.Controls.Add(this.pictureBoxMask);
            this.groupBoxMask.Controls.Add(this.buttonMaskLoad);
            this.groupBoxMask.Location = new System.Drawing.Point(611, 21);
            this.groupBoxMask.Name = "groupBoxMask";
            this.groupBoxMask.Size = new System.Drawing.Size(236, 230);
            this.groupBoxMask.TabIndex = 15;
            this.groupBoxMask.TabStop = false;
            this.groupBoxMask.Text = "Mask";
            // 
            // pictureBoxMask
            // 
            this.pictureBoxMask.Location = new System.Drawing.Point(6, 20);
            this.pictureBoxMask.Name = "pictureBoxMask";
            this.pictureBoxMask.Size = new System.Drawing.Size(224, 167);
            this.pictureBoxMask.TabIndex = 16;
            this.pictureBoxMask.TabStop = false;
            // 
            // buttonMaskLoad
            // 
            this.buttonMaskLoad.Location = new System.Drawing.Point(6, 193);
            this.buttonMaskLoad.Name = "buttonMaskLoad";
            this.buttonMaskLoad.Size = new System.Drawing.Size(224, 28);
            this.buttonMaskLoad.TabIndex = 1;
            this.buttonMaskLoad.Text = "Load Mask Img";
            this.buttonMaskLoad.UseVisualStyleBackColor = true;
            this.buttonMaskLoad.Click += new System.EventHandler(this.buttonMaskLoad_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxMulti);
            this.groupBox1.Location = new System.Drawing.Point(611, 257);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(236, 53);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Multiplier";
            // 
            // textBoxMulti
            // 
            this.textBoxMulti.Location = new System.Drawing.Point(6, 20);
            this.textBoxMulti.Name = "textBoxMulti";
            this.textBoxMulti.Size = new System.Drawing.Size(82, 21);
            this.textBoxMulti.TabIndex = 0;
            this.textBoxMulti.Text = "1.0";
            this.textBoxMulti.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxBinMode
            // 
            this.textBoxBinMode.Location = new System.Drawing.Point(7, 325);
            this.textBoxBinMode.Multiline = true;
            this.textBoxBinMode.Name = "textBoxBinMode";
            this.textBoxBinMode.ReadOnly = true;
            this.textBoxBinMode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxBinMode.Size = new System.Drawing.Size(130, 110);
            this.textBoxBinMode.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 307);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "BinMode:";
            // 
            // checkBoxBinUnint
            // 
            this.checkBoxBinUnint.AutoSize = true;
            this.checkBoxBinUnint.Checked = true;
            this.checkBoxBinUnint.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxBinUnint.Location = new System.Drawing.Point(2, 282);
            this.checkBoxBinUnint.Name = "checkBoxBinUnint";
            this.checkBoxBinUnint.Size = new System.Drawing.Size(138, 16);
            this.checkBoxBinUnint.TabIndex = 19;
            this.checkBoxBinUnint.Text = "Binary Uninterrupted";
            this.checkBoxBinUnint.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 196);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 12);
            this.label4.TabIndex = 21;
            this.label4.Text = "Ill Timing";
            // 
            // textBoxIll
            // 
            this.textBoxIll.Location = new System.Drawing.Point(72, 192);
            this.textBoxIll.Name = "textBoxIll";
            this.textBoxIll.Size = new System.Drawing.Size(65, 21);
            this.textBoxIll.TabIndex = 20;
            this.textBoxIll.Text = "0";
            this.textBoxIll.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(344, 84);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(40, 21);
            this.numericUpDown1.TabIndex = 22;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // buttonToBot
            // 
            this.buttonToBot.Location = new System.Drawing.Point(390, 83);
            this.buttonToBot.Name = "buttonToBot";
            this.buttonToBot.Size = new System.Drawing.Size(55, 23);
            this.buttonToBot.TabIndex = 23;
            this.buttonToBot.Text = "To Bot";
            this.buttonToBot.UseVisualStyleBackColor = true;
            this.buttonToBot.Click += new System.EventHandler(this.buttonToBot_Click);
            // 
            // buttonImgs
            // 
            this.buttonImgs.Location = new System.Drawing.Point(611, 332);
            this.buttonImgs.Name = "buttonImgs";
            this.buttonImgs.Size = new System.Drawing.Size(105, 23);
            this.buttonImgs.TabIndex = 29;
            this.buttonImgs.Text = "Built-in Images";
            this.buttonImgs.UseVisualStyleBackColor = true;
            this.buttonImgs.Click += new System.EventHandler(this.buttonImgs_Click);
            // 
            // labelImageType
            // 
            this.labelImageType.AutoSize = true;
            this.labelImageType.Location = new System.Drawing.Point(781, 337);
            this.labelImageType.Name = "labelImageType";
            this.labelImageType.Size = new System.Drawing.Size(60, 12);
            this.labelImageType.TabIndex = 30;
            this.labelImageType.Text = "No Image";
            // 
            // txtbox_loadseq
            // 
            this.txtbox_loadseq.Location = new System.Drawing.Point(117, 29);
            this.txtbox_loadseq.Name = "txtbox_loadseq";
            this.txtbox_loadseq.Size = new System.Drawing.Size(328, 21);
            this.txtbox_loadseq.TabIndex = 35;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.pictureBox3);
            this.groupBox6.Location = new System.Drawing.Point(611, 361);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(210, 194);
            this.groupBox6.TabIndex = 39;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Image in Zyla";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(3, 20);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(200, 160);
            this.pictureBox3.TabIndex = 36;
            this.pictureBox3.TabStop = false;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.labelmaxnum);
            this.groupBox8.Controls.Add(this.textBoxMaxRandom);
            this.groupBox8.Controls.Add(this.buttonResetPick);
            this.groupBox8.Controls.Add(this.label5);
            this.groupBox8.Controls.Add(this.numericUpDownReplaceAt);
            this.groupBox8.Controls.Add(this.buttonRandomPick);
            this.groupBox8.Controls.Add(this.textBoxPickFileName);
            this.groupBox8.Controls.Add(this.label23);
            this.groupBox8.Controls.Add(this.bnSeqLoad);
            this.groupBox8.Controls.Add(this.numericUpDown1);
            this.groupBox8.Controls.Add(this.buttonToBot);
            this.groupBox8.Controls.Add(this.txtbox_loadseq);
            this.groupBox8.Location = new System.Drawing.Point(149, 5);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(451, 112);
            this.groupBox8.TabIndex = 60;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Sequence Setting";
            // 
            // labelmaxnum
            // 
            this.labelmaxnum.AutoSize = true;
            this.labelmaxnum.Location = new System.Drawing.Point(153, 89);
            this.labelmaxnum.Name = "labelmaxnum";
            this.labelmaxnum.Size = new System.Drawing.Size(30, 12);
            this.labelmaxnum.TabIndex = 60;
            this.labelmaxnum.Text = "Max";
            // 
            // textBoxMaxRandom
            // 
            this.textBoxMaxRandom.Enabled = false;
            this.textBoxMaxRandom.Location = new System.Drawing.Point(189, 84);
            this.textBoxMaxRandom.Name = "textBoxMaxRandom";
            this.textBoxMaxRandom.Size = new System.Drawing.Size(56, 21);
            this.textBoxMaxRandom.TabIndex = 59;
            // 
            // buttonResetPick
            // 
            this.buttonResetPick.Location = new System.Drawing.Point(388, 58);
            this.buttonResetPick.Name = "buttonResetPick";
            this.buttonResetPick.Size = new System.Drawing.Size(57, 21);
            this.buttonResetPick.TabIndex = 58;
            this.buttonResetPick.Text = "Reset";
            this.buttonResetPick.UseVisualStyleBackColor = true;
            this.buttonResetPick.Click += new System.EventHandler(this.buttonResetPick_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 89);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 12);
            this.label5.TabIndex = 57;
            this.label5.Text = "Replace At";
            // 
            // numericUpDownReplaceAt
            // 
            this.numericUpDownReplaceAt.Enabled = false;
            this.numericUpDownReplaceAt.Location = new System.Drawing.Point(90, 85);
            this.numericUpDownReplaceAt.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericUpDownReplaceAt.Name = "numericUpDownReplaceAt";
            this.numericUpDownReplaceAt.Size = new System.Drawing.Size(40, 21);
            this.numericUpDownReplaceAt.TabIndex = 56;
            // 
            // buttonRandomPick
            // 
            this.buttonRandomPick.Location = new System.Drawing.Point(15, 58);
            this.buttonRandomPick.Name = "buttonRandomPick";
            this.buttonRandomPick.Size = new System.Drawing.Size(96, 21);
            this.buttonRandomPick.TabIndex = 55;
            this.buttonRandomPick.Text = "Pick path";
            this.buttonRandomPick.UseVisualStyleBackColor = true;
            this.buttonRandomPick.Click += new System.EventHandler(this.buttonPick_Click);
            // 
            // textBoxPickFileName
            // 
            this.textBoxPickFileName.Enabled = false;
            this.textBoxPickFileName.Location = new System.Drawing.Point(117, 58);
            this.textBoxPickFileName.Name = "textBoxPickFileName";
            this.textBoxPickFileName.Size = new System.Drawing.Size(267, 21);
            this.textBoxPickFileName.TabIndex = 54;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(251, 86);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(89, 12);
            this.label23.TabIndex = 52;
            this.label23.Text = "Image Number";
            // 
            // debounceTimer
            // 
            this.debounceTimer.AutoReset = false;
            this.debounceTimer.Enabled = true;
            this.debounceTimer.Interval = 1000D;
            this.debounceTimer.SynchronizingObject = this;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBoxPicker);
            this.groupBox2.Location = new System.Drawing.Point(471, 456);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(123, 78);
            this.groupBox2.TabIndex = 40;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Picker";
            // 
            // comboBoxPicker
            // 
            this.comboBoxPicker.DisplayMember = "0";
            this.comboBoxPicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPicker.FormattingEnabled = true;
            this.comboBoxPicker.Items.AddRange(new object[] {
            "NONE",
            "RANDOM",
            "SEQUENTIAL"});
            this.comboBoxPicker.Location = new System.Drawing.Point(6, 19);
            this.comboBoxPicker.Name = "comboBoxPicker";
            this.comboBoxPicker.Size = new System.Drawing.Size(111, 20);
            this.comboBoxPicker.TabIndex = 61;
            // 
            // DProjectForm240905
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 569);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.labelImageType);
            this.Controls.Add(this.buttonImgs);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxIll);
            this.Controls.Add(this.checkBoxBinUnint);
            this.Controls.Add(this.textBoxBinMode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxMask);
            this.Controls.Add(this.groupBoxTF);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.buttonProjSeqs);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.txResult);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bnCleanUp);
            this.Controls.Add(this.bnHalt);
            this.Controls.Add(this.bnInit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DProjectForm240905";
            this.Text = "DProject";
            this.Load += new System.EventHandler(this.AlpSampleForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBoxTF.ResumeLayout(false);
            this.groupBoxTF.PerformLayout();
            this.groupBoxMask.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMask)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReplaceAt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.debounceTimer)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bnInit;
        private System.Windows.Forms.Button bnSeqLoad;
        private System.Windows.Forms.Button bnHalt;
        private System.Windows.Forms.Button bnCleanUp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txResult;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Button buttonProjSeqs;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBoxTF;
        private System.Windows.Forms.TextBox textBoxTF;
        private System.Windows.Forms.GroupBox groupBoxMask;
        private System.Windows.Forms.PictureBox pictureBoxMask;
        private System.Windows.Forms.Button buttonMaskLoad;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonLoadTF;
        private System.Windows.Forms.Button buttonWatch;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.TextBox textBoxTFread;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxMulti;
        private System.Windows.Forms.TextBox textBoxBinMode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxBinUnint;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxIll;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button buttonToBot;
        private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.Button buttonImgs;
        private System.Windows.Forms.Label labelImageType;
        private System.Windows.Forms.TextBox txtbox_loadseq;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label label23;
        private Button buttonResetPick;
        private Label label5;
        private NumericUpDown numericUpDownReplaceAt;
        private Button buttonRandomPick;
        private TextBox textBoxPickFileName;
        private TextBox textBoxMaxRandom;
        private Label labelmaxnum;
        private GroupBox groupBox2;
        private ComboBox comboBoxPicker;
    }
}

