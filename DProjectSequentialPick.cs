using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Dproject;
using OpenCvSharp;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using OpenCvSharp.Extensions;
using ButtonTest;
using System.Runtime.InteropServices.ComTypes;
using System.Data.SqlClient;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Timers;

namespace DProject
{
    public partial class DProjectForm240905 : Form
    {
        UInt32 m_DevId, m_SeqId1;
        Int32 m_DmdWidth = 1024, m_DmdHeight = 768;
        public List<Mat> fileContents = null;
        public List<Mat> filelistContents = null;
        // List<List<Mat>> Imageseqlists = null;
        public int fileContents_length = 0;
        Mat mat_mask;
        double OffsetX = 0.0, OffsetY = 0.0;
        //Thread thread1 = null;
        FileSystemWatcher watcher;
        private object e;

        public List<string> filenamelist = null;

        // Path for Image Load
        string prevpath_Image = "";
        string prevpath_Mask = "";
        string prevpath_Phase = "";
        string prevpath_LattFolder = "";
        string prevpath_ImgList= "";

        // Path for Image sequence flip
        // string SeqflipPath = "";
        private FileSystemWatcher fileSystemWatcher;
        // private HashSet<string> processedFiles = new HashSet<string>(); // 처리된 파일 경로 저장
        // private readonly object lockObject = new object(); // 스레드 안전을 위한 객체
        private System.Timers.Timer debounceTimer;


        // Lattice Variables
        Mat matLat = null;
        string str_lattpath = "";

        // Load seq lists
//        List<List<int>> intLists;
        int SeqRepeat = 0;

        public DProjectForm240905()
        {
            InitializeComponent();
            fileContents = new List<Mat>();
            filenamelist = new List<string>();
            filelistContents = new List<Mat>();
            m_DmdWidth = 1024; m_DmdHeight = 768;
            m_DevId = UInt32.MaxValue;
        }

        // Convert error string
        private string AlpErrorString(AlpImport.Result result)
        {
            return String.Format("{0}", result);
        }

        private void bnInit_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                Action safeProject = delegate { bnInit_Click(sender, e); };
                this.Invoke(safeProject);
            }
            else
            {
                AlpImport.Result result;

                // allocate one ALP device
                result = AlpImport.DevAlloc(0, 0, ref m_DevId);
                txResult.Text = "DevAlloc " + AlpErrorString(result);
                if (AlpImport.Result.ALP_OK != result) return;  // error -> exit

                // determine image data size by DMD type
                Int32 DmdType = Int32.MaxValue;
                m_DmdWidth = 0; m_DmdHeight = 0;
                AlpImport.DevInquire(m_DevId, AlpImport.DevTypes.ALP_DEV_DMDTYPE, ref DmdType);
                AlpImport.DevInquire(m_DevId, AlpImport.DevTypes.ALP_DEV_DISPLAY_WIDTH, ref m_DmdWidth);
                AlpImport.DevInquire(m_DevId, AlpImport.DevTypes.ALP_DEV_DISPLAY_HEIGHT, ref m_DmdHeight);
                switch ((AlpImport.DmdTypes)DmdType)
                {
                    case AlpImport.DmdTypes.ALP_DMDTYPE_XGA:
                    case AlpImport.DmdTypes.ALP_DMDTYPE_XGA_055A:
                    case AlpImport.DmdTypes.ALP_DMDTYPE_XGA_055X:
                    case AlpImport.DmdTypes.ALP_DMDTYPE_XGA_07A:
                        txResult.Text = String.Format("XGA DMD {0}", DmdType);
                        m_DmdWidth = 1024;  // fall-back: old API versions did not support ALP_DEV_DISPLAY_WIDTH and _HEIGHT
                        m_DmdHeight = 768;
                        break;
                    case AlpImport.DmdTypes.ALP_DMDTYPE_1080P_095A:
                    case AlpImport.DmdTypes.ALP_DMDTYPE_DISCONNECT:
                        txResult.Text = String.Format("1080p DMD {0}", DmdType);
                        break;
                    case AlpImport.DmdTypes.ALP_DMDTYPE_WUXGA_096A:
                        txResult.Text = String.Format("WUXGA DMD {0}", DmdType);
                        break;
                    case AlpImport.DmdTypes.ALP_DMDTYPE_SXGA_PLUS:
                        txResult.Text = String.Format("SXGA+ DMD {0}", DmdType);
                        m_DmdWidth = 1400;
                        m_DmdHeight = 1050;
                        break;
                    default:
                        txResult.Text = String.Format("Unknown DMD Type {0}: {1}x{2}", DmdType, m_DmdWidth, m_DmdHeight);
                        if (m_DmdHeight == 0 || m_DmdWidth == 0)
                            // Clean up... AlpImport.DevHalt(m_DevId); m_DevId = UInt32.MaxValue;
                            return;
                        else
                            // Continue, because at least the API DLL knows this DMD type :-)
                            break;
                }

                // (Not available prior to ALP-4)
                result = AlpImport.DevControlEx_SynchGate(m_DevId, 1, true, 1, 0, 0);
                //result = AlpImport.DevControlEx_SynchGate(m_DevId, 2, true, 1, 0, 0);
                //result = AlpImport.DevControlEx_SynchGate(m_DevId, 3, true, 1, 0, 0);

                txResult.Text = "SynchGate1 " + AlpErrorString(result);
                if (AlpImport.Result.ALP_OK != result) return;  // error -> exit

                bnHalt.Enabled = true;
                bnSeqLoad.Enabled = true;
                bnCleanUp.Enabled = true;
                buttonProjSeqs.Enabled = true;
            }

        }

        //string[] Imagearray;
        //string[,]Imagelistlog = new string[20, 20];

        private void bnSeq1_Click(object sender, EventArgs e)
        {         
            var filePath = string.Empty;
            using (OpenFileDialog openfiledialog = new OpenFileDialog())
            {
                openfiledialog.InitialDirectory = prevpath_Image;
                //openfiledialog.Filter = "jpg files (*.jpg)|*.jpg|bmp files (*.bmp)|*.bmp|binary files (*.bin)|*.bin|tiff files (*.tiff)|*.tiff";
                openfiledialog.Filter = "Image Files (BMP,TIFF)|*.bmp;*.tiff;*.tif|Image Files(JPG,PNG,GIF)|*.jpg;*.png;*.gif";
                openfiledialog.FilterIndex = 1;
                openfiledialog.Multiselect = true;
                openfiledialog.RestoreDirectory = true;           

                if (openfiledialog.ShowDialog() == DialogResult.OK && openfiledialog.FileNames.Length > 0) // 이미지 가져와서 실행
                {
                    labelImageType.Text = "No Image";
                    //ClearImageArray(fileContents);
                    //fileContents = new Image[openfiledialog.FileNames.Length];
                    fileContents.Clear(); // 리스트 fileContents 초기화
                    filenamelist.Clear();
                    int i = 0; Image fileContent = null; // 사진 fileContent 선언
                    foreach (string path in openfiledialog.FileNames) // 각각의 경로에 있는 파일들에 대한 루프
                    {
                        fileContent = (Image)Image.FromFile(path).Clone(); // 사진 fileContent는 경로 path에 있는 사진
                        //fileContents[i] = ResizeImage(fileContent, m_DmdWidth, m_DmdHeight);
                        //fileContent.Dispose();
                        //i++;

                        if (fileContent.Width != m_DmdWidth && fileContent.Height != m_DmdHeight) // file Content의 사이즈를 DMD에 맞게 조절
                            fileContent = ResizeImage(fileContent, m_DmdWidth, m_DmdHeight);


                        Mat m = ToMat((Bitmap)fileContent); // 변환된 사진 fileContent를 비트맵 이미지에서 Mat m으로 변환
                        m.ConvertTo(m, MatType.CV_32FC1, 1 / 255.0); // Mat m의 규격화 및 형식 설정
                        fileContents.Add(m); // 리스트 fileContents에 사진 fileContent로부터 얻은 Mat m 추가
                        i++; // i 추가하고 루프 돌기
                        
                        string imageName = Path.GetFileName(path);
                        txtbox_loadseq.Text = imageName;
                        filenamelist.Add(imageName);
                    }
                    prevpath_Image = Path.GetDirectoryName(openfiledialog.FileName); // Set Path                     
                    fileContents_length = i; // i는 루프의 횟수, 즉 선택한 파일의 개수
                    numericUpDown1.Maximum = fileContents_length - 1;
                    numericUpDownReplaceAt.Maximum = fileContents_length - 1;

                    if (pictureBox1.Image != null)
                    {
                        try { pictureBox1.Image.Dispose(); } catch { }; //picturebox에 이미지 넣기
                    }
                    if (fileContent != null)
                        pictureBox1.Image = ResizeImage(fileContent, pictureBox1.Width, pictureBox1.Height);
                    else
                        txResult.Text = "Err!"; 
                    
                    /*
                    if (Imageseqlists[0].Count != 0 && listnumber != 0 && fileContents.Count != Imageseqlists[0].Count)
                    {
                        MessageBox.Show("Select same number("+Convert.ToString(Imageseqlists[0].Count) + ") of images with sequence.", "Number Mismatching");
                    }
                    */

                }
                /*
                List<Mat> fileContentsCopy = new List<Mat>();
                Mat clonedList = new Mat();

                foreach (Mat mat in fileContents)
                {
                    clonedList = mat.Clone();
                    fileContentsCopy.Add(clonedList);
                }
                Imageseqlists.RemoveAt(listnumber);
                Imageseqlists.Insert(listnumber, fileContentsCopy);
                */

            }
        }

         //int Seqcount = 0;
        //int Seqnumber = 0;
        //int SeqRepeat=1;

        public void projDMD()
        {
            if (this.InvokeRequired)
            {
                Action safeProject = delegate { projDMD(); };
                this.Invoke(safeProject);
            }
            else
            {
                Mat imgsum, inputimg;
                Byte[] inputseq;
                AlpImport.Result result;

                if (m_DmdWidth < m_DmdHeight)
                {
                    Console.WriteLine("This function is implemented only for W > H case.");
                    return;
                }

                byte[] FSarr = null;
                double multi_factor = Convert.ToDouble(textBoxMulti.Text);

                textBoxBinMode.Text = "Current time : ";
                textBoxBinMode.Text += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                textBoxBinMode.Text += "\n";

                //                int Seqnumber = Convert.ToInt32(textBoxSeqNum.Text);
                //int Seqcount = (int)numericUpDownIterNum.Value;
                int Seqcount = 0;
                int Seqnumber = 0;
                SeqRepeat = 0;
 //               if (SeqRepeat != 0) Seqnumber = Seqnumber % SeqRepeat;

                /* CHANGE HERE */

                int currfileContents_length = 0;
                List<Mat> currfileContents = null;

                if (SeqRepeat == 0)
                {
                    currfileContents_length = fileContents_length;
                    currfileContents = fileContents;
                }
                /*
                else
                {
                    if (Seqnumber >= SeqRepeat)
                    {
                        MessageBox.Show("Number in the list " + Convert.ToString(Seqnumber) + "exceeds the Repeat number " + Convert.ToString(SeqRepeat));
                        return;
                    }

                    currfileContents = new List<Mat>();

                    // currfileContents.Clear(); // 리스트 fileContents 초기화
                    foreach (int imgidx in intLists[Seqnumber]) // 각각의 경로에 있는 파일들에 대한 루프
                    {
                        if (imgidx >= fileContents_length)
                        {
                            MessageBox.Show("Number in the list " + Convert.ToString(imgidx) + " exceeds the Repeat number " + Convert.ToString(fileContents_length));
                            return;
                        }
                        textBoxBinMode.Text += "Img " + imgidx.ToString() + " Load ... ";

                        Mat m = fileContents[imgidx].Clone(); // 변환된 사진 fileContent를 비트맵 이미지에서 Mat m으로 변환
                        textBoxBinMode.Text += "Mat prepared... ";

                        currfileContents.Add(m); // 리스트 fileContents에 사진 fileContent로부터 얻은 Mat m 추가
                        currfileContents_length++; // i 추가하고 루프 돌기
                        textBoxBinMode.Text += "Success\n";
                    }
                }*/

                if(textBoxPickFileName.Text != "")
                {
                    if (!File.Exists(textBoxPickFileName.Text))
                    {
                        textBoxPickFileName.Text = "No filename";
                    }
                    else
                    {
                        //Random random = new Random();
                        string basepath = Path.GetDirectoryName(textBoxPickFileName.Text);
                        string fname = Path.GetFileNameWithoutExtension(textBoxPickFileName.Text);
                        string fext = Path.GetExtension(textBoxPickFileName.Text);
                        string[] parts = fname.Split('_');

                        string fpath = textBoxPickFileName.Text;

                        if (File.Exists(fpath))
                        {
                            textBoxPickFileName.Text = fpath;
                            currfileContents_length = fileContents_length;
                            Image fileContent = (Image)Image.FromFile(fpath).Clone(); // 사진 fileContent는 경로 path에 있는 사진

                            if (fileContent.Width != m_DmdWidth && fileContent.Height != m_DmdHeight) // file Content의 사이즈를 DMD에 맞게 조절
                                fileContent = ResizeImage(fileContent, m_DmdWidth, m_DmdHeight);

                            Mat m = ToMat((Bitmap)fileContent); // 변환된 사진 fileContent를 비트맵 이미지에서 Mat m으로 변환
                            m.ConvertTo(m, MatType.CV_32FC1, 1 / 255.0); // Mat m의 규격화 및 형식 설정
                            string imageName = Path.GetFileName(fpath);

                            currfileContents[(int)numericUpDownReplaceAt.Value] = m;
                            filenamelist[(int)numericUpDownReplaceAt.Value] = imageName;
                        }
                        else
                        {
                            textBoxPickFileName.Text = "Such file does not exist : " + fpath;
                        }

                        int currnum = 0;
                        currnum = int.TryParse(parts[parts.Length - 1], out currnum) ? currnum : -1;
                        string newpath;
                        int nextnum;
                        if (comboBoxPicker.SelectedIndex == 1)
                        {
                            Random random = new Random();
                            nextnum = random.Next(Convert.ToInt32(textBoxMaxRandom.Text)) % Convert.ToInt32(textBoxMaxRandom.Text);
                        }
                        else if(comboBoxPicker.SelectedIndex == 2)
                        {
                            nextnum = (currnum + 1) % Convert.ToInt32(textBoxMaxRandom.Text);
                        }
                        else
                        {
                            nextnum = currnum;
                        }
                        newpath = basepath + "\\" + String.Join("_", parts, 0, parts.Length - 1) + "_" + nextnum.ToString() + fext;
                        if (File.Exists(newpath))
                        {
                            textBoxPickFileName.Text = newpath;
                        }
                        else
                        {
                            textBoxPickFileName.Text = "Such file does not exist : " + newpath;
                        }
                    }

                }

                textBoxBinMode.Text += currfileContents_length.ToString() + " / " + fileContents_length.ToString() + "\n";
                ////////////////
                if (m_DevId == UInt32.MaxValue)
                {
                    textBoxBinMode.Text += "DMD Alloc error";
                    return;
                }


                // Bitmap Affineimage;
                // Mat MAffine;


                inputseq = new byte[currfileContents_length * m_DmdWidth * m_DmdHeight];
                for (int i = 0; i < currfileContents_length; i++)
                {

                    inputimg = currfileContents[i].Clone();
                    float[,] farr2 = { { 1, 0, 0 + (float)OffsetX }, { 0, 1, 0 + (float)OffsetY } };
                    InputArray affinemat2 = InputArray.Create<float>(farr2);
                    Cv2.WarpAffine(inputimg, inputimg, affinemat2, new OpenCvSharp.Size(inputimg.Width, inputimg.Height));

                    if (mat_mask != null && inputimg.Width == mat_mask.Width && inputimg.Height == mat_mask.Height)
                        imgsum = inputimg.Mul(mat_mask);
                    else
                        imgsum = inputimg + 0;
                    Cv2.Threshold(-imgsum, imgsum, 1.0, 1.0, ThresholdTypes.Trunc);
                    Cv2.Threshold(-imgsum, imgsum, 1.0, 1.0, ThresholdTypes.Trunc);
                    FSarr = FloydSteinbergMat(imgsum, inputimg.Height, inputimg.Width, 0.0, multi_factor);
                    if (inputimg != null)
                    {
                        inputimg.Dispose();
                    }
                    for (Int32 y = 0; y < m_DmdHeight; y++)
                        for (Int32 x = 0; x < m_DmdWidth; x++)
                            inputseq[i * m_DmdWidth * m_DmdHeight + y * m_DmdWidth + x] = FSarr[y * m_DmdWidth + x];
                }



                if (FSarr != null)
                {
                    byte[] FSarr2 = FSarr.ToArray();
                    Mat FSmat = new Mat(currfileContents[0].Height, currfileContents[0].Width, MatType.CV_8U, FSarr2);

                    //Bitmap bmp = FSmat.ToBitmap();
                    Bitmap bmp = ResizeImage(ToBitmap(FSmat), pictureBox1.Width, pictureBox1.Height);
                    if (pictureBox1.Image != null)
                    {
                        try { pictureBox1.Image.Dispose(); } catch { };
                    }
                    pictureBox1.Image = (Image)bmp; pictureBox1.Refresh();

                    Mat FSSmat = FSmat;
                    Mat RFSmat = new Mat(currfileContents[0].Height, currfileContents[0].Width, MatType.CV_8U, FSarr2);

                    Mat RFmatrix = Cv2.GetRotationMatrix2D(new Point2f(FSSmat.Width / 2, FSSmat.Height / 2), 35, 1.0);
                    Cv2.WarpAffine(FSSmat, RFSmat, RFmatrix, new OpenCvSharp.Size(FSSmat.Width, FSSmat.Height));
                    pictureBox3.Image = ResizeImage(ToBitmap(RFSmat), pictureBox3.Width, pictureBox3.Height);
                }
                result = AlpImport.ProjHalt(m_DevId);
                txResult.Text = "ProjHalt " + AlpErrorString(result);

                result = AlpImport.SeqAlloc(m_DevId, 1, currfileContents_length, ref m_SeqId1);
                textBoxBinMode.Text += "SeqAlloc " + AlpErrorString(result);
                textBoxBinMode.AppendText(Environment.NewLine);
                if (AlpImport.Result.ALP_OK != result) return;  // error -> exit

                // Use Binary Bit 1 mode
                result = AlpImport.SeqControl(m_DevId, m_SeqId1, AlpImport.SeqTypes.ALP_BITNUM, 1);
                textBoxBinMode.Text = textBoxBinMode.Text + "SeqControl BITNUM " + AlpErrorString(result);
                textBoxBinMode.AppendText(Environment.NewLine);

                if (checkBoxBinUnint.Checked)
                {
                    // Use Binary Uninterrupted mode
                    result = AlpImport.SeqControl(m_DevId, m_SeqId1, AlpImport.SeqTypes.ALP_BIN_MODE, (int)AlpImport.SeqTypes.ALP_BIN_UNINTERRUPTED);
                    textBoxBinMode.Text = textBoxBinMode.Text + "SeqControl BinUninterrupted " + AlpErrorString(result);
                    textBoxBinMode.AppendText(Environment.NewLine);
                }


                // Load image data from PC memory to ALP memory
                result = AlpImport.SeqPut(m_DevId, m_SeqId1, 0, currfileContents_length, ref inputseq);
                textBoxBinMode.Text = textBoxBinMode.Text + "SeqPut " + AlpErrorString(result);
                textBoxBinMode.AppendText(Environment.NewLine);



                // Set Triggering
                if (radioButton1.Checked)
                {
                    int trig_type = (int)AlpImport.ALP_DEFAULT;
                    switch (comboBox1.SelectedIndex)
                    {
                        case 0:
                            trig_type = (int)AlpImport.DevTypes.ALP_EDGE_RISING;
                            break;
                        case 1:
                            trig_type = (int)AlpImport.DevTypes.ALP_EDGE_FALLING;
                            break;
                            /*
                            case 2:
                                trig_type = (int)AlpImport.ALP_DEFAULT;
                                break;
                            */
                    }

                    // Slave Mode
                    result = AlpImport.ProjControl(m_DevId, AlpImport.ProjTypes.ALP_PROJ_MODE, (int)AlpImport.ProjModes.ALP_SLAVE);
                    textBoxBinMode.Text = textBoxBinMode.Text + "Trig Proj Control PROJ_MODE" + AlpErrorString(result);
                    textBoxBinMode.AppendText(Environment.NewLine);

                    // Select Trigger edge
                    result = AlpImport.DevControl(m_DevId, AlpImport.DevTypes.ALP_TRIGGER_EDGE, trig_type);


                    /*
                    result = AlpImport.ProjControl(m_DevId, AlpImport.ProjTypes.ALP_PROJ_MODE, (int)AlpImport.ProjModes.ALP_MASTER);
                    textBoxBinMode.Text = textBoxBinMode.Text + "Trig Proj Control PROJ_MODE" + AlpErrorString(result);
                    textBoxBinMode.AppendText(Environment.NewLine);
                    */
                    /*
                    result = AlpImport.ProjControl(m_DevId, AlpImport.ProjTypes.ALP_PROJ_STEP, proj_step);
                    textBoxBinMode.Text = textBoxBinMode.Text + "Trig Proj Control PROJ_STEP " + AlpErrorString(result) + "PROJ = " + Convert.ToString(proj_step);
                    textBoxBinMode.AppendText(Environment.NewLine);
                    /*
                    result = AlpImport.DevControl(m_DevId, AlpImport.DevTypes.ALP_TRIGGER_EDGE, proj_step);
                    textBoxBinMode.Text = textBoxBinMode.Text + "Trig DEV Control TRIGGER_EDGE" + AlpErrorString(result) + "PROJ = " + Convert.ToString(proj_step);
                    textBoxBinMode.AppendText(Environment.NewLine);
                    */
                }

                else
                {
                    result = AlpImport.ProjControl(m_DevId, AlpImport.ProjTypes.ALP_PROJ_STEP, AlpImport.ALP_DEFAULT);
                    textBoxBinMode.Text = textBoxBinMode.Text + "Trig Proj Control PROJ_STEP DEAFULT " + AlpErrorString(result);
                    textBoxBinMode.AppendText(Environment.NewLine);
                }

                int illTime = Convert.ToInt32(textBoxIll.Text);
                int picTime = Convert.ToInt32(textBox1.Text);
                if (illTime == 0) illTime = (int)AlpImport.ALP_DEFAULT;
                if (picTime == 0) picTime = (int)AlpImport.ALP_DEFAULT;

                // Set Timing
                result = AlpImport.SeqTiming(m_DevId, m_SeqId1, illTime, picTime, 0, (int)AlpImport.ALP_DEFAULT, 0);
                textBoxBinMode.Text = textBoxBinMode.Text + "SeqControl TIMING " + AlpErrorString(result);
                textBoxBinMode.AppendText(Environment.NewLine);


                // Start display
                if (AlpImport.Result.ALP_OK != result) return;  // error -> exit
                result = AlpImport.ProjStartCont(m_DevId, m_SeqId1);
                txResult.Text = "ProjStartCont " + AlpErrorString(result);

                StreamWriter logwriter;
                string executableDirectory = AppDomain.CurrentDomain.BaseDirectory;
                // Specify the file path by combining the executable directory with the file name

                string folderPath = Path.Combine(executableDirectory, DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"));

                // 폴더가 존재하지 않으면 생성
                if (!Directory.Exists(folderPath))
                {
                    try
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("폴더 생성 오류: " + ex.Message);
                        return;
                    }
                }

                // 파일 이름과 경로
                string filename = "DMDLog_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".txt";
                string filePath = Path.Combine(folderPath, filename);
                logwriter = File.AppendText(filePath);


                logwriter.Write(DateTime.Now.ToString("yyyy-MM-dd tt hh:mm:ss  ") + "\n");// 날짜
                if (radioButton1.Checked) { logwriter.Write("- Trigger: On\n"); } //트리거 유무
                else if (radioButton2.Checked) { logwriter.Write("- Trigger: Off\n"); }
                string formattedString = $"- IlluminateTime: {illTime} us, Picturetime: {picTime} us\n";
                logwriter.Write(formattedString); //picTime & IllTime
                logwriter.Write("- Mask file: " + Masklogpath + "\n");
                logwriter.Write("- Multiplier: " + textBoxMulti.Text + "\n"); // Multiplier
                logwriter.Write("- Phase txt file: " + textBoxTF.Text + "\n"); // Phase txt file               
                logwriter.Write("<Image list>"); // Sequence에 사용된 이미지들
                logwriter.Write("Iteration: " + Convert.ToString(Seqcount));
                logwriter.Write("/ Image Seq: " + Convert.ToString(Seqnumber));

                logwriter.WriteLine("\n - Seq");


                //  if (intLists != null && intLists.Count > Seqnumber) { 
                //                    foreach (int imgidx in intLists[Seqnumber]) // 각각의 경로에 있는 파일들에 대한 루프
                for(int imgidx= 0; imgidx < filenamelist.Count; imgidx++) // 각각의 경로에 있는 파일들에 대한 루프
                {
                    // Iterate through each column in the current row
                    // Extract or process the non-null element                        
                    logwriter.Write(filenamelist[imgidx]);
                        logwriter.Write(", ");
                    }
             //   }
                /*for (int i=0; i<Imagelistlog.GetLength(0);i++)
                {
                    logwriter.WriteLine("- Seq" + Convert.ToString(i)+"\n");
                    for (int j = 0; j < Imagelistlog.GetLength(1); j++)
                    {
                        // Check if the element is non-empty before writing
                        if (!string.IsNullOrEmpty(Imagelistlog[i,j]))
                        {
                            logwriter.WriteLine(Imagelistlog[i, j]);
                        }
                    }
                } */

                logwriter.Close();
                // if(SeqRepeat!=0) currfileContents.Clear(); // 리스트 fileContents 초기화

               // textBoxSeqNum.Text = Convert.ToString(Seqnumber+1);
              //  numericUpDownIterNum.Value = Seqcount+1;
            }
        }
        private void buttonProjSeqs_Click(object sender, EventArgs e)
        {
            /*
            int count = CountNonNullElements(Imageseqlists);
            if (count == SeqRepeat || checkbox_Low.Checked)
            { projDMD(); }
            else { MessageBox.Show("Fill the whole image lists(" + Convert.ToString(count)+"), or change the # of Seq(" + Convert.ToString(SeqRepeat)+").\nOr check the 'Low' checkbox.", "Number Mismatching"); }
            */
            try
            {
                projDMD();
            } catch(Exception errcode)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter("errlog.txt"))
                    {
                        sw.WriteLine(errcode.Message);
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        sw.WriteLine(textBoxBinMode.Text);
                    }
                }
                catch
                {

                }
            }
        }

        private void AlpSampleForm_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            bnHalt.Enabled = false;
            bnCleanUp.Enabled = false;
            buttonProjSeqs.Enabled = false;
        }

        string Masklogpath = "No mask";
        private void buttonMaskLoad_Click(object sender, EventArgs e)
        {
            
            Image loadedImage;
            InitializeOpenFileDialog1();
            openFileDialog1.Filter = "Image Files (BMP,TIFF)|*.bmp;*.tiff;*.tif|Image Files(JPG,PNG,GIF)|*.jpg;*.png;*.gif";
            openFileDialog1.InitialDirectory = prevpath_Mask;
            DialogResult dr = this.openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                // Read the files
                String file = openFileDialog1.FileName;
                prevpath_Mask = Path.GetDirectoryName(openFileDialog1.FileName); // Set Path
                
                Masklogpath = openFileDialog1.FileName;
                StreamWriter logwriter;
                logwriter = File.AppendText("Mask_Log.txt");
                logwriter.Write(DateTime.Now.ToString("yyyy-MM-dd tt hh:mm:ss  ") + Masklogpath + "\n");
                logwriter.Close();

                // Create a PictureBox.
                try
                {
                    loadedImage = Image.FromFile(file);
                    if (pictureBoxMask.Image != null)
                    {
                        try { pictureBoxMask.Image.Dispose(); } catch { };
                    }

                    pictureBoxMask.Image = new Bitmap(ResizeImage(loadedImage, pictureBoxMask.Width, pictureBoxMask.Height));

                    Mat m = ToMat((Bitmap)loadedImage);
                    mat_mask = new Mat();
                    m.ConvertTo(mat_mask, MatType.CV_32FC1, 1 / 255.0);

                    float[] copied = new float[mat_mask.Total() * mat_mask.Channels()];
                    mat_mask.GetArray(0, 0, copied);
                    BinaryWriter writer = new BinaryWriter(File.Open(file + ".MASK", FileMode.Create));
                    writer.Write(mat_mask.Width);
                    writer.Write(mat_mask.Height);
                    copied = new float[mat_mask.Total() * mat_mask.Channels()];
                    mat_mask.GetArray(0, 0, copied);
                    foreach (var value in copied)
                    {
                        writer.Write(value);
                    }

                    writer.Close();
                }
                catch (SecurityException ex)
                { 
                    // The user lacks appropriate permissions to read files, discover paths, etc.
                    MessageBox.Show("Security error. Please contact your administrator for details.\n\n" +
                        "Error message: " + ex.Message + "\n\n" +
                        "Details (send to Support):\n\n" + ex.StackTrace
                    );
                }
                catch (Exception ex)
                {
                    // Could not load the image - probably related to Windows file system permissions.
                    MessageBox.Show("Cannot display the image: " + file.Substring(file.LastIndexOf('\\'))
                        + ". You may not have permission to read the file, or " +
                        "it may be corrupt.\n\nReported error: " + ex.Message);
                }
            }
        }

        private void bnHalt_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                Action safeProject = delegate { bnHalt_Click(sender, e); };
                this.Invoke(safeProject);
            }
            else
            {
                AlpImport.Result result = AlpImport.ProjHalt(m_DevId);
                txResult.Text = "ProjHalt " + AlpErrorString(result);
            }
        }

        private void bnCleanUp_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                Action safeProject = delegate { bnCleanUp_Click(sender, e); };
                this.Invoke(safeProject);
            }
            else
            {// Disable SynchGate1 output: Omit "Gate" parameter
             // (Not available prior to ALP-4)
                AlpImport.DevControlEx_SynchGate(m_DevId, 1, true);
                //AlpImport.DevControlEx_SynchGate(m_DevId, 2, true);
                //AlpImport.DevControlEx_SynchGate(m_DevId, 3, true);

                // Recommendation: always call DevHalt() before DevFree()
                AlpImport.Result result = AlpImport.DevFree(m_DevId);
                txResult.Text = "DevFree " + AlpErrorString(result);
                if (AlpImport.Result.ALP_OK != result) return;  // error -> exit
                m_DevId = UInt32.MaxValue;

                bnHalt.Enabled = false;
                bnCleanUp.Enabled = false;
                buttonProjSeqs.Enabled = false;

                if (pictureBox1.Image != null)
                {
                    try { pictureBox1.Image.Dispose(); } catch { };
                }
                pictureBox1.Image = null;
            }
        }

        public void LoadTF()
        {
            if (this.InvokeRequired)
            {
                Action safeProject = delegate { LoadTF(); };
                this.Invoke(safeProject);
            }
            else
            {
                string pathstring = textBoxTF.Text;
                if (File.Exists(pathstring))
                {
                    try
                    {
                        using (StreamReader file = new StreamReader(pathstring))
                        {
                            Console.WriteLine("Readed");
                            int counter = 0;
                            string ln;

                            while ((ln = file.ReadLine()) != null)
                            {
                                String[] slist = ln.Split('=');
                                if (slist[0].Equals("OffsetX"))
                                {
                                    OffsetX = Convert.ToDouble(slist[1]);
                                }
                                else if (slist[0].Equals("OffsetY"))
                                {
                                    OffsetY = Convert.ToDouble(slist[1]);
                                }
                                else if (slist[0].Equals("Filename"))
                                {
                                    str_lattpath = slist[1];
                                }
                                counter++;
                            }
                            file.Close();
                            Console.WriteLine(OffsetX);
                            Console.WriteLine(OffsetY);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Err");
                    }
                }
                try
                {
                    textBoxTFread.Text = string.Format("({0},{1})", OffsetX, OffsetY);
                }
                catch
                {
                    Console.WriteLine("Error");
                }
            }
        }
        private void buttonLoadTF_Click(object sender, EventArgs e)
        {
            LoadTF();
        }

        private Object outputLock = new Object();

      /*  private void watch(string path)
        {
            watcher = new FileSystemWatcher();
            watcher.Path = Path.GetDirectoryName(path);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.*";
            watcher.Changed += new FileSystemEventHandler(WatchThread);
            watcher.EnableRaisingEvents = true;
        }
      */
        private void watch(string fullpath)
        {
            watcher = new FileSystemWatcher(Path.GetDirectoryName(fullpath));
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = Path.GetFileName(fullpath);
            watcher.Changed += WatchThread;
            watcher.EnableRaisingEvents = true;
        }
        private void buttonWatch_Click(object sender, EventArgs e)
        {
            try
            {
                StopWatching();
            }
            catch(Exception errcode)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter("errlog.txt"))
                    {
                        sw.WriteLine("Watch StopWatching() call error");
                        sw.WriteLine(errcode.Message);
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        sw.WriteLine(textBoxBinMode.Text);
                    }
                }
                catch
                {

                }
            }

            try
            {
                watch(textBoxTF.Text);
            }
            catch (Exception errcode)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter("errlog.txt"))
                    {
                        sw.WriteLine("Watch watch() call error");
                        sw.WriteLine(errcode.Message);
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        sw.WriteLine(textBoxBinMode.Text);
                    }
                }
                catch
                {

                }
            }
            this.BackColor = Color.Red;
        }


        /* ==================================================
         * 
         * THREADING
         * ==================================================
         */

        /*
        private void WatchThread(object s, FileSystemEventArgs e)
        {

            if (Monitor.TryEnter(outputLock)) {
                try
                {
                    AlpImport.Result result = AlpImport.ProjHalt(m_DevId);
                    txResult.Text = "ProjHalt " + AlpErrorString(result);
                    bnHalt_Click(s, e);
                    bnCleanUp_Click(s, e);
                    bnInit_Click(s, e);

                    LoadTF();
                    projDMD();

                    if (checkBoxBinUnint.Checked) RenewLatticeImage();
                    Thread.Sleep(500);
                }
                finally
                {
                    Thread.Sleep(500);
                    Monitor.Exit(outputLock);
                }
            }

            //}
        }
        */

        /* ==================================================
         * 
         * User Defined Function
         * ==================================================
         */

        bool EnterFlag=false;

        private void WatchThread(object s, FileSystemEventArgs e)
        {
            String txerr = "";
            try
            {
                if (e.ChangeType != WatcherChangeTypes.Changed)
                {
                    return;
                }

                if (Monitor.TryEnter(outputLock) && !EnterFlag)
                {
                    try
                    {
                        EnterFlag = true;

                        var oldOffsetX = OffsetX;
                        var oldOffsetY = OffsetY;

                        LoadTF();
                        if (Math.Abs(oldOffsetX - OffsetX) > 1e-3 || Math.Abs(oldOffsetY - OffsetY) > 1e-3)
                        {
                            AlpImport.Result result = AlpImport.ProjHalt(m_DevId);
                            // txResult.Text = "ProjHalt " + AlpErrorString(result);
                            txerr += "ProjHalt " + AlpErrorString(result);
                            bnHalt_Click(s, e);
                            // Thread.Sleep(100);

                            bnCleanUp_Click(s, e);
                            Thread.Sleep(100);
                            bnInit_Click(s, e);
                            Thread.Sleep(100);
                            txerr += "Proj DMD ";
                            projDMD();
                            txerr += "Proj Sleep ";
                            Thread.Sleep(500);
                        }
                    }
                    finally
                    {
                        Thread.Sleep(500);
                        Monitor.Exit(outputLock);
                        EnterFlag = false;
                    }
                }
            }
            catch (Exception errcode)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter("errlog.txt"))
                    {
                        sw.WriteLine("Watch Thread error");
                        sw.WriteLine(errcode.Message);
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        sw.WriteLine(textBoxBinMode.Text);
                        sw.WriteLine(txerr);
                    }
                }
                catch
                {

                }
            }
            //}
        }
        private void InitializeOpenFileDialog1()
        {
            // Set the file dialog to filter for graphics files.
            this.openFileDialog1.Filter =
                "Images (*.BMP;*.JPG;*.GIF;*.TIFF)|*.BMP;*.JPG;*.GIF;*.TIFF;*.tif|" +
                "All files (*.*)|*.*";

            // Allow the user to select multiple images.
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "Select Mask Browser";
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            //   if (thread1 != null && thread1.IsAlive) thread1.Abort();
            this.BackColor = default(Color);
            this.watcher.Changed -= WatchThread;
            if(this.watcher != null) this.watcher.Dispose();

        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }


        /*
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Mat m = new Mat();
            
            if (Imageseqlists[(int)numericUpDown2.Value].Count>0)
            {
                numericUpDown1.Maximum = Imageseqlists[(int)numericUpDown2.Value].Count - 1;
                if ((int)numericUpDown1.Value > Imageseqlists[(int)numericUpDown2.Value].Count - 1)
                {
                    numericUpDown1.Maximum = Imageseqlists[(int)numericUpDown2.Value].Count - 1;
                    numericUpDown1.Value = 0;
                }
                Imageseqlists[(int)numericUpDown2.Value][(int)numericUpDown1.Value].CopyTo(m);
                m.ConvertTo(m, MatType.CV_8U, 255.0);

                Bitmap bmp = ResizeImage(ToBitmap(m), pictureBox1.Width, pictureBox1.Height);
                try { pictureBox1.Image.Dispose(); } catch { };
                pictureBox1.Image = (Image)bmp; pictureBox1.Refresh();
            }
            else 
            {
                MessageBox.Show("No images in this image sequence.\nFill this image lists: Seq(" + Convert.ToString(numericUpDown2.Value) + "), Image number(" + Convert.ToString(numericUpDown1.Value) + ").", "Image null");
                numericUpDown2.Value = 0; numericUpDown1.Value = 0;             
            }
        }
        */

        private void buttonToBot_Click(object sender, EventArgs e)
        {
            Mat temp;
            String tempfilename;
            Console.WriteLine(fileContents_length);

            temp = fileContents[fileContents_length - 1];
            fileContents[fileContents_length - 1] = fileContents[(int)numericUpDown1.Value];
            fileContents[(int)numericUpDown1.Value] = temp;

            tempfilename = filenamelist[fileContents_length - 1];
            filenamelist[fileContents_length - 1] = filenamelist[(int)numericUpDown1.Value];
            filenamelist[(int)numericUpDown1.Value] = tempfilename;

            Mat m = new Mat();
            fileContents[(int)numericUpDown1.Value].CopyTo(m);
            m.ConvertTo(m, MatType.CV_8U, 255.0);

            Bitmap bmp = ResizeImage(ToBitmap(m), pictureBox1.Width, pictureBox1.Height);
            if (pictureBox1.Image != null)
            {
                try { pictureBox1.Image.Dispose(); } catch { };
            }
            pictureBox1.Image = (Image)bmp; pictureBox1.Refresh();

        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            //var filePath = string.Empty;
            using (OpenFileDialog openfiledialog = new OpenFileDialog())
            {
                openfiledialog.InitialDirectory = prevpath_Phase;
                openfiledialog.Filter = "Text files (*.txt)|*.txt";
                openfiledialog.FilterIndex = 1;
                openfiledialog.Multiselect = false;
                openfiledialog.RestoreDirectory = true;

                if (openfiledialog.ShowDialog() == DialogResult.OK && openfiledialog.FileNames.Length > 0)
                {
                    prevpath_Phase = Path.GetDirectoryName(openfiledialog.FileName); // Set Path
                    textBoxTF.Text = openfiledialog.FileName;
                }
            }
        }
        /*
        private void button1_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;
            using (OpenFileDialog openfiledialog = new OpenFileDialog())
            {
                openfiledialog.InitialDirectory = prevpath_LattFolder;
                openfiledialog.Filter = "Binary files (*.dat, *.bin)|*.dat;*.bin";
                openfiledialog.FilterIndex = 1;
                openfiledialog.Multiselect = false;
                openfiledialog.RestoreDirectory = true;

                if (openfiledialog.ShowDialog() == DialogResult.OK && openfiledialog.FileNames.Length > 0)
                {
                    prevpath_LattFolder = Path.GetDirectoryName(openfiledialog.FileName); // Set Path
                    textBoxLattPath.Text = openfiledialog.FileName;
                }
                str_lattpath = textBoxLattPath.Text;
            }
        }*/

        private void buttonImgs_Click(object sender, EventArgs e)
        {
            using (var form1 = new CustomWindow())
            {
                form1.ShowDialog();
                Mat m = form1.mat;
                String str = form1.str_imgtype;
                if (m != null && str != "No Image")
                {
                    labelImageType.Text = str;
                   
                    m.ConvertTo(m, MatType.CV_8U, 255.0);
                    Mat MBuild = new Mat();
                    m.ConvertTo(MBuild, MatType.CV_32FC1, 1 / 255.0);

                    fileContents.Clear();
                    fileContents.Add(MBuild);

                    fileContents_length = 1;
                    numericUpDown1.Maximum = fileContents_length - 1;
                    numericUpDownReplaceAt.Maximum = fileContents_length - 1;
                    // pictureBoxIplLattImg.ImageIpl = m;
                    if (pictureBox1.Image != null)
                    {
                        try { pictureBox1.Image.Dispose(); } catch { };
                    }
                    if (m != null)
                        pictureBox1.Image = ResizeImage(ToBitmap(m), pictureBox1.Width, pictureBox1.Height);
                    else
                        txResult.Text = "Err!";

                }
            }
            //form1.Show();    
        }
        /*
        private void RenewLatticeImage()
        {
            if (this.InvokeRequired)
            {
                Action safeRenew = delegate { RenewLatticeImage(); };
                this.Invoke(safeRenew);
            }
            else
            {
                if (checkBoxSynch.Checked)
                {
                    textBoxLattPath.Text = str_lattpath;
                }
                var filename = textBoxLattPath.Text;
                if (!File.Exists(filename)) return;
                Byte[] bt = File.ReadAllBytes(filename);
                int lattsizewidth = 0, lattsizeheight = 0;
                int left, bot, width, height;
                Mat m = null;


                if (int.TryParse(textBoxLattSizeWidth.Text, out lattsizewidth) &&
                    int.TryParse(textBoxLattSizeHeight.Text, out lattsizeheight) &&
                    int.TryParse(textBoxLattLeft.Text, out left) &&
                    int.TryParse(textBoxLattBot.Text, out bot) &&
                    int.TryParse(textBoxLattWidth.Text, out width) &&
                    int.TryParse(textBoxLattHeight.Text, out height))
                {
                    if (width == 0 || height == 0)
                    {
                        m = new Mat(lattsizewidth, lattsizeheight, MatType.CV_8U);
                        for (int i = 0; i < lattsizeheight; i++)
                        {
                            for (int j = 0; j < lattsizewidth; j++)
                            {
                                m.Set<byte>(i, j, (byte)(bt[(i * lattsizewidth) + j] * 255));
                            }
                        }
                    }
                    else if (height + bot < lattsizeheight && width + left < lattsizewidth)
                    {
                        m = new Mat(width, height, MatType.CV_8U);
                        for (int i = 0; i < height; i++)
                        {
                            for (int j = 0; j < width; j++)
                            {
                                m.Set<byte>(i, j, (byte)(bt[((i + bot) * lattsizewidth) + (j + left)] * 255));
                            }
                        }
                    }
                }
                if (m != null && lattsizewidth != 0 && lattsizeheight != 0)
                {
                    //try { pictureBoxIplLattImg.ImageIpl.Dispose(); } catch { };
                    pictureBoxIplLattImg.ImageIpl = m;
                }
            }
        }
        */
        private void textBoxLattSize_TextChanged(object sender, EventArgs e)
        {
            //RenewLatticeImage();
        }

        private void textBoxLattLeft_TextChanged(object sender, EventArgs e)
        {
            //RenewLatticeImage();
        }

        private void textBoxLattTop_TextChanged(object sender, EventArgs e)
        {
            //RenewLatticeImage();
        }

        private void textBoxLattWidth_TextChanged(object sender, EventArgs e)
        {
            //RenewLatticeImage();
        }
        private void textBoxLattHeight_TextChanged(object sender, EventArgs e)
        {
            // RenewLatticeImage();
        }

        private void buttonLoadLattImage_Click(object sender, EventArgs e)
        {
            // RenewLatticeImage();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        /*
        private void label15_Click(object sender, EventArgs e)
        {
            label15.Text = "";
        }

        private void btt_Mag_Click(object sender, EventArgs e)
        {
            double M = Convert.ToDouble(txt_mag.Text);
            label15.Text = "M = " + M.ToString();
        }


        private void label16_Click(object sender, EventArgs e)
        {
            label16.Text = "";
        }

        private void btt_adrow_Click(object sender, EventArgs e)
        {
            int adrow = Convert.ToInt16(txt_adrow.Text);
            label16.Text = "adrow = " + adrow.ToString();
        }

        private void label17_Click(object sender, EventArgs e)
        {
            label17.Text = "";
        }

        private void btt_adcol_Click(object sender, EventArgs e)
        {
            int adcol = Convert.ToInt16(txt_adcol.Text);
            label17.Text = "adcol = " + adcol.ToString();
        }

        private void label18_Click(object sender, EventArgs e)
        {
            label18.Text = "";
        }

        private void btt_angle_Click(object sender, EventArgs e)
        {
            double angle = Convert.ToDouble(txt_angle.Text);
            label18.Text = "angle = " + angle.ToString();
        }
        */
        //Draw 창 띄우기

        /*
    private void btt_Draw_Click(object sender, EventArgs e)
    {
        using (var draw = new Draw())
        {
            draw.ShowDialog();
            //form1.Show();

            Rows = draw.dataVar.GetLength(0);
            Cols = draw.dataVar.GetLength(1);
            arr = new double[Rows, Cols];
            csvarr = new double[Rows, Cols];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    arr[i, j] = 255 * draw.dataVar[j, i];
                    csvarr[i, j] = draw.dataVar[i, j];
                   //Console.Write($"{draw.dataVar[i, j]}");
                }
            //Console.WriteLine();
            }
        }
    }
    double[,] arr;
    int Rows;
    int Cols;
    double[,] csvarr;

    private void btt_Drawload_Click(object sender, EventArgs e) //원본 Draw 이미지 Load
    {
        Mat Original = new Mat(arr.GetLength(0), arr.GetLength(1), MatType.CV_8UC1);
        //이미지 Mat 생성

        for (int i = 0; i < Original.Rows; i++)
        {
            for (int j = 0; j < Original.Cols; j++)
            {
                byte pixelValue = (byte)arr[i, j];
                Original.Set<byte>(i, j, pixelValue);
            }
        }
        Mat Load = new Mat();
        Cv2.Resize(Original, Load, new OpenCvSharp.Size(arr.GetLength(0) * 100, arr.GetLength(1) * 100), 0, 0, InterpolationFlags.Nearest);
        pictureBox2.Image = BitmapConverter.ToBitmap(Load);
    }
    int adrow = 0;
    int adcol=0;
    double angle = -8.0;
    double M = 8.25; 


    Bitmap Affineimage;
    Mat MAffine;

    private void btt_Mainimage_Click(object sender, EventArgs e)
    {
        AffineTF();
        MAffine.ConvertTo(MAffine, MatType.CV_32FC1, 1 / 255.0);
        Imageseqlists[0].Clear();
        Imageseqlists[0].Add(MAffine);
        fileContents_length = 1;
        numericUpDown1.Maximum = fileContents_length - 1;
        pictureBox1.Image = ResizeImage(Affineimage, pictureBox1.Width, pictureBox1.Height);
    }


    private void btt_drawsave_Click(object sender, EventArgs e)
    {
        using (SaveFileDialog saveFileDialog = new SaveFileDialog())
        {
            saveFileDialog.Filter = "CSV file|*.csv";
            saveFileDialog.FileName = "Draw.csv";
            saveFileDialog.Title = "Save CSV file";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                SaveArrayAsCsv(csvarr, filePath);

            }

        }
    }

    static void SaveArrayAsCsv(double[,] array, string filePath)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    writer.Write(array[col, row]);

                    if (col < cols - 1)
                    writer.Write(","); // Add a comma between values
                }

                writer.WriteLine(); // Add a new line after each row
            }
        }

    }


    private void btt_TFdrawsave_Click(object sender, EventArgs e)
    {
        using (SaveFileDialog saveFileDialog = new SaveFileDialog())
        {
            saveFileDialog.Filter = "JPEG Image|*.jpg";
            saveFileDialog.FileName = "AffineTFimage.jpg";
            saveFileDialog.Title = "Save Image";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                // Save the Bitmap as a JPEG image
                Affineimage.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }
    }


        */

        public Mat ResizeMat(Mat inputMat, int width, int height)
        {
            // Create a new Mat to store the resized image
            Mat resizedMat = new Mat();

            // Resize the inputMat to the specified width and height
            Cv2.Resize(inputMat, resizedMat, new OpenCvSharp.Size(width, height));

            return resizedMat;
        }

        public static Mat ToMat(Bitmap bitmap)
        {
            int w = bitmap.Width;
            int h = bitmap.Height;
            Mat m = new Mat(new OpenCvSharp.Size(w, h), MatType.CV_8U);
            //Mat m = Mat.Zeros(h,w);
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    var v = bitmap.GetPixel(j, i).R;
                    m.Set<Byte>(i, j, v);
                }
            }
            return m;
        }


        /*
        private void button3_Click(object sender, EventArgs e)
        {
            Mat m = new Mat();
            if ((int)numericUpDown1.Value>Imageseqlists[(int)numericUpDown2.Value].Count)
            {
                numericUpDown1.Maximum = Imageseqlists[(int)numericUpDown2.Value].Count - 1;
                numericUpDown1.Value = 0;
            }
            Imageseqlists[(int)numericUpDown2.Value][(int)numericUpDown1.Value].CopyTo(m);
            numericUpDown1.Maximum = Imageseqlists[(int)numericUpDown2.Value].Count - 1;
            m.ConvertTo(m, MatType.CV_8U, 255.0);

            Bitmap bmp = ResizeImage(ToBitmap(m), pictureBox1.Width, pictureBox1.Height);
            try { pictureBox1.Image.Dispose(); } catch { };
            pictureBox1.Image = (Image)bmp; pictureBox1.Refresh();
        }

        int[] OArray;
        static List<Mat> GenerateFileContents(List<Mat> ListContents, int[] OArray)
        {
            List<Mat> FileContents = new List<Mat>();

            foreach (int index in OArray)
            {
                // Adjust index for 0-based indexing
                int adjustedIndex = index - 1;

                // Check if the index is valid
                if (adjustedIndex >= 0 && adjustedIndex < ListContents.Count)
                {
                    // Add the corresponding image to fileContents
                    FileContents.Add(ListContents[adjustedIndex]);
                }
            }

            return FileContents;
        }

        public class CsvReader
        {
            public int[] ReadCsvToArray(string filePath)
            {
                // Read all lines from the CSV file
                string[] lines = File.ReadAllLines(filePath);

                // Assuming there's only one line in the CSV file containing the array
                if (lines.Length == 1)
                {
                    // Get the array string by removing square brackets and splitting by comma
                    string[] values = lines[0].Trim('[', ']').Split(',');

                    // Convert string array to int array
                    int[] array = new int[values.Length];
                    for (int i = 0; i < values.Length; i++)
                    {
                        array[i] = int.Parse(values[i]);
                    }

                    return array;
                }
                else
                {
                    // MessageBox.Show("Invalid dimension of csv Array", "List Diemension Mismatching");
                    // Handle the case when the CSV file has more than one line or no lines
                    throw new InvalidOperationException("Invalid Dimension of csv Array. You should select [1,:]");

                }
            }
        }

        private void numericUpDownSeq_ValueChanged(object sender, EventArgs e)
        {
            // Clear existing items
            comboBox2.Items.Clear();
            numericUpDownSeq.Minimum = 1;
            numericUpDownSeq.Maximum = 20;
            // Add items from 1 to 10
            for (int i = 0; i < (int)numericUpDownSeq.Value; i++)
            {
                comboBox2.Items.Add(i);
            }
            comboBox2.SelectedIndex = 0;
            SeqRepeat = (int)numericUpDownSeq.Value;
            if ((int)numericUpDownSeq.Value > 0)
            {
                numericUpDown2.Maximum = (int)numericUpDownSeq.Value - 1;
            }
            else { numericUpDown2.Maximum = 0; }
        }

        private void groupBox8_Enter(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown2.Maximum = (int)numericUpDownSeq.Value - 1;
        }

                        */

        private void btt_Reset_Click(object sender, EventArgs e)
        {
            int Seqcount = 0;
            int Seqnumber = 0;
            //textBoxSeqNum.Text = Convert.ToString(Seqnumber);
          //  numericUpDownIterNum.Value = Seqcount;
        }
        
        public static Bitmap ToBitmap(Mat mat)
        {
            int w = mat.Width;
            int h = mat.Height;
            Bitmap bitmap = new Bitmap(w, h);
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    var v = mat.At<Byte>(i, j);
                    bitmap.SetPixel(j, i, Color.FromArgb(v, v, v));
                }
            }
            return bitmap;
        }
        /*
        private void buttonTxtSeq_Click(object sender, EventArgs e)
        {
            //var filePath = string.Empty;
            using (OpenFileDialog openfiledialog = new OpenFileDialog())
            {
                openfiledialog.InitialDirectory = prevpath_ImgList;
                openfiledialog.Filter = "Text files (*.txt)|*.txt";
                openfiledialog.FilterIndex = 1;
                openfiledialog.Multiselect = false;
                openfiledialog.RestoreDirectory = true;

                if (openfiledialog.ShowDialog() == DialogResult.OK && openfiledialog.FileNames.Length > 0)
                {
                    prevpath_ImgList = Path.GetDirectoryName(openfiledialog.FileName); // Set Path
                    textBoxVarlistName.Text = openfiledialog.FileName;

                    string[] lines = File.ReadAllLines(openfiledialog.FileName);
                    intLists = new List<List<int>>();

                    try
                    {

                        foreach (string line in lines)
                        {
                            string[] words = line.Split(' ');
                            List<int> intList = new List<int>();

                            foreach (string word in words)
                            {
                                if (int.TryParse(word, out int number))
                                {
                                    intList.Add(number);
                                }
                            }

                            intLists.Add(intList);
                        }
                        SeqRepeat = intLists.Count;
                        labelRepeatNum.Text = "Repeat: " + Convert.ToString(SeqRepeat);
                    }
                    catch
                    {
                        textBoxBinMode.Text = textBoxBinMode.Text + "\n ERR on varlist load";
                    }

                    int r = intLists.Count;
                    int c = 0;
                    for(int i = 0; i < r; i++)
                    {
                        List<int> t =  intLists[i];
                        c = (c > t.Count) ? c : t.Count;
                    }
                    tableLayoutPanel1.RowCount = r;
                    tableLayoutPanel1.ColumnCount= c+1;
                    // tableLayoutPanel1.AutoSize = true;
                    tableLayoutPanel1.ColumnStyles.Clear();

                    for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
                    {
                        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
                    }
                    tableLayoutPanel1.RowStyles.Clear();

                    for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
                    {
                        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
                    }

                    for (int i = 0; i < r; i++)
                    {
                        Label label = new Label();
                        label.Text = "Seq " + (i).ToString();
                        tableLayoutPanel1.Controls.Add(label, 0, i);
                        for (int j = 0; j < intLists[i].Count; j++)
                        {
                            Label label2 = new Label();
                            label2.Text = (intLists[i][j]).ToString();
                            tableLayoutPanel1.Controls.Add(label2, j+1, i);
                        }
                    }
                }
            }
        }
        */
        /*
        private void buttonSeqReset_Click(object sender, EventArgs e)
        {
            intLists = new List<List<int>>();
            SeqRepeat = 0;
            labelRepeatNum.Text = "Repeat: " + Convert.ToString(SeqRepeat);
            textBoxVarlistName.Text = "No varlist";

            int r = 0;
            int c = 0;
            tableLayoutPanel1.RowCount = r;
            tableLayoutPanel1.ColumnCount = c + 1;
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.ColumnStyles.Clear();

            for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            }
            tableLayoutPanel1.RowStyles.Clear();

            for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            }

        }
        */

        private void textBoxSeqNum_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonPick_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;
            using (OpenFileDialog openfiledialog = new OpenFileDialog())
            {
                openfiledialog.InitialDirectory = prevpath_Image;
                //openfiledialog.Filter = "jpg files (*.jpg)|*.jpg|bmp files (*.bmp)|*.bmp|binary files (*.bin)|*.bin|tiff files (*.tiff)|*.tiff";
                openfiledialog.Filter = "Image Files (BMP,TIFF)|*.bmp;*.tiff;*.tif|Image Files(JPG,PNG,GIF)|*.jpg;*.png;*.gif";
                openfiledialog.FilterIndex = 1;
                openfiledialog.Multiselect = false;
                openfiledialog.RestoreDirectory = true;

                if (openfiledialog.ShowDialog() == DialogResult.OK) // 이미지 가져와서 실행
                {
                    textBoxPickFileName.Text = openfiledialog.FileName;
                    textBoxPickFileName.Enabled = true;
                    buttonResetPick.Enabled = true;
                    textBoxMaxRandom.Enabled = true;
                    numericUpDownReplaceAt.Enabled = true;
                }
            }
        }

        private void buttonResetPick_Click(object sender, EventArgs e)
        {
            textBoxPickFileName.Text = "";
            textBoxPickFileName.Enabled = false;
            buttonResetPick.Enabled = false;
            textBoxMaxRandom.Enabled = false;
            numericUpDownReplaceAt.Enabled = false;
        }

        /*
private void numericUpDownIterNum_ValueChanged(object sender, EventArgs e)
{
   if(SeqRepeat == 0)
       {
       textBoxSeqNum.Text = Convert.ToString(0);
   }
       else
       {
       textBoxSeqNum.Text = Convert.ToString(Convert.ToInt32(numericUpDownIterNum.Value) % SeqRepeat);
   }
}
*/

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Mat m = new Mat();
            fileContents[(int)numericUpDown1.Value].CopyTo(m);
            m.ConvertTo(m, MatType.CV_8U, 255.0);

            Bitmap bmp = ResizeImage(ToBitmap(m), pictureBox1.Width, pictureBox1.Height);
            if (pictureBox1.Image != null)
            {
                try { pictureBox1.Image.Dispose(); } catch { };
            }
            pictureBox1.Image = (Image)bmp; pictureBox1.Refresh();

            txtbox_loadseq.Text = filenamelist[(int)numericUpDown1.Value];
        }



        //private void btt_LogReset_Click(object sender, EventArgs e)
        //{
        //    Array.Clear(Imagelistlog, 0, Imagelistlog.Length);
        //}

        //private void btt_Seqflip_Load_Click(object sender, EventArgs e)
        //{
        //    using (OpenFileDialog openfiledialog = new OpenFileDialog())
        //    {
        //        openfiledialog.InitialDirectory = SeqflipPath;
        //        openfiledialog.Filter = "All files (*.*)|*.*";
        //        openfiledialog.FilterIndex = 1;
        //        openfiledialog.Multiselect = false;
        //        openfiledialog.RestoreDirectory = true;

        //        if (openfiledialog.ShowDialog() == DialogResult.OK && openfiledialog.FileNames.Length > 0)
        //        {
        //            // 파일의 디렉터리 경로를 SeqflipPath에 설정
        //            SeqflipPath = Path.GetDirectoryName(openfiledialog.FileName);

        //            // 선택한 파일의 전체 경로를 txt_SeqFlip 텍스트 박스에 설정
        //            txt_SeqFlip.Text = Path.GetDirectoryName(openfiledialog.FileName); // 경로만 설정
        //        }
        //    }
        //}

        //private void btt_Seqflip_Watch_Click(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(txt_SeqFlip.Text))
        //    {
        //        SeqflipPath = txt_SeqFlip.Text;

        //        // FileSystemWatcher 초기화 및 설정
        //        if (fileSystemWatcher != null)
        //        {
        //            fileSystemWatcher.Dispose();
        //        }

        //        fileSystemWatcher = new FileSystemWatcher();
        //        fileSystemWatcher.Path = SeqflipPath;
        //        fileSystemWatcher.Filter = "*.*"; // 모든 파일 감시
        //        fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
        //        fileSystemWatcher.IncludeSubdirectories = true;

        //        // 이벤트 핸들러 설정
        //        fileSystemWatcher.Created += OnNewFileCreated;

        //        // 감시 시작
        //        fileSystemWatcher.EnableRaisingEvents = true;

        //        //MessageBox.Show("Started watching: " + SeqflipPath);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please specify a directory path.");
        //    }

        //    this.BackColor = Color.RoyalBlue;

        //}
        /*
        private void OnNewFileCreated(object sender, FileSystemEventArgs e)
        {
            lock (lockObject)
            {
                if (processedFiles.Contains(e.FullPath))
                {
                    // 이미 처리된 파일이면 무시
                    return;
                }

                processedFiles.Add(e.FullPath);
                debounceTimer.Stop(); // 이전 타이머 중지
                debounceTimer.Start(); // 타이머 시작
            }
        }
        */
        //private void DebounceTimer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    lock (lockObject)
        //    {
        //        // UI 스레드에서 버튼 클릭 메서드 호출
        //        if (InvokeRequired)
        //        {
        //            Invoke(new Action(() =>
        //            {
        //                bnHalt_Click(null, null);
        //                bnCleanUp_Click(null, null);
        //                bnInit_Click(null, null);
        //                projDMD();
        //            }));
        //        }
        //        else
        //        {
        //            bnHalt_Click(null, null);
        //            bnCleanUp_Click(null, null);
        //            bnInit_Click(null, null);
        //            projDMD();
        //        }

        //        processedFiles.Clear(); // 처리된 파일 리스트 비우기
        //    }
        //}


        //private void btt_Seqflip_Stop_Click(object sender, EventArgs e)
        //{
        //    if (fileSystemWatcher != null)
        //    {
        //        StopWatching();
        //    }
        //    else
        //    {
        //        txt_SeqFlip.Text = "Stopped watching. Set new path to restart.";
        //    }
        //}

         private void StopWatching()
    {
        if (fileSystemWatcher != null)
        {
                fileSystemWatcher.EnableRaisingEvents = false;
                if(fileSystemWatcher!=null) fileSystemWatcher.Dispose();
                fileSystemWatcher = null; // 더 이상 사용하지 않도록 설정
               // txt_SeqFlip.Text = "Stopped watching. Set new path to restart.";
                this.BackColor = default(Color);
            }
    }

        //private void groupBox7_Enter(object sender, EventArgs e)
        //{

        //}

        //static int CountNonNullElements(List<List<Mat>> list)
        //{
        //    int count = 0;

        //    // Iterate through each list in the outer list
        //    foreach (var innerList in list)
        //    {
        //        // Check if the inner list is not null and contains elements
        //        if (innerList != null && innerList.Count > 0)
        //        {
        //            count++; // Increment count for each non-null list
        //        }
        //    }

        //    return count;
        //}


        public static byte[] FloydSteinbergMat(Mat mat, int row, int col, double nz, double multi)
        {
            Random rand = new Random();
            double u1, u2, r, e, s;
            double[,] res = new double[row, col];
            byte[] returnVal = new byte[row * col];
            for (int i = 0; i < row - 1; i++)
            {
                for (int j = 0; j < col - 1; j++)
                {
                    res[i, j] = mat.At<float>(i, j) * multi;
                    if (res[i, j] > 1) res[i, j] = 1;
                }
            }
            for (int i = 0; i < row - 1; i++)
            {
                u1 = 1.0 - rand.NextDouble(); u2 = 1.0 - rand.NextDouble();
                r = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2) * nz;
                s = (res[i, 0] + r > 0.5) ? 1.0 : 0.0;
                e = s - res[i, 0];
                res[i + 1, 0] = res[i + 1, 0] - 5.0 / 16.0 * e;
                res[i + 1, 1] = res[i + 1, 2] - 1.0 / 16.0 * e;
                res[i, 1] = res[i, 1] - 7.0 / 16.0 * e;
                for (int j = 1; j < col - 1; j++)
                {
                    u1 = 1.0 - rand.NextDouble(); u2 = 1.0 - rand.NextDouble();
                    r = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2) * nz;
                    s = (res[i, j] + r > 0.5) ? 1.0 : 0.0;
                    e = s - res[i, j];
                    res[i + 1, j - 1] = res[i + 1, j - 1] - 3.0 / 16.0 * e;
                    res[i + 1, j] = res[i + 1, j] - 5.0 / 16.0 * e;
                    res[i + 1, j + 1] = res[i + 1, j + 1] - 1.0 / 16.0 * e;
                    res[i, j + 1] = res[i, j + 1] - 7.0 / 16.0 * e;
                }
                u1 = 1.0 - rand.NextDouble(); u2 = 1.0 - rand.NextDouble();
                r = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2) * nz;
                s = (res[i, col - 2] + r > 0.5) ? 1.0 : 0.0;
                e = s - res[i, col - 2];
                res[i + 1, col - 2] = res[i + 1, col - 2] - 3.0 / 16.0 * e;
                res[i + 1, col - 1] = res[i + 1, col - 1] - 5.0 / 16.0 * e;
            }
            for (int j = 1; j < col - 1; j++)
            {
                u1 = 1.0 - rand.NextDouble(); u2 = 1.0 - rand.NextDouble();
                r = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2) * nz;
                s = (res[row - 2, j] + r > 0.5) ? 1.0 : 0.0;
                e = s - res[row - 2, j];
                res[row - 2, j + 1] = res[row - 2, j + 1] - 7.0 / 16.0 * e;
            }
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    returnVal[i * col + j] = (res[i, j] > 0.5) ? (byte)255 : (byte)0;
                }
            }
            return returnVal;
        }

        //public void AffineTF()
        //{

        //    adrow = Convert.ToInt32(txt_adrow.Text);
        //    adcol = Convert.ToInt32(txt_adcol.Text);
        //    int m = 20;


        //    double[,] mag = new double[arr.GetLength(0) * m, arr.GetLength(1) * m];

        //    for (int a = 0; a < arr.GetLength(0); a++)
        //    {
        //        for (int b = 0; b < arr.GetLength(1); b++)
        //        {
        //            for (int i = a * m; i < a * m + adrow; i++)
        //            {
        //                for (int j = b * m; j < (b + 1) * m; j++)
        //                {
        //                    mag[i, j] = 0;
        //                }
        //            }
        //            for (int i = a * m + adrow; i < (a + 1) * m - adrow; i++)
        //            {
        //                for (int j = b * m; j < b * m + adcol; j++)
        //                {
        //                    mag[i, j] = 0;
        //                }
        //                for (int j = b * m + adcol; j < (b + 1) * m - adcol; j++)
        //                {
        //                    mag[i, j] = arr[a, b];
        //                }
        //                for (int j = (b + 1) * m - adcol; j < (b + 1) * m; j++)
        //                {
        //                    mag[i, j] = 0;
        //                }
        //            }
        //            for (int i = (a + 1) * m - adrow; i < (a + 1) * m; i++)
        //            {
        //                for (int j = b * m; j < (b + 1) * m; j++)
        //                {
        //                    mag[i, j] = 0;
        //                }
        //            }
        //        }
        //    }//띄엄 띄엄 정사각형을 행렬 arr에 따라 행렬 mag에 배열

        //    for (int a = 0; a < arr.GetLength(0); a++)
        //    {
        //        for (int b = 0; b < arr.GetLength(1) - 1; b++)
        //        {
        //            if (arr[a, b] != 0 & arr[a, b + 1] != 0)
        //            {
        //                for (int i = a * m + adrow; i < (a + 1) * m - adrow; i++)
        //                {
        //                    for (int j = (b + 1) * m - adcol; j < (b + 1) * m; j++)
        //                    {
        //                        mag[i, j] = arr[a, b];
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    //사각형의 상하좌우 중 우 붙이기

        //    for (int a = 0; a < arr.GetLength(0); a++)
        //    {
        //        for (int b = 1; b < arr.GetLength(1); b++)
        //        {
        //            if (arr[a, b - 1] != 0 & arr[a, b] != 0)
        //            {
        //                for (int i = a * m + adrow; i < (a + 1) * m - adrow; i++)
        //                {
        //                    for (int j = b * m; j < b * m + adcol; j++)
        //                    {
        //                        mag[i, j] = arr[a, b];
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    //사각형의 상하좌우 중 좌 붙이기

        //    for (int a = 0; a < arr.GetLength(0) - 1; a++)
        //    {
        //        for (int b = 0; b < arr.GetLength(1); b++)
        //        {
        //            if (arr[a, b] != 0 & arr[a + 1, b] != 0)
        //            {
        //                for (int j = b * m + adcol; j < (b + 1) * m - adcol; j++)
        //                {
        //                    for (int i = (a + 1) * m - adrow; i < (a + 1) * m; i++)
        //                    {
        //                        mag[i, j] = arr[a, b];
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    //사각형의 상하좌우 중 하 붙이기

        //    for (int a = 1; a < arr.GetLength(0); a++)
        //    {
        //        for (int b = 0; b < arr.GetLength(1); b++)
        //        {
        //            if (arr[a - 1, b] != 0 & arr[a, b] != 0)
        //            {
        //                for (int j = b * m + adcol; j < (b + 1) * m - adcol; j++)
        //                {
        //                    for (int i = a * m; i < a * m + adrow; i++)
        //                    {
        //                        mag[i, j] = arr[a, b];
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    // 사각형의 상하좌우 중 상 붙이기
        //    for (int a = 0; a < arr.GetLength(0) - 1; a++)
        //    {
        //        for (int b = 0; b < arr.GetLength(1) - 1; b++)
        //        {
        //            if (arr[a, b] != 0 & arr[a + 1, b] != 0 & arr[a, b + 1] != 0 & arr[a + 1, b + 1] != 0)
        //            {
        //                for (int i = (a + 1) * m - adrow; i < (a + 1) * m; i++)
        //                {
        //                    for (int j = (b + 1) * m - adcol; j < (b + 1) * m; j++)
        //                    {
        //                        mag[i, j] = arr[a, b];
        //                    }
        //                    for (int j = (b + 1) * m; j < (b + 1) * m + adcol; j++)
        //                    {
        //                        mag[i, j] = arr[a, b + 1];
        //                    }
        //                }
        //                for (int i = (a + 1) * m; i < (a + 1) * m + adrow; i++)
        //                {
        //                    for (int j = (b + 1) * m - adcol; j < (b + 1) * m; j++)
        //                    {
        //                        mag[i, j] = arr[a + 1, b];
        //                    }
        //                    for (int j = (b + 1) * m; j < (b + 1) * m + adcol; j++)
        //                    {
        //                        mag[i, j] = arr[a + 1, b + 1];
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    //남은 모서리 채우기

        //    int A = mag.GetLength(0);
        //    int B = mag.GetLength(1);


        //    Mat image = new Mat(A, B, MatType.CV_8UC1);
        //    //이미지 Mat 생성

        //    for (int y = 0; y < image.Rows; y++)
        //    {
        //        for (int x = 0; x < image.Cols; x++)
        //        {
        //            byte pixelValue = (byte)mag[y, x];
        //            image.Set<byte>(y, x, pixelValue);
        //        }
        //    }
        //    //이미지 Mat에 m배 확대되어있는 행렬 데이터 삽입
        //    M = Convert.ToDouble(txt_mag.Text) / m;
        //    double Offset = 0;
        //    angle = Convert.ToDouble(txt_angle.Text);

        //    Mat Mag = new Mat();
        //    Cv2.Resize(image, Mag, new OpenCvSharp.Size(mag.GetLength(0) * M, mag.GetLength(1) * M), 0, 0, InterpolationFlags.Nearest);
        //    // Mat image를 M배 확대한 Mat Mag를 얻음

        //    Mat dst = new Mat(768, 1024, MatType.CV_8UC1);

        //    int g = Convert.ToInt32(Math.Truncate(0.5 * (dst.Rows - Mag.Rows)));
        //    int f = Convert.ToInt32(Math.Truncate(0.5 * (dst.Rows + Mag.Rows)));
        //    int c = Convert.ToInt32(Math.Truncate(0.5 * (dst.Cols - Mag.Cols)));
        //    int d = Convert.ToInt32(Math.Truncate(0.5 * (dst.Cols + Mag.Cols)));

        //    for (int i = 0; i < g; i++)
        //    {
        //        for (int j = 0; j < dst.Cols; j++)
        //        {
        //            dst.Set<double>(i, j, Offset);
        //        }
        //    }

        //    for (int i = g; i < f; i++)
        //    {
        //        for (int j = 0; j < c; j++)
        //        {
        //            dst.Set<double>(i, j, Offset);
        //        }
        //        for (int j = c; j < d; j++)
        //        {
        //            dst.Set<double>(i, j, Mag.At<double>(i - g, j - c));
        //        }
        //        for (int j = d; j < dst.Cols; j++)
        //        {
        //            dst.Set<double>(i, j, Offset);
        //        }
        //    }

        //    for (int i = f; i < dst.Rows; i++)
        //    {
        //        for (int j = 0; j < dst.Cols; j++)
        //        {
        //            dst.Set<double>(i, j, Offset);
        //        }
        //    }
        //    // Mat dst는 1024x768 pixel을 갖고, 이미지가 중심에 있다.

        //    Mat Affine = new Mat();

        //    Mat matrix = Cv2.GetRotationMatrix2D(new Point2f(dst.Width / 2, dst.Height / 2), angle, 1.0);
        //    Cv2.WarpAffine(dst, Affine, matrix, new OpenCvSharp.Size(dst.Width, dst.Height));
        //    //Mat dst를 아핀 변환하여 얻은 Mat Affine
        //    Affineimage = BitmapConverter.ToBitmap(Affine);
        //    MAffine = Affine;
        //    txtbox_loadseq.Text = "Draw Image";
        //}
    }
}
