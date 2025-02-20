using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ALP_CSharp_DotNet
{
    public partial class AlpSampleForm : Form
    {
        UInt32 m_DevId, m_SeqId1, m_SeqId2;
        Int32 m_DmdWidth, m_DmdHeight;

        public AlpSampleForm()
        {
            InitializeComponent();

            m_DmdWidth = m_DmdHeight = 0;
            m_DevId = UInt32.MaxValue;
        }

        // Convert error string
        private string AlpErrorString(AlpImport.Result result)
        {
            return String.Format("{0}", result);
        }

        private void bnInit_Click(object sender, EventArgs e)
        {
            AlpImport.Result result;

            // allocate one ALP device
            result = AlpImport.DevAlloc(0, 0, ref m_DevId); //DevAlloc: 하드웨어 셋 할당, ALP 핸들 반환, AlpDevFree로 해제 가능
            // DevAlloc(DeviceNum=0, InitFlag=0, ALP 장치 식별자를 기록할 변수의 주소: ref m_DevId)
            txResult.Text = "DevAlloc " + AlpErrorString(result);
            if (AlpImport.Result.ALP_OK != result) return;  // error -> exit

            // determine image data size by DMD type
            Int32 DmdType = Int32.MaxValue;
            m_DmdWidth = 0; m_DmdHeight = 0;
            AlpImport.DevInquire(m_DevId, AlpImport.DevTypes.ALP_DEV_DMDTYPE, ref DmdType); // DevInquire: ALP 디바이스에 특정된 파라미터 세팅을 조회한다.
            // DevInquire( 어떤 장치?: m_Devldm, 어떤 값을 조회할래?: DevTypes.ALP_DEV_DMDTYPE, 정보를 어디에 기록할래?: ref DmdType)
            AlpImport.DevInquire(m_DevId, AlpImport.DevTypes.ALP_DEV_DISPLAY_WIDTH, ref m_DmdWidth);
            // 위에서 이어진다. m_Devld 장치에서 DMD 거울의 column 수를 조회해서 m_DmdWidth에 기록한다.
            AlpImport.DevInquire(m_DevId, AlpImport.DevTypes.ALP_DEV_DISPLAY_HEIGHT, ref m_DmdHeight);
            //  m_Devld 장치에서 DMD 거울의 row 수를 조회해서 m_DmdHeight에 기록한다.
            
            switch ((AlpImport.DmdTypes)DmdType) // DMD의 버전에 따라서 각각의 픽셀을 설정하는 조건문
            {
                case AlpImport.DmdTypes.ALP_DMDTYPE_XGA: //  (AlpDevControl, AlpDevInquire)의 상수값 ALP_DMDTYPE_XGA=1
                case AlpImport.DmdTypes.ALP_DMDTYPE_XGA_055A: //  
                case AlpImport.DmdTypes.ALP_DMDTYPE_XGA_055X: // ALP_DMDTYPE_XGA_055X=6
                case AlpImport.DmdTypes.ALP_DMDTYPE_XGA_07A: // ALP_DMDTYPE_XGA_07A = 4
                    txResult.Text = String.Format("XGA DMD {0}", DmdType); 
                    m_DmdWidth = 1024;  // fall-back: old API versions did not support ALP_DEV_DISPLAY_WIDTH and _HEIGHT
                    m_DmdHeight = 768;  // 위의 경우에는 다음과 같이 설정된다.
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

            // Allocate 2 sequences of 1 image, each
            result = AlpImport.SeqAlloc(m_DevId, 1, 1, ref m_SeqId1);
            // 사진 시퀀스에 대한 ALP 메모리 제공. AlpSeqPut 함수를 이용해 ALP RAM에 사진을 로드 가능. AlpSeqFree로 해제
            // SeqAlloc( ALP 디바이스 지정, 표시할 패턴의 비트 심도(Gray:1~12, FLEX_PWM:1~32), 시퀀스에 속한 그림의 수, ALP 식별자를 기록할 변수의 주소)
            txResult.Text = "SeqAlloc1 " + AlpErrorString(result);
            if (AlpImport.Result.ALP_OK != result) return;  // error -> exit

            result = AlpImport.SeqAlloc(m_DevId, 1, 1, ref m_SeqId2);
            // m_Devld의 장치에서 비트 심도 1, 시퀀스의 그림의 수는 1개로 사진 시퀀스에 대한 ALP 메모리를 제공하고, m_Seqld2에 이를 기록한다.
            txResult.Text = "SeqAlloc2 " + AlpErrorString(result);
            if (AlpImport.Result.ALP_OK != result) return;  // error -> exit

            // Example: pulse Synch Output 1 each first of three frames.
			// (Not available prior to ALP-4)
            result = AlpImport.DevControlEx_SynchGate(m_DevId, 1, true, 1,0,0); // 단순한 32비트 숫자에 맞지않는 데이터 객체
            // m_Devld에서 하나의 gate로 Polarity high 하게(true) params byte[] Gate
            txResult.Text = "SynchGate1 " + AlpErrorString(result);
            if (AlpImport.Result.ALP_OK != result) return;  // error -> exit
        }

        private void bnSeq1_Click(object sender, EventArgs e)
        {
            Byte[] ChessBoard64 = new Byte[m_DmdWidth * m_DmdHeight * 10];
            // Fill ChessBoard64 with a Chess Board pattern
            for (int k = 0; k < 10; k++)
            {
                for (Int32 y = 0; y < m_DmdHeight; y++)
                    for (Int32 x = 0; x < m_DmdWidth; x++)
                        if (((x & 64) == 0) ^ ((y & 64) == 0))
                            ChessBoard64[k*m_DmdWidth * m_DmdHeight + y * m_DmdWidth + x] = 0;
                        else
                            ChessBoard64[y * m_DmdWidth + x] = 255;   // >=128: white
            }
            AlpImport.Result result;
            // Load image data from PC memory to ALP memory
            result = AlpImport.SeqPut(m_DevId, m_SeqId1, 0, 0, ref ChessBoard64);
            //SeqPut(장치 이름, 시퀀스, 오프셋(기본값 0), 시퀀스에 로드할 사진 수, 사용할 데이터의 포인터(주소 or 이름))
            txResult.Text = "SeqPut " + AlpErrorString(result);

            var proj_step = (int)AlpImport.DevTypes.ALP_LEVEL_HIGH;
            result = AlpImport.ProjControl(m_DevId, AlpImport.ProjTypes.ALP_PROJ_MODE, (int)AlpImport.ProjModes.ALP_SLAVE);
            //AlpSequent( 장치 이름, ControlType, Controlvalue: ProjModes.ALP_SLAVE )  
            txResult.Text = txResult.Text + "Trig Proj Control PROJ_MODE" + AlpErrorString(result);
            txResult.AppendText(Environment.NewLine);
            

            result = AlpImport.DevControl(m_DevId, AlpImport.DevTypes.ALP_TRIGGER_EDGE, proj_step);
            // DevControl(장치 이름, ControlType: 수정할 매개변수, ControlValue: 변수의 값): ALP의 표시 속성을 변경하는데 사용됨.
            txResult.Text = txResult.Text + "Trig DEV Control TRIGGER_EDGE" + AlpErrorString(result) + "PROJ = " + Convert.ToString(proj_step);
            txResult.AppendText(Environment.NewLine);


            // Start display
            if (AlpImport.Result.ALP_OK != result) return;  // error -> exit
            result = AlpImport.ProjStartCont(m_DevId, m_SeqId1);
            // ProhStartCont(디바이스, 시퀀스): 지정된 시퀀스를 무한 반복으로 표시. AlpProjHalt 또는 AlpDevHalt를 이용해 중지 가능
            txResult.Text = "ProjStartCont " + AlpErrorString(result);
        }

        private void bnSeq2_Click(object sender, EventArgs e)
        {
            Byte[] Stripes32 = new Byte[m_DmdWidth * m_DmdHeight];
            // Fill Stripes32 with vertical stripes
            for (Int32 y = 0; y < m_DmdHeight; y++)
                for (Int32 x = 0; x < m_DmdWidth; x++)
                    if (((x & 32) == 0))
                        Stripes32[y * m_DmdWidth + x] = 0;
                    else
                        Stripes32[y * m_DmdWidth + x] = 255;   // >=128: white
           
            AlpImport.Result result;
            // Load image data from PC memory to ALP memory
            result = AlpImport.SeqPut(m_DevId, m_SeqId2, 0, 0, ref Stripes32);
            // m_Devid 장치에 m_seqld2 시퀀스로 오프셋 0, 모든 사진을 로드(기본값)하며, 사진은 Stripes32를 사용한다.
            txResult.Text = "SeqPut " + AlpErrorString(result);
            // Start display
            if (AlpImport.Result.ALP_OK != result) return;  // error -> exit
            result = AlpImport.ProjStartCont(m_DevId, m_SeqId2);
            // m_Devid에 m_Seqid2를 무한반복으로 표시한다.
            txResult.Text = "ProjStartCont " + AlpErrorString(result);
        }

        private void bnHalt_Click(object sender, EventArgs e)
        {
            AlpImport.Result result = AlpImport.ProjHalt(m_DevId); // 시퀀스 표시를 중지하고, ALP를 유휴대기상태 ALP_PROJ_IDLE로 설정
            // m_Devid의 반복을 멈춰라!
            txResult.Text = "ProjHalt " + AlpErrorString(result);
        }

        private void bnCleanUp_Click(object sender, EventArgs e)
        {
            // Disable SynchGate1 output: Omit "Gate" parameter
			// (Not available prior to ALP-4)
            AlpImport.DevControlEx_SynchGate(m_DevId, 1, true);

            // Recommendation: always call DevHalt() before DevFree()
            AlpImport.Result result = AlpImport.DevFree(m_DevId); // Alloc을 해제하여 하드웨어 셋 할당을 해제
            txResult.Text = "DevFree " + AlpErrorString(result);
            if (AlpImport.Result.ALP_OK != result) return;  // error -> exit
            m_DevId = UInt32.MaxValue;
        }
    }
}
