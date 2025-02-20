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
            result = AlpImport.DevAlloc(0, 0, ref m_DevId); //DevAlloc: �ϵ���� �� �Ҵ�, ALP �ڵ� ��ȯ, AlpDevFree�� ���� ����
            // DevAlloc(DeviceNum=0, InitFlag=0, ALP ��ġ �ĺ��ڸ� ����� ������ �ּ�: ref m_DevId)
            txResult.Text = "DevAlloc " + AlpErrorString(result);
            if (AlpImport.Result.ALP_OK != result) return;  // error -> exit

            // determine image data size by DMD type
            Int32 DmdType = Int32.MaxValue;
            m_DmdWidth = 0; m_DmdHeight = 0;
            AlpImport.DevInquire(m_DevId, AlpImport.DevTypes.ALP_DEV_DMDTYPE, ref DmdType); // DevInquire: ALP ����̽��� Ư���� �Ķ���� ������ ��ȸ�Ѵ�.
            // DevInquire( � ��ġ?: m_Devldm, � ���� ��ȸ�ҷ�?: DevTypes.ALP_DEV_DMDTYPE, ������ ��� ����ҷ�?: ref DmdType)
            AlpImport.DevInquire(m_DevId, AlpImport.DevTypes.ALP_DEV_DISPLAY_WIDTH, ref m_DmdWidth);
            // ������ �̾�����. m_Devld ��ġ���� DMD �ſ��� column ���� ��ȸ�ؼ� m_DmdWidth�� ����Ѵ�.
            AlpImport.DevInquire(m_DevId, AlpImport.DevTypes.ALP_DEV_DISPLAY_HEIGHT, ref m_DmdHeight);
            //  m_Devld ��ġ���� DMD �ſ��� row ���� ��ȸ�ؼ� m_DmdHeight�� ����Ѵ�.
            
            switch ((AlpImport.DmdTypes)DmdType) // DMD�� ������ ���� ������ �ȼ��� �����ϴ� ���ǹ�
            {
                case AlpImport.DmdTypes.ALP_DMDTYPE_XGA: //  (AlpDevControl, AlpDevInquire)�� ����� ALP_DMDTYPE_XGA=1
                case AlpImport.DmdTypes.ALP_DMDTYPE_XGA_055A: //  
                case AlpImport.DmdTypes.ALP_DMDTYPE_XGA_055X: // ALP_DMDTYPE_XGA_055X=6
                case AlpImport.DmdTypes.ALP_DMDTYPE_XGA_07A: // ALP_DMDTYPE_XGA_07A = 4
                    txResult.Text = String.Format("XGA DMD {0}", DmdType); 
                    m_DmdWidth = 1024;  // fall-back: old API versions did not support ALP_DEV_DISPLAY_WIDTH and _HEIGHT
                    m_DmdHeight = 768;  // ���� ��쿡�� ������ ���� �����ȴ�.
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
            // ���� �������� ���� ALP �޸� ����. AlpSeqPut �Լ��� �̿��� ALP RAM�� ������ �ε� ����. AlpSeqFree�� ����
            // SeqAlloc( ALP ����̽� ����, ǥ���� ������ ��Ʈ �ɵ�(Gray:1~12, FLEX_PWM:1~32), �������� ���� �׸��� ��, ALP �ĺ��ڸ� ����� ������ �ּ�)
            txResult.Text = "SeqAlloc1 " + AlpErrorString(result);
            if (AlpImport.Result.ALP_OK != result) return;  // error -> exit

            result = AlpImport.SeqAlloc(m_DevId, 1, 1, ref m_SeqId2);
            // m_Devld�� ��ġ���� ��Ʈ �ɵ� 1, �������� �׸��� ���� 1���� ���� �������� ���� ALP �޸𸮸� �����ϰ�, m_Seqld2�� �̸� ����Ѵ�.
            txResult.Text = "SeqAlloc2 " + AlpErrorString(result);
            if (AlpImport.Result.ALP_OK != result) return;  // error -> exit

            // Example: pulse Synch Output 1 each first of three frames.
			// (Not available prior to ALP-4)
            result = AlpImport.DevControlEx_SynchGate(m_DevId, 1, true, 1,0,0); // �ܼ��� 32��Ʈ ���ڿ� �����ʴ� ������ ��ü
            // m_Devld���� �ϳ��� gate�� Polarity high �ϰ�(true) params byte[] Gate
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
            //SeqPut(��ġ �̸�, ������, ������(�⺻�� 0), �������� �ε��� ���� ��, ����� �������� ������(�ּ� or �̸�))
            txResult.Text = "SeqPut " + AlpErrorString(result);

            var proj_step = (int)AlpImport.DevTypes.ALP_LEVEL_HIGH;
            result = AlpImport.ProjControl(m_DevId, AlpImport.ProjTypes.ALP_PROJ_MODE, (int)AlpImport.ProjModes.ALP_SLAVE);
            //AlpSequent( ��ġ �̸�, ControlType, Controlvalue: ProjModes.ALP_SLAVE )  
            txResult.Text = txResult.Text + "Trig Proj Control PROJ_MODE" + AlpErrorString(result);
            txResult.AppendText(Environment.NewLine);
            

            result = AlpImport.DevControl(m_DevId, AlpImport.DevTypes.ALP_TRIGGER_EDGE, proj_step);
            // DevControl(��ġ �̸�, ControlType: ������ �Ű�����, ControlValue: ������ ��): ALP�� ǥ�� �Ӽ��� �����ϴµ� ����.
            txResult.Text = txResult.Text + "Trig DEV Control TRIGGER_EDGE" + AlpErrorString(result) + "PROJ = " + Convert.ToString(proj_step);
            txResult.AppendText(Environment.NewLine);


            // Start display
            if (AlpImport.Result.ALP_OK != result) return;  // error -> exit
            result = AlpImport.ProjStartCont(m_DevId, m_SeqId1);
            // ProhStartCont(����̽�, ������): ������ �������� ���� �ݺ����� ǥ��. AlpProjHalt �Ǵ� AlpDevHalt�� �̿��� ���� ����
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
            // m_Devid ��ġ�� m_seqld2 �������� ������ 0, ��� ������ �ε�(�⺻��)�ϸ�, ������ Stripes32�� ����Ѵ�.
            txResult.Text = "SeqPut " + AlpErrorString(result);
            // Start display
            if (AlpImport.Result.ALP_OK != result) return;  // error -> exit
            result = AlpImport.ProjStartCont(m_DevId, m_SeqId2);
            // m_Devid�� m_Seqid2�� ���ѹݺ����� ǥ���Ѵ�.
            txResult.Text = "ProjStartCont " + AlpErrorString(result);
        }

        private void bnHalt_Click(object sender, EventArgs e)
        {
            AlpImport.Result result = AlpImport.ProjHalt(m_DevId); // ������ ǥ�ø� �����ϰ�, ALP�� ���޴����� ALP_PROJ_IDLE�� ����
            // m_Devid�� �ݺ��� �����!
            txResult.Text = "ProjHalt " + AlpErrorString(result);
        }

        private void bnCleanUp_Click(object sender, EventArgs e)
        {
            // Disable SynchGate1 output: Omit "Gate" parameter
			// (Not available prior to ALP-4)
            AlpImport.DevControlEx_SynchGate(m_DevId, 1, true);

            // Recommendation: always call DevHalt() before DevFree()
            AlpImport.Result result = AlpImport.DevFree(m_DevId); // Alloc�� �����Ͽ� �ϵ���� �� �Ҵ��� ����
            txResult.Text = "DevFree " + AlpErrorString(result);
            if (AlpImport.Result.ALP_OK != result) return;  // error -> exit
            m_DevId = UInt32.MaxValue;
        }
    }
}
