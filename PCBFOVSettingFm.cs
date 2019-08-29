using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace AutoAPP
{
    public partial class PCBFOVSettingFm :Form
    {
        public PCBFOVSettingFm()
        {
            InitializeComponent();
        }
        private string strConfigIniPath = @"D:\EYSPI\Bin\Config";
        private string iniSec = "fovpcb";
        private string iniKeyLine1 = "Line1";
        private string iniKeyLine2 = "Line2";
        private string iniKeyLine3 = "Line3";
        private string iniKeyLine4 = "Line4";
        private string iniKeyLine5 = "Line5";
        private string iniKeyLine6 = "Line6";
        private string iniKeyLine7 = "Line7";
        private string iniKeyLine8 = "Line8";
        private string iniKeyLine9 = "Line9";
        private string iniKeyLine10 = "Line10";
        private string iniKeyLine11 = "Line11";
        private string iniKeyLine12 = "Line12";
        private string iniKeyLine13 = "Line13";
        private string iniKeyLine14 = "Line14";
        private string iniKeyLine15 = "Line15";
        private string iniKeyLine16 = "Line16";
        private string iniKeyLine17 = "Line17";
        private string iniKeyLine18 = "Line18";
        private string RS_AT = "@";
        private string RS_UP = "|";
        private void btnSavePCBFOVSetting_Click(object sender, EventArgs e)
        {

            string strOnLinePCBFOVIniFilePath = Path.Combine(strConfigIniPath, "FOVPCB.ini");


            string strLineInfo1 = this.tbxLineName1.Text + RS_AT + this.tbxLineIP1.Text + RS_AT + this.chkLineDis1.Checked.ToString().ToUpper();
            string strLineInfo2 = this.tbxLineName2.Text + RS_AT + this.tbxLineIP2.Text + RS_AT + this.chkLineDis2.Checked.ToString().ToUpper();
            string strLineInfo3 = this.tbxLineName3.Text + RS_AT + this.tbxLineIP3.Text + RS_AT + this.chkLineDis3.Checked.ToString().ToUpper();
            string strLineInfo4 = this.tbxLineName4.Text + RS_AT + this.tbxLineIP4.Text + RS_AT + this.chkLineDis4.Checked.ToString().ToUpper();
            string strLineInfo5 = this.tbxLineName5.Text + RS_AT + this.tbxLineIP5.Text + RS_AT + this.chkLineDis5.Checked.ToString().ToUpper();
            string strLineInfo6 = this.tbxLineName6.Text + RS_AT + this.tbxLineIP6.Text + RS_AT + this.chkLineDis6.Checked.ToString().ToUpper();
            string strLineInfo7 = this.tbxLineName7.Text + RS_AT + this.tbxLineIP7.Text + RS_AT + this.chkLineDis7.Checked.ToString().ToUpper();
            string strLineInfo8 = this.tbxLineName8.Text + RS_AT + this.tbxLineIP8.Text + RS_AT + this.chkLineDis8.Checked.ToString().ToUpper();
            string strLineInfo9 = this.tbxLineName9.Text + RS_AT + this.tbxLineIP9.Text + RS_AT + this.chkLineDis9.Checked.ToString().ToUpper();
            string strLineInfo10 = this.tbxLineName10.Text + RS_AT + this.tbxLineIP10.Text + RS_AT + this.chkLineDis10.Checked.ToString().ToUpper();
            string strLineInfo11 = this.tbxLineName11.Text + RS_AT + this.tbxLineIP11.Text + RS_AT + this.chkLineDis11.Checked.ToString().ToUpper();
            string strLineInfo12 = this.tbxLineName12.Text + RS_AT + this.tbxLineIP12.Text + RS_AT + this.chkLineDis12.Checked.ToString().ToUpper();
            string strLineInfo13 = this.tbxLineName13.Text + RS_AT + this.tbxLineIP13.Text + RS_AT + this.chkLineDis13.Checked.ToString().ToUpper();
            string strLineInfo14 = this.tbxLineName14.Text + RS_AT + this.tbxLineIP14.Text + RS_AT + this.chkLineDis14.Checked.ToString().ToUpper();
            string strLineInfo15 = this.tbxLineName15.Text + RS_AT + this.tbxLineIP15.Text + RS_AT + this.chkLineDis15.Checked.ToString().ToUpper();
            string strLineInfo16 = this.tbxLineName16.Text + RS_AT + this.tbxLineIP16.Text + RS_AT + this.chkLineDis16.Checked.ToString().ToUpper();


            

            if (File.Exists(strOnLinePCBFOVIniFilePath))
            {

                INIFileHelper.WriteIniData(iniSec, iniKeyLine1, strLineInfo1, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine2, strLineInfo2, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine3, strLineInfo3, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine4, strLineInfo4, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine5, strLineInfo5, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine6, strLineInfo6, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine7, strLineInfo7, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine8, strLineInfo8, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine9, strLineInfo9, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine10, strLineInfo10, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine11, strLineInfo11, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine12, strLineInfo12, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine13, strLineInfo13, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine14, strLineInfo14, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine15, strLineInfo15, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine16, strLineInfo16, strOnLinePCBFOVIniFilePath);
                //INIFileHelper.WriteIniData(iniSec, iniKeyLine17, strLineInfo2, strOnLinePCBFOVIniFilePath);
                //INIFileHelper.WriteIniData(iniSec, iniKeyLine2, strLineInfo2, strOnLinePCBFOVIniFilePath);
                

            }
            else
            {
                File.WriteAllText(strOnLinePCBFOVIniFilePath, string.Empty, Encoding.Default);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine1, strLineInfo1, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine2, strLineInfo2, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine3, strLineInfo3, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine4, strLineInfo4, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine5, strLineInfo5, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine6, strLineInfo6, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine7, strLineInfo7, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine8, strLineInfo8, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine9, strLineInfo9, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine10, strLineInfo10, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine11, strLineInfo11, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine12, strLineInfo12, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine13, strLineInfo13, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine14, strLineInfo14, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine15, strLineInfo15, strOnLinePCBFOVIniFilePath);
                INIFileHelper.WriteIniData(iniSec, iniKeyLine16, strLineInfo16, strOnLinePCBFOVIniFilePath);
            }
            int iCount = 0;
            string strRealInfo = string.Empty;
            if (this.chkLineDis1.Checked) { iCount++; strRealInfo += strLineInfo1 + RS_UP; }
            if (this.chkLineDis2.Checked) {iCount++; strRealInfo += strLineInfo2 + RS_UP;}
            if (this.chkLineDis3.Checked) {iCount++; strRealInfo += strLineInfo3 + RS_UP;}
            if (this.chkLineDis4.Checked) {iCount++; strRealInfo += strLineInfo4 + RS_UP;}
            if (this.chkLineDis5.Checked) {iCount++; strRealInfo += strLineInfo5 + RS_UP;}
            if (this.chkLineDis6.Checked) {iCount++; strRealInfo += strLineInfo6 + RS_UP;}
            if (this.chkLineDis7.Checked) {iCount++; strRealInfo += strLineInfo7 + RS_UP;}
            if (this.chkLineDis8.Checked) {iCount++; strRealInfo += strLineInfo8 + RS_UP;}
            if (this.chkLineDis9.Checked) {iCount++; strRealInfo += strLineInfo9 + RS_UP;}
            if (this.chkLineDis10.Checked) {iCount++; strRealInfo += strLineInfo10 + RS_UP;}
            if (this.chkLineDis11.Checked){ iCount++; strRealInfo += strLineInfo11 + RS_UP;}
            if (this.chkLineDis12.Checked) {iCount++; strRealInfo += strLineInfo12 + RS_UP;}
            if (this.chkLineDis13.Checked) {iCount++; strRealInfo += strLineInfo13 + RS_UP;}
            if (this.chkLineDis14.Checked) {iCount++; strRealInfo += strLineInfo14 + RS_UP;}
            if (this.chkLineDis15.Checked) {iCount++; strRealInfo += strLineInfo15 + RS_UP;}
            if (this.chkLineDis16.Checked) { iCount++; strRealInfo += strLineInfo16; }

            INIFileHelper.WriteIniData("fovpcb", "iCount", iCount+"", strOnLinePCBFOVIniFilePath);
            INIFileHelper.WriteIniData("fovpcb", "realInfo", strRealInfo , strOnLinePCBFOVIniFilePath);
            bool bTmp = true;
            if (iCount > 0)
            {
                string[] arrStrReal = strRealInfo.Split('|');
                foreach (string LineInfo in arrStrReal)
                {
                    if (string.IsNullOrEmpty(LineInfo)) continue;
                    string strTmpIP = LineInfo.Split('@')[1];
                    if (Directory.Exists("\\\\" + strTmpIP + "\\EYSPI"))
                    {

                    }
                    else
                    {
                        bTmp = false;
                        MessageBox.Show("网络链接错误 或 设备共享文件夹设置错误 !IP=>" + strTmpIP + "  线体=>" + LineInfo.Split('@')[0]);
                    }

                }

            }
            if (bTmp)
            MessageBox.Show("保存成功");
        }

        private void showSettingView()
        {
            string strOnLinePCBFOVIniFilePath = Path.Combine(strConfigIniPath, "FOVPCB.ini");
            string strLineInfo1 = this.tbxLineName1.Text + RS_AT + this.tbxLineIP1.Text + RS_AT + this.chkLineDis1.Checked.ToString().ToUpper();
            string strLineInfo2 = this.tbxLineName2.Text + RS_AT + this.tbxLineIP2.Text + RS_AT + this.chkLineDis2.Checked.ToString().ToUpper();
            string strLineInfo3 = this.tbxLineName3.Text + RS_AT + this.tbxLineIP3.Text + RS_AT + this.chkLineDis3.Checked.ToString().ToUpper();
            string strLineInfo4 = this.tbxLineName4.Text + RS_AT + this.tbxLineIP4.Text + RS_AT + this.chkLineDis4.Checked.ToString().ToUpper();
            string strLineInfo5 = this.tbxLineName5.Text + RS_AT + this.tbxLineIP5.Text + RS_AT + this.chkLineDis5.Checked.ToString().ToUpper();
            string strLineInfo6 = this.tbxLineName6.Text + RS_AT + this.tbxLineIP6.Text + RS_AT + this.chkLineDis6.Checked.ToString().ToUpper();
            string strLineInfo7 = this.tbxLineName7.Text + RS_AT + this.tbxLineIP7.Text + RS_AT + this.chkLineDis7.Checked.ToString().ToUpper();
            string strLineInfo8 = this.tbxLineName8.Text + RS_AT + this.tbxLineIP8.Text + RS_AT + this.chkLineDis8.Checked.ToString().ToUpper();
            string strLineInfo9 = this.tbxLineName9.Text + RS_AT + this.tbxLineIP9.Text + RS_AT + this.chkLineDis9.Checked.ToString().ToUpper();
            string strLineInfo10 = this.tbxLineName10.Text + RS_AT + this.tbxLineIP10.Text + RS_AT + this.chkLineDis10.Checked.ToString().ToUpper();
            string strLineInfo11 = this.tbxLineName11.Text + RS_AT + this.tbxLineIP11.Text + RS_AT + this.chkLineDis11.Checked.ToString().ToUpper();
            string strLineInfo12 = this.tbxLineName12.Text + RS_AT + this.tbxLineIP12.Text + RS_AT + this.chkLineDis12.Checked.ToString().ToUpper();
            string strLineInfo13 = this.tbxLineName13.Text + RS_AT + this.tbxLineIP13.Text + RS_AT + this.chkLineDis13.Checked.ToString().ToUpper();
            string strLineInfo14 = this.tbxLineName14.Text + RS_AT + this.tbxLineIP14.Text + RS_AT + this.chkLineDis14.Checked.ToString().ToUpper();
            string strLineInfo15 = this.tbxLineName15.Text + RS_AT + this.tbxLineIP15.Text + RS_AT + this.chkLineDis15.Checked.ToString().ToUpper();
            string strLineInfo16 = this.tbxLineName16.Text + RS_AT + this.tbxLineIP16.Text + RS_AT + this.chkLineDis16.Checked.ToString().ToUpper();
            if (File.Exists(strOnLinePCBFOVIniFilePath))
            {
                 strLineInfo1 = INIFileHelper.ReadIniData(iniSec, iniKeyLine1, string.Empty, strOnLinePCBFOVIniFilePath);
                 strLineInfo2 = INIFileHelper.ReadIniData(iniSec, iniKeyLine2, string.Empty, strOnLinePCBFOVIniFilePath);
                 strLineInfo3 = INIFileHelper.ReadIniData(iniSec, iniKeyLine3, string.Empty, strOnLinePCBFOVIniFilePath);
                 strLineInfo4 = INIFileHelper.ReadIniData(iniSec, iniKeyLine4, string.Empty, strOnLinePCBFOVIniFilePath);
                 strLineInfo5 = INIFileHelper.ReadIniData(iniSec, iniKeyLine5, string.Empty, strOnLinePCBFOVIniFilePath);
                 strLineInfo6 = INIFileHelper.ReadIniData(iniSec, iniKeyLine6, string.Empty, strOnLinePCBFOVIniFilePath);
                 strLineInfo7 = INIFileHelper.ReadIniData(iniSec, iniKeyLine7, string.Empty, strOnLinePCBFOVIniFilePath);
                 strLineInfo8 = INIFileHelper.ReadIniData(iniSec, iniKeyLine8, string.Empty, strOnLinePCBFOVIniFilePath);
                 strLineInfo9 = INIFileHelper.ReadIniData(iniSec, iniKeyLine9, string.Empty, strOnLinePCBFOVIniFilePath);
                 strLineInfo10 = INIFileHelper.ReadIniData(iniSec, iniKeyLine10, string.Empty, strOnLinePCBFOVIniFilePath);
                 strLineInfo11 = INIFileHelper.ReadIniData(iniSec, iniKeyLine11, string.Empty, strOnLinePCBFOVIniFilePath);
                 strLineInfo12 = INIFileHelper.ReadIniData(iniSec, iniKeyLine12, string.Empty, strOnLinePCBFOVIniFilePath);
                 strLineInfo13 = INIFileHelper.ReadIniData(iniSec, iniKeyLine13, string.Empty, strOnLinePCBFOVIniFilePath);
                 strLineInfo14 = INIFileHelper.ReadIniData(iniSec, iniKeyLine14, string.Empty, strOnLinePCBFOVIniFilePath);
                 strLineInfo15 = INIFileHelper.ReadIniData(iniSec, iniKeyLine15, string.Empty, strOnLinePCBFOVIniFilePath);
                 strLineInfo16 = INIFileHelper.ReadIniData(iniSec, iniKeyLine16, string.Empty, strOnLinePCBFOVIniFilePath);

                 this.tbxLineName1.Text = strLineInfo1.Split('@')[0];
                 this.tbxLineIP1.Text = strLineInfo1.Split('@')[1]; 
                 this.chkLineDis1.Checked =  bool.Parse(strLineInfo1.Split('@')[2]);

                 this.tbxLineName2.Text = strLineInfo2.Split('@')[0];
                 this.tbxLineIP2.Text = strLineInfo2.Split('@')[1];
                 this.chkLineDis2.Checked = bool.Parse(strLineInfo2.Split('@')[2]);

                 this.tbxLineName3.Text = strLineInfo3.Split('@')[0];
                 this.tbxLineIP3.Text = strLineInfo3.Split('@')[1];
                 this.chkLineDis3.Checked = bool.Parse(strLineInfo3.Split('@')[2]);

                 this.tbxLineName4.Text = strLineInfo4.Split('@')[0];
                 this.tbxLineIP4.Text = strLineInfo4.Split('@')[1];
                 this.chkLineDis4.Checked = bool.Parse(strLineInfo4.Split('@')[2]);

                 this.tbxLineName5.Text = strLineInfo5.Split('@')[0];
                 this.tbxLineIP5.Text = strLineInfo5.Split('@')[1];
                 this.chkLineDis5.Checked = bool.Parse(strLineInfo5.Split('@')[2]);

                 this.tbxLineName6.Text = strLineInfo6.Split('@')[0];
                 this.tbxLineIP6.Text = strLineInfo6.Split('@')[1];
                 this.chkLineDis6.Checked = bool.Parse(strLineInfo6.Split('@')[2]);

                 this.tbxLineName7.Text = strLineInfo7.Split('@')[0];
                 this.tbxLineIP7.Text = strLineInfo7.Split('@')[1];
                 this.chkLineDis7.Checked = bool.Parse(strLineInfo7.Split('@')[2]);

                 this.tbxLineName8.Text = strLineInfo8.Split('@')[0];
                 this.tbxLineIP8.Text = strLineInfo8.Split('@')[1];
                 this.chkLineDis8.Checked = bool.Parse(strLineInfo8.Split('@')[2]);

                 this.tbxLineName9.Text = strLineInfo9.Split('@')[0];
                 this.tbxLineIP9.Text = strLineInfo9.Split('@')[1];
                 this.chkLineDis9.Checked = bool.Parse(strLineInfo9.Split('@')[2]);

                 this.tbxLineName10.Text = strLineInfo10.Split('@')[0];
                 this.tbxLineIP10.Text = strLineInfo10.Split('@')[1];
                 this.chkLineDis10.Checked = bool.Parse(strLineInfo10.Split('@')[2]);

                 this.tbxLineName11.Text = strLineInfo11.Split('@')[0];
                 this.tbxLineIP11.Text = strLineInfo11.Split('@')[1];
                 this.chkLineDis11.Checked = bool.Parse(strLineInfo11.Split('@')[2]);

                 this.tbxLineName12.Text = strLineInfo12.Split('@')[0];
                 this.tbxLineIP12.Text = strLineInfo12.Split('@')[1];
                 this.chkLineDis12.Checked = bool.Parse(strLineInfo12.Split('@')[2]);

                 this.tbxLineName13.Text = strLineInfo13.Split('@')[0];
                 this.tbxLineIP13.Text = strLineInfo13.Split('@')[1];
                 this.chkLineDis13.Checked = bool.Parse(strLineInfo13.Split('@')[2]);

                 this.tbxLineName14.Text = strLineInfo14.Split('@')[0];
                 this.tbxLineIP14.Text = strLineInfo14.Split('@')[1];
                 this.chkLineDis14.Checked = bool.Parse(strLineInfo14.Split('@')[2]);

                 this.tbxLineName15.Text = strLineInfo15.Split('@')[0];
                 this.tbxLineIP15.Text = strLineInfo15.Split('@')[1];
                 this.chkLineDis15.Checked = bool.Parse(strLineInfo15.Split('@')[2]);

                 this.tbxLineName16.Text = strLineInfo16.Split('@')[0];
                 this.tbxLineIP16.Text = strLineInfo16.Split('@')[1];
                 this.chkLineDis16.Checked = bool.Parse(strLineInfo16.Split('@')[2]);

            }

        }

        private void PCBFOVSettingFm_Load(object sender, EventArgs e)
        {
            showSettingView();
        }

        private void btnQCPCBFOVSetting_Click(object sender, EventArgs e)
        {
            this.Close();
            //base.Enabled = true;
        }
    }
}
