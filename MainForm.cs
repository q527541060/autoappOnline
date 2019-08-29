using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DataBean;
using AppLayerLib;

using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using EYSPIToAOI;

using System.Xml;
using System.Text;
using System.Linq;

using System.Threading;
using System.Collections.Generic;
namespace AutoAPP
{
    public partial class MainForm : Form
    {

        bu_Joch bu_joch = new bu_Joch();
        public static bu_Peng bu_peng = new bu_Peng();
        public static ThreadProcessFovEYSPIToAOI _runnerFovThreadProcessFovEYSPIToAOI;
        public static ThreadProcessFov _runnerFov ;
        public static ThreadProcessWeiXin _runnerWeiXin;
        public static ThreadProcessDaHua _runnerDaHua;
        public static ThreadProcessLuBangTong _runnerLuBangTong;
        public static ThreadProcessLuBangTongPRD _runnerLuBangTongPRD;
        public static ThreadProcess _runnerDeleteProcess;
        private DateTime dtDeleteDataLastTime = Convert.ToDateTime("2011-11-11 00:00:00");
        private DateTime dtLastRunTime = Convert.ToDateTime("2011-11-11 00:00:00");
        private DateTime dtEXFOLastRunTime = Convert.ToDateTime("2011-11-11 00:00:00");
        //private DateTime dtEXJUFEILastRunTime = Convert.ToDateTime("2011-11-11 00:00:00");
        private DateTime dtLastRunTimeForJUFEI = Convert.ToDateTime("2011-11-11 00:00:00");
        private DateTime dtLastRunTimeForZZFox = Convert.ToDateTime("2011-11-11 00:00:00");
        private DateTime dtLastRunTimeForCaiHuang = Convert.ToDateTime("2011-11-11 00:00:00");
        private DateTime dtStartTimeForDetele = new DateTime(2011, 11, 11);
        public static int intProcess = 0;
        //tmp 
        public static bool bFov = true, bPcb = false, bPcbSecond = true,bCloseFov=false;

        private int intSPCDataBaseType = 0;
        private int intDeleteDays = 0;
        private bool blnDeleteData = false;
        private bool blnDeleteDay = false;
        //private bool blnBackUpData = false;
        private string strBackUpPath = string.Empty;
        private bool blnSynchronousDataForEXFO = false;
        private bool blnSaveCSVFileForFoxconn = false;
        private bool bEnExportLHFOXNGInfo = false;//龙华富士康导出ng整版信息btn
        private bool blnExportCDFOXNGinfo = false;//成都富士康导出ng信息btn
        private bool bEnDataExp = false;// 数据导出开关
        private bool bEnDataExpCDFOX = false;//成都富士康
        private bool bEnDataExpLHFox = false;//龙华富士康
        private bool bEnDataForGYFOX = false;//贵阳富士康开关;
        private bool bEnDataForCaiHuang = false;// 深圳市彩煌实业发展有限公司 ;
        private bool bEnDataForLeiNeng = false;// 深圳雷能
        private bool bEnDataForDefualt = false;// default
        public static bool bEnDataForRenBao = false;// 昆山仁宝;    add By Peng 20180906
        public static bool bEnDataForFovPcb = false;//Fov_Pcb    add By Peng 20180925
        public static bool bEnDataForOnLineFovPcb = false; // FOVPCB Online
        public static bool bEnDataTianJinWeiYe = false;// 天津伟业
        public static  bool bEnDataForFov = false;// Fov  add by peng 20181115
        public static bool bEnDataForPCB = false; // pcb add by peng 20181115
        private bool bEnWeiXin_suzhou = false; // 苏州维信 20181125
        private bool bEnZheJaingDaHua = false;// 浙江大华  20181226
        private bool bEnDataLuBangTong = false; // 鲁帮通 20181228
        public static bool bEnDataZHAOCHISamsung = false;// 兆驰三星双段合并 20190707
        public static bool bEnDataSkyWorth = false;// 创维数字 20190707

        private bool bWeiXin = true;
        private bool bEquipStatusOPen = true;
        private bool bEnClosePCBFOV = false;
        private bool bEnPerDayDel = false;
        private bool bEnPerHourDel = false;
        public static bool bEnEYSPIToAOI = false;
        public static bool bEnEquipStatus = false;
        public static  bool bEnDataBoe = false; // 合肥京东方 20190530
        public static bool bEnDataSpcData = false;
        private string strSaveEquipStatusPath = string.Empty;
        private string strConfigPath = @"D:\EYSPI\Bin\AutoAPPConfig";
        private string strConfigIniPath = @"D:\EYSPI\Bin\Config";
        private string _strIniCaiHuangPath = "D://EYSPI//DataExport//iniFile";
        private string _strIniAutoAPPConfigFileName = "autoApp.ini";
        private string _strLogPengPath = @"D:\EYSPI\Bin\SPILogs\Peng";
        private string _strExportPath = @"D:\EYSPI\DataExport";
        private string RS_DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private string RS_LINEEND = "\r\n";
        private string strSaveCSVFileForFoxconnPath = string.Empty;
        public static string _strToken = string.Empty;
        private InspectConfig.ConfigData _configData = null;
        private AutoAPP.Basefunction _baseFuc = new Basefunction();
        public static AppLayerLib.AppSettingHandler _appSettingHandle = new AppLayerLib.AppSettingHandler();

        //public  string _IP = "\\\\192.168.1.12\\";
        private string _strConfigNamePath = @"D:\EYSPI\Bin\Config\Inspect.bin";
        private string _strRealConfigPath = "";
        private bool blnSaveDataForJUFEI = false;
        private string strSaveJUFEIPath = string.Empty;

        //private ImgCSCoreIM.Em_SPCAutoExportFormat strSaveCSVFileForFoxconnFormat;
        TextBox txtEdit = new TextBox();
        private bool blnImmediatelyDelete = false;
        private bool blnDeleteDataEveryHour = false;
        private int iPCBLimit = 0, iDeleteCount = 0;
        //
        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentThread();
        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentProcessorNumberEx();
        [DllImport("kernel32.dll")]
        static extern IntPtr SetThreadAffinityMask(IntPtr hThread, IntPtr dwThreadAffinityMask);

        [DllImport("kernel32.dll")]
        static extern UIntPtr SetThreadAffinityMask(IntPtr hThread, UIntPtr dwThreadAffinityMask);
        //
        private const int INT_TWO = 2;

        private Thread _threadMain;
        private bool _bIsKeepRunning = true;
        private int _iMainSleepTime = 1000;
        private bool _bIsRunMainFlag = true;

        //add by peng 20190312
        //cpu2业务逻辑thread
        private Thread _threadMainCpTwo;
        //cpu2线程开关 默认打开
        private bool _bIsRunMainCp2Flag = true;
        //eyspiToAoi
        private Thread _threadMainCpEYSPIToAOI ;
        private bool _bIsRunMainCpEYSPIToAOIFlag = true;
        //fov/pcb
        private Thread _threadMainCpFOVPCB;
        private bool _bIsRunMainCpFOVPCBFlag = true;
        //fov/pcb online
        private Thread _threadMainCpOnLineFOVPCB;
        private bool _bIsRunMainCpOnLineFOVPCBFlag = true;
        //delete
        private Thread _threadMainCpDelete;
        private bool _bIsRunMainCpDeleteFlag = true;
        //spcDataUpload
        private Thread _threadMainCpSPCDataUpload;
        private bool _bIsRunMainCpSPCDataUploadFlag = true;
        // showMessage
        private Thread _threadMainShowMessage;
        private bool _bIsRunMainShowMessageFlag = true;

        //UI 自动删除是否需要去new thread
        private bool _bIsNewThreadWithUiAutoDelete =true;

        
        private static int RunningInstance()
        {

            Process currentProcess = Process.GetCurrentProcess();
            foreach (Process process in Process.GetProcessesByName(currentProcess.ProcessName))
            {
                if ((process.Id != currentProcess.Id) &&
                    (System.Reflection.Assembly.GetExecutingAssembly().Location == currentProcess.MainModule.FileName))
                {
                    MessageBox.Show("软件已经启动,请关闭后重新打开!");
                    Application.Exit();
                    return -1;
                }
            }
            return 0;

        }
        

       
        public MainForm()
        {
            //if (RunningInstance() == -1)
            //{
            //    return;
            //}
            InitializeComponent();
            txtEdit.KeyDown += new KeyEventHandler(txtEdit_KeyDown);
            lbBackUpFile.ItemHeight = lbDatatableName.ItemHeight;
        }

        /// <summary>  
        /// KeyDown事件定义  
        /// </summary>  
        private void txtEdit_KeyDown(object sender, KeyEventArgs e)
        {
            //Enter键 更新项并隐藏编辑框  
            if (e.KeyCode == Keys.Enter)
            {
                lbDatatableName.Items[lbDatatableName.SelectedIndex] = txtEdit.Text;
                txtEdit.Visible = false;
            }
            //Esc键 直接隐藏编辑框  
            if (e.KeyCode == Keys.Escape)
                txtEdit.Visible = false;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //StopWork();

                DoStopWork();
                //if (bP.Stop() == false)
                //{
                //    e.Cancel = false;
                //}
                //if (!bu_peng.StopFovSpcThread())
                //{
                //    e.Cancel = false;
                //}
                if (this.notifyIcon1 != null || this.notifyIcon1.Visible == true)
                {
                    notifyIcon1.Visible = false;
                    notifyIcon1.Dispose();

                }
                System.Threading.Thread t = System.Threading.Thread.CurrentThread;
                AppLogHelp.WriteLog(LogFileFormate.AppMain, "AutoAPP 关闭");
                //notifyIcon1.Visible = false;
                while (t != null && t.ThreadState != System.Threading.ThreadState.Aborted)
                {
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    //System.Threading.Thread.Sleep(300);
                    //if (t.ThreadState == System.Threading.ThreadState.Running)
                    //{
                    //    System.Windows.Forms.Application.Exit();
                    //}
                }

                
            }
            catch (System.Threading.ThreadAbortException ex)
            {

                //log.WriteLog("线程关闭 ! " + ex.Message);
                //AppLogHelp.WriteLog(LogFileFormate.SQL, "TestConnection : " + strReturn);
                Application.Exit();
                //MessageBox.Show(ex.Message);
            }

            finally
            {
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            
            try
            {
                if (RunningInstance() == -1)
                {
                    return;
                }
                //delete configBin
                _baseFuc.DeleteDataAutoAPPConfigBin(strConfigPath, 60);
                //LoadDataSet();
                //log.WriteLog("ini;;");
                //this.timer2.Enabled = true;
                
                this.Text = this.Text + "(" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + ")";
                this.Tag = this.Text;

                int interval = Properties.Settings.Default.TimeTerval;
                
                GetAppSetting();

                this.cbBoxPcbFov.Checked = bEnClosePCBFOV;

                //log.WriteLog("autoAPP load...","autoAPP");

                AppLogHelp.WriteLog(LogFileFormate.AppMain, "autoAPP load...");
                //modity the inspect.bin
                //_strConfigNamePath = _IP + _strConfigNamePath;  // 20190826 moditied
                //_strConfigNamePath = _strConfigNamePath.Replace("D:\\", "");
                if (File.Exists(@_strConfigNamePath))
                {
                    //_strConfigNamePath = _strConfigNamePath.Replace(Path.GetExtension(_strConfigNamePath), ".spk");
                    _strRealConfigPath = Path.Combine( strConfigPath, "Inspect.spk");  // 20190826 moditied
                    //_strRealConfigPath = _IP + _strRealConfigPath;
                    //_strRealConfigPath = _strRealConfigPath.Replace("D:\\","");
                    File.Copy(_strConfigNamePath, _strRealConfigPath, true);
                    System.Threading.Thread.Sleep(200);

                    
                    InspectConfig.ConfigHandler _configReader= new InspectConfig.ConfigHandler();
			        _configData=new InspectConfig.ConfigData();

                    _configReader.ReadConfig(_strRealConfigPath);

                    _configData=_configReader._configData;
                   // bu_peng._configData = _configData;
                    //File.Delete(_strConfigNamePath);
                    ReadAppsetingFrUI();
                }
                else
                {
                    MessageBox.Show("autoAPPConfig中不存在inspect.bin文件,请检测文件");
                    return;
                }

                showFroms(sender, e);

            }
            catch (Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.Message);
                AppLogHelp.WriteLog(LogFileFormate.AppMain, ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
        //private bool bCaiHuangOpen = true;
        //private bool bLeiNengOpen = true;
        //private bool bDeleteDataOpen = true;
        //private bool bEXFLOpen = true;
        //private bool bFoxconnCsv = true;
        //private bool bJUFEIOpen = true;
        //private bool bCDFoxOpen = true;
        //private bool bLHFoxOpen = true;
        //private bool bTDWYOpen = true;
        //private Dictionary<string,string> dic = new Dictionary<string,string>();
        private string RS_Start_Infoview = " => view start...";
        private void showFroms(object sender, EventArgs e)
        {
            //delete logdetail
            if (this.tabMain.TabPages.Contains(this.tpgLogDetail))
            {
                this.tabMain.TabPages.Remove(this.tpgLogDetail);
            }

            #region "Delete Data"
            if (!blnDeleteData)
            {
                this.chkAuto.Checked = false;
                //this.btnAutoPost.Enabled = false;
                this.tabMain.TabPages.Remove(this.tpgDeleteData);
                this.tabMain.TabPages.Remove(this.tpgLoadFile);
            }
            else
            {
                if (!this.tabMain.TabPages.Contains(this.tpgDeleteData))
                {
                    this.tabMain.TabPages.Add(this.tpgDeleteData);
                }
                if (!this.tabMain.TabPages.Contains(this.tpgLoadFile))
                {
                    //this.tabMain.TabPages.Add(this.tpgLoadFile);
                    this.tabMain.TabPages.Add(this.tpgLoadFile);
                }

                //   this.groupBox1.Enabled = false;
                //lin 20190813
                //this.txtMySQLConn.Text = Properties.Settings.Default.MySQLConnect;
                this.txtMySQLConn.Text = WSClnt.PubStaticParam._strSPIdbConnectionString;
                if (intDeleteDays > 0)
                    intDeleteDays = -1 * intDeleteDays;

                this.dtpEnd.Value = Convert.ToDateTime(DateTime.Now.AddDays(intDeleteDays).ToString("yyyy-MM-dd 00:00:00"));

                if (blnDeleteData)
                {
                    this.dtpStart.Value = Convert.ToDateTime("2011-11-11 00:00:00");
                }
                else
                {
                    this.dtpStart.Value = Convert.ToDateTime(this.dtpEnd.Value.AddMonths(-1).ToString("yyyy-MM-dd 00:00:00"));
                }

                this.chkBackUP.Checked = false;
                this.txtSaveFolder.Text = strBackUpPath;

                this.rdbHour.Checked = bEnPerHourDel;

                this.rdbDay.Checked = bEnPerDayDel;

                this.chkAuto.Checked = true;
                this.btnTestDB.PerformClick();
                this.btnAutoPost_Click(sender, e);

            }
            #endregion

            #region "Log Detail"
            string strPath = System.IO.Path.Combine(Application.StartupPath, "log");
            if (!System.IO.Directory.Exists(strPath))
            {
                System.IO.Directory.CreateDirectory(strPath);
            }

            #endregion

            #region "Synchronous Data EXFO"

            if (!blnSynchronousDataForEXFO)
            {
                this.chkEXFOAuto.Checked = false;
                this.btnEXFOAuto.Enabled = false;
                this.tabMain.TabPages.Remove(this.tpgEXFO);
            }
            else
            {
                if (!this.tabMain.TabPages.Contains(this.tpgEXFO))
                {
                    this.tabMain.TabPages.Add(this.tpgEXFO);
                }
                string strEXiniConecction = string.Empty, strEXiniAutoTime = string.Empty;
                this.groupBox3.Enabled = true;  // moditied by peng 20181012
                //add by peng 20181012
                string strEXIniFile = Path.Combine(strConfigIniPath, _strIniAutoAPPConfigFileName);
                if (File.Exists(strEXIniFile))
                {
                    strEXiniConecction = WSClnt.INIFileHelper.ReadIniData("autoAPP", "EXFOConnectionString", "", strEXIniFile);
                    strEXiniAutoTime = WSClnt.INIFileHelper.ReadIniData("autoAPP", "EXFOAutoTimeString", "", strEXIniFile);
                }
                this.txtEXFOSQLConn.Text = Properties.Settings.Default.MESTestConnectionString;
                this.txtEXFOStartTime.Text = Properties.Settings.Default.EXFOStartTime;
                if (!string.IsNullOrEmpty(strEXiniConecction))
                {
                    this.txtEXFOSQLConn.Text = strEXiniConecction;
                }
                if (!string.IsNullOrEmpty(strEXiniAutoTime))
                {
                    this.txtEXFOStartTime.Text = strEXiniAutoTime;
                }

                this.dtpEXFOEndTime.Value = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                this.dtpEXFOStartTime.Value = Convert.ToDateTime(this.dtpEXFOEndTime.Value.AddDays(-1).ToString("yyyy-MM-dd 00:00:00"));

                this.txtCSVFile.Text = strSaveCSVFileForFoxconnPath;
                this.chkEXFOAuto.Checked = true;
                this.btnEXFOAuto_Click(sender, e);

            }

            #endregion

            #region "Save CSV File For 富士康"

            if (!blnSaveCSVFileForFoxconn)
            {
                this.chkCSVAutoRun.Checked = false;
                this.btnCSVAuto.Enabled = false;
                this.tabMain.TabPages.Remove(this.tpgAutoCSV);
            }
            else
            {

                if (!this.tabMain.TabPages.Contains(this.tpgAutoCSV))
                {
                    this.tabMain.TabPages.Add(this.tpgAutoCSV);
                }


                this.btnCSVAuto.Enabled = true;
                //lin 20190813
                //this.txtCSVMySQLConnect.Text = Properties.Settings.Default.MySQLConnect;
                this.txtCSVMySQLConnect.Text =WSClnt.PubStaticParam._strSPIdbConnectionString;
                this.txtCSVRunTime.Text = Properties.Settings.Default.CSVStartTime;
                this.chkCSVAutoRun.Checked = true;
                btnCSVAuto_Click(sender, e);
                this.dtpCSVEndTime.Value = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                this.dtpCSVStartTime.Value = Convert.ToDateTime(this.dtpCSVEndTime.Value.AddDays(-1).ToString("yyyy-MM-dd 00:00:00"));
                this.txtCSVFile.Text = @"D:\EYSPI\DataExport\";

            }
            #endregion

            #region "Save DATA JUFEI "
            if (!blnSaveDataForJUFEI)
            {
                this.chkJufeiAutoRun.Checked = false;
                this.btnSaveJufei.Enabled = false;
                this.tabMain.TabPages.Remove(this.tpgDataJUFEI);
            }
            else
            {
                if (blnSaveCSVFileForFoxconn)
                {
                    if (!this.tabMain.TabPages.Contains(this.tpgDataJUFEI))
                    {
                        this.tabMain.TabPages.Add(this.tpgDataJUFEI);
                    }
                    this.chkCSVAutoRun.Checked = false;
                    this.btnCSVAuto.Enabled = false;
                    this.tabMain.TabPages.Remove(this.tpgAutoCSV);

                    //lin 20190813
                    //this.textJufeiMySQLConnect.Text = Properties.Settings.Default.MySQLConnect;
                    this.textJufeiMySQLConnect.Text = WSClnt.PubStaticParam._strSPIdbConnectionString;
                    this.txtJufeiRunTime.Text = Properties.Settings.Default.JUFEIStartTime;
                    this.chkJufeiAutoRun.Checked = true;
                    this.btnSaveJufei.Enabled = true;
                    btnJuFeiFileRun_Click(sender, e);
                    this.dtpEndTimeJufei.Value = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                    this.dtpStartTimeJufei.Value = Convert.ToDateTime(this.dtpEndTimeJufei.Value.AddDays(-1).ToString("yyyy-MM-dd 00:00:00"));
                    this.savePathJufei.Text = @"D:\EYSPI\DataExport\";
                }

            }

            #endregion

            #region  "成都富士康  龙华富士康  defultTools"

            if (bEnDataExp)
            {
                //  this.timer1.Enabled = false; joch why enable timer1
                string path = strConfigPath;
                var files = Directory.GetFiles(path, "*.bin");
                int max = int.MinValue, min = int.MaxValue;
                foreach (var file in files)
                {
                    if (!string.IsNullOrEmpty(file))
                    {
                        var vv = Path.GetFileNameWithoutExtension(file);
                        System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"^\d+$");
                        if (re.IsMatch(vv))
                        {
                            int value = int.Parse(vv);
                            if (value < min)
                                min = value;
                            if (value > max)
                                max = value;
                        }
                    }
                }
                string zeroAPPfile = Path.Combine(strConfigPath, max + ".bin");
                AppSettingHandler appSettingHandle = new AppSettingHandler();
                if (File.Exists(zeroAPPfile))
                {
                    appSettingHandle.Read(zeroAPPfile, _configData);
                }
                else
                {
                    appSettingHandle.Read();
                }
                //bEnExportLHFOXNGInfo = appSettingHandle._appSettingData.stDataExpVT.bEnExportNGInfo;
                //appSettingHandle._appSettingData.csvFormat = CSVFormat.Zzfoxconn;
                if (bEnDataExpCDFOX)
                {
                    if (!this.tabMain.TabPages.Contains(this.tabCDFOX))
                    {
                        this.tabMain.TabPages.Add(this.tabCDFOX);
                    }
                    //this.timer2.Enabled = true;
                    //log.WriteLog("成都富士康线程 start: [ ]", "CDFOX");
                    AppLogHelp.WriteLog(LogFileFormate.MES, "成都富士康线程 start:");
                    bu_peng.AutoStartCdFox(_configData);
                }
                else
                {
                    this.tabMain.TabPages.Remove(this.tabCDFOX);
                }
                if (bEnDataExpLHFox && blnSaveCSVFileForFoxconn)
                {
                    if (!this.tabMain.TabPages.Contains(this.tabLhFo))
                    {
                        this.tabMain.TabPages.Add(this.tabLhFo);
                    }
                    //this.chkCSVAutoRun.Checked = false;
                    //this.btnCSVAuto.Enabled = false;
                    //this.tabControl1.TabPages.Remove(this.tpgAutoCSV);
                    //this.timer2.Enabled = true;
                    //log.WriteLog("龙华富士康线程 start: [ ]", "LHFOX");
                    AppLogHelp.WriteLog(LogFileFormate.MES, "龙华富士康线程 start: [ ]");
                    bu_peng.AutoStartLHFox(_strRealConfigPath);
                }
                else
                {
                    this.tabMain.TabPages.Remove(this.tabLhFo);
                }
                if (bEnDataForDefualt)//增加defaultTools
                {
                    if (!this.tabMain.TabPages.Contains(this.tpgTools))
                    {
                        this.tabMain.TabPages.Add(this.tpgTools);
                    }
                    if (string.IsNullOrEmpty(this.txtConfigFile.Text))
                    {
                        this.txtConfigFile.Text = strConfigIniPath;
                    }

                    //this.timer2.Enabled = true;
                    //log.WriteLog("defaultTools线程 start: [ ]", "defaultTools");
                    bu_peng.AutoStartDefaultTools(this.txtConfigFile.Text, _strRealConfigPath);
                }
                else
                {
                    this.tabMain.TabPages.Remove(this.tpgTools);
                }
            }
            // modify by Peng 20180412;
            else
            {
                this.tabMain.TabPages.Remove(this.tabLhFo);
                this.tabMain.TabPages.Remove(this.tabCDFOX);
                this.tabMain.TabPages.Remove(this.tpgTools);
            }
            #endregion

            #region" 贵阳富士康 "
            if (!bEnDataForGYFOX)
            {
                this.tabMain.TabPages.Remove(this.tpgGYFox);
            }
            else
            {
                //this.timer1.Enabled = true;
            }
            #endregion

            #region "深圳彩煌实业"
            if (bEnDataForCaiHuang == false)
            {
                this.chkRunCaiHuang.Checked = false;
                this.btnCSVAutoForCaiHuang.Enabled = false;
                this.tabMain.TabPages.Remove(this.CaiHuang);
            }
            else
            {
                if (!this.tabMain.TabPages.Contains(this.CaiHuang))
                {
                    this.tabMain.TabPages.Add(this.CaiHuang);
                }
                //lin 20190813
                //this.txtCSVMySQLConnectForCaiHuang.Text = Properties.Settings.Default.MySQLConnect;
                this.txtCSVMySQLConnectForCaiHuang.Text =WSClnt.PubStaticParam._strSPIdbConnectionString;
                this.txtCSVRunTimeForCaiHuang.Text = Properties.Settings.Default.CSVStartTime;
                //this.chkCSVAutoRun.Checked = true;
                btnCSVAutoForCaiHuang_Click(sender, e);
                this.dtpCSVEndTimeForCaiHuang.Value = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                this.dtpCSVStartTimeForCaiHuang.Value = Convert.ToDateTime(this.dtpCSVEndTimeForCaiHuang.Value.AddDays(-1).ToString("yyyy-MM-dd 00:00:00"));
                this.txtCSVFileForCaiHuang.Text = @"D:\EYSPI\DataExport\";
            }

            #endregion

            #region "深圳雷能"
            if (bEnDataForLeiNeng)
            {
                //this.timer1.Enabled = false;
                //this.timer2.Enabled = true;
                AppLogHelp.WriteLog(LogFileFormate.MES, "LeiNeng start..");

                bu_peng.startLNRun();
            }
            #endregion

            #region " 苏州仁宝 "
            if (!bEnDataForRenBao)
            {
                this.tabMain.TabPages.Remove(this.tpgAutoRenBao);
            }
            else
            {
                if (!this.tabMain.TabPages.Contains(this.tpgAutoRenBao))
                {
                    this.tabMain.TabPages.Add(this.tpgAutoRenBao);
                }
                //this.timer2.Enabled = true;
                //this.btnStartRenBao.Text = "开启监控";

                //btnStartRenBao_Click(sender, e);
                //bu_peng
                //ini
                string tmpPath = Path.Combine(_strLogPengPath, "RenBao");
                string strINIFile = Path.Combine(tmpPath, "renbao.ini");

                if (Directory.Exists(tmpPath) == false)
                {
                    Directory.CreateDirectory(tmpPath);
                }
                if (File.Exists(strINIFile) == false)
                {
                    using (FileStream fs = new FileStream(strINIFile, FileMode.Create))
                    {

                    }
                    if (string.IsNullOrEmpty(this.tbTextPathRenBao.Text))
                    {
                        //
                        this.tbTextPathRenBao.Text = _strExportPath;
                        WSClnt.INIFileHelper.WriteIniData("RenBao", "iniPath", this.tbTextPathRenBao.Text, strINIFile);
                    }
                    else
                    {
                        WSClnt.INIFileHelper.WriteIniData("RenBao", "iniPath", this.tbTextPathRenBao.Text, strINIFile);
                    }

                    if (string.IsNullOrEmpty(this.tbTextPathRenBao2.Text))
                    {
                        //
                        this.tbTextPathRenBao2.Text = _strExportPath;
                        WSClnt.INIFileHelper.WriteIniData("RenBao", "iniPath2", this.tbTextPathRenBao2.Text, strINIFile);
                    }
                    else
                    {
                        WSClnt.INIFileHelper.WriteIniData("RenBao", "iniPath2", this.tbTextPathRenBao2.Text, strINIFile);
                    }
                }
                else
                {
                    this.tbTextPathRenBao.Text = WSClnt.INIFileHelper.ReadIniData("RenBao", "iniPath", "", strINIFile);
                    this.tbTextPathRenBao2.Text = WSClnt.INIFileHelper.ReadIniData("RenBao", "iniPath2", "", strINIFile);
                }
                string strAutoAPPIniFile = @"D:\EYSPI\Bin\Config\autoApp.ini";
                if (File.Exists(strAutoAPPIniFile))
                {
                    this.tbRenBaoSaoplant.Text = WSClnt.INIFileHelper.ReadIniData("KunShanRenBao", "tbRenBaoSaoplant", "", strAutoAPPIniFile);
                    this.tbRenBaoGroupName.Text = WSClnt.INIFileHelper.ReadIniData("KunShanRenBao", "tbRenBaoGroupName", "", strAutoAPPIniFile);
                    this.tbRenBaoLineName.Text = WSClnt.INIFileHelper.ReadIniData("KunShanRenBao", "tbRenBaoLineName", "", strAutoAPPIniFile);

                    this.tbRenBaoCuscode.Text = WSClnt.INIFileHelper.ReadIniData("KunShanRenBao", "tbRenBaoCuscode", "", strAutoAPPIniFile);
                    this.tbRenBaoPross.Text = WSClnt.INIFileHelper.ReadIniData("KunShanRenBao", "tbRenBaoPross", "", strAutoAPPIniFile);
                    this.tbRenBaoFactorySN.Text = WSClnt.INIFileHelper.ReadIniData("KunShanRenBao", "tbRenBaoFactorySN", "", strAutoAPPIniFile);

                    this.tbRenBaoSide.Text = WSClnt.INIFileHelper.ReadIniData("KunShanRenBao", "tbRenBaoSide", "", strAutoAPPIniFile);
                    this.tbRenBaoEquipType.Text = WSClnt.INIFileHelper.ReadIniData("KunShanRenBao", "tbRenBaoEquipType", "", strAutoAPPIniFile);
                    this.tbRenBaoOp.Text = WSClnt.INIFileHelper.ReadIniData("KunShanRenBao", "tbRenBaoOp", "", strAutoAPPIniFile);


                }
                btnStartRenBao_Click(sender, e);
            }

            #endregion

            #region "Fov_Pcb"
            if (bEnDataForFovPcb)
            {
                if (bEnDataForPCB || bEnDataForFov)
                {
                    if (!this.tabMain.TabPages.Contains(this.Fov_Pcb_Image))
                    {
                        this.tabMain.TabPages.Add(this.Fov_Pcb_Image);

                        
                        //初始化 参数

                    }

                    //初始化 
                    // iniEYSPIToAOIFiles();
                    bFov = true;
                }
                else
                {
                    this.tabMain.TabPages.Remove(this.Fov_Pcb_Image);
                }

            }
            else
            {
                this.tabMain.TabPages.Remove(this.Fov_Pcb_Image);
            }
            #endregion

            #region "天津伟业"
            if (!bEnDataTianJinWeiYe)
            {
                this.tabMain.TabPages.Remove(this.tpgTianJinWeiYe);
            }
            else
            {
                if (!this.tabMain.TabPages.Contains(this.tpgTianJinWeiYe))
                {
                    this.tabMain.TabPages.Add(this.tpgTianJinWeiYe);
                }
                //this.timer2.Enabled = true;
                bu_peng.AutoStartTianJinWeiYe(_configData);
            }
            #endregion

            #region " 苏州维信"
            if (bEnWeiXin_suzhou)
            {
                if (!this.tabMain.TabPages.Contains(this.WeinXin_SZ))
                {
                    this.tabMain.TabPages.Add(this.WeinXin_SZ);
                }
            }
            else
            {
                this.tabMain.TabPages.Remove(this.WeinXin_SZ);
            }
            #endregion

            #region " 浙江大华"
            if (bEnZheJaingDaHua)
            {
                if (!this.tabMain.TabPages.Contains(this.tbDaHua))
                {
                    this.tabMain.TabPages.Add(this.tbDaHua);
                }
                string strDaHuaSaveLogPath = bu_Peng.ReadIniData("DaHua", "iniPath", string.Empty, Path.Combine(strConfigIniPath, _strIniAutoAPPConfigFileName));
                if (string.IsNullOrEmpty(strDaHuaSaveLogPath) == false)
                {
                    this.tbDahuaFilePath.Text = strDaHuaSaveLogPath;
                }
                else
                {
                    string strDirTmp = @"D:\EYSPI\DataExport\Res";
                    if (Directory.Exists(strDirTmp) == false)
                    {
                        Directory.CreateDirectory(strDirTmp);
                    }
                    this.tbDahuaFilePath.Text = strDirTmp;
                }
                button2_Click(sender, e);
            }
            else
            {
                this.tabMain.TabPages.Remove(this.tbDaHua);
            }

            #endregion

            #region "鲁帮通"

            if (bEnDataLuBangTong)
            {
                if (!this.tabMain.TabPages.Contains(this.tpLuBangTong))
                {
                    this.tabMain.TabPages.Add(this.tpLuBangTong);
                }

                // Read INI
                string strAutoAPPIniFile = Path.Combine(strConfigIniPath, _strIniAutoAPPConfigFileName);
                if (File.Exists(strAutoAPPIniFile))
                {
                    string strTmpReadIni = string.Empty;
                    System.Threading.Thread.Sleep(300);
                    strTmpReadIni = bu_Peng.ReadIniData("LuBangTong", "clientId", string.Empty, strAutoAPPIniFile);
                    if (string.IsNullOrEmpty(strTmpReadIni) == false)
                    {
                        this.clientId.Text = strTmpReadIni;
                    }
                    strTmpReadIni = bu_Peng.ReadIniData("LuBangTong", "clientSecret", string.Empty, strAutoAPPIniFile);
                    if (string.IsNullOrEmpty(strTmpReadIni) == false)
                    {
                        this.clientSecret.Text = strTmpReadIni;
                    }
                    strTmpReadIni = bu_Peng.ReadIniData("LuBangTong", "tbloginId", string.Empty, strAutoAPPIniFile);
                    if (string.IsNullOrEmpty(strTmpReadIni) == false)
                    {
                        this.tbloginId.Text = strTmpReadIni;
                    }
                    strTmpReadIni = bu_Peng.ReadIniData("LuBangTong", "tbLoginPass", string.Empty, strAutoAPPIniFile);
                    if (string.IsNullOrEmpty(strTmpReadIni) == false)
                    {
                        this.tbLoginPass.Text = strTmpReadIni;
                    }
                    strTmpReadIni = bu_Peng.ReadIniData("LuBangTong", "tbScope", string.Empty, strAutoAPPIniFile);
                    if (string.IsNullOrEmpty(strTmpReadIni) == false)
                    {
                        this.tbScope.Text = strTmpReadIni;
                    }
                    strTmpReadIni = bu_Peng.ReadIniData("LuBangTong", "URLLOGIN", string.Empty, strAutoAPPIniFile);
                    if (string.IsNullOrEmpty(strTmpReadIni) == false)
                    {
                        this.URLLOGIN.Text = strTmpReadIni;
                    }
                    strTmpReadIni = bu_Peng.ReadIniData("LuBangTong", "tbStatusURL", string.Empty, strAutoAPPIniFile);
                    if (string.IsNullOrEmpty(strTmpReadIni) == false)
                    {
                        this.tbStatusURL.Text = strTmpReadIni;
                    }
                    strTmpReadIni = bu_Peng.ReadIniData("LuBangTong", "tbErrorUrl", string.Empty, strAutoAPPIniFile);
                    if (string.IsNullOrEmpty(strTmpReadIni) == false)
                    {
                        this.tbErrorUrl.Text = strTmpReadIni;
                    }
                    strTmpReadIni = bu_Peng.ReadIniData("LuBangTong", "tbOn", string.Empty, strAutoAPPIniFile);
                    if (string.IsNullOrEmpty(strTmpReadIni) == false)
                    {
                        this.tbOn.Text = strTmpReadIni;
                    }
                    strTmpReadIni = bu_Peng.ReadIniData("LuBangTong", "tbSf", string.Empty, strAutoAPPIniFile);
                    if (string.IsNullOrEmpty(strTmpReadIni) == false)
                    {
                        this.tbSf.Text = strTmpReadIni;
                    }
                    strTmpReadIni = bu_Peng.ReadIniData("LuBangTong", "tbMachineNo", string.Empty, strAutoAPPIniFile);
                    if (string.IsNullOrEmpty(strTmpReadIni) == false)
                    {
                        this.tbMachineNo.Text = strTmpReadIni;
                    }
                    strTmpReadIni = bu_Peng.ReadIniData("LuBangTong", "tbITEMVALUE", string.Empty, strAutoAPPIniFile);
                    if (string.IsNullOrEmpty(strTmpReadIni) == false)
                    {
                        this.tbITEMVALUE.Text = strTmpReadIni;
                    }
                    strTmpReadIni = bu_Peng.ReadIniData("LuBangTong", "tbMO", string.Empty, strAutoAPPIniFile);
                    if (string.IsNullOrEmpty(strTmpReadIni) == false)
                    {
                        this.tbMO.Text = strTmpReadIni;
                    }
                    strTmpReadIni = bu_Peng.ReadIniData("LuBangTong", "tbUSERNO", string.Empty, strAutoAPPIniFile);
                    if (string.IsNullOrEmpty(strTmpReadIni) == false)
                    {
                        this.tbUSERNO.Text = strTmpReadIni;
                    }
                }


                btnLogin_Click(sender, e);
            }
            else
            {
                this.tabMain.TabPages.Remove(this.tpLuBangTong);
            }

            #endregion

            #region "EYSPIToAOI"

            if (bEnEYSPIToAOI)
            {
                if (!this.tabMain.TabPages.Contains(this.tpEYSPIToAOI))
                {
                    this.tabMain.TabPages.Add(this.tpEYSPIToAOI);
                }
                //初始化 
                //iniEYSPIToAOIFiles();
                bAOI = true;
            }
            else
            {
                this.tabMain.TabPages.Remove(this.tpEYSPIToAOI);
            }
            #endregion

            #region "HuaQin"
            if (bEnNanChangHuaQin)
            {
                if (!this.tabMain.TabPages.Contains(this.tbgHuaQin))
                {
                    this.tabMain.TabPages.Add(this.tbgHuaQin);
                }
            }
            else
            {
                this.tabMain.TabPages.Remove(this.tbgHuaQin);
            }


            #endregion

            #region "EquipStatus"
            if (bEnEquipStatus)
            {
                if (!this.tabMain.TabPages.Contains(this.tbtbStatusSys))
                {
                    this.tabMain.TabPages.Add(this.tbtbStatusSys);
                }
            }
            else
            {
                this.tabMain.TabPages.Remove(this.tbtbStatusSys);
            }
            #endregion

            #region "Boe"
            if (bEnDataBoe)
            {
                if (!this.tabMain.TabPages.Contains(this.tpgBoe))
                {
                    this.tabMain.TabPages.Add(this.tpgBoe);
                }
            }
            else
            {
                this.tabMain.TabPages.Remove(this.tpgBoe);
            }

            #endregion

            #region "SpcData"

            if (bEnDataSpcData)
            {
                if (!this.tabMain.TabPages.Contains(this.tbSPCUpload))
                {
                    this.tabMain.TabPages.Add(this.tbSPCUpload);
                }
            }
            else
            {
                this.tabMain.TabPages.Remove(this.tbSPCUpload);
            }
            #endregion

            #region "ZHAOCHISamsung"

            if (bEnDataZHAOCHISamsung)
            {
                if (!this.tabMain.TabPages.Contains(this.tpZHAOCHISamsung))
                {
                    this.tabMain.TabPages.Add(this.tpZHAOCHISamsung);
                }
            }
            else
            {
                if (this.tabMain.TabPages.Contains(this.tpZHAOCHISamsung))
                this.tabMain.TabPages.Remove(this.tpZHAOCHISamsung);
                //tpZHAOCHISamsung.Hide();
                //tpZHAOCHISamsung.Parent = null;
            }
            #endregion

            #region"SkyWorth"
            if (bEnDataSkyWorth)
            {
                if (!this.tabMain.TabPages.Contains(this.tpSkyWorth))
                {
                    this.tabMain.TabPages.Add(this.tpSkyWorth);
                }
                ReadSkyWorthIniPrams();
            }
            else
            {
                if (this.tabMain.TabPages.Contains(tpSkyWorth))
                    this.tabMain.TabPages.Remove(this.tpSkyWorth);
            }
            #endregion
        }
        
        private void showFroms()
        {

            #region "Delete Data"
            if (!blnDeleteData)
            {

                if (this.tabMain.TabPages.Contains(this.tpgDeleteData))
                {
                    //tpgDeleteData.Parent = tabMain;
                    this.tabMain.TabPages.Remove(this.tpgDeleteData);
                }
                if (this.tabMain.TabPages.Contains(this.tpgLoadFile))
                {
                    //tpgDeleteData.Parent = tabMain;
                    this.tabMain.TabPages.Remove(this.tpgLoadFile);
                }
                //tpgLoadFile.Parent = null;
            }
            else
            {
                if (!this.tabMain.TabPages.Contains(this.tpgDeleteData))
                {
                    //tpgDeleteData.Parent = tabMain;
                    this.tabMain.TabPages.Add(tpgDeleteData);
                }
                if (!this.tabMain.TabPages.Contains(this.tpgLoadFile))
                {
                    //tpgLoadFile.Parent = tabMain;
                    this.tabMain.TabPages.Add(tpgLoadFile);
                }
                //if (dic.ContainsKey(LogFileFormate.Delete.ToString()) == false)
                //{
                //    dic.Add(LogFileFormate.Delete.ToString(), RS_Start_Infoview);
                //}
            }
            #endregion

            #region "Log Detail"
            string strPath = System.IO.Path.Combine(Application.StartupPath, "log");
            if (!System.IO.Directory.Exists(strPath))
            {
                System.IO.Directory.CreateDirectory(strPath);
            }

            #endregion

            #region "Synchronous Data EXFO"

            if (!blnSynchronousDataForEXFO)
            {
                this.chkEXFOAuto.Checked = false;
                this.btnEXFOAuto.Enabled = false;
                if (this.tabMain.TabPages.Contains(this.tpgEXFO))
                {
                    this.tabMain.TabPages.Remove(this.tpgEXFO);
                }
            }
            else
            {
                if (!this.tabMain.TabPages.Contains(this.tpgEXFO))
                {
                    this.tabMain.TabPages.Add(this.tpgEXFO);
                    //tpgEXFO.Parent = tabMain;
                }
                //if (dic.ContainsKey(LogFileFormate.MES.ToString() + EM_MES_FORMAT._EXFO) == false)
                //{
                //    dic.Add(LogFileFormate.MES.ToString() + EM_MES_FORMAT._EXFO, RS_Start_Infoview);
                //}
            }

            #endregion

            #region "Save CSV File For 富士康"

            if (!blnSaveCSVFileForFoxconn)
            {
                if (this.tabMain.TabPages.Contains(this.tpgAutoCSV))
                {
                    this.tabMain.TabPages.Remove(this.tpgAutoCSV);
                }
                //tpgAutoCSV.Parent = null;
            }
            else
            {
                if (!this.tabMain.TabPages.Contains(this.tpgAutoCSV))
                {
                    this.tabMain.TabPages.Add(this.tpgAutoCSV);
                    //tpgAutoCSV.Parent = tabMain;
                }
                //if (dic.ContainsKey(LogFileFormate.MES.ToString() + EM_MES_FORMAT._SaveCSVFileForFoxconn) == false)
                //{
                //    dic.Add(LogFileFormate.MES.ToString() + EM_MES_FORMAT._SaveCSVFileForFoxconn, RS_Start_Infoview);
                //}
                //dic.Add(LogFileFormate.MES.ToString(), EM_MES_FORMAT.SaveCSVFileForFoxconn + RS_Start_Infoview);
            }
            #endregion

            #region "Save DATA JUFEI "
            if (!blnSaveDataForJUFEI)
            {
                if (this.tabMain.TabPages.Contains(this.tpgDataJUFEI))
                {
                    this.tabMain.TabPages.Remove(this.tpgDataJUFEI);
                }
                //tpgDataJUFEI.Parent = null;
            }
            else
            {
                if (blnSaveDataForJUFEI)
                {
                    if (!this.tabMain.TabPages.Contains(this.tpgDataJUFEI))
                    {
                        this.tabMain.TabPages.Add(this.tpgDataJUFEI);
                        //tpgDataJUFEI.Parent = tabMain;
                    }
                    //if (dic.ContainsKey(LogFileFormate.MES.ToString() + EM_MES_FORMAT._JUFEI) == false)
                    //{
                    //    dic.Add(LogFileFormate.MES.ToString() + EM_MES_FORMAT._JUFEI, RS_Start_Infoview);
                    //}

                }
            }
            #endregion

            #region  "成都富士康  龙华富士康  defultTools"

            if (bEnDataExp)
            {

                if (bEnDataExpCDFOX)
                {
                    if (!this.tabMain.TabPages.Contains(this.tabCDFOX))
                    {
                        this.tabMain.TabPages.Add(this.tabCDFOX);
                        //tabCDFOX.Parent = tabMain;
                    }
                    //if (dic.ContainsKey(LogFileFormate.MES.ToString() + EM_MES_FORMAT._CDFOX) == false)
                    //{
                    //    dic.Add(LogFileFormate.MES.ToString() + EM_MES_FORMAT._CDFOX, RS_Start_Infoview);
                    //}
                    //dic.Add(LogFileFormate.MES.ToString(), EM_MES_FORMAT.CDFOX + RS_Start_Infoview);
                }
                else
                {
                    if (this.tabMain.TabPages.Contains(this.tabCDFOX))
                    {
                        this.tabMain.TabPages.Remove(this.tabCDFOX);
                    }
                    //tabCDFOX.Parent = null;
                }
                if (bEnDataExpLHFox && blnSaveCSVFileForFoxconn)
                {
                    if (!this.tabMain.TabPages.Contains(this.tabLhFo))
                    {
                        this.tabMain.TabPages.Add(this.tabLhFo);
                        //tabLhFo.Parent = tabMain;
                    }
                    //if (dic.ContainsKey(LogFileFormate.MES.ToString() + EM_MES_FORMAT._LHFox) == false)
                    //{
                    //    dic.Add(LogFileFormate.MES.ToString() + EM_MES_FORMAT._LHFox, RS_Start_Infoview);
                    //}
                    //dic.Add(LogFileFormate.MES.ToString(), EM_MES_FORMAT.LHFox + RS_Start_Infoview);
                }
                else
                {
                    if (this.tabMain.TabPages.Contains(this.tabLhFo))
                    {
                        this.tabMain.TabPages.Remove(this.tabLhFo);
                    }
                    //tabLhFo.Parent = null;
                }
                //if (bEnDataForDefualt)//增加defaultTools
                //{
                //    if (!this.tabMain.TabPages.Contains(this.tpgTools))
                //    {
                //       // this.tabMain.TabPages.Add(this.tpgTools);
                //        tpgTools.Parent = tabMain;
                //    }
                //    if (string.IsNullOrEmpty(this.txtConfigFile.Text))
                //    {
                //        this.txtConfigFile.Text = strConfigIniPath;
                //    }

                //    //this.timer2.Enabled = true;
                //    //log.WriteLog("defaultTools线程 start: [ ]", "defaultTools");
                //    bu_peng.AutoStartDefaultTools(this.txtConfigFile.Text,  _strRealConfigPath);
                //}
                //else
                //{
                //    //this.tabMain.TabPages.Remove(this.tpgTools);
                //    tpgTools.Parent = null;
                //}
            }
            // modify by Peng 20180412;
            else
            {
                if ( this.tabMain.TabPages.Contains(this.tabLhFo))
                {
                    this.tabMain.TabPages.Remove(this.tabLhFo);
                }
                if (this.tabMain.TabPages.Contains(this.tabCDFOX))
                {
                    this.tabMain.TabPages.Remove(this.tabCDFOX);
                }
                if (this.tabMain.TabPages.Contains(this.tpgTools))
                {
                    this.tabMain.TabPages.Remove(this.tpgTools);
                }
                //tabLhFo.Parent = null;
                //tabCDFOX.Parent = null;
                //tpgTools.Parent = null;
                
                
            }
            #endregion

            #region" 贵阳富士康 "
            if (!bEnDataForGYFOX)
            {
                if (this.tabMain.TabPages.Contains(this.tpgGYFox))
                {
                    //this.tabMain.TabPages.Remove(this.tpgGYFox);
                    this.tabMain.TabPages.Remove(this.tpgGYFox);
                }
                
                //tpgGYFox.Parent = null;
            }
            else
            {
                //this.timer1.Enabled = true;
            }
            #endregion

            #region "深圳彩煌实业"
            if (bEnDataForCaiHuang == false)
            {
                this.chkRunCaiHuang.Checked = false;
                this.btnCSVAutoForCaiHuang.Enabled = false;
                if (this.tabMain.TabPages.Contains(this.CaiHuang))
                {
                    this.tabMain.TabPages.Remove(this.CaiHuang);
                }
                //CaiHuang.Parent = null;
            }
            else
            {
                if (!this.tabMain.TabPages.Contains(this.CaiHuang))
                {
                    this.tabMain.TabPages.Add(this.CaiHuang);
                    //CaiHuang.Parent = tabMain;
                }
                //if (dic.ContainsKey(LogFileFormate.MES.ToString() + EM_MES_FORMAT._CaiHuang) == false)
                //{
                //    dic.Add(LogFileFormate.MES.ToString() + EM_MES_FORMAT._CaiHuang, RS_Start_Infoview);
                //}
                //dic.Add(LogFileFormate.MES.ToString(), EM_MES_FORMAT.CaiHuang + RS_Start_Infoview);
            }

            #endregion

            #region "深圳雷能"
            if (bEnDataForLeiNeng)
            {
                //this.timer1.Enabled = false;
                //this.timer2.Enabled = true;

                AppLogHelp.WriteLog(LogFileFormate.MES, "LeiNeng start..");
                //if (dic.ContainsKey(LogFileFormate.MES.ToString() + EM_MES_FORMAT._LeiNeng) == false)
                //{
                //    dic.Add(LogFileFormate.MES.ToString() + EM_MES_FORMAT._LeiNeng, RS_Start_Infoview);
                //}
                //dic.Add(LogFileFormate.MES.ToString(), EM_MES_FORMAT.LeiNeng + RS_Start_Infoview);
                    
                    bu_peng.startLNRun();
                    
            }
            else
            {
                CaiHuang.Parent = null;
            }
            #endregion

            #region " 苏州仁宝 "
            if (!bEnDataForRenBao)
            {
                if (this.tabMain.TabPages.Contains(tpgAutoRenBao) )
                this.tabMain.TabPages.Remove(this.tpgAutoRenBao);
                //tpgAutoRenBao.Parent = null;
            }
            else
            {
                //if (dic.ContainsKey(LogFileFormate.MES.ToString() + EM_MES_FORMAT._WeiXin_suzhou) == false)
                //{
                //    dic.Add(LogFileFormate.MES.ToString() + EM_MES_FORMAT._WeiXin_suzhou, RS_Start_Infoview);
                //}
                //dic.Add(LogFileFormate.MES.ToString(), EM_MES_FORMAT.WeiXin_suzhou + RS_Start_Infoview);
                if (!this.tabMain.TabPages.Contains(this.tpgAutoRenBao))
                {
                    this.tabMain.TabPages.Add(this.tpgAutoRenBao);
                    //tpgAutoRenBao.Parent = tabMain;
                }
            }

            #endregion

            #region "Fov_Pcb"
            if (bEnDataForFovPcb)
            {
                if (bEnDataForPCB || bEnDataForFov)
                {
                    if (!this.tabMain.TabPages.Contains(this.Fov_Pcb_Image))
                    {
                        this.tabMain.TabPages.Add(this.Fov_Pcb_Image);
                        //Fov_Pcb_Image.Parent = tabMain;

                    }
                    //初始化 
                    // iniEYSPIToAOIFiles();
                    // bFov = true;
                    //if (dic.ContainsKey(LogFileFormate.FOVPCB.ToString()) == false)
                    //{
                    //    dic.Add(LogFileFormate.FOVPCB.ToString(), RS_Start_Infoview);
                    //}
                    //dic.Add(LogFileFormate.FOVPCB.ToString(), RS_Start_Infoview);
                }
                else
                {
                    if (this.tabMain.TabPages.Contains(Fov_Pcb_Image))
                        this.tabMain.TabPages.Remove(this.Fov_Pcb_Image);
                    //Fov_Pcb_Image.Parent = null;
                }

            }
            else
            {
                if (this.tabMain.TabPages.Contains(Fov_Pcb_Image))
                    this.tabMain.TabPages.Remove(this.Fov_Pcb_Image);
                //Fov_Pcb_Image.Parent = null;
            }
            #endregion

            #region "天津伟业"
            if (!bEnDataTianJinWeiYe)
            {
                if (this.tabMain.TabPages.Contains(tpgTianJinWeiYe))
                this.tabMain.TabPages.Remove(this.tpgTianJinWeiYe);
                //tpgTianJinWeiYe.Parent = null;
            }
            else
            {
                if (!this.tabMain.TabPages.Contains(this.tpgTianJinWeiYe))
                {
                    this.tabMain.TabPages.Add(this.tpgTianJinWeiYe);
                    //tpgTianJinWeiYe.Parent = tabMain;
                }
                //if (dic.ContainsKey(LogFileFormate.MES.ToString()) == false)
                //{
                //    dic.Add(LogFileFormate.MES.ToString(), EM_MES_FORMAT._TianJinWeiYe + RS_Start_Infoview);
                //}
            }
            #endregion

            #region " 苏州维信"
            if (bEnWeiXin_suzhou)
            {
                if (!this.tabMain.TabPages.Contains(this.WeinXin_SZ))
                {
                    this.tabMain.TabPages.Add(this.WeinXin_SZ);
                    //WeinXin_SZ.Parent = tabMain;
                }
                //if (dic.ContainsKey(LogFileFormate.MES.ToString() + EM_MES_FORMAT._WeiXin_suzhou.ToString()) == false)
                //{
                //    dic.Add(LogFileFormate.MES.ToString() + EM_MES_FORMAT._WeiXin_suzhou, RS_Start_Infoview);
                //}
                //dic.Add(LogFileFormate.MES.ToString(), EM_MES_FORMAT.WeiXin_suzhou + RS_Start_Infoview);
            }
            else
            {
                if (this.tabMain.TabPages.Contains(WeinXin_SZ))
                this.tabMain.TabPages.Remove(this.WeinXin_SZ);
                //WeinXin_SZ.Parent = null;
            }
            #endregion

            #region " 浙江大华"
            if (bEnZheJaingDaHua)
            {
                if (!this.tabMain.TabPages.Contains(this.tbDaHua))
                {
                    this.tabMain.TabPages.Add(this.tbDaHua);
                    //tbDaHua.Parent = tabMain;
                }
                //dic.Add(LogFileFormate.MES.ToString(), EM_MES_FORMAT.ZheJaingDaHua + RS_Start_Infoview);
                //if (dic.ContainsKey(LogFileFormate.MES.ToString() + EM_MES_FORMAT._ZheJaingDaHua.ToString()) == false)
                //{
                //    dic.Add(LogFileFormate.MES.ToString() + EM_MES_FORMAT._ZheJaingDaHua, RS_Start_Infoview);
                //}
            }
            else
            {
                if (this.tabMain.TabPages.Contains(tbDaHua))
                this.tabMain.TabPages.Remove(this.tbDaHua);
                //tbDaHua.Parent = null;
            }

            #endregion

            #region "鲁帮通"

            if (bEnDataLuBangTong)
            {
                if (!this.tabMain.TabPages.Contains(this.tpLuBangTong))
                {
                    this.tabMain.TabPages.Add(this.tpLuBangTong);
                    //tpLuBangTong.Parent = tabMain;
                }
                //if (dic.ContainsKey(LogFileFormate.MES.ToString() + EM_MES_FORMAT._DataLuBangTong.ToString()) == false)
                //{
                //    dic.Add(LogFileFormate.MES.ToString() + EM_MES_FORMAT._DataLuBangTong, RS_Start_Infoview);
                //}
                //dic.Add(LogFileFormate.MES.ToString(), EM_MES_FORMAT.DataLuBangTong + RS_Start_Infoview);
            }
            else
            {
                if (this.tabMain.TabPages.Contains(tpLuBangTong))
                this.tabMain.TabPages.Remove(this.tpLuBangTong);
                //tpLuBangTong.Parent = null;
            }

            #endregion

            #region "EYSPIToAOI"

            if (bEnEYSPIToAOI)
            {
                if (!this.tabMain.TabPages.Contains(this.tpEYSPIToAOI))
                {
                    this.tabMain.TabPages.Add(this.tpEYSPIToAOI);
                    //tpEYSPIToAOI.Parent = tabMain;
                }
                //if (dic.ContainsKey(LogFileFormate.EYspiToAOI.ToString()) == false)
                //{
                //    dic.Add(LogFileFormate.EYspiToAOI.ToString() , RS_Start_Infoview);
                //}
                //dic.Add(LogFileFormate.EYspiToAOI.ToString(), RS_Start_Infoview);
            }
            else
            {
                if (this.tabMain.TabPages.Contains(tpEYSPIToAOI))
                this.tabMain.TabPages.Remove(this.tpEYSPIToAOI);
                //tpEYSPIToAOI.Parent = null;
            }
            #endregion

            #region "HuaQin"
            if (bEnNanChangHuaQin)
            {
                if (!this.tabMain.TabPages.Contains(this.tbgHuaQin))
                {
                    this.tabMain.TabPages.Add(this.tbgHuaQin);
                    //tbgHuaQin.Parent = tabMain;
                }
                //dic.Add(LogFileFormate.MES.ToString(), EM_MES_FORMAT._DataLuBangTong + RS_Start_Infoview);
            }
            else
            {
                if (this.tabMain.TabPages.Contains(tbgHuaQin))
                this.tabMain.TabPages.Remove(this.tbgHuaQin);
                //tbgHuaQin.Parent = null;
            }


            #endregion

            #region "EquipStatus"
            if (bEnEquipStatus)
            {
                if (!this.tabMain.TabPages.Contains(this.tbtbStatusSys))
                {
                    this.tabMain.TabPages.Add(this.tbtbStatusSys);
                    //tbtbStatusSys.Parent = tabMain;
                }
                //if (dic.ContainsKey(LogFileFormate.EquipStatus.ToString()) == false)
                //{
                //    dic.Add(LogFileFormate.EquipStatus.ToString(), RS_Start_Infoview);
                //}
            }
            else
            {
                if (this.tabMain.TabPages.Contains(tbtbStatusSys))
                this.tabMain.TabPages.Remove(this.tbtbStatusSys);
                //tbtbStatusSys.Parent = null;
            }
            #endregion

            #region "Boe"
            if (bEnDataBoe)
            {
                if (!this.tabMain.TabPages.Contains(this.tpgBoe))
                {
                    this.tabMain.TabPages.Add(this.tpgBoe);
                    //tpgBoe.Parent = tabMain;
                }
                //if (dic.ContainsKey(LogFileFormate.MES.ToString() + EM_MES_FORMAT._Boe) == false)
                //{
                //    dic.Add(LogFileFormate.MES.ToString() + EM_MES_FORMAT._Boe, RS_Start_Infoview);
                //}
                //dic.Add(LogFileFormate.MES.ToString(), EM_MES_FORMAT.Boe + RS_Start_Infoview);
            }
            else
            {
                //tpgBoe.Parent = null;
                if (this.tabMain.TabPages.Contains(tpgBoe))
                this.tabMain.TabPages.Remove(this.tpgBoe);
            }

            #endregion

            #region "SpcData"
            if (bEnDataSpcData)
            {
                if (!this.tabMain.TabPages.Contains(this.tbSPCUpload))
                {
                    this.tabMain.TabPages.Add(this.tbSPCUpload);
                    //tbSPCUpload.Parent = tabMain;
                }
                //if (dic.ContainsKey(LogFileFormate.SPCDataUpload.ToString()) == false)
                //{
                //    dic.Add(LogFileFormate.SPCDataUpload.ToString(), RS_Start_Infoview);
                //}
                //dic.Add(LogFileFormate.SPCDataUpload.ToString(),  RS_Start_Infoview);
            }
            else
            {
                if (this.tabMain.TabPages.Contains(tbSPCUpload))
                this.tabMain.TabPages.Remove(this.tbSPCUpload);
                //tbSPCUpload.Parent = null;
            }
            #endregion

            #region "ZHAOCHISamsung"

            if (bEnDataZHAOCHISamsung)
            {
                if (!this.tabMain.TabPages.Contains(this.tpZHAOCHISamsung))
                {
                    this.tabMain.TabPages.Add(this.tpZHAOCHISamsung);
                    //tpZHAOCHISamsung.Parent = tabMain;
                }
                //if (dic.ContainsKey(LogFileFormate.MES.ToString() + EM_MES_FORMAT._ZHAOCHISamsung) == false)
                //{
                //    dic.Add(LogFileFormate.MES.ToString() + EM_MES_FORMAT._ZHAOCHISamsung, RS_Start_Infoview);
                //}
                //dic.Add(LogFileFormate.MES.ToString(), EM_MES_FORMAT.ZHAOCHISamsung + RS_Start_Infoview);
            }
            else
            {
                if (this.tabMain.TabPages.Contains(tpZHAOCHISamsung))
                this.tabMain.TabPages.Remove(this.tpZHAOCHISamsung);
                //tpZHAOCHISamsung.Hide();
                //if (this.tabMain.TabPages.Contains(this.tpZHAOCHISamsung))
                //{
                ////this.tabMain.TabPages.Add(this.tpZHAOCHISamsung);
                ////tpZHAOCHISamsung.Parent = null;
                //}
            }
            #endregion

            //tpSkyWorth
            #region"SkyWorth"
            if (bEnDataSkyWorth)
            {
                if (!this.tabMain.TabPages.Contains(this.tpSkyWorth))
                {
                    this.tabMain.TabPages.Add(this.tpSkyWorth);
                }
            }
            else
            {
                if (this.tabMain.TabPages.Contains(tpSkyWorth))
                    this.tabMain.TabPages.Remove(this.tpSkyWorth);
            }
            #endregion

        }

        private void btnTestDB_Click(object sender, EventArgs e)
        {
            try
            {
                //moditied by peng 20190314  
                MessageBox.Show(bu_joch.TestConnection(this.txtMySQLConn.Text), "系统提示",

                                                         MessageBoxButtons.OK, MessageBoxIcon.Warning,

                                                         MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                //MessageBox.Show(bu_joch.TestConnection(this.txtMySQLConn.Text, log));
                //this.timer2.Enabled = true;
                //this.timer2.Start();
            }
            catch (System.Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.Message);
            }
            finally
            {
                //LoadDataSet();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                dtDeleteDataLastTime = Convert.ToDateTime("2011-11-11 00:00:00");
                blnDeleteDataEveryHour = rdbHour.Checked;
                blnDeleteDay = rdbDay.Checked;
                iPCBLimit = Convert.ToInt32(nudPCBLimit.Value);
                bool blnSaveData = this.chkBackUP.Checked;
                string strSaveFloder = this.txtSaveFolder.Text;

                if (this.dtpStart.Checked == false)
                {
                    MessageBox.Show("请选择开始日期");
                }
                if (this.dtpEnd.Checked == false)
                {
                    MessageBox.Show("请选择结束日期");
                }
                if (string.IsNullOrEmpty(strSaveFloder))
                {
                    MessageBox.Show("请选择备份数据目录");
                }


                if (dtpStart.Checked == true && dtpEnd.Checked == true)
                {
                    if (MessageBox.Show("确定开始手动删除SPC数据", "手动删除", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        //this.timer2.Enabled = true;
                        bu_joch.AutoStart(this.dtpStart.Value, dtpEnd.Value, blnSaveData, strSaveFloder, blnDeleteDataEveryHour, iPCBLimit);
                    }
                }
                //LoadDataSet();
            }
            catch (System.Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.Message);
                AppLogHelp.WriteError(LogFileFormate.Delete, ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DoStopWork();
            
            if (this.notifyIcon1 != null || this.notifyIcon1.Visible == true)
            {
                notifyIcon1.Visible = false;
                notifyIcon1.Dispose();
            }
        }
        private string RS_KG_KEY = "    ";
        private bool bIsKeepShowMessageColumn = true;
        private string RS_iDDataGridViewTextBoxColumn = "iDDataGridViewTextBoxColumn";
        private string RS_threadDataGridViewTextBoxColumn = "threadDataGridViewTextBoxColumn";
        private string RS_messageDataGridViewTextBoxColumn = "messageDataGridViewTextBoxColumn";
        private void LoadDataSet()
        {
            try
            {
                this.Text = this.Tag + RS_KG_KEY + DateTime.Now.ToString(RS_DATETIME_FORMAT);
                DataTable _dtShowMessage = new DataTable();
                

                //if (bIsKeepShowMessageColumn)
                //{
                    bIsKeepShowMessageColumn = false;
                    _dtShowMessage.Columns.Add(RS_iDDataGridViewTextBoxColumn, typeof(string));   //新建第一列
                    _dtShowMessage.Columns.Add(RS_threadDataGridViewTextBoxColumn, typeof(string));      //新建第二列
                    _dtShowMessage.Columns.Add(RS_messageDataGridViewTextBoxColumn, typeof(string));
                //}
                //DataTable dt = new DataTable();            //新建对象
                //dt.Columns.Add("iDDataGridViewTextBoxColumn", typeof(string));   //新建第一列
                //dt.Columns.Add("threadDataGridViewTextBoxColumn", typeof(string));      //新建第二列
                //dt.Columns.Add("messageDataGridViewTextBoxColumn", typeof(string));
                //if (dic != null && dic.Count > 0)
                //{
                //    int i = 0;string value=string.Empty;
                //    foreach (string key in dic.Keys)
                //    {
                //        i++;
                //        dic.TryGetValue(key, out value);
                //        _dtShowMessage.Rows.Add(i, key, value);

                //    }
                //}
                //_dtShowMessage.Rows.Add(1, 2, 3);
                //dataGridView1.Rows.
                //FillDataGridViewWithDataSource(dataGridView1, _dtShowMessage);
                //if (dic != null && dic.Count > 0)
                //{
                //    dic.Clear();
                //}
               // _dtShowMessage.Clear();
            }
            catch (System.Exception ex)
            {
                //log.WriteErr("LoadDataSet错误 ! " + ex.Message);
                AppLogHelp.WriteError(LogFileFormate.AppMain, "LoadDataSet=>  " + ex.Message);
            }
            finally
            {
            }
        }
        private void FillDataGridViewWithDataSource(DataGridView dataGridView, DataTable dTable)
        {
            //1.清空旧数据
            //if (dataGridView.Rows.Count > 0)
            //{
            //    dataGridView.Rows.Clear();
            //}
            if (dTable != null && dTable.Rows.Count > 0)
            {
                dataGridView.Columns[RS_iDDataGridViewTextBoxColumn].DataPropertyName = RS_iDDataGridViewTextBoxColumn;
                dataGridView.Columns[RS_threadDataGridViewTextBoxColumn].DataPropertyName = RS_threadDataGridViewTextBoxColumn;
                dataGridView.Columns[RS_messageDataGridViewTextBoxColumn].DataPropertyName = RS_messageDataGridViewTextBoxColumn;
                //设置数据源，部分显示数据            
                dataGridView.DataSource = dTable;
                //dataGridView.AutoGenerateColumns = false;
                //if (dic != null && dic.Count > 0)
                //{
                //    dic.Clear();
                //}
                //dTable.Clear();
            }
            
        }

        private void 查看日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string strPath = System.IO.Path.Combine(Application.StartupPath, "log");
                if (System.IO.Directory.Exists(strPath))
                {
                    System.Diagnostics.Process.Start(strPath);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAutoPost_Click(object sender, EventArgs e)
        {
            try
            {

                dtDeleteDataLastTime = Convert.ToDateTime("2011-11-11 00:00:00");
                blnDeleteDataEveryHour = rdbHour.Checked;
                blnDeleteDay = rdbDay.Checked;
                iPCBLimit = Convert.ToInt32(nudPCBLimit.Value);

                if (this.btnAutoPost.Text == "停止删除")
                {
                    this.chkImmediatelyDelete.Checked = false; //打开立即删除
                    this.btnAutoPost.Text = "自动删除";
                    _bIsNewThreadWithUiAutoDelete = false;   // 关闭状态
                    //this.timer1.Enabled = false;
                    this.btnPost.Enabled = true;
                    this.dtpEnd.Enabled = true;
                    this.dtpStart.Enabled = true;
                    this.nudPCBLimit.Enabled = true;
                    blnDeleteData = false;

                    this.rdbHour.Enabled = true;
                    this.rdbDay.Enabled = true;
                    this.chkImmediatelyDelete.Enabled = true;
                }
                else if (this.btnAutoPost.Text == "自动删除")
                {
                    this.chkImmediatelyDelete.Checked = true;  //打开立即删除
                    _bIsNewThreadWithUiAutoDelete = true; // 打开状态;
                    this.btnAutoPost.Text = "停止删除";
                    //this.timer1.Enabled = true;
                    this.btnPost.Enabled = false;
                    this.dtpStart.Enabled = false;
                    this.dtpEnd.Enabled = false;
                    this.nudPCBLimit.Enabled = false;
                    blnDeleteData = true;

                    this.rdbHour.Enabled = false;
                    this.rdbDay.Enabled = false;
                    this.chkImmediatelyDelete.Enabled = false;


                    
                }
                //save params

                string strAutoAPPIniFile = Path.Combine(strConfigIniPath, _strIniAutoAPPConfigFileName);
                if (!File.Exists(strAutoAPPIniFile))
                {
                    //如果INI文件不在config目录下.则读取运行目录下的ini文件
                    strAutoAPPIniFile = Path.Combine(Application.StartupPath, _strIniAutoAPPConfigFileName);
                }
                if (File.Exists(strAutoAPPIniFile))
                {
                    bu_Peng.WriteIniData("Delete", "EnPerHourDel", this.rdbHour.Checked.ToString(), strAutoAPPIniFile);
                    bu_Peng.WriteIniData("Delete", "EnPerDayDel", this.rdbDay.Checked.ToString(), strAutoAPPIniFile);
                    //bu_Peng.WriteIniData("Delete", "intDeleteDays", Convert.ToInt32(this.nudPCBLimit.Value)+"", strAutoAPPIniFile);
                }
            }
            catch (System.Exception ex)
            {
                AppLogHelp.WriteLog(LogFileFormate.MES, ex.Message);
                MessageBox.Show(ex.ToString());
            }
        }


        public static void showMessage(string strMessage)
        {
            intProcess++;
            MessageBox.Show(strMessage);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = true;  //显示在系统任务栏
                this.WindowState = FormWindowState.Normal;  //还原窗体
                                                            // this.notifyIcon1.Visible = false;  //托盘图标隐藏
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
            }
            
        }

        private void frmMain_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)  //判断是否最小化
            {
                this.ShowInTaskbar = false;  //不显示在系统任务栏
                this.notifyIcon1.Visible = true;  //托盘图标可见
            }
        }

        private void GetAppSetting()
        {
            try
            {
                #region "----"
                //AppSettingHandler appSettingHandle = new AppSettingHandler();
                //appSettingHandle.Read();
                //intDeleteDays = 30;// appSettingHandle._appSettingData.stSPCParams.DeleteDays;
                ////else
                ////{
                //intSPCDataBaseType = 2;// appSettingHandle._appSettingData.stSPCParams.bySPCDataBaseType;
                //blnDeleteDay = true;// appSettingHandle._appSettingData.stSPCParams.EnDeleteSPC;
                //blnBackUpData = appSettingHandle._appSettingData.stSPCParams.EnBackUpDeletedSPC;
                //strBackUpPath =  appSettingHandle._appSettingData.stSPCParams.strSPCBackUpDeletedPath;
                ////同步数据 EXFO
                //blnSynchronousDataForEXFO = appSettingHandle._appSettingData.stSPCParams.blnSynchronousData;
                //// 同步zzfox
                //blnSaveCSVFileForFoxconn = false;//appSettingHandle._appSettingData.stSPCParams.bEnAutoExport;
                //bEnDataForLeiNeng = appSettingHandle._appSettingData.stSPCParams.bEnLeiNengExport;
                //// 
                //bEnDataExp = false;//appSettingHandle._appSettingData.bEnDataExp;              
                ////strSaveCSVFileForFoxconnPath =  appSettingHandle._appSettingData.stSPCParams.strSPCAutoExportPath;
                ////strSaveCSVFileForFoxconnFormat = appSettingHandle._appSettingData.stSPCParams.emSPCAutoExportFormat;   
                ////bEnDataForGYFOX = true;

                //// 同步CaiHuang
                //bEnDataForCaiHuang = appSettingHandle._appSettingData.stSPCParams.bEnCaiHuangExport;   
                //if (appSettingHandle._appSettingData.csvFormat == CSVFormat.JuFei)
                //{
                //    blnSaveDataForJUFEI = true;
                //}
                //if (appSettingHandle._appSettingData.csvFormat == CSVFormat.Foxconn_ChengDu)
                //{
                //    bEnDataExpCDFOX = true;
                //}
                //if (appSettingHandle._appSettingData.csvFormat == CSVFormat.YouJia_SuZhou)
                //{

                //}
                #endregion
                //}
                string strAutoAPPIniFile = Path.Combine(strConfigIniPath, _strIniAutoAPPConfigFileName);   //20190826 moditied
                //strAutoAPPIniFile = _IP + strAutoAPPIniFile;
                //strAutoAPPIniFile = strAutoAPPIniFile.Replace("D:\\","");
                //strAutoAPPIniFile = Path.Combine(_IP, strAutoAPPIniFile);

                //modify by joch  20180904
                if (!File.Exists(strAutoAPPIniFile))
                {
                    //如果INI文件不在config目录下.则读取运行目录下的ini文件
                    strAutoAPPIniFile = Path.Combine(Application.StartupPath, _strIniAutoAPPConfigFileName);
                }
                try
                {
                    if (File.Exists(strAutoAPPIniFile))
                    {
                        //sleep 200 ms
                        System.Threading.Thread.Sleep(200);
                        System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"^\d+$");
                        string strTmpReadIniResult = string.Empty;
                        //blnBackUpData
                        //modify 此功能在deleteData界面上设置
                        //string strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "blnBackUpData", string.Empty, strAutoAPPIniFile);
                        //if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        //{
                        //    blnBackUpData = true;
                        //}
                        ////建议ini操作都放到inifilehelper文件去. 代码一样.避免修改漏掉.
                        //intSPCDataBaseType
                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "SPCDataBaseType", "1", strAutoAPPIniFile);
                        if (string.Equals("2", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            intSPCDataBaseType = 2;
                        }

                        //blnSynchronousDataForEXFO
                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "SynchronousDataForEXFO", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            blnSynchronousDataForEXFO = true;
                        }
                        //blnSaveCSVFileForFoxconn
                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "SaveCSVFileForFoxconn", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            blnSaveCSVFileForFoxconn = true;
                        }
                        else
                        {
                            blnSaveCSVFileForFoxconn = false;
                        }
                        //blnSaveCSVFileForFoxconn
                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "SaveDataForJUFEI", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            blnSaveDataForJUFEI = true;
                        }
                        //bEnDataForLeiNeng
                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnDataForLeiNeng", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnDataForLeiNeng = true;
                        }
                        //bEnDataForCaiHuang
                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnDataForCaiHuang", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnDataForCaiHuang = true;
                        }
                        else
                        {
                            bEnDataForCaiHuang = false;
                        }
                        //bEnDataExp
                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnDataExp", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnDataExp = true;
                        }
                        else
                        {
                            bEnDataExp = false;
                        }

                        //Delete
                        strTmpReadIniResult = bu_Peng.ReadIniData("Delete", "intDeleteDays", "100", strAutoAPPIniFile);

                        //if (re.IsMatch(strTmpReadIniResult) && strTmpReadIniResult != "0")
                        //{
                        //    intDeleteDays = int.Parse(strTmpReadIniResult);
                        //}

                        try
                        {
                            intDeleteDays = Convert.ToInt32(strTmpReadIniResult);
                        }
                        catch (Exception)
                        {
                            intDeleteDays = 100;
                        }

                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "DeleteData", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            blnDeleteData = true;
                        }
                        else
                        {
                            blnDeleteData = false;
                        }
                        //strBackUpPath
                        strTmpReadIniResult = bu_Peng.ReadIniData("Delete", "strBackUpPath", strConfigPath, strAutoAPPIniFile);
                        strBackUpPath = strTmpReadIniResult;

                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnDataExpCDFOX", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnDataExpCDFOX = true;
                        }
                        else
                        {
                            bEnDataExpCDFOX = false;
                        }

                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnDataExpLHFox", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnDataExpLHFox = true;
                        }
                        else
                        {
                            bEnDataExpLHFox = false;
                        }

                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnDataForDefualt", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnDataForDefualt = true;
                        }
                        else
                        {
                            bEnDataForDefualt = false;
                        }

                        //
                        //strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnDataForGuiYang", string.Empty, strAutoAPPIniFile);
                        //if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        //{
                        //    bEnDataForGYFOX = true;
                        //}
                        
                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnDataForRenBao", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnDataForRenBao = true;
                        }
                        else
                        {
                            bEnDataForRenBao = false;
                        }
                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnDataForFov", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnDataForFovPcb = true;
                        }
                        else
                        {
                            bEnDataForFovPcb = false;
                        }
                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnDataTianJinWeiYe", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnDataTianJinWeiYe = true;
                        }
                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnDataJusForFov", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnDataForFov = true;
                        }
                        else
                        {
                            bEnDataForFov = false;
                        }
                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnDataJusForPcb", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnDataForPCB = true;
                        }
                        else
                        {
                            bEnDataForPCB = false;
                        }
                        //EnWeiXin_suzhou
                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnWeiXin_suzhou", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnWeiXin_suzhou = true;
                        }
                        else
                        {
                            bEnWeiXin_suzhou = false;
                        }
                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnDataClosePCBFOV", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnClosePCBFOV = true;
                        }
                        else
                        {
                            bEnClosePCBFOV = false;
                        }
                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnDataDahua", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnZheJaingDaHua = true;
                        }
                        else
                        {
                            bEnZheJaingDaHua = false;
                        }
                        //EnDataLuBangTong
                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnDataLuBangTong", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnDataLuBangTong = true;
                        }
                        else
                        {
                            bEnDataLuBangTong = false;
                        }

                        //EnDelete
                        strTmpReadIniResult = bu_Peng.ReadIniData("Delete", "EnPerHourDel", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnPerHourDel = true;
                        }
                        else
                        {
                            bEnPerHourDel = false;
                        }
                        strTmpReadIniResult = bu_Peng.ReadIniData("Delete", "EnPerDayDel", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnPerDayDel = true;
                        }
                        else
                        {
                            bEnPerDayDel = false;
                        }

                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnDataEYSPIToAOI", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnEYSPIToAOI = true;
                        }
                        else
                        {
                            bEnEYSPIToAOI = false;
                        }
                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnNanChangHuaQin", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnNanChangHuaQin = true;
                        }
                        else
                        {
                            bEnNanChangHuaQin = false;
                        }
                        _FovDirPath = bu_Peng.ReadIniData("autoAPP", "ExToFovImage", string.Empty, strAutoAPPIniFile);
                       // _FovDirPath = _FovDirPath.Replace("D:\\", "");

                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnEquipStatus", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnEquipStatus = true;
                        }
                        else
                        {
                            bEnEquipStatus = false;
                        }
                        strSaveEquipStatusPath = bu_Peng.ReadIniData("autoAPP", "SaveEquipStatusPath", string.Empty, strAutoAPPIniFile);
                        //INIFileHelper.WriteIniData("autoAPP", "EnEquipStatus",(AappSettingData.stMonitorSystemParams.byLogFormat == (byte)Em_MonitorLogType.AutoAPP).ToString() , strAutoAPPIniFile);
                        //    INIFileHelper.WriteIniData("autoAPP", "SaveEquipStatusPath", AappSettingData.stMonitorSystemParams.strExportLogPath, strAutoAPPIniFile);
                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnDataBoe", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnDataBoe = true;
                        }
                        else
                        {
                            bEnDataBoe = false;
                        }
                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnDataSpcData", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnDataSpcData = true;
                        }
                        else
                        {
                            bEnDataSpcData = false;
                        }
                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "EnDataZHAOCHISamsung", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnDataZHAOCHISamsung = true;
                        }
                        else
                        {
                            bEnDataZHAOCHISamsung = false;
                        }
                        //ZHAOCHISamsung_TMPFILE_PATH
                        ZHAOCHISamsung_TMPFILE_PATH = bu_Peng.ReadIniData("autoAPP", "ZHAOCHISamsung_TMPFILE_PATH", string.Empty, strAutoAPPIniFile);
                        ZHAOCHISamsung_SAVEFILE_PATH = bu_Peng.ReadIniData("autoAPP", "ZHAOCHISamsung_SAVEFILE_PATH", string.Empty, strAutoAPPIniFile);
                        strTmpReadIniResult = bu_Peng.ReadIniData("autoAPP", "bEnDataSkyWorth", string.Empty, strAutoAPPIniFile);
                        if (string.Equals("true", strTmpReadIniResult, StringComparison.CurrentCultureIgnoreCase))
                        {
                            bEnDataSkyWorth = true;
                        }
                        else
                        {
                            bEnDataSkyWorth = false;
                        }
                        //bEnDataSkyWorth
                    }
                    else
                    {
                        //log.WriteErr("无AutoAPP配置文件!");
                    }
                }
                catch (Exception ex)
                {
                    //log.WriteErr(" 读取autoAPPiniConfig配置项 错误:" + ex.Message);
                    AppLogHelp.WriteError(LogFileFormate.AppMain, " 读取autoAPPiniConfig配置项 错误:" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.ToString());
                AppLogHelp.WriteError(LogFileFormate.AppMain, " autoAPPiniConfig配置项 错误:" + ex.Message);
            }
            finally
            {
                //LoadDataSet();
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                this.openFileDialog1.InitialDirectory = strBackUpPath;
                if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.lbBackUpFile.Items.Clear();
                    this.lbDatatableName.Items.Clear();
                    foreach (string strFileName in this.openFileDialog1.FileNames)
                    {
                        this.lbBackUpFile.Items.Add(strFileName);
                        this.lbDatatableName.Items.Add(Path.GetFileNameWithoutExtension(strFileName).Split('_')[0]);
                    }
                }

            }
            catch (Exception ex)
            {
                //log.WriteErr(ex);
                AppLogHelp.WriteError(LogFileFormate.AppMain, ex.Message);
            }
            finally
            {
                //LoadDataSet();

            }

        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                bu_Joch _bu = new bu_Joch();
                string strTableName = string.Empty;
                string strBackUpFile = string.Empty;
                int intSuccessReturn = 0;
                int intUnSuccessReturn = 0;
                string strUnSuccessFile = string.Empty;

                for (int i = 0; i < this.lbBackUpFile.Items.Count; i++)
                {
                    strTableName = this.lbDatatableName.Items[i].ToString();
                    strBackUpFile = this.lbBackUpFile.Items[i].ToString();
                    try
                    {
                        //lin 20190813
                        //intSuccessReturn += _bu.LoadDataFromFile(strTableName, strBackUpFile, Properties.Settings.Default.MySQLConnect);
                        intSuccessReturn += _bu.LoadDataFromFile(strTableName, strBackUpFile,  WSClnt.PubStaticParam._strSPIdbConnectionString);
                    }
                    catch (Exception exx)
                    {
                        intUnSuccessReturn++;
                        strUnSuccessFile += Path.GetFileName(strBackUpFile) + " Load Data Exception :" + exx.Message + "\r\n";
                    }
                }

                MessageBox.Show(string.Format("成功还原{0}条数据, 还原错误{1}个文件: {2}", intSuccessReturn, intUnSuccessReturn, strUnSuccessFile));
            }
            catch (Exception ex)
            {
                //log.WriteErr(ex);
                AppLogHelp.WriteError(LogFileFormate.Delete, ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                //LoadDataSet();

            }

        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.txtSaveFolder.Text = this.folderBrowserDialog1.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                //log.WriteErr(ex);
            }
            finally
            {
                //LoadDataSet();

            }

        }

        private void btnEXFOAuto_Click(object sender, EventArgs e)
        {
            dtLastRunTime = Convert.ToDateTime("2011-11-11 00:00:00");

            //ini 更新数据
            string strEXINIFile = Path.Combine(strConfigIniPath,_strIniAutoAPPConfigFileName);
            if (File.Exists(strEXINIFile))
            {
                WSClnt.INIFileHelper.WriteIniData("autoAPP", "EXFOConnectionString", this.txtEXFOSQLConn.Text.Trim(), strEXINIFile);
                WSClnt.INIFileHelper.WriteIniData("autoAPP", "EXFOAutoTimeString", this.txtEXFOStartTime.Text.Trim(), strEXINIFile);
            }
            if (this.btnEXFOAuto.Text == "停止同步")
            {
                this.btnEXFOAuto.Text = "自动同步";
                //this.timer1.Enabled = false;
                this.btnEXFOInsert.Enabled = true;
                this.dtpEXFOStartTime.Enabled = true;
                this.dtpEXFOEndTime.Enabled = true;
                blnSynchronousDataForEXFO = false;
            }
            else if (this.btnEXFOAuto.Text == "自动同步")
            {
                this.btnEXFOAuto.Text = "停止同步";
                //this.timer1.Enabled = true;
                this.btnEXFOInsert.Enabled = false;
                this.dtpEXFOStartTime.Enabled = false;
                this.dtpEXFOEndTime.Enabled = false;
                blnSynchronousDataForEXFO = true;
            }

        }

        private void btnEXFOTest_Click(object sender, EventArgs e)
        {
            try
            {
                string strDatatableName = Properties.Settings.Default.EXFOTableName;
                DataTable dtReturn = bu_joch.GetDataTableFromEXFO(this.txtEXFOSQLConn.Text, strDatatableName);
                if (dtReturn != null)
                {
                    MessageBox.Show("连接测试成功！");
                }
                //this.timer2.Enabled = true;
            }
            catch (System.Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.Message);
            }
            finally
            {
                //LoadDataSet();
            }
        }

        private void btnEXFOInsert_Click(object sender, EventArgs e)
        {
            try
            {

                //ini 跟新数据
                string strEXINIFile = Path.Combine(strConfigIniPath, _strIniAutoAPPConfigFileName);
                if (File.Exists(strEXINIFile))
                {
                    WSClnt.INIFileHelper.WriteIniData("autoAPP", "EXFOConnectionString", this.txtEXFOSQLConn.Text.Trim(), strEXINIFile);
                    WSClnt.INIFileHelper.WriteIniData("autoAPP", "EXFOAutoTimeString", this.txtEXFOStartTime.Text.Trim(), strEXINIFile);
                }
                

                dtEXFOLastRunTime = Convert.ToDateTime("2011-11-11 00:00:00");

                if (this.dtpEXFOStartTime.Checked == false)
                {
                    MessageBox.Show("请选择开始日期");
                }
                if (this.dtpEXFOEndTime.Checked == false)
                {
                    MessageBox.Show("请选择结束日期");
                }


                if (dtpEXFOStartTime.Checked == true && dtpEXFOEndTime.Checked == true)
                {
                    if (MessageBox.Show("确定开始手动同步SPC数据", "手动同步", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        //this.timer2.Enabled = true;
                        bu_joch.AutoStartEXFO(this.dtpEXFOStartTime.Value, dtpEXFOEndTime.Value);
                    }
                }
                //LoadDataSet();
            }
            catch (System.Exception ex)
            {
               // log.WriteErr("错误 ! " + ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        private void lbDatatableName_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (txtEdit.Visible == false && lbDatatableName.Items != null && lbDatatableName.SelectedIndex > -1)
                {
                    int itemSelected = lbDatatableName.SelectedIndex;
                    string itemText = lbDatatableName.Items[itemSelected].ToString();

                    Rectangle rect = new Rectangle(lbDatatableName.GetItemRectangle(itemSelected).Location,
                        new Size(lbDatatableName.GetItemRectangle(itemSelected).Width,
                        lbDatatableName.GetItemRectangle(itemSelected).Height + 5)
                        );

                    txtEdit.Parent = lbDatatableName;
                    txtEdit.Bounds = rect;
                    txtEdit.Multiline = true;
                    txtEdit.Visible = true;
                    txtEdit.Text = itemText;
                    txtEdit.Focus();
                    txtEdit.SelectAll();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }


        }

        private void lbDatatableName_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                txtEdit.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }

        private void btnCSVTest_Click(object sender, EventArgs e)
        {
            try
            {
                string strMsg = bu_joch.TestConnection(this.txtCSVMySQLConnect.Text);
                MessageBox.Show(strMsg);
                //this.timer2.Enabled = true;
                //this.timer2.Start();
            }
            catch (System.Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.Message);
            }
            finally
            {
                //LoadDataSet();
            }
        }

        private void btnCSVAuto_Click(object sender, EventArgs e)
        {
            try
            {

                dtLastRunTime = Convert.ToDateTime("2011-11-11 00:00:00");

                if (this.btnCSVAuto.Text == "停止自動運行")
                {
                    this.btnCSVAuto.Text = "開始自動運行";
                    //this.timer1.Enabled = false;
                    this.btnCSVSave.Enabled = true;
                    this.dtpCSVEndTime.Enabled = true;
                    this.dtpCSVStartTime.Enabled = true;
                    blnSaveCSVFileForFoxconn = false;
                }
                else if (this.btnCSVAuto.Text == "開始自動運行")
                {
                    this.btnCSVAuto.Text = "停止自動運行";
                    //this.timer1.Enabled = true;
                    this.btnCSVSave.Enabled = false;
                    this.dtpCSVEndTime.Enabled = false;
                    this.dtpCSVStartTime.Enabled = false;
                    blnSaveCSVFileForFoxconn = true;
                }

            }
            catch (System.Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.Message);
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCSVSave_Click(object sender, EventArgs e)
        {
            try
            {
                dtLastRunTime = Convert.ToDateTime("2011-11-11 00:00:00");

                if (this.dtpCSVStartTime.Checked == false)
                {
                    MessageBox.Show("请选择开始日期");
                }
                if (this.dtpCSVEndTime.Checked == false)
                {
                    MessageBox.Show("请选择结束日期");
                }
                string strSaveFloder = this.txtCSVFile.Text;
                if (string.IsNullOrEmpty(strSaveFloder))
                {
                    MessageBox.Show("请选择CSV保存目录");
                }
                int AintSectionTime = (int)nudTimeSpan.Value;
                if (dtpCSVStartTime.Checked == true && dtpCSVEndTime.Checked == true)
                {
                    if (MessageBox.Show("确定开始手动生成富士康CSV数据文件", "手动生成", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        //this.timer2.Enabled = true;
                        bu_peng.AutoStartFoxconn(this.dtpCSVStartTime.Value, dtpCSVEndTime.Value, strSaveFloder, AintSectionTime);
                    }
                }
                //LoadDataSet();
            }
            catch (System.Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSCVFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.txtCSVFile.Text = this.folderBrowserDialog1.SelectedPath;
                }
            }
            catch (Exception ex)
            {

                AppLogHelp.WriteError(LogFileFormate.MES, ex.ToString());
            }
            finally
            {
                //LoadDataSet();

            }
        }

        private void btnSaveJufei_Click(object sender, EventArgs e)
        {
            try
            {
                dtLastRunTime = Convert.ToDateTime("2011-11-11 00:00:00");

                if (this.dtpStartTimeJufei.Checked == false)
                {
                    MessageBox.Show("请选择开始日期");
                }
                if (this.dtpEndTimeJufei.Checked == false)
                {
                    MessageBox.Show("请选择结束日期");
                }
                string strSaveFloder = this.savePathJufei.Text;
                if (string.IsNullOrEmpty(strSaveFloder))
                {
                    MessageBox.Show("请选择文件保存目录");
                }
                if (dtpStartTimeJufei.Checked == true && dtpEndTimeJufei.Checked == true)
                {
                    if (MessageBox.Show("确定开始导出聚飞数据文件", "手动生成", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        //this.timer2.Enabled = true;
                        bu_peng.AutoStartJufei(this.dtpStartTimeJufei.Value, this.dtpEndTimeJufei.Value, strSaveFloder);
                    }
                }
                //LoadDataSet();
            }
            catch (System.Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        private void chkCSVAutoRun_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnJufeiFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.savePathJufei.Text = this.folderBrowserDialog1.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                //log.WriteErr(ex);
            }
            finally
            {
                //LoadDataSet();

            }
        }

        private void btnJuFeiFileRun_Click(object sender, EventArgs e)
        {
            try
            {
                dtLastRunTime = Convert.ToDateTime("2011-11-11 00:00:00");

                if (this.btnJuFeiFileRun.Text == "停止自动")
                {
                    this.btnJuFeiFileRun.Text = "自动";
                    //this.timer1.Enabled = false;
                    this.btnSaveJufei.Enabled = true;
                    this.dtpEndTimeJufei.Enabled = true;
                    this.dtpStartTimeJufei.Enabled = true;
                    blnSaveDataForJUFEI = false;
                }
                else if (this.btnJuFeiFileRun.Text == "自动")
                {
                    this.btnJuFeiFileRun.Text = "停止自动";
                    //this.timer1.Enabled = true;
                    this.btnSaveJufei.Enabled = false;
                    this.dtpEndTimeJufei.Enabled = false;
                    this.dtpStartTimeJufei.Enabled = false;
                    blnSaveDataForJUFEI = true;
                }

            }
            catch (System.Exception ex)
            {
                ///log.WriteErr("错误 ! " + ex.Message);
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnTestJufei_Click(object sender, EventArgs e)
        {
            try
            {
                string strMsg = bu_joch.TestConnection(this.textJufeiMySQLConnect.Text);
                MessageBox.Show(strMsg);
                //this.timer2.Enabled = true;
                //this.timer2.Start();
            }
            catch (System.Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.Message);
            }
            finally
            {
                //LoadDataSet();
            }
        }

        private void btnTxtConfig_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.txtConfigFile.Text = this.folderBrowserDialog1.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                //log.WriteErr(ex);
            }
            finally
            {
                //LoadDataSet();

            }
        }

        private void btnCSVTestForCaiHuang_Click(object sender, EventArgs e)
        {
            try
            {
                string strMsg = bu_peng.TestConnection(this.txtCSVMySQLConnect.Text);
                MessageBox.Show(strMsg);
                //this.timer2.Enabled = true;
                //this.timer2.Start();
            }
            catch (System.Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.Message);
            }
            finally
            {
                //LoadDataSet();
            }
        }

        private void btnCSVAutoForCaiHuang_Click(object sender, EventArgs e)
        {
            try
            {

                dtLastRunTime = Convert.ToDateTime("2011-11-11 00:00:00");

                if (this.btnCSVAutoForCaiHuang.Text == "停止自動運行")
                {
                    this.btnCSVAutoForCaiHuang.Text = "開始自動運行";
                    //this.timer1.Enabled = false;
                    this.btnCSVSaveForCaiHuang.Enabled = true;
                    this.dtpCSVEndTimeForCaiHuang.Enabled = true;
                    this.dtpCSVStartTimeForCaiHuang.Enabled = true;
                    this.txtCSVRunTimeForCaiHuang.Enabled = true;
                    bEnDataForCaiHuang = false;
                }
                else if (this.btnCSVAutoForCaiHuang.Text == "開始自動運行")
                {
                    this.btnCSVAutoForCaiHuang.Text = "停止自動運行";
                    //this.timer1.Enabled = true;
                    this.btnCSVSaveForCaiHuang.Enabled = false;
                    this.dtpCSVEndTimeForCaiHuang.Enabled = false;
                    this.dtpCSVStartTimeForCaiHuang.Enabled = false;
                    this.txtCSVRunTimeForCaiHuang.Enabled = false;
                    bEnDataForCaiHuang = true;
                }

            }
            catch (System.Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.Message);
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCSVSaveForCaiHuang_Click(object sender, EventArgs e)
        {
            try
            {
                dtLastRunTime = Convert.ToDateTime("2011-11-11 00:00:00");
                if (this.dtpCSVStartTimeForCaiHuang.Checked == false)
                {
                    MessageBox.Show("请选择开始日期");
                }
                if (this.dtpCSVEndTimeForCaiHuang.Checked == false)
                {
                    MessageBox.Show("请选择结束日期");
                }
                string strSaveFloder = this.txtCSVFileForCaiHuang.Text;
                if (string.IsNullOrEmpty(strSaveFloder))
                {
                    MessageBox.Show("请选择CSV保存目录");
                }
                if (dtpCSVStartTimeForCaiHuang.Checked == true && dtpCSVEndTimeForCaiHuang.Checked == true)
                {
                    if (MessageBox.Show("确定开始手动彩煌实业CSV数据文件", "手动生成", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        //this.timer2.Enabled = true;
                        //bu.AutoStartFoxconn(this.dtpCSVStartTime.Value, dtpCSVEndTime.Value, strSaveFloder, AintSectionTime, log);
                        bu_peng.AutoStartCaiHuang(this.dtpCSVStartTimeForCaiHuang.Value, this.dtpCSVEndTimeForCaiHuang.Value, this.txtCSVFileForCaiHuang.Text);
                    }
                }
                //LoadDataSet();
            }
            catch (System.Exception ex)
            {
               // log.WriteErr("错误 ! " + ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSaveCaiHuang_Click(object sender, EventArgs e)
        {
            try
            {
                string strFile = Path.Combine(_strIniCaiHuangPath, "caihuang.ini");
                if (!Directory.Exists(_strIniCaiHuangPath))
                {
                    Directory.CreateDirectory(_strIniCaiHuangPath);
                }
                if (!File.Exists(strFile))
                {
                    using (FileStream fs = new FileStream(strFile, FileMode.Create))
                    {
                    }
                }
                if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.txtCSVFileForCaiHuang.Text = this.folderBrowserDialog1.SelectedPath;

                    //add by peng 20180802   添加做一个保存数据至本地;
                    if (File.Exists(strFile))
                    {
                        bu_Peng.WriteIniData("Path", "path", this.txtCSVFileForCaiHuang.Text, strFile);
                    }
                    else
                    {
                        MessageBox.Show("iniFile is not exits!");
                    }

                }
            }
            catch (Exception ex)
            {
                //log.WriteErr(ex);
                AppLogHelp.WriteLog(LogFileFormate.MES, "btnSaveCaiHuang_Click" + ex.Message);
            }
            finally
            {
                //LoadDataSet();

            }
        }

        private void iniCaiHuangPath()
        {
            string strFile = System.IO.Path.Combine(_strIniCaiHuangPath, "caihuang.ini");
            try
            {
                if (System.IO.File.Exists(strFile))
                {
                    string strTmp = bu_Peng.ReadIniData("Path", "path", string.Empty, strFile);
                    if (!string.IsNullOrEmpty(strTmp))
                    {
                        this.txtCSVFileForCaiHuang.Text = strTmp;
                    }
                }
            }
            catch (Exception ex)
            {
               // log.WriteErr(ex);
                AppLogHelp.WriteError(LogFileFormate.MES, "iniCaiHuangPath:" + ex.Message);
            }
            finally
            {
                //LoadDataSet();
            }

        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {

            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                //PubParams._log.WriteErr(ex);
                MessageBox.Show(ex.ToString());
                //  throw ex;
            }
        }

        private void tsmiShowAPP_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.ShowInTaskbar = true;  //显示在系统任务栏
                    this.WindowState = FormWindowState.Normal;  //还原窗体
                    // this.notifyIcon1.Visible = false;  //托盘图标隐藏
                }
                else
                {
                    this.WindowState = FormWindowState.Minimized;
                }
            }
            catch (Exception ex)
            {
                //PubParams._log.WriteErr(ex);
                MessageBox.Show(ex.ToString());
                //  throw ex;
            }
        }

        private const int CP_NOCLOSE_BUTTON = 0x200;

        protected override CreateParams CreateParams
        {
            get
            {

                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        private void tsmiExit_Click_1(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        private void btnTxtPathRenBao_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    string tmpPath = Path.Combine(_strLogPengPath, "RenBao");
                    string strINIFile = Path.Combine(tmpPath, "renbao.ini");
                    this.tbTextPathRenBao.Text = this.folderBrowserDialog1.SelectedPath;
                    WSClnt.INIFileHelper.WriteIniData("RenBao", "iniPath", this.tbTextPathRenBao.Text, strINIFile);
                }
            }
            catch (Exception ex)
            {
                //log.WriteErr(ex);
            }
            finally
            {
                //LoadDataSet();

            }
        }

        private void btnStartRenBao_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.btnStartRenBao.Text == "停止监控并退出")
                {
                    this.btnStartRenBao.Text = "开启监控";
                    //隐藏btn读取文件按钮
                    //this.btnTouchReabBao.Visible = false;
                    bu_peng.StopThreadRenBao();
                }
                else if (this.btnStartRenBao.Text == "开启监控")
                {
                    this.btnStartRenBao.Text = "停止监控并退出";
                    this.btnTouchReabBao.Visible = true;
                    //textPath
                    if (string.IsNullOrEmpty(this.tbTextPathRenBao.Text))
                    {
                        this.tbTextPathRenBao.Text = @"D:\EYSPI\DataExport\";
                    }
                    //log.WriteLog("线程开启", "RenBao");
                    AppLogHelp.WriteLog(LogFileFormate.MES, "RenBao 线程开启");
                    _bRenBaoFileIsOpen = true;  //open renbaoFile
                    bu_peng.StartRenBao(this.tbTextPathRenBao.Text,this.tbTextPathRenBao2.Text);
                }

            }
            catch (System.Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.Message);
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnTxtPathRenBao2_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    string tmpPath = Path.Combine(_strLogPengPath, "RenBao");
                    string strINIFile = Path.Combine(tmpPath, "renbao.ini");
                    this.tbTextPathRenBao2.Text = this.folderBrowserDialog1.SelectedPath;
                    WSClnt.INIFileHelper.WriteIniData("RenBao", "iniPath2", this.tbTextPathRenBao2.Text, strINIFile);
                }
            }
            catch (Exception ex)
            {
                //log.WriteErr(ex);
                AppLogHelp.WriteError(LogFileFormate.MES, "RenBao!" + ex.Message);
            }
            finally
            {
                //LoadDataSet();

            }
        }

        private void buttonRenBaoSaveINI_Click(object sender, EventArgs e)
        {
            string strAutoAPPIniFile = Path.Combine(strConfigIniPath, _strIniAutoAPPConfigFileName);
            try
            {
                if(File.Exists(strAutoAPPIniFile))
                {
                    WSClnt.INIFileHelper.WriteIniData("KunShanRenBao", "tbRenBaoSaoplant", this.tbRenBaoSaoplant.Text, strAutoAPPIniFile);
                    WSClnt.INIFileHelper.WriteIniData("KunShanRenBao", "tbRenBaoGroupName", this.tbRenBaoGroupName.Text, strAutoAPPIniFile);
                    WSClnt.INIFileHelper.WriteIniData("KunShanRenBao", "tbRenBaoLineName", this.tbRenBaoLineName.Text, strAutoAPPIniFile);

                    WSClnt.INIFileHelper.WriteIniData("KunShanRenBao", "tbRenBaoCuscode", this.tbRenBaoCuscode.Text, strAutoAPPIniFile);
                    WSClnt.INIFileHelper.WriteIniData("KunShanRenBao", "tbRenBaoPross", this.tbRenBaoPross.Text, strAutoAPPIniFile);
                    WSClnt.INIFileHelper.WriteIniData("KunShanRenBao", "tbRenBaoFactorySN", this.tbRenBaoFactorySN.Text, strAutoAPPIniFile);

                    WSClnt.INIFileHelper.WriteIniData("KunShanRenBao", "tbRenBaoSide", this.tbRenBaoSide.Text, strAutoAPPIniFile);
                    WSClnt.INIFileHelper.WriteIniData("KunShanRenBao", "tbRenBaoEquipType", this.tbRenBaoEquipType.Text, strAutoAPPIniFile);
                    WSClnt.INIFileHelper.WriteIniData("KunShanRenBao", "tbRenBaoOp", this.tbRenBaoOp.Text, strAutoAPPIniFile);
                    MessageBox.Show("保存设置成功");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        private bool _bRenBaoFileIsOpen = false;
        private bool _bDaHua = true;
        private bool _bIsLuBangTongLogin = false;
        private bool _bLuBangTongThread = true;
        private bool _bLuBangTongPrdThread = true;
        private bool _bLuBangTongSnUpLoadSnThread = true;
        private string _strLuBangTongPrdPath = "";
        private string _FovDirPath = "";
        //EYSPIToAOI
        public static bool bAOI = true;
        private bool bEnNanChangHuaQin = false;
        private bool _bHuaQin = true;
        //Boe
        private bool bEnDataBoeOPen = true;
        //SkyWorth
        private bool bEnDataSkyWorthOpen = true;
        //SPCData
        private bool bSpcOpen = true;
        //ZHAOCHISamsung
        private bool bOpenZHAOCHISamsung = true;
        private string ZHAOCHISamsung_TMPFILE_PATH = string.Empty;
        private string ZHAOCHISamsung_SAVEFILE_PATH = string.Empty;
        [DllImport("psapi.dll")]
        static extern int EmptyWorkingSet(IntPtr hwProc);
        public static void ClearMemory()
        {
            GC.Collect();
            //GC.WaitForPendingFinalizers();
            //Process[] processes = Process.GetProcesses();
            //foreach (Process process in processes)
            //{
            //    try
            //    {
            //        EmptyWorkingSet(process.Handle);
            //    }
            //    catch
            //    {
            //        return;
            //    }
            //}
        }

        private void cbBoxPcbFov_CheckedChanged(object sender, EventArgs e)
        {
            string strAutoAPPIniFile = Path.Combine(strConfigIniPath, _strIniAutoAPPConfigFileName);
                //modify by joch  20180904
            if (File.Exists(strAutoAPPIniFile))
            {
                System.Threading.Thread.Sleep(300);
                bu_Peng.WriteIniData("autoAPP", "EnDataClosePCBFOV", this.cbBoxPcbFov.Checked.ToString(), strAutoAPPIniFile);

            }

        }

        private void btnDahuaFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    //string tmpPath = Path.Combine(strConfigIniPath, "DaHua");
                    string strINIFile = Path.Combine(strConfigIniPath, _strIniAutoAPPConfigFileName);
                    this.tbDahuaFilePath.Text = this.folderBrowserDialog1.SelectedPath;
                    WSClnt.INIFileHelper.WriteIniData("DaHua", "iniPath", this.tbDahuaFilePath.Text, strINIFile);
                }
            }
            catch (Exception ex)
            {
                //log.WriteErr(ex);
                AppLogHelp.WriteError(LogFileFormate.MES, "DaHua" + ex.Message);
            }
            finally
            {
                //LoadDataSet();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.btnAutoDahua.Text == "停止自动运行")
            {
                this.btnAutoDahua.Text = "自动运行";
                
                bEnZheJaingDaHua = false;
                if (_runnerDaHua != null
                            && (_runnerDaHua.threadProcess.ThreadState == System.Threading.ThreadState.Running
                            || _runnerDaHua.threadProcess.ThreadState == System.Threading.ThreadState.Background
                            || _runnerDaHua.Running))
                {
                    _bDaHua = true;
                    //log.WriteLog("关闭", "DaHua");
                    _runnerDaHua.Stop();
                }
            }
            else if (this.btnAutoDahua.Text == "自动运行")
            {
                this.btnAutoDahua.Text = "停止自动运行";

                bEnZheJaingDaHua = true;
                
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (this.btnLogin.Text == "登录")
            {
                try
                {
                    string url = this.URLLOGIN.Text.Trim();
                    string clientId = this.clientId.Text.Trim();
                    string clientSecret = this.clientSecret.Text.Trim();
                    string loginID = this.btnLogin.Text.Trim();
                    string pwd = this.tbLoginPass.Text.Trim();
                    string spo = this.tbScope.Text.Trim();
                    if (string.IsNullOrEmpty(url))
                    {
                        url = "http://{mesdomain:port}/mesapi/clientlogin";
                    }
                    if (string.IsNullOrEmpty(url))
                    {
                        clientId = "s8030";
                    }
                    if (string.IsNullOrEmpty(url))
                    {
                        clientSecret = "mesclientsecret";
                    }
                    if (string.IsNullOrEmpty(url))
                    {
                        loginID = "Login";
                    }
                    if (string.IsNullOrEmpty(url))
                    {
                    }
                    if (string.IsNullOrEmpty(url))
                    {
                        spo = "Operator";
                    }
                    string strJson = string.Empty;
                    strJson = string.Concat(new string[]
				    {
					    "{ \"clientId\":\"",
					    clientId,
					    "\",\"clientSecret\":\"",
					    clientSecret,
					    "\",\"loginId\":\"",
					    loginID,
					    "\",\"loginPass\":\"",
					    this.tbLoginPass.Text,
					    "\",\"scope\":\"",
					    spo,
					    "\"}"
				    });
                    //test
                    string strHttpResult = WebHttpHelp.HttpPost(this.URLLOGIN.Text, strJson);
                    strHttpResult = strHttpResult.Replace("\"{", "{").Replace("\\\"", "\"").Replace("}\"", "}");
                    JsonConverStatus clogin = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonConverStatus>(strHttpResult);
                    _strToken = clogin.data;
                    if (clogin.message.Trim() == "success" || !string.IsNullOrEmpty(clogin.data))
                    {
                        MessageBox.Show("登陆成功!");
                        //log.WriteLog("登陆成功!", "LuBangTong");
                        AppLogHelp.WriteLog(LogFileFormate.MES, "LuBangTong登陆成功!" );
                        this.btnLogin.Text = "已登录";
                        _bIsLuBangTongLogin = true;
                    }
                    else
                    {
                        MessageBox.Show("登录失败!" + clogin.message);
                        //log.WriteLog("登陆失败!" + clogin.message, "LuBangTong");
                        AppLogHelp.WriteLog(LogFileFormate.MES, "LuBangTong登陆失败!");
                        this.btnLogin.Text = "登录";
                        _bIsLuBangTongLogin = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    //log.WriteLog("登陆异常!" + ex.Message, "LuBangTong");
                    AppLogHelp.WriteLog(LogFileFormate.MES, "LuBangTong登陆异常!" + ex.Message);
                    // this.writeLog(this.strFilePath, "LOGIN:登陆异常!:" + ex.Message);
                }
                finally
                {

                }
            }
            else if (this.btnLogin.Text == "已登录")
            {

            }

        }

        private void btnUploadStatus_Click(object sender, EventArgs e)
        {
            string strAutoAPPIniFile = Path.Combine(strConfigIniPath, _strIniAutoAPPConfigFileName);
            if (File.Exists(strAutoAPPIniFile))
            {
                System.Threading.Thread.Sleep(300);
                bu_Peng.WriteIniData("LuBangTong", "clientId", this.clientId.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "clientSecret", this.clientSecret.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbloginId", this.tbloginId.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbLoginPass", this.tbLoginPass.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbScope", this.tbScope.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "URLLOGIN", this.URLLOGIN.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbStatusURL", this.tbStatusURL.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbErrorUrl", this.tbErrorUrl.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbOn", this.tbOn.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbSf", this.tbSf.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbMachineNo", this.tbMachineNo.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbITEMVALUE", this.tbITEMVALUE.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbMO", this.tbMO.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbUSERNO", this.tbUSERNO.Text, strAutoAPPIniFile);
                MessageBox.Show("保存成功!");
            }


        }

        private void uploadError_Click(object sender, EventArgs e)
        {
            string strAutoAPPIniFile = Path.Combine(strConfigIniPath, _strIniAutoAPPConfigFileName);
            if (File.Exists(strAutoAPPIniFile))
            {
                System.Threading.Thread.Sleep(300);
                bu_Peng.WriteIniData("LuBangTong", "clientId", this.clientId.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "clientSecret", this.clientSecret.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbloginId", this.tbloginId.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbLoginPass", this.tbLoginPass.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbScope", this.tbScope.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "URLLOGIN", this.URLLOGIN.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbStatusURL", this.tbStatusURL.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbErrorUrl", this.tbErrorUrl.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbOn", this.tbOn.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbSf", this.tbSf.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbMachineNo", this.tbMachineNo.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbITEMVALUE", this.tbITEMVALUE.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbMO", this.tbMO.Text, strAutoAPPIniFile);
                bu_Peng.WriteIniData("LuBangTong", "tbUSERNO", this.tbUSERNO.Text, strAutoAPPIniFile);
                MessageBox.Show("保存成功!");
            }
        }
        //Add by asce.woo-20190312
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //threadMain1
            _threadMain = new Thread(new ThreadStart(RunMain));
            _threadMain.IsBackground = true;
            _threadMain.Start();

            //threadMain2

            _threadMainCpTwo = new Thread(new ThreadStart(RunMainCpTwo));
            _threadMainCpTwo.IsBackground = true;
            _threadMainCpTwo.Start();

            //thread三点照合

            _threadMainCpEYSPIToAOI = new Thread(new ThreadStart(RunMainCpEYspiToAOI));
            _threadMainCpEYSPIToAOI.IsBackground = true;
            _threadMainCpEYSPIToAOI.Start();

            //thread FOVPCB  _threadMainCpFOVPCB
            _threadMainCpFOVPCB = new Thread(new ThreadStart(RunMainCpFOVPCB));
            _threadMainCpFOVPCB.IsBackground = true;
            _threadMainCpFOVPCB.Start();

            //thread OnLineFOVPCB  _threadMainCpOnLineFOVPCB
            _threadMainCpOnLineFOVPCB = new Thread(new ThreadStart(RunMainCpOnLineFOVPCB));
            _threadMainCpOnLineFOVPCB.IsBackground = true;
            _threadMainCpOnLineFOVPCB.Start();

            //thread 自动删除
            _threadMainCpDelete = new Thread(new ThreadStart(RunMainCpDelete));
            _threadMainCpDelete.IsBackground = true;
            _threadMainCpDelete.Start();

            //thread SPC上传

            _threadMainCpSPCDataUpload = new Thread(new ThreadStart(RunMainCpSPCDataUpload));
            _threadMainCpSPCDataUpload.IsBackground = true;
            _threadMainCpSPCDataUpload.Start();

            //show message .  read appsetting
            _threadMainShowMessage = new Thread(new ThreadStart(RunMainShowMessage));
            _threadMainShowMessage.IsBackground = true;
            _threadMainShowMessage.Start();
        }

        private delegate void SetDelegageRunMain();
        private void RunMain()
        {
            int iCore = Environment.ProcessorCount;
            iCore--;
            iCore--;
            SetThreadAffinityMask(GetCurrentThread(), new UIntPtr(SetCpuID(iCore)));
            //SetThreadAffinityMask(GetCurrentThread(), new IntPtr(1 << INT_TWO));
            SetDelegageRunMain sm = new SetDelegageRunMain(DoRunMain);
            //this.BeginInvoke(new SetDelegageRunMain(DoRunMain));
            while (_bIsKeepRunning)
            {
                if (_bIsRunMainFlag)
                {
                    //_bIsRunMainFlag = false;
                    
                    //this.BeginInvoke(new SetDelegageRunMain(DoRunMain));
                    this.BeginInvoke(sm);
                        //Application.DoEvents();
                    
                }
                Thread.Sleep(_iMainSleepTime);
                
            }
        }
        private delegate void SetDelegageRunMainDelete();
        private void RunMainCpDelete()
        {
            int iCore = Environment.ProcessorCount;
            iCore--;
            iCore--;
            iCore--;
            
            if (iCore <= 0)
            {
                iCore = 2;
            }
            SetThreadAffinityMask(GetCurrentThread(), new UIntPtr(SetCpuID(iCore)));

            //SetThreadAffinityMask(GetCurrentThread(), new IntPtr(1 << INT_TWO));
            //this.BeginInvoke(new SetDelegageRunMain(DoRunMain));
            SetDelegageRunMainDelete sd = new SetDelegageRunMainDelete(DoRunMainDelete);
            while (_bIsKeepRunning)
            {
                if (_bIsRunMainCpDeleteFlag)
                {
                    //log.WriteLog("cpu start log", "Processor");
                    //log.WriteLog(Marshal.PtrToStringAnsi(GetCurrentProcessorNumberEx()), "Processor");
                    //_bIsRunMainCp2Flag = false;
                    //this.BeginInvoke(new SetDelegageRunMainDelete(DoRunMainDelete));
                    this.BeginInvoke(sd);
                }
                Thread.Sleep(_iMainSleepTime);
            }
        }
        private delegate void SetDelegageRunMainFOVPCB();
        private void RunMainCpFOVPCB()
        {
            //int iCore = Environment.ProcessorCount;
            //iCore--;
            //iCore--;
            ////iCore--;
            //if (iCore <= 0)
            //{
            //    iCore = 2;
            //}
            //SetThreadAffinityMask(GetCurrentThread(), new UIntPtr(SetCpuID(iCore)));
            //MyInvoke mi = new MyInvoke(DoRunMainFOVPCB);
            //SetThreadAffinityMask(GetCurrentThread(), new IntPtr(1 << INT_TWO));
            //this.BeginInvoke(new SetDelegageRunMain(DoRunMain));
            //object[] obj = new object[4];
            //obj[0] = log;
            //obj[1] = _configData;
            //obj[2] = _FovDirPath;
            //obj[3] = _appSettingHandle;
            SetDelegageRunMainFOVPCB fov = new SetDelegageRunMainFOVPCB(DoRunMainFOVPCB);
            while (_bIsKeepRunning)
            {
                if (_bIsRunMainCpFOVPCBFlag)
                {
                    //log.WriteLog("cpu start log", "Processor");
                    //log.WriteLog(Marshal.PtrToStringAnsi(GetCurrentProcessorNumberEx()), "Processor");
                    //_bIsRunMainCp2Flag = false;
                    //ReadFovImages(log, _configData, _FovDirPath,_appSettingHandle)

                    //}
                    // Thread.Sleep(_iMainSleepTime);
                    //if (_threadMainCpFOVPCB != null && _threadMainCpFOVPCB.IsAlive)
                    //{
                    //    Application.DoEvents();
                    //}
                    this.BeginInvoke(fov);
                    //DoRunMainFOVPCB();
                }
                Thread.Sleep(_iMainSleepTime);
            }
        }
        private delegate void SetDelegageRunMainOnLineFOVPCB();
        private void RunMainCpOnLineFOVPCB()
        {
            //int iCore = Environment.ProcessorCount;
            //iCore--;
            //iCore--;
            ////iCore--;
            //if (iCore <= 0)
            //{
            //    iCore = 2;
            //}
            //SetThreadAffinityMask(GetCurrentThread(), new UIntPtr(SetCpuID(iCore)));
            //MyInvoke mi = new MyInvoke(DoRunMainFOVPCB);
            //SetThreadAffinityMask(GetCurrentThread(), new IntPtr(1 << INT_TWO));
            //this.BeginInvoke(new SetDelegageRunMain(DoRunMain));
            //object[] obj = new object[4];
            //obj[0] = log;
            //obj[1] = _configData;
            //obj[2] = _FovDirPath;
            //obj[3] = _appSettingHandle;
            SetDelegageRunMainFOVPCB fov = new SetDelegageRunMainFOVPCB(DoRunMainOnLineFOVPCB);
            while (_bIsKeepRunning)
            {
                if (_bIsRunMainCpOnLineFOVPCBFlag)
                {
                    
                    this.BeginInvoke(fov);
                    
                }
                Thread.Sleep(_iMainSleepTime);
            }
        }
        private delegate void SetDelegageRunMainEYspiToAOI();
        private void RunMainCpEYspiToAOI()
        {
            int iCore = Environment.ProcessorCount;
            iCore--;
            //iCore--;
            if (iCore <= 0)
            {
                iCore = 2;
            }
            SetThreadAffinityMask(GetCurrentThread(), new UIntPtr(SetCpuID(iCore)));
            SetDelegageRunMainEYspiToAOI si = new SetDelegageRunMainEYspiToAOI(DoRunMainCpEYSPIToAOI);
            while (_bIsKeepRunning)
            {
                if (_bIsRunMainCpEYSPIToAOIFlag)
                {
                    //log.WriteLog("cpu start log", "Processor");
                    //log.WriteLog(Marshal.PtrToStringAnsi(GetCurrentProcessorNumberEx()), "Processor");
                    //_bIsRunMainCp2Flag = false;
                    //this.BeginInvoke(new SetDelegageRunMainEYspiToAOI(DoRunMainCpEYSPIToAOI));
                    this.BeginInvoke(si);
                }
                Thread.Sleep(_iMainSleepTime);
            }
        }
        private delegate void SetDelegageRunMainTwo();
        private void RunMainCpTwo()
        {
            int iCore = Environment.ProcessorCount;
            iCore--;
            iCore--;
            if (iCore <= 0)
            {
                iCore = 2;
            }
            SetThreadAffinityMask(GetCurrentThread(), new UIntPtr(SetCpuID(iCore)));

            //SetThreadAffinityMask(GetCurrentThread(), new IntPtr(1 << INT_TWO));
            
            //this.BeginInvoke(new SetDelegageRunMain(DoRunMain));
            SetDelegageRunMainTwo sm =new SetDelegageRunMainTwo(DoRunMainCp);
            while (_bIsKeepRunning)
            {
                if (_bIsRunMainCp2Flag)
                {
                    //log.WriteLog("cpu start log", "Processor");
                    //log.WriteLog(Marshal.PtrToStringAnsi(GetCurrentProcessorNumberEx()), "Processor");
                    //_bIsRunMainCp2Flag = false;
                    //sm.BeginInvoke(null,null);
                    this.BeginInvoke(sm);
                }
                Thread.Sleep(_iMainSleepTime);
            }
        }
        private delegate void SetDelegageSPCDataUpload();
        private void RunMainCpSPCDataUpload()
        {
            int iCore = Environment.ProcessorCount;
            iCore--;
            iCore--;
            iCore--;
            iCore--;
            if (iCore <= 0)
            {
                iCore = 2;
            }
            SetThreadAffinityMask(GetCurrentThread(), new UIntPtr(SetCpuID(iCore)));

            //SetThreadAffinityMask(GetCurrentThread(), new IntPtr(1 << INT_TWO));

            //this.BeginInvoke(new SetDelegageRunMain(DoRunMain));
            SetDelegageRunMainTwo sm = new SetDelegageRunMainTwo(DoRunMainSPCDataUpload);
            while (_bIsKeepRunning)
            {
                if (_bIsRunMainCpSPCDataUploadFlag)
                {
                    //log.WriteLog("cpu start log", "Processor");
                    //log.WriteLog(Marshal.PtrToStringAnsi(GetCurrentProcessorNumberEx()), "Processor");
                    //_bIsRunMainCp2Flag = false;
                    //sm.BeginInvoke(null,null);
                    this.BeginInvoke(sm);
                }
                Thread.Sleep(_iMainSleepTime);
            }
        }
        private delegate void SetDelegageRunMainShowMessage();
        private void RunMainShowMessage()
        {
            int iCore = Environment.ProcessorCount;
            iCore--;
            iCore--;
            iCore--;
            iCore--;
            if (iCore <= 0)
            {
                iCore = 2;
            }
            SetThreadAffinityMask(GetCurrentThread(), new UIntPtr(SetCpuID(iCore)));
            //SetThreadAffinityMask(GetCurrentThread(), new IntPtr(1 << INT_TWO));
            //this.BeginInvoke(new SetDelegageRunMain(DoRunMain));
            SetDelegageRunMainShowMessage sm = new SetDelegageRunMainShowMessage(DoRunMainShowMessage);
            while (_bIsKeepRunning)
            {
                if (_bIsRunMainShowMessageFlag)
                {
                    //log.WriteLog("cpu start log", "Processor");
                    //log.WriteLog(Marshal.PtrToStringAnsi(GetCurrentProcessorNumberEx()), "Processor");
                    //_bIsRunMainCp2Flag = false;
                    //sm.BeginInvoke(null,null);
                    this.BeginInvoke(sm);
                }
                GetAppSetting();
                ReadAppsetingFrUI();
                
                
                Thread.Sleep(_iMainSleepTime);
            }
        }
        private void DoRunMain()
        {

                #region "Synchronous Data EXFO"
                if (blnSynchronousDataForEXFO && intSPCDataBaseType == 2)
                {
                    DateTime dtNow = DateTime.Now;
                    string strEXFOStartTime = "12:00";
                    if (this.txtEXFOStartTime != null)
                    {
                        strEXFOStartTime = this.txtEXFOStartTime.Text;
                    }
                    
                    if ((dtNow - dtEXFOLastRunTime).TotalMinutes >= 1 && dtNow.ToString("HH:mm") == strEXFOStartTime.Trim())
                    {
                        //GetAppSetting();

                        DateTime dtStartTime = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 00:00:00");
                        DateTime dtEndTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");

                        //this.timer2.Enabled = true;
                        //log.WriteLog("exfo线程启动！", "Main");
                        AppLogHelp.WriteLog(LogFileFormate.MES, "exfo线程启动！");
                        bu_joch.AutoStartEXFO(dtStartTime, dtEndTime);
                        dtEXFOLastRunTime = DateTime.Now;
                    }
                }
                #endregion

                #region "Save CSV FIle For Foxconn"
                if (blnSaveCSVFileForFoxconn && intSPCDataBaseType == 2)
                {
                    string strFoxconnStartTime = "12:00";
                    DateTime dtNow = DateTime.Now;
                    if (this.txtCSVRunTime != null)
                    {
                        strFoxconnStartTime = this.txtCSVRunTime.Text;
                    }
                    if (((dtNow - dtLastRunTimeForZZFox).TotalHours >= 1 && dtNow.ToString("HH:mm") == strFoxconnStartTime.Trim()))
                    {
                        //GetAppSetting();
                        DateTime dtStartTime = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 00:00:00");
                        DateTime dtEndTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
                        string strCSVFile = this.txtCSVFile.Text;
                        int intSectionTime = (int)this.nudTimeSpan.Value;

                        //this.timer2.Enabled = true;
                        //log.WriteLog("线程启动！", "Main");
                        AppLogHelp.WriteLog(LogFileFormate.MES, "ZZFoxconn线程启动！");
                        bu_peng.AutoStartFoxconn(dtStartTime, dtEndTime, strCSVFile, intSectionTime);
                        dtLastRunTimeForZZFox = DateTime.Now;
                    }
                }
                #endregion

                #region "Save FIle For JUFEI"
                if (blnSaveDataForJUFEI && intSPCDataBaseType == 2)
                {

                    DateTime dtNow = DateTime.Now;
                    string strJufeiStartTime = "12:00";
                    if (this.txtJufeiRunTime != null)
                    {
                        strJufeiStartTime = this.txtJufeiRunTime.Text;
                    }

                    if (((dtNow - dtLastRunTimeForJUFEI).TotalHours >= 1 && dtNow.ToString("HH:mm") == strJufeiStartTime.Trim()))
                    {
                       // GetAppSetting();
                        DateTime dtStartTime = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 00:00:00");
                        DateTime dtEndTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
                        string strCSVJufeiFile = this.savePathJufei.Text;

                        //
                        //this.timer2.Enabled = true;
                        //log.WriteLog("线程启动！", "Main");
                        AppLogHelp.WriteLog(LogFileFormate.MES, "Jufei线程启动！");
                        bu_peng.AutoStartJufei(dtStartTime, dtEndTime, strCSVJufeiFile);
                        dtLastRunTimeForJUFEI = DateTime.Now;
                    }
                }
                #endregion

                #region "CcaiHuang"
                if (bEnDataForCaiHuang && intSPCDataBaseType == 2)
                {
                    string strFoxconnStartTime = "12:00";
                    DateTime dtNow = DateTime.Now;
                    iniCaiHuangPath();
                    if (this.txtCSVRunTimeForCaiHuang != null)
                    {
                        strFoxconnStartTime = this.txtCSVRunTimeForCaiHuang.Text;
                    }
                    if ((dtNow - dtLastRunTimeForCaiHuang).TotalHours >= 1 && dtNow.ToString("HH:mm") == strFoxconnStartTime.Trim())
                    {
                        //GetAppSetting();
                        DateTime dtStartTime = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 00:00:00");
                        DateTime dtEndTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
                        string strCSVFile = this.txtCSVFileForCaiHuang.Text;
                        //int intSectionTime = (int)this.nudTimeSpan.Value;

                        //this.timer2.Enabled = true;
                        //log.WriteLog("线程启动！", "Main");
                        AppLogHelp.WriteLog(LogFileFormate.MES, "CcaiHuang线程启动！");
                        bu_peng.AutoStartCaiHuang(this.dtpCSVStartTimeForCaiHuang.Value, this.dtpCSVEndTimeForCaiHuang.Value, this.txtCSVFileForCaiHuang.Text);
                        //bu.AutoStartFoxconn(dtStartTime, dtEndTime, strCSVFile, intSectionTime, log);
                        dtLastRunTimeForCaiHuang = DateTime.Now;
                    }
                }
                #endregion

                #region " ZHAOCHISamsung双段合并 "
                if (bEnDataZHAOCHISamsung)
                {
                    if (!this.tabMain.TabPages.Contains(this.tpZHAOCHISamsung))
                    {
                        //this.tabMain.TabPages.Add(this.tpZHAOCHISamsung);
                    }
                    if (bOpenZHAOCHISamsung)
                    {
                        bOpenZHAOCHISamsung = false;
                        AppLogHelp.WriteLog(LogFileFormate.MES, "ZHAOCHISamsung thread start...");
                        bu_peng.AutoZHAOCHI_Samsung(ZHAOCHISamsung_TMPFILE_PATH, ZHAOCHISamsung_SAVEFILE_PATH);
                    }
                }
                else
                {
                    try
                    {
                        if (bu_peng._ZHAOCHI_SamsungThread != null
                            && (bu_peng._ZHAOCHI_SamsungThread.ThreadState == System.Threading.ThreadState.Running
                            | bu_peng._ZHAOCHI_SamsungThread.ThreadState == System.Threading.ThreadState.Background
                            | bu_peng._ZHAOCHI_SamsungThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin
                            ))
                        {
                            bu_peng.stopThreadZHAOCHISamsungThread();
                            if (this.tabMain.TabPages.Contains(this.tpZHAOCHISamsung))
                            {
                                //this.tabMain.TabPages.Remove(this.tpZHAOCHISamsung);
                            }
                            AppLogHelp.WriteLog(LogFileFormate.MES, "ZHAOCHI_Samsung线程关闭 ");
                        }
                    }
                    catch (Exception e)
                    {
                        bOpenZHAOCHISamsung = true;
                        //return;
                    }
                    finally
                    {
                        bOpenZHAOCHISamsung = true;
                    }
                    //bOpenZHAOCHISamsung = true;

                }
                #endregion

            
        }
        private void DoRunMainCp()
        {
            #region "weixin"

            if (bEnWeiXin_suzhou)
            {
                try
                {
                    if (bWeiXin)
                    {
                        bWeiXin = false;
                        bu_peng.autoStartWeiXin();
                    }
                }
                catch (Exception ex)
                {
                    //log.WriteErr(ex.Message, "WeiXin");
                    AppLogHelp.WriteError(LogFileFormate.MES, "WeiXin "+ex.Message);
                    bWeiXin = true;
                }
            }
            else
            {
                //this.tabMain.TabPages.Remove(this.WeinXin_SZ);
                if (_runnerWeiXin != null
                    && (_runnerWeiXin.threadProcess.ThreadState == System.Threading.ThreadState.Running
                    | _runnerWeiXin.threadProcess.ThreadState == System.Threading.ThreadState.Background
                    | _runnerWeiXin.threadProcess.ThreadState == System.Threading.ThreadState.WaitSleepJoin
                    | _runnerWeiXin.Running))
                {
                    bWeiXin = true;
                    _runnerWeiXin.Stop();
                }
            }

            #endregion

            #region "RenBao抓取文件"
            if (this.btnTouchReabBao.Visible && _bRenBaoFileIsOpen)
            {
                try
                {
                    _bRenBaoFileIsOpen = false;
                    bu_peng.StartRenBao_ToFile(this.tbTextPathRenBao.Text, this.tbTextPathRenBao2.Text);
                }
                catch (Exception ex)
                {
                    _bRenBaoFileIsOpen = true;
                    return;
                }
            }

            #endregion

            #region "大华"
            if (bEnZheJaingDaHua)
            {
                if (_bDaHua)
                {
                    _bDaHua = false;
                    
                    AppLogHelp.WriteLog(LogFileFormate.EquipStatus, "DaHua线程开启 " );
                    bu_peng.StartDaHua(this.tbDahuaFilePath.Text);

                }
            }
            else
            {
                if (_runnerDaHua != null
                        && (_runnerDaHua.threadProcess.ThreadState == System.Threading.ThreadState.Running
                        | _runnerDaHua.threadProcess.ThreadState == System.Threading.ThreadState.Background
                        | _runnerDaHua.threadProcess.ThreadState == System.Threading.ThreadState.WaitSleepJoin
                        | _runnerDaHua.Running))
                {
                    _bDaHua = true;
                    //log.WriteLog("线程关闭", "DaHua");
                    AppLogHelp.WriteLog(LogFileFormate.MES, "DaHua线程关闭 ");
                    _runnerDaHua.Stop();

                }
            }

            #endregion

            #region "南昌华勤"
            if (bEnNanChangHuaQin)
            {
                if (_bHuaQin)
                {
                    _bHuaQin = false;
                    AppLogHelp.WriteLog(LogFileFormate.MES, "HuaQin线程开启 ");
                    bu_peng.startHuaQinByNanChangRun( _configData);
                }
            }

            #endregion

            #region " 鲁帮通 "
            if (bEnDataLuBangTong && _bIsLuBangTongLogin)   //test
            {

                //if (!this.tabMain.TabPages.Contains(this.tpLuBangTong))
                //{
                //    this.tabMain.TabPages.Add(this.tpLuBangTong);
                //}
                try
                {
                    if (_bLuBangTongThread)
                    {
                        //test
                        _bLuBangTongThread = false;
                        bu_peng.StartLuBangTong(this.tbUSERNO.Text,
                        this.tbMO.Text,
                        this.tbITEMVALUE.Text,
                        this.tbMachineNo.Text,
                        this.tbOn.Text,
                        this.tbSf.Text,
                        this.clientId.Text,
                        this.clientSecret.Text, this.tbStatusURL.Text, this.tbErrorUrl.Text);
                    }
                    if (_bLuBangTongSnUpLoadSnThread)
                    {
                        _bLuBangTongSnUpLoadSnThread = false;
                    }
                    //鲁帮通生产数据
                    if (_bLuBangTongPrdThread)
                    {
                        _bLuBangTongPrdThread = false;
                        _strLuBangTongPrdPath = this.tbErrorUrl.Text.Replace("uploadErrorInfo", "uploadPrdLogInfo");
                        bu_peng.StartLuBangTongPRD(this.tbUSERNO.Text,
                        this.tbMO.Text,
                        this.tbITEMVALUE.Text,
                        this.tbMachineNo.Text,
                        this.tbOn.Text,
                        this.tbSf.Text,
                        this.clientId.Text,
                        this.clientSecret.Text, this.tbStatusURL.Text, _strLuBangTongPrdPath);
                    }
                }
                catch (Exception ex)
                {
                    //log.WriteErr(ex.Message, "LuBangTong");
                    AppLogHelp.WriteLog(LogFileFormate.MES, "LuBangTong线程开启 ");
                    _bLuBangTongThread = true;
                    _bLuBangTongPrdThread = true;
                }
            }
            else
            {
                //this.tabCont1.TabPages.Remove(this.tpLuBangTong);

                if (_runnerLuBangTong != null
                        && (_runnerLuBangTong.threadProcess.ThreadState == System.Threading.ThreadState.Running
                        | _runnerLuBangTong.threadProcess.ThreadState == System.Threading.ThreadState.Background
                        | _runnerLuBangTong.threadProcess.ThreadState == System.Threading.ThreadState.WaitSleepJoin
                        | _runnerLuBangTong.Running))
                {
                    _bLuBangTongThread = true;
                    _runnerLuBangTong.Stop();
                }

                if (_runnerLuBangTongPRD != null
                        && (_runnerLuBangTongPRD.threadProcess.ThreadState == System.Threading.ThreadState.Running
                        | _runnerLuBangTongPRD.threadProcess.ThreadState == System.Threading.ThreadState.Background
                        | _runnerLuBangTongPRD.threadProcess.ThreadState == System.Threading.ThreadState.WaitSleepJoin
                        | _runnerLuBangTongPRD.Running))
                {
                    _bLuBangTongPrdThread = true;
                    _runnerLuBangTongPRD.Stop();
                }
            }

            #endregion

            #region "设备状态"
            if (bEnEquipStatus)
            {
                try
                {
                    //if (!this.tabMain.TabPages.Contains(this.tbtbStatusSys))
                    //{
                    //    this.tabMain.TabPages.Add(this.tbtbStatusSys);
                    //}
                    if (bEquipStatusOPen)
                    {
                        bEquipStatusOPen = false;
                        //log.WriteLog("thread start...", "EquipStatus");
                        AppLogHelp.WriteLog(LogFileFormate.EquipStatus, "设备状态线程开启 ");
                        bu_peng.startStatusRun(strSaveEquipStatusPath);
                    }
                }
                catch (Exception ex)
                {
                    //log.WriteErr(ex.Message, "EquipStatus");
                    AppLogHelp.WriteError(LogFileFormate.EquipStatus, "设备状态线程异常 ");
                    bEquipStatusOPen = true;
                }
            }
            else
            {
                try
                {
                    if (bu_peng._StatusThread != null
                        && (bu_peng._StatusThread.ThreadState == System.Threading.ThreadState.Running
                        | bu_peng._StatusThread.ThreadState == System.Threading.ThreadState.Background
                        | bu_peng._StatusThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin
                        ))
                    {
                        bu_peng.stopThreadStatus();
                        //if (this.tabMain.TabPages.Contains(this.tbtbStatusSys))
                        //{
                        //    this.tabMain.TabPages.Remove(this.tbtbStatusSys);
                        //}
                        AppLogHelp.WriteLog(LogFileFormate.EquipStatus, "设备状态线程关闭 ");
                    }
                }
                catch (Exception e)
                {
                    bEquipStatusOPen = true;
                    //return;
                }
                finally
                {
                    bEquipStatusOPen = true;
                }
            }

            #endregion
            //_bIsRunMainCp2Flag = true;

            #region "Boe"
            if (bEnDataBoe)
            {
                try
                {
                    if (bEnDataBoeOPen)
                    {
                        bEnDataBoeOPen = false;
                        //log.WriteLog("thread start...", "Boe");
                        AppLogHelp.WriteLog(LogFileFormate.MES, "Boe thread start... ");
                        bu_peng.startBoeRun();
                    }
                }
                catch (Exception ex)
                {
                    //log.WriteErr(ex.Message, "Boe");
                    AppLogHelp.WriteError(LogFileFormate.MES, "Boe thread  " + ex.Message);
                    bEnDataBoeOPen = true;
                }
            }
            else
            {
                try
                {
                    if (bu_peng._BoeThread != null
                        && (bu_peng._BoeThread.ThreadState == System.Threading.ThreadState.Running
                        | bu_peng._BoeThread.ThreadState == System.Threading.ThreadState.Background
                        | bu_peng._BoeThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin
                        ))
                    {
                        bu_peng.stopThreadBoe();
                        AppLogHelp.WriteLog(LogFileFormate.MES, "Boe thread  close... ");
                    }
                }
                catch (Exception e)
                {
                    bEnDataBoeOPen = true;
                    //return;
                }
                finally
                {
                    bEnDataBoeOPen = true;
                }
            }

            #endregion

            #region " skyWorth "
            if (bEnDataSkyWorth)
            {
                try
                {
                    if (bEnDataSkyWorthOpen)
                    {
                        bEnDataSkyWorthOpen = false;
                        //log.WriteLog("thread start...", "Boe");
                        AppLogHelp.WriteLog(LogFileFormate.MES, "skyWorth thread start... ");
                        bu_peng.startSkyWorthRun();
                    }
                }
                catch (Exception ex)
                {
                    //log.WriteErr(ex.Message, "Boe");
                    AppLogHelp.WriteError(LogFileFormate.MES, "skyWorth thread  " + ex.Message);
                    bEnDataSkyWorthOpen = true;
                }
            }
            else
            {
                try
                {
                    if (bu_peng._SkyWorthThread != null
                        && (bu_peng._SkyWorthThread.ThreadState == System.Threading.ThreadState.Running
                        | bu_peng._SkyWorthThread.ThreadState == System.Threading.ThreadState.Background
                        | bu_peng._SkyWorthThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin
                        ))
                    {
                        bu_peng.stopThreadSkyWorth();
                        AppLogHelp.WriteLog(LogFileFormate.MES, "EnDataSkyWorth thread  close... ");
                    }
                }
                catch (Exception e)
                {
                    bEnDataSkyWorthOpen = true;
                    //return;
                }
                finally
                {
                    bEnDataSkyWorthOpen = true;
                }
            }
            //bEnDataSkyWorth

            #endregion

            //Thread.Sleep(100);
        }
        private void DoRunMainCpEYSPIToAOI()
        {
            #region  "三点照合"
            if (bEnDataForFovPcb)
            {
                if (bEnEYSPIToAOI)
                {
                    //if (!this.tabMain.TabPages.Contains(this.tpEYSPIToAOI))
                    //{
                    //    this.tabMain.TabPages.Add(this.tpEYSPIToAOI);
                    //}
                    //else
                    //{

                    //}
                    if (bAOI)
                    {
                        //log.WriteLog("进来-----", "bEnEYSPIToAOI线程");
                        bAOI = false;

                        //临时可关闭PCB FOV线程;
                        //if (this.cbBoxPcbFov.Checked)
                        //{
                        try
                        {
                            //this.timer2.Enabled = true;
                            //log.WriteLog("thread start", "EYSPIToAOI");
                            AppLogHelp.WriteLog(LogFileFormate.EYspiToAOI, "thread start..");
                            //this.tabCont1.TabPages.Remove(this.Fov_Pcb_Image);
                            int iLimitNum = Convert.ToInt32(this.tbFileNum.Text.Trim());
                            if (iLimitNum == 0)
                            {
                                iLimitNum = 50;
                            }
                            bu_peng.AutoStartForEYSPIToAOI(_configData, iLimitNum, _FovDirPath,_appSettingHandle);
                        }
                        catch (Exception exx)
                        {
                            bAOI = true;
                            return;
                        }
                    }
                }
                else
                {

                    try
                    {
                        while (_runnerFovThreadProcessFovEYSPIToAOI != null
                        && (_runnerFovThreadProcessFovEYSPIToAOI.threadProcess.ThreadState == System.Threading.ThreadState.Running
                        | _runnerFovThreadProcessFovEYSPIToAOI.threadProcess.ThreadState == System.Threading.ThreadState.Background
                        | _runnerFovThreadProcessFovEYSPIToAOI.threadProcess.ThreadState == System.Threading.ThreadState.WaitSleepJoin
                        | _runnerFovThreadProcessFovEYSPIToAOI.Running))
                        {
                            _runnerFovThreadProcessFovEYSPIToAOI.Stop();
                            //log.WriteLog("stop the thread success", "EYSPIToAOI线程");
                            AppLogHelp.WriteLog(LogFileFormate.EYspiToAOI, "stop the thread success");
                            bAOI = true;
                            //this.tabMain.TabPages.Remove(this.tpEYSPIToAOI);
                        }
                    }
                    catch (Exception exx)
                    {
                        bAOI = true;
                        return;
                    }
                    finally
                    {
                        bAOI = true;
                    }
                    //ClearMemory();
                }
            }


            #endregion
        }
        private void DoRunMainFOVPCB()
        {
            #region "Fov_Pcb"

            //GetAppSetting();
            //log.WriteLog("我","我是PCBFOV");
            if (bEnDataForFovPcb)
            {
                if (bEnDataForPCB || bEnDataForFov)
                {
                    if (!this.tabMain.TabPages.Contains(this.Fov_Pcb_Image))
                    {
                        this.tabMain.TabPages.Add(this.Fov_Pcb_Image);
                    }
                    if (bFov && !bEnClosePCBFOV)
                    {
                            bFov = false;
                            //bu_peng.ReadFovImages(log, _configData);
                            try
                            {
                                AppLogHelp.WriteLog(LogFileFormate.FOVPCB, "thread start..");
                                bu_peng.AutoStartFov( _configData, _FovDirPath,_appSettingHandle);
                            }
                            catch (Exception exx)
                            {
                                //MessageBox.Show(exx.Message);
                                bFov = true;
                                return;
                            }
                            finally
                            {
                                //bFov = true;
                            }
                       // }
                    }
                    
                }
                else
                {
                    try
                    {
                        while (_runnerFov != null
                        && (_runnerFov.threadProcess.ThreadState == System.Threading.ThreadState.Running
                        | _runnerFov.threadProcess.ThreadState == System.Threading.ThreadState.Background
                        | _runnerFov.threadProcess.ThreadState == System.Threading.ThreadState.WaitSleepJoin
                        | _runnerFov.Running))
                        {
                            _runnerFov.Stop();
                            bFov = true;
                            //log.WriteLog("stop the thread", "FOV/PCB线程");
                            AppLogHelp.WriteLog(LogFileFormate.FOVPCB, "stop the thread");
                            this.tabMain.TabPages.Remove(this.Fov_Pcb_Image);
                            if (AutoAPP.bu_Peng.arrAPP_SaveOrLoadImgDataInFo != null && AutoAPP.bu_Peng.arrAPP_SaveOrLoadImgDataInFo.Count > 0)
                            {
                                AutoAPP.bu_Peng.arrAPP_SaveOrLoadImgDataInFo.Clear();
                            }
                        }
                    }
                    catch (Exception exx)
                    {
                        bFov = true;
                        return;
                    }
                    finally
                    {
                        bFov = true;
                    }
                    ClearMemory();

                }
            }
            else
            {

                try
                {
                    while (_runnerFov != null
                    && (_runnerFov.threadProcess.ThreadState == System.Threading.ThreadState.Running
                    | _runnerFov.threadProcess.ThreadState == System.Threading.ThreadState.Background
                    | _runnerFov.threadProcess.ThreadState == System.Threading.ThreadState.WaitSleepJoin
                    | _runnerFov.Running))
                    {
                        _runnerFov.Stop();
                        //log.WriteLog("关闭","FOV/PCB线程");
                        AppLogHelp.WriteLog(LogFileFormate.FOVPCB, "stop the thread");
                        bFov = true;
                        this.tabMain.TabPages.Remove(this.Fov_Pcb_Image);
                        if (AutoAPP.bu_Peng.arrAPP_SaveOrLoadImgDataInFo != null && AutoAPP.bu_Peng.arrAPP_SaveOrLoadImgDataInFo.Count > 0)
                        {
                            AutoAPP.bu_Peng.arrAPP_SaveOrLoadImgDataInFo.Clear();
                        }
                    }
                }
                catch (Exception exx)
                {
                    bFov = true;
                    return;
                }
                ClearMemory();
            }


            #endregion
        }
        private void DoRunMainOnLineFOVPCB()
        {
            if (bEnDataForOnLineFovPcb)
            {
                // bEnDataForOnLineFovPcb = false;
                if (bIsOpenOnlinePCBFOV)
                {
                    bIsOpenOnlinePCBFOV = false;
                    try
                    {
                        AutoStartOnlinePCbFov(PCBFOV_ONLINE_THREAD_COUNT);
                    }
                    catch (Exception exx)
                    {
                        //bEnDataForOnLineFovPcb = true;
                        //continue;
                        MessageBox.Show("ONLINE PCB FOV 线程异常! 请关闭重开按钮!");
                        AppLogHelp.WriteError(LogFileFormate.FOVPCB, "DoRunMainOnLineFOVPCB  ONLINE PCB FOV 线程异常！" + exx.Message);
                    }
                }

            }
            else
            {
                StopOnlinePCBFOVThread();
                if (this.btnOnLinePCBFOV.Text == "关闭联网模式")
                {
                    this.btnOnLinePCBFOV.Text = "开启联网模式";
                }
            }

        }
        private void DoRunMainDelete()
        {
            //DateTime dtLastRunTime = dtNow.AddMinutes(-10);
            #region "Delete Data"
            if (blnDeleteData && intSPCDataBaseType == 2)
            {
                //dtDeleteDataLastTime = Convert.ToDateTime("2011-11-11 00:00:00");
                string strStartTime = "00";
                
                //GetAppSetting();
                DateTime dtNow = DateTime.Now;
                blnDeleteDataEveryHour = rdbHour.Checked;
                blnImmediatelyDelete = chkImmediatelyDelete.Checked;
                blnDeleteDay = rdbDay.Checked;
                iPCBLimit = Convert.ToInt32(nudPCBLimit.Value);
                bool blnSaveData = this.chkBackUP.Checked;
                string strSaveFloder = this.txtSaveFolder.Text;
                //add by peng 20190704 
                //if (!this.tabMain.TabPages.Contains(this.tpgDeleteData))
                //{
                //    this.tabMain.TabPages.Add(this.tpgDeleteData);
                //}

                if (this.cboDeleteRunHour != null)
                {
                    strStartTime = this.cboDeleteRunHour.Text;
                }

                if (!blnImmediatelyDelete && iDeleteCount == 0)
                {
                    dtDeleteDataLastTime = DateTime.Now;
                }

                if (blnDeleteDataEveryHour)
                {
                    //GetAppSetting();
                    if ((dtNow - dtDeleteDataLastTime).TotalHours > 1)
                    {
                        dtDeleteDataLastTime = DateTime.Now;
                        //如果此时自动删除线程已控制关闭 ,
                        if (_bIsNewThreadWithUiAutoDelete)
                        {
                            _bIsNewThreadWithUiAutoDelete = false;


                            if (intDeleteDays > 0)
                                intDeleteDays = -1 * intDeleteDays;
                            //增加一个分钟时间
                            DateTime dtEndTime = Convert.ToDateTime(DateTime.Now.AddDays(intDeleteDays).ToString("yyyy-MM-dd") + " 00:00:00");
                            //this.timer2.Enabled = true;
                            //log.WriteLog("线程启动！", "Main");
                            //log.WriteLog(dtEndTime.ToString("yyyyMMdd HH:mm:ss"), "delete max time");
                            AppLogHelp.WriteLog(LogFileFormate.Delete, "thread start..");
                            try
                            {
                                bu_joch.AutoStart(dtEndTime, blnSaveData, strSaveFloder, blnDeleteDataEveryHour, iPCBLimit);

                                while (_runnerDeleteProcess != null
                                       && (_runnerDeleteProcess.Running | _runnerDeleteProcess.threadProcess.ThreadState == System.Threading.ThreadState.Running
                                       | _runnerDeleteProcess.threadProcess.ThreadState == System.Threading.ThreadState.Background
                                       | _runnerDeleteProcess.threadProcess.ThreadState == System.Threading.ThreadState.WaitSleepJoin
                                       ))
                                {
                                    Thread.Sleep(300);
                                    _runnerDeleteProcess.Stop();
                                    //log.WriteLog("stop the thread", "runnerDeleteProcess");
                                    AppLogHelp.WriteLog(LogFileFormate.Delete, "stop the thread..");
                                }
                            }
                            catch (Exception exxx)
                            {
                                _bIsNewThreadWithUiAutoDelete = true;
                                dtDeleteDataLastTime = DateTime.Now;
                                return;
                            }
                            finally
                            {

                            }
                            //DoDeleteFunc(dtStartTimeForDetele, dtEndTime, Properties.Settings.Default.MySQLConnect, blnSaveData, strSaveFloder, blnDeleteDataEveryHour, iPCBLimit);
                            iDeleteCount++;
                            dtDeleteDataLastTime = DateTime.Now;
                            dtLastRunTime = DateTime.Now;
                            //finish
                            Thread.Sleep(_iMainSleepTime);
                            _bIsNewThreadWithUiAutoDelete = true;
                        }
                    }
                }
                else
                {

                    if ((dtNow - dtLastRunTime).TotalHours >= 12 && dtNow.ToString("HH") == strStartTime.Trim())
                    {
                        //如果此时自动删除线程已控制关闭 ,
                        if (_bIsNewThreadWithUiAutoDelete)
                        {
                            _bIsNewThreadWithUiAutoDelete = false;
                            dtLastRunTime = DateTime.Now;
                            ////关闭线程
                            //if (_runnerDeleteProcess != null)
                            //{
                            //    _runnerDeleteProcess.Stop();
                            //}
                            //finish
                            Thread.Sleep(_iMainSleepTime);

                            //GetAppSetting();

                            if (intDeleteDays > 0)
                                intDeleteDays = -1 * intDeleteDays;

                            DateTime dtEndTime = Convert.ToDateTime(DateTime.Now.AddDays(intDeleteDays).ToString("yyyy-MM-dd") + " 00:00:00");

                            //this.timer2.Enabled = true;
                            //log.WriteLog("线程启动！", "Main");
                            AppLogHelp.WriteLog(LogFileFormate.Delete, "线程启动..");
                            try
                            {
                                bu_joch.AutoStart(dtEndTime, blnSaveData, strSaveFloder, blnDeleteDataEveryHour, iPCBLimit);

                                while (_runnerDeleteProcess != null
                                       && (_runnerDeleteProcess.Running | _runnerDeleteProcess.threadProcess.ThreadState == System.Threading.ThreadState.Running
                                       | _runnerDeleteProcess.threadProcess.ThreadState == System.Threading.ThreadState.Background
                                       | _runnerDeleteProcess.threadProcess.ThreadState == System.Threading.ThreadState.WaitSleepJoin
                                       ))
                                {
                                    Thread.Sleep(300);
                                    _runnerDeleteProcess.Stop();
                                    //log.WriteLog("关闭前段未执行完毕线程", "runnerDeleteProcess");
                                    AppLogHelp.WriteLog(LogFileFormate.Delete, "关闭前段未执行完毕线程..");
                                }
                            }
                            catch (Exception exxx)
                            {
                                _bIsNewThreadWithUiAutoDelete = true;
                                dtLastRunTime = DateTime.Now;
                                return;
                            }
                            finally
                            {
                                //while (_runnerDeleteProcess != null
                                //    | _runnerDeleteProcess.Running | _runnerDeleteProcess.threadProcess.ThreadState == ThreadState.Running
                                //    | _runnerDeleteProcess.threadProcess.ThreadState == System.Threading.ThreadState.Background)
                                //{
                                //    Thread.Sleep(300);
                                //    _runnerDeleteProcess.threadProcess.Abort();
                                //    log.WriteLog("关闭前段未执行完毕线程", "runnerDeleteProcess");
                                //}
                            }
                            //DoDeleteFunc(dtStartTimeForDetele, dtEndTime, Properties.Settings.Default.MySQLConnect, blnSaveData, strSaveFloder, blnDeleteDataEveryHour, iPCBLimit);
                            iDeleteCount++;
                            dtDeleteDataLastTime = DateTime.Now;
                            dtLastRunTime = DateTime.Now;
                            //_bIsNewThreadWithUiAutoDelete = true;
                        }
                    }
                }
            }
            else
            {
                //if (!this.tabMain.TabPages.Contains(this.tpgDeleteData))
                //{

                //}
                while (_runnerDeleteProcess != null
                                       && (_runnerDeleteProcess.Running | _runnerDeleteProcess.threadProcess.ThreadState == System.Threading.ThreadState.Running
                                       | _runnerDeleteProcess.threadProcess.ThreadState == System.Threading.ThreadState.Background
                                       | _runnerDeleteProcess.threadProcess.ThreadState == System.Threading.ThreadState.WaitSleepJoin
                                       ))
                {
                    Thread.Sleep(300);
                    _runnerDeleteProcess.Stop();
                    //log.WriteLog("stop the thread", "runnerDeleteProcess");
                    AppLogHelp.WriteLog(LogFileFormate.Delete, "stop the thread..");
                }
                //if (this.tabMain.TabPages.Contains(this.tpgDeleteData))
                //{
                //    this.tabMain.TabPages.Remove(this.tpgDeleteData);
                //}
            }
            #endregion
        }
        private void DoRunMainSPCDataUpload()
        {
            #region "SPCData"

            if (bEnDataSpcData)
            {
                try
                {
                    if (bSpcOpen)
                    {
                        //if (!this.tabMain.TabPages.Contains(this.tbSPCUpload))
                        //{
                        //    this.tabMain.TabPages.Add(this.tbSPCUpload);
                        //}
                        bSpcOpen = false;
                        //log.WriteLog("thread start...", "SPCData");
                        AppLogHelp.WriteLog(LogFileFormate.SPCDataUpload, "thread start....");
                        bu_peng.AutoStartSPC();
                    }
                }
                catch (Exception ex)
                {
                    //log.WriteErr(ex.Message, "SPCData");
                    AppLogHelp.WriteError(LogFileFormate.SPCDataUpload, ex.Message);
                    bEnDataBoeOPen = true;
                }
            }
            else
            {
                try
                {
                    if (bu_peng._SPCDataThread != null
                        && (bu_peng._SPCDataThread.ThreadState == System.Threading.ThreadState.Running
                        | bu_peng._SPCDataThread.ThreadState == System.Threading.ThreadState.Background
                        | bu_peng._SPCDataThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin
                        ))
                    {
                        bu_peng.stopThreadSpc();
                        //this.tabMain.TabPages.Remove(this.tbSPCUpload);
                        AppLogHelp.WriteLog(LogFileFormate.SPCDataUpload, "stop the thread..");
                    }
                }
                catch (Exception e)
                {
                    bSpcOpen = true;
                    return;
                }
                finally
                {
                    bSpcOpen = true;
                }
            }
            #endregion
        }
        private void DoRunMainShowMessage()
        {
            
            //LoadDataSet();
            showFroms();
        }
        private void ReadAppsetingFrUI()
        {
            string path = Path.Combine( @"D:\EYSPI\Bin\AutoAPPConfig\");   //20190826 moditied 
            //path = _IP + path;
            //path = path.Replace("D:\\", "");
            DirectoryInfo info = new DirectoryInfo(path);
            FileInfo newestFile = info.GetFiles("*.bin").OrderBy(n => n.LastWriteTime).Last();
            if (newestFile.Exists)
            {
                System.Threading.Thread.Sleep(200);
                if (_baseFuc.IsFileInUse(newestFile.FullName)) //在使用
                {
                    System.Threading.Thread.Sleep(100);
                };
                _appSettingHandle.Read(newestFile.FullName, _configData);
                //log.WriteLog("INSPECT.BIN:"+_strRealConfigPath, "PL_HXDZ");

            }
            else
            {
                return;
            }
        }
        private void ReadOnlineAppsetingFrUI(string Aip,ref AppLayerLib.AppSettingHandler AappSettingHandle,ref InspectConfig.ConfigData configData)
        {
            string path = Path.Combine(@"D:\EYSPI\Bin\AutoAPPConfig\");   //20190826 moditied 
            Aip = "\\\\" + Aip + "\\";
            //config
            string strRealConfigPath = Path.Combine(strConfigPath, "Inspect.spk");  // 20190826 moditied
            strRealConfigPath = Aip + _strRealConfigPath;
            strRealConfigPath = strRealConfigPath.Replace("D:\\", "").Replace("D://", "");
            string strConfigNamePath = Aip + _strConfigNamePath;
            strConfigNamePath = strConfigNamePath.Replace("D:\\", "").Replace("D://", "");
            File.Copy(strConfigNamePath, strRealConfigPath, true);
            System.Threading.Thread.Sleep(200);

            InspectConfig.ConfigHandler configReader = new InspectConfig.ConfigHandler();
            //configData = new InspectConfig.ConfigData();
            configReader.ReadConfig(strRealConfigPath);
            configData = configReader._configData;
            
            path = Aip + path;
            path = path.Replace("D:\\", "");
            DirectoryInfo info = new DirectoryInfo(path);
            FileInfo newestFile = info.GetFiles("*.bin").OrderBy(n => n.LastWriteTime).Last();
            if (newestFile.Exists)
            {
                System.Threading.Thread.Sleep(200);
                if (_baseFuc.IsFileInUse(newestFile.FullName)) //在使用
                {
                    System.Threading.Thread.Sleep(100);
                };
                AappSettingHandle.Read(newestFile.FullName, configData);
                //log.WriteLog("INSPECT.BIN:"+_strRealConfigPath, "PL_HXDZ");

            }
            else
            {
                return;
            }
        }
        private ulong SetCpuID(int id)
        {
            ulong cpuid = 0;
            if (id < 0 || id >= Environment.ProcessorCount)
            {
                id = 0;
            }
            cpuid |= 1UL << id;
            return cpuid;
        }

        private void DoStopWork()
        {
            _bIsKeepRunning = false;
            if (_threadMain != null && _threadMain.IsAlive) _threadMain.Abort();

            //
            DoAbortThread(_threadMainCpDelete);
            DoAbortThread(_threadMainCpEYSPIToAOI);
            DoAbortThread(_threadMainCpFOVPCB);
            DoAbortThread(_threadMainCpSPCDataUpload);
            DoAbortThread(_threadMainCpTwo);
            DoAbortThread(_threadMainShowMessage);
        }

        private void DoAbortThread(Thread Athread)
        {
            if (Athread != null && Athread.IsAlive) Athread.Abort();
        }


        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //EYSPIToAOI.Main m = new EYSPIToAOI.Main();
            //m.Show();
            UI_Setting settingForm = new UI_Setting();
            // settingForm._dict = _dict;
            settingForm.TopMost = true;
            settingForm.ShowDialog();
            if (settingForm._applySetting == true)
            {
                EYSPIToAOIHelp HELP = new EYSPIToAOIHelp();
                HELP.ReadAppSetting();
            }
        }

        private void intUIEYSPIToAOIBtn_Click(object sender, EventArgs e)
        {
            iniEYSPIToAOIFiles();

        }

        private void tsmpEYSPIToAOI_Click(object sender, EventArgs e)
        {
            iniEYSPIToAOIFiles();
        }

        private void iniEYSPIToAOIFiles()
        {
            //删除D:\EYSPI\DataExport\ExToFovImage下data文件

            string frUIDataDirPath = @"D:\EYSPI\DataExport\ExToFovImage";
            string frAOIDataPath = @"D:\EYSPI\DataExport\LinkFrAOI";
            string frAOIDataPathCopy = @"D:\EYSPI\DataExport\ExToEyspiToAOI";
            string frAOIDataProcessPath = @"D:\EYSPI\DataExport\LinkFrAOI\Processed";
            string toAOIDataPath = @"D:\EYSPI\DataExport\LinkToAOI";
            string frFOVImage = @"D:\EYSPI\DataExport\LinkToAOI\FOVImage";
            string EXFOVPath = _FovDirPath;
            string EYSPIPath = EXFOVPath.Replace("ExToFovImage", "ExToEyspiToAOI");
            if (Directory.Exists(EXFOVPath))
            {
                string[] arrDir = Directory.GetDirectories(EXFOVPath);
                if (arrDir != null && arrDir.Length > 0)
                {
                    foreach (string dirPath in arrDir)
                    {
                        if (Directory.Exists(dirPath))
                        {
                            Directory.Delete(dirPath, true);
                        }
                    }
                }
            }
            if (Directory.Exists(EYSPIPath))
            {
                string[] arrDir = Directory.GetDirectories(EYSPIPath);
                if (arrDir != null && arrDir.Length > 0)
                {
                    foreach (string dirPath in arrDir)
                    {
                        if (Directory.Exists(dirPath))
                        {
                            Directory.Delete(dirPath, true);
                        }
                    }
                }
            }
            if (Directory.Exists(frUIDataDirPath))
            {
                string[] arrDir = Directory.GetDirectories(frUIDataDirPath);
                if (arrDir != null && arrDir.Length > 0)
                {
                    foreach (string dirPath in arrDir)
                    {
                        if (Directory.Exists(dirPath))
                        {
                            Directory.Delete(dirPath, true);
                        }
                    }
                }
                if (Directory.Exists(frAOIDataPath))
                {
                    string[] strFiles = Directory.GetFiles(frAOIDataPath);
                    if (strFiles != null && strFiles.Length > 0)
                    {
                        foreach (string str in strFiles)
                        {
                            if (File.Exists(str))
                            {
                                File.Delete(str);
                            }
                        }
                    }
                }

            }
            if (Directory.Exists(frAOIDataPathCopy))
            {
                string[] arrDir = Directory.GetDirectories(frAOIDataPathCopy);
                if (arrDir != null && arrDir.Length > 0)
                {
                    foreach (string dirPath in arrDir)
                    {
                        if (Directory.Exists(dirPath))
                        {
                            Directory.Delete(dirPath, true);
                        }
                    }
                }
            }
            if (Directory.Exists(frAOIDataProcessPath))
            {
                string[] strFiles = Directory.GetFiles(frAOIDataProcessPath);
                if (strFiles != null && strFiles.Length > 0)
                {
                    foreach (string str in strFiles)
                    {
                        if (File.Exists(str))
                        {
                            File.Delete(str);
                        }
                    }
                }
            }
            if (Directory.Exists(toAOIDataPath))
            {
                string[] strFiles = Directory.GetFiles(toAOIDataPath);
                if (strFiles != null && strFiles.Length > 0)
                {
                    foreach (string str in strFiles)
                    {
                        if (File.Exists(str))
                        {
                            File.Delete(str);
                        }
                    }
                }
            }

            if (Directory.Exists(frFOVImage))
            {
                Directory.Delete(frFOVImage,true);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            MessageBox.Show("初始化成功...");
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            //log.WriteErr("autoAPP 已关闭", "autoAPP");
            AppLogHelp.WriteLog(LogFileFormate.AppMain, "autoAPP 已关闭..");
        }

        private void btnEYSPITOAOI_Click(object sender, EventArgs e)
        {
            UI_Setting settingForm = new UI_Setting();
            // settingForm._dict = _dict;
            settingForm.TopMost = true;
            settingForm.ShowDialog();
            if (settingForm._applySetting == true)
            {
                EYSPIToAOIHelp HELP = new EYSPIToAOIHelp();
                HELP.ReadAppSetting();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            iniEYSPIToAOIFiles();
        }

        private void btnSerlizeFOVPCB_Click(object sender, EventArgs e)
        {
            iniEYSPIToAOIFiles();
        }

        private void btnSkyWorth_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txSaveSkyWorthPath.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnSaveSkyWorthPrams_Click(object sender, EventArgs e)
        {

            //ini save
            string strAutoAPPIniFile = Path.Combine(strConfigIniPath, _strIniAutoAPPConfigFileName);
            INIFileHelper.WriteIniData("SkyWorth", "tbHeightU", this.tbHeightU.Text, strAutoAPPIniFile);
            INIFileHelper.WriteIniData("SkyWorth", "tbHeightL", this.tbHeightL.Text, strAutoAPPIniFile);
            INIFileHelper.WriteIniData("SkyWorth", "tbAreaU", this.tbAreaU.Text, strAutoAPPIniFile);
            INIFileHelper.WriteIniData("SkyWorth", "tbAreaL", this.tbAreaL.Text, strAutoAPPIniFile);
            INIFileHelper.WriteIniData("SkyWorth", "tbVolU", this.tbVolU.Text, strAutoAPPIniFile);
            INIFileHelper.WriteIniData("SkyWorth", "tbVolL", this.tbVolL.Text, strAutoAPPIniFile);
            INIFileHelper.WriteIniData("SkyWorth", "tbShiftXU", this.tbShiftXU.Text, strAutoAPPIniFile);
            INIFileHelper.WriteIniData("SkyWorth", "tbShiftXL", this.tbShiftXL.Text, strAutoAPPIniFile);
            INIFileHelper.WriteIniData("SkyWorth", "tbShiftYU", this.tbShiftYU.Text, strAutoAPPIniFile);
            INIFileHelper.WriteIniData("SkyWorth", "tbShiftYL", this.tbShiftYL.Text, strAutoAPPIniFile);
            INIFileHelper.WriteIniData("SkyWorth", "txSaveSkyWorthPath", this.txSaveSkyWorthPath.Text, strAutoAPPIniFile);

            MessageBox.Show("保存成功..");
        }
        private void ReadSkyWorthIniPrams()
        {
            string strAutoAPPIniFile = Path.Combine(strConfigIniPath, _strIniAutoAPPConfigFileName);
            if (File.Exists(strAutoAPPIniFile))
            {
                Thread.Sleep(100);
                this.tbHeightU.Text = INIFileHelper.ReadIniData("SkyWorth", "tbHeightU", string.Empty, strAutoAPPIniFile) == "" ? "0" : INIFileHelper.ReadIniData("SkyWorth", "tbHeightU", string.Empty, strAutoAPPIniFile);
                this.tbHeightL.Text = INIFileHelper.ReadIniData("SkyWorth", "tbHeightL", string.Empty, strAutoAPPIniFile)== "" ? "0" :INIFileHelper.ReadIniData("SkyWorth", "tbHeightL", string.Empty, strAutoAPPIniFile);
                this.tbAreaU.Text = INIFileHelper.ReadIniData("SkyWorth", "tbAreaU", string.Empty, strAutoAPPIniFile)== "" ? "0" :INIFileHelper.ReadIniData("SkyWorth", "tbAreaU", string.Empty, strAutoAPPIniFile);
                this.tbAreaL.Text = INIFileHelper.ReadIniData("SkyWorth", "tbAreaL", string.Empty, strAutoAPPIniFile)== "" ? "0" :INIFileHelper.ReadIniData("SkyWorth", "tbAreaL", string.Empty, strAutoAPPIniFile);
                this.tbVolU.Text = INIFileHelper.ReadIniData("SkyWorth", "tbVolU", string.Empty, strAutoAPPIniFile)== "" ? "0" :INIFileHelper.ReadIniData("SkyWorth", "tbVolU", string.Empty, strAutoAPPIniFile);
                this.tbVolL.Text = INIFileHelper.ReadIniData("SkyWorth", "tbVolL", string.Empty, strAutoAPPIniFile)== "" ? "0" :INIFileHelper.ReadIniData("SkyWorth", "tbVolL", string.Empty, strAutoAPPIniFile);
                this.tbShiftXU.Text = INIFileHelper.ReadIniData("SkyWorth", "tbShiftXU", string.Empty, strAutoAPPIniFile)== "" ? "0" :INIFileHelper.ReadIniData("SkyWorth", "tbShiftXU", string.Empty, strAutoAPPIniFile);
                this.tbShiftXL.Text = INIFileHelper.ReadIniData("SkyWorth", "tbShiftXL", string.Empty, strAutoAPPIniFile)== "" ? "0" :INIFileHelper.ReadIniData("SkyWorth", "tbShiftXL", string.Empty, strAutoAPPIniFile);
                this.tbShiftYU.Text = INIFileHelper.ReadIniData("SkyWorth", "tbShiftYU", string.Empty, strAutoAPPIniFile)== "" ? "0" :INIFileHelper.ReadIniData("SkyWorth", "tbShiftYU", string.Empty, strAutoAPPIniFile);
                this.tbShiftYL.Text = INIFileHelper.ReadIniData("SkyWorth", "tbShiftYL", string.Empty, strAutoAPPIniFile) == "" ? "0" : INIFileHelper.ReadIniData("SkyWorth", "tbShiftYL", string.Empty, strAutoAPPIniFile);
                this.txSaveSkyWorthPath.Text = INIFileHelper.ReadIniData("SkyWorth", "txSaveSkyWorthPath", string.Empty, strAutoAPPIniFile)==""? @"D:\EYSPI\Bin\SPILogs\Peng":INIFileHelper.ReadIniData("SkyWorth", "txSaveSkyWorthPath", string.Empty, strAutoAPPIniFile);
            }
        }





        #region "联网PCB"

        private string[] RS_ONLINE_PCBFOV_CONFIG_PATHS = null;
        private string[] RS_ONLINE_PCBFOV_IPS = null;
        private bool bIsOpenOnlinePCBFOV = true;
        private static int PCBFOV_ONLINE_THREAD_COUNT = 16;
        private Thread[] _pcbFovAllThreads = new System.Threading.Thread[PCBFOV_ONLINE_THREAD_COUNT];
        private PCBFOVConfig[] _PCBFOVConfig = new PCBFOVConfig[PCBFOV_ONLINE_THREAD_COUNT];

        
        //private AutoAPP.Basefunction _baseFuc = new Basefunction();

        private void AutoStartOnlinePCbFov(int iThreadCount)
        {

            for (int i = 0; i < iThreadCount; i++)
            {
                _pcbFovAllThreads[i] = new System.Threading.Thread(new ParameterizedThreadStart(ReadFovImagesOnline));
                _pcbFovAllThreads[i].IsBackground = true;
                _pcbFovAllThreads[i].Start(_PCBFOVConfig[i]);
            }

        }
        private void StopOnlinePCBFOVThread()
        {
            try
            {
                if (_pcbFovAllThreads != null && _pcbFovAllThreads.Length > 0)
                {
                    for (int i = 0; i < _pcbFovAllThreads.Length; i++)
                    {

                        while (_pcbFovAllThreads[i] != null
                        && (_pcbFovAllThreads[i].ThreadState == System.Threading.ThreadState.Running
                        | _pcbFovAllThreads[i].ThreadState == System.Threading.ThreadState.Background
                        | _pcbFovAllThreads[i].ThreadState == System.Threading.ThreadState.WaitSleepJoin
                        ))
                        {
                            Thread.Sleep(100);
                            _pcbFovAllThreads[i].Abort();
                            AppLogHelp.WriteLog(LogFileFormate.FOVPCB,i+ "   stop the thread success");
                            //this.tabMain.TabPages.Remove(this.Fov_Pcb_Image);
                        }

                    }

                }
            }
            catch (Exception exx)
            {
                //bFov = true;
                AppLogHelp.WriteError(LogFileFormate.FOVPCB, exx.Message + "   stop the thread Exception");
                return;
            }

        }
        private void ReadFovImagesOnline(object objPCBFOVConfig)
        {
            PCBFOVConfig pcbFovConfig = (PCBFOVConfig)objPCBFOVConfig;
            InspectConfig.ConfigData Aconfig = pcbFovConfig.Aconfig;
            string ExToFovImagePath = pcbFovConfig.ExToFovImagePath;
            AppLayerLib.AppSettingHandler appSettingHandle = pcbFovConfig.AappSettingHandle;
            string ip = pcbFovConfig.IP;
            ip = "\\\\" + ip + "\\";
            string lineName = pcbFovConfig.LineName;
            AppLogHelp.WriteLog(LogFileFormate.FOVPCB, "ip=>"+ip+ " pcvfovthread start...");
            string[] arrStrFovFileLines = null;
            string[] arrFovFiles = null; string strFirstPath = "", FovDirPath = string.Empty,
                strDataExportSaveFovImagePath = string.Empty;
            int interval = 0;
            bool bEnDataFov = false;
            string[] arrStrTxtPCBPath = null;
             SavePCBImages _savePCB = new SavePCBImages();
            SaveFovImages _saveFOV = new SaveFovImages();
            ImgCSCoreIM.ST_SaveOrLoadImgData _stImgData = new ImgCSCoreIM.ST_SaveOrLoadImgData();

            Byte[] _arrByteFov = { 0 };
            string _strFovDateDirPath = string.Empty;
            //ImgCSCoreIM.ST_SaveOrLoadImgData[] _ST_SaveOrLoadImgData;
        //private ImgCSCoreIM.ST_SaveOrLoadImgData _saveImage;
            string _strPerWidthHeight = string.Empty;
            bool _bNeedZoom = true;
            EYSPIToAOIHelp helpAOI = new EYSPIToAOIHelp();
            List<float> arrPosXmm = new List<float>(), arrPosYmm = new List<float>();
            List<int> arrIFovID = new List<int>();
            List<APP_SaveOrLoadImgDataInFo> arrAPP_SaveOrLoadImgDataInFo = new List<APP_SaveOrLoadImgDataInFo>();
            while (true)
            {
                if (bEnDataForOnLineFovPcb == false)
                {
                    arrAPP_SaveOrLoadImgDataInFo.Clear();
                    break;
                }
                try
                {
                    //UI给出fov的目录路径;
                    FovDirPath = ExToFovImagePath;
                    interval = appSettingHandle._appSettingData.stDataExpVT.IntervalSecond;
                    //int iMaxParallelCount = 2;
                    bEnDataFov = appSettingHandle._appSettingData.stDataExpVT.bUseAutoAppToGenImage;// 开关Fov保存图片
                    strDataExportSaveFovImagePath = string.Empty;
                    //ParallelOptions parallelOpt = new ParallelOptions();
                    //parallelOpt.MaxDegreeOfParallelism = iMaxParallelCount;
                    //if (Directory.GetDirectories(FovDirPath) != null && Directory.GetDirectories(FovDirPath).Length > 0)
                    //bEnDataFov = false;

                    if (bEnDataFov)
                    {
                        //if (bEnDataFov)
                        if (Directory.Exists(FovDirPath))
                        {
                            if (Directory.GetDirectories(FovDirPath) != null && Directory.GetDirectories(FovDirPath).Length > 0)
                            {
                                System.Threading.Thread.Sleep(300);
                                strFirstPath = Directory.GetDirectories(FovDirPath).OrderBy(p => Directory.GetLastWriteTime(p)).First();
                                string strBarcode = string.Empty, strDate = string.Empty;
                                DateTime dtStartTime = new DateTime();
                                //int iWidth = 0, iHeight = 0;
                                //MessageBox.Show("strFirstPath__" + strFirstPath);
                                if (string.IsNullOrEmpty(strFirstPath) == false)
                                {
                                    strDate = (strFirstPath.Split('\\').Last()).Split('_').First();
                                    strBarcode = (strFirstPath.Split('\\').Last()).Split('_')[1];
                                }
                                strFirstPath = Path.Combine(ip, strFirstPath);   //20190826 moditied

                                string strFovDataFileLoadOverByUI = Path.Combine(strFirstPath, "FovPosInfo" + ".txt");  //当存储完FovData后  UI会存一个file文件;
                                string strFovDataFinishByUI = Path.Combine(strFirstPath, "FovDataFinish.fs");  //当存储完FovData后  UI会存一个file文件;
                                string strFovFileFinishByUI = Path.Combine(strFirstPath, "FOVFileFinish.fs");
                                //read image
                                //if (File.Exists(strFovDataFileLoadOverByUI))
                                if (Directory.Exists(strFirstPath))
                                {
                                    DateTime dtTime = DateTime.Now;
                                    while (true)
                                    {

                                        DateTime dtEndTime = DateTime.Now;
                                        //log.WriteLog("---WAIT");
                                        if ((dtEndTime - dtTime).TotalSeconds > 120)  //TEST
                                        {
                                            Directory.Delete(strFirstPath, true);
                                            break;
                                        }
                                        //log.WriteLog(strFirstPath);
                                        //string strTmpFirstDir = string.Empty;
                                        if (Directory.Exists(strFirstPath)
                                            && File.Exists(strFovDataFileLoadOverByUI)
                                            && File.Exists(strFovDataFinishByUI)
                                            && File.Exists(strFovFileFinishByUI)
                                            )
                                        {
                                            System.Threading.Thread.Sleep(800);
                                            arrStrTxtPCBPath = Directory.GetFiles(strFirstPath, "*.pcb");
                                            if (arrStrTxtPCBPath != null && arrStrTxtPCBPath.Length > 0)
                                            {
                                                //strFirstPath = strFirstPath.Replace(strBarcode, Path.GetFileNameWithoutExtension(arrStrTxtPCBPath[0]));
                                                strBarcode = Path.GetFileNameWithoutExtension(arrStrTxtPCBPath[0]);
                                            }
                                            //Path.GetFileNameWithoutExtension(Path.Combine(strFirstPath,"*.pcb"));
                                            arrStrFovFileLines = File.ReadAllLines(strFovDataFileLoadOverByUI);
                                            //arrPosXmm = null; arrPosYmm = null;
                                            //arrIFovID = null;
                                            string strJobName = string.Empty;
                                            //MessageBox.Show("arrStrFovFileLines:" + arrStrFovFileLines.Length);
                                            if (arrStrFovFileLines != null && arrStrFovFileLines.Length > 0)
                                            {
                                                int iTmpOrg = arrIFovID.Count;
                                                if (arrStrFovFileLines.Length > iTmpOrg)
                                                {
                                                    for (int i = iTmpOrg; i < arrStrFovFileLines.Length - iTmpOrg; i++)
                                                    {
                                                        arrPosXmm.Add(0f);
                                                        arrPosYmm.Add(0f);
                                                        arrIFovID.Add(0);
                                                    }
                                                }
                                                //arrPosXmm = new float[arrStrFovFileLines.Length];
                                                //arrPosYmm = new float[arrStrFovFileLines.Length];
                                                //arrIFovID = new int[arrStrFovFileLines.Length];
                                                for (int i = 0; i < arrStrFovFileLines.Length; i++)
                                                {
                                                    arrIFovID[i] = int.Parse(arrStrFovFileLines[i].Split(',')[0]);
                                                    arrPosXmm[i] = float.Parse(arrStrFovFileLines[i].Split(',')[5]);
                                                    arrPosYmm[i] = float.Parse(arrStrFovFileLines[i].Split(',')[6]);
                                                    if (i == 0)
                                                    {
                                                        strJobName = arrStrFovFileLines[i].Split(',')[1];
                                                        dtStartTime = Convert.ToDateTime(arrStrFovFileLines[i].Split(',')[8]);
                                                        _strPerWidthHeight = arrStrFovFileLines[i].Split(',')[9];
                                                        _bNeedZoom = bool.Parse(arrStrFovFileLines[i].Split(',')[10]);
                                                        strDataExportSaveFovImagePath = arrStrFovFileLines[i].Split(',')[11];
                                                    }
                                                }
                                            }
                                            // if (Directory.Exists(strFirstPath))
                                            if (File.Exists(strFovDataFileLoadOverByUI))
                                            {
                                                //SaveOrLoadImgData.bUseOpcv = true;
                                                //SaveOrLoadImgData.iImgFormat = 2;
                                                //Directory infoFile = Directory();
                                                arrFovFiles = _baseFuc.GetFiles(strFirstPath, ".imgdat");//Directory.GetFiles(strFirstPath);
                                                //arrFovFiles = dir.GetDirectories().OrderBy(n >= dir.LastWriteTime).Last();
                                                if (arrFovFiles != null && arrFovFiles.Length > 0)
                                                {
                                                    //log.WriteLog("loadFovData start", "SaveFovPCBThread");
                                                    AppLogHelp.WriteLog(LogFileFormate.FOVPCB, "loadFovData start");
                                                    int iCount = arrFovFiles.Length; //((int)(iCount / _iProcessorCount) + 1)
                                                    //Parallel.ForEach(Partitioner.Create(0, iCount, 2), range =>
                                                    //{
                                                    //arrAPP_SaveOrLoadImgDataInFo;
                                                    int iListSaveOrLoadImgDataInFoCount = arrAPP_SaveOrLoadImgDataInFo.Count;

                                                    //if (arrAPP_SaveOrLoadImgDataInFo == null || arrAPP_SaveOrLoadImgDataInFo.Count<=0)
                                                    //{
                                                    for (int i = iListSaveOrLoadImgDataInFoCount; i < iCount - iListSaveOrLoadImgDataInFoCount; i++)
                                                    {
                                                        APP_SaveOrLoadImgDataInFo SaveOrLoadImgDataInFo = new APP_SaveOrLoadImgDataInFo();
                                                        _stImgData = new ImgCSCoreIM.ST_SaveOrLoadImgData();
                                                        SaveOrLoadImgDataInFo.stImgData.iImgFormat = 2;
                                                        SaveOrLoadImgDataInFo.stImgData.bUseOpcv = false;
                                                        SaveOrLoadImgDataInFo.stImgData.bUseFastLoadBmp = false;
                                                        arrAPP_SaveOrLoadImgDataInFo.Add(SaveOrLoadImgDataInFo);
                                                    }
                                                    //}
                                                    for (int i = 0; i < iCount; i++)
                                                    {
                                                        //_baseFuc.LoadImadat(strFirstPath + "\\" + i.ToString(), ref arrAPP_SaveOrLoadImgDataInFo[i].stImgData);
                                                        //arrAPP_SaveOrLoadImgDataInFo[i].stImgData = _stImgData;
                                                        arrAPP_SaveOrLoadImgDataInFo[i].fPosXmm = arrPosXmm[i];
                                                        arrAPP_SaveOrLoadImgDataInFo[i].fPosYmm = arrPosYmm[i];
                                                        arrAPP_SaveOrLoadImgDataInFo[i].iFovID = arrIFovID[i];
                                                    }
                                                    //do save pcb image

                                                    string str = string.Empty;
                                                    //Parallel.ForEach(Partitioner.Create(0, iMaxParallelCount, (int)(iCount / _iProcessorCount) + 1), range =>
                                                    //{
                                                    //    for (int i = range.Item1; i < range.Item2; ++i)
                                                    //    {
                                                    //if (i == 0 && appSettingHandle._appSettingData.stDataExpVT.stSavePCBImageParams.bEnabled)
                                                    //{
                                                    //    //SavePCBImages savePCB = new SavePCBImages();
                                                    //    _savePCB.SaveWholePCBImage(strBarcode, strDate, strJobName, arrAPP_SaveOrLoadImgDataInFo
                                                    //    , ref appSettingHandle._appSettingData, ref _configData, dtStartTime, _strPerWidthHeight, _baseFuc, _stImgData);
                                                    //    log.WriteLog("savePCB success end", "SavePCBThread");
                                                    //}
                                                    //if (i == 1 && appSettingHandle._appSettingData.stDataExpVT.bEnSaveFovImage)
                                                    if (appSettingHandle._appSettingData.stDataExpVT.bEnSaveFovImage)
                                                    {
                                                        //_bNeedZoom = true;

                                                        str = _saveFOV.SaveFovImagesInfo(strFovDataFileLoadOverByUI, strFirstPath, strBarcode, strDate, dtStartTime,
                                                                        arrAPP_SaveOrLoadImgDataInFo,
                                                                        ref appSettingHandle._appSettingData,
                                                                        ref Aconfig, _bNeedZoom, _baseFuc, _strPerWidthHeight, strDataExportSaveFovImagePath,
                                                                        _stImgData, ExToFovImagePath,
                                                                        iCount,
                                                                        pcbFovConfig.IP
                                                                        )
                                                                        ;
                                                        //double tmp = (DateTime.Now - dtTime).TotalSeconds;
                                                        if (string.IsNullOrEmpty(str))
                                                        {
                                                            // log.WriteLog("saveFOV success end" + str, "SaveFOVThread");
                                                            //this.rtbFovPcbLog.Text = DateTime.Now.ToString(RS_DATETIME_FORMAT) + "saveFOV success end=>" + strFirstPath + RS_LINEEND + this.rtbFovPcbLog.Text;
                                                            writeRichTextBoxLog("saveFOV success end=>" + strFirstPath);
                                                            //rtbFovPcbLog.SelectionColor = Color.Green;
                                                            AppLogHelp.WriteLog(LogFileFormate.FOVPCB, "saveFOV success end=>" + strFirstPath);
                                                        }
                                                        else
                                                        {
                                                            //log.WriteErr("error:saveFOV fail end" + str, "SaveFOVThread");
                                                            //this.rtbFovPcbLog.Text += RS_LINEEND + DateTime.Now.ToString(RS_DATETIME_FORMAT) + "saveFOV fail end=>" + strFirstPath+RS_LINEEND;
                                                            //rtbFovPcbLog.SelectionColor = Color.Red;
                                                            writeRichTextBoxLog("saveFOV fail end=>" + strFirstPath);
                                                            AppLogHelp.WriteError(LogFileFormate.FOVPCB, "saveFOV fail end=>" + strFirstPath);
                                                        }
                                                    }
                                                    else if (appSettingHandle._appSettingData.stDataExpVT.stSavePCBImageParams.bEnabled)
                                                    {
                                                        for (int i = 0; i < iCount; i++)
                                                        {
                                                            _baseFuc.LoadImadat(strFirstPath + "\\" + i.ToString(), ref arrAPP_SaveOrLoadImgDataInFo[i].stImgData);

                                                        }
                                                        _savePCB.SaveWholePCBImage(strBarcode, strDate, strJobName, arrAPP_SaveOrLoadImgDataInFo
                                                        , ref appSettingHandle._appSettingData, ref Aconfig, dtStartTime, _strPerWidthHeight, _baseFuc, _stImgData, ExToFovImagePath, pcbFovConfig.IP);

                                                        //rtbFovPcbLog.SelectionAlignment = HorizontalAlignment.Center;
                                                        writeRichTextBoxLog("savePCB success end=>" + strFirstPath);
                                                        //this.rtbFovPcbLog.Text = DateTime.Now.ToString(RS_DATETIME_FORMAT) + "savePCB success end=>" + strFirstPath + RS_LINEEND + this.rtbFovPcbLog.Text;
                                                        //rtbFovPcbLog.SelectionColor = Color.Green;
                                                        //rtbFovPcbLog.Select(0, 0);
                                                        //rtbFovPcbLog.ScrollToCaret();
                                                        //this.rtbFovPcbLog.ScrollToCaret();
                                                        AppLogHelp.WriteLog(LogFileFormate.FOVPCB, "savePCB success end=>" + strFirstPath);
                                                    }
                                                    //  }
                                                    //});
                                                    Directory.Delete(strFirstPath, true);
                                                    //arrAPP_SaveOrLoadImgDataInFo.Clear();
                                                    ClearALLImgData(arrAPP_SaveOrLoadImgDataInFo);

                                                    ClearMemory();
                                                }
                                                //arrAPP_SaveOrLoadImgDataInFo = null;
                                            }
                                            arrStrFovFileLines = null;
                                            //arrPosXmm = null; arrPosYmm = null;
                                            //arrIFovID = null;
                                            break;
                                        }
                                        else
                                        {
                                            System.Threading.Thread.Sleep(100); //sleep timer
                                            //log.WriteErr("Error_FOVFileFinish.fs_ISNOT_EXISTS", "FOV/PCB");
                                            //AppLogHelp.WriteError(LogFileFormate.FOVPCB, "Error_FOVFileFinish.fs_ISNOT_EXISTS");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                System.Threading.Thread.Sleep(1000); //sleep timer
                            }
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(2000); //sleep timer
                            //break;
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(100); //sleep timer
                        //AutoAPP.MainForm.bFov = true;
                        //break;
                    }
                }
                catch (Exception e)
                {
                    //log.WriteErr("错误:" + e.Message, "FOV线程 ReadFovImages");
                    AppLogHelp.WriteError(LogFileFormate.FOVPCB,ip+  " ReadFovImages " + e.Message);
                    //System.Windows.Forms.MessageBox.Show("FOV线程 ReadFovImages异常__请手动检查ExToFovImage文件夹是否转换成功.." + e.Message);
                    //MessageBox.Show("FOV线程 ReadFovImages异常__请手动检查ExToFovImage文件夹是否转换成功.." + e.Message + "  [异常路径..]"+strFirstPath, "系统提示",

                    //                                        MessageBoxButtons.OK, MessageBoxIcon.Warning,

                    //                                        MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    //AutoAPP.MainForm.bFov = true;
                    return;
                }
                //finally
                //{
                //    //AutoAPP.MainForm.bFov = true;
                //    //return;
                //    //Thread.Sleep(30);
                //}
                //break;
            }


        }
        private void writeRichTextBoxLog(string message)
        {
            if (this.rtbFovPcbLog.Text != null && this.rtbFovPcbLog.Text.Length > 1024)
            {
                this.rtbFovPcbLog.Text = "";
            }
            //this.rtbFovPcbLog.SelectionColor = Color.Green;
            string strOldInfo = this.rtbFovPcbLog.Text;
            this.rtbFovPcbLog.Text = DateTime.Now.ToString(RS_DATETIME_FORMAT) + "  " + message;
            
            this.rtbFovPcbLog.AppendText (   RS_LINEEND + strOldInfo);

        }
        private void ClearALLImgData(List<APP_SaveOrLoadImgDataInFo> arrAPP_SaveOrLoadImgDataInFo)
        {
            try
            {
                int i = 0, iFovLength = arrAPP_SaveOrLoadImgDataInFo.Count, iImgLength = 0;

                for (; i < iFovLength; ++i)
                {
                    if (arrAPP_SaveOrLoadImgDataInFo[i] != null
                        && arrAPP_SaveOrLoadImgDataInFo[i].stImgData.byImgDataR != null)
                    {
                        iImgLength = arrAPP_SaveOrLoadImgDataInFo[i].stImgData.byImgDataR.Length;
                        Array.Clear(arrAPP_SaveOrLoadImgDataInFo[i].stImgData.byImgDataR, 0, iImgLength);
                        Array.Clear(arrAPP_SaveOrLoadImgDataInFo[i].stImgData.byImgDataG, 0, iImgLength);
                        Array.Clear(arrAPP_SaveOrLoadImgDataInFo[i].stImgData.byImgDataB, 0, iImgLength);
                    }
                }
            }
            catch (System.Exception ex)
            {

            }

        }

        #endregion

        private void btnOnLinePCBFOV_Click(object sender, EventArgs e)
        {
            bool bIsHaveSavePCBFOVSetting = false;
            string strOnLinePCBFOVIniFilePath = Path.Combine(strConfigIniPath,"FOVPCB.ini");
            string strIpICount = string.Empty, strIP=string.Empty,strRealInfo=string.Empty;
            ///读取PCB FOV联网ini
            if (File.Exists(strOnLinePCBFOVIniFilePath))
            {
                strIpICount = INIFileHelper.ReadIniData("fovpcb", "iCount", "", strOnLinePCBFOVIniFilePath);
                strRealInfo = INIFileHelper.ReadIniData("fovpcb", "realInfo", "" , strOnLinePCBFOVIniFilePath);
                if (string.IsNullOrEmpty(strIpICount) == false && strIpICount != "0")
                {
                    bIsHaveSavePCBFOVSetting = true;
                }
                //192.168.1.2_192.168.1.3
                //strIP = INIFileHelper.ReadIniData("fovpcb", "ip", "", strOnLinePCBFOVIniFilePath);

            }
            if (bIsHaveSavePCBFOVSetting)
            {
                //改变box文字
                if (this.btnOnLinePCBFOV.Text == "开启联网模式")
                {
                    this.btnOnLinePCBFOV.Text = "关闭联网模式";

                    PCBFOV_ONLINE_THREAD_COUNT = int.Parse(strIpICount);

                    _PCBFOVConfig = new PCBFOVConfig[PCBFOV_ONLINE_THREAD_COUNT];

                    for (int i = 0; i < int.Parse(strIpICount); i++)
                    {

                        //PCBFOVConfig pCBFOVConfig = new PCBFOVConfig();
                        //AppLayerLib.AppSettingHandler AappSettingHandle =new AppSettingHandler();
                        _PCBFOVConfig[i].IP = strRealInfo.Split('|')[i].Split('@')[1];
                        _PCBFOVConfig[i].LineName = strRealInfo.Split('|')[i].Split('@')[0];
                         AppLayerLib.AppSettingHandler AappSettingHandle = new AppSettingHandler();
                         InspectConfig.ConfigData configData = new InspectConfig.ConfigData();
                         ReadOnlineAppsetingFrUI(_PCBFOVConfig[i].IP, ref AappSettingHandle, ref  configData);
                         _PCBFOVConfig[i].AappSettingHandle = AappSettingHandle;
                         _PCBFOVConfig[i].Aconfig = configData;
                        string strAutoAPPIniFile = Path.Combine(strConfigIniPath, _strIniAutoAPPConfigFileName);   //20190826 moditied
                        strAutoAPPIniFile ="\\\\"+ _PCBFOVConfig[i].IP +"\\"+ strAutoAPPIniFile;
                        strAutoAPPIniFile = strAutoAPPIniFile.Replace("D:\\", "").Replace("D://", "");

                        string FovDirPath = bu_Peng.ReadIniData("autoAPP", "ExToFovImage", string.Empty, strAutoAPPIniFile);
                        FovDirPath = FovDirPath.Split('\\').Last();
                        _PCBFOVConfig[i].ExToFovImagePath = "\\\\" + _PCBFOVConfig[i].IP + "\\" + FovDirPath;
                        bEnDataForOnLineFovPcb = true;
                    }


                    
                }
                else if (this.btnOnLinePCBFOV.Text == "关闭联网模式")
                {
                    this.btnOnLinePCBFOV.Text = "开启联网模式";
                    bEnDataForOnLineFovPcb = false;
                    bIsOpenOnlinePCBFOV = true;
                    Thread.Sleep(300);
                    StopOnlinePCBFOVThread();
                }
                



            }
            else
            {
                MessageBox.Show("未设置联网参数,请设置...");
            }

        }

        private void btnFovPCB_Click(object sender, EventArgs e)
        {
            //this.Enabled = false;
            PCBFOVSettingFm fm = new PCBFOVSettingFm();
            fm.ShowDialog();
            
        }

        
    }

   
}
