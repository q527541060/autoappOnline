using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Text;
using System.Runtime.InteropServices;

using System.Linq;
using System.Collections.Generic;
using AutoAPP.ServiceReference1;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using ImgCSCoreIM;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Collections;

namespace AutoAPP
{
        /// <summary>
        /// 
        /// </summary>
        public class ZZFoxPDB
        {
            public static readonly string RS_EXPRT_TITLE = " Date,Time,Acount,GoodAcount,FileAcount,CapacityOfHour,PreGood,RunTime,WaitTime,cyclesTime,BadTime";
            public static readonly string RS_DATE_FORMAT = "yyyyMMdd";
            public static readonly string RS_TIME_FORMAT = "HHmmss";
            public static readonly string RS_SPIT = ",";
            public static readonly string RS_UNLINE = "\r\n";
            public static readonly string RS_DATE_FORMAT_COV = "yyyy-MM-dd";
            public static readonly string lineUnder = "\r\n";
            public static readonly string RS_STR_FORMAT_4F = "0.0000";
            public static readonly string RS_ZERO = "0";
            public static readonly string RS_PER = "%";
        }
        public class LHFox
        {
            public static readonly string RS_AT = "@";
            public static readonly string RS_UNLINE = "\r\n";
            public static readonly string RS_UNDER_LINE = "_";
        }
        public class JUFEI
        {
            public static readonly string JF_TITLE = "任务单,开始时间,结束时间,线体,程序名,总测单板数,良品数,良率,不良率,误报数,误报率,,,,," +
                "无锡,少锡,多锡,高度偏高,高度偏底,面积偏多,面积偏少,X偏移,Y偏移,短路,锡型不良,污点,共面,短路,焊盘面积错误 ," +
                " ,待机时间,运行时间,停机时间,故障时间";
            public static readonly string JF_DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
            public static readonly string JF_Spit = ",";
            public static readonly string JF_EndLine = "\r\n";
        }
        public class CDFOX
        {

            //lin 20190813
            //public static readonly string strConnectString = "server=127.0.0.1;user id=root;database=spidb;Charset=utf8";
            public static readonly string strConnectString =  WSClnt.PubStaticParam._strSPIdbConnectionString;
            public static readonly string RS_CD_Spit = ",";
            public static readonly string RS_END_LINE = "\r\n";
            
        }
        
        public class PadInfo
        {
            //  PadID,Component,Type,Area(%),Height,Volume(%),XOffset,YOffset,
            //  PadSize(X),PadSize(Y),Area,Height(%),Volume,Result,Errcode,PinNum,
            //  Barcode,Date,Time,ArrayID
            public string padID { get; set; }
            public string component { get; set; }
            public string Type { get; set; }
            public string perArea { get; set; }
            public string height { get; set; }
            public string perVolume { get; set; }
            public string xOffset { get; set; }
            public string yOffset { get; set; }
            public string padSizeX { get; set; }
            public string padSizeY { get; set; }
            public string area { get; set; }
            public string perHeight { get; set; }
            public string volume { get; set; }
            public string result { get; set; }
            public string errcode { get; set; }
            public string pinNum { get; set; }
            public string barcode { get; set; }
            public string date { get; set; }
            public string time { get; set; }
            public string arrayID { get; set; }
            //add by Peng 20180509----- 面积、体积、高度、X/Y偏移、分组选项
            public string areaH { get; set; }
            public string areaL { get; set; }
            public string volumeH { get; set; }
            public string volumeL { get; set; }
            public string heightH { get; set; }
            public string heightL { get; set; }
            public string xOffsetH { get; set; }
            public string yOffsetH { get; set; }
            public string shiftDataType { get; set; }
            public string padGroup { get;set; }

            public string areaPerH { get; set; }
            public string areaPerL { get; set; }
            public string volumePerH { get; set; }
            public string volumePerL { get; set; }
            public string heightPerH { get; set; }
            public string heightPerL { get; set; }
            public string xOffsetPerH { get; set; }
            public string yOffsetPerH { get; set; }
            public string posXmm { get; set; }
            public string posYmm { get; set; }
            public string bridgeWidth { get; set; }
            public string bridgeLength { get; set; }
            public string bridgeHeight { get; set; }
            public PadInfo()
            {

            }
            
        }
        //Q.F.2019.04.09
        public class APP_SaveOrLoadImgDataInFo
        {
            public ImgCSCoreIM.ST_SaveOrLoadImgData stImgData;// { get; set; }
            //public bool bUseFastLoadBmp { get; set; }
            //public bool bUseOpcv { get; set; }
            //public byte[] byImgDataB { get; set; }
            //public byte[] byImgDataG { get; set; }
            //public byte[] byImgDataR { get; set; }
            //public int iHeight { get; set; }
            //public int iImgFormat { get; set; }
            //public int iWidth { get; set; }
            public bool bHasLoaded;
            public float fPosXmm { get; set; }
            public float fPosYmm { get; set; }
            public int iFovID { get; set; }
            public APP_SaveOrLoadImgDataInFo() { }
        }

    public class bu_Peng
    {
        private ThreadProcessFoxconn runnerFoxconn;
        private ThreadProcessCDFO runnerCDfox;
        private ThreadProcessJUFEI runnerJF;
        private ThreadProcessLHFO runnerLHfox;
        private ThreadProcessDefaultTools runnerDefaultTools;
        private ThreadProcessGYFO runnerGYfox;
        private ThreadProcessCAIHUANG runnerCAIhuang;
        //private ThreadProcessPCB runnerPcb;
        private ThreadProcessTianJinWeiYe runnerTianJinWeiYe;
        private ThreadProcessWeiXin runnWeiXin;
        private ThreadProcessDaHua runnDaHua;
        private ThreadProcessLuBangTong runnLuBangTong;
        private ThreadProcessLuBangTongPRD runnLuBangTongPRD;
        //private ThreadProcessFovEYSPIToAOI runnEYSPIToAOI;
        private static readonly string RS_FORMAT_DATETIME = "yyyy-MM-dd HH:mm:ss";
        private  readonly string RS_FORMAT_DATETIME_SM = "yyyy/MM/dd HH:mm:ss";
        public static readonly string RS_Format_DateTimeFileName = "yyyyMMddHHmmss";
        private string ThreadName = "Save CSV For FOXCNN";
        private string ThreadNameCDCNN = "Save CSV For CDCNN";
        private string ThreadNameLHCNN = "Save CSV For LHCNN";
        private string ThreadJFCNN = "Save CSV FOR JFCNN ";
        private string ThreadFovName = "Fov";
        private string ThreadPcbName = "Pcb";
        private string ThreadNameStatus = "Status";
        //private string ThreadGYCNN = "Save Data For GYCNN";
        //private ImageProcessingCS.Basefunction _baseFuncXU = new ImageProcessingCS.Basefunction();
        private static int _iProcessorCount = Environment.ProcessorCount;
        private SavePCBImages _savePCB = new SavePCBImages();
        private SaveFovImages _saveFOV = new SaveFovImages();
        ImgCSCoreIM.ST_SaveOrLoadImgData _stImgData = new ImgCSCoreIM.ST_SaveOrLoadImgData();
        public InspectConfig.ConfigData _configData = null;

        private EYSPIToAOIHelp cEYSPIToAOIHelp = new EYSPIToAOIHelp();

        #region API函数声明

        [DllImport("kernel32")]//返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);


        #endregion

        #region 读Ini文件
        /// <summary>
        /// 读取INI配置的值
        /// </summary>
        /// <param name="Section">节点名称</param>
        /// <param name="Key">属性名称</param>
        /// <param name="NoText">默认空值</param>
        /// <param name="iniFilePath">ini文件路径</param>
        /// <returns></returns>
        public static string ReadIniData(string Section, string Key, string NoText, string iniFilePath)
        {
            if (File.Exists(iniFilePath))
            {
                StringBuilder temp = new StringBuilder(1024);
                GetPrivateProfileString(Section, Key, NoText, temp, 1024, iniFilePath);
                return temp.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        #endregion

        #region 写Ini文件
        /// <summary>
        /// 写入INI配置文件
        /// </summary>
        /// <param name="Section">节名称</param>
        /// <param name="Key">属性名称</param>
        /// <param name="Value">属性值</param>
        /// <param name="iniFilePath">ini文件路径</param>
        /// <returns></returns>
        public static bool WriteIniData(string Section, string Key, string Value, string iniFilePath)
        {
            if (File.Exists(iniFilePath))
            {
                long OpStation = WritePrivateProfileString(Section, Key, Value, iniFilePath);
                if (OpStation == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion
        
        /// <summary>
        ///  连接测试
        /// </summary>
        /// <param name="strMySQLConnect"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public string TestConnection(string strMySQLConnect)
        {
            try
            {
                string strReturn = "连接测试成功！";
                MySqlConnection conn = new MySqlConnection(strMySQLConnect);
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "  SELECT * from tbpadmeasure  LIMIT 1  ";
                    MySqlDataReader reader = cmd.ExecuteReader();
                }
                catch (System.Exception ex)
                {
                    strReturn = strMySQLConnect.ToString() + "连接出错：" + ex.Message.ToString();
                    //log.WriteErr("错误 ! " + strReturn, "Main");
                    AppLogHelp.WriteError(LogFileFormate.SQL, "TestConnection Exception: " + strReturn);
                }

                //log.WriteLog(strReturn, "Main");
                AppLogHelp.WriteLog(LogFileFormate.SQL, "TestConnection : " + strReturn);
                return strReturn;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public string TestConnectionFox(string strMysqlConn)
        {
            try
            {
                string strReturn = "测试未链接";
                MySqlConnection conn = new MySqlConnection(strMysqlConn);
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                    {
                        conn.Open();
                    }
                }
                catch (System.Exception ex)
                {
                    strReturn = strMysqlConn.ToString() + "连接出错：" + ex.Message.ToString();
                    //log.WriteErr("错误 ! " + strReturn, "Main");
                    AppLogHelp.WriteError(LogFileFormate.SQL, "TestConnectionFox : " + strReturn);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        strReturn = "测试链接成功!";
                    }
                }
                //log.WriteLog(strReturn, "Main");
                AppLogHelp.WriteLog(LogFileFormate.SQL, "TestConnectionFox : " + strReturn);
                return strReturn;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
     
        /// <summary>
        /// 更新配置文件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void UpdateConfigKey(string key, string value)
        {

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(System.Windows.Forms.Application.ExecutablePath + ".config");

            XmlNode xNode;
            XmlElement xElem1;
            XmlElement xElem2;
            XmlElement xElem3;

            xNode = xDoc.SelectSingleNode("//applicationSettings");

            xElem1 = (XmlElement)xNode.SelectSingleNode("//setting[@name='" + key + "']");
            if (xElem1 != null)
                xElem1.FirstChild.InnerText = value;
            //xElem1.SetAttribute("value", value);
            else
            {
                xElem2 = xDoc.CreateElement("setting");
                xElem2.SetAttribute("name", key);
                xElem2.SetAttribute("serializeAs", "String");
                xNode.AppendChild(xElem2);

                xElem3 = xDoc.CreateElement("value");
                xElem3.InnerText = value;
                xElem2.AppendChild(xElem3);
            }
            xDoc.Save(System.Windows.Forms.Application.ExecutablePath + ".config");
        }

        /// <summary>
        /// stop
        /// </summary>
        /// <returns></returns>
        public Boolean Stop()
        {
            Boolean blnReturn = true;
            try
            {
                if (runnerCDfox != null)
                    if (runnerCDfox.Running)
                    {
                        runnerCDfox.Stop();
                        blnReturn = false;                       
                    }
                if (runnerJF != null)
                    if (runnerJF.Running)
                    {
                        runnerJF.Stop();
                        blnReturn = false;
                    }
                if (runnerLHfox != null)
                    if (runnerLHfox.Running)
                    {
                        runnerLHfox.Stop();
                        blnReturn = false;
                    }
                if (runnerFoxconn != null)
                    if (runnerFoxconn.Running)
                    {
                        // 确认是否停止
                        //if (MessageBox.Show("后台线程正在执行，是否结束线程并退出？", "确认", MessageBoxButtons.OKCancel) != DialogResult.OK)
                        //{
                        runnerFoxconn.Stop();
                        blnReturn = false;
                        //}
                        //else
                        //{
                        //    blnReturn = true;
                        //}
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return blnReturn;
        }
      
        #region @"郑州富士康保存CSV文件         " 

        /// <summary>
        /// 开始 富士康 MES进程
        /// </summary>
        /// <param name="dtStartTime"></param>
        /// <param name="dtEndTime"></param>
        /// <param name="log"></param>
        public void AutoStartFoxconn(DateTime dtStartTime, DateTime dtEndTime,String strCVSFile, int AintSectionTime)
        {
            try
            {
                runnerFoxconn = new ThreadProcessFoxconn(dtStartTime, dtEndTime, strCVSFile, AintSectionTime);
                runnerFoxconn.Run();
            }
            catch (Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
            }

        }

        /// <summary>
        ///  ExportZZFox
        /// </summary>
        /// <param name="AstrDir"></param>
        /// <param name="AstartTime"></param>
        /// <param name="AsendTime"></param>
        /// <param name="AintSectionTime"></param>
        public static void ExportZZFoxconnInformat(
            string AsMySqlConnectionString,
            string AstrDir,
            DateTime AstartTime,   //界面设置导出开始时间
            DateTime AsendTime,    //界面设置导出结束时间
            int AintSectionTime   //界面设置导出区域时间 单位 /分
            )      
        {
            string sqlFx = "";
            string sqlFb = "", sqlFs = "", sqlFsr = "";
            string C_AOI = "AOI";
            int sec = AintSectionTime;
            string timeFile = AstrDir + "\\" + "timeTmp.txt";
            try
            {
                System.Text.StringBuilder strbld = new System.Text.StringBuilder();

                
                if (!string.IsNullOrEmpty(AstrDir)
                    && AintSectionTime != 0
                    )
                {                    
                    //间隔多少秒
                    //TimeSpan ts = new TimeSpan(0, 0, AintSectionTime * 60);
                    //开始时间 结束时间差
                    //int es = (int)AstartTime.Subtract(AsendTime).Duration().TotalSeconds;
                    //int multiple = 0;
                    //if (es % (AintSectionTime * 60) == 0) multiple = es / (AintSectionTime * 60);
                    //else multiple = es / (AintSectionTime * 60) + 1;
                    sqlFx = "SELECT t.Customer,t.Factory,t.Floor,t.Line,t.EquipID,t.EquipName,t.Module " +
                            " FROM TBEquipStatus t " +
                            " WHERE t.UpdateTime >=STR_TO_DATE('" + AstartTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S') " +
                            " and t.UpdateTime <=STR_TO_DATE('" + AsendTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S')" +
                            " GROUP BY t.Customer,t.Factory,t.Floor,t.Line;";
                    string sCustomer = "";
                    string sFactory = "";
                    string sFloor = "";
                    string sLine = "";
                    string sEquipID = "";
                    string sFileName = "";
                    string sFilePath = "";
                    string sEquipName = "";
                    string sModule = "";
                    //DBdata获取 设备信息
                    System.Data.DataTable dataTable = getDataTableForZZFox(AsMySqlConnectionString, sqlFx);
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        sCustomer = (string)dataTable.Rows[0][0];
                        sFactory = (string)dataTable.Rows[0][1];
                        sFloor = (string)dataTable.Rows[0][2];
                        sLine = (string)dataTable.Rows[0][3];
                        sEquipID = (string)dataTable.Rows[0][4];
                        sEquipName = (string)dataTable.Rows[0][5];
                        sModule = (string)dataTable.Rows[0][6];
                    }   
                    #region  "设备参数"
                    string equipNmFile = Path.Combine(AstrDir, C_AOI + "_" + sFactory + sFloor + sLine + "_" + sEquipID +  ".csv");
                    if (!File.Exists(equipNmFile))
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("Sinictekfloor2" + ZZFoxPDB.lineUnder +
                            sLine + ZZFoxPDB.lineUnder +
                            "SPI" + ZZFoxPDB.lineUnder +
                            "HOLLY" + ZZFoxPDB.lineUnder +
                            sEquipID + ZZFoxPDB.lineUnder +
                            sEquipName + ZZFoxPDB.lineUnder +
                            AstartTime.ToString("yyyyMMdd") + ZZFoxPDB.lineUnder +
                            "Date,Time,Ymove delta X,X move delta Y,Lighness R%,Lighness G%,Lighness B%,3D led brightness,scale?value" + ZZFoxPDB.lineUnder +
                            AstartTime.ToString("yyyyMMdd")+ ",190234,0.1,1.1,30,50,80,70,100" + ZZFoxPDB.lineUnder +
                            AstartTime.ToString("yyyyMMdd")+ ",190300,2.1,2.1,31,51,81,71,101" + ZZFoxPDB.lineUnder +
                            AstartTime.ToString("yyyyMMdd")+ ",190308,3.1,3.1,32,52,82,72,102" + ZZFoxPDB.lineUnder +
                            AstartTime.ToString("yyyyMMdd")+ ",190311,4.1,4.1,33,53,83,73,103" + ZZFoxPDB.lineUnder +
                            AstartTime.ToString("yyyyMMdd")+ ",190439,5.1,5.1,34,54,84,74,104" + ZZFoxPDB.lineUnder +
                            AstartTime.ToString("yyyyMMdd")+ ",190445,6.1,6.1,35,55,85,75,105" + ZZFoxPDB.lineUnder +
                            AstartTime.ToString("yyyyMMdd")+ ",190547,7.1,7.1,36,56,86,76,106" + ZZFoxPDB.lineUnder);

                        using (FileStream fs = new FileStream(equipNmFile, FileMode.Create))
                        {
                            StreamWriter sw = new StreamWriter(fs,System.Text.Encoding.Default);
                            sw.Write(sb);
                            sw.Close();
                        }
                    }

                    #endregion
                    DateTime dtStartTime = AstartTime;
                    while (dtStartTime < AsendTime)
                    {
                        DateTime dtEndTime = dtStartTime.AddMinutes(AintSectionTime);
                        if (!string.IsNullOrEmpty(sFactory)
                           && !string.IsNullOrEmpty(sFloor)
                           && !string.IsNullOrEmpty(sLine)
                           && !string.IsNullOrEmpty(sEquipID)
                           )
                        {
                            sFileName = C_AOI + "_" + sFactory + sFloor + sLine + "_" + sEquipID + "_" + dtStartTime.ToString(ZZFoxPDB.RS_DATE_FORMAT);
                            sFilePath = Path.Combine(AstrDir, sFileName + ".csv");
                             
                        }
                        else
                        {
                            return;
                        }
                        bool isNewFile = false;
                        if (!File.Exists(sFilePath))
                        {
                            using (FileStream fsLine1 = new FileStream(sFilePath, FileMode.Create))
                            {
                                ;
                            }
                            isNewFile = true;
                        }
                        #region    "測試總数量报表"
                        //nowTime = AstartTime.AddSeconds(AintSectionTime*60); lastTime = nowTime.AddSeconds(-sec*60); AintSectionTime += sec;
                        sqlFb = " SELECT t.PCBID,t.Result from TBBoard as t  " +
                                " WHERE   t.StartTime >= STR_TO_DATE('" + dtStartTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S') " +
                                " AND t.StartTime < STR_TO_DATE('" + dtEndTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S');";
                        sqlFsr =" SELECT sum(t.EndTime - t.StartTime)  FROM TBBoard as t " +
                                " WHERE  t.StartTime >= STR_TO_DATE('" + dtStartTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S') " +
                                " AND t.EndTime <STR_TO_DATE('" + dtEndTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S'); ";
                        sqlFs = " SELECT sum(Run=1 and error=0) * TimeInterval,sum(Idle=1 and error=0) * TimeInterval,sum(error = 1) * TimeInterval " +
                                " FROM TBEquipStatus as t  " +
                                " WHERE t.UpdateTime >= STR_TO_DATE('" + dtStartTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S') " +
                                " and  t.UpdateTime<STR_TO_DATE('" + dtEndTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S'); ";                        
                        if (!string.IsNullOrEmpty(sFactory)
                            && !string.IsNullOrEmpty(sFloor)
                            && !string.IsNullOrEmpty(sLine)
                            && !string.IsNullOrEmpty(sEquipID)
                            )
                        {
                            if (isNewFile)
                            {                           
                                strbld.Append(sFactory + sFloor + ZZFoxPDB.lineUnder +
                                        sLine + ZZFoxPDB.lineUnder +
                                        "AOI" + ZZFoxPDB.lineUnder +
                                        "HOLLY" + ZZFoxPDB.lineUnder +
                                        sEquipName + ZZFoxPDB.lineUnder +
                                        sEquipID + ZZFoxPDB.lineUnder +
                                        AstartTime.ToString(ZZFoxPDB.RS_DATE_FORMAT) + ZZFoxPDB.lineUnder +
                                        "No Check" + ZZFoxPDB.lineUnder);
                                strbld.Append(ZZFoxPDB.RS_EXPRT_TITLE + ZZFoxPDB.lineUnder);
                                isNewFile = false;
                            }
                            //DBdata获取 runSeconds  badSeconds waitSeconds 运行时间 故障时间 等待时间
                            System.Data.DataTable dl = getDataTableForZZFox(AsMySqlConnectionString,sqlFs);
                            string runSeconds = "";
                            string waitSeconds = "";
                            string cycleSeconds = "";
                            string badSeconds = "";
                            
                            if (dl != null && dl.Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(dl.Rows[0][0].ToString()))
                                {
                                    runSeconds = dl.Rows[0][0].ToString();
                                }
                                if (!string.IsNullOrEmpty(dl.Rows[0][1].ToString()))
                                {                                  
                                    waitSeconds = dl.Rows[0][1].ToString();
                                }
                                if (!string.IsNullOrEmpty(dl.Rows[0][2].ToString()))
                                {
                                    badSeconds = dl.Rows[0][2].ToString();
                                }
                            }
                            //DBdata 得到cycleSeconds 循环时间
                            System.Data.DataTable dsr = getDataTableForZZFox(AsMySqlConnectionString, sqlFsr);
                            if (dsr != null && dsr.Rows.Count > 0)
                            {
                                for (int m = 0; m < dsr.Rows.Count; m++)
                                {
                                    if (!string.IsNullOrEmpty(dsr.Rows[m][0].ToString()))
                                    {

                                        cycleSeconds = dsr.Rows[m][0].ToString();
                                    }
                                }
                            }
                            //DBdata获取 spc板信息
                            System.Data.DataTable de = getDataTableForZZFox(AsMySqlConnectionString, sqlFb);
                            int deAllAcount = 0, deGoodAcount = 0, deNgAcount = 0, dePassAcount = 0;
                            if (de != null && de.Rows.Count > 0)
                            {
                                for (int m = 0; m < de.Rows.Count; m++)
                                {

                                    if (de.Rows[m][1].ToString() == "0")
                                    {
                                        deGoodAcount++;
                                    }
                                    else if (de.Rows[m][1].ToString() == "2") { dePassAcount++; }
                                    else if (de.Rows[m][1].ToString() == "1") { deNgAcount++; }
                                    if (!string.IsNullOrEmpty(de.Rows[m][0].ToString()))
                                    {
                                        deAllAcount++;
                                    }
                                }
                            }
                            float sAcount = 0f;
                            if (deAllAcount == 0)
                            {
                                sAcount = 0f;
                            }
                            else
                            {
                                sAcount = ((float)(deGoodAcount + dePassAcount) / (float)deAllAcount);
                            }
                            if (string.IsNullOrEmpty(runSeconds)) runSeconds = ZZFoxPDB.RS_ZERO;//运行时间(时)                                                                                         
                            if (string.IsNullOrEmpty(waitSeconds)) waitSeconds = ZZFoxPDB.RS_ZERO;//等待时间(时)                                                                                     
                            if (string.IsNullOrEmpty(cycleSeconds)) cycleSeconds = ZZFoxPDB.RS_ZERO;//循环时间 (秒)                          
                            if (string.IsNullOrEmpty(badSeconds)) badSeconds = ZZFoxPDB.RS_ZERO;//故障时间(时)

                            runSeconds = (double.Parse(runSeconds) / (double)3600).ToString(ZZFoxPDB.RS_STR_FORMAT_4F);
                            waitSeconds = (double.Parse(waitSeconds) / (double)3600).ToString(ZZFoxPDB.RS_STR_FORMAT_4F);
                            badSeconds = (double.Parse(badSeconds) / (double)3600).ToString(ZZFoxPDB.RS_STR_FORMAT_4F);
                            //兑换成小时--
                            //TimeSpan tsRun = new TimeSpan(0, 0, 0, (int)(double.Parse(runSeconds)));
                            //runSeconds = tsRun.Hours + ":" + tsRun.Minutes + ":" + tsRun.Seconds + ":" + tsRun.Milliseconds;
                            //TimeSpan tsWait = new TimeSpan(0, 0, 0, (int)(double.Parse(waitSeconds)));
                            //waitSeconds = tsWait.Hours + ":" + tsWait.Minutes + ":" + tsWait.Seconds;
                            //TimeSpan tsBad = new TimeSpan(0, 0, 0, (int)(double.Parse(badSeconds)));
                            //badSeconds = tsBad.Hours + ":" + tsBad.Minutes + ":" + tsBad.Seconds;                          
                            // 
                            // 如果数据都为0则放弃输出
                            //if (   deAllAcount == 0
                            //    && deGoodAcount == 0
                            //    && dePassAcount == 0
                            //    && deNgAcount == 0
                            //    && sAcount == 0
                            //    && ZZFoxPDB.RS_STR_FORMAT_4F.Equals(runSeconds)
                            //    && ZZFoxPDB.RS_STR_FORMAT_4F.Equals(waitSeconds)
                            //    && ZZFoxPDB.RS_STR_FORMAT_4F.Equals(badSeconds)
                            //    && ZZFoxPDB.RS_ZERO.Equals(cycleSeconds)
                            //    )
                            //{
                            //}
                            //else
                            //{
                                strbld.Append(dtStartTime.ToString(ZZFoxPDB.RS_DATE_FORMAT) + ZZFoxPDB.RS_SPIT +
                                           dtStartTime.ToString(ZZFoxPDB.RS_TIME_FORMAT) + ZZFoxPDB.RS_SPIT +
                                           deAllAcount + ZZFoxPDB.RS_SPIT +
                                           (deGoodAcount + dePassAcount) + ZZFoxPDB.RS_SPIT +
                                           deNgAcount + ZZFoxPDB.RS_SPIT +
                                           deAllAcount * (60 / sec) + ZZFoxPDB.RS_SPIT +
                                           sAcount * 100 + ZZFoxPDB.RS_PER + ZZFoxPDB.RS_SPIT +//直通率
                                           runSeconds + ZZFoxPDB.RS_SPIT +
                                           waitSeconds + ZZFoxPDB.RS_SPIT +
                                           cycleSeconds + ZZFoxPDB.RS_SPIT +
                                           badSeconds + ZZFoxPDB.RS_SPIT + ZZFoxPDB.RS_UNLINE
                                 );
                           // }
                            if (!string.IsNullOrEmpty(strbld.ToString()))
                            {
                                FileStream fsLine1 = new FileStream(sFilePath, FileMode.Append);

                                StreamWriter wsLine = new StreamWriter(fsLine1);
                                wsLine.Write(strbld);
                                wsLine.Flush();
                                wsLine.Close();
                                fsLine1.Close();
                                strbld.Clear();
                            }
                        }
                        #endregion
                        dtStartTime = dtStartTime.AddMinutes(AintSectionTime);
                    }
                }
            }
            catch (Exception ex)
            {
                string error = "  function: (ExportZZFoxconnInformat)  数据导出出错 ";
                //log.WriteErr("错误 ! " + error + ex.Message );
                AppLogHelp.WriteError(LogFileFormate.FOVPCB, error + ex.Message);
            }
        }
        public static DataTable getDataTableForZZFox(string AsMySqlConnecionString , string AsMySqlQueryStr)
        {
            MySqlConnection mysqlCon = new MySqlConnection(AsMySqlConnecionString);
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter daptMySQL= new MySqlDataAdapter();
            DataTable dt = new DataTable();
            string sResult = string.Empty;
            try
            {                           
                openMySqlConnection(AsMySqlConnecionString, mysqlCon);
                cmd.Connection = mysqlCon;
                cmd.CommandText = AsMySqlQueryStr;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 500;
                daptMySQL.SelectCommand = cmd;
                daptMySQL.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (mysqlCon.State != ConnectionState.Closed)
                {
                    mysqlCon.Close();                   
                }
                cmd.Dispose();
                daptMySQL.Dispose();
            }
            return dt;
        }
        public static void openMySqlConnection(string AsMySqlConnecionString,MySqlConnection mysqlConnection)
        {            
            try
            {
                if (string.IsNullOrEmpty(AsMySqlConnecionString))
                {
                    throw new Exception("(openMySqlConnection) error : mySqlConnectionString empty");
                }
                else
                {
                    if (mysqlConnection.State != ConnectionState.Open)
                    {
                        mysqlConnection.Open();
                    }
                }
            }
            catch (Exception ex)
            {
                string error = "  function: (openMySqlConnection)  数据库连接出错 ";
                //log.WriteErr("错误 ! " + error + ex.Message  );
            }
        }
        #endregion

        #region "聚飞"

        /// <summary>
        /// 开始 聚飞 MES进程
        /// </summary>
        /// <param name="dtStartTime"></param>
        /// <param name="dtEndTime"></param>
        /// <param name="log"></param>
        public void AutoStartJufei(DateTime dtStartTime, DateTime dtEndTime, String strFile)
        {
            try
            {
                runnerJF = new ThreadProcessJUFEI(dtStartTime, dtEndTime, strFile);
                runnerJF.Run();
            }
            catch (Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.ToString(), ThreadJFCNN);
                AppLogHelp.WriteError(LogFileFormate.AppMain, ThreadJFCNN + ex.ToString());
            }

        }

        public static void ExDataForJUFEI(
            string AsMySqlConnectionString,
            string AstrDir,
            DateTime AstartTime,
            DateTime AsendTime)
        {
            string sqlQueryPad = "";

            string sqlQueryPcbBd = "";

            string sFilePath = "";

            string jobName = string.Empty;
            try
            {

                sqlQueryPcbBd = " SELECT " +
                                " pcb.PCBID, " +
                                " pcb.StartTime, " +
                                " pcb.EndTime, " +
                                " pcb.LineNo, " +
                                " jobInfo.JobName, " +
                                " pcb.Result " +
                                " FROM " +
                                " spidb.TBBoard AS pcb " +
                                " INNER JOIN spidb.TBJobInfo AS jobInfo ON pcb.JobIndex = jobInfo.SerNo " +
                                " WHERE " +
                                " pcb.StartTime >= STR_TO_DATE('" + AstartTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S')   " +
                                " AND pcb.EndTime < STR_TO_DATE('" + AsendTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S')  ";

                sqlQueryPad = " SELECT " +
                                " pad.DefectType, " +
                                " pcb.StartTime, " +
                                " pcb.EndTime, " +
                                " pad.JudgeRes " +
                                " FROM " +
                                " spidb.TBPadMeasure AS pad " +
                                " INNER JOIN spidb.TBBoard AS pcb ON pad.PCBID = pcb.PCBID " +
                                " AND pad.JobIndex = pcb.JobIndex " +
                                " WHERE " +
                                " pcb.PCBID = '{0}' " +
                                 " AND pcb.StartTime >= STR_TO_DATE('" + AstartTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S')   " +
                                " AND pcb.EndTime < STR_TO_DATE('" + AsendTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S')  ";
                //DataTable pcb
                DataTable dtPCB = getDataTableForZZFox(AsMySqlConnectionString, sqlQueryPcbBd);
                //DataTable pad 的集合;
                DataTable[] dtPad = new DataTable[dtPCB.Rows.Count];
                if (dtPCB != null && dtPCB.Rows.Count > 0)
                {
                    System.Threading.Thread.Sleep(5000);
                    for (int i = 0; i < dtPCB.Rows.Count; i++)
                    {
                        string hPcb = dtPCB.Rows[i][0].ToString();
                        string sqlQueryPadNew = string.Format(sqlQueryPad, hPcb);
                        dtPad[i] = getDataTableForZZFox(AsMySqlConnectionString, sqlQueryPadNew);
                    }
                }

                DateTime dStartTime = AstartTime;
                int iAcount = 0;
                string sLineNo = string.Empty;

                while (dStartTime < AsendTime)
                {
                    bool bFileIs8 = false;
                    bool bFileIs12 = false;
                    bool bFileIs20 = false;
                    iAcount++;
                    DateTime dEndTime = dStartTime;
                    System.Text.StringBuilder strBld = new System.Text.StringBuilder();
                    sFilePath = Path.Combine(AstrDir, dStartTime.ToString("yyyy-MM-dd") + ".csv");
                    if (!File.Exists(sFilePath))
                    {
                        using (FileStream fsLine1 = new FileStream(sFilePath, FileMode.Create))
                        {
                            StreamWriter sw = new StreamWriter(fsLine1);
                            sw.WriteLine(JUFEI.JF_TITLE);
                            sw.Close();
                        }
                    }
                    int iMissing = 0, iInsufficient = 0, iExcess = 0, iOverHeight = 0,
                           iLowHeight = 0, iOverArea = 0, iLowArea = 0, iShiftX = 0, iShiftY = 0,
                           iBridge = 0, iShapeError = 0, iSmeared = 0, iCoplanarity = 0, iPreBridge = 0,
                           iPadAreaError = 0;
                    int iGoodCount = 0, iPcbCount = 0;
                    string hh = dStartTime.ToString("HHmmss");
                    if (dStartTime.ToString("HH:mm:ss").Equals("08:00:00"))
                    {
                        iAcount = 1;
                        dEndTime = dStartTime.AddHours(4);
                        if (dtPCB != null && dtPCB.Rows.Count > 0)
                        {
                            for (int x = 0; x < dtPCB.Rows.Count; x++)
                            {
                                if ((DateTime)dtPCB.Rows[x][1] >= dStartTime && (DateTime)dtPCB.Rows[x][2] < dEndTime)
                                {
                                    iPcbCount++;
                                    if (int.Parse(dtPCB.Rows[x][5].ToString()) == 0 || int.Parse(dtPCB.Rows[x][5].ToString()) == 2)
                                    {
                                        iGoodCount++;
                                    }
                                    jobName = (string)dtPCB.Rows[x][4];
                                    sLineNo = (string)dtPCB.Rows[x][3];
                                }
                            }

                        }
                        bFileIs8 = true;
                        //任务单,开始时间,结束时间,线体,程序名,总测单板数,良品数,良率,不良率,误报数,误报率," +
                        //"无锡,少锡,多锡,高度偏高,高度偏底,面积偏多,面积偏少,X偏移,Y偏移,短路,锡型不良,污点,共面,短路,焊盘面积错误 ," +
                        //" ,待机时间,运行时间,停机时间,故障时间                   
                    }
                    else if (dStartTime.ToString("HH:mm:ss").Equals("12:00:00"))
                    {
                        dEndTime = dStartTime.AddHours(8);
                        if (dtPCB != null && dtPCB.Rows.Count > 0)
                        {
                            for (int x = 0; x < dtPCB.Rows.Count; x++)
                            {
                                if ((DateTime)dtPCB.Rows[x][1] >= dStartTime && (DateTime)dtPCB.Rows[x][2] < dEndTime)
                                {
                                    iPcbCount++;
                                    if (int.Parse(dtPCB.Rows[x][5].ToString()) == 0 || int.Parse(dtPCB.Rows[x][5].ToString()) == 2)
                                    {
                                        iGoodCount++;
                                    }
                                    jobName = (string)dtPCB.Rows[x][4];
                                    sLineNo = (string)dtPCB.Rows[x][3];
                                }
                            }

                        }
                        bFileIs12 = true;
                    }
                    else if (dStartTime.ToString("HH:mm:ss").Equals("20:00:00"))
                    {
                        dEndTime = dStartTime.AddHours(12);
                        if (dtPCB != null && dtPCB.Rows.Count > 0)
                        {
                            for (int x = 0; x < dtPCB.Rows.Count; x++)
                            {
                                if ((DateTime)dtPCB.Rows[x][1] >= dStartTime && (DateTime)dtPCB.Rows[x][2] < dEndTime)
                                {
                                    iPcbCount++;
                                    if (int.Parse(dtPCB.Rows[x][5].ToString()) == 0 || int.Parse(dtPCB.Rows[x][5].ToString()) == 2)
                                    {
                                        iGoodCount++;
                                    }
                                    jobName = (string)dtPCB.Rows[x][4];
                                    sLineNo = (string)dtPCB.Rows[x][3];
                                }
                            }

                        }
                        bFileIs20 = true;
                    }
                    if (iPcbCount > 0)
                    {
                        getErrorTypeCount(dtPad, dStartTime, dEndTime, iMissing, iInsufficient, iExcess, iOverHeight,
                               iLowHeight, iOverArea, iLowArea, iShiftX, iShiftY, iBridge, iShapeError, iSmeared, iCoplanarity, iPreBridge, iPadAreaError,
                               iAcount, sLineNo, jobName, iPcbCount, iGoodCount, ref strBld);
                    }
                    if (strBld != null)
                    {
                        using (FileStream sf = new FileStream(sFilePath, FileMode.Append))
                        {
                            StreamWriter sw = new StreamWriter(sf);
                            sw.Write(strBld);
                            sw.Close();
                        }
                    }
                    dStartTime = dStartTime.AddHours(4);
                }
            }
            catch (Exception ex)
            {
                string error = "  Function: (AutoStartJufei)  数据导出出错 ";
                //log.WriteErr("错误 ! " + error + ex.Message);
                //AppLogHelp.WriteError(LogFileFormate.AppMain, ThreadJFCNN + ex.ToString());
            }
            finally
            {
            }
        }
        // iAcount, sLineNo, jobName, iPcbCount, iGoodCount);
        public static void getErrorTypeCount(DataTable[] dtPad, DateTime dStartTime, DateTime dEndTime, int iMissing, int iInsufficient, int iExcess, int iOverHeight,
                           int iLowHeight, int iOverArea, int iLowArea, int iShiftX, int iShiftY,
                           int iBridge, int iShapeError, int iSmeared, int iCoplanarity, int iPreBridge,
                           int iPadAreaError, int iAcount, string sLineNo, string jobName, int iPcbCount, int iGoodCount, ref System.Text.StringBuilder strBld)
        {
            try
            {
                if (dtPad != null && dtPad.Length > 0)
                {
                    for (int p = 0; p < dtPad.Length; p++)
                    {
                        if (dtPad[p] != null && dtPad[p].Rows.Count > 0)
                        {
                            for (int k = 0; k < dtPad[p].Rows.Count; k++)
                            {
                                DateTime a = (DateTime)dtPad[p].Rows[k][1]; DateTime b = (DateTime)dtPad[p].Rows[k][2]; int c = int.Parse((dtPad[p].Rows[k][3].ToString()));
                                if (dStartTime <= (DateTime)dtPad[p].Rows[k][1] && dEndTime > (DateTime)dtPad[p].Rows[k][2] && int.Parse(dtPad[p].Rows[k][3].ToString()) == 1)
                                {
                                    switch (int.Parse((dtPad[p].Rows[k][0].ToString())))
                                    {
                                        case 0:
                                            iMissing++;
                                            break;
                                        case 1:
                                            iInsufficient++;
                                            break;
                                        case 2:
                                            iExcess++;
                                            break;
                                        case 3:
                                            iOverHeight++;
                                            break;
                                        case 4:
                                            iLowHeight++;
                                            break;
                                        case 5:
                                            iOverArea++;
                                            break;
                                        case 6:
                                            iLowArea++;
                                            break;
                                        case 7:
                                            iShiftX++;
                                            break;
                                        case 8:
                                            iShiftY++;
                                            break;
                                        case 9:
                                            iBridge++;
                                            break;
                                        case 10:
                                            iShapeError++;
                                            break;
                                        case 11:
                                            iSmeared++;
                                            break;
                                        case 12:
                                            iCoplanarity++;
                                            break;
                                        case 13:
                                            iPreBridge++;
                                            break;
                                        case 14:
                                            iPadAreaError++;
                                            break;
                                        default:
                                            break;

                                    }
                                }
                            }
                        }
                    }
                }
                strBld.Append(iAcount + JUFEI.JF_Spit
                                     + dStartTime.ToString(JUFEI.JF_DATETIME_FORMAT) + JUFEI.JF_Spit
                                     + dEndTime.ToString(JUFEI.JF_DATETIME_FORMAT) + JUFEI.JF_Spit
                                     + sLineNo + JUFEI.JF_Spit
                                     + jobName + JUFEI.JF_Spit
                                     + iPcbCount + JUFEI.JF_Spit
                                     + iGoodCount + JUFEI.JF_Spit
                                     + ((double)iGoodCount / (double)iPcbCount) * 100 + "%" + JUFEI.JF_Spit
                                     + (1.0 - (double)iGoodCount / (double)iPcbCount) * 100 + "%" + JUFEI.JF_Spit
                                     + "0" + JUFEI.JF_Spit
                                     + "0" + JUFEI.JF_Spit + JUFEI.JF_Spit + JUFEI.JF_Spit + JUFEI.JF_Spit + JUFEI.JF_Spit
                                     + iMissing + JUFEI.JF_Spit
                                     + iInsufficient + JUFEI.JF_Spit
                                     + iExcess + JUFEI.JF_Spit
                                     + iOverHeight + JUFEI.JF_Spit
                                     + iLowHeight + JUFEI.JF_Spit
                                     + iOverArea + JUFEI.JF_Spit
                                     + iLowArea + JUFEI.JF_Spit
                                     + iShiftX + JUFEI.JF_Spit
                                     + iShiftY + JUFEI.JF_Spit
                                     + iBridge + JUFEI.JF_Spit
                                     + iShapeError + JUFEI.JF_Spit
                                     + iSmeared + JUFEI.JF_Spit
                                     + iCoplanarity + JUFEI.JF_Spit
                                     + iPreBridge + JUFEI.JF_Spit
                                     + iPadAreaError + JUFEI.JF_Spit + JUFEI.JF_Spit
                                     + "0" + JUFEI.JF_Spit
                                     + "0" + JUFEI.JF_Spit
                                     + "0" + JUFEI.JF_Spit
                                     + "0" + JUFEI.JF_EndLine

                                     );
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region "成都富士康输出deFault"

        /// <summary>
        /// 成都富士康线程开启
        /// </summary>
        /// <param name="log"></param>
        public void AutoStartCdFox( InspectConfig.ConfigData _Config)
        {
            try
            {
                runnerCDfox = new ThreadProcessCDFO( _Config);
                runnerCDfox.Run();
            }
            catch (Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.ToString(), ThreadNameCDCNN);
                AppLogHelp.WriteError(LogFileFormate.AppMain, ThreadNameCDCNN + ex.ToString());
            }

        } 
        /// <summary>
        /// 导出成都富士康  .csv
        /// </summary>
        /// <param name="AsMySqlConnectionString"></param>
        /// <param name="nextPcbId"></param>
        /// <param name="strLastPcbPath"></param>
        /// <param name="strLastPcbFile"></param>
        /// <param name="strLastAppsettingTmpFile"></param>
        /// <param name="strAppsettingPath"></param>
        /// <param name="appSettingHandle"></param>
        /// <param name="timer"></param>
        /// <param name="log"></param>
        public static void SaveFileForCdFox( 
            string AsMySqlConnectionString,
            string strLastPcbPath, string strLastPcbFile, string strLastAppsettingTmpFile, string strAppsettingPath,
            AppLayerLib.AppSettingHandler appSettingHandle,
            int timer, InspectConfig.ConfigData _ConfigAl)
        {         
            
                string sqlPadInfoSec = " SELECT  " +
                                    "pad.PadID, " +
                                    "0," +//"padm.ComponentID," +
                                    "0," +//"padm.PackageType," +
                                    "pad.PerArea," +
                                    "pad.ABSHeight," +
                                    "pad.PerVolume," +
                                    "pad.ShiftX," +
                                    "pad.ShiftY," +
                                    "0," +//"padm.SizeXmm," +
                                    "0," +//"padm.SizeYmm," +
                                    "pad.ABSArea," +
                                    "pad.PerHeight," +
                                    "pad.ABSVolume," +
                                    "pad.JudgeRes," +
                                    "pad.DefectType," +
                                    "0," +//"padm.PinNumber," +
                                    "0," +//"padm.ArrayID, " +
                                    "0," +//"padC.AreaU, " +
                                    "0," +//"padC.AreaL," +
                                    "0," +//"padC.VolU," +
                                    "0," +//"padC.VolL," +
                                    "0," +//"padC.HeightU," +
                                    "0," +//"padC.HeightL," +
                                    "0," +//"padC.ShiftXU," +
                                    "0," +//"padC.ShiftYU," +
                                    "0," +//"padC.ShiftDataType, " +
                                    "0," +//"padm.PosXmm, " +
                                    "0," +//"padm.PosYmm, " +
                                    "0," +//"padC.BridgeWidth, " +
                                    "0," +//"padC.BridgeLength, " +
                                    "0 " +//"padC.BridgeHeight " +
                    //"  FROM  " +
                    //" spidb.TBPadMeasure pad,spidb.TBSimplePad padm,spidb.TBPadConditionParams padC  " +
                    //" WHERE " +
                    //" pad.PCBID = '{0}' AND pad.JudgeRes IN(1,2) AND " +
                    //" pad.PadID = padm.PadID AND padm.JobIndex = pad.JobIndex  AND " +
                    //" padm.PadCndtParamsIndex = padC.IndexSerNo AND padm.JobIndex = padC.JobIndex   ";
                                    "  FROM  " +
                                    " spidb.TBPadMeasure pad " +//pad,spidb.TBSimplePad padm,spidb.TBPadConditionParams padC  " +
                                    " WHERE " +
                                    " pad.PCBID = '{0}' ";//AND pad.JudgeRes IN(1,2)   ";
                string sqlSimplePadInfoSec = " SELECT  " +
                                    "padm.ComponentID," +
                                    "padm.PackageType," +
                                    "padm.SizeXmm," +
                                    "padm.SizeYmm," +
                                    "padm.PinNumber," +
                                    "padm.ArrayID, " +
                                    "padm.PosXmm, " +
                                    "padm.PosYmm, " +
                                    //"padm.PadCndtParamsIndex  " +
                                    "padC.AreaU, " +
                                    "padC.AreaL," +
                                    "padC.VolU," +
                                    "padC.VolL," +
                                    "padC.HeightU," +
                                    "padC.HeightL," +
                                    "padC.ShiftXU," +
                                    "padC.ShiftYU," +
                                    "padC.ShiftDataType, " +
                    //"" +//"padm.PosXmm, " +
                    //"" +//"padm.PosYmm, " +
                                    "padC.BridgeWidth, " +
                                    "padC.BridgeLength, " +
                                    "padC.BridgeHeight " +

                                    "  FROM  " +
                                    " spidb.TBSimplePad padm,spidb.TBPadConditionParams padC " +
                                    " WHERE " +
                                    " padm.PadCndtParamsIndex = padC.IndexSerNo AND padm.JobIndex = padC.JobIndex AND" +
                                    " padm.JobIndex = '{0}' GROUP BY padm.PadID  ";
                string sqlSimplePadInfo = string.Empty;
                string sqlPadConditionParamsInfoSec = " SELECT  " +
                                    //"pad.PadID, " +
                                    "" +//"padm.ComponentID," +
                    //"padm.PackageType," +
                                    //"pad.PerArea," +
                                    //"pad.ABSHeight," +
                                    //"pad.PerVolume," +
                                    //"pad.ShiftX," +
                                    //"pad.ShiftY," +
                                    //"" +//"padm.SizeXmm," +
                                    //"" +//"padm.SizeYmm," +
                                    //"pad.ABSArea," +
                                    //"pad.PerHeight," +
                                    //"pad.ABSVolume," +
                                    //"pad.JudgeRes," +
                                    //"pad.DefectType," +
                                    //"" +//"padm.PinNumber," +
                                    //"" +//"padm.ArrayID, " +
                                    "padC.AreaU, " +
                                    "padC.AreaL," +
                                    "padC.VolU," +
                                    "padC.VolL," +
                                    "padC.HeightU," +
                                    "padC.HeightL," +
                                    "padC.ShiftXU," +
                                    "padC.ShiftYU," +
                                    "padC.ShiftDataType, " +
                                    //"" +//"padm.PosXmm, " +
                                    //"" +//"padm.PosYmm, " +
                                    "padC.BridgeWidth, " +
                                    "padC.BridgeLength, " +
                                    "padC.BridgeHeight " +
                    //"  FROM  " +
                    //" spidb.TBPadMeasure pad,spidb.TBSimplePad padm,spidb.TBPadConditionParams padC  " +
                    //" WHERE " +
                    //" pad.PCBID = '{0}' AND pad.JudgeRes IN(1,2) AND " +
                    //" pad.PadID = padm.PadID AND padm.JobIndex = pad.JobIndex  AND " +
                    //" padm.PadCndtParamsIndex = padC.IndexSerNo AND padm.JobIndex = padC.JobIndex   ";
                                    "  FROM  " +
                                    " spidb.TBPadConditionParams padC" +//pad,spidb.TBSimplePad padm,spidb.TBPadConditionParams padC  " +
                                    " WHERE " +
                                    " padC.IndexSerNo = '{0}' AND padC.JobIndex = '{1}'";//AND pad.JudgeRes IN(1,2)   ";
                string sqlPadConditionParamsInfo = string.Empty;
                while (true)
                {
                    try
                    {
                        string nextPcb = string.Empty;
                        if (File.Exists(strLastPcbFile))
                        {
                            using (StreamReader srMain = new StreamReader(strLastPcbFile))
                            {
                                string str = "";
                                if ((str = srMain.ReadLine()) != null)
                                {
                                    nextPcb = str;
                                }
                            }
                        }
                        string sql = "SELECT t.PCBID,t.StartTime,t.PCBBarcode,t.JobIndex,t.Result,t.lineNo,job.JobName FROM spidb.TBBoard t INNER JOIN spidb.TBJobInfo job ON t.JobIndex = job.SerNo WHERE t.PCBID >  '" + nextPcb + "'" + "AND t.OPConfirmed ='1'   ORDER BY t.PCBID ASC";

                        DataTable dt = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            //wait the review station confirm
                            //log.WriteLog(DateTime.Now.ToString(RS_FORMAT_DATETIME), "CDFOX start..");
                            System.Threading.Thread.Sleep(1500);
                            dt = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, sql);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i][0] != null)
                                {
                                    string path = strAppsettingPath;
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
                                    string sAppSettingFileTmp = strAppsettingPath + "\\" + max + ".bin";
                                    if (File.Exists(sAppSettingFileTmp))
                                    {
                                        appSettingHandle.Read(sAppSettingFileTmp, _ConfigAl);
                                        //timer = appSettingHandle._appSettingData.stDataExpVT.IntervalSecond;       //等待间隔时间
                                        //储存本次pcbID至   strLastAppsettingTmpFile
                                        using (FileStream fsApp = new FileStream(strLastAppsettingTmpFile, FileMode.Create))
                                        {
                                            StreamWriter swApp = new StreamWriter(fsApp, System.Text.Encoding.Default);
                                            swApp.Write(dt.Rows[i][0].ToString());
                                            swApp.Close();
                                        }
                                    }
                                    string jobIndex = dt.Rows[i][3].ToString();
                                    string resultPCB = dt.Rows[i][4].ToString();
                                    if (resultPCB == "1") resultPCB = "Fail";
                                    else if (resultPCB == "0") resultPCB = "Good";
                                    else if (resultPCB == "2") resultPCB = "Pass";
                                    else resultPCB = "Fail";
                                    string lineName = dt.Rows[i][5].ToString();
                                    string sqlPadInfo = string.Format(sqlPadInfoSec, dt.Rows[i][0].ToString());
                                    sqlSimplePadInfo = string.Format(sqlSimplePadInfoSec, jobIndex);
                                    //" ,spidb.TBSimplePad padm " +       
                                    //" INNER JOIN spidb.TBSimplePad padm ON pad.PadID = padm.PadID AND padm.JobIndex = pad.JobIndex  " +
                                    //" INNER JOIN spidb.TBPadConditionParams padC ON padm.PadCndtParamsIndex = padC.IndexSerNo AND padm.JobIndex = padC.JobIndex  " +
                                    //" WHERE " +
                                    //" pad.PCBID = '"+ dt.Rows[i][0].ToString() + "'";//And padm.JobIndex = " + jobIndex;
                                    string strDate = ((DateTime)dt.Rows[i][1]).ToString("yyyyMMdd");
                                    string strTime = ((DateTime)dt.Rows[i][1]).ToString("HHmmss");
                                    string strDateTime = ((DateTime)dt.Rows[i][1]).ToString("yyyyMMdd HH:mm:ss");
                                    string barCode = dt.Rows[i][2].ToString();
                                    string jobName = dt.Rows[i][6].ToString();
                                    DataTable dsqlDefult = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, sqlPadInfo);
                                    DataTable dsqSamplePad = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, sqlSimplePadInfo);
                                    PadInfo[] pads;
                                    if (dsqlDefult != null && dsqlDefult.Rows.Count > 0 && dsqSamplePad != null && dsqSamplePad.Rows.Count > 0)
                                    {
                                        pads = new PadInfo[dsqlDefult.Rows.Count];
                                        for (int j = 0; j < dsqlDefult.Rows.Count; j++)
                                        {
                                            string strType = dsqlDefult.Rows[j][25].ToString();
                                            pads[j] = new PadInfo();
                                            //pcbid
                                            pads[j].padID = dsqlDefult.Rows[j][0].ToString();
                                            //commId
                                            pads[j].component = dsqSamplePad.Rows[j][0].ToString();
                                            //type
                                            pads[j].Type = dsqSamplePad.Rows[j][1].ToString();
                                            //perArea
                                            pads[j].perArea = dsqlDefult.Rows[j][3].ToString();
                                            //height
                                            pads[j].height = dsqlDefult.Rows[j][4].ToString();
                                            //perVolume
                                            pads[j].perVolume = dsqlDefult.Rows[j][5].ToString();
                                            //XOffset
                                            pads[j].xOffset = dsqlDefult.Rows[j][6].ToString();
                                            //YOffset
                                            pads[j].yOffset = dsqlDefult.Rows[j][7].ToString();
                                            //PadSize(X)
                                            pads[j].padSizeX = dsqSamplePad.Rows[j][2].ToString();
                                            //PadSize(Y)
                                            pads[j].padSizeY = dsqSamplePad.Rows[j][3].ToString();
                                            //Area
                                            pads[j].area = dsqlDefult.Rows[j][10].ToString();
                                            //perHeight
                                            pads[j].perHeight = dsqlDefult.Rows[j][11].ToString();
                                            //Volume
                                            pads[j].volume = dsqlDefult.Rows[j][12].ToString();
                                            //Result                                                                               
                                            pads[j].result = dsqlDefult.Rows[j][13].ToString();
                                            //Errcode
                                            pads[j].errcode = dsqlDefult.Rows[j][14].ToString();
                                            //PinNum
                                            pads[j].pinNum = dsqSamplePad.Rows[j][4].ToString();
                                            //Barcode
                                            pads[j].barcode = barCode;
                                            //Date
                                            pads[j].date = strDate;
                                            //Time
                                            pads[j].time = strTime;
                                            //ArrayID
                                            pads[j].arrayID = dsqSamplePad.Rows[j][5].ToString();
                                            pads[j].areaH = dsqSamplePad.Rows[j][8].ToString();
                                            pads[j].areaL = dsqSamplePad.Rows[j][9].ToString();
                                            pads[j].volumeH = dsqSamplePad.Rows[j][10].ToString();
                                            pads[j].volumeL = dsqSamplePad.Rows[j][11].ToString();
                                            pads[j].heightH = dsqSamplePad.Rows[j][12].ToString();
                                            pads[j].heightL = dsqSamplePad.Rows[j][13].ToString();
                                            if (strType == "0")
                                            {
                                                pads[j].xOffsetH = dsqSamplePad.Rows[j][14].ToString();
                                                pads[j].yOffsetH = dsqSamplePad.Rows[j][15].ToString();
                                            }
                                            else
                                            {
                                                pads[j].xOffsetH = "0";
                                                pads[j].yOffsetH = "0";
                                            }
                                            pads[j].shiftDataType = dsqSamplePad.Rows[j][16].ToString();
                                            pads[j].posXmm = dsqSamplePad.Rows[j][6].ToString();
                                            pads[j].posYmm = dsqSamplePad.Rows[j][7].ToString();
                                            pads[j].bridgeWidth = dsqSamplePad.Rows[j][17].ToString();
                                            pads[j].bridgeLength = dsqSamplePad.Rows[j][18].ToString();
                                            pads[j].bridgeHeight = dsqSamplePad.Rows[j][19].ToString();
                                        }
                                        //导出PCB title-------0
                                        System.Text.StringBuilder strBld = new System.Text.StringBuilder();

                                        //头文件
                                        strBld.Append("Model name" + "," + jobName + "\r\n"
                                                      + "Line number" + "," + lineName + "\r\n"
                                                      + "Board Status" + "," + resultPCB + "\r\n"
                                                      + "Barcode" + "," + barCode + "\r\n"
                                                      + "Date time" + "," + strDateTime + "\r\n");
                                        // padTile
                                        bool[] padTitle = appSettingHandle._appSettingData.stDataExpVT.bPadExportedItems;
                                        //
                                        getStrBuldForDefultTitle(padTitle, ref strBld);
                                        string filePath = appSettingHandle._appSettingData.stDataExpVT.strBackUpExportedFilePath;
                                        //if (pads != null && pads.Length >0)
                                        //{
                                        if (string.IsNullOrEmpty(barCode)) barCode = "NOREAD";//dt.Rows[i][0].ToString();
                                        string file = Path.Combine(filePath, barCode + ((DateTime)dt.Rows[i][1]).ToString("yyyyMMddHHmmss") + ".csv");
                                        if (resultPCB != "Good")
                                        {
                                            getStrBuldForSaveExportForData(pads, padTitle, resultPCB, ref strBld);
                                        }
                                        //log.WriteErr(file + "-" + pads + "-" + padTitle.Length + "---" + strBld);
                                        if (Directory.Exists(filePath) == false)
                                        {
                                            Directory.CreateDirectory(filePath);
                                        }
                                        using (FileStream fsPads = new FileStream(file, FileMode.Create))
                                        {
                                            StreamWriter srPads = new StreamWriter(fsPads, System.Text.Encoding.Default);
                                            srPads.Write(strBld);
                                            srPads.Close();
                                            //log.WriteLog("Function Success ! 导出csv文件成功 !","CDFOX");
                                            AppLogHelp.WriteLog(LogFileFormate.MES, "CDFOX Function Success ! 导出csv文件成功 !");
                                            //log.WriteLog(DateTime.Now.ToString(RS_FORMAT_DATETIME), "CDFOX end..");
                                        }
                                        //}
                                    }
                                    //储存本次pcbID至 strLastPcbFile 以便下次读取
                                    using (FileStream fsPcb = new FileStream(strLastPcbFile, FileMode.Create))
                                    {
                                        StreamWriter swPcb = new StreamWriter(fsPcb, System.Text.Encoding.Default);
                                        swPcb.Write(dt.Rows[i][0].ToString());
                                        swPcb.Close();
                                    }
                                }
                            }
                            GC.Collect();
                        }
                        else
                        {

                            System.Threading.Thread.Sleep(100);
                            //break;
                        }
                    }
                    catch (Exception ex)
                    {
                        string error = "  Function: (SaveFileForCdFox)  数据导出出错 ";
                        //log.WriteErr("错误 ! " + error + ex.ToString());
                        AppLogHelp.WriteError(LogFileFormate.MES, "CDFOX Function Success ! (SaveFileForCdFox)  数据导出出错" + ex.ToString());
                        continue;
                    }
                }
            
        }

        //get the padmode values

        public static void getStrBuldForSaveExportForData(PadInfo[] APads, bool[] padTitle,string resultPCB, ref System.Text.StringBuilder strBld)
        {
            if (resultPCB == "Fail" || resultPCB == "Pass")
            {
                for (int i = 0; i < APads.Length; i++)
                {
                    //  PadID,Component,Type,Area(%),Height,Volume(%),XOffset,YOffset,
                    //  PadSize(X),PadSize(Y),Area,Height(%),Volume,Result,Errcode,PinNum,
                    //  Barcode,Date,Time,ArrayID
                    // Console.WriteLine(" pad values coming!");
                    //  PadID                   
                    if (APads[i].result != "0")
                    {
                        if (padTitle[0] != false)
                        {
                            strBld.Append(APads[i].padID + CDFOX.RS_CD_Spit);
                        }
                        //Component
                        if (padTitle[1] != false)
                        {
                            strBld.Append(APads[i].component + CDFOX.RS_CD_Spit);
                        }
                        //Type
                        if (padTitle[2] != false)
                        {
                            strBld.Append(APads[i].Type + CDFOX.RS_CD_Spit);
                        }
                        //Area(%)
                        if (padTitle[3] != false)
                        {
                            strBld.Append(APads[i].perArea + CDFOX.RS_CD_Spit);
                        }
                        //Height
                        if (padTitle[4] != false)
                        {
                            strBld.Append(APads[i].height + CDFOX.RS_CD_Spit);
                        }
                        //Volume(%)
                        if (padTitle[5] != false)
                        {
                            strBld.Append(APads[i].perVolume + CDFOX.RS_CD_Spit);
                        }
                        //XOffset
                        if (padTitle[6] != false)
                        {
                            strBld.Append(APads[i].xOffset + CDFOX.RS_CD_Spit);
                        }
                        //YOffset
                        if (padTitle[7] != false)
                        {
                            strBld.Append(APads[i].yOffset + CDFOX.RS_CD_Spit);
                        }
                        //PadSize(X)
                        if (padTitle[8] != false)
                        {
                            strBld.Append(APads[i].padSizeX + CDFOX.RS_CD_Spit);
                        }
                        //PadSize(Y)
                        if (padTitle[9] != false)
                        {
                            strBld.Append(APads[i].padSizeY + CDFOX.RS_CD_Spit);
                        }
                        //Area
                        if (padTitle[10] != false)
                        {
                            strBld.Append(APads[i].area + CDFOX.RS_CD_Spit);
                        }
                        //Height(%)
                        if (padTitle[11] != false)
                        {
                            strBld.Append(APads[i].perHeight + CDFOX.RS_CD_Spit);
                        }
                        //Volume
                        if (padTitle[12] != false)
                        {
                            strBld.Append(APads[i].volume + CDFOX.RS_CD_Spit);
                        }
                        //Result
                        //if (padTitle[13] !=false )
                        //{
                        string padResult = APads[i].result;
                        if (padResult == "1") padResult = "Fail";
                        if (padResult == "2") padResult = "Pass";
                        strBld.Append(padResult + CDFOX.RS_CD_Spit);
                        //}
                        //Errcode
                        if (padTitle[14] != false)
                        {
                            strBld.Append(APads[i].errcode + CDFOX.RS_CD_Spit);

                        }
                        //PinNum
                        if (padTitle[15] != false)
                        {
                            strBld.Append(APads[i].pinNum + CDFOX.RS_CD_Spit);
                        }
                        //Barcode
                        if (padTitle[16] != false)
                        {
                            strBld.Append(APads[i].barcode + CDFOX.RS_CD_Spit);
                        }
                        //Date
                        if (padTitle[17] != false)
                        {
                            strBld.Append(APads[i].date + CDFOX.RS_CD_Spit);
                        }
                        //Time
                        if (padTitle[18] != false)
                        {
                            strBld.Append(APads[i].time + CDFOX.RS_CD_Spit);
                        }
                        //ArrayID
                        if (padTitle[19] != false)
                        {
                            strBld.Append(APads[i].arrayID + CDFOX.RS_CD_Spit);
                        }
                        //AreaH
                        if (padTitle[20] == true)
                        {
                            strBld.Append(APads[i].areaH + CDFOX.RS_CD_Spit);
                            //if (APads[i].res.jugRes != JudgeRes.Good)
                            //strPld.Append(APads[i].strArrayID + RS_SPLIT);
                        }
                        //AreaL
                        if (padTitle[21] == true)
                        {
                            strBld.Append(APads[i].areaL + CDFOX.RS_CD_Spit);

                        }
                        //VolumH
                        if (padTitle[22] == true)
                        {
                            strBld.Append(APads[i].volumeH + CDFOX.RS_CD_Spit);
                        }
                        //VolumL
                        if (padTitle[23] == true)
                        {
                            strBld.Append(APads[i].volumeL + CDFOX.RS_CD_Spit);
                            //if (APads[i].res.jugRes != JudgeRes.Good)
                        }
                        //HeightH
                        if (padTitle[24] == true)
                        {
                            strBld.Append(APads[i].heightH + CDFOX.RS_CD_Spit);
                            //if (APads[i].res.jugRes != JudgeRes.Good)
                        }
                        //HeightL
                        if (padTitle[25] == true)
                        {
                            strBld.Append(APads[i].heightL + CDFOX.RS_CD_Spit);
                            //if (APads[i].res.jugRes != JudgeRes.Good)
                        }
                        
                        //ShiftX
                        if (padTitle[26] == true)
                        {
                            strBld.Append(APads[i].xOffsetH + CDFOX.RS_CD_Spit);
                        }
                        //ShiftY
                        if (padTitle[27] == true)
                        {
                            strBld.Append(APads[i].yOffsetH + CDFOX.RS_CD_Spit);
                        }
                        //padGroup
                        if (padTitle[28] == true)
                        {
                            strBld.Append(APads[i].padGroup + CDFOX.RS_CD_Spit);
                        }
                        //PosXmm
                        if (padTitle[29] == true)
                        {
                            strBld.Append(APads[i].posXmm + CDFOX.RS_CD_Spit);
                        }
                        //PosYmm
                        if (padTitle[30] == true)
                        {
                            strBld.Append(APads[i].posYmm + CDFOX.RS_CD_Spit);
                        }
                        //BridgeWidth
                        if (padTitle[31] == true)
                        {
                            strBld.Append(APads[i].bridgeWidth + CDFOX.RS_CD_Spit);
                        }
                        //BridgeLength
                        if (padTitle[32] == true)
                        {
                            strBld.Append(APads[i].bridgeLength + CDFOX.RS_CD_Spit);
                        }
                        //BridgeHeight
                        if (padTitle[33] == true)
                        {
                            strBld.Append(APads[i].bridgeHeight + CDFOX.RS_CD_Spit);
                        }
                        strBld.Append(CDFOX.RS_END_LINE);
                    }
                }
            }

        }
        //get title
        public static void getStrBuldForDefultTitle(bool[] padTitle,ref System.Text.StringBuilder strBld)
        {
               // "PadID,Component,Type,Area(%),Height,Volume(%),XOffset,YOffset,PadSize(X),PadSize(Y),Area,Height(%),Volume,Result,Errcode,PinNum,Barcode,Date,Time,ArrayID" + CDFOX.RS_END_LINE
                //  PadID
            
                if (padTitle[0] != false)
                {
                    strBld.Append("PadID" + CDFOX.RS_CD_Spit);
                }
                //Component
                if (padTitle[1] !=false )
                {
                    strBld.Append("Component" + CDFOX.RS_CD_Spit);
                }
                //Type
                if (padTitle[2] !=false  )
                {
                    strBld.Append("Type" + CDFOX.RS_CD_Spit);
                }
                //Area(%)
                if (padTitle[3] !=false )
                {
                    strBld.Append("Area(%)" + CDFOX.RS_CD_Spit);
                }
                //Height
                if (padTitle[4] !=false  )
                {
                    strBld.Append("Height" + CDFOX.RS_CD_Spit);
                }
                //Volume(%)
                if (padTitle[5] !=false  )
                {
                    strBld.Append("Volume(%)" + CDFOX.RS_CD_Spit);
                }
                //XOffset
                if (padTitle[6] !=false  )
                {
                    strBld.Append("XOffset"+ CDFOX.RS_CD_Spit);
                }
                //YOffset
                if (padTitle[7] !=false  )
                {
                    strBld.Append("YOffset" + CDFOX.RS_CD_Spit);
                }
                //PadSize(X)
                if (padTitle[8] !=false  )
                {
                    strBld.Append("PadSize(X)" + CDFOX.RS_CD_Spit);
                }
                //PadSize(Y)
                if (padTitle[9] !=false  )
                {
                    strBld.Append("PadSize(Y)" + CDFOX.RS_CD_Spit);
                }
                //Area
                if (padTitle[10] !=false  )
                {
                    strBld.Append("Area" + CDFOX.RS_CD_Spit);
                }
                //Height(%)
                if (padTitle[11] !=false )
                {
                    strBld.Append("Height(%)" + CDFOX.RS_CD_Spit);
                }
                //Volume
                if (padTitle[12] !=false )
                {
                    strBld.Append("Volume" + CDFOX.RS_CD_Spit);
                }
                //Result
                //if (padTitle[13] !=false  || )
                //{
                    strBld.Append("Result" + CDFOX.RS_CD_Spit);
                //}
                //Errcode
                if (padTitle[14] !=false )
                {
                    strBld.Append("Errcode" + CDFOX.RS_CD_Spit);

                }
                //PinNum
                if (padTitle[15] !=false )
                {
                    strBld.Append("PinNum" + CDFOX.RS_CD_Spit);
                }
                //Barcode
                if (padTitle[16] !=false )
                {
                    strBld.Append("Barcode" + CDFOX.RS_CD_Spit);
                }
                //Date
                if (padTitle[17] !=false  )
                {
                    strBld.Append("Date" + CDFOX.RS_CD_Spit);
                }
                //Time
                if (padTitle[18] !=false )
                {
                    strBld.Append("Time" + CDFOX.RS_CD_Spit);
                }
                //ArrayID
                if (padTitle[19] !=false )
                {
                    strBld.Append("ArrayID" + CDFOX.RS_CD_Spit);
                }
                if (padTitle[20] == true)
                {
                    strBld.Append("AreaU" + CDFOX.RS_CD_Spit);
                }
                //AreaL
                if (padTitle[21] == true)
                {
                    strBld.Append("AreaL" + CDFOX.RS_CD_Spit);
                }
                //VolumeU
                if (padTitle[22] == true)
                {
                    strBld.Append("VolumeU" + CDFOX.RS_CD_Spit);
                }
                //VolumeL
                if (padTitle[23] == true)
                {
                    strBld.Append("VolumeL" + CDFOX.RS_CD_Spit);
                }
                //HeightU
                if (padTitle[24] == true)
                {
                    strBld.Append("HeightU" + CDFOX.RS_CD_Spit);
                }
                //HeightL
                if (padTitle[25] == true)
                {
                    strBld.Append("HeightL" + CDFOX.RS_CD_Spit);
                }
                //XOffsetH
                if (padTitle[26] == true)
                {
                    strBld.Append("XOffsetH" + CDFOX.RS_CD_Spit);
                }
                //YOffset
                if (padTitle[27] == true)
                {
                    strBld.Append("YOffsetH" + CDFOX.RS_CD_Spit);
                }
                //padGroup
                if (padTitle[28] == true)
                {
                    strBld.Append("PadGroup" + CDFOX.RS_CD_Spit);
                }
            //add Peng 20180919
                if (padTitle[29] == true)
                {
                    strBld.Append("PosXmm" + CDFOX.RS_CD_Spit);
                }
                if (padTitle[30] == true)
                {
                    strBld.Append("PosYmm" + CDFOX.RS_CD_Spit);
                }
                if (padTitle[31] == true)
                {
                    strBld.Append("BridgeWidth" + CDFOX.RS_CD_Spit);
                }
                if (padTitle[32] == true)
                {
                    strBld.Append("BridgeLength" + CDFOX.RS_CD_Spit);
                }
                if (padTitle[33] == true)
                {
                    strBld.Append("BridgeHeight" + CDFOX.RS_CD_Spit);
                }
                //面积、体积、高度、X/Y偏移、焊盘分组选项
                strBld.Append(CDFOX.RS_END_LINE);
        }
        #endregion

        #region "龙华富士康"
        /// <summary>
        ///  开启龙华富士康线程
        /// </summary>
        /// <param name="log"></param>
        public void AutoStartLHFox( string AstrRealConfigPath)
        {
            try
            {
                runnerLHfox = new ThreadProcessLHFO(  AstrRealConfigPath);
                runnerLHfox.Run();
            }
            catch (Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.ToString(), ThreadNameLHCNN);
                AppLogHelp.WriteError(LogFileFormate.MES, ex.ToString()+ ThreadNameLHCNN);
            }
        }
        /// <summary>
        ///  输出ng信息至选定路径;
        /// </summary>
        /// <param name="AsMySqlConnectionString"></param>
        /// <param name="strLastPcbPath"></param>
        /// <param name="strLastPcbFile"></param>
        /// <param name="strLastAppsettingTmpFile"></param>
        /// <param name="strAppsettingPath"></param>
        /// <param name="appSettingHandle"></param>
        /// <param name="timer"></param>
        /// <param name="log"></param>
        public static void SaveDataForLHfoxNG(string AsMySqlConnectionString,
            string strLastPcbPath, string strLastPcbFile, string strLastAppsettingTmpFile, string strAppsettingPath,
            AppLayerLib.AppSettingHandler appSettingHandle,
            int timer, string _strRealConfigPath)
        {
            try
            {
                string sqlPadInfoSec = "  SELECT  " +
                                                                "padm.ComponentID," +
                                                                "pad.DefectType," +
                                                                "padm.ArrayID, " +
                                                                "pad.PadID, " +
                                                                "padm.PinNumber, " +
                                                                "pad.ABSHeight,pad.PerVolume*100 " +
                                                            "  FROM  " +
                                                                " spidb.TBPadMeasure pad " +
                                                            "  INNER JOIN spidb.TBSimplePad padm ON pad.PadID = padm.PadID AND padm.JobIndex = pad.JobIndex " +
                                                            "  WHERE " +
                                                               " pad.PCBID = '{0}' " +
                                                               "   AND pad.JudgeRes = '1' ORDER BY padm.ArrayID";

               string sqlPadALLInfoSec  = " SELECT  " +
                                                           "pad.PadID, " +
                                                           "padm.ComponentID," +
                                                           "padm.PackageType," +
                                                           "pad.PerArea*100," +
                                                           "pad.ABSHeight," +
                                                           "pad.PerVolume*100," +
                                                           "pad.ShiftX," +
                                                           "pad.ShiftY," +
                                                           "padm.SizeXmm," +
                                                           "padm.SizeYmm," +
                                                           "pad.ABSArea," +
                                                           "pad.PerHeight*100," +
                                                           "pad.ABSVolume," +
                                                           "pad.JudgeRes," +
                                                           "pad.DefectType," +
                                                           "padm.PinNumber," +
                                                           "padm.ArrayID " +
                                                       "  FROM  " +
                                                           " spidb.TBPadMeasure pad " +
                                                       " INNER JOIN spidb.TBSimplePad padm ON pad.PadID = padm.PadID AND padm.JobIndex = pad.JobIndex " +
                                                       " WHERE " +
                                                          " pad.PCBID = '{0}' ";
                string sqlPadInfo = string.Empty;
                while (true)
                {
                    string nextPcb = string.Empty;
                    if (File.Exists(strLastPcbFile))
                    {
                        using (StreamReader srMain = new StreamReader(strLastPcbFile))
                        {
                            string str = "";
                            if ((str = srMain.ReadLine()) != null)
                            {
                                nextPcb = str;
                            }
                        }
                    }
                    string sql = "SELECT t.PCBID,t.StartTime,t.PCBBarcode,t.JobIndex,t.LineNo FROM spidb.TBBoard t WHERE t.PCBID >  '" + nextPcb + "'" + "AND t.OPConfirmed ='1'  ORDER BY t.PCBID ASC";
                    DataTable dt = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, sql );
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        System.Threading.Thread.Sleep(2000); //sleep 5s
                        dt = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, sql);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i][0].ToString() != null)
                            {
                                string path = strAppsettingPath;
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
                                string sAppSettingFileTmp = strAppsettingPath + "\\" + max + ".bin";
                                if (File.Exists(sAppSettingFileTmp))
                                {
                                    appSettingHandle.Read(sAppSettingFileTmp, _strRealConfigPath);
                                    timer = appSettingHandle._appSettingData.stDataExpVT.IntervalSecond;       //等待间隔时间
                                    //储存本次pcbID至   strLastAppsettingTmpFile
                                    using (FileStream fsApp = new FileStream(strLastAppsettingTmpFile, FileMode.Create))
                                    {
                                        StreamWriter swApp = new StreamWriter(fsApp, System.Text.Encoding.Default);
                                        swApp.Write(dt.Rows[i][0].ToString());
                                        swApp.Close();
                                    }
                                }
                                string stationName = appSettingHandle._appSettingData.stDataExpVT.strSetFOP;
                                string jobIndex = dt.Rows[i][3].ToString();
                                string strLineNo = dt.Rows[i][4].ToString();
                                string barCode = dt.Rows[i][2].ToString();
                                //appSettingHandle._appSettingData.stDataExpVT.bEnExportNGInfo = true;
                                if (appSettingHandle._appSettingData.stDataExpVT.bEnExportNGInfo)
                                {
                                    DataTable dsqlDefult;
                                    if (true)
                                    {
                                        #region "NG信息---"
                                         sqlPadInfo = string.Format(sqlPadInfoSec,dt.Rows[i][0].ToString());
                                        string strDate = ((DateTime)dt.Rows[i][1]).ToString("yyyyMMddHHmmss");
                                        string strTime = ((DateTime)dt.Rows[i][1]).ToString("HHmmss");
                                        
                                         dsqlDefult = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, sqlPadInfo);
                                        //PadInfo[] pads;
                                        string component = string.Empty, errcode = string.Empty, arrayID = string.Empty;
                                        System.Text.StringBuilder strBld = new System.Text.StringBuilder();
                                        System.Collections.ArrayList list = new System.Collections.ArrayList();
                                        if (dsqlDefult != null && dsqlDefult.Rows.Count > 0)
                                        {
                                            for (int j = 0; j < dsqlDefult.Rows.Count; j++)
                                            {
                                                //ARRAY ID
                                                arrayID = dsqlDefult.Rows[j][2].ToString();
                                                if (!list.Contains(arrayID))
                                                {
                                                    list.Add(arrayID);
                                                }
                                            }
                                        }
                                        if (list != null && list.Count > 0)
                                        {
                                            foreach (string strArrayID in list)
                                            {
                                                errcode = string.Empty; string tmpMsg = string.Empty;
                                                for (int j = 0; j < dsqlDefult.Rows.Count; j++)
                                                {
                                                    //ARRAY ID
                                                    arrayID = dsqlDefult.Rows[j][2].ToString();
                                                    string strHeight = dsqlDefult.Rows[j][5].ToString();
                                                    string strPerVomle = dsqlDefult.Rows[j][6].ToString();
                                                    if (strArrayID == arrayID)
                                                    {
                                                        //commId
                                                        component = dsqlDefult.Rows[j][0].ToString();
                                                        if (string.IsNullOrEmpty(component))
                                                        {
                                                            component = dsqlDefult.Rows[j][3].ToString() + LHFox.RS_UNDER_LINE + dsqlDefult.Rows[j][4].ToString();
                                                        }
                                                        //Errcode
                                                        errcode = GetPadErrorCodeStr(byte.Parse(dsqlDefult.Rows[j][1].ToString().Trim()), appSettingHandle);
                                                        tmpMsg += LHFox.RS_AT + component + "," + errcode + "," + strHeight + "," + strPerVomle;
                                                    }
                                                }
                                                strBld.Append(barCode + LHFox.RS_AT + stationName + LHFox.RS_AT
                                                              + arrayID + LHFox.RS_AT
                                                              + strDate
                                                              + tmpMsg + LHFox.RS_UNLINE);
                                            }
                                            string filePath = appSettingHandle._appSettingData.stDataExpVT.strBackUpExportedFilePath;

                                            if (string.IsNullOrEmpty(barCode)) barCode = dt.Rows[i][0].ToString();
                                            string file = Path.Combine(filePath, barCode + ".txt");

                                            using (FileStream fsPads = new FileStream(file, FileMode.Create))
                                            {
                                                StreamWriter srPads = new StreamWriter(fsPads, System.Text.Encoding.Default);
                                                srPads.Write(strBld);
                                                srPads.Close();
                                                //log.WriteLog("Function Success ! 导出NG txt文件成功 !", "LHFOX");
                                                AppLogHelp.WriteError(LogFileFormate.MES, "Function Success ! 导出NG txt文件成功 !");
                                            }

                                        }
                                        #endregion
                                    }
                                    if (true)
                                    {
                                        #region "所有焊盘格式信息"

                                        sqlPadInfo = string.Format(sqlPadALLInfoSec, dt.Rows[i][0].ToString());
                                        string strDate = ((DateTime)dt.Rows[i][1]).ToString("yyyyMMdd");
                                        string strTime = ((DateTime)dt.Rows[i][1]).ToString("HHmmss");
                                        string strDateTime = ((DateTime)dt.Rows[i][1]).ToString("yyyyMMddHHmmss");
                                        //string barCode = dt.Rows[i][2].ToString();
                                        dsqlDefult = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, sqlPadInfo);
                                        PadInfo[] pads;
                                        if (dsqlDefult != null && dsqlDefult.Rows.Count > 0)
                                        {
                                            pads = new PadInfo[dsqlDefult.Rows.Count];
                                            for (int j = 0; j < dsqlDefult.Rows.Count; j++)
                                            {
                                                pads[j] = new PadInfo();
                                                //pcbid
                                                pads[j].padID = dsqlDefult.Rows[j][0].ToString();
                                                //commId
                                                pads[j].component = dsqlDefult.Rows[j][1].ToString();
                                                //type
                                                pads[j].Type = dsqlDefult.Rows[j][2].ToString();
                                                //perArea
                                                pads[j].perArea = dsqlDefult.Rows[j][3].ToString();
                                                //height
                                                pads[j].height = dsqlDefult.Rows[j][4].ToString();
                                                //perVolume
                                                pads[j].perVolume = dsqlDefult.Rows[j][5].ToString();
                                                //XOffset
                                                pads[j].xOffset = dsqlDefult.Rows[j][6].ToString();
                                                //YOffset
                                                pads[j].yOffset = dsqlDefult.Rows[j][7].ToString();
                                                //PadSize(X)
                                                pads[j].padSizeX = dsqlDefult.Rows[j][8].ToString();
                                                //PadSize(Y)
                                                pads[j].padSizeY = dsqlDefult.Rows[j][9].ToString();
                                                //Area
                                                pads[j].area = dsqlDefult.Rows[j][10].ToString();
                                                //perHeight
                                                pads[j].perHeight = dsqlDefult.Rows[j][11].ToString();
                                                //Volume
                                                pads[j].volume = dsqlDefult.Rows[j][12].ToString();
                                                //Result
                                                pads[j].result = dsqlDefult.Rows[j][13].ToString();
                                                //Errcode
                                                pads[j].errcode = GetPadErrorCodeStr(byte.Parse(dsqlDefult.Rows[j][14].ToString()), appSettingHandle);
                                                //PinNum
                                                pads[j].pinNum = dsqlDefult.Rows[j][15].ToString();
                                                //Barcode
                                                pads[j].barcode = barCode;
                                                //Date
                                                pads[j].date = strDate;
                                                //Time
                                                pads[j].time = strTime;
                                                //ArrayID
                                                pads[j].arrayID = dsqlDefult.Rows[j][16].ToString();
                                            }
                                            //导出PCB title-------0
                                            System.Text.StringBuilder strBld = new System.Text.StringBuilder();
                                            string strLinePre = string.Empty;
                                            //【樓棟樓層】
                                            strLinePre = appSettingHandle._appSettingData.stDataExpVT.strCustomer;
                                            strBld.Append(strLinePre + CDFOX.RS_END_LINE);
                                            // S04	【線別】：S01，S02…		
                                            strLinePre = strLineNo;
                                            strBld.Append(strLinePre + CDFOX.RS_END_LINE);
                                            // SPI	【設備名稱】：AOI，SPI
                                            //strLinePre = "SPI";
                                            strLinePre = appSettingHandle._appSettingData.stDataExpVT.strTestType;
                                            strBld.Append(strLinePre + CDFOX.RS_END_LINE);
                                            // TRI	【設備型號】：TRI，KY，HL，JET…		
                                            strLinePre = appSettingHandle._appSettingData.stDataExpVT.strMachine;
                                            strBld.Append(strLinePre + CDFOX.RS_END_LINE);
                                            // T2001250379-001	【設備編號】：設備出廠編號
                                            strLinePre = appSettingHandle._appSettingData.stDataExpVT.strProductID;
                                            strBld.Append(strLinePre + CDFOX.RS_END_LINE);
                                            // SPI	【工站】：SPI		
                                            strLinePre = "SPI";
                                            strBld.Append(strLinePre + CDFOX.RS_END_LINE);
                                            // 20161117023736	【測試時間】
                                            strLinePre = strDateTime;
                                            strBld.Append(strLinePre + CDFOX.RS_END_LINE);
                                            // F3YGXD664705Z1	【條碼】		
                                            strBld.Append(barCode + CDFOX.RS_END_LINE);
                                            // padTile
                                            strBld.Append("ComponentID,"
                                                            + "Type,"
                                                            + "Area(%),"
                                                            + "Height,"
                                                            + "Volume(%),"
                                                            + "XOffset,"
                                                            + "YOffset,"
                                                            + "PadSize(X),"
                                                            + "PadSize(Y),"
                                                            + "Area,"
                                                            + "Height(%),"
                                                            + "Volume,"
                                                            + "Result,"
                                                            + "PinNum,"
                                                            + "Barcode,"
                                                            + "Date,"
                                                            + "Time,"
                                                            + "ArrayID" + "\r\n");
                                            string filePath = appSettingHandle._appSettingData.strDataExpPath;
                                            if (string.IsNullOrEmpty(filePath))
                                            {
                                                filePath = Path.Combine("D://EYSPI//DataExport//Res", strDate);
                                            }
                                            else
                                            {
                                                filePath = Path.Combine(filePath, strDate);
                                            }
                                            if (Directory.Exists(filePath) == false)
                                            {
                                                Directory.CreateDirectory(filePath);
                                            }
                                            if (pads != null && pads.Length > 0)
                                            {
                                                if (string.IsNullOrEmpty(barCode)) barCode = dt.Rows[i][0].ToString();
                                                string fileName = "SPI" + "_" +
                                                  appSettingHandle._appSettingData.stDataExpVT.strCustomer + strLineNo + "_" +
                                                  appSettingHandle._appSettingData.stDataExpVT.strMachine + "_" +
                                                  barCode + "_" +
                                                  strDateTime + ".csv";

                                                string file = Path.Combine(filePath, fileName );
                                                GetPadValuesForFox(pads, ref strBld);
                                                using (FileStream fsPads = new FileStream(file, FileMode.Create))
                                                {
                                                    StreamWriter srPads = new StreamWriter(fsPads, System.Text.Encoding.Default);
                                                    srPads.Write(strBld);
                                                    srPads.Close();
                                                    //log.WriteLog("Function Success ! 导出csv文件成功 !", "LHFOX");
                                                    AppLogHelp.WriteError(LogFileFormate.MES, "LHFOX Function Success ! 导出csv文件成功 !!");
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                                else
                                {
                                    //add by Peng 20180410
                                    #region "所有焊盘格式信息"

                                     sqlPadInfo = string.Format(sqlPadALLInfoSec,dt.Rows[i][0].ToString());
                                    string strDate = ((DateTime)dt.Rows[i][1]).ToString("yyyyMMdd");
                                    string strTime = ((DateTime)dt.Rows[i][1]).ToString("HHmmss");
                                    string strDateTime = ((DateTime)dt.Rows[i][1]).ToString("yyyyMMddHHmmss");
                                    //string barCode = dt.Rows[i][2].ToString();
                                    DataTable dsqlDefult = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, sqlPadInfo);
                                    PadInfo[] pads;
                                    if (dsqlDefult != null && dsqlDefult.Rows.Count > 0)
                                    {
                                        pads = new PadInfo[dsqlDefult.Rows.Count];
                                        for (int j = 0; j < dsqlDefult.Rows.Count; j++)
                                        {
                                            pads[j] = new PadInfo();
                                            //pcbid
                                            pads[j].padID = dsqlDefult.Rows[j][0].ToString();
                                            //commId
                                            pads[j].component = dsqlDefult.Rows[j][1].ToString();
                                            //type
                                            pads[j].Type = dsqlDefult.Rows[j][2].ToString();
                                            //perArea
                                            pads[j].perArea = dsqlDefult.Rows[j][3].ToString();
                                            //height
                                            pads[j].height = dsqlDefult.Rows[j][4].ToString();
                                            //perVolume
                                            pads[j].perVolume = dsqlDefult.Rows[j][5].ToString();
                                            //XOffset
                                            pads[j].xOffset = dsqlDefult.Rows[j][6].ToString();
                                            //YOffset
                                            pads[j].yOffset = dsqlDefult.Rows[j][7].ToString();
                                            //PadSize(X)
                                            pads[j].padSizeX = dsqlDefult.Rows[j][8].ToString();
                                            //PadSize(Y)
                                            pads[j].padSizeY = dsqlDefult.Rows[j][9].ToString();
                                            //Area
                                            pads[j].area = dsqlDefult.Rows[j][10].ToString();
                                            //perHeight
                                            pads[j].perHeight = dsqlDefult.Rows[j][11].ToString();
                                            //Volume
                                            pads[j].volume = dsqlDefult.Rows[j][12].ToString();
                                            //Result
                                            pads[j].result = dsqlDefult.Rows[j][13].ToString();
                                            //Errcode
                                            pads[j].errcode = GetPadErrorCodeStr(byte.Parse(dsqlDefult.Rows[j][14].ToString()), appSettingHandle);
                                            //PinNum
                                            pads[j].pinNum = dsqlDefult.Rows[j][15].ToString();
                                            //Barcode
                                            pads[j].barcode = barCode;
                                            //Date
                                            pads[j].date = strDate;
                                            //Time
                                            pads[j].time = strTime;
                                            //ArrayID
                                            pads[j].arrayID = dsqlDefult.Rows[j][16].ToString();
                                        }
                                        //导出PCB title-------0
                                        System.Text.StringBuilder strBld = new System.Text.StringBuilder();
                                        string strLinePre = string.Empty;
                                        //【樓棟樓層】
                                        strLinePre = appSettingHandle._appSettingData.stDataExpVT.strCustomer;
                                        strBld.Append(strLinePre + CDFOX.RS_END_LINE);
                                        // S04	【線別】：S01，S02…		
                                        strLinePre = strLineNo;
                                        strBld.Append(strLinePre + CDFOX.RS_END_LINE);
                                        // SPI	【設備名稱】：AOI，SPI
                                        //strLinePre = "SPI";
                                        strLinePre = appSettingHandle._appSettingData.stDataExpVT.strTestType;
                                        strBld.Append(strLinePre + CDFOX.RS_END_LINE);
                                        // TRI	【設備型號】：TRI，KY，HL，JET…		
                                        strLinePre = appSettingHandle._appSettingData.stDataExpVT.strMachine;
                                        strBld.Append(strLinePre + CDFOX.RS_END_LINE);
                                        // T2001250379-001	【設備編號】：設備出廠編號
                                        strLinePre = appSettingHandle._appSettingData.stDataExpVT.strProductID;
                                        strBld.Append(strLinePre + CDFOX.RS_END_LINE);
                                        // SPI	【工站】：SPI		
                                        strLinePre = "SPI";
                                        strBld.Append(strLinePre + CDFOX.RS_END_LINE);
                                        // 20161117023736	【測試時間】
                                        strLinePre = strDateTime;
                                        strBld.Append(strLinePre + CDFOX.RS_END_LINE);
                                        // F3YGXD664705Z1	【條碼】		
                                        strBld.Append(barCode + CDFOX.RS_END_LINE);
                                        // padTile
                                        strBld.Append("ComponentID,"
                                                        + "Type,"
                                                        + "Area(%),"
                                                        + "Height,"
                                                        + "Volume(%),"
                                                        + "XOffset,"
                                                        + "YOffset,"
                                                        + "PadSize(X),"
                                                        + "PadSize(Y),"
                                                        + "Area,"
                                                        + "Height(%),"
                                                        + "Volume,"
                                                        + "Result,"
                                                        + "PinNum,"
                                                        + "Barcode,"
                                                        + "Date,"
                                                        + "Time,"
                                                        + "ArrayID" + "\r\n");
                                        string filePath = appSettingHandle._appSettingData.strDataExpPath;
                                        if (string.IsNullOrEmpty(filePath))
                                        {
                                            filePath = Path.Combine("D://EYSPI//DataExport//Res", strDate);

                                        }
                                        else
                                        {
                                            filePath = Path.Combine(filePath, strDate);
                                        }
                                        if (Directory.Exists(filePath) == false)
                                        {
                                            Directory.CreateDirectory(filePath);
                                        }
                                        if (pads != null && pads.Length > 0)
                                        {
                                            if (string.IsNullOrEmpty(barCode)) barCode = dt.Rows[i][0].ToString();
                                            string fileName = "SPI" + "_" +
                                              appSettingHandle._appSettingData.stDataExpVT.strCustomer + strLineNo + "_" +
                                              appSettingHandle._appSettingData.stDataExpVT.strMachine + "_" +
                                              barCode + "_" +
                                              strDateTime + ".csv";

                                            string file = Path.Combine(filePath, fileName );
                                            GetPadValuesForFox(pads, ref strBld);
                                            using (FileStream fsPads = new FileStream(file, FileMode.Create))
                                            {
                                                StreamWriter srPads = new StreamWriter(fsPads, System.Text.Encoding.Default);
                                                srPads.Write(strBld);
                                                srPads.Close();
                                                //log.WriteLog("Function Success ! 导出csv文件成功 !", "LHFOX");
                                                AppLogHelp.WriteError(LogFileFormate.MES, "LHFOX Function Success ! 导出csv文件成功 !!");
                                            }
                                        }
                                    }
                                    #endregion
                                }                               
                                //储存本次pcbID至 strLastPcbFile 以便下次读取
                                using (FileStream fsPcb = new FileStream(strLastPcbFile, FileMode.Create))
                                {
                                    StreamWriter swPcb = new StreamWriter(fsPcb, System.Text.Encoding.Default);
                                    swPcb.Write(dt.Rows[i][0].ToString());
                                    swPcb.Close();
                                }
                            }
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(timer);
                    }
                }
            }
            catch (Exception ex)
            {
                string error = "  Function: (SaveDataForLHfoxNG)  数据导出出错 ";
                
            }

        }
        /// <summary>
        /// 获得UI自定义错误代码
        /// </summary>
        /// <param name="type"></param>
        /// <param name="appSettingHandle"></param>
        /// <returns></returns>
        private static string GetPadErrorCodeStr(byte type, AppLayerLib.AppSettingHandler appSettingHandle)
        {
            bool bEnUserDefinedErrorCode = appSettingHandle._appSettingData.bEnUserDefinedErrorCode == true
                   && appSettingHandle._appSettingData.strUserDefinedErrorCodes != null;
            string sErrorCode = "";
            try
            {
                if (bEnUserDefinedErrorCode)
                {
                    sErrorCode = appSettingHandle._appSettingData.strUserDefinedErrorCodes[type];
                }
                else
                {
                    switch (type)
                    {
                        case 0:
                            sErrorCode = "Missing";break;
                        case 1:
                            sErrorCode = "Insufficient"; break;
                        case 2:
                            sErrorCode = "Excess"; break;
                        case 3:
                            sErrorCode = "OverHeight"; break;
                        case 4:
                            sErrorCode = "LowHeight"; break;
                        case 5:
                            sErrorCode = "OverArea"; break;
                        case 6:
                            sErrorCode = "LowArea"; break;
                        case 7:
                            sErrorCode = "ShiftX"; break;
                        case 8:
                            sErrorCode = "ShiftY"; break;
                        case 9:
                            sErrorCode = "Bridge"; break;
                        case 10:
                            sErrorCode = "ShapeError"; break;
                        case 11:
                            sErrorCode = "Smeared"; break;
                        case 12:
                            sErrorCode = "Coplanarity"; break;
                        case 13:
                            sErrorCode = "PreBridge"; break;
                        case 14:
                            sErrorCode = "PadAreaError"; break;
                        default :
                            break;
                    }
                    
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return sErrorCode;
        }
        private static void GetPadValuesForFox(PadInfo[] APads,ref System.Text.StringBuilder strBld)
        {
            for (int i = 0; i < APads.Length; i++)
            {
                if (APads[i].result == "4") continue;                                                            
                  //Component                  
                   strBld.Append(APads[i].component + CDFOX.RS_CD_Spit);                    
                    //Type
                   strBld.Append(APads[i].Type + CDFOX.RS_CD_Spit);                    
                    //Area(%)
                        strBld.Append(APads[i].perArea + CDFOX.RS_CD_Spit);                    
                    //Height
                        strBld.Append(float.Parse(APads[i].height)*0.001 + CDFOX.RS_CD_Spit);
                    //Volume(%)
                        strBld.Append(APads[i].perVolume + CDFOX.RS_CD_Spit);
                    //XOffset
                        strBld.Append(APads[i].xOffset + CDFOX.RS_CD_Spit);
                    //YOffset
                        strBld.Append(APads[i].yOffset + CDFOX.RS_CD_Spit);
                    //PadSize(X)
                        strBld.Append(APads[i].padSizeX + CDFOX.RS_CD_Spit);
                    //PadSize(Y)
                        strBld.Append(APads[i].padSizeY + CDFOX.RS_CD_Spit);
                    //Area
                        strBld.Append( float.Parse(APads[i].area)*0.000001 + CDFOX.RS_CD_Spit);
                    //Height(%)
                        strBld.Append(APads[i].perHeight + CDFOX.RS_CD_Spit);
                    //Volume
                        strBld.Append( float.Parse(APads[i].volume)*0.000000001 + CDFOX.RS_CD_Spit);
                    //Result
                       string strResult = APads[i].result;
                       if (strResult == "1" )//| strResult == "2")
                       {
                           strResult = APads[i].errcode;
                       }
                       else
                       {
                           strResult = "PASS";
                       }
                       strBld.Append(strResult + CDFOX.RS_CD_Spit);
                    //Errcode
                        //strBld.Append(APads[i].errcode + CDFOX.RS_CD_Spit);
                    //PinNum
                        strBld.Append(APads[i].pinNum + CDFOX.RS_CD_Spit);
                    //Barcode
                        strBld.Append(APads[i].barcode + CDFOX.RS_CD_Spit);
                    //Date
                        strBld.Append(APads[i].date + CDFOX.RS_CD_Spit);
                    //Time
                        strBld.Append(APads[i].time + CDFOX.RS_CD_Spit);
                    //ArrayID
                        strBld.Append(APads[i].arrayID + CDFOX.RS_CD_Spit);
                    strBld.Append(CDFOX.RS_END_LINE);
                
            }
        }
        #endregion

        #region "defaultTools"
        public string strMsg = string.Empty;
        public string LineOne = string.Empty;
        public string LineTwo = string.Empty;
        public string LineThree = string.Empty;
        public string LineFour = string.Empty;
        public string LineFive = string.Empty;
        public string LineFileName = string.Empty;
        public string LineFileFormat = string.Empty;
        public string LineSix = string.Empty;
        public string LineArray = string.Empty;
        public string LineOther = string.Empty;
        public string LineUiResultForPreson = string.Empty;

        public string sec = "IniSec";
        public string iniFilePath = @"D:\1\default.ini";
        public void AutoStartDefaultTools(string strConfigPath, string AstrRealConfigPath)
        {
            try
            {
                runnerDefaultTools = new ThreadProcessDefaultTools(strConfigPath, AstrRealConfigPath);
                runnerDefaultTools.Run();
            }
            catch (Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
                AppLogHelp.WriteLog(LogFileFormate.MES,ThreadName+ "进程关闭");
            }

        }
        public  void SaveFileForDefaultTools(
            string AsMySqlConnectionString,
            string strLastPcbPath, string strLastPcbFile, string strLastAppsettingTmpFile, string strAppsettingPath,
            AppLayerLib.AppSettingHandler appSettingHandle,
            int timer,
            string AsIniPath,
            string AstDir,string _strRealConfigPath)
        {
            try
            {
                string[] arrLineOne = new string[3];
                string[] arrLineTwo = new string[2];
                string[] arrLineThree = new string[3];
                string[] arrLineFour = new string[1];
                string[] arrLineFive = new string[36];
                string[] arrLineFileName = new string[4];
                //string[] arrLineFileFormat = new string[1];
                string[] arrLineOther = new string[8];
                //string strLineSix = string.Empty;           
                // ini
                string iniPath = AsIniPath;
                try
                {
                    getTheIniTia(iniPath);
                    //Line One
                    if (this.LineSix == "0")
                    {
                        arrLineOne = LineOne.Split(new char[1] { ',' }, StringSplitOptions.None);
                        arrLineTwo = LineTwo.Split(new char[1] { ',' }, StringSplitOptions.None);
                        arrLineThree = LineThree.Split(new char[1] { ',' }, StringSplitOptions.None);
                        arrLineFour = LineFour.Split(new char[1] { ',' }, StringSplitOptions.None);
                    }
                    if (this.LineSix == "1")
                    {
                        arrLineOther = this.LineOther.Split(new char[1] { ',' }, StringSplitOptions.None);
                    }
                    arrLineFive = LineFive.Split(new char[1] { ',' }, StringSplitOptions.None);
                    arrLineFileName = LineFileName.Split(new char[1] { ',' }, StringSplitOptions.None);
                    //arrLineFileFormat = LineOne.Split(new char[1] { ',' }, StringSplitOptions.None);
                }
                catch (Exception ee)
                {
                    throw new Exception( ee.Message);
                }
                while (true)
                {
                    string nextPcb = string.Empty;
                    if (File.Exists(strLastPcbFile))
                    {
                        using (StreamReader srMain = new StreamReader(strLastPcbFile))
                        {
                            string str = "";
                            if ((str = srMain.ReadLine()) != null)
                            {
                                nextPcb = str;
                            }
                        }
                    }
                    string sql = "SELECT t.PCBID,t.StartTime,t.PCBBarcode,t.JobIndex,t.Result,t.lineNo,job.JobName FROM spidb.TBBoard t INNER JOIN spidb.TBJobInfo job ON t.JobIndex = job.SerNo WHERE t.PCBID >  '" + nextPcb + "'" + "AND t.OPConfirmed ='1'   ORDER BY t.PCBID ASC";
                    DataTable dt = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, sql);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //wait the review station confirm
                        System.Threading.Thread.Sleep(5000);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i][0] != null)
                            {
                                string path = strAppsettingPath;
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
                                string sAppSettingFileTmp = strAppsettingPath + "\\" + max + ".bin";
                                if (File.Exists(sAppSettingFileTmp))
                                {
                                    appSettingHandle.Read(sAppSettingFileTmp, _strRealConfigPath);
                                    timer = appSettingHandle._appSettingData.stDataExpVT.IntervalSecond;       //等待间隔时间
                                    //储存本次pcbID至   strLastAppsettingTmpFile
                                    using (FileStream fsApp = new FileStream(strLastAppsettingTmpFile, FileMode.Create))
                                    {
                                        StreamWriter swApp = new StreamWriter(fsApp, System.Text.Encoding.Default);
                                        swApp.Write(dt.Rows[i][0].ToString());
                                        swApp.Close();
                                    }
                                }                               
                                //arrayBoard
                                #region  "拼板输出"
                                string strSqlArray = "SELECT bc.ArrayID,bc.ArrayBarCode,bc.ArrayID FROM spidb.TBBarCode as bc WHERE bc.PCBID = '" + dt.Rows[i][0].ToString() + "'";
                                DataTable dsqlArray = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, strSqlArray);
                                string jobIndex = dt.Rows[i][3].ToString();
                                string resultPCB = dt.Rows[i][4].ToString();
                                if (resultPCB == "1") resultPCB = "Fail";
                                else if (resultPCB == "0") resultPCB = "Good";
                                else if (resultPCB == "2") resultPCB = "Pass";
                                else resultPCB = "Fail";
                                string lineName = dt.Rows[i][5].ToString();
                                string strDate = ((DateTime)dt.Rows[i][1]).ToString("yyyyMMdd");
                                string strTime = ((DateTime)dt.Rows[i][1]).ToString("HHmmss");
                                string strDateTime = ((DateTime)dt.Rows[i][1]).ToString("yyyyMMdd HH:mm:ss");
                                string barCode = dt.Rows[i][2].ToString();
                                string jobName = dt.Rows[i][6].ToString();
                                if (dsqlArray != null && dsqlArray.Rows.Count > 0 && LineArray == "1")
                                {
                                    for (int j = 0; j < dsqlArray.Rows.Count; j++)
                                    {                                       
                                        string sqlPadInfo = " SELECT  " +
                                                                "pad.PadID, " +
                                                                "padm.ComponentID," +
                                                                "padm.PackageType," +
                                                                "pad.PerArea," +
                                                                "pad.ABSHeight," +
                                                                "pad.PerVolume," +
                                                                "pad.ShiftX," +
                                                                "pad.ShiftY," +
                                                                "padm.SizeXmm," +
                                                                "padm.SizeYmm," +
                                                                "pad.ABSArea," +
                                                                "pad.PerHeight," +
                                                                "pad.ABSVolume," +
                                                                "pad.JudgeRes," +
                                                                "pad.DefectType," +
                                                                "padm.PinNumber," +
                                                                "padm.ArrayID, " +
                                                                "padC.AreaU, " +
                                                                "padC.AreaL," +
                                                                "padC.VolU," +
                                                                "padC.VolL," +
                                                                "padC.HeightU," +
                                                                "padC.HeightL," +
                                                                "padC.ShiftXU," +
                                                                "padC.ShiftYU," +
                                                                "padC.ShiftDataType,  " +

                                                                "padC.AreaU*pad.ABSArea*0.000001,  " +
                                                                "padC.AreaL*pad.ABSArea*0.000001," +
                                                                "padC.VolU*pad.ABSVolume*0.000000001," +
	                                                            "padC.VolL*pad.ABSVolume*0.000000001," +
	                                                            "padC.HeightU*pad.ABSHeight*0.001," +
	                                                            "padC.HeightL*pad.ABSHeight*0.001," +
	                                                            "padC.ShiftXU*pad.ShiftX," +
                                                                "padC.ShiftYU*pad.ShiftY " +
                                                                "FROM  " +
                                                                "spidb.TBPadMeasure pad  " +
                                            //" ,spidb.TBSimplePad padm " +       
                                                            " INNER JOIN spidb.TBSimplePad padm ON pad.PadID = padm.PadID AND padm.JobIndex = pad.JobIndex  " +
                                                            " INNER JOIN spidb.TBPadConditionParams padC ON padm.PadCndtParamsIndex = padC.IndexSerNo AND padm.JobIndex = padC.JobIndex  " +
                                                            " WHERE " +
                                                            " pad.PCBID = '" + dt.Rows[i][0].ToString() + "'" +
                                                            " And pad.ArrayIDIndex = '" + dsqlArray.Rows[j][0].ToString() + "'";//And padm.JobIndex = " + jobIndex;                                      
                                        DataTable dsqlDefult = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, sqlPadInfo);
                                        PadInfo[] pads = null;
                                        if (dsqlDefult != null && dsqlDefult.Rows.Count > 0)
                                        {
                                            pads = new PadInfo[dsqlDefult.Rows.Count];
                                            for (int m = 0; m < dsqlDefult.Rows.Count; m++)
                                            {
                                                string strType = dsqlDefult.Rows[m][25].ToString();
                                                pads[m] = new PadInfo();
                                                //pcbid
                                                pads[m].padID = dsqlDefult.Rows[m][0].ToString();
                                                //commId
                                                pads[m].component = dsqlDefult.Rows[m][1].ToString();
                                                //type
                                                pads[m].Type = dsqlDefult.Rows[m][2].ToString();
                                                //perArea
                                                pads[m].perArea = dsqlDefult.Rows[m][3].ToString();
                                                //height
                                                pads[m].height = dsqlDefult.Rows[m][4].ToString();
                                                //perVolume
                                                pads[m].perVolume = dsqlDefult.Rows[m][5].ToString();
                                                //XOffset
                                                pads[m].xOffset = dsqlDefult.Rows[m][6].ToString();
                                                //YOffset
                                                pads[m].yOffset = dsqlDefult.Rows[m][7].ToString();
                                                //PadSize(X)
                                                pads[m].padSizeX = dsqlDefult.Rows[m][8].ToString();
                                                //PadSize(Y)
                                                pads[m].padSizeY = dsqlDefult.Rows[m][9].ToString();
                                                //Area
                                                pads[m].area = dsqlDefult.Rows[m][10].ToString();
                                                //perHeight
                                                pads[m].perHeight = dsqlDefult.Rows[m][11].ToString();
                                                //Volume
                                                pads[m].volume = dsqlDefult.Rows[m][12].ToString();
                                                //Result
                                                string padResult = "";
                                                if (dsqlDefult.Rows[m][13].ToString() == "0") padResult = "Good";
                                                else if (dsqlDefult.Rows[m][13].ToString() == "1") padResult = "NG";
                                                else if (dsqlDefult.Rows[m][13].ToString() == "2") padResult = "Pass";
                                                else padResult = "NG";
                                                pads[m].result = padResult;
                                                //Errcode
                                                pads[m].errcode = dsqlDefult.Rows[m][14].ToString();
                                                //PinNum
                                                pads[m].pinNum = dsqlDefult.Rows[m][15].ToString();
                                                //Barcode
                                                pads[m].barcode = barCode;
                                                //Date
                                                pads[m].date = strDate;
                                                //Time
                                                pads[m].time = strTime;
                                                //ArrayID
                                                pads[m].arrayID = dsqlDefult.Rows[m][16].ToString();
                                                pads[m].areaPerH = dsqlDefult.Rows[m][17].ToString();
                                                pads[m].areaPerL = dsqlDefult.Rows[m][18].ToString();
                                                pads[m].volumePerH = dsqlDefult.Rows[m][19].ToString();
                                                pads[m].volumePerL = dsqlDefult.Rows[m][20].ToString();
                                                pads[m].heightPerH = dsqlDefult.Rows[m][21].ToString();
                                                pads[m].heightPerL = dsqlDefult.Rows[m][22].ToString();

                                                pads[m].xOffsetPerH = dsqlDefult.Rows[m][23].ToString();
                                                pads[m].yOffsetPerH = dsqlDefult.Rows[m][24].ToString();
                                                pads[m].shiftDataType = dsqlDefult.Rows[m][25].ToString();
                                                pads[m].areaH = dsqlDefult.Rows[m][26].ToString();
                                                pads[m].areaL = dsqlDefult.Rows[m][27].ToString();
                                                pads[m].volumeH = dsqlDefult.Rows[m][28].ToString();
                                                pads[m].volumeL = dsqlDefult.Rows[m][29].ToString();
                                                pads[m].heightH = dsqlDefult.Rows[m][30].ToString();
                                                pads[m].heightL = dsqlDefult.Rows[m][31].ToString();
                                                pads[m].xOffsetH = dsqlDefult.Rows[m][32].ToString();
                                                pads[m].yOffsetH = dsqlDefult.Rows[m][33].ToString();
                                            }
                                            //导出PCB title-------0
                                            System.Text.StringBuilder strBld = new System.Text.StringBuilder();
                                            string strFileName = "";
                                            if (arrLineFileName.Length > 0)
                                            {
                                                if (arrLineFileName[0] == "1") strFileName += jobName + "_";
                                                if (arrLineFileName[1] == "1") strFileName += dsqlArray.Rows[j][1].ToString() +"-"+ dsqlArray.Rows[j][2].ToString() + "_";
                                                if (arrLineFileName[2] == "1") strFileName += strDate+strTime + "_";
                                            }
                                            int iStart = strFileName.Length - 2, iEnd = strFileName.Length - 1;
                                            if (strFileName == "") throw new  Exception( "error[ 文件后缀为空! 请重新选择] ");

                                            strFileName = strFileName.Substring(0, strFileName.Length - 1) + LineFileFormat;
                                            //}
                                            //基础格式
                                            #region "基础格式"
                                            if (LineSix == "0")
                                            {
                                                //LineOne
                                                if (arrLineOne.Length > 0)
                                                {
                                                    //key
                                                    for (int n = 0; n < arrLineOne.Length; n++)
                                                    {
                                                        if (!string.IsNullOrEmpty(arrLineOne[n]))
                                                        {
                                                            strBld.Append(arrLineOne[n] + CDFOX.RS_CD_Spit);
                                                            
                                                        }
                                                        if (n == arrLineOne.Length - 1)
                                                        {
                                                            strBld.Append(CDFOX.RS_END_LINE);
                                                        }
                                                    }
                                                    //value
                                                    if (!string.IsNullOrEmpty(arrLineOne[0])) strBld.Append(jobName + CDFOX.RS_CD_Spit);
                                                    if (!string.IsNullOrEmpty(arrLineOne[1])) strBld.Append(lineName + CDFOX.RS_CD_Spit);
                                                    if (!string.IsNullOrEmpty(arrLineOne[2])) strBld.Append(appSettingHandle._appSettingData.strLotNumber + CDFOX.RS_CD_Spit);
                                                    if (!string.IsNullOrEmpty(arrLineOne[0]) || !string.IsNullOrEmpty(arrLineOne[1]) || !string.IsNullOrEmpty(arrLineOne[2])) strBld.Append(CDFOX.RS_END_LINE);
                                                }
                                                //LineTwo
                                                if (arrLineTwo.Length > 0)
                                                {
                                                    //key//value
                                                    if (!string.IsNullOrEmpty(arrLineTwo[0])) strBld.Append(arrLineTwo[0] + CDFOX.RS_CD_Spit + resultPCB + CDFOX.RS_CD_Spit);
                                                    if (!string.IsNullOrEmpty(arrLineTwo[1])) strBld.Append(arrLineTwo[1] + CDFOX.RS_CD_Spit + dt.Rows[i][2].ToString() + CDFOX.RS_CD_Spit);
                                                    if (!string.IsNullOrEmpty(arrLineTwo[0]) || !string.IsNullOrEmpty(arrLineTwo[1])) strBld.Append(CDFOX.RS_END_LINE);
                                                }
                                                //LineThree
                                                if (arrLineThree.Length > 0)
                                                {
                                                    if (!string.IsNullOrEmpty(arrLineThree[0])) strBld.Append(arrLineThree[0] + CDFOX.RS_CD_Spit + dsqlArray.Rows[j][2].ToString() + CDFOX.RS_CD_Spit);
                                                    if (!string.IsNullOrEmpty(arrLineThree[1])) strBld.Append(arrLineThree[1] + CDFOX.RS_CD_Spit + resultPCB + CDFOX.RS_CD_Spit);
                                                    if (!string.IsNullOrEmpty(arrLineThree[2])) strBld.Append(arrLineThree[2] + CDFOX.RS_CD_Spit + dsqlArray.Rows[j][1].ToString() + CDFOX.RS_CD_Spit);
                                                    if (!string.IsNullOrEmpty(arrLineThree[0]) || !string.IsNullOrEmpty(arrLineThree[1]) || !string.IsNullOrEmpty(arrLineThree[2])) strBld.Append(CDFOX.RS_END_LINE);
                                                }
                                                //LineFour
                                                if (arrLineFour.Length > 0)
                                                {
                                                    if (!string.IsNullOrEmpty(arrLineFour[0])) strBld.Append(arrLineFour[0] + CDFOX.RS_CD_Spit + strDate + strTime + CDFOX.RS_END_LINE);
                                                }                                               
                                            }                                           
                                            #endregion
                                            // padTile
                                            bool[] padTitle = appSettingHandle._appSettingData.stDataExpVT.bPadExportedItems;                                                
                                            //                                          
                                           string filePath = appSettingHandle._appSettingData.stDataExpVT.strBackUpExportedFilePath;
                                            getThePadsKeyAndValues(arrLineFive, pads, ref strBld,LineUiResultForPreson);
                                            using (FileStream fsPads = new FileStream(Path.Combine(AstDir,strFileName), FileMode.Create))
                                            {
                                                StreamWriter srPads = new StreamWriter(fsPads, System.Text.Encoding.Default);
                                                srPads.Write(strBld);
                                                srPads.Close();
                                                //log.WriteLog(" Function Success ! 导出csv文件成功 !", "DefualTools");
                                            }
                                            //}
                                        }                                        
                                    }
                                }
                                #endregion
                                // no array
                                if (LineArray == "0")
                                {                                   
                                    string sqlPadInfo = " SELECT  " +
                                                                "pad.PadID, " +
                                                                "padm.ComponentID," +
                                                                "padm.PackageType," +
                                                                "pad.PerArea," +
                                                                "pad.ABSHeight," +
                                                                "pad.PerVolume," +
                                                                "pad.ShiftX," +
                                                                "pad.ShiftY," +
                                                                "padm.SizeXmm," +
                                                                "padm.SizeYmm," +
                                                                "pad.ABSArea," +
                                                                "pad.PerHeight," +
                                                                "pad.ABSVolume," +
                                                                "pad.JudgeRes," +
                                                                "pad.DefectType," +
                                                                "padm.PinNumber," +
                                                                "padm.ArrayID, " +
                                                                "padC.AreaU, " +
                                                                "padC.AreaL," +
                                                                "padC.VolU," +
                                                                "padC.VolL," +
                                                                "padC.HeightU," +
                                                                "padC.HeightL," +
                                                                "padC.ShiftXU," +
                                                                "padC.ShiftYU," +
                                                                "padC.ShiftDataType,  " +

                                                                "padC.AreaU*pad.ABSArea*0.000001,  " +
                                                                "padC.AreaL*pad.ABSArea*0.000001," +
                                                                "padC.VolU*pad.ABSVolume*0.000000001," +
                                                                "padC.VolL*pad.ABSVolume*0.000000001," +
                                                                "padC.HeightU*pad.ABSHeight*0.001," +
                                                                "padC.HeightL*pad.ABSHeight*0.001," +
                                                                "padC.ShiftXU*pad.ShiftX," +
                                                                "padC.ShiftYU*pad.ShiftY " +
                                                                "FROM  " +
                                                                "spidb.TBPadMeasure pad  " +
                                                            " INNER JOIN spidb.TBSimplePad padm ON pad.PadID = padm.PadID AND padm.JobIndex = pad.JobIndex  " +
                                                            " INNER JOIN spidb.TBPadConditionParams padC ON padm.PadCndtParamsIndex = padC.IndexSerNo AND padm.JobIndex = padC.JobIndex  " +
                                                            " WHERE " +
                                                            " pad.PCBID = '" + dt.Rows[i][0].ToString() + "'";
                                       DataTable dsqlDefult = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, sqlPadInfo);
                                        PadInfo[] pads = null;
                                        if (dsqlDefult != null && dsqlDefult.Rows.Count > 0)
                                        {
                                            pads = new PadInfo[dsqlDefult.Rows.Count];
                                            for (int m = 0; m < dsqlDefult.Rows.Count; m++)
                                            {
                                                string strType = dsqlDefult.Rows[m][25].ToString();
                                                pads[m] = new PadInfo();
                                                //pcbid
                                                pads[m].padID = dsqlDefult.Rows[m][0].ToString();
                                                //commId
                                                pads[m].component = dsqlDefult.Rows[m][1].ToString();
                                                //type
                                                pads[m].Type = dsqlDefult.Rows[m][2].ToString();
                                                //perArea
                                                pads[m].perArea = dsqlDefult.Rows[m][3].ToString();
                                                //height
                                                pads[m].height = dsqlDefult.Rows[m][4].ToString();
                                                //perVolume
                                                pads[m].perVolume = dsqlDefult.Rows[m][5].ToString();
                                                //XOffset
                                                pads[m].xOffset = dsqlDefult.Rows[m][6].ToString();
                                                //YOffset
                                                pads[m].yOffset = dsqlDefult.Rows[m][7].ToString();
                                                //PadSize(X)
                                                pads[m].padSizeX = dsqlDefult.Rows[m][8].ToString();
                                                //PadSize(Y)
                                                pads[m].padSizeY = dsqlDefult.Rows[m][9].ToString();
                                                //Area
                                                pads[m].area = dsqlDefult.Rows[m][10].ToString();
                                                //perHeight
                                                pads[m].perHeight = dsqlDefult.Rows[m][11].ToString();
                                                //Volume
                                                pads[m].volume = dsqlDefult.Rows[m][12].ToString();
                                                //Result
                                                string padResult = "";
                                                if (dsqlDefult.Rows[m][13].ToString() == "0") padResult = "Good";
                                                else if (dsqlDefult.Rows[m][13].ToString() == "1") padResult = "NG";
                                                else if (dsqlDefult.Rows[m][13].ToString() == "2") padResult = "Pass";
                                                else padResult = "NG";
                                                pads[m].result = padResult;
                                                //Errcode
                                                pads[m].errcode = dsqlDefult.Rows[m][14].ToString();
                                                //PinNum
                                                pads[m].pinNum = dsqlDefult.Rows[m][15].ToString();
                                                //Barcode
                                                pads[m].barcode = barCode;
                                                //Date
                                                pads[m].date = strDate;
                                                //Time
                                                pads[m].time = strTime;
                                                //ArrayID
                                                pads[m].arrayID = dsqlDefult.Rows[m][16].ToString();
                                                pads[m].areaPerH = dsqlDefult.Rows[m][17].ToString();
                                                pads[m].areaPerL = dsqlDefult.Rows[m][18].ToString();
                                                pads[m].volumePerH = dsqlDefult.Rows[m][19].ToString();
                                                pads[m].volumePerL = dsqlDefult.Rows[m][20].ToString();
                                                pads[m].heightPerH = dsqlDefult.Rows[m][21].ToString();
                                                pads[m].heightPerL = dsqlDefult.Rows[m][22].ToString();

                                                pads[m].xOffsetPerH = dsqlDefult.Rows[m][23].ToString();
                                                pads[m].yOffsetPerH = dsqlDefult.Rows[m][24].ToString();
                                                pads[m].shiftDataType = dsqlDefult.Rows[m][25].ToString();

                                                pads[m].areaH = dsqlDefult.Rows[m][26].ToString();
                                                pads[m].areaL = dsqlDefult.Rows[m][27].ToString();
                                                pads[m].volumeH = dsqlDefult.Rows[m][28].ToString();
                                                pads[m].volumeL = dsqlDefult.Rows[m][29].ToString();
                                                pads[m].heightH = dsqlDefult.Rows[m][30].ToString();
                                                pads[m].heightL = dsqlDefult.Rows[m][31].ToString();
                                                pads[m].xOffsetH = dsqlDefult.Rows[m][32].ToString();
                                                pads[m].yOffsetH = dsqlDefult.Rows[m][33].ToString();
                                            }
                                            //导出PCB title-------0
                                            System.Text.StringBuilder strBld = new System.Text.StringBuilder();
                                            string strFileName = "";
                                            if (arrLineFileName.Length > 0)
                                            {
                                                if (arrLineFileName[0] == "1") strFileName += jobName + "_";
                                                if (arrLineFileName[1] == "1") strFileName += dt.Rows[i][2].ToString() + "_";
                                                if (arrLineFileName[2] == "1") strFileName += strDate + strTime + "_";
                                            }
                                            int iStart = strFileName.Length - 2, iEnd = strFileName.Length - 1;
                                            if (strFileName == "") throw new Exception("error[ 文件后缀为空! 请重新选择] ");
                                            strFileName = strFileName.Substring(0, strFileName.Length - 1) + LineFileFormat;
                                            // padTile
                                            //bool[] padTitle = appSettingHandle._appSettingData.stDataExpVT.bPadExportedItems;
                                            //     
                                            #region "基础格式"
                                            if (LineSix == "0")
                                            {
                                                //LineOne
                                                if (arrLineOne.Length > 0)
                                                {
                                                    //key
                                                    for (int n = 0; n < arrLineOne.Length; n++)
                                                    {
                                                        if (!string.IsNullOrEmpty(arrLineOne[n]))
                                                        {
                                                            strBld.Append(arrLineOne[n] + CDFOX.RS_CD_Spit);                                                           
                                                        }
                                                        if (n == arrLineOne.Length - 1)
                                                        {
                                                            strBld.Append(CDFOX.RS_END_LINE);
                                                        }
                                                    }
                                                    //value
                                                    if (!string.IsNullOrEmpty(arrLineOne[0])) strBld.Append(jobName + CDFOX.RS_CD_Spit);
                                                    if (!string.IsNullOrEmpty(arrLineOne[1])) strBld.Append(lineName + CDFOX.RS_CD_Spit);
                                                    if (!string.IsNullOrEmpty(arrLineOne[2])) strBld.Append(appSettingHandle._appSettingData.strLotNumber + CDFOX.RS_CD_Spit);
                                                    if (!string.IsNullOrEmpty(arrLineOne[0]) || !string.IsNullOrEmpty(arrLineOne[1]) || !string.IsNullOrEmpty(arrLineOne[2])) strBld.Append(CDFOX.RS_END_LINE);
                                                }
                                                //LineTwo
                                                if (arrLineTwo.Length > 0)
                                                {
                                                    //key//value
                                                    if (!string.IsNullOrEmpty(arrLineTwo[0])) strBld.Append(arrLineTwo[0] + CDFOX.RS_CD_Spit + resultPCB + CDFOX.RS_CD_Spit);
                                                    if (!string.IsNullOrEmpty(arrLineTwo[1])) strBld.Append(arrLineTwo[1] + CDFOX.RS_CD_Spit + dt.Rows[i][2].ToString() + CDFOX.RS_CD_Spit);
                                                    if (!string.IsNullOrEmpty(arrLineTwo[0]) || !string.IsNullOrEmpty(arrLineTwo[1])) strBld.Append(CDFOX.RS_END_LINE);
                                                }
                                                //LineThree
                                                if (arrLineThree.Length > 0)
                                                {
                                                    if (!string.IsNullOrEmpty(arrLineThree[0])) strBld.Append(arrLineThree[0] + CDFOX.RS_CD_Spit + "0" + CDFOX.RS_CD_Spit);
                                                    if (!string.IsNullOrEmpty(arrLineThree[1])) strBld.Append(arrLineThree[1] + CDFOX.RS_CD_Spit + resultPCB + CDFOX.RS_CD_Spit);
                                                    if (!string.IsNullOrEmpty(arrLineThree[2])) strBld.Append(arrLineThree[2] + CDFOX.RS_CD_Spit + dt.Rows[i][2].ToString() + CDFOX.RS_CD_Spit);
                                                    if (!string.IsNullOrEmpty(arrLineThree[0]) || !string.IsNullOrEmpty(arrLineThree[1]) || !string.IsNullOrEmpty(arrLineThree[2])) strBld.Append(CDFOX.RS_END_LINE);
                                                }
                                                //LineFour
                                                if (arrLineFour.Length > 0)
                                                {
                                                    if (!string.IsNullOrEmpty(arrLineFour[0])) strBld.Append(arrLineFour[0] + CDFOX.RS_CD_Spit + strDate + strTime + CDFOX.RS_END_LINE);
                                                }
                                            }
                                            #endregion
                                            string filePath = appSettingHandle._appSettingData.stDataExpVT.strBackUpExportedFilePath;
                                            getThePadsKeyAndValues(arrLineFive, pads, ref strBld,LineUiResultForPreson);
                                            using (FileStream fsPads = new FileStream(Path.Combine(AstDir, strFileName), FileMode.Create))
                                            {
                                                StreamWriter srPads = new StreamWriter(fsPads, System.Text.Encoding.Default);
                                                srPads.Write(strBld);
                                                srPads.Close();
                                                
                                            }
                                        }
                                }
                                //储存本次pcbID至 strLastPcbFile 以便下次读取
                                using (FileStream fsPcb = new FileStream(strLastPcbFile, FileMode.Create))
                                {
                                    StreamWriter swPcb = new StreamWriter(fsPcb, System.Text.Encoding.Default);
                                    swPcb.Write(dt.Rows[i][0].ToString());
                                    swPcb.Close();
                                }
                            }
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(timer);
                    }
                }
            }
            catch (Exception ex)
            {
                string error = "  Function: (SaveFileForDefaultTools)  数据导出出错 ";
                //log.WriteErr("错误 ! " + error + ex.ToString());
                AppLogHelp.WriteError(LogFileFormate.SQL, error +ex.Message);
            }            
        }
        

        private  void getTheIniTia(string iniPath)
        {
            try
            {
                iniFilePath = Path.Combine(iniPath, "default.ini");
                if (File.Exists(iniFilePath) == false)
                {
                    throw new Exception("iniFilePath is not exist !");
                }
                this.LineSix = ReadIniData(sec, "LineSix", string.Empty, iniFilePath);
                if (this.LineSix == "0")
                {
                    this.LineOne = ReadIniData(sec, "LineOne", string.Empty, iniFilePath);
                    this.LineTwo = ReadIniData(sec, "LineTwo", string.Empty, iniFilePath);
                    this.LineThree = ReadIniData(sec, "LineThree", string.Empty, iniFilePath);
                    this.LineFour = ReadIniData(sec, "LineFour", string.Empty, iniFilePath);
                }
                if (this.LineSix == "1")
                {
                    this.LineOther = ReadIniData(sec, "LineOther", string.Empty, iniFilePath);
                }
                //this.iniFilePath = 
                this.LineArray = ReadIniData(sec, "LineArray", string.Empty, iniFilePath);
                this.LineFive = ReadIniData(sec, "LineFive", string.Empty, iniFilePath);
                this.LineFileName = ReadIniData(sec, "LineFileName", string.Empty, iniFilePath);
                this.LineFileFormat = ReadIniData(sec, "LineFileFormat", string.Empty, iniFilePath);
                this.LineUiResultForPreson = ReadIniData(sec, "LineUiResultForPreson", string.Empty, iniFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void getThePadsKeyAndValues(string[] padTitle, PadInfo[] Apads, ref StringBuilder strBld, string LineUiResultForPreson)
        {
            if (!string.IsNullOrEmpty(padTitle[0]))
            {
                strBld.Append("PadID" + CDFOX.RS_CD_Spit);
            }
            //Component
            if (!string.IsNullOrEmpty(padTitle[1]))
            {
                strBld.Append("Component" + CDFOX.RS_CD_Spit);
            }
            //Type
            if (!string.IsNullOrEmpty(padTitle[2]))
            {
                strBld.Append("Type" + CDFOX.RS_CD_Spit);
            }
            //Area(%)
            if (!string.IsNullOrEmpty(padTitle[3]))
            {
                strBld.Append("Area(%)" + CDFOX.RS_CD_Spit);
            }
            //Height
            if (!string.IsNullOrEmpty(padTitle[4]))
            {
                strBld.Append("Height" + CDFOX.RS_CD_Spit);
            }
            //Volume(%)
            if (!string.IsNullOrEmpty(padTitle[5]))
            {
                strBld.Append("Volume(%)" + CDFOX.RS_CD_Spit);
            }
            //XOffset
            if (!string.IsNullOrEmpty(padTitle[6]))
            {
                strBld.Append("XOffset" + CDFOX.RS_CD_Spit);
            }
            //YOffset
            if (!string.IsNullOrEmpty(padTitle[7]))
            {
                strBld.Append("YOffset" + CDFOX.RS_CD_Spit);
            }
            //PadSize(X)
            if (!string.IsNullOrEmpty(padTitle[8]))
            {
                strBld.Append("PadSize(X)" + CDFOX.RS_CD_Spit);
            }
            //PadSize(Y)
            if (!string.IsNullOrEmpty(padTitle[9]))
            {
                strBld.Append("PadSize(Y)" + CDFOX.RS_CD_Spit);
            }
            //Area
            if (!string.IsNullOrEmpty(padTitle[10]))
            {
                strBld.Append("Area" + CDFOX.RS_CD_Spit);
            }
            //Height(%)
            if (!string.IsNullOrEmpty(padTitle[11]))
            {
                strBld.Append("Height(%)" + CDFOX.RS_CD_Spit);
            }
            //Volume
            if (!string.IsNullOrEmpty(padTitle[12]))
            {
                strBld.Append("Volume" + CDFOX.RS_CD_Spit);
            }
            //Result
            if (!string.IsNullOrEmpty(padTitle[13]))
            {
            strBld.Append("Result" + CDFOX.RS_CD_Spit);
            }
            //Errcode
            if (!string.IsNullOrEmpty(padTitle[14]))
            {
                strBld.Append("Errcode" + CDFOX.RS_CD_Spit);

            }
            //PinNum
            if (!string.IsNullOrEmpty(padTitle[15]))
            {
                strBld.Append("PinNum" + CDFOX.RS_CD_Spit);
            }
            //Barcode
            if (!string.IsNullOrEmpty(padTitle[16]))
            {
                strBld.Append("Barcode" + CDFOX.RS_CD_Spit);
            }
            //Date
            if (!string.IsNullOrEmpty(padTitle[17]))
            {
                strBld.Append("Date" + CDFOX.RS_CD_Spit);
            }
            //Time
            if (!string.IsNullOrEmpty(padTitle[18]))
            {
                strBld.Append("Time" + CDFOX.RS_CD_Spit);
            }
            //ArrayID
            if (!string.IsNullOrEmpty(padTitle[19]))
            {
                strBld.Append("ArrayID" + CDFOX.RS_CD_Spit);
            }
            if (!string.IsNullOrEmpty(padTitle[20]))
            {
                strBld.Append("AreaU" + CDFOX.RS_CD_Spit);
            }
            //AreaL
            if (!string.IsNullOrEmpty(padTitle[21]))
            {
                strBld.Append("AreaL" + CDFOX.RS_CD_Spit);
            }
            //VolumeU
            if (!string.IsNullOrEmpty(padTitle[22]))
            {
                strBld.Append("VolumeU" + CDFOX.RS_CD_Spit);
            }
            //VolumeL
            if (!string.IsNullOrEmpty(padTitle[23]))
            {
                strBld.Append("VolumeL" + CDFOX.RS_CD_Spit);
            }
            //HeightU
            if (!string.IsNullOrEmpty(padTitle[24]))
            {
                strBld.Append("HeightU" + CDFOX.RS_CD_Spit);
            }
            //HeightL
            if (!string.IsNullOrEmpty(padTitle[25]))
            {
                strBld.Append("HeightL" + CDFOX.RS_CD_Spit);
            }
            //XOffsetH
            if (!string.IsNullOrEmpty(padTitle[26]))
            {
                strBld.Append("XOffsetH" + CDFOX.RS_CD_Spit);
            }
            //YOffset
            if (!string.IsNullOrEmpty(padTitle[27]))
            {
                strBld.Append("AreaH(%)" + CDFOX.RS_CD_Spit);
            }
            //padGroup
            if (!string.IsNullOrEmpty(padTitle[28]))
            {
                strBld.Append("AreaL(%)" + CDFOX.RS_CD_Spit);
            }
            if (!string.IsNullOrEmpty(padTitle[29]))
            {
                strBld.Append("VolumeH(%)" + CDFOX.RS_CD_Spit);
            }
            if (!string.IsNullOrEmpty(padTitle[30]))
            {
                strBld.Append("VolumeL(%)" + CDFOX.RS_CD_Spit);
            }
            if (!string.IsNullOrEmpty(padTitle[31]))
            {
                strBld.Append("HeightH(%)" + CDFOX.RS_CD_Spit);
            }
            if (!string.IsNullOrEmpty(padTitle[32]))
            {
                strBld.Append("HeightL(%)" + CDFOX.RS_CD_Spit);
            }
            if (!string.IsNullOrEmpty(padTitle[33]))
            {
                strBld.Append("XoffsetH(%)" + CDFOX.RS_CD_Spit);
            }
            if (!string.IsNullOrEmpty(padTitle[35]))
            {
                strBld.Append("YoffsetH(%)" + CDFOX.RS_CD_Spit);
            }
            //面积、体积、高度、X/Y偏移、焊盘分组选项
            strBld.Append(CDFOX.RS_END_LINE);
            //LineUiResultForPreson    0 :All   1:PassAndNG   2:NG
            if (Apads.Length > 0)
            {
                foreach (PadInfo pad in Apads)
                {
                    if (LineUiResultForPreson == "2")
                    {
                        if (pad.result != "NG") continue;                        
                    }
                    if (LineUiResultForPreson == "1")
                    {
                        if (pad.result == "Good") continue;
                    }
                    if (!string.IsNullOrEmpty(padTitle[0]))
                    {
                        strBld.Append(pad.padID + CDFOX.RS_CD_Spit);
                    }
                    //Component
                    if (!string.IsNullOrEmpty(padTitle[1]))
                    {
                        strBld.Append(pad.component+ CDFOX.RS_CD_Spit);
                    }
                    //Type
                    if (!string.IsNullOrEmpty(padTitle[2]))
                    {
                        strBld.Append(pad.Type + CDFOX.RS_CD_Spit);
                    }
                    //Area(%)
                    if (!string.IsNullOrEmpty(padTitle[3]))
                    {
                        strBld.Append(pad.perArea + CDFOX.RS_CD_Spit);
                    }
                    //Height
                    if (!string.IsNullOrEmpty(padTitle[4]))
                    {
                        strBld.Append(pad.height+ CDFOX.RS_CD_Spit);
                    }
                    //Volume(%)
                    if (!string.IsNullOrEmpty(padTitle[5]))
                    {
                        strBld.Append(pad.heightPerH + CDFOX.RS_CD_Spit);
                    }
                    //XOffset
                    if (!string.IsNullOrEmpty(padTitle[6]))
                    {
                        strBld.Append(pad.xOffset + CDFOX.RS_CD_Spit);
                    }
                    //YOffset
                    if (!string.IsNullOrEmpty(padTitle[7]))
                    {
                        strBld.Append(pad.yOffset + CDFOX.RS_CD_Spit);
                    }
                    //PadSize(X)
                    if (!string.IsNullOrEmpty(padTitle[8]))
                    {
                        strBld.Append(pad.padSizeX + CDFOX.RS_CD_Spit);
                    }
                    //PadSize(Y)
                    if (!string.IsNullOrEmpty(padTitle[9]))
                    {
                        strBld.Append(pad.padSizeY + CDFOX.RS_CD_Spit);
                    }
                    //Area
                    if (!string.IsNullOrEmpty(padTitle[10]))
                    {
                        strBld.Append(pad.area + CDFOX.RS_CD_Spit);
                    }
                    //Height(%)
                    if (!string.IsNullOrEmpty(padTitle[11]))
                    {
                        strBld.Append(pad.perHeight + CDFOX.RS_CD_Spit);
                    }
                    //Volume
                    if (!string.IsNullOrEmpty(padTitle[12]))
                    {
                        strBld.Append(pad.volume + CDFOX.RS_CD_Spit);
                    }
                    //Result
                    if (!string.IsNullOrEmpty(padTitle[13]))
                    {
                        strBld.Append(pad.result + CDFOX.RS_CD_Spit);
                    }
                    //Errcode
                    if (!string.IsNullOrEmpty(padTitle[14]))
                    {
                        strBld.Append(pad.errcode + CDFOX.RS_CD_Spit);

                    }
                    //PinNum
                    if (!string.IsNullOrEmpty(padTitle[15]))
                    {
                        strBld.Append(pad.pinNum + CDFOX.RS_CD_Spit);
                    }
                    //Barcode
                    if (!string.IsNullOrEmpty(padTitle[16]))
                    {
                        strBld.Append(pad.barcode + CDFOX.RS_CD_Spit);
                    }
                    //Date
                    if (!string.IsNullOrEmpty(padTitle[17]))
                    {
                        strBld.Append(pad.date + CDFOX.RS_CD_Spit);
                    }
                    //Time
                    if (!string.IsNullOrEmpty(padTitle[18]))
                    {
                        strBld.Append(pad.time + CDFOX.RS_CD_Spit);
                    }
                    //ArrayID
                    if (!string.IsNullOrEmpty(padTitle[19]))
                    {
                        strBld.Append(pad.arrayID + CDFOX.RS_CD_Spit);
                    }
                    if (!string.IsNullOrEmpty(padTitle[20]))
                    {
                        strBld.Append(pad.areaH + CDFOX.RS_CD_Spit);
                    }
                    //AreaL
                    if (!string.IsNullOrEmpty(padTitle[21]))
                    {
                        strBld.Append(pad.areaL + CDFOX.RS_CD_Spit);
                    }
                    //VolumeU
                    if (!string.IsNullOrEmpty(padTitle[22]))
                    {
                        strBld.Append(pad.volumeH + CDFOX.RS_CD_Spit);
                    }
                    //VolumeL
                    if (!string.IsNullOrEmpty(padTitle[23]))
                    {
                        strBld.Append(pad.volumeL + CDFOX.RS_CD_Spit);
                    }
                    //HeightU
                    if (!string.IsNullOrEmpty(padTitle[24]))
                    {
                        strBld.Append(pad.heightH + CDFOX.RS_CD_Spit);
                    }
                    //HeightL
                    if (!string.IsNullOrEmpty(padTitle[25]))
                    {
                        strBld.Append(pad.heightL + CDFOX.RS_CD_Spit);
                    }
                    //XOffsetH
                    if (!string.IsNullOrEmpty(padTitle[26]))
                    {
                        strBld.Append(pad.xOffsetH + CDFOX.RS_CD_Spit);
                    }
                    //YOffset
                    if (!string.IsNullOrEmpty(padTitle[27]))
                    {
                        strBld.Append(pad.areaPerH + CDFOX.RS_CD_Spit);
                    }
                    //padGroup
                    if (!string.IsNullOrEmpty(padTitle[28]))
                    {
                        strBld.Append(pad.areaPerL + CDFOX.RS_CD_Spit);
                    }
                    if (!string.IsNullOrEmpty(padTitle[29]))
                    {
                        strBld.Append(pad.volumePerH + CDFOX.RS_CD_Spit);
                    }
                    if (!string.IsNullOrEmpty(padTitle[30]))
                    {
                        strBld.Append(pad.volumePerL + CDFOX.RS_CD_Spit);
                    }
                    if (!string.IsNullOrEmpty(padTitle[31]))
                    {
                        strBld.Append(pad.heightPerH + CDFOX.RS_CD_Spit);
                    }
                    if (!string.IsNullOrEmpty(padTitle[32]))
                    {
                        strBld.Append(pad.heightPerL + CDFOX.RS_CD_Spit);
                    }
                    if (!string.IsNullOrEmpty(padTitle[33]))
                    {
                        strBld.Append(pad.xOffsetPerH + CDFOX.RS_CD_Spit);
                    }
                    if (!string.IsNullOrEmpty(padTitle[35]))
                    {
                        strBld.Append(pad.yOffsetPerH + CDFOX.RS_CD_Spit);
                    }
                    strBld.Append(CDFOX.RS_END_LINE);

                }

            }



        }
        #endregion

        #region" 贵阳富士康"
        public void AutoStartGYFox(DateTime lastTime,DateTime nowTime)
        {
            try
            {
                runnerGYfox = new ThreadProcessGYFO(lastTime,nowTime );
                runnerGYfox.Run();
            }
            catch (Exception ex)
            {
                 
            }

        }
        public void SaveDataForGYFox(string AsMySqlConnecionString,DateTime lastTime,DateTime nowTime)
        {
            string sqlFx = "";
            string sqlPcb = "";
            //string strGyJosn = "";
            try
            {
                sqlFx = "SELECT t.Factory,t.Floor,t.Line,t.EquipID,t.EquipName,t.Module " +
                           " FROM TBEquipStatus t " +
                           " WHERE t.UpdateTime >=STR_TO_DATE('" + lastTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S') " +
                           " and t.UpdateTime <=STR_TO_DATE('" + nowTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S')" +
                           " ORDER BY t.UpdateTime";
                
                string sFactory = "";
                string sFloor = "";
                string sLine = "";
                
                //DBdata获取 设备信息
                System.Data.DataTable dataTable = getDataTableForZZFox(AsMySqlConnecionString, sqlFx);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    //
                    System.Threading.Thread.Sleep(10000);
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        if (string.IsNullOrEmpty(dataTable.Rows[i][0].ToString().Trim()) ||
                            string.IsNullOrEmpty(dataTable.Rows[i][1].ToString().Trim()) ||
                            string.IsNullOrEmpty(dataTable.Rows[i][2].ToString().Trim()))
                        {
                            continue;
                        }
                        sFactory = dataTable.Rows[i][0].ToString().Trim();
                        sFloor = dataTable.Rows[i][1].ToString().Trim();
                        sLine = dataTable.Rows[i][2].ToString().Trim();
                    }
                }
                sqlPcb = "SELECT " +
                            "pcb.LineNo," +
                            "pcb.PCBBarcode," +
                            "pcb.Result, " +
                            "pcb.StartTime "+
                        "FROM " +
                            "spidb.TBBoard AS pcb " +
                        "WHERE " +
                            "pcb.StartTime >= STR_TO_DATE('" + lastTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S')  " +
                        "AND pcb.StartTime <STR_TO_DATE('" + nowTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S')  ";
                 DataTable dsqlDefult = bu_Peng.getDataTableForZZFox(AsMySqlConnecionString, sqlPcb);
                 if (dsqlDefult.Rows.Count > 0 && dsqlDefult != null)
                 {
                     StringBuilder strBld = new StringBuilder();
                     for (int j = 0; j < dsqlDefult.Rows.Count; j++)
                     {
                         string result = "NG";
                         if(dsqlDefult.Rows[j][2].ToString() == "2" || dsqlDefult.Rows[j][2].ToString() == "0")
                         {
                             result = "OK";
                         }
                         //“{"SN":"100245k","Line":"GS01","Position":"A-1F","StationName":"AOI-A","Model":"Bond","Shift":"D","TestTime":"2018/05/23 16:16:45","Result":"OK","Yield":"99.8"}”
                         strBld.Append("{\"SN:" + "\""+dsqlDefult.Rows[j][1].ToString()+ "," +
                             "\"Line:\"" + dsqlDefult.Rows[j][0].ToString()+ "," +
                              "\"Position:\"" + sFactory+"-"+sFloor + "," +
                              "\"StationName:\"" +"SPI"+ "," +
                              "\"Model:\"" +"Bond"+ "," +
                              "\"Shift:\"" +"D"+ "," +
                              "\"TestTime:\"" +  dsqlDefult.Rows[j][3].ToString()+ "," +
                               "\"Result:\"" +result+ "," +
                               "\"Yield:\"" +"99.8"+ "," );
                     }
                     string file = Path.Combine(@"D:\1",nowTime.ToString("yyyyMMddHHmmss")+".txt");
                     FileStream fs = new FileStream(file,FileMode.Create);
                     StreamWriter sw = new StreamWriter(fs,Encoding.Default);
                     sw.Write(strBld);
                     sw.Close();
                     fs.Close();
                     //log.WriteLog("Function Success ! Data上传成功 !", "GYFOX");
                 }

            }
            catch (Exception ex)
            {
                string error = "  Function: (SaveDataForGYFox)  数据出错 ";
                //log.WriteErr("错误 ! " + error + ex.ToString());
            }


        }


        #endregion

        #region "深圳市彩煌实业"
        public void AutoStartCaiHuang(DateTime dtStartTime, DateTime dtEndTime, String strCVSFile)
        {
            try
            {
                runnerCAIhuang = new ThreadProcessCAIHUANG(dtStartTime, dtEndTime, strCVSFile);
                runnerCAIhuang.Run();

            }catch(Exception e)
            {
                //log.WriteErr("错误 ! " + e.ToString(), ThreadName);
            }

        }

        public static void SaveCsvFileForCaiHuang(string strSqlConnection, string strCSVFilePath, DateTime startTime, DateTime endTime)
        {
             //string strAppsettingPath = @"D:\EYSPI\Bin\AutoAPPConfig";
            try
            {
                
                string sql = " SELECT t.PCBID,t.JobIndex,t.lineNo,t.StartTime,t.EndTime,t.Result,job.JobName,t.PCBBarcode FROM spidb.TBBoard t,spidb.TBJobInfo job  "
                            +" WHERE t.JobIndex = job.SerNo And t.StartTime >=STR_TO_DATE('" + startTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S')  AND t.EndTime < STR_TO_DATE('" + endTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S')   ORDER BY t.PCBID ASC";
                    DataTable dt = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, sql);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //wait the review station confirm
                        //System.Threading.Thread.Sleep(5000);
                        int iSerNoRows = 0;
                        
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i][0] != null)
                            {
                                string PCBID = dt.Rows[i][0].ToString();
                                string JobIndex = dt.Rows[i][1].ToString();

                                string lineName = dt.Rows[i][2].ToString();
                                string strStartTime = dt.Rows[i][3].ToString();
                                string strEndTime = dt.Rows[i][4].ToString();
                                string strResult = dt.Rows[i][5].ToString();
                                string jobName = dt.Rows[i][6].ToString();
                                string strBarcode = dt.Rows[i][7].ToString();
                               
                                string strDateFormat = DateTime.Parse(strStartTime).ToString("yyyyMMdd");
                                string strDateTime = DateTime.Parse(strStartTime).ToString("yyyyMMddHHmmss");
                                //if (string.IsNullOrEmpty(strBarcode) || string.Equals("noread", strBarcode, StringComparison.CurrentCultureIgnoreCase)) strBarcode = strDateTime;
                                string strFile = Path.Combine(strCSVFilePath, strDateFormat+".csv");
                                if (string.IsNullOrEmpty(strBarcode) || string.Equals("noread", strBarcode, StringComparison.CurrentCultureIgnoreCase)) strBarcode = strDateTime ;
                                if (strResult == "0" || strResult == "2")
                                {
                                    strResult = "Pass";
                                }
                                else
                                {
                                    strResult = "Fail";
                                }
                                //array
                                string sqlArrayBarcode = "SELECT bc.BarCode,bc.ArrayID,bc.ArrayBarCode FROM spidb.TBBarCode bc WHERE bc.PCBID = '" + PCBID + "' ORDER BY bc.ArrayID";
                                DataTable dtArrayBarcode = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, sqlArrayBarcode);
                                string[] arrParams = new string[7];
                                if (dtArrayBarcode != null && dtArrayBarcode.Rows.Count > 0)
                                {
                                    if (dtArrayBarcode.Rows.Count > 1)
                                    {
                                        for (int k = 0; k < dtArrayBarcode.Rows.Count; k++)
                                        {
                                            iSerNoRows++;
                                            string strArrayBarcode = dtArrayBarcode.Rows[k][2].ToString();
                                            string strArrayID = dtArrayBarcode.Rows[k][1].ToString();
                                            if (string.IsNullOrEmpty(strArrayBarcode) || string.Equals("noread", strArrayBarcode, StringComparison.CurrentCultureIgnoreCase)) strArrayBarcode = strDateTime + "_" + strArrayID;
                                            string strArrayResult = "Pass";
                                            //pad
                                            string sqlPadInfo = " SELECT  " +
                                                        "pad.JudgeRes " +
                                                        "  FROM  " +
                                                        " spidb.TBPadMeasure pad " +
                                                        " WHERE " +
                                                        " pad.PCBID = '" + PCBID + "'" +
                                                        " and pad.JobIndex + '" + JobIndex + "'" +
                                                        " and pad.ArrayIDIndex =  '" + strArrayID+"'"+
                                                        " ORDER BY pad.ArrayIDIndex";//And padm.JobIndex = " + jobIndex;
                                            DataTable dtPadInfo = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, sqlPadInfo);
                                            if (dtPadInfo != null && dtPadInfo.Rows.Count > 0)
                                            {
                                                for (int j = 0; j < dtPadInfo.Rows.Count; j++)
                                                {
                                                    if (dtPadInfo.Rows[j][0].ToString() == "0" || dtPadInfo.Rows[j][0].ToString() == "2")
                                                    {
                                                        strArrayResult = "Pass";
                                                    }
                                                    else
                                                    {
                                                        strArrayResult = "Fail";
                                                        break;
                                                    }
                                                }
                                            }
                                            
                                            arrParams[0] = iSerNoRows+"";
                                            arrParams[1] = lineName + "";
                                            arrParams[2] = jobName + "";
                                            arrParams[3] = strArrayBarcode + "";
                                            arrParams[4] = strStartTime + "";
                                            arrParams[5] = strEndTime + "";
                                            arrParams[6] = strArrayResult + "";
                                            CreateOrAppendFile(strFile,  arrParams, ref iSerNoRows);
                                        }
                                        
                                    }
                                    else
                                    {
                                        iSerNoRows++;
                                        arrParams[0] = iSerNoRows + "";
                                        arrParams[1] = lineName + "";
                                        arrParams[2] = jobName + "";
                                        arrParams[3] = strBarcode + "";
                                        arrParams[4] = strStartTime + "";
                                        arrParams[5] = strEndTime + "";
                                        arrParams[6] = strResult + "";
                                        CreateOrAppendFile(strFile, arrParams,ref iSerNoRows);
                                    }
                                }

                            }
                        }
                    }

            }
            catch (Exception ex)
            {
                string error = "  Function: (SaveCsvFileForCaiHuang)  出错 ";
                //log.WriteErr("错误 ! " + error + ex.ToString());
                AppLogHelp.WriteError(LogFileFormate.Delete, "错误 ! " + error + ex.ToString());
                
            }

        }

        private static void CreateOrAppendFile(string strFile, string[] arrParams,ref int  iSerNoRows)
        {
            try
            {
                System.Text.StringBuilder strBld = new StringBuilder();
                
                if (File.Exists(strFile) == false)
                {
                    arrParams[0] = "1";
                    iSerNoRows = 1;
                    strBld.Append(arrParams[0] + CDFOX.RS_CD_Spit
                         + arrParams[1] + CDFOX.RS_CD_Spit
                         + arrParams[2] + CDFOX.RS_CD_Spit
                         + arrParams[3] + CDFOX.RS_CD_Spit
                         + arrParams[4] + CDFOX.RS_CD_Spit
                         + arrParams[5] + CDFOX.RS_CD_Spit
                         + arrParams[6] + CDFOX.RS_END_LINE);
                    using (FileStream fs = new FileStream(strFile, FileMode.Create))
                    {
                        StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                        sw.Write("SerialNumber,WorkcenterName,ProductName,Barcode,StartDate,EndDate,TestResult" + CDFOX.RS_END_LINE
                            + strBld);
                        sw.Close();
                        //log.WriteLog("Function Success !多拼板 导出成功 !", "CaiHuang");
                        AppLogHelp.WriteLog(LogFileFormate.MES, "CaiHuang Function Success !多拼板 导出成功 !");

                    }
                }
                else
                {
                    strBld.Append(arrParams[0] + CDFOX.RS_CD_Spit
                         + arrParams[1] + CDFOX.RS_CD_Spit
                         + arrParams[2] + CDFOX.RS_CD_Spit
                         + arrParams[3] + CDFOX.RS_CD_Spit
                         + arrParams[4] + CDFOX.RS_CD_Spit
                         + arrParams[5] + CDFOX.RS_CD_Spit
                         + arrParams[6] + CDFOX.RS_END_LINE);
                    using (FileStream fs = new FileStream(strFile, FileMode.Append))
                    {
                        StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                        sw.Write(strBld);
                        sw.Close();
                        //log.WriteLog("Function Success !仅一个单板 导出成功 !", "CaiHuang");
                        AppLogHelp.WriteLog(LogFileFormate.MES, "CaiHuang Function Success !仅一个单板 导出成功 !");
                    }
                }
            }
            catch (Exception ex)
            {
                string error = "  Function: (CreateOrAppendFile)  出错 ";
                //log.WriteErr("错误 ! " + error + ex.ToString());
                AppLogHelp.WriteError(LogFileFormate.MES, "CreateOrAppendFile--" + error + ex.ToString());
            }

        }
        #endregion

        #region" 深圳雷能 "
        
        private string _strFromUIFile = @"D:\EYSPI\Bin\SPILogs\Peng\LeiNeng\infomation.txt";
        private string _strFromAutoAPPFile = @"D:\EYSPI\Bin\SPILogs\Peng\LeiNeng\from_autoapp.txt";
        public void startLNRun( )
        {
            System.Threading.Thread lnThread;
            
            try
            {
                lnThread = new System.Threading.Thread(new System.Threading.ThreadStart(CountThreadProcLeiNeng));
                lnThread.Priority = System.Threading.ThreadPriority.Normal;
                lnThread.Start();
            }catch(Exception ex)
            {
                //log.WriteErr("错误!"+ ex.Message,"雷能");
                AppLogHelp.WriteError(LogFileFormate.MES, "雷能" + ex.Message);
            }
        }
        private void CountThreadProcLeiNeng()
        {
            string strPath = string.Empty;
            string strResultXML = string.Empty,strMsg = string.Empty;
            string strMsgErr = string.Empty;
            bool bResult;
            try
            {
                while (true)
                {
                    strPath = Path.GetDirectoryName(_strFromAutoAPPFile);
                    if (Directory.Exists(strPath) == false)
                    {
                        Directory.CreateDirectory(strPath);
                    }
                    if (File.Exists(_strFromUIFile))
                    {
                        System.Threading.Thread.Sleep(300); //等待文件写入;
                        string strXML = File.ReadAllText(_strFromUIFile,Encoding.Default);
                        try
                        {
                            WebReference.IWebServiceservice webService = new WebReference.IWebServiceservice();
                            bResult = webService.getIMSData("CheckBarCode", strXML, ref strResultXML, ref strMsg);
                           // _log.WriteErr("log:" + bResult, "雷能");
                        }
                        catch (Exception ex)
                        {
                            strMsgErr = strResultXML + ex.Message;
                            File.WriteAllText(_strFromAutoAPPFile, strMsgErr, Encoding.Default);
                           // _log.WriteErr("错误:" + strMsgErr, "雷能");
                            AppLogHelp.WriteError(LogFileFormate.MES, "雷能 错误:" + strMsgErr);
                        }
                        if (string.IsNullOrEmpty(strResultXML) && string.IsNullOrEmpty(strMsgErr))
                        {
                            //_log.WriteLog("success", "雷能");
                            AppLogHelp.WriteLog(LogFileFormate.MES, "雷能 success" );
                        }
                        else
                        {
                            //_log.WriteErr("错误:" + strMsgErr + strResultXML, "雷能");
                            AppLogHelp.WriteLog(LogFileFormate.MES, "雷能 success" + strMsgErr + strResultXML);
                        }
                        File.WriteAllText(_strFromAutoAPPFile, strMsgErr + strResultXML, Encoding.Default);
                        //delete ui file
                        File.Delete(_strFromUIFile);
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
            catch (Exception e)
            {
                //_log.WriteErr("错误!" + e.Message, "雷能[UpDateTheLeiNengEx]");
            }
        }

        #endregion

        #region "昆山仁宝 "
        //lin 20190813
        //private   string _strConnectString = "server=127.0.0.1;user id=root;database=spidb;Charset=utf8";
        private string _strConnectString = WSClnt.PubStaticParam._strSPIdbConnectionString;
        private string _strLogPengPath = @"D:\EYSPI\Bin\SPILogs\Peng";
        private string _strRenBao_Title = "TIME,LINE,MachineID,EventCode,EventMessage,ErrorCode,ErrorMessage";
        private string _strEndLine = "\r\n";
        private string _strSpit = ",";
        private string _strCsvPath = "";
        private string _strCsvPath2 = "";
        

        public void StartRenBao(string strCsvPath,string strCsvPath2)
        {
            try
            {
                //this._log = log;
                this._strCsvPath = strCsvPath;
                this._strCsvPath2 = strCsvPath2;
                StartThreadForRenBao();
            }
            catch (Exception e)
            {
                //_log.WriteErr("错误:" + e.Message, "线程RenBao");
            }
        }
        private void SaveDataForRenBao()
        {
            try
            {
                //TmpDir
                _strLogPengPath = Path.Combine(_strLogPengPath, "RenBao");
                string strLogDataPath = Path.Combine(_strLogPengPath, "RenBaoToData");
                if (Directory.Exists(_strLogPengPath) == false)
                {
                    Directory.CreateDirectory(_strLogPengPath);
                }
                if (Directory.Exists(_strCsvPath) == false)
                {
                    Directory.CreateDirectory(_strCsvPath);
                }
                if (Directory.Exists(_strCsvPath2) == false)
                {
                    Directory.CreateDirectory(_strCsvPath2);
                }
                if (Directory.Exists(strLogDataPath) == false)
                {
                    Directory.CreateDirectory(strLogDataPath);
                }
                string strRenBaoFileByStart = Path.Combine(_strLogPengPath, "renbao_start.txt");
                string strRenBaoFileByStop = Path.Combine(_strLogPengPath, "renbao_stop.txt");
                string strRenBaoFileByError = Path.Combine(_strLogPengPath, "renbao_error.txt");
                string strRenBaoFileByErrorReset = Path.Combine(_strLogPengPath, "renbao_errorReset.txt");

                string strRenBaoFileByReSig = Path.Combine(strLogDataPath, "renbao_ReSig.txt");
                string strRenBaoFileByInBoard = Path.Combine(strLogDataPath, "renbao_InBoard.txt");
                string strRenBaoFileByInSpect = Path.Combine(strLogDataPath, "renbao_InSpect.txt");
                string strRenBaoFileByReadyOut = Path.Combine(strLogDataPath, "renbao_ReadyOut.txt");
                string strRenBaoFileByBVSig = Path.Combine(strLogDataPath, "renbao_BVSig.txt");
                string strRenBaoFileByOutBoard = Path.Combine(strLogDataPath, "renbao_OutBoard.txt");
                string strRenBaoFileByInspectChecked = Path.Combine(strLogDataPath, "renbao_InspectChecked.txt"); //true false  ini默认false
                string strRenBaoFileByLastOutBoard = Path.Combine(strLogDataPath, "renbao_LastOutBoard.txt");
                string strRenBaoFileByInspectTime = Path.Combine(strLogDataPath, "renbao_InspectTime.txt");

                string strStartAndReset = Path.Combine(_strLogPengPath, "renbao_startAndReset.txt");
                System.Data.DataTable dataTable = null; System.Data.DataTable dataTablePCB = null; DataTable dtArrayResult = null;
                //初始化
                IniFileRenBaoReset(strRenBaoFileByStart, strRenBaoFileByStop, strRenBaoFileByError, strStartAndReset, strRenBaoFileByErrorReset);
                IniFileRenBaoReset(strRenBaoFileByInBoard, strRenBaoFileByInSpect, strRenBaoFileByReadyOut,
                    strRenBaoFileByBVSig, strRenBaoFileByOutBoard, strRenBaoFileByLastOutBoard, strRenBaoFileByInspectTime, strRenBaoFileByReSig, strRenBaoFileByInspectChecked);
                DateTime dtNow = DateTime.Now;

                string strStart = string.Empty, strError = string.Empty,
                                strStop = string.Empty, strRun = string.Empty;
                string strTime = string.Empty;
                string strTime2 = string.Empty;
                string strFileNameTime = string.Empty;
                string strLineName = string.Empty;
                string strMachine = string.Empty;
                string strErrorContent = string.Empty;

                string strFactory = string.Empty;
                string strCustor = string.Empty;
                string strFool = string.Empty;
                string strEquipName = string.Empty;
                string strSide = string.Empty;
                //string strEquipID = dataTable.Rows[i][17].ToString();
                string strModule = string.Empty;
                string strErrorContentEnglish = string.Empty;
                //GetErrorCodeForStrEquipStatus(strErrorContent, ref strErrorContentEnglish);
                string strFile = string.Empty;//Path.Combine(_strCsvPath, "Status_SPI_" + strFileNameTime);
                string strOP = string.Empty;
                string strFileContent = string.Empty;
                Random rand = new Random();
                string strAutoAPPIniFile = @"D:\EYSPI\Bin\Config\autoApp.ini";
                string arraybarcode1 = "", arraybarcode2 = "", strarrayResult1 = "", strarrayResult2 = "";
                //test
                //dtNow = Convert.ToDateTime("2018-09-17 11:07:19");
                DateTime dtTMP = dtNow;
                string strSql = "", strSqlForPCBInfo = "";
                string[] arrStutsKey5 = new string[5];
                string[] arrDataValue5 = new string[5];
                string[] arrStrKey = new string[8];
                string[] arrStrVaule = new string[8];
                string[] arrStrKey6 = new string[6];
                string[] arrStrVaule6 = new string[6];
                arrStrKey[0] = "TIME";
                arrStrKey[1] = "Program";
                arrStrKey[2] = "InWaitingTime";
                arrStrKey[3] = "OutWaitingTime";
                arrStrKey[4] = "SN1";
                arrStrKey[5] = "Result1";
                arrStrKey[6] = "SN2";
                arrStrKey[7] = "Result2";
                arrStutsKey5[0] = "TIME";
                arrStutsKey5[1] = "EventCode";
                arrStutsKey5[2] = "EventMessage";
                arrStutsKey5[3] = "ErrorCode";
                arrStutsKey5[4] = "ErrorMessage";
                arrStrKey6[0] = "TIME";
                arrStrKey6[1] = "Program";
                arrStrKey6[2] = "InWaitingTime";
                arrStrKey6[3] = "OutWaitingTime";
                arrStrKey6[4] = "SN";
                arrStrKey6[5] = "Result";
                string KunShanRenBao = "KunShanRenBao", tbRenBaoSaoplant = "tbRenBaoSaoplant", tbRenBaoGroupName = "tbRenBaoGroupName",
                                    tbRenBaoLineName = "tbRenBaoLineName", tbRenBaoCuscode = "tbRenBaoCuscode", tbRenBaoPross = "tbRenBaoPross",
                                    tbRenBaoFactorySN = "tbRenBaoFactorySN", tbRenBaoSide = "tbRenBaoSide", tbRenBaoEquipType = "tbRenBaoEquipType",
                                    tbRenBaoOp = "tbRenBaoOp";
                while (true)
                {
                    //DBdata获取 设备信息
                    strSql = "SELECT " +
                                    "stu.Line," +
                                    "stu.EquipID," +
                                    "stu.`Start`," +
                                    "stu.`Stop`," +
                                    "stu.Error," +
                                    "stu.`Run`, " +
                                    "stu.ErrContent," +
                                    "stu.UpdateTime ," +
                                    " stu.ReqSig,stu.InBoard,stu.InspectBoard,stu.ReadyOut,stu.BVSig,stu.OutBoard,  " + // index:8
                        // index 13
                                    " stu.Factory, " +
                                    " stu.Customer, " +
                                    " stu.Floor, " +
                                    " stu.EquipName, " +
                        //" stu.EquipID, "+
                                    " stu.Module " +
                                "FROM " +
                                    "spidb.tbequipstatus stu " +
                                "WHERE " +
                                    "stu.UpdateTime >= '" + dtTMP + "' ORDER BY stu.UpdateTime;";
                    // strSql

                    dataTable = getDataTableForZZFox(_strConnectString, strSql);

                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        //
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            strStart = dataTable.Rows[i][2].ToString(); strError = dataTable.Rows[i][4].ToString();
                            strStop = dataTable.Rows[i][3].ToString(); strRun = dataTable.Rows[i][5].ToString();
                            strTime = ((DateTime)dataTable.Rows[i][7]).ToString("yyyy/MM/dd HH:mm:ss");
                            strTime2 = ((DateTime)dataTable.Rows[i][7]).ToString("yyyy-MM-dd HH:mm:ss");
                            strFileNameTime = ((DateTime)dataTable.Rows[i][7]).ToString("yyyyMMddHHmmss");
                            strLineName = dataTable.Rows[i][0].ToString();
                            strMachine = dataTable.Rows[i][1].ToString();
                            strErrorContent = dataTable.Rows[i][6].ToString();

                            strFactory = dataTable.Rows[i][13].ToString();
                            strCustor = dataTable.Rows[i][14].ToString();
                            strFool = dataTable.Rows[i][15].ToString();
                            strEquipName = dataTable.Rows[i][16].ToString();
                            strSide = "";
                            //string strEquipID = dataTable.Rows[i][17].ToString();
                            strModule = dataTable.Rows[i][17].ToString();
                            strErrorContentEnglish = "";
                            GetErrorCodeForStrEquipStatus(strErrorContent, ref strErrorContentEnglish);
                            strFile = "";//Path.Combine(_strCsvPath, "Status_SPI_" + strFileNameTime);
                            strOP = "";
                            strFileContent = _strRenBao_Title + _strEndLine;

                            
                            if (File.Exists(strAutoAPPIniFile))
                            {
                                strFactory = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoSaoplant, "", strAutoAPPIniFile);
                                strModule = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoGroupName, "", strAutoAPPIniFile);
                                strLineName = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoLineName, "", strAutoAPPIniFile);

                                strCustor = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoCuscode, "", strAutoAPPIniFile);
                                strFool = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoPross, "", strAutoAPPIniFile);
                                strMachine = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoFactorySN, "", strAutoAPPIniFile);

                                strSide = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoSide, "", strAutoAPPIniFile);
                                strEquipName = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoEquipType, "", strAutoAPPIniFile);
                                strOP = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoOp, "", strAutoAPPIniFile);
                            }

                            //此模块记录PCB进板出板时间
                            // RQ进板信号
                            if (string.IsNullOrEmpty(dataTable.Rows[i][8].ToString()) == false && dataTable.Rows[i][8].ToString() == "True")
                            {
                                if (File.Exists(strRenBaoFileByReSig))
                                {
                                    if (string.IsNullOrEmpty(File.ReadAllText(strRenBaoFileByReSig, Encoding.Default)))
                                    {
                                        File.WriteAllText(strRenBaoFileByReSig, strTime2, Encoding.Default);

                                    }
                                }
                            }
                            //InBoard
                            if (string.IsNullOrEmpty(dataTable.Rows[i][9].ToString()) == false && dataTable.Rows[i][9].ToString() == "True")
                            {
                                if (File.Exists(strRenBaoFileByInBoard))
                                {
                                    if (string.IsNullOrEmpty(File.ReadAllText(strRenBaoFileByInBoard, Encoding.Default)))
                                    {
                                        File.WriteAllText(strRenBaoFileByInBoard, strTime2, Encoding.Default);

                                    }
                                }
                            }
                            //Inspect
                            if (string.IsNullOrEmpty(dataTable.Rows[i][10].ToString()) == false && dataTable.Rows[i][10].ToString() == "True")
                            {
                                if (File.Exists(strRenBaoFileByInSpect))
                                {
                                    if (string.IsNullOrEmpty(File.ReadAllText(strRenBaoFileByInSpect, Encoding.Default)))
                                    {
                                        File.WriteAllText(strRenBaoFileByInSpect, strTime2, Encoding.Default);

                                        if (File.Exists(strRenBaoFileByInspectChecked))
                                        {
                                            File.WriteAllText(strRenBaoFileByInspectChecked, "true", Encoding.Default);
                                        }
                                    }

                                    if (File.Exists(strRenBaoFileByInspectTime))
                                    {
                                        File.WriteAllText(strRenBaoFileByInspectTime, strTime2, Encoding.Default);
                                    }
                                }
                            }
                            //ReadyOut
                            if (string.IsNullOrEmpty(dataTable.Rows[i][11].ToString()) == false && dataTable.Rows[i][11].ToString() == "True")
                            {
                                if (File.Exists(strRenBaoFileByReadyOut))
                                {
                                    if (string.IsNullOrEmpty(File.ReadAllText(strRenBaoFileByReadyOut, Encoding.Default)))
                                    {
                                        File.WriteAllText(strRenBaoFileByReadyOut, strTime2, Encoding.Default);
                                    }
                                }
                            }
                            //BVsig
                            if (string.IsNullOrEmpty(dataTable.Rows[i][12].ToString()) == false && dataTable.Rows[i][12].ToString() == "True")
                            {
                                if (File.Exists(strRenBaoFileByBVSig))
                                {
                                    if (string.IsNullOrEmpty(File.ReadAllText(strRenBaoFileByBVSig, Encoding.Default)))
                                    {
                                        File.WriteAllText(strRenBaoFileByBVSig, strTime2, Encoding.Default);
                                    }
                                }
                            }
                            //outBoard
                            //string out11 = dataTable.Rows[i][13].ToString();
                            if (string.IsNullOrEmpty(dataTable.Rows[i][13].ToString()) == false && dataTable.Rows[i][13].ToString() == "True")
                            {

                                if (File.Exists(strRenBaoFileByInspectChecked))
                                {
                                    //如果此时没有check inspect 那么就记录lastOutBoard;
                                    if (File.ReadAllText(strRenBaoFileByInspectChecked) == "true")
                                    {
                                        File.WriteAllText(strRenBaoFileByLastOutBoard, strTime2, Encoding.Default);

                                        string strJobName = "", strBarcode = "", strResult = "";
                                        double iWaitBoardInTime = 0, iWaitBoardOutTime = 0;
                                        string ttTime = File.ReadAllText(strRenBaoFileByInSpect, Encoding.Default);
                                        DateTime ttDateTime = DateTime.Parse(ttTime);
                                        strSqlForPCBInfo = "SELECT " +
                                        "pcb.PCBBarcode, " +
                                        "job.JobName, " +
                                        " pcb.Result, " +
                                        " pcb.Operator, " +
                                        " pcb.PCBID, " +
                                        " pcb.StartTime  " +
                                        " FROM " +
                                        "    spidb.tbboard AS pcb, " +
                                        "    spidb.TBJobInfo AS job " +
                                        "WHERE " +
                                            //"    pcb.StartTime >= '{0}' " +
                                        " pcb.EndTime >= '" + ttDateTime + "' " +
                                        "AND job.SerNo = pcb.JobIndex ;";
                                        dataTablePCB = getDataTableForZZFox(_strConnectString, strSqlForPCBInfo);
                                        if (dataTablePCB != null && dataTablePCB.Rows.Count > 0)
                                        {
                                            //wait 10s 人员判定
                                            System.Threading.Thread.Sleep(8000);

                                            for (int iBarIndex = 0; iBarIndex < dataTablePCB.Rows.Count; iBarIndex++)
                                            {
                                                strBarcode = dataTablePCB.Rows[iBarIndex][0].ToString();
                                                if (string.IsNullOrEmpty(strBarcode))
                                                {
                                                    strBarcode = "scan off";
                                                }
                                                strJobName = dataTablePCB.Rows[iBarIndex][1].ToString();
                                                strResult = dataTablePCB.Rows[iBarIndex][2].ToString();
                                                //string strOperator = dataTablePCB.Rows[iBarIndex][3].ToString();
                                                //strOP = strOperator;
                                                string strPCBID = dataTablePCB.Rows[iBarIndex][4].ToString();   //pcbid
                                                string strPCBstartTime = dataTablePCB.Rows[iBarIndex][5].ToString();   //startTime
                                                string strPCBFilestartTime = ((DateTime)dataTablePCB.Rows[iBarIndex][5]).ToString("yyyyMMddHHmmss");
                                                if (strResult == "0" || strResult == "2")
                                                {
                                                    strResult = "PASS";
                                                }
                                                else
                                                {
                                                    strResult = "FAIL";
                                                }

                                                //get the array
                                                string strArraySql = " SELECT " +
                                                 "   bc.ArrayBarCode, " +
                                                 " bc.ArrayID  " +

                                                " FROM " +
                                                "    spidb.TBBarCode as bc " +
                                                " WHERE " +
                                                "  bc.PCBID = '" + strPCBID + "' AND " +
                                                "  bc.BarCode = '" + strBarcode + "'";
                                                string strResultArraySql = "";


                                                //获取候板时间
                                                DateTime dtInBoardTime = new DateTime(), dtBVTime = new DateTime(), dtRSTime = new DateTime();

                                                if (File.Exists(strRenBaoFileByInBoard))
                                                {
                                                    string tpInBoardTime = File.ReadAllText(strRenBaoFileByInBoard, Encoding.Default);
                                                    if (string.IsNullOrEmpty(tpInBoardTime) == false)
                                                    {
                                                        dtInBoardTime = Convert.ToDateTime(tpInBoardTime);
                                                    }
                                                    else if (string.IsNullOrEmpty(File.ReadAllText(strRenBaoFileByInSpect, Encoding.Default)) == false)
                                                    {
                                                        dtInBoardTime = Convert.ToDateTime(File.ReadAllText(strRenBaoFileByInSpect, Encoding.Default));
                                                    }
                                                }
                                                if (File.Exists(strRenBaoFileByReSig))
                                                {
                                                    string tpReSigTime = File.ReadAllText(strRenBaoFileByReSig, Encoding.Default);
                                                    if (string.IsNullOrEmpty(tpReSigTime) == false)
                                                    {
                                                        dtRSTime = Convert.ToDateTime(tpReSigTime);
                                                    }
                                                }
                                                if (File.Exists(strRenBaoFileByBVSig))
                                                {
                                                    string tpBVSigTime = File.ReadAllText(strRenBaoFileByBVSig, Encoding.Default);
                                                    if (string.IsNullOrEmpty(tpBVSigTime) == false)
                                                    {
                                                        dtBVTime = Convert.ToDateTime(tpBVSigTime);
                                                    }
                                                    else if (string.IsNullOrEmpty(File.ReadAllText(strRenBaoFileByReadyOut, Encoding.Default)) == false)
                                                    {
                                                        dtBVTime = Convert.ToDateTime(File.ReadAllText(strRenBaoFileByReadyOut, Encoding.Default));
                                                    }
                                                    else if (string.IsNullOrEmpty(File.ReadAllText(strRenBaoFileByInspectTime, Encoding.Default)) == false)
                                                    {
                                                        dtBVTime = Convert.ToDateTime(File.ReadAllText(strRenBaoFileByInspectTime, Encoding.Default));
                                                    }
                                                }


                                                if (dtRSTime != null && dtInBoardTime != null)
                                                {
                                                    iWaitBoardInTime = (dtInBoardTime - dtRSTime).TotalSeconds;
                                                    iWaitBoardInTime = iWaitBoardInTime + 0.1 * rand.Next(1, 5);

                                                }
                                                if (dtBVTime != null && strTime2 != null)
                                                {
                                                    // iWaitBoardOutTime = (Convert.ToDateTime(strTime2) - dtBVTime).TotalSeconds;
                                                }

                                                if (iWaitBoardInTime <= 0 || iWaitBoardInTime > 100)
                                                {
                                                    iWaitBoardInTime = 0.1 * rand.Next(5, 9);
                                                    //strWaitBoardTime = iWaitBoardInTime+"";
                                                }
                                                //if (iWaitBoardOutTime < 0) iWaitBoardOutTime = 1;
                                                iWaitBoardOutTime = 0.1 * rand.Next(5, 10);


                                                string strErrorMessage = "";
                                                //等待人员判断
                                                //DataTable dtTest;
                                                //while (true)
                                                //{

                                                //    dtTest = getDataTableForZZFox(_strConnectString, "SELECT pcb.PCBID FROM spidb.tbboard pcb WHERE pcb.PCBID = '" + strPCBID + "' AND pcb.OPConfirmed =1;", _log);
                                                //    if(dtTest !=null)
                                                //}

                                                DataTable dtArrayPCB = getDataTableForZZFox(_strConnectString, strArraySql );
                                                if (dtArrayPCB != null && dtArrayPCB.Rows.Count > 0)
                                                {
                                                    if (dtArrayPCB.Rows.Count > 1)
                                                    {
                                                        for (int n = 0; n < dtArrayPCB.Rows.Count; n++)
                                                        {
                                                            string arrbarcode = dtArrayPCB.Rows[n][0].ToString();
                                                            if (string.IsNullOrEmpty(arrbarcode) || string.Equals("noread", arrbarcode, StringComparison.CurrentCultureIgnoreCase))
                                                            {
                                                                arrbarcode = "scan off";
                                                            }
                                                            //if (string.IsNullOrEmpty(arrbarcode)) { arrbarcode = strBarcode; }
                                                            string strarrayID = dtArrayPCB.Rows[n][1].ToString();
                                                            string strarrayResult = "PASS";
                                                            strResultArraySql = "SELECT COUNT(PCBID) FROM spidb.TBPadMeasure as pad WHERE pad.PCBID = '" + strPCBID + "' AND pad.ArrayIDIndex ='" + strarrayID + "' AND pad.JudgeRes = '1'";//string.Format(strResultArraySql, strarrayID);
                                                            dtArrayResult = getDataTableForZZFox(_strConnectString, strResultArraySql );
                                                            if (dtArrayResult != null && dtArrayResult.Rows.Count > 0)
                                                            {
                                                                if (dtArrayResult.Rows[0][0].ToString() != "0")
                                                                {
                                                                    strarrayResult = "FAIL";
                                                                }
                                                                else
                                                                {
                                                                    strarrayResult = "PASS";
                                                                }
                                                            }
                                                            if (n == 0)
                                                            {
                                                                arraybarcode1 = arrbarcode;
                                                                if (arraybarcode1 == "scan off")
                                                                {
                                                                    arraybarcode1 = strBarcode;
                                                                }
                                                                strarrayResult1 = strarrayResult;

                                                            }
                                                            if (n == 1)
                                                            {

                                                                arraybarcode2 = arrbarcode + "";
                                                                strarrayResult2 = strarrayResult;
                                                            }
                                                        }
                                                        //输出csv文件;
                                                        StringBuilder sb = new StringBuilder();
                                                        sb.Append("TIME" + _strSpit +
                                                            "LINE" + _strSpit +
                                                            "MACHINEID" + _strSpit +
                                                            "Program" + _strSpit +
                                                            "InWaitingTime" + _strSpit +
                                                            "OutWaitingTime" + _strSpit +
                                                            "SN1" + _strSpit +
                                                            "Result1 " + _strSpit +
                                                            "SN2" + _strSpit +
                                                            "Result2" +
                                                            _strEndLine);
                                                        sb.Append(strPCBstartTime + _strSpit
                                                            + strLineName + _strSpit
                                                            + strMachine + _strSpit
                                                            + strJobName + _strSpit
                                                            + iWaitBoardInTime + _strSpit
                                                            + iWaitBoardOutTime + _strSpit
                                                            + arraybarcode1 + _strSpit
                                                            + strarrayResult1 + _strSpit
                                                            + arraybarcode2 + _strSpit
                                                            + strarrayResult2 + _strEndLine);
                                                        arrStrVaule[0] = strPCBstartTime;
                                                        arrStrVaule[1] = strJobName;
                                                        arrStrVaule[2] = iWaitBoardInTime + "";
                                                        arrStrVaule[3] = iWaitBoardOutTime + "";
                                                        arrStrVaule[4] = arraybarcode1;
                                                        arrStrVaule[5] = strarrayResult1;
                                                        arrStrVaule[6] = arraybarcode2;
                                                        arrStrVaule[7] = strarrayResult2;
                                                        string strFile2 = Path.Combine(this._strCsvPath2, "Data_SPI" + "_" + strPCBFilestartTime + ".csv");

                                                        File.WriteAllText(strFile2, sb.ToString(), Encoding.Default);

                                                        //Ini  初始化文件;
                                                        IniFileRenBaoReset(strRenBaoFileByInBoard, strRenBaoFileByInSpect, strRenBaoFileByReadyOut,
                                                                            strRenBaoFileByBVSig, strRenBaoFileByOutBoard, strRenBaoFileByLastOutBoard,
                                                                            strRenBaoFileByInspectTime, strRenBaoFileByReSig, strRenBaoFileByInspectChecked);
                                                        
                                                        bool bResult = UploadDataFileForRenBaoMES(strFactory, strLineName, strModule, strCustor, strFool, strMachine, strEquipName, strOP,strSide, "Data_SPI" + "_" + strPCBFilestartTime + ".csv", out strErrorMessage, arrStrVaule, arrStrKey);
                                                        sb.Clear();
                                                        if (bResult)
                                                        {
                                                            //_log.WriteLog("导出array本地测试文件并上抛成功--", "线程RenBao");
                                                            AppLogHelp.WriteLog(LogFileFormate.MES, "线程RenBao 导出array本地测试文件并上抛成功--");
                                                        }
                                                        else
                                                        {
                                                            //_log.WriteErr("上抛array失败错误信息:" + strErrorMessage, "线程RenBao");
                                                            AppLogHelp.WriteLog(LogFileFormate.MES, "线程RenBao 上抛array失败错误信息:" + strErrorMessage);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //输出csv文件;
                                                        StringBuilder sb = new StringBuilder();
                                                        sb.Append("TIME" + _strSpit +
                                                            "LINE" + _strSpit +
                                                            "MACHINEID" + _strSpit +
                                                            "Program" + _strSpit +
                                                            "InWaitingTime" + _strSpit +
                                                            "OutWaitingTime" + _strSpit +
                                                            "SN" + _strSpit +
                                                            "Result" +
                                                            //"SN2 " + _strSpit +
                                                            //"Result2" +
                                                            _strEndLine);
                                                        sb.Append(strPCBstartTime + _strSpit
                                                            + strLineName + _strSpit
                                                            + strMachine + _strSpit
                                                            + strJobName + _strSpit
                                                            + iWaitBoardInTime + _strSpit
                                                            + iWaitBoardOutTime + _strSpit
                                                            + strBarcode + _strSpit
                                                            + strResult + _strEndLine);

                                                        
                                                        arrStrVaule6[0] = strPCBstartTime;
                                                        arrStrVaule6[1] = strJobName;
                                                        arrStrVaule6[2] = iWaitBoardInTime + "";
                                                        arrStrVaule6[3] = iWaitBoardOutTime + "";
                                                        arrStrVaule6[4] = strBarcode;
                                                        arrStrVaule6[5] = strResult;

                                                        string strFile2 = Path.Combine(this._strCsvPath2, "Data_SPI" + "_" + strPCBFilestartTime + ".csv");

                                                        File.WriteAllText(strFile2, sb.ToString(), Encoding.Default);
                                                        
                                                        bool bResult = UploadDataFileForRenBaoMES(strFactory, strLineName, strModule, strCustor, strFool, strMachine, strEquipName, strOP,strSide, "Data_SPI" + "_" + strPCBFilestartTime + ".csv", out strErrorMessage, arrStrVaule6, arrStrKey6);
                                                        //Ini  初始化文件;
                                                        IniFileRenBaoReset(strRenBaoFileByInBoard, strRenBaoFileByInSpect, strRenBaoFileByReadyOut,
                                                                            strRenBaoFileByBVSig, strRenBaoFileByOutBoard, strRenBaoFileByLastOutBoard,
                                                                            strRenBaoFileByInspectTime, strRenBaoFileByReSig, strRenBaoFileByInspectChecked);
                                                        if (bResult)
                                                        {
                                                            //_log.WriteLog("导出测试文件并上抛成功--", "线程RenBao");
                                                            AppLogHelp.WriteLog(LogFileFormate.MES, "线程RenBao导出测试文件并上抛成功--");
                                                        }
                                                        else
                                                        {
                                                            //_log.WriteErr("上抛失败错误信息:" + strErrorMessage, "线程RenBao");
                                                            AppLogHelp.WriteError(LogFileFormate.MES, "线程RenBao上抛失败错误信息--" + strErrorMessage);
                                                        }
                                                        //_log.WriteLog("导出array本地测试文件并上抛成功--", "线程RenBao");
                                                        sb.Clear();
                                                    }
                                                }
                                                strResultArraySql = string.Empty; strArraySql = string.Empty;
                                                strPCBID = string.Empty;   //pcbid
                                                strPCBstartTime = string.Empty;   //startTime
                                                strPCBFilestartTime = string.Empty;
                                            }
                                        }
                                        else
                                        {
                                            //_log.WriteErr(ttTime + "_时间 检查", "仁宝");
                                            //Ini  初始化文件;
                                            IniFileRenBaoReset(strRenBaoFileByInBoard, strRenBaoFileByInSpect, strRenBaoFileByReadyOut,
                                                                strRenBaoFileByBVSig, strRenBaoFileByOutBoard, strRenBaoFileByLastOutBoard,
                                                                strRenBaoFileByInspectTime, strRenBaoFileByReSig, strRenBaoFileByInspectChecked);
                                        }
                                        strJobName = string.Empty; strBarcode = string.Empty; strResult = string.Empty;
                                        iWaitBoardInTime = 0; iWaitBoardOutTime = 0;
                                        
                                    }
                                }
                                else
                                {
                                    // 记录lastOutBoard
                                }
                            }

                            dtTMP = (DateTime)dataTable.Rows[i][7];

                            

                            string strErrormessage = "";
                            //error
                            if (strError == "True")
                            {
                                if (string.IsNullOrEmpty(File.ReadAllText(strRenBaoFileByError)))
                                {
                                    // error
                                    strFileContent = strFileContent +
                                    strTime + _strSpit +
                                    strLineName + _strSpit +
                                    strMachine + _strSpit +
                                    "C02" + _strSpit +
                                    "TroubleStopStart" + _strSpit +
                                    strErrorContent + _strSpit +
                                    strErrorContentEnglish ;
                                    strFile = Path.Combine(_strCsvPath, "Status_SPI_" + dtTMP.ToString("yyyyMMddHHmmss") + ".csv");
                                    arrDataValue5[0] = strTime;
                                    arrDataValue5[1] = "C02";
                                    arrDataValue5[2] = "TroubleStopStart";
                                    arrDataValue5[3] = strErrorContent;
                                    arrDataValue5[4] = strErrorContentEnglish;
                                    File.WriteAllText(strFile, strFileContent, Encoding.Default);
                                    File.WriteAllText(strStartAndReset, "StartAndReset", Encoding.Default);
                                    File.WriteAllText(strRenBaoFileByError, "error", Encoding.Default);
                                    bool bResult = UploadDataFileForRenBaoMES(strFactory, strLineName, strModule, strCustor, strFool, strMachine, strEquipName, strOP, strSide, "Status_SPI_" + dtTMP.ToString("yyyyMMddHHmmss") + ".csv", out strErrormessage, arrDataValue5, arrStutsKey5);

                                    if (bResult)
                                    {
                                        //_log.WriteLog("设备状态C02本地文成功--", "线程RenBao");
                                        AppLogHelp.WriteLog(LogFileFormate.MES, "设备状态C02本地文成功--");
                                        //SaveDataForZHEJIANGDAHUA
                                    }
                                    else
                                    {
                                       //_log.WriteErr("设备状态C02上抛失败错误信息:" + strErrormessage, "线程RenBao");
                                        AppLogHelp.WriteError(LogFileFormate.MES, "设备状态C02上抛失败错误信息:" + strErrormessage);
                                    }
                                }
                            }
                            //软件点开始
                            else if (strStart == "True" && strRun == "True")
                            {
                                //读取本地临时文件，如果此时存在，那么读取做出判定; 如果不存在则创建一个新的文件
                                //  第一种情况  软件点开始没有 并非解除异常
                                if (string.IsNullOrEmpty(File.ReadAllText(strStartAndReset)))
                                {
                                    if (string.IsNullOrEmpty(File.ReadAllText(strRenBaoFileByStart)))
                                    {
                                        strFileContent = strFileContent +
                                            strTime + _strSpit +
                                            strLineName + _strSpit +
                                            strMachine + _strSpit +

                                            "C01" + _strSpit +
                                            "ProductionStart" + _strSpit +
                                            "null" + _strSpit +
                                            "null" ;
                                        arrDataValue5[0] = strTime;
                                        arrDataValue5[1] = "C01";
                                        arrDataValue5[2] = "ProductionStart";
                                        arrDataValue5[3] = "";
                                        arrDataValue5[4] = "";
                                        //输出csv
                                        strFile = Path.Combine(_strCsvPath, "Status_SPI_" + dtTMP.ToString("yyyyMMddHHmmss") + ".csv");
                                        File.WriteAllText(strFile, strFileContent, Encoding.Default);
                                        File.WriteAllText(strRenBaoFileByStart, "start", Encoding.Default);
                                        File.WriteAllText(strRenBaoFileByStop, "", Encoding.Default);
                                        File.WriteAllText(strRenBaoFileByError, "", Encoding.Default);

                                        bool bResult = UploadDataFileForRenBaoMES(strFactory, strLineName, strModule, strCustor, strFool, strMachine, strEquipName, strOP,strSide, "Status_SPI_" + dtTMP.ToString("yyyyMMddHHmmss") + ".csv", out strErrormessage, arrDataValue5, arrStutsKey5);

                                        if (bResult)
                                        {
                                            //_log.WriteLog("设备状态C01本地文件成功--", "线程RenBao");
                                            AppLogHelp.WriteLog(LogFileFormate.MES, "设备状态C01本地文件成功--");
                                        }
                                        else
                                        {
                                            //_log.WriteErr("设备状态C01上抛失败错误信息:" + strErrormessage, "线程RenBao");
                                            AppLogHelp.WriteError(LogFileFormate.MES, "设备状态C01上抛失败错误信息"+ strErrormessage);
                                        }
                                    }
                                }
                                else
                                {
                                    //解除异常状态 3
                                    strFileContent = strFileContent +
                                    strTime + _strSpit +
                                    strLineName + _strSpit +
                                    strMachine + _strSpit +
                                    "C03" + _strSpit +
                                    "TroubleStopEnd" + _strSpit +
                                    "null" + _strSpit +
                                    "null" ;
                                    //输出csv
                                    arrDataValue5[0] = strTime;
                                    arrDataValue5[1] = "C03";
                                    arrDataValue5[2] = "TroubleStopEnd";
                                    arrDataValue5[3] = "";
                                    arrDataValue5[4] = "";
                                    strFile = Path.Combine(_strCsvPath, "Status_SPI_" + dtTMP.ToString("yyyyMMddHHmmss") + ".csv");
                                    File.WriteAllText(strFile, strFileContent, Encoding.Default);
                                    //File.WriteAllText(strRenBaoFileByErrorReset, "errorReset", Encoding.Default);
                                    File.WriteAllText(strStartAndReset, "", Encoding.Default);
                                    File.WriteAllText(strRenBaoFileByError, "", Encoding.Default);
                                    File.WriteAllText(strRenBaoFileByStop, "", Encoding.Default);

                                    bool bResult = UploadDataFileForRenBaoMES(strFactory, strLineName, strModule, strCustor, strFool, strMachine, strEquipName, strOP,strSide, "Status_SPI_" + dtTMP.ToString("yyyyMMddHHmmss") + ".csv", out strErrormessage, arrDataValue5, arrStutsKey5);

                                    if (bResult)
                                    {
                                        //_log.WriteLog("设备状态C03本地文件成功--", "线程RenBao");
                                        AppLogHelp.WriteLog(LogFileFormate.MES, "设备状态C03本地文件成功-");
                                    }
                                    else
                                    {
                                       // _log.WriteErr("设备状态C03上抛失败错误信息:" + strErrormessage, "线程RenBao");
                                        AppLogHelp.WriteError(LogFileFormate.MES, "设备状态C03上抛失败错误信息:"+ strErrormessage);
                                    }

                                    //_log.WriteLog("解除异常状态log输出", "RenBao");
                                }
                            }
                            else if (strStop == "True")
                            {
                                if (string.IsNullOrEmpty(File.ReadAllText(strRenBaoFileByStop)))
                                {
                                    //stop
                                    strFileContent = strFileContent +
                                    strTime + _strSpit +
                                    strLineName + _strSpit +
                                    strMachine + _strSpit +
                                    "C04" + _strSpit +
                                    "ProductionEnd" + _strSpit +
                                    "null" + _strSpit +
                                    "null" ;
                                    arrDataValue5[0] = strTime;
                                    arrDataValue5[1] = "C04";
                                    arrDataValue5[2] = "ProductionEnd";
                                    arrDataValue5[3] = "";
                                    arrDataValue5[4] = "";
                                    strFile = Path.Combine(_strCsvPath, "Status_SPI_" + dtTMP.ToString("yyyyMMddHHmmss") + ".csv");
                                    File.WriteAllText(strFile, strFileContent, Encoding.Default);
                                    File.WriteAllText(strRenBaoFileByStart, "", Encoding.Default);
                                    File.WriteAllText(strRenBaoFileByError, "", Encoding.Default);
                                    File.WriteAllText(strRenBaoFileByStop, "stop", Encoding.Default);
                                    bool bResult = UploadDataFileForRenBaoMES(strFactory, strLineName, strModule, strCustor, strFool, strMachine, strEquipName, strOP, strSide,"Status_SPI_" + dtTMP.ToString("yyyyMMddHHmmss") + ".csv", out strErrormessage, arrDataValue5, arrStutsKey5);

                                    if (bResult)
                                    {
                                        //_log.WriteLog("设备状态C04本地文件成功--", "线程RenBao");
                                        AppLogHelp.WriteLog(LogFileFormate.MES, "设备状态C04本地文件成功--");
                                    }
                                    else
                                    {
                                        //_log.WriteErr("设备状态C04上抛失败错误信息:" + strErrormessage, "线程RenBao");
                                        AppLogHelp.WriteError(LogFileFormate.MES, "设备状态C04上抛失败错误信息:" + strErrormessage);
                                    }
                                    //_log.WriteLog("停止状态log输出成功", "RenBao");
                                }

                            }
                            ClearMemory();
                            strFileContent = string.Empty;
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(5000);
                    }
                    strSql = string.Empty; strSqlForPCBInfo = string.Empty;
                }
            }
            catch (Exception ex)
            {
                //_log.WriteErr("错误:" + e.Message, "ThreadName:RenBao");
                AppLogHelp.WriteError(LogFileFormate.MES, "ThreadName:RenBao" + ex.Message);
            }


        }

        private void SaveDataForRenBaoNewFunction()
        {
            try
            {

                DateTime dtNow = DateTime.Now;

                string strStart = string.Empty, strError = string.Empty,
                                strStop = string.Empty, strRun = string.Empty;
                string strTime = string.Empty;
                string strTime2 = string.Empty;
                string strFileNameTime = string.Empty;
                string strLineName = string.Empty;
                string strMachine = string.Empty;
                string strErrorContent = string.Empty;

                string strFactory = string.Empty;
                string strCustor = string.Empty;
                string strFool = string.Empty;
                string strEquipName = string.Empty;
                string strSide = string.Empty;
                //string strEquipID = dataTable.Rows[i][17].ToString();
                string strModule = string.Empty;
                string strErrorContentEnglish = string.Empty;
                //GetErrorCodeForStrEquipStatus(strErrorContent, ref strErrorContentEnglish);
                string strFile = string.Empty;//Path.Combine(_strCsvPath, "Status_SPI_" + strFileNameTime);
                string strOP = string.Empty;
                string strFileContent = string.Empty;
                Random rand = new Random();
                string strAutoAPPIniFile = @"D:\EYSPI\Bin\Config\autoApp.ini";
                string arraybarcode1 = "", arraybarcode2 = "", strarrayResult1 = "", strarrayResult2 = "";
                //test
                //dtNow = Convert.ToDateTime("2019-01-03 17:19:25");
                //DateTime dtTMP = dtNow;
                string strSql = "", strSqlForPCBInfo = "";
                string[] arrStutsKey5 = new string[5];
                string[] arrDataValue5 = new string[5];
                string[] arrStrKey = new string[8];
                string[] arrStrVaule = new string[8];
                string[] arrStrKey6 = new string[6];
                string[] arrStrVaule6 = new string[6];
                arrStrKey[0] = "TIME";
                arrStrKey[1] = "Program";
                arrStrKey[2] = "InWaitingTime";
                arrStrKey[3] = "OutWaitingTime";
                arrStrKey[4] = "SN1";
                arrStrKey[5] = "Result1";
                arrStrKey[6] = "SN2";
                arrStrKey[7] = "Result2";
                arrStutsKey5[0] = "TIME";
                arrStutsKey5[1] = "EventCode";
                arrStutsKey5[2] = "EventMessage";
                arrStutsKey5[3] = "ErrorCode";
                arrStutsKey5[4] = "ErrorMessage";
                arrStrKey6[0] = "TIME";
                arrStrKey6[1] = "Program";
                arrStrKey6[2] = "InWaitingTime";
                arrStrKey6[3] = "OutWaitingTime";
                arrStrKey6[4] = "SN";
                arrStrKey6[5] = "Result";
                string KunShanRenBao = "KunShanRenBao", tbRenBaoSaoplant = "tbRenBaoSaoplant", tbRenBaoGroupName = "tbRenBaoGroupName",
                                    tbRenBaoLineName = "tbRenBaoLineName", tbRenBaoCuscode = "tbRenBaoCuscode", tbRenBaoPross = "tbRenBaoPross",
                                    tbRenBaoFactorySN = "tbRenBaoFactorySN", tbRenBaoSide = "tbRenBaoSide", tbRenBaoEquipType = "tbRenBaoEquipType",
                                    tbRenBaoOp = "tbRenBaoOp";

                System.Data.DataTable dataTable = null; System.Data.DataTable dataTablePCB = null; DataTable dtArrayResult = null, dtStatusPCB = null, dtArrayPCB = null;
                string strJobName = "", strBarcode = "", strResult = "";
                double iWaitBoardInTime = 0, iWaitBoardOutTime = 0;   //
                DateTime ttDateTime, dtEndTime, dtStartTime;
                //DateTime dtStatusStartTime, dtStatusEndTime;
                string strArraySql = string.Empty, strResultArraySql = string.Empty, strStatusSql = string.Empty;
                ttDateTime = DateTime.Now;
                DateTime dt1, dt2;
                int iTmp = 0;
                string strSqlForPCBInfoSec = "SELECT " +
                   "pcb.PCBBarcode, " +
                   "job.JobName, " +
                   " pcb.Result, " +
                   " pcb.Operator, " +
                   " pcb.PCBID, " +
                   " pcb.StartTime,  " +
                   " pcb.EndTime " +
                   " FROM " +
                   "    spidb.tbboard AS pcb, " +
                   "    spidb.TBJobInfo AS job " +
                   "WHERE " +
                    //"    pcb.StartTime >= '{0}' " +
                   " pcb.StartTime > '{0}' " +
                   "AND job.SerNo = pcb.JobIndex ;";
                string strArraySqlSec = " SELECT " +
                            "   bc.ArrayBarCode, " +
                            " bc.ArrayID  " +
                           " FROM " +
                           "    spidb.TBBarCode as bc " +
                           " WHERE " +
                           "  bc.PCBID = '{0}' AND " +
                           "  bc.BarCode = '{1}'";
                string strResultArraySqlSec = "SELECT COUNT(PCBID) FROM spidb.TBPadMeasure as pad WHERE pad.PCBID = '{0}' AND pad.ArrayIDIndex ='{1}' AND pad.JudgeRes = '1'";//string.Format(strResultArraySql, strarrayID);
                while (true)
                {
                    if (File.Exists(strAutoAPPIniFile))
                    {
                        strFactory = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoSaoplant, "", strAutoAPPIniFile);
                        strModule = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoGroupName, "", strAutoAPPIniFile);
                        strLineName = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoLineName, "", strAutoAPPIniFile);

                        strCustor = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoCuscode, "", strAutoAPPIniFile);
                        strFool = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoPross, "", strAutoAPPIniFile);
                        strMachine = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoFactorySN, "", strAutoAPPIniFile);

                        strSide = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoSide, "", strAutoAPPIniFile);
                        strEquipName = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoEquipType, "", strAutoAPPIniFile);
                        strOP = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoOp, "", strAutoAPPIniFile);
                    }
                    strSqlForPCBInfo = string.Format(strSqlForPCBInfoSec, ttDateTime);

                    dataTablePCB = getDataTableForZZFox(_strConnectString, strSqlForPCBInfo);
                    if (dataTablePCB != null && dataTablePCB.Rows.Count > 0)
                    {
                        //wait 10s 人员判定
                        System.Threading.Thread.Sleep(4000);
                        dataTablePCB = getDataTableForZZFox(_strConnectString, strSqlForPCBInfo);
                        for (int iBarIndex = 0; iBarIndex < dataTablePCB.Rows.Count; iBarIndex++)
                        {
                            strBarcode = dataTablePCB.Rows[iBarIndex][0].ToString();
                            if (string.IsNullOrEmpty(strBarcode))
                            {
                                strBarcode = "scan off";
                            }
                            strJobName = dataTablePCB.Rows[iBarIndex][1].ToString();
                            strResult = dataTablePCB.Rows[iBarIndex][2].ToString();
                            //string strOperator = dataTablePCB.Rows[iBarIndex][3].ToString();
                            //strOP = strOperator;
                            string strPCBID = dataTablePCB.Rows[iBarIndex][4].ToString();   //pcbid
                            string strPCBstartTime = dataTablePCB.Rows[iBarIndex][5].ToString();   //startTime
                            string strPCBFilestartTime = ((DateTime)dataTablePCB.Rows[iBarIndex][5]).ToString("yyyyMMddHHmmss");
                            dtEndTime = (DateTime)dataTablePCB.Rows[iBarIndex][6];
                            dtStartTime = (DateTime)dataTablePCB.Rows[iBarIndex][5];

                            ttDateTime = dtStartTime;
                            if (strResult == "0" || strResult == "2")
                            {
                                strResult = "PASS";
                            }
                            else
                            {
                                strResult = "FAIL";
                            }

                            //get the array
                            strArraySql = string.Format(strArraySqlSec, strPCBID, strBarcode);

                            strResultArraySql = "";

                            //前待板时间 iWaitBoardInTime    后待板时间 iWaitBoardInTime
                            #region "前待板时间 iWaitBoardInTime    后待板时间 iWaitBoardInTime"
                            iWaitBoardInTime = 0;
                            iWaitBoardOutTime = 0;
                            //strStatusSql = " SELECT " +
                            //" sta.UpdateTime " +
                            //" FROM " +
                            //" spidb.tbequipstatus AS sta " +
                            //" WHERE " +
                            //" sta.UpdateTime >= '" + dtStartTime.AddSeconds(-4.0) + "' " +
                            //" AND sta.UpdateTime <= '" + dtEndTime.AddSeconds(4.0) + "' " +
                            //" AND sta.InspectBoard = '1' ORDER BY sta.UpdateTime ";
                            //dtStatusPCB = getDataTableForZZFox(_strConnectString, strStatusSql, _log);
                            //if (dtStatusPCB != null && dtStatusPCB.Rows.Count > 0)
                            //{
                            //    dtStatusStartTime = (DateTime)dtStatusPCB.Rows[0][0];
                            //    _log.WriteLog(dtStatusStartTime + ":test", "测试板的开始时间--");
                            //    dtStatusEndTime = (DateTime)dtStatusPCB.Rows[dtStatusPCB.Rows.Count - 1][0];
                            //    //inBoard
                            //    strStatusSql = " SELECT stu.InBoard,stu.Frequency,stu.ReqSig,stu.InspectBoard,stu.UpdateTime,stu.ReadyOut,stu.OutBoard FROM spidb.tbequipstatus stu WHERE stu.UpdateTime > '" + dtStatusStartTime.AddMinutes(-10) + "' AND stu.UpdateTime <'" + dtStatusStartTime + "' ORDER BY stu.UpdateTime DESC ";
                            //    dtStatusPCB = getDataTableForZZFox(_strConnectString, strStatusSql, _log);
                            //    if (dtStatusPCB != null && dtStatusPCB.Rows.Count > 0)
                            //    {

                            //        DateTime dTimeEndStatusPCB = (DateTime)dtStatusPCB.Rows[0][4], dTimeStartStatusPCB = (DateTime)dtStatusPCB.Rows[0][4];
                            //        for (int i = 0; i < dtStatusPCB.Rows.Count; i++)
                            //        {
                            //            if (dtStatusPCB.Rows[i][5].ToString() == "True" || dtStatusPCB.Rows[i][6].ToString() == "True")
                            //            {
                            //                dTimeStartStatusPCB = (DateTime)dtStatusPCB.Rows[i][4];
                            //                break;
                            //            }
                            //            if (dtStatusPCB.Rows[i][3].ToString() == "True")
                            //            {
                            //                dTimeStartStatusPCB = ((DateTime)dtStatusPCB.Rows[i][4]).AddSeconds(-1.5);
                            //                break;
                            //            }
                            //            //if (dtStatusPCB.Rows[i][2].ToString() == "True")
                            //            //{
                            //            //    iWaitBoardInTime += double.Parse(dtStatusPCB.Rows[i][1].ToString()) - rand.Next(1, 2) * 0.1;
                            //            //}
                            //        }
                            //        iWaitBoardInTime = (dTimeEndStatusPCB - dTimeStartStatusPCB).TotalSeconds;
                            //    }
                            //    //outBoard
                            //    strStatusSql = " SELECT stu.InspectBoard,stu.Frequency,stu.ReadyOut,stu.OutBoard,stu.InBoard,stu.ReqSig FROM spidb.tbequipstatus stu WHERE stu.UpdateTime > '" + dtStatusEndTime.AddSeconds(-7.0) + "' AND stu.UpdateTime <'" + dtStatusEndTime.AddMinutes(30.0) + "' ORDER BY stu.UpdateTime  ";
                            //    dt1 = DateTime.Now;
                            //    while (true)
                            //    {
                            //        dt2 = DateTime.Now;
                            //        dtStatusPCB = getDataTableForZZFox(_strConnectString, strStatusSql, _log);
                            //        if (dtStatusPCB != null && dtStatusPCB.Rows.Count > 0)
                            //        {
                            //            bool bTmpStatus = false; bool bIsRealoutAndOutBoard = false;
                            //            for (int i = 0; i < dtStatusPCB.Rows.Count; i++)
                            //            {
                            //                if (string.IsNullOrEmpty(dtStatusPCB.Rows[i][0].ToString()) == false)
                            //                {
                            //                    if (dtStatusPCB.Rows[i][4].ToString() == "True" || dtStatusPCB.Rows[i][5].ToString() == "True")
                            //                    {
                            //                        bTmpStatus = true;
                            //                        break;
                            //                    }
                            //                }
                            //            }
                            //            if (dtStatusPCB.Rows[dtStatusPCB.Rows.Count - 1][2].ToString() != "True" && dtStatusPCB.Rows[dtStatusPCB.Rows.Count - 1][3].ToString() != "True")
                            //            {
                            //                bIsRealoutAndOutBoard = true;
                            //            }
                            //            if (bTmpStatus || bIsRealoutAndOutBoard)
                            //            {
                            //                if (dtStatusPCB != null && dtStatusPCB.Rows.Count > 0)
                            //                {
                            //                    iTmp = 0;
                            //                    for (int i = 0; i < dtStatusPCB.Rows.Count; i++)
                            //                    {
                            //                        if (string.IsNullOrEmpty(dtStatusPCB.Rows[i][0].ToString()) == false)
                            //                        {
                            //                            if (dtStatusPCB.Rows[i][4].ToString() == "True" || dtStatusPCB.Rows[i][5].ToString() == "True")
                            //                            {
                            //                                break;
                            //                            }
                            //                        }
                            //                        if (dtStatusPCB.Rows[i][2].ToString() == "True" || dtStatusPCB.Rows[i][3].ToString() == "True")
                            //                        {
                            //                            iTmp++;
                            //                            iWaitBoardOutTime += double.Parse(dtStatusPCB.Rows[i][1].ToString()) - rand.Next(1, 2) * 0.1;
                            //                        }
                            //                        //else if (dtStatusPCB.Rows[i][3].ToString() == "True")
                            //                        //{
                            //                        //    iWaitBoardOutTime += double.Parse(dtStatusPCB.Rows[i][1].ToString()) ;
                            //                        //}
                            //                    }
                            //                    if (iTmp == 1)
                            //                    {
                            //                        iWaitBoardOutTime = iWaitBoardOutTime - rand.Next(11, 13) * 0.1;
                            //                    }
                            //                    //if ( iWaitBoardOutTime > 3)
                            //                    //{
                            //                    //    iWaitBoardOutTime = iWaitBoardOutTime - rand.Next(20, 25) * 0.1;
                            //                    //}
                            //                }

                            //                break;
                            //            }
                            //            else
                            //            {
                            //                System.Threading.Thread.Sleep(3000);
                            //                if ((dt2 - dt1).TotalSeconds >= 30)// secnond 30
                            //                {
                            //                    break;
                            //                }
                            //            }
                            //        }

                            //    }



                            //}
                            #endregion

                            if (iWaitBoardInTime <= 0)
                            {
                                iWaitBoardInTime = 0.1 * rand.Next(5, 9);
                                //strWaitBoardTime = iWaitBoardInTime+"";
                            }
                            if (iWaitBoardOutTime <= 0)
                            {
                                iWaitBoardOutTime = 0.1 * rand.Next(5, 10);
                            }
                            string strErrorMessage = "";
                            dtArrayPCB = getDataTableForZZFox(_strConnectString, strArraySql);
                            if (dtArrayPCB != null && dtArrayPCB.Rows.Count > 0)
                            {
                                if (dtArrayPCB.Rows.Count > 1)
                                {
                                    for (int n = 0; n < dtArrayPCB.Rows.Count; n++)
                                    {
                                        string arrbarcode = dtArrayPCB.Rows[n][0].ToString();
                                        if (string.IsNullOrEmpty(arrbarcode) || string.Equals("noread", arrbarcode, StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            arrbarcode = "scan off";
                                        }
                                        //if (string.IsNullOrEmpty(arrbarcode)) { arrbarcode = strBarcode; }
                                        string strarrayID = dtArrayPCB.Rows[n][1].ToString();
                                        string strarrayResult = "PASS";
                                        strResultArraySql = string.Format(strResultArraySqlSec, strPCBID, strarrayID);
                                        //  "SELECT COUNT(PCBID) FROM spidb.TBPadMeasure as pad WHERE pad.PCBID = '" + strPCBID + "' AND pad.ArrayIDIndex ='" + strarrayID + "' AND pad.JudgeRes = '1'";//string.Format(strResultArraySql, strarrayID);
                                        dtArrayResult = getDataTableForZZFox(_strConnectString, strResultArraySql);
                                        if (dtArrayResult != null && dtArrayResult.Rows.Count > 0)
                                        {
                                            if (dtArrayResult.Rows[0][0].ToString() != "0")
                                            {
                                                strarrayResult = "FAIL";
                                            }
                                            else
                                            {
                                                strarrayResult = "PASS";
                                            }
                                        }
                                        if (n == 0)
                                        {
                                            arraybarcode1 = arrbarcode;
                                            if (arraybarcode1 == "scan off")
                                            {
                                                arraybarcode1 = strBarcode;
                                            }
                                            strarrayResult1 = strarrayResult;
                                        }
                                        if (n == 1)
                                        {
                                            arraybarcode2 = arrbarcode + "";
                                            strarrayResult2 = strarrayResult;
                                        }
                                    }
                                    //输出csv文件;
                                    StringBuilder sb = new StringBuilder();
                                    sb.Append("TIME" + _strSpit +
                                        "LINE" + _strSpit +
                                        "MACHINEID" + _strSpit +
                                        "Program" + _strSpit +
                                        "InWaitingTime" + _strSpit +
                                        "OutWaitingTime" + _strSpit +
                                        "SN1" + _strSpit +
                                        "Result1 " + _strSpit +
                                        "SN2" + _strSpit +
                                        "Result2" +
                                        _strEndLine);
                                    sb.Append(strPCBstartTime + _strSpit
                                        + strLineName + _strSpit
                                        + strMachine + _strSpit
                                        + strJobName + _strSpit
                                        + iWaitBoardInTime + _strSpit
                                        + iWaitBoardOutTime + _strSpit
                                        + arraybarcode1 + _strSpit
                                        + strarrayResult1 + _strSpit
                                        + arraybarcode2 + _strSpit
                                        + strarrayResult2 + _strEndLine);
                                    arrStrVaule[0] = strPCBstartTime;
                                    arrStrVaule[1] = strJobName;
                                    arrStrVaule[2] = iWaitBoardInTime + "";
                                    arrStrVaule[3] = iWaitBoardOutTime + "";
                                    arrStrVaule[4] = arraybarcode1;
                                    arrStrVaule[5] = strarrayResult1;
                                    arrStrVaule[6] = arraybarcode2;
                                    arrStrVaule[7] = strarrayResult2;
                                    string strFile2 = Path.Combine(this._strCsvPath2, "Data_SPI" + "_" + strPCBFilestartTime + ".csv");

                                    File.WriteAllText(strFile2, sb.ToString(), Encoding.Default);
                                    sb.Clear();
                                    ////if (bResult)
                                    ////{
                                    //_log.WriteLog("导出array本地测试文件并上抛成功--", "线程RenBao");
                                    ////}
                                    ////else
                                    ////{
                                    ////    _log.WriteErr("上抛array失败错误信息:" + strErrorMessage, "线程RenBao");
                                    ////}
                                    //_log.WriteLog("进来--", "renbao");
                                    bool bResult = UploadDataFileForRenBaoMES(strFactory, strLineName, strModule, strCustor, strFool, strMachine, strEquipName, strOP, strSide, "Data_SPI" + "_" + strPCBFilestartTime + ".csv", out strErrorMessage, arrStrVaule, arrStrKey);
                                    //sb.Clear();
                                    if (bResult)
                                    {
                                        //log.WriteLog("array success:", "RenBao");
                                        AppLogHelp.WriteLog(LogFileFormate.MES, "RenBao array success:");
                                    }
                                    else
                                    {
                                        //_log.WriteErr("array fail:" + strErrorMessage, "RenBao");
                                        AppLogHelp.WriteLog(LogFileFormate.MES, "RenBao array fail:" + strErrorMessage);
                                    }
                                }
                                else
                                {
                                    // 输出csv文件;
                                    StringBuilder sb = new StringBuilder();
                                    sb.Append("TIME" + _strSpit +
                                        "LINE" + _strSpit +
                                        "MACHINEID" + _strSpit +
                                        "Program" + _strSpit +
                                        "InWaitingTime" + _strSpit +
                                        "OutWaitingTime" + _strSpit +
                                        "SN" + _strSpit +
                                        "Result" +
                                        //"SN2 " + _strSpit +
                                        //"Result2" +
                                        _strEndLine);
                                    sb.Append(strPCBstartTime + _strSpit
                                        + strLineName + _strSpit
                                        + strMachine + _strSpit
                                        + strJobName + _strSpit
                                        + iWaitBoardInTime + _strSpit
                                        + iWaitBoardOutTime + _strSpit
                                        + strBarcode + _strSpit
                                        + strResult + _strEndLine);


                                    arrStrVaule6[0] = strPCBstartTime;
                                    arrStrVaule6[1] = strJobName;
                                    arrStrVaule6[2] = iWaitBoardInTime + "";
                                    arrStrVaule6[3] = iWaitBoardOutTime + "";
                                    arrStrVaule6[4] = strBarcode;
                                    arrStrVaule6[5] = strResult;

                                    string strFile2 = Path.Combine(this._strCsvPath2, "Data_SPI" + "_" + strPCBFilestartTime + ".csv");

                                    File.WriteAllText(strFile2, sb.ToString(), Encoding.Default);

                                    bool bResult = UploadDataFileForRenBaoMES(strFactory, strLineName, strModule, strCustor, strFool, strMachine, strEquipName, strOP, strSide, "Data_SPI" + "_" + strPCBFilestartTime + ".csv", out strErrorMessage, arrStrVaule6, arrStrKey6);
                                    //Ini  初始化文件;
                                    if (bResult)
                                    {
                                        //_log.WriteLog("noarray success", "RenBao");
                                        AppLogHelp.WriteLog(LogFileFormate.MES, "RenBao!noarray success" );

                                    }
                                    else
                                    {
                                        //_log.WriteErr("noarray fail:" + strErrorMessage, "RenBao");
                                        AppLogHelp.WriteError(LogFileFormate.MES, "RenBao!" + strErrorMessage);
                                    }
                                    sb.Clear();
                                }
                            }
                            strResultArraySql = string.Empty; strArraySql = string.Empty;
                            strPCBID = string.Empty;   //pcbid
                            strPCBstartTime = string.Empty;   //startTime
                            strPCBFilestartTime = string.Empty;
                        }
                        System.Threading.Thread.Sleep(100);
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(2000);
                        // _log.WriteErr(ttDateTime + ":wait Data...", "RenBao");
                        //Ini  初始化文件;
                    }
                    strJobName = string.Empty; strBarcode = string.Empty; strResult = string.Empty;
                    iWaitBoardInTime = 0; iWaitBoardOutTime = 0;
                    ClearMemory();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void IniFileRenBaoReset(string strA,string strB,string strC,string strD,string strE)
        {
            File.WriteAllText(strA,"",Encoding.Default);
            File.WriteAllText(strB, "", Encoding.Default);
            File.WriteAllText(strC, "", Encoding.Default);
            File.WriteAllText(strD, "", Encoding.Default);
            File.WriteAllText(strE, "", Encoding.Default);
        }
        private void IniFileRenBaoReset(string strA, string strB, string strC, string strD, string strE, string strF, string strH, string strI, string strGTureOrFalse)
        {
            File.WriteAllText(strA, "", Encoding.Default);
            File.WriteAllText(strB, "", Encoding.Default);
            File.WriteAllText(strC, "", Encoding.Default);
            File.WriteAllText(strD, "", Encoding.Default);
            File.WriteAllText(strE, "", Encoding.Default);
            File.WriteAllText(strF, "", Encoding.Default);
            File.WriteAllText(strH, "", Encoding.Default);
            File.WriteAllText(strI, "", Encoding.Default);
            File.WriteAllText(strGTureOrFalse, "false", Encoding.Default);
        }
        private void StartThreadForRenBao()
        {
            try
            {
                System.Threading.Thread threadRenBao = new System.Threading.Thread(new System.Threading.ThreadStart(SaveDataForRenBaoNewFunction));
                threadRenBao.Priority = System.Threading.ThreadPriority.Lowest;
                threadRenBao.Start();
            }
            catch (Exception e)
            {
                //_log.WriteErr("错误:" + e.Message, "线程RenBao");
            }
        }

        /// <summary>
        /// RenBaoFiles
        /// </summary>
        private void StartThreadForRenBao_ToFile()
        {
            try
            {
                System.Threading.Thread threadRenBao = new System.Threading.Thread(new System.Threading.ThreadStart(SaveDataForRenBao_ToFile));
                threadRenBao.Priority = System.Threading.ThreadPriority.Lowest;
                threadRenBao.Start();
            }
            catch (Exception e)
            {
                //_log.WriteErr("错误:" + e.Message, "线程RenBaoFile");
                AppLogHelp.WriteError(LogFileFormate.MES, "线程RenBaoFile:" + e.Message);

            }
        }
        //private void SaveDataForRenBao_ToFile()
        //{
        //    try
        //    {
        //        string[] arrStutsKey5 = new string[5];
        //        string[] arrDataValue5 = new string[5];
        //        string[] arrStrKey = new string[8];
        //        string[] arrStrVaule = new string[8];
        //        string[] arrStrKey6 = new string[6];
        //        string[] arrStrVaule6 = new string[6];
        //        arrStrKey[0] = "TIME";
        //        arrStrKey[1] = "Program";
        //        arrStrKey[2] = "InWaitingTime";
        //        arrStrKey[3] = "OutWaitingTime";
        //        arrStrKey[4] = "SN1";
        //        arrStrKey[5] = "Result1";
        //        arrStrKey[6] = "SN2";
        //        arrStrKey[7] = "Result2";
        //        arrStutsKey5[0] = "TIME";
        //        arrStutsKey5[1] = "EventCode";
        //        arrStutsKey5[2] = "EventMessage";
        //        arrStutsKey5[3] = "ErrorCode";
        //        arrStutsKey5[4] = "ErrorMessage";
        //        arrStrKey6[0] = "TIME";
        //        arrStrKey6[1] = "Program";
        //        arrStrKey6[2] = "InWaitingTime";
        //        arrStrKey6[3] = "OutWaitingTime";
        //        arrStrKey6[4] = "SN";
        //        arrStrKey6[5] = "Result";
        //        string KunShanRenBao = "KunShanRenBao", tbRenBaoSaoplant = "tbRenBaoSaoplant", tbRenBaoGroupName = "tbRenBaoGroupName",
        //                            tbRenBaoLineName = "tbRenBaoLineName", tbRenBaoCuscode = "tbRenBaoCuscode", tbRenBaoPross = "tbRenBaoPross",
        //                            tbRenBaoFactorySN = "tbRenBaoFactorySN", tbRenBaoSide = "tbRenBaoSide", tbRenBaoEquipType = "tbRenBaoEquipType",
        //                            tbRenBaoOp = "tbRenBaoOp";
        //        StreamReader sr = null;
        //        string strStatusTitle = "TIME,LINE";
        //        string strLine = null;
        //        string[] arrStatus = null;
        //        string strErrormessage = string.Empty;

        //        string strLineName = string.Empty;
        //        string strMachine = string.Empty;
        //        string strErrorContent = string.Empty;

        //        string strFactory = string.Empty;
        //        string strCustor = string.Empty;
        //        string strFool = string.Empty;
        //        string strEquipName = string.Empty;
        //        string strSide = string.Empty;
        //        //string strEquipID = dataTable.Rows[i][17].ToString();
        //        string strModule = string.Empty;
        //        string strErrorContentEnglish = string.Empty;
        //        //GetErrorCodeForStrEquipStatus(strErrorContent, ref strErrorContentEnglish);
        //        string strFile = string.Empty;//Path.Combine(_strCsvPath, "Status_SPI_" + strFileNameTime);
        //        string strOP = string.Empty;
        //        string strFileContent = string.Empty;
        //        string strAutoAPPIniFile = @"D:\EYSPI\Bin\Config\autoApp.ini";
        //        bool bClientResult = false;
        //        string strTimeSettingFilePath = @"D:\EYSPI\Bin\SPILogs\Peng\TimeSetting";
        //        if (Directory.Exists(strTimeSettingFilePath) == false)
        //        {
        //            Directory.CreateDirectory(strTimeSettingFilePath);
        //        }
        //        strTimeSettingFilePath = Path.Combine(strTimeSettingFilePath,"Time.txt");
        //        int iSecond = 60;
        //        if (File.Exists(strTimeSettingFilePath) == false)
        //        {
        //            File.WriteAllText(strTimeSettingFilePath, "60", Encoding.Default);
        //        }
        //        else
        //        {
        //            if (string.IsNullOrEmpty(File.ReadAllText(strTimeSettingFilePath, Encoding.Default)))
        //            {

        //            }
        //            else
        //            {
        //                iSecond = int.Parse(File.ReadAllText(strTimeSettingFilePath, Encoding.Default));
        //            }
        //        }
        //        while (true)
        //        {

        //            DateTime dtTime = DateTime.Now.AddSeconds(-iSecond);
        //            if (Directory.Exists(_strCsvPath) && Directory.Exists(_strCsvPath2))
        //            {
        //                if (File.Exists(strAutoAPPIniFile))
        //                {

        //                    strFactory = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoSaoplant, "", strAutoAPPIniFile);
        //                    strModule = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoGroupName, "", strAutoAPPIniFile);
        //                    strLineName = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoLineName, "", strAutoAPPIniFile);

        //                    strCustor = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoCuscode, "", strAutoAPPIniFile);
        //                    strFool = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoPross, "", strAutoAPPIniFile);
        //                    strMachine = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoFactorySN, "", strAutoAPPIniFile);

        //                    strSide = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoSide, "", strAutoAPPIniFile);
        //                    strEquipName = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoEquipType, "", strAutoAPPIniFile);
        //                    strOP = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoOp, "", strAutoAPPIniFile);
        //                }
        //                IEnumerable<string> list = Directory.GetFiles(_strCsvPath).Where(p => File.GetCreationTime(p) < dtTime);
        //                IEnumerable<string> list2 = Directory.GetFiles(_strCsvPath2).Where(p => File.GetCreationTime(p) < dtTime);
                        
        //                if (list != null && list.Count() > 0)
        //                {
                            
        //                    foreach (string strFilePath in list)
        //                    {
        //                        if (File.Exists(strFilePath))
        //                        {
        //                            sr = new StreamReader(strFilePath, Encoding.Default);
        //                            while ((strLine = sr.ReadLine()) != null)
        //                            {
        //                                if (strLine.Contains(strStatusTitle) || strLine == strStatusTitle)
        //                                {
        //                                    continue;
        //                                }
        //                                //arrDataValue5 = strLine.Split(',');
        //                                if (strLine.Split(',') != null && strLine.Split(',').Length == 7)
        //                                {
        //                                    arrDataValue5[0] = strLine.Split(',')[0];
        //                                    arrDataValue5[1] = strLine.Split(',')[3];
        //                                    arrDataValue5[2] = strLine.Split(',')[4];
        //                                    arrDataValue5[3] = strLine.Split(',')[5];
        //                                    arrDataValue5[4] = strLine.Split(',')[6];
        //                                    bClientResult = UploadDataFileForRenBaoMES(strFactory, strLineName, strModule, strCustor, strFool, strMachine, strEquipName, strOP, strSide, Path.GetFileName(strFilePath) , out strErrormessage, arrDataValue5, arrStutsKey5);
                                            
                                            
        //                                }
        //                            }
        //                            sr.Close();
        //                            if (bClientResult)
        //                            {
        //                                _log.WriteLog("RenBao上抛设备状态成功", "RenBao_File");

        //                                File.Delete(strFilePath);

        //                            }
        //                            else
        //                            {
        //                                _log.WriteLog("RenBao上抛设备状态失败:" + strErrormessage + "  [文件名]" + strFilePath, "RenBao_File");

        //                            }
        //                        }

        //                    }
        //                }
                        
        //                if (list2 != null && list2.Count() > 0)
        //                {
                            
        //                    foreach (string strFilePath2 in list2)
        //                    {
        //                        if (File.Exists(strFilePath2))
        //                        {
        //                            sr = new StreamReader(strFilePath2, Encoding.Default);
        //                            while ((strLine = sr.ReadLine()) != null)
        //                            {
        //                                if (strLine.Contains(strStatusTitle) || strLine == strStatusTitle)
        //                                {
        //                                    continue;
        //                                }
        //                                if (strLine.Split(',') != null && strLine.Split(',').Length ==8)
        //                                {
        //                                    arrStrVaule6[0] = strLine.Split(',')[0];
        //                                    arrStrVaule6[1] = strLine.Split(',')[3];
        //                                    arrStrVaule6[2] = strLine.Split(',')[4];
        //                                    arrStrVaule6[3] = strLine.Split(',')[5];
        //                                    arrStrVaule6[4] = strLine.Split(',')[6];
        //                                    arrStrVaule6[5] = strLine.Split(',')[7];
        //                                    bClientResult = UploadDataFileForRenBaoMES(strFactory, strLineName, strModule, strCustor, strFool, strMachine, strEquipName, strOP, strSide, Path.GetFileName(strFilePath2), out strErrormessage, arrStrVaule6, arrStrKey6);
                                            
        //                                    //_log.WriteLog("RenBao上抛单PCS成功","RenBao_File");
        //                                }
        //                                else if (strLine.Split(',') != null && strLine.Split(',').Length == 10)
        //                                {
        //                                    arrStrVaule[0] = strLine.Split(',')[0];
        //                                    arrStrVaule[1] = strLine.Split(',')[3];
        //                                    arrStrVaule[2] = strLine.Split(',')[4];
        //                                    arrStrVaule[3] = strLine.Split(',')[5];
        //                                    arrStrVaule[4] = strLine.Split(',')[6];
        //                                    arrStrVaule[5] = strLine.Split(',')[7];
        //                                    arrStrVaule[6] = strLine.Split(',')[8];
        //                                    arrStrVaule[7] = strLine.Split(',')[9];
        //                                    bClientResult = UploadDataFileForRenBaoMES(strFactory, strLineName, strModule, strCustor, strFool, strMachine, strEquipName, strOP, strSide, Path.GetFileName(strFilePath2), out strErrormessage, arrStrVaule, arrStrKey);
        //                                    //_log.WriteLog("RenBao上抛多PCS成功", "RenBao_File");
        //                                }
        //                            }
        //                            sr.Close();
        //                            if (bClientResult)
        //                            {
        //                                _log.WriteLog("RenBao上抛PCS成功", "RenBao_File");

        //                                File.Delete(strFilePath2);

        //                            }
        //                            else
        //                            {
        //                                _log.WriteLog("RenBao上抛PCS失败:" + strErrormessage + "  [文件名]" + strFilePath2, "RenBao_File");
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    System.Threading.Thread.Sleep(5000);
        //                }

        //                list = null;
        //                list2 = null;
        //                ClearMemory();
        //            }
        //            else
        //            {
        //                System.Threading.Thread.Sleep(5000);
        //                break;
        //            }

        //        }

        //    }
        //    catch (Exception exx)
        //    {
        //        _log.WriteErr(exx.Message, "线程RenBaoFile");
        //    }
        //}

        private void SaveDataForRenBao_ToFile()
        {
            try
            {
                //TmpDir
                _strLogPengPath = Path.Combine(_strLogPengPath, "RenBao");
                string strLogDataPath = Path.Combine(_strLogPengPath, "RenBaoToData");
                if (Directory.Exists(_strLogPengPath) == false)
                {
                    Directory.CreateDirectory(_strLogPengPath);
                }
                if (Directory.Exists(_strCsvPath) == false)
                {
                    Directory.CreateDirectory(_strCsvPath);
                }
                if (Directory.Exists(_strCsvPath2) == false)
                {
                    Directory.CreateDirectory(_strCsvPath2);
                }
                if (Directory.Exists(strLogDataPath) == false)
                {
                    Directory.CreateDirectory(strLogDataPath);
                }
                string strRenBaoFileByStart = Path.Combine(_strLogPengPath, "renbao_start.txt");
                string strRenBaoFileByStop = Path.Combine(_strLogPengPath, "renbao_stop.txt");
                string strRenBaoFileByError = Path.Combine(_strLogPengPath, "renbao_error.txt");
                string strRenBaoFileByErrorReset = Path.Combine(_strLogPengPath, "renbao_errorReset.txt");

                string strRenBaoFileByReSig = Path.Combine(strLogDataPath, "renbao_ReSig.txt");
                string strRenBaoFileByInBoard = Path.Combine(strLogDataPath, "renbao_InBoard.txt");
                string strRenBaoFileByInSpect = Path.Combine(strLogDataPath, "renbao_InSpect.txt");
                string strRenBaoFileByReadyOut = Path.Combine(strLogDataPath, "renbao_ReadyOut.txt");
                string strRenBaoFileByBVSig = Path.Combine(strLogDataPath, "renbao_BVSig.txt");
                string strRenBaoFileByOutBoard = Path.Combine(strLogDataPath, "renbao_OutBoard.txt");
                string strRenBaoFileByInspectChecked = Path.Combine(strLogDataPath, "renbao_InspectChecked.txt"); //true false  ini默认false
                string strRenBaoFileByLastOutBoard = Path.Combine(strLogDataPath, "renbao_LastOutBoard.txt");
                string strRenBaoFileByInspectTime = Path.Combine(strLogDataPath, "renbao_InspectTime.txt");

                string strStartAndReset = Path.Combine(_strLogPengPath, "renbao_startAndReset.txt");
                System.Data.DataTable dataTable = null; //System.Data.DataTable dataTablePCB = null; DataTable dtArrayResult = null;
                //初始化
                IniFileRenBaoReset(strRenBaoFileByStart, strRenBaoFileByStop, strRenBaoFileByError, strStartAndReset, strRenBaoFileByErrorReset);
                IniFileRenBaoReset(strRenBaoFileByInBoard, strRenBaoFileByInSpect, strRenBaoFileByReadyOut,
                    strRenBaoFileByBVSig, strRenBaoFileByOutBoard, strRenBaoFileByLastOutBoard, strRenBaoFileByInspectTime, strRenBaoFileByReSig, strRenBaoFileByInspectChecked);
                DateTime dtNow = DateTime.Now;

                string strStart = string.Empty, strError = string.Empty,
                                strStop = string.Empty, strRun = string.Empty;
                string strTime = string.Empty;
                string strTime2 = string.Empty;
                string strFileNameTime = string.Empty;
                string strLineName = string.Empty;
                string strMachine = string.Empty;
                string strErrorContent = string.Empty;

                string strFactory = string.Empty;
                string strCustor = string.Empty;
                string strFool = string.Empty;
                string strEquipName = string.Empty;
                string strSide = string.Empty;
                //string strEquipID = dataTable.Rows[i][17].ToString();
                string strModule = string.Empty;
                string strErrorContentEnglish = string.Empty;
                //GetErrorCodeForStrEquipStatus(strErrorContent, ref strErrorContentEnglish);
                string strFile = string.Empty;//Path.Combine(_strCsvPath, "Status_SPI_" + strFileNameTime);
                string strOP = string.Empty;
                string strFileContent = string.Empty;
                Random rand = new Random();
                string strAutoAPPIniFile = @"D:\EYSPI\Bin\Config\autoApp.ini";
                //string arraybarcode1 = "", arraybarcode2 = "", strarrayResult1 = "", strarrayResult2 = "";
                //test
                //dtNow = Convert.ToDateTime("2019-01-03 17:19:25");
                DateTime dtTMP = dtNow;
                string strSql = "", strSqlForPCBInfo = "";
                string[] arrStutsKey5 = new string[5];
                string[] arrDataValue5 = new string[5];
                string[] arrStrKey = new string[8];
                string[] arrStrVaule = new string[8];
                string[] arrStrKey6 = new string[6];
                string[] arrStrVaule6 = new string[6];
                arrStrKey[0] = "TIME";
                arrStrKey[1] = "Program";
                arrStrKey[2] = "InWaitingTime";
                arrStrKey[3] = "OutWaitingTime";
                arrStrKey[4] = "SN1";
                arrStrKey[5] = "Result1";
                arrStrKey[6] = "SN2";
                arrStrKey[7] = "Result2";
                arrStutsKey5[0] = "TIME";
                arrStutsKey5[1] = "EventCode";
                arrStutsKey5[2] = "EventMessage";
                arrStutsKey5[3] = "ErrorCode";
                arrStutsKey5[4] = "ErrorMessage";
                arrStrKey6[0] = "TIME";
                arrStrKey6[1] = "Program";
                arrStrKey6[2] = "InWaitingTime";
                arrStrKey6[3] = "OutWaitingTime";
                arrStrKey6[4] = "SN";
                arrStrKey6[5] = "Result";
                string KunShanRenBao = "KunShanRenBao", tbRenBaoSaoplant = "tbRenBaoSaoplant", tbRenBaoGroupName = "tbRenBaoGroupName",
                                    tbRenBaoLineName = "tbRenBaoLineName", tbRenBaoCuscode = "tbRenBaoCuscode", tbRenBaoPross = "tbRenBaoPross",
                                    tbRenBaoFactorySN = "tbRenBaoFactorySN", tbRenBaoSide = "tbRenBaoSide", tbRenBaoEquipType = "tbRenBaoEquipType",
                                    tbRenBaoOp = "tbRenBaoOp";
                string strSqlSec = "SELECT " +
                                    "stu.Line," +
                                    "stu.EquipID," +
                                    "stu.`Start`," +
                                    "stu.`Stop`," +
                                    "stu.Error," +
                                    "stu.`Run`, " +
                                    "stu.ErrContent," +
                                    "stu.UpdateTime ," +
                                    " stu.ReqSig,stu.InBoard,stu.InspectBoard,stu.ReadyOut,stu.BVSig,stu.OutBoard,  " + // index:8
                    // index 13
                                    " stu.Factory, " +
                                    " stu.Customer, " +
                                    " stu.Floor, " +
                                    " stu.EquipName, " +
                    //" stu.EquipID, "+
                                    " stu.Module " +
                                "FROM " +
                                    "spidb.tbequipstatus stu " +
                                "WHERE " +
                                    "stu.UpdateTime >= '{0}' ORDER BY stu.UpdateTime;";
                while (true)
                {
                    //DBdata获取 设备信息
                    strSql = string.Format(strSqlSec, dtTMP);

                    // strSql

                    dataTable = getDataTableForZZFox(_strConnectString, strSql);

                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        //
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            strStart = dataTable.Rows[i][2].ToString(); strError = dataTable.Rows[i][4].ToString();
                            strStop = dataTable.Rows[i][3].ToString(); strRun = dataTable.Rows[i][5].ToString();
                            strTime = ((DateTime)dataTable.Rows[i][7]).ToString("yyyy/MM/dd HH:mm:ss");
                            strTime2 = ((DateTime)dataTable.Rows[i][7]).ToString("yyyy-MM-dd HH:mm:ss");
                            strFileNameTime = ((DateTime)dataTable.Rows[i][7]).ToString("yyyyMMddHHmmss");
                            strLineName = dataTable.Rows[i][0].ToString();
                            strMachine = dataTable.Rows[i][1].ToString();
                            strErrorContent = dataTable.Rows[i][6].ToString();

                            strFactory = dataTable.Rows[i][13].ToString();
                            strCustor = dataTable.Rows[i][14].ToString();
                            strFool = dataTable.Rows[i][15].ToString();
                            strEquipName = dataTable.Rows[i][16].ToString();
                            strSide = "";
                            //string strEquipID = dataTable.Rows[i][17].ToString();
                            strModule = dataTable.Rows[i][17].ToString();
                            strErrorContentEnglish = "";
                            GetErrorCodeForStrEquipStatus(strErrorContent, ref strErrorContentEnglish);
                            strFile = "";//Path.Combine(_strCsvPath, "Status_SPI_" + strFileNameTime);
                            strOP = "";
                            strFileContent = _strRenBao_Title + _strEndLine;


                            if (File.Exists(strAutoAPPIniFile))
                            {
                                strFactory = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoSaoplant, "", strAutoAPPIniFile);
                                strModule = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoGroupName, "", strAutoAPPIniFile);
                                strLineName = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoLineName, "", strAutoAPPIniFile);

                                strCustor = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoCuscode, "", strAutoAPPIniFile);
                                strFool = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoPross, "", strAutoAPPIniFile);
                                strMachine = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoFactorySN, "", strAutoAPPIniFile);

                                strSide = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoSide, "", strAutoAPPIniFile);
                                strEquipName = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoEquipType, "", strAutoAPPIniFile);
                                strOP = WSClnt.INIFileHelper.ReadIniData(KunShanRenBao, tbRenBaoOp, "", strAutoAPPIniFile);
                            }

                            dtTMP = (DateTime)dataTable.Rows[i][7];
                            string strErrormessage = "";
                            //error
                            if (strError == "True")
                            {
                                if (string.IsNullOrEmpty(File.ReadAllText(strRenBaoFileByError)))
                                {
                                    // error
                                    strFileContent = strFileContent +
                                    strTime + _strSpit +
                                    strLineName + _strSpit +
                                    strMachine + _strSpit +
                                    "C02" + _strSpit +
                                    "TroubleStopStart" + _strSpit +
                                    strErrorContent + _strSpit +
                                    strErrorContentEnglish;
                                    strFile = Path.Combine(_strCsvPath, "Status_SPI_" + dtTMP.ToString("yyyyMMddHHmmss") + ".csv");
                                    arrDataValue5[0] = strTime;
                                    arrDataValue5[1] = "C02";
                                    arrDataValue5[2] = "TroubleStopStart";
                                    arrDataValue5[3] = strErrorContent;
                                    arrDataValue5[4] = strErrorContentEnglish;
                                    File.WriteAllText(strFile, strFileContent, Encoding.Default);
                                    File.WriteAllText(strStartAndReset, "StartAndReset", Encoding.Default);
                                    File.WriteAllText(strRenBaoFileByError, "error", Encoding.Default);
                                    bool bResult = UploadDataFileForRenBaoMES(strFactory, strLineName, strModule, strCustor, strFool, strMachine, strEquipName, strOP, strSide, "Status_SPI_" + dtTMP.ToString("yyyyMMddHHmmss") + ".csv", out strErrormessage, arrDataValue5, arrStutsKey5);

                                    if (bResult)
                                    {
                                        //_log.WriteLog("C02 success...", "RenBao");
                                        AppLogHelp.WriteLog(LogFileFormate.MES, " RenBao C02 success... ");
                                    }
                                    else
                                    {
                                        //_log.WriteErr("C02 fail..:" + strErrormessage, "RenBao");
                                        AppLogHelp.WriteLog(LogFileFormate.MES,  "RenBao"+strErrormessage);
                                    }
                                }
                            }
                            //软件点开始
                            else if (strStart == "True" && strRun == "True")
                            {
                                //读取本地临时文件，如果此时存在，那么读取做出判定; 如果不存在则创建一个新的文件
                                //  第一种情况  软件点开始没有 并非解除异常
                                if (string.IsNullOrEmpty(File.ReadAllText(strStartAndReset)))
                                {
                                    if (string.IsNullOrEmpty(File.ReadAllText(strRenBaoFileByStart)))
                                    {
                                        strFileContent = strFileContent +
                                            strTime + _strSpit +
                                            strLineName + _strSpit +
                                            strMachine + _strSpit +

                                            "C01" + _strSpit +
                                            "ProductionStart" + _strSpit +
                                            "null" + _strSpit +
                                            "null";
                                        arrDataValue5[0] = strTime;
                                        arrDataValue5[1] = "C01";
                                        arrDataValue5[2] = "ProductionStart";
                                        arrDataValue5[3] = "";
                                        arrDataValue5[4] = "";
                                        //输出csv
                                        strFile = Path.Combine(_strCsvPath, "Status_SPI_" + dtTMP.ToString("yyyyMMddHHmmss") + ".csv");
                                        File.WriteAllText(strFile, strFileContent, Encoding.Default);
                                        File.WriteAllText(strRenBaoFileByStart, "start", Encoding.Default);
                                        File.WriteAllText(strRenBaoFileByStop, "", Encoding.Default);
                                        File.WriteAllText(strRenBaoFileByError, "", Encoding.Default);

                                        bool bResult = UploadDataFileForRenBaoMES(strFactory, strLineName, strModule, strCustor, strFool, strMachine, strEquipName, strOP, strSide, "Status_SPI_" + dtTMP.ToString("yyyyMMddHHmmss") + ".csv", out strErrormessage, arrDataValue5, arrStutsKey5);

                                        if (bResult)
                                        {
                                            //_log.WriteLog("C01 success..", "RenBao");
                                            AppLogHelp.WriteLog(LogFileFormate.MES, " RenBao C01 success.. ");

                                        }
                                        else
                                        {
                                           // _log.WriteErr("C01 fail..:" + strErrormessage, "RenBao");
                                            AppLogHelp.WriteLog(LogFileFormate.MES, "RenBao C01 fail.." + strErrormessage);
                                        }
                                    }
                                }
                                else
                                {
                                    //解除异常状态 3
                                    strFileContent = strFileContent +
                                    strTime + _strSpit +
                                    strLineName + _strSpit +
                                    strMachine + _strSpit +
                                    "C03" + _strSpit +
                                    "TroubleStopEnd" + _strSpit +
                                    "null" + _strSpit +
                                    "null";
                                    //输出csv
                                    arrDataValue5[0] = strTime;
                                    arrDataValue5[1] = "C03";
                                    arrDataValue5[2] = "TroubleStopEnd";
                                    arrDataValue5[3] = "";
                                    arrDataValue5[4] = "";
                                    strFile = Path.Combine(_strCsvPath, "Status_SPI_" + dtTMP.ToString("yyyyMMddHHmmss") + ".csv");
                                    File.WriteAllText(strFile, strFileContent, Encoding.Default);
                                    //File.WriteAllText(strRenBaoFileByErrorReset, "errorReset", Encoding.Default);
                                    File.WriteAllText(strStartAndReset, "", Encoding.Default);
                                    File.WriteAllText(strRenBaoFileByError, "", Encoding.Default);
                                    File.WriteAllText(strRenBaoFileByStop, "", Encoding.Default);

                                    bool bResult = UploadDataFileForRenBaoMES(strFactory, strLineName, strModule, strCustor, strFool, strMachine, strEquipName, strOP, strSide, "Status_SPI_" + dtTMP.ToString("yyyyMMddHHmmss") + ".csv", out strErrormessage, arrDataValue5, arrStutsKey5);

                                    if (bResult)
                                    {
                                        //_log.WriteLog("C03 success..", "RenBao");
                                        AppLogHelp.WriteLog(LogFileFormate.MES, " RenBao C03 success.. ");
                                    }
                                    else
                                    {
                                        //_log.WriteErr("C03 fail.." + strErrormessage, "RenBao");
                                        AppLogHelp.WriteLog(LogFileFormate.MES, "RenBaoC03 fail.." + strErrormessage);
                                    }

                                    //_log.WriteLog("解除异常状态log输出", "RenBao");
                                }
                            }
                            else if (strStop == "True")
                            {
                                if (string.IsNullOrEmpty(File.ReadAllText(strRenBaoFileByStop)))
                                {
                                    //stop
                                    strFileContent = strFileContent +
                                    strTime + _strSpit +
                                    strLineName + _strSpit +
                                    strMachine + _strSpit +
                                    "C04" + _strSpit +
                                    "ProductionEnd" + _strSpit +
                                    "null" + _strSpit +
                                    "null";
                                    arrDataValue5[0] = strTime;
                                    arrDataValue5[1] = "C04";
                                    arrDataValue5[2] = "ProductionEnd";
                                    arrDataValue5[3] = "";
                                    arrDataValue5[4] = "";
                                    strFile = Path.Combine(_strCsvPath, "Status_SPI_" + dtTMP.ToString("yyyyMMddHHmmss") + ".csv");
                                    File.WriteAllText(strFile, strFileContent, Encoding.Default);
                                    File.WriteAllText(strRenBaoFileByStart, "", Encoding.Default);
                                    File.WriteAllText(strRenBaoFileByError, "", Encoding.Default);
                                    File.WriteAllText(strRenBaoFileByStop, "stop", Encoding.Default);
                                    bool bResult = UploadDataFileForRenBaoMES(strFactory, strLineName, strModule, strCustor, strFool, strMachine, strEquipName, strOP, strSide, "Status_SPI_" + dtTMP.ToString("yyyyMMddHHmmss") + ".csv", out strErrormessage, arrDataValue5, arrStutsKey5);

                                    if (bResult)
                                    {
                                        //_log.WriteLog("C04 success..", "线程RenBao");
                                        AppLogHelp.WriteLog(LogFileFormate.MES, " RenBao C04 success.. ");
                                    }
                                    else
                                    {
                                        //_log.WriteErr("C04 fail.." + strErrormessage, "线程RenBao");
                                        AppLogHelp.WriteLog(LogFileFormate.MES, "RenBaoC03 fail.." + strErrormessage);
                                    }
                                    //_log.WriteLog("停止状态log输出成功", "RenBao");
                                }

                            }
                            ClearMemory();
                            strFileContent = string.Empty;
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000);
                        //_log.WriteLog(dtTMP+":wait Status...","RenBao");
                    }
                    System.Threading.Thread.Sleep(100);
                    strSql = string.Empty; strSqlForPCBInfo = string.Empty;
                }
            }
            catch (Exception e)
            {
               // _log.WriteErr("错误:" + e.Message, "ThreadName:RenBao");
                AppLogHelp.WriteLog(LogFileFormate.MES, " ThreadName:RenBao " + e.Message);
            }

        }

        public void StartRenBao_ToFile(string strCsvPath, string strCsvPath2)
        {
            try
            {
                //this._log = log;
                this._strCsvPath = strCsvPath;
                this._strCsvPath2 = strCsvPath2;
                StartThreadForRenBao_ToFile();
            }
            catch (Exception e)
            {
                //_log.WriteErr("错误:" + e.Message, "线程RenBao");
            }
        }

        
        private void GetErrorCodeForStrEquipStatus(string AstrErrorcode,ref string AstrErrorContent)
        {
            try
            {
                switch (AstrErrorcode)
                {
                    case "20000":
                        AstrErrorContent = "EMG";
                        break;
                    case "20001":
                        AstrErrorContent = "OpenDoor";
                        break;
                    case "20002":
                        AstrErrorContent = "NoAir";
                        break;
                    case "20003":
                        AstrErrorContent = "X_LMT_P,//X LMT +";
                        break;
                    case "20004":
                        AstrErrorContent = "X_LMT_N,//X LMT -";
                        break;
                    case "20005":
                        AstrErrorContent = "Y_LMT_P,//Y LMT +";
                        break;
                    case "20006":
                        AstrErrorContent = "Y_LMT_N,//Y LMT -";
                        break;
                    case "20007":
                        AstrErrorContent = "Z_LMT_P,//Z LMT +";
                        break;
                    case "20008":
                        AstrErrorContent = "Z_LMT_N,//Z LMT -";
                        break;
                    case "20009":
                        AstrErrorContent = "InLet_TimeOut_ByPass_Lane1, //inlet sensor time out";
                        break;
                    case "20010":
                        AstrErrorContent = "InLet_TimeOut_AutoMode_Lane1,//inlet sensor time out";
                        break;
                    case "20011":
                        AstrErrorContent = "InLet2Center_TimeOut_ByPass_Lane1";
                        break;
                    case "20012":
                        AstrErrorContent = "InLet2Center_TimeOut_Lane1";
                        break;
                    case "20013":
                        AstrErrorContent = "SlowDown_Sensor_Err_Lane1";
                        break;
                    case "20014":
                        AstrErrorContent = "AdjWidth_Reach_Max_Lane1";
                        break;
                    case "20015":
                        AstrErrorContent = "AdjWidth_Reach_Min_Lane1";
                        break;
                    case "20016":
                        AstrErrorContent = "OutLet_TimeOut_ByPass_Lane1, //OutLet sensor time out";
                        break;
                    case "20017":
                        AstrErrorContent = "OutLet_TimeOut_AutoMode_Lane1,//OutLet sensor time out";
                        break;
                    case "20018":
                        AstrErrorContent = "Center2OutLet_TimeOut_Lane1";
                        break;
                    case "20019":
                        AstrErrorContent = "Stopper_Err_Lane1";
                        break;
                    case "20020":
                        AstrErrorContent = "Stopper_DualBoard_Lane1";
                        break;
                    case "20021":
                        AstrErrorContent = "Table_Err_Lane1";
                        break;
                    case "20022":
                        AstrErrorContent = "InLet_TimeOut_ByPass_Lane2, //inlet sensor time out";
                        break;
                    case "20023":
                        AstrErrorContent = "InLet_TimeOut_AutoMode_Lane2,//inlet sensor time out";
                        break;
                    case "20024":
                        AstrErrorContent = "InLet2Center_TimeOut_ByPass_Lane2";
                        break;
                    case "20025":
                        AstrErrorContent = "InLet2Center_TimeOut_Lane2";
                        break;
                    case "20026":
                        AstrErrorContent = "SlowDown_Sensor_Err_Lane2";
                        break;
                    case "20027":
                        AstrErrorContent = "AdjWidth_Reach_Max_Lane2";
                        break;
                    case "20028":
                        AstrErrorContent = "AdjWidth_Reach_Min_Lane2";
                        break;
                    case "20029":
                        AstrErrorContent = "OutLet_TimeOut_ByPass_Lane2, //OutLet sensor time out";
                        break;
                    case "20030":
                        AstrErrorContent = "OutLet_TimeOut_AutoMode_Lane2,//OutLet sensor time out";
                        break;
                    case "20031":
                        AstrErrorContent = "Center2OutLet_TimeOut_Lane2";
                        break;
                    case "20032":
                        AstrErrorContent = "Stopper_Err_Lane2";
                        break;
                    case "20033":
                        AstrErrorContent = "Stopper_DualBoard_Lane2";
                        break;
                    case "20034":
                        AstrErrorContent = "Table_Err_Lane2";
                        break;
                    case "20035":
                        AstrErrorContent = "AdjWidth_Reach_Max_Lane2_Rail3";
                        break;
                    case "20036":
                        AstrErrorContent = "AdjWidth_Reach_Min_Lane2_Rail3";
                        break;
                    case "10000":
                        AstrErrorContent = "UI_MarkErr";
                        break;
                    case "10001":
                        AstrErrorContent = "UI_BarcodeErr";
                        break;
                    case "10002":
                        AstrErrorContent = "UI_CPK";
                        break;
                    case "10003":
                        AstrErrorContent = "UI_PassRatio";
                        break;
                    case "10004":
                        AstrErrorContent = "UI_RealChart";
                        break;
                    case "10005":
                        AstrErrorContent = "UI_Coplanarity";
                        break;
                    case "10006":
                        AstrErrorContent = "UI_PassRatio";
                        break;
                    case "10007":
                        AstrErrorContent = "UI_NGContinuity";
                        break;
                    case "10008":
                        AstrErrorContent = "UI_NormalAlarm";
                        break;
                    case "10009":
                        AstrErrorContent = "UI_ManualInput";
                        break;
                    case "10010":
                        AstrErrorContent = "ExtBrcd_PreScan";
                        break;
                    case "10011":
                        AstrErrorContent = "ExtBrcd_PostScan";
                        break;
                    case "10012":
                        AstrErrorContent = "ExtBrcd_MultiRd";
                        break;
                    case "10013":
                        AstrErrorContent = "UI_MESErr";
                        break;
                    case "10014":
                        AstrErrorContent = "UI_OCVErr";
                        break;
                    default :
                        break;
                }


            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public void StopThreadRenBao()
        {
            try
            {
                System.Threading.Thread t = System.Threading.Thread.CurrentThread;
                if (t != null && t.ThreadState != System.Threading.ThreadState.Aborted )
                {
                    //_log.WriteLog("线程RenBao关闭成功", "RenBao");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    //System.Threading.Thread.Sleep(300);
                    
                }
                //_log.WriteLog("线程RenBao关闭成功", "RenBao");
            }
            catch (Exception e)
            {
                //_log.WriteErr("线程关闭错误:" + e.Message, "线程RenBao");
            }
        }

        
        private bool UploadDataFileForRenBaoMES(string AstrFactory,  //厂区
            string AstrLineName, // 线体
            string AstrModule,   // 模组
            string AstrCustCode, // 客户
            string AstrFool,  //制程
            string AstrEquipID,  //设备ID
            string AstrEquipType, //类型
            string AstrOP,
            string AsreSide,
            string AstrFileName,
            out string Amessage,
            string[] AarrValue,
            string[] AarrKey//length ==6
            )
        {
            string strSide = "";
             Amessage = "";
            if (string.IsNullOrEmpty(AstrFool))
            {
                AstrFool = "SMT";
                strSide = "S";
            }
            else
            {
                strSide = AstrFool.Substring(0,1);
            }
            ExecutionResult execRes = new ExecutionResult();
            ServiceLibraryClient client = new ServiceLibraryClient();
            List<SingleLogItemDef> cLogList = new List<SingleLogItemDef>();
            ClientTransferData cTmpClientData = new ClientTransferData();
            SingleLogItemDef cSingleLogItem = null;
            cTmpClientData.SapPlant = AstrFactory;
            cTmpClientData.LineName = AstrLineName;
            cTmpClientData.GroupName = AstrModule;
            cTmpClientData.CustCode = AstrCustCode;
            cTmpClientData.Process = AstrFool;
            cTmpClientData.MountSide = AsreSide;
            cTmpClientData.FactorySN = AstrEquipID;
            cTmpClientData.LogFileName = AstrFileName;
            cTmpClientData.EquipType = AstrEquipType;
            cTmpClientData.OpeUser = AstrOP;
            
            if (AarrValue.Length > 0 )
            {
                for (int j = 0; j < AarrValue.Length; j++)
                {
                    cSingleLogItem = new SingleLogItemDef();
                    cSingleLogItem.KeyStr = AarrKey[j];
                    cSingleLogItem.KeyValue = AarrValue[j];
                    cLogList.Add(cSingleLogItem);
                    cSingleLogItem = null;
                }
                cTmpClientData.LogFieldList = cLogList.ToArray<SingleLogItemDef>();
            }
            try
            {
                //_log.WriteLog("client接口","renbao");
                execRes = JsonConvert.DeserializeObject<ExecutionResult>(client.CompalClientTransferEquipSerialData(JsonConvert.SerializeObject(cTmpClientData)));
                
                
            }
            catch (Exception ex)
            {
               // _log.WriteLog("异常---服务器接口" + execRes.Message, "RenBao");
                execRes.Status = false;
                execRes.Message = ex.Message;
                return execRes.Status;
            }finally
            {
                Amessage = execRes.Message;
                execRes = null;
                cTmpClientData = null;
                if (cLogList != null)
                {
                    cLogList.Clear();
                }
                client.Close();
                //client = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
            
            return execRes.Status;
        }
        
        #endregion

        #region  "FOV-PCB多线程"
        public static ThreadProcessFov runnerFov;
        private ThreadProcessFovEYSPIToAOI runnEYSPIToAOI;
        //private AppLayerLib.AppSettingHandler _appSettingHandle = new AppLayerLib.AppSettingHandler();
        public void AutoStartFov( InspectConfig.ConfigData Aconfig, string ExToFovImagePath, AppLayerLib.AppSettingHandler AappSettingHandle)
        {
            try
            {
                runnerFov = new ThreadProcessFov( Aconfig, ExToFovImagePath, AappSettingHandle);
                runnerFov.Run();
            }
            catch (Exception e)
            {
                //log.WriteErr("fov错误:" + e.Message, ThreadFovName);
                AppLogHelp.WriteError(LogFileFormate.FOVPCB, e.Message);
            }
        }
       
        private static Byte[] _arrByteFov = { 0 };
        private static string _strFovDateDirPath = string.Empty;
        private ImgCSCoreIM.ST_SaveOrLoadImgData[] _ST_SaveOrLoadImgData;
        private ImgCSCoreIM.ST_SaveOrLoadImgData _saveImage;
        private string _strPerWidthHeight = string.Empty;
        private bool _bNeedZoom = true;
        private EYSPIToAOIHelp helpAOI = new EYSPIToAOIHelp();
        private AutoAPP.Basefunction _baseFuc = new Basefunction();

        public static List<APP_SaveOrLoadImgDataInFo> arrAPP_SaveOrLoadImgDataInFo = new List<APP_SaveOrLoadImgDataInFo>();

        private void ClearALLImgData()
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
        List<float> arrPosXmm = new List<float>(), arrPosYmm = new List<float>();
        List<int> arrIFovID = new List<int>();
        public void ReadFovImages( InspectConfig.ConfigData Aconfig, string ExToFovImagePath, AppLayerLib.AppSettingHandler appSettingHandle)
        {
                _configData = Aconfig;
               //// bool bTmp = true;
               // AppLayerLib.AppSettingHandler appSettingHandle = new AppLayerLib.AppSettingHandler();
                string[] arrStrFovFileLines = null;
                string[] arrFovFiles = null; string strFirstPath = "", FovDirPath = string.Empty,
                    strDataExportSaveFovImagePath = string.Empty;
                int interval = 0;
                bool bEnDataFov = false;
                string[] arrStrTxtPCBPath = null;
                while (true)
                {
                    if (!AutoAPP.MainForm.bEnDataForPCB && !AutoAPP.MainForm.bEnDataForFov)
                    {
                        break;
                    }
                    //实例化appSettingHandler
                    //_appSettingHandle = AutoAPP.MainForm._appSettingHandle;
                    //_appSettingHandle._.stDataExpVT.stSavePCBImageParams.byImageAdjustPercent
                    
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
                                    //strFirstPath = Path.Combine(AutoAPP.MainForm._IP, strFirstPath);   //20190826 moditied

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
                                                                            ref _configData, _bNeedZoom, _baseFuc, _strPerWidthHeight, strDataExportSaveFovImagePath,
                                                                            _stImgData, ExToFovImagePath,
                                                                            iCount,""
                                                                            )
                                                                            ;
                                                            //double tmp = (DateTime.Now - dtTime).TotalSeconds;
                                                            if (string.IsNullOrEmpty(str))
                                                            {
                                                               // log.WriteLog("saveFOV success end" + str, "SaveFOVThread");
                                                                AppLogHelp.WriteLog(LogFileFormate.FOVPCB, "saveFOV success end=>" + strFirstPath);
                                                            }
                                                            else
                                                            {
                                                                //log.WriteErr("error:saveFOV fail end" + str, "SaveFOVThread");
                                                                AppLogHelp.WriteError(LogFileFormate.FOVPCB, "saveFOV fail end=>" + strFirstPath);
                                                            }
                                                        }
                                                        else if (appSettingHandle._appSettingData.stDataExpVT.stSavePCBImageParams.bEnabled)
                                                        {
                                                            for (int i = 0; i < iCount; i++)
                                                            {
                                                                _baseFuc.LoadImadat(strFirstPath + "\\" + i.ToString(), ref arrAPP_SaveOrLoadImgDataInFo[i].stImgData);
                                                                
                                                            }
                                                            //SavePCBImages savePCB = new SavePCBImages();
                                                            
                                                            _savePCB.SaveWholePCBImage(strBarcode, strDate, strJobName, arrAPP_SaveOrLoadImgDataInFo
                                                            , ref appSettingHandle._appSettingData, ref _configData, dtStartTime, _strPerWidthHeight, _baseFuc, _stImgData, ExToFovImagePath, "");
                                                            //log.WriteLog("savePCB success end", "SavePCBThread");
                                                            AppLogHelp.WriteLog(LogFileFormate.FOVPCB, "savePCB success end=>" + strFirstPath);
                                                        }
                                                        //  }
                                                        //});
                                                        Directory.Delete(strFirstPath, true);
                                                        //arrAPP_SaveOrLoadImgDataInFo.Clear();
                                                        ClearALLImgData();

                                                        #region  "UI Save Days"
                                                        //log.WriteLog("delStart:");
                                                        cEYSPIToAOIHelp.ReadAppSetting();
                                                        if (cEYSPIToAOIHelp._appSetting.bEnDeleteUIFovImages)
                                                        {
                                                            //appSettingHandle._appSettingData.stDataExpVT.
                                                            //delete fr UI Fov days
                                                            int iFovDays = appSettingHandle._appSettingData.stDataExpVT.shSaveFovImageMaximalDays;
                                                            int iPcbDays = appSettingHandle._appSettingData.stDataExpVT.stSavePCBImageParams.shSaveMaximalDays;
                                                            //log.WriteLog(iPcbDays+"", "UIPCB天数");
                                                            string strSaveFovPath = appSettingHandle._appSettingData.stDataExpVT.strSaveFovImagePath;
                                                            string strSavePcbPath = appSettingHandle._appSettingData.stDataExpVT.stSavePCBImageParams.strSaveImagePath;
                                                            if (Directory.Exists(strSaveFovPath))
                                                            {
                                                                _baseFuc.DeleteDataGenDays(strSaveFovPath, 30, iFovDays);
                                                            }
                                                            if (Directory.Exists(strSavePcbPath))
                                                            {

                                                                _baseFuc.DeleteDataGenDays(strSavePcbPath, 30, iPcbDays);
                                                            }
                                                        }
                                                        //log.WriteLog("delEnd:");
                                                        #endregion

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
                            System.Threading.Thread.Sleep(300); //sleep timer
                            AutoAPP.MainForm.bFov = true;
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        //log.WriteErr("错误:" + e.Message, "FOV线程 ReadFovImages");
                        AppLogHelp.WriteError(LogFileFormate.FOVPCB, "ReadFovImages " + e.Message);
                        //System.Windows.Forms.MessageBox.Show("FOV线程 ReadFovImages异常__请手动检查ExToFovImage文件夹是否转换成功.." + e.Message);
                        //MessageBox.Show("FOV线程 ReadFovImages异常__请手动检查ExToFovImage文件夹是否转换成功.." + e.Message + "  [异常路径..]"+strFirstPath, "系统提示",

                        //                                        MessageBoxButtons.OK, MessageBoxIcon.Warning,

                        //                                        MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        AutoAPP.MainForm.bFov = true;
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

        public void AutoStartForEYSPIToAOI(InspectConfig.ConfigData Aconfig, int iLimitFileNum, string ExToFovImagePath, AppLayerLib.AppSettingHandler appSettingHandle)
        {
            try
            {
                runnEYSPIToAOI = new ThreadProcessFovEYSPIToAOI( Aconfig, iLimitFileNum, ExToFovImagePath, appSettingHandle);
                runnEYSPIToAOI.Run();
            }
            catch (Exception e)
            {
                 
                AppLogHelp.WriteError(LogFileFormate.EYspiToAOI, "设备状态线程异常 "+ e.Message);
            }
        }

        //private string _ExEYSPIToSPIPath = @"D:\EYSPI\DataExport\ExToEyspiToAOI";
        
        public void ReadFovImagesForEYSPIToAOI( InspectConfig.ConfigData Aconfig, int iLimitFileNum, string ExToFovImagePath, AppLayerLib.AppSettingHandler appSettingHandle)
        {
            
            while (true)
            {

                if (AutoAPP.MainForm.bEnEYSPIToAOI == false)
                {
                    break;
                }
                //实例化appSettingHandler
                try
                {
                    //appSettingHandle._appSettingData.stDataExpVT.strSaveFovImagePath = @"D:\EYSPI\DataExport\LinkToAOI";
                    //appSettingHandle._appSettingData.stDataExpVT.stSavePCBImageParams.strSaveImagePath = @"D:\EYSPI\DataExport\PCBImage";
                    //appSettingHandle._appSettingData.stDataExpVT.bEnSaveFovImage = false;
                    //appSettingHandle._appSettingData.stDataExpVT.stSavePCBImageParams.bEnabled = false;
                    //appSettingHandle._appSettingData.stDataExpVT.byFovImageAdjustPercent = 100;
                    //appSettingHandle._appSettingData.stDataExpVT.stSavePCBImageParams.byImageAdjustPercent = 30;
                    //appSettingHandle._appSettingData.stDataExpVT.stSavePCBImageParams.bFillBlack = true;
                    //appSettingHandle._appSettingData.stDataExpVT.bUseAutoAppToGenImage = true;
                    //appSettingHandle._configData._bEnGenROiCADImg = false;
                    //appSettingHandle._appSettingData.bEnGenAOIFile = true;
                    //D:\EYSPI\DataExport\ExToFovImage
                    //UI给出fov的目录路径;
                    int interval = appSettingHandle._appSettingData.stDataExpVT.IntervalSecond;
                    bool bEnDataFov = appSettingHandle._appSettingData.stDataExpVT.bUseAutoAppToGenImage;// 开关Fov保存图片
                    string strDataExportSaveFovImagePath = string.Empty;
                    //ParallelOptions parallelOpt = new ParallelOptions();
                    //parallelOpt.MaxDegreeOfParallelism = iMaxParallelCount;
                    //if (Directory.GetDirectories(FovDirPath) != null && Directory.GetDirectories(FovDirPath).Length > 0)
                    //bEnDataFov = false;
                    if (bEnDataFov)
                    {
                        if (appSettingHandle._appSettingData.bEnGenAOIFile || appSettingHandle._appSettingData.bEnLinkToAOIStation)
                        {
                            if (string.IsNullOrEmpty(ExToFovImagePath) == false)
                            {
                                ExToFovImagePath = ExToFovImagePath.Replace("ExToFovImage", "ExToEyspiToAOI");
                            }
                            helpAOI.StartProcess( appSettingHandle._appSettingData, ExToFovImagePath);
                        }
                        System.Threading.Thread.Sleep(300);
                    }
                }
                catch (Exception e)
                {
                    //log.WriteErr("错误:" + e.Message, "eyspiToAOI");
                    AppLogHelp.WriteError(LogFileFormate.EYspiToAOI, e.Message);
                    //log.WriteErr("错误:" + e.Message, "FOV线程 ReadFovImages");
                    ////System.Windows.Forms.MessageBox.Show("FOV线程 ReadFovImages异常__请手动检查ExToFovImage文件夹是否转换成功.." + e.Message);
                    //MessageBox.Show("FOV线程 ReadFovImages异常__请手动检查ExToFovImage文件夹是否转换成功.." + e.Message, "系统提示",
                    //AutoAPP.MainForm.bAOI = true;
                    return;
                }
                //System.Threading.Thread.Sleep(1000); //sleep timer
            }
            //AutoAPP.MainForm.bAOI = true;

        }


        

        [DllImport("psapi.dll")]
        static extern int EmptyWorkingSet(IntPtr hwProc);

        /// <summary>
        /// 释放内存
        /// </summary>
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

        #endregion

        #region "天津天地伟业"
        public void AutoStartTianJinWeiYe(InspectConfig.ConfigData Aconfig)
        {
            try
            {
                runnerTianJinWeiYe = new ThreadProcessTianJinWeiYe(Aconfig);
                runnerTianJinWeiYe.Run();
            }
            catch (Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.ToString(), ThreadNameLHCNN);
                AppLogHelp.WriteError(LogFileFormate.AppMain, ThreadNameCDCNN + ex.ToString());
            }
        }
        public static void SaveDataForTianJinWeiYeMes(string AsMySqlConnectionString,
            string strLastPcbPath, string strLastPcbFile, string strLastAppsettingTmpFile, string strAppsettingPath,
            AppLayerLib.AppSettingHandler appSettingHandle,
            int timer,string _strRealConfigPath)
        {
            try
            {
                while (true)
                {
                    string nextPcb = string.Empty;
                    if (File.Exists(strLastPcbFile))
                    {
                        using (StreamReader srMain = new StreamReader(strLastPcbFile))
                        {
                            string str = "";
                            if ((str = srMain.ReadLine()) != null)
                            {
                                nextPcb = str;
                            }
                        }
                    }
                    string sql = "SELECT t.PCBID,t.StartTime,t.PCBBarcode,t.JobIndex,t.LineNo FROM spidb.TBBoard t WHERE t.PCBID >  '" + nextPcb + "'" + "{0}  and t.Result in (0,2)   ORDER BY t.PCBID ASC";
                    sql = string.Format(sql, "");
                    System.Threading.Thread.Sleep(8000);
                    DataTable dt = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, sql );
                    if (dt != null && dt.Rows.Count > 0)
                    {
                         //sleep 5s
                        //sql = string.Format(sql, " AND t.OPConfirmed ='1' ");
                        //DataTable dtConfimOP = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, sql, log);
                        //if(dt)
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i][0] != null)
                            {
                                string path = strAppsettingPath;
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
                                string sAppSettingFileTmp = strAppsettingPath + "\\" + max + ".bin";
                                if (File.Exists(sAppSettingFileTmp))
                                {
                                    appSettingHandle.Read(sAppSettingFileTmp,_strRealConfigPath);
                                    timer = appSettingHandle._appSettingData.stDataExpVT.IntervalSecond;       //等待间隔时间
                                    //储存本次pcbID至   strLastAppsettingTmpFile
                                    using (FileStream fsApp = new FileStream(strLastAppsettingTmpFile, FileMode.Create))
                                    {
                                        StreamWriter swApp = new StreamWriter(fsApp, System.Text.Encoding.Default);
                                        swApp.Write(dt.Rows[i][0].ToString());
                                        swApp.Close();
                                    }
                                }
                                string stationName = appSettingHandle._appSettingData.stDataExpVT.strSetFOP;
                                string jobIndex = dt.Rows[i][3].ToString();
                                string strLineNo = dt.Rows[i][4].ToString();
                                string barCode = dt.Rows[i][2].ToString();
                                string strLineName = appSettingHandle._appSettingData.LineName;
                                string strGroupID = appSettingHandle._appSettingData.stDataExpVT.strGroupName;    //UI 工序号
                                string strCreater = appSettingHandle._appSettingData.strDataExpOperator;
                                string strEquipNO = appSettingHandle._appSettingData.stDataExpVT.strMachine;      //UI  设备ID
                                string strVfree1 = appSettingHandle._appSettingData.stDataExpVT.strOperatorType;  //UI   操作编码
                                string strVfree2 = "";//   UI备用
                                string strVfree3 = "";//   UI备用
                                string strVfree4 = "";//   UI备用
                                string strVfree5 = "";//   UI备用
                                if (string.IsNullOrEmpty(strLineName) ||
                                    string.IsNullOrEmpty(strGroupID) ||
                                    string.IsNullOrEmpty(strCreater) ||
                                    string.IsNullOrEmpty(strEquipNO) ||
                                    //string.IsNullOrEmpty(strURL) ||
                                    string.IsNullOrEmpty(strVfree1))
                                {
                                    //strMsg = "UI Param is Null !ex:line,group,equip,url;";
                                }
                                //appSettingHandle._appSettingData.stDataExpVT.bEnExportNGInfo = true;

                                if (string.IsNullOrEmpty(barCode)) barCode = "noread";
                                Demo.BarCodeProAPI.DocInfoModel DemoModel = new Demo.BarCodeProAPI.DocInfoModel();
                                DemoModel.BarCode = barCode;
                                DemoModel.CreatedBy = strCreater;
                                DemoModel.EquipNO = strEquipNO;
                                DemoModel.LineNO = strLineName;
                                DemoModel.ProcessCode = strGroupID;
                                DemoModel.vfree1 = strVfree1;
                                DemoModel.vfree2 = strVfree2;
                                DemoModel.vfree3 = strVfree3;
                                DemoModel.vfree4 = strVfree4;
                                DemoModel.vfree5 = strVfree5;

                                TJ_TDYDLL(DemoModel);
                                //储存本次pcbID至 strLastPcbFile 以便下次读取
                                using (FileStream fsPcb = new FileStream(strLastPcbFile, FileMode.Create))
                                {
                                    StreamWriter swPcb = new StreamWriter(fsPcb, System.Text.Encoding.Default);
                                    swPcb.Write(dt.Rows[i][0].ToString());
                                    swPcb.Close();
                                }
                            }
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(5000);
                    }
                }


            }
            catch (Exception ex)
            {
                //log.WriteErr("错误!" + ex.Message, "天地伟业[SaveDataForTianJinWeiYeMes]");
            }


        }
        private static void TJ_TDYDLL(Demo.BarCodeProAPI.DocInfoModel DemoModel)
        {
            try
            {
                List<Demo.BarCodeProAPI.DocInfoModel> List = new List<Demo.BarCodeProAPI.DocInfoModel>();
                List.Add(DemoModel);
                Demo.BarCodeProAPI.BarCodeProAPISoapClient oClient = new Demo.BarCodeProAPI.BarCodeProAPISoapClient();
                Demo.BarCodeProAPI.DocReturnModel Rtn = oClient.Do(List.ToArray());
                if (Rtn.IsSuccess == true)
                {
                    //log.WriteLog("webService上抛成功", "TianJinWeiYe");
                    AppLogHelp.WriteLog(LogFileFormate.MES, "webService上抛成功:" + Rtn.Msg);
                }
                else
                {
                    //log.WriteErr("webService上抛失败:" + Rtn.Msg, "TianJinWeiYe");
                    AppLogHelp.WriteError(LogFileFormate.MES, "webService上抛失败:" + Rtn.Msg);
                }
            }
            catch (Exception e)
            {
                //log.WriteErr("webService接口出现异常." + e.Message, "TianJinWeiYe");
                AppLogHelp.WriteError(LogFileFormate.MES, "TianJinWeiYe webService接口出现异常." + e.Message);
                return;
            }
        }

        #endregion

        #region "苏州维信"
        public void autoStartWeiXin()
        {
            try
            {
                runnWeiXin = new ThreadProcessWeiXin();
                runnWeiXin.Run();
            }
            catch (Exception e)
            {
                //log.WriteErr("WeiXin错误:" + e.Message, "runnWeiXin");
                //throw e;
            }
        }
        public void UploadDataForWeiXin()
        {
            string strAutoAPPWeiXinPath = @"D:\EYSPI\Bin\SPILogs\Peng\WeiXin_upload";
            string strPath = string.Empty;
            try
            {
                while (true)
                {
                    strMsg = string.Empty;
                    if (Directory.Exists(strAutoAPPWeiXinPath) == false)
                    {
                        System.Threading.Thread.Sleep(3000);
                        
                    }
                    else
                    {
                        if (Directory.GetDirectories(strAutoAPPWeiXinPath) != null && Directory.GetFiles(strAutoAPPWeiXinPath).Length > 0)
                        {
                            System.Threading.Thread.Sleep(300);
                            string strFirstPath = Directory.GetFiles(strAutoAPPWeiXinPath).OrderBy(p => Directory.GetLastWriteTime(p)).First();
                            if (File.Exists(strFirstPath))
                            {
                                strPath = strFirstPath;
                                string strContent = File.ReadAllText(strFirstPath, Encoding.Default);
                                Model.MainInfo mfeMainInfo = JsonHelper.DeserializeString2Object<Model.MainInfo>(strContent);
                                string[] arrReturn = MFlexEquipmentProject.EquipmentInterActive.MES_SaveResult(mfeMainInfo);
                                
                                if (arrReturn.Length > 0)
                                {
                                    if (arrReturn[0] == "0")
                                    {
                                        //File.Delete(strFile);
                                        // arrReturn[1].ToString();
                                        //log.WriteLog("上传成功!!", "WeiXin");
                                        AppLogHelp.WriteLog(LogFileFormate.MES, "上传成功!!");
                                        File.Delete(strFirstPath);
                                    }
                                    else
                                    {
                                        strMsg += arrReturn[1].ToString();
                                        //log.WriteErr("上传失败!!文件名:" + (strFirstPath) + strMsg, "WeiXin");
                                        AppLogHelp.WriteLog(LogFileFormate.MES, "上传失败!!文件名:" + (strFirstPath) + strMsg);
                                        File.Delete(strFirstPath);
                                    }
                                    
                                    
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {
               // log.WriteErr("WeiXin错误:出现错误上传的文件是:" +(strPath)+ e.Message, "runnWeiXin");
                AppLogHelp.WriteLog(LogFileFormate.MES, "WeiXin错误:出现错误上传的文件是:" + (strPath) + e.Message);
               // File.Delete(strPath);
               // throw e;
            }


        }

        #endregion

        #region "浙江大华"

        public void StartDaHua(string strCsvPath)
        {
            try
            {
                
                //this._strCsvPath = strCsvPath;
                runnDaHua = new ThreadProcessDaHua(strCsvPath);
                runnDaHua.Run();
               
            }
            catch (Exception e)
            {
                //_log.WriteErr("错误:" + e.Message, "线程RenBao");
                AppLogHelp.WriteError(LogFileFormate.MES, "StartDaHua " + e.Message);
            }
        }

        public void SaveDataForZHEJIANGDAHUA(string strCsvPath)
        {
            try
            {
                //TmpDir
                
                _strLogPengPath = Path.Combine(_strLogPengPath, "DaHua");
                string strLogDataPath = Path.Combine(_strLogPengPath, "RenBaoToData");
                if (Directory.Exists(_strLogPengPath) == false)
                {
                    Directory.CreateDirectory(_strLogPengPath);
                }
                if (Directory.Exists(strCsvPath) == false)
                {
                    Directory.CreateDirectory(strCsvPath);
                }
                
                if (Directory.Exists(strLogDataPath) == false)
                {
                    Directory.CreateDirectory(strLogDataPath);
                }
                string strRenBaoFileByStart = Path.Combine(_strLogPengPath, "renbao_start.txt");
                string strRenBaoFileByStop = Path.Combine(_strLogPengPath, "renbao_stop.txt");
                string strRenBaoFileByError = Path.Combine(_strLogPengPath, "renbao_error.txt");
                string strRenBaoFileByErrorReset = Path.Combine(_strLogPengPath, "renbao_errorReset.txt");

                string strRenBaoFileByTmpDir = Path.Combine(_strLogPengPath, "TmpDir.txt");

                string strRenBaoFileByReSig = Path.Combine(strLogDataPath, "renbao_ReSig.txt");
                string strRenBaoFileByInBoard = Path.Combine(strLogDataPath, "renbao_InBoard.txt");
                string strRenBaoFileByInSpect = Path.Combine(strLogDataPath, "renbao_InSpect.txt");
                string strRenBaoFileByReadyOut = Path.Combine(strLogDataPath, "renbao_ReadyOut.txt");
                string strRenBaoFileByBVSig = Path.Combine(strLogDataPath, "renbao_BVSig.txt");
                string strRenBaoFileByOutBoard = Path.Combine(strLogDataPath, "renbao_OutBoard.txt");
                string strRenBaoFileByInspectChecked = Path.Combine(strLogDataPath, "renbao_InspectChecked.txt"); //true false  ini默认false
                string strRenBaoFileByLastOutBoard = Path.Combine(strLogDataPath, "renbao_LastOutBoard.txt");
                string strRenBaoFileByInspectTime = Path.Combine(strLogDataPath, "renbao_InspectTime.txt");

                string strStartAndReset = Path.Combine(_strLogPengPath, "renbao_startAndReset.txt");
                System.Data.DataTable dataTable = null; System.Data.DataTable dataTablePCB = null; DataTable dtArrayResult = null;
                //初始化
                IniFileRenBaoReset(strRenBaoFileByStart, strRenBaoFileByStop, strRenBaoFileByError, strStartAndReset, strRenBaoFileByErrorReset);
                IniFileRenBaoReset(strRenBaoFileByInBoard, strRenBaoFileByInSpect, strRenBaoFileByReadyOut,
                    strRenBaoFileByBVSig, strRenBaoFileByOutBoard, strRenBaoFileByLastOutBoard, strRenBaoFileByInspectTime, strRenBaoFileByReSig, strRenBaoFileByInspectChecked);
                DateTime dtNow = DateTime.Now;

                string strStart = string.Empty, strError = string.Empty,
                                strStop = string.Empty, strRun = string.Empty;
                string strTime = string.Empty;
                string strTime2 = string.Empty;
                string strFileNameTime = string.Empty;
                string strLineName = string.Empty;
                string strMachine = string.Empty;
                string strErrorContent = string.Empty;

                string strFactory = string.Empty;
                string strCustor = string.Empty;
                string strFool = string.Empty;
                string strEquipName = string.Empty;
                string strSide = string.Empty;
                //string strEquipID = dataTable.Rows[i][17].ToString();
                string strModule = string.Empty;
                string strErrorContentEnglish = string.Empty;
                //GetErrorCodeForStrEquipStatus(strErrorContent, ref strErrorContentEnglish);
                string strFile = string.Empty;//Path.Combine(_strCsvPath, "Status_SPI_" + strFileNameTime);
                string strOP = string.Empty;
                string strFileContent = string.Empty;
                Random rand = new Random();
                string strAutoAPPIniFile = @"D:\EYSPI\Bin\Config\autoApp.ini";
                //string arraybarcode1 = "", arraybarcode2 = "", strarrayResult1 = "", strarrayResult2 = "";
                //test
                //dtNow = Convert.ToDateTime("2018-09-17 11:07:19");
                DateTime dtTMP = dtNow;
                string strSql = "", strSqlForPCBInfo = "";
                string[] arrStutsKey5 = new string[5];
                string[] arrDataValue5 = new string[5];
                arrStutsKey5[0] = "EquipmentID";
                arrStutsKey5[1] = "RunStatus";
                arrStutsKey5[2] = "ErrorCode";
                arrStutsKey5[3] = "ErrorMessage";
                arrStutsKey5[4] = "DateTime";
                string strEquls = "=",strUnderLine = "_", strFilePath = string.Empty;

                while (true)
                {
                    //DBdata获取 设备信息
                    strSql = "SELECT " +
                                    "stu.Line," +
                                    "stu.EquipID," +
                                    "stu.`Start`," +
                                    "stu.`Stop`," +
                                    "stu.Error," +
                                    "stu.`Run`, " +
                                    "stu.ErrContent," +
                                    "stu.UpdateTime ," +
                                    " stu.ReqSig,stu.InBoard,stu.InspectBoard,stu.ReadyOut,stu.BVSig,stu.OutBoard,  " + // index:8
                        // index 13
                                    " stu.Factory, " +
                                    " stu.Customer, " +
                                    " stu.Floor, " +
                                    " stu.EquipName, " +
                        //" stu.EquipID, "+
                                    " stu.Module " +
                                "FROM " +
                                    "spidb.tbequipstatus stu " +
                                "WHERE " +
                                    "stu.UpdateTime >= '" + dtTMP + "' ORDER BY stu.UpdateTime;";
                    // strSql

                    dataTable = getDataTableForZZFox(_strConnectString, strSql);
                    
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        //
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            strStart = dataTable.Rows[i][2].ToString(); strError = dataTable.Rows[i][4].ToString();
                            strStop = dataTable.Rows[i][3].ToString(); strRun = dataTable.Rows[i][5].ToString();
                            strTime = ((DateTime)dataTable.Rows[i][7]).ToString("yyyy/MM/dd HH:mm:ss");
                            strTime2 = ((DateTime)dataTable.Rows[i][7]).ToString("yyyy-MM-dd HH:mm:ss");
                            strFileNameTime = ((DateTime)dataTable.Rows[i][7]).ToString("yyyyMMddHHmmss");
                            strLineName = dataTable.Rows[i][0].ToString();
                            strMachine = dataTable.Rows[i][1].ToString();
                            strErrorContent = dataTable.Rows[i][6].ToString();

                            strFactory = dataTable.Rows[i][13].ToString();
                            strCustor = dataTable.Rows[i][14].ToString();
                            strFool = dataTable.Rows[i][15].ToString();
                            strEquipName = dataTable.Rows[i][16].ToString();
                            strSide = "";
                            //string strEquipID = dataTable.Rows[i][17].ToString();
                            strModule = dataTable.Rows[i][17].ToString();
                            strErrorContentEnglish = "";
                            GetErrorCodeForStrEquipStatus(strErrorContent, ref strErrorContentEnglish);
                            strFile = "";//Path.Combine(_strCsvPath, "Status_SPI_" + strFileNameTime);
                            strOP = "";
                            //strFileContent = _strRenBao_Title + _strEndLine;

                            strFilePath = Path.Combine(strCsvPath, strEquipName+strUnderLine+strFileNameTime+".txt");
                            //if (File.Exists(strAutoAPPIniFile))
                            //{
                            //    strEquipName = WSClnt.INIFileHelper.ReadIniData("KunShanRenBao", "tbRenBaoEquipType", "", strAutoAPPIniFile);
                            //}

                            dtTMP = (DateTime)dataTable.Rows[i][7];

                            

                            string strErrormessage = "";
                            //error
                            if (strError == "True")
                            {
                                
                                // error
                                strFileContent = arrStutsKey5[0] + strEquls + strEquipName + _strEndLine
                                                + arrStutsKey5[1] + strEquls + "Error" + _strEndLine
                                                + arrStutsKey5[2] + strEquls + strErrorContent + _strEndLine
                                                + arrStutsKey5[3] + strEquls + strErrorContentEnglish + _strEndLine
                                                + arrStutsKey5[4] + strEquls + strFileNameTime;
                                
                                
                            }
                            //软件点开始
                            else if (strStart == "True" && strRun == "True")
                            {
                                strFileContent = arrStutsKey5[0] + strEquls + strEquipName + _strEndLine
                                                + arrStutsKey5[1] + strEquls + "Run" + _strEndLine
                                                + arrStutsKey5[2] + strEquls + "N/A" + _strEndLine
                                                + arrStutsKey5[3] + strEquls + "N/A" + _strEndLine
                                                + arrStutsKey5[4] + strEquls + strFileNameTime;
                                
                            }
                            else if (strStop == "True")
                            {
                                strFileContent = arrStutsKey5[0] + strEquls + strEquipName + _strEndLine
                                                + arrStutsKey5[1] + strEquls + "Stop" + _strEndLine
                                                + arrStutsKey5[2] + strEquls + "N/A" + _strEndLine
                                                + arrStutsKey5[3] + strEquls + "N/A" + _strEndLine
                                                + arrStutsKey5[4] + strEquls + strFileNameTime;
                            }
                            if (string.IsNullOrEmpty(strFileContent) == false)
                            {
                                using (FileStream fs = new FileStream(strRenBaoFileByTmpDir, FileMode.Create))
                                {
                                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                                    sw.Write(strFileContent);
                                    sw.Close();
                                }
                                if (File.Exists(strRenBaoFileByTmpDir))
                                {
                                    File.Copy(strRenBaoFileByTmpDir, strFilePath, true);
                                    System.Threading.Thread.Sleep(200);
                                    File.Delete(strRenBaoFileByTmpDir);
                                }
                            }
                            ClearMemory(); 
                            strFileContent = string.Empty;
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(5000);
                    }
                    strSql = string.Empty; strSqlForPCBInfo = string.Empty;
                }
            }
            catch (Exception e)
            {
                //_log.WriteErr("错误:" + e.Message, "ThreadName:DaHua");
                AppLogHelp.WriteLog(LogFileFormate.MES, "ThreadName:DaHua." + e.Message);
            }


        }

        #endregion

        #region " 鲁帮通 "
        private JsonConverStatus _cStatus = new JsonConverStatus();
        public void StartLuBangTong(string AtbUSERNO,
                            string AtbMO,
                            string AtbITEMVALUE,
                            string AtbMachineNo,
                            string AtbOn,
                            string AtbSf,
                            string AclientId, string AclientSecret, string AStatusURL, string AsErrorUrl)
        {
            try
            {
                runnLuBangTong = new ThreadProcessLuBangTong( AtbUSERNO,
                             AtbMO,
                             AtbITEMVALUE,
                             AtbMachineNo,
                             AtbOn,
                             AtbSf,
                             AclientId,  AclientSecret,  AStatusURL,  AsErrorUrl);
                runnLuBangTong.Run();
            }
            catch (Exception ex)
            {
                //log.WriteErr(ex.Message, "LuBangTong");
            }
        }

        public void SaveDataLuBangTong(string AtbUSERNO,
                            string AtbMO,
                            string AtbITEMVALUE,
                            string AtbMachineNo,
                            string AtbOn,
                            string AtbSf,
                            string AclientId, string AclientSecret, string AStatusURL, string AsErrorUrl)
        {

            try
            {

                _strLogPengPath = Path.Combine(_strLogPengPath, "LuBangTong");
                string strLogDataPath = Path.Combine(_strLogPengPath, "RenBaoToData");
                if (Directory.Exists(_strLogPengPath) == false)
                {
                    Directory.CreateDirectory(_strLogPengPath);
                }

                if (Directory.Exists(strLogDataPath) == false)
                {
                    Directory.CreateDirectory(strLogDataPath);
                }
                string strRenBaoFileByStart = Path.Combine(_strLogPengPath, "renbao_start.txt");
                string strRenBaoFileByStop = Path.Combine(_strLogPengPath, "renbao_stop.txt");
                string strRenBaoFileByError = Path.Combine(_strLogPengPath, "renbao_error.txt");
                string strRenBaoFileByErrorReset = Path.Combine(_strLogPengPath, "renbao_errorReset.txt");

                string strRenBaoFileByTmpDir = Path.Combine(_strLogPengPath, "TmpDir.txt");

                string strRenBaoFileByReSig = Path.Combine(strLogDataPath, "renbao_ReSig.txt");
                string strRenBaoFileByInBoard = Path.Combine(strLogDataPath, "renbao_InBoard.txt");
                string strRenBaoFileByInSpect = Path.Combine(strLogDataPath, "renbao_InSpect.txt");
                string strRenBaoFileByReadyOut = Path.Combine(strLogDataPath, "renbao_ReadyOut.txt");
                string strRenBaoFileByBVSig = Path.Combine(strLogDataPath, "renbao_BVSig.txt");
                string strRenBaoFileByOutBoard = Path.Combine(strLogDataPath, "renbao_OutBoard.txt");
                string strRenBaoFileByInspectChecked = Path.Combine(strLogDataPath, "renbao_InspectChecked.txt"); //true false  ini默认false
                string strRenBaoFileByLastOutBoard = Path.Combine(strLogDataPath, "renbao_LastOutBoard.txt");
                string strRenBaoFileByInspectTime = Path.Combine(strLogDataPath, "renbao_InspectTime.txt");

                string strStartAndReset = Path.Combine(_strLogPengPath, "renbao_startAndReset.txt");
                string strErrorFilePath = Path.Combine(_strLogPengPath, "renbao_error.txt");
                File.WriteAllText(strStartAndReset, "0",Encoding.Default);
                File.WriteAllText(strStartAndReset, "0", Encoding.Default);
                System.Data.DataTable dataTable = new DataTable(); System.Data.DataTable dataTablePCB = new DataTable();
                DateTime dtNow = DateTime.Now;

                string strStart = string.Empty, strError = string.Empty,
                                strStop = string.Empty, strRun = string.Empty;
                string strTime = string.Empty;
                string strTime2 = string.Empty;
                string strFileNameTime = string.Empty;
                string strLineName = string.Empty;
                string strMachine = string.Empty;
                string strErrorContent = string.Empty;

                string strFactory = string.Empty;
                string strCustor = string.Empty;
                string strFool = string.Empty;
                string strEquipName = string.Empty;
                string strSide = string.Empty;
                //string strEquipID = dataTable.Rows[i][17].ToString();
                string strModule = string.Empty;
                string strErrorContentEnglish = string.Empty;
                //GetErrorCodeForStrEquipStatus(strErrorContent, ref strErrorContentEnglish);
                string strFile = string.Empty;//Path.Combine(_strCsvPath, "Status_SPI_" + strFileNameTime);
                string strOP = string.Empty;
                string strFileContent = string.Empty;
                Random rand = new Random();
                string strAutoAPPIniFile = @"D:\EYSPI\Bin\Config\autoApp.ini";
                DateTime dtTMP = DateTime.Now;
                string strSql = "", strSqlForPCBInfo = "";
                string[] arrStutsKey5 = new string[5];
                string[] arrDataValue5 = new string[5];
                arrStutsKey5[0] = "EquipmentID";
                arrStutsKey5[1] = "RunStatus";
                arrStutsKey5[2] = "ErrorCode";
                arrStutsKey5[3] = "ErrorMessage";
                arrStutsKey5[4] = "DateTime";
                string strEquls = "=", strUnderLine = "_", strFilePath = string.Empty, strIdel = string.Empty;
                int iFrequency = 0;
                int iFileNum = 0;
                string strBarcode = "Noread";
                string strJson = string.Empty;
                string strHttpResult = string.Empty;

                //00：停机或正在执行停机中
                //10：启动中
                //20：待机中
                //30：生产运行中
                //99：故障停机中

                string[] arrStatus = { "00","10","20","30","99"};

                // 
                DateTime dtTimeStart = DateTime.Now;
                string strSqlSec = "SELECT " +
                                    "stu.Line," +
                                    "stu.EquipID," +
                                    "stu.`Start`," +
                                    "stu.`Stop`," +
                                    "stu.Error," +
                                    "stu.`Run`, " +
                                    "stu.ErrContent," +
                                    "stu.UpdateTime ," +
                                    " stu.ReqSig,stu.InBoard,stu.InspectBoard,stu.ReadyOut,stu.BVSig,stu.Floor," + // index:8 stu.OutBoard
                    // index 13
                                    " stu.Factory, " +
                                    " stu.Customer, " +
                                    " stu.Floor, " +
                                    " stu.EquipName, " +
                    //" stu.EquipID, "+
                                    " stu.Module, " +
                                    " stu.`Idle` ," +
                                    " stu.Frequency " +
                                "FROM " +
                                    "spidb.tbequipstatus stu " +
                                "WHERE " +
                    //>=STR_TO_DATE('" + AstartTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S') " +  STR_TO_DATE('" + dtTMP.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S')
                                    "stu.UpdateTime > '{0}'" + "ORDER BY stu.UpdateTime ";
                string strSelectPCBarcodeSQLSec = "SELECT pcbV.PCBBarcode FROM spidb.TBBoard pcbV WHERE pcbV.EndTime = ( " +
                                                                       " SELECT MAX(pcb.EndTime) FROM spidb.TBBoard pcb WHERE pcb.EndTime <= '{0}')";
                while (true)
                {
                    //DBdata获取 设备信息
                    strSql = string.Format(strSqlSec, dtTMP);
                    // strSql

                    //log.WriteLog("sql:"+strSql,"LuBangTong"  );
                    dataTable = getDataTableForZZFox(_strConnectString, strSql);
                    //log.WriteLog("log sucess :个数" + dataTable.Rows.Count, "LuBangTong");
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        //
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            strStart = dataTable.Rows[i][2].ToString(); strError = dataTable.Rows[i][4].ToString();
                            strStop = dataTable.Rows[i][3].ToString(); strRun = dataTable.Rows[i][5].ToString();
                            strIdel = dataTable.Rows[i][19].ToString();
                            
                            strLineName = dataTable.Rows[i][0].ToString();
                            strMachine = dataTable.Rows[i][1].ToString();
                            strErrorContent = dataTable.Rows[i][6].ToString();

                            iFrequency = int.Parse(dataTable.Rows[i][20].ToString());
                            strErrorContentEnglish = "";
                            GetErrorCodeForStrEquipStatus(strErrorContent, ref strErrorContentEnglish);
                            strFile = "";//Path.Combine(_strCsvPath, "Status_SPI_" + strFileNameTime);
                            strOP = "";
                            //strFileContent = _strRenBao_Title + _strEndLine;

                            //strFilePath = Path.Combine(strCsvPath, strEquipName + strUnderLine + strFileNameTime + ".txt");
                            if (File.Exists(strAutoAPPIniFile))
                            {
                                strEquipName = WSClnt.INIFileHelper.ReadIniData("KunShanRenBao", "tbRenBaoEquipType", "", strAutoAPPIniFile);
                            }

                            dtTMP =  (DateTime)dataTable.Rows[i][7];
                            //log.WriteLog("时间:" + dtTMP,"LuBangTong");
                            string strErrormessage = "";
                            //error
                            if (strError == "True")
                            {
                                if (File.Exists(strErrorFilePath))
                                {
                                    if (File.ReadAllText(strErrorFilePath, Encoding.Default) != strErrorContent)
                                    {
                                        //File.WriteAllText(strStartAndReset, "0", Encoding.Default);
                                        string strSelectPCBarcodeSQL = string.Format(strSelectPCBarcodeSQLSec, dtTMP);
                                        //log.WriteLog(strSelectPCBarcodeSQL);
                                        try
                                        {
                                            //log.WriteLog("select pcb start","LuBangTong");
                                            dataTablePCB = getDataTableForZZFox(_strConnectString, strSelectPCBarcodeSQL);
                                            //log.WriteLog("select pcb end", "LuBangTong");
                                        }
                                        catch (Exception eee)
                                        {
                                            throw eee;
                                        }
                                        if (dataTablePCB != null && dataTablePCB.Rows.Count > 0)
                                        {
                                            strBarcode = dataTablePCB.Rows[0][0].ToString();
                                        }
                                        // error
                                        strFileContent = arrStatus[4];
                                        try
                                        {
                                            strJson = string.Concat(new string[]
				                        {
					                        "{ \"clientId\":\"",
					                        AclientId,
					                        "\",\"clientSecret\":\"",
					                        AclientSecret,
					                        "\",\"status\":\"",
					                        strFileContent,
					                        "\" }"
				                        });
                                            strJson = JsonConvert.SerializeObject(new JsonConverErrorData
                                            {
                                                clientId = AclientId,
                                                clientSecret = AclientSecret,
                                                at = AutoAPP.MainForm._strToken,
                                                on = AtbOn,
                                                sn = strBarcode,
                                                sf = AtbSf,
                                                error = strErrorContent,
                                                msg = strErrorContentEnglish,
                                                md = new MdInfomation
                                                {
                                                    SN = strBarcode,
                                                    MachineNo = AtbMachineNo,
                                                    MO = AtbMO,
                                                    ITEMVALUE = AtbITEMVALUE,
                                                    USERNO = AtbUSERNO
                                                }
                                            });
                                            //this.writeLog(this.strFilePath, "uploadError:" + strJson);
                                            strHttpResult = WebHttpHelp.HttpPost(AsErrorUrl, strJson);
                                            //this.writeLog(this.strFilePath, "uploadError返回结果:" + strHttpResult);
                                            //log.WriteLog("http start","LuBangTong");
                                            strHttpResult = strHttpResult.Replace("\"{", "{").Replace("\\\"", "\"").Replace("}\"", "}");
                                            //log.WriteLog("http end", "LuBangTong");
                                            _cStatus = JsonConvert.DeserializeObject<JsonConverStatus>(strHttpResult);
                                            if (bool.Parse(_cStatus.success))
                                            {
                                                //log.WriteLog(_cStatus.message + ";uploadError success!", "LuBangTong");
                                                AppLogHelp.WriteLog(LogFileFormate.AppMain, "LuBangTong "+_cStatus.message + ";uploadError success!");
                                                File.WriteAllText(strErrorFilePath, strErrorContent, Encoding.Default);
                                            }
                                            else
                                            {
                                                //log.WriteLog(_cStatus.message + ";uploadError fail!", "LuBangTong");
                                                AppLogHelp.WriteLog(LogFileFormate.AppMain, "LuBangTong  "+_cStatus.message + ";uploadError fail!");
                                               
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            //log.WriteLog(ex.Message + ";uploadError exception!", "LuBangTong");
                                            AppLogHelp.WriteError(LogFileFormate.AppMain, "LuBangTong  "+ex.Message + ";uploadError exception!");
                                        }
                                        strSelectPCBarcodeSQL = string.Empty;
                                        try
                                        {
                                            strJson = string.Concat(new string[]
				                                {
					                                "{ \"clientId\":\"",
					                                AclientId,
					                                "\",\"clientSecret\":\"",
					                                AclientSecret,
					                                "\",\"status\":\"",
					                                strFileContent,
					                                "\" }"
				                                });
                                            //this.writeLog(this.strFilePath, strJson);
                                            //log.WriteLog("http start json:" + strHttpResult, "LuBangTong");
                                            strHttpResult = WebHttpHelp.HttpPost(AStatusURL, strJson);
                                            //log.WriteLog("http end", "LuBangTong");
                                            strHttpResult = strHttpResult.Replace("\"{", "{").Replace("\\\"", "\"").Replace("}\"", "}");
                                            //this.writeLog(this.strFilePath, "uploadStatus返回结果:" + strHttpResult);
                                            //log.WriteLog("http end result:" + strHttpResult, "LuBangTong");
                                            _cStatus = JsonConvert.DeserializeObject<JsonConverStatus>(strHttpResult);
                                            if (bool.Parse(_cStatus.success))
                                            {
                                                //MessageBox.Show("上传成功!");
                                                //this.writeLog(this.strFilePath, "uploadStatus:上传成功!");
                                                //log.WriteLog(_cStatus.message + "   uploadStatus:success!", "LuBangTong");
                                                AppLogHelp.WriteLog(LogFileFormate.AppMain, "LuBangTong  " + _cStatus.message + "   uploadStatus:success!");
                                                File.WriteAllText(strStartAndReset, "0", Encoding.Default); // modity by peng 20190108
                                                //File.WriteAllText(strErrorFilePath, "0", Encoding.Default);
                                            }
                                            else
                                            {
                                                //MessageBox.Show("上传失败!:" + cStatus.message);
                                                //this.writeLog(this.strFilePath, "uploadStatus:上传失败!:" + cStatus.message);
                                                //log.WriteLog(_cStatus.message + "uploadStatus:fail!", "LuBangTong");
                                                AppLogHelp.WriteLog(LogFileFormate.AppMain, "LuBangTong  " + _cStatus.message + "   uploadStatus:fail!");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            //MessageBox.Show(ex.Message);
                                            //this.writeLog(this.strFilePath, "uploadStatus:上传异常!:" + ex.Message);
                                            //log.WriteLog(ex.Message + "uploadStatus:exception!", "LuBangTong");
                                            AppLogHelp.WriteLog(LogFileFormate.AppMain, "LuBangTong  " + _cStatus.message + "   uploadStatus:exception!");
                                        }
                                    }
                                }
                                
                                
                            }
                            //软件点开始
                            else if (strStart == "True" && strRun == "True")
                            {
                                strFileContent = arrStatus[3];

                            }
                            else if (strStop == "True")
                            {
                                strFileContent = arrStatus[0];
                            }
                            else if (strIdel == "Ture")
                            {
                                strFileContent = arrStatus[2];
                            }
                            else
                            {
                                strFileContent = arrStatus[3];
                            }

                            
                            if (File.Exists(strStartAndReset))
                            {
                                 iFileNum = int.Parse(File.ReadAllText(strStartAndReset, Encoding.Default)) ;
                                 if (iFileNum >= 300)
                                 {
                                     try
                                     {
                                         strJson = string.Concat(new string[]
				                                {
					                                "{ \"clientId\":\"",
					                                AclientId,
					                                "\",\"clientSecret\":\"",
					                                AclientSecret,
					                                "\",\"status\":\"",
					                                strFileContent,
					                                "\" }"
				                                });
                                         //this.writeLog(this.strFilePath, strJson);
                                         //log.WriteLog("http start json:" + strHttpResult, "LuBangTong");
                                         strHttpResult = WebHttpHelp.HttpPost(AStatusURL, strJson);
                                         //log.WriteLog("http end", "LuBangTong");
                                         strHttpResult = strHttpResult.Replace("\"{", "{").Replace("\\\"", "\"").Replace("}\"", "}");
                                         //this.writeLog(this.strFilePath, "uploadStatus返回结果:" + strHttpResult);
                                         //log.WriteLog("http end result:" + strHttpResult, "LuBangTong");
                                         _cStatus = JsonConvert.DeserializeObject<JsonConverStatus>(strHttpResult);
                                         if (bool.Parse(_cStatus.success))
                                         {
                                             //MessageBox.Show("上传成功!");
                                             //this.writeLog(this.strFilePath, "uploadStatus:上传成功!");
                                             //log.WriteLog(_cStatus.message + "   uploadStatus:success!", "LuBangTong");
                                             AppLogHelp.WriteLog(LogFileFormate.AppMain, "LuBangTong" + "uploadStatus:success!" );
                                             File.WriteAllText(strStartAndReset, "0", Encoding.Default); // modity by peng 20190108
                                             File.WriteAllText(strErrorFilePath, "0", Encoding.Default);
                                         }
                                         else
                                         {
                                             //MessageBox.Show("上传失败!:" + cStatus.message);
                                             //this.writeLog(this.strFilePath, "uploadStatus:上传失败!:" + cStatus.message);
                                             //log.WriteLog(_cStatus.message + "uploadStatus:fail!", "LuBangTong");
                                             AppLogHelp.WriteError(LogFileFormate.AppMain, "LuBangTong" + "uploadStatus:fail!");
                                         }
                                     }
                                     catch (Exception ex)
                                     {
                                         //MessageBox.Show(ex.Message);
                                         //this.writeLog(this.strFilePath, "uploadStatus:上传异常!:" + ex.Message);
                                         //log.WriteLog(ex.Message + "uploadStatus:exception!", "LuBangTong");
                                         AppLogHelp.WriteError(LogFileFormate.AppMain, "LuBangTong" + "uploadStatus:exception" + ex.Message);
                                     }
                                 }
                                 else
                                 {
                                     File.WriteAllText(strStartAndReset, iFrequency + iFileNum + "", Encoding.Default); // modity by peng 20190108
                                 }
                            }

                            
                            strFileContent = string.Empty;
                        }
                        ClearMemory();
                        System.Threading.Thread.Sleep(100);
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(5000);
                    }
                    strSql = string.Empty; strSqlForPCBInfo = string.Empty;
                }

            }
            catch (Exception exx)
            {
                //log.WriteErr("错误[]"+exx.Message,"LuBangTong");
                AppLogHelp.WriteError(LogFileFormate.AppMain, "LuBangTong" + exx.ToString());
            }
            finally
            {

            }


        }
        public void StartLuBangTongPRD(string AtbUSERNO,
                            string AtbMO,
                            string AtbITEMVALUE,
                            string AtbMachineNo,
                            string AtbOn,
                            string AtbSf,
                            string AclientId, string AclientSecret, string AStatusURL, string AsErrorUrl)
        {
            try
            {
                runnLuBangTongPRD = new ThreadProcessLuBangTongPRD(AtbUSERNO,
                             AtbMO,
                             AtbITEMVALUE,
                             AtbMachineNo,
                             AtbOn,
                             AtbSf,
                             AclientId, AclientSecret, AStatusURL, AsErrorUrl);
                runnLuBangTongPRD.Run();
            }
            catch (Exception ex)
            {
                //log.WriteErr(ex.Message, "LuBangTong");
                AppLogHelp.WriteError(LogFileFormate.MES, "LuBangTong" + ex.ToString());
            }
        }

        public void SaveDataLuBangTongPRD(string AtbUSERNO,
                            string AtbMO,
                            string AtbITEMVALUE,
                            string AtbMachineNo,
                            string AtbOn,
                            string AtbSf,
                            string AclientId, string AclientSecret, string AStatusURL, string AsErrorUrl)
        {
            try
            {
                _strLogPengPath = Path.Combine(_strLogPengPath, "LuBangTong");
                string strLogDataPath = Path.Combine(_strLogPengPath, "RenBaoToData");
                if (Directory.Exists(_strLogPengPath) == false)
                {
                    Directory.CreateDirectory(_strLogPengPath);
                }
                if (Directory.Exists(strLogDataPath) == false)
                {
                    Directory.CreateDirectory(strLogDataPath);
                }
                DateTime dtNow = DateTime.Now;

                string strStart = string.Empty, strError = string.Empty,
                                strStop = string.Empty, strRun = string.Empty;
                string strTime = string.Empty;
                string strTime2 = string.Empty;
                string strFileNameTime = string.Empty;
                string strLineName = string.Empty;
                string strMachine = string.Empty;
                string strErrorContent = string.Empty;

                string strFactory = string.Empty;
                string strCustor = string.Empty;
                string strFool = string.Empty;
                string strEquipName = string.Empty;
                string strSide = string.Empty;
                //string strEquipID = dataTable.Rows[i][17].ToString();
                string strModule = string.Empty;
                string strErrorContentEnglish = string.Empty;
                //GetErrorCodeForStrEquipStatus(strErrorContent, ref strErrorContentEnglish);
                string strFile = string.Empty;//Path.Combine(_strCsvPath, "Status_SPI_" + strFileNameTime);
                string strOpComFirmed = string.Empty;
                string strOpComFirmedTime = string.Empty;
                string strFileContent = string.Empty;
                Random rand = new Random();
                string strAutoAPPIniFile = @"D:\EYSPI\Bin\Config\autoApp.ini";
                string arraybarcode1 = "", arraybarcode2 = "", strarrayResult1 = "", strarrayResult2 = "";
                
                //DateTime dtTMP = dtNow;
                string strSql = "", strSqlForPCBInfo = "", strJson = string.Empty, strHttpResult = string.Empty;

                System.Data.DataTable dataTable = null; System.Data.DataTable dataTablePCB = null; DataTable dtArrayResult = null, dtPadTable=null, dtStatusPCB = null, dtArrayPCB = null;
                string strJobName = "", strBarcode = "", strResult = "";
                DateTime ttDateTime, dtEndTime, dtStartTime;
                string strArraySql = string.Empty, strResultArraySql = string.Empty, strStatusSql = string.Empty;
                ttDateTime = DateTime.Now;
                //test
                //ttDateTime = Convert.ToDateTime("2017-03-17 21:59:26");
                int iTmp = 0;
                string strResultArraySqlSec = "SELECT COUNT(PCBID) FROM spidb.TBPadMeasure as pad WHERE pad.PCBID = '{0}' AND pad.ArrayIDIndex ='{1}' AND pad.JudgeRes = '1'";
                string strSqlForPCBInfoSec = "SELECT " +
                   " pcb.PCBBarcode, " +
                   " pcb.Result, " +
                   " pcb.Operator, " +
                   " pcb.PCBID, " +
                   " pcb.StartTime,  " +
                   " pcb.EndTime, " +
                   " job.JobName,pcb.OPConfirmed,pcb.OPConfirmTime " +
                   " FROM " +
                   "    spidb.tbboard AS pcb " +
                   "    INNER JOIN spidb.TBJobInfo job ON pcb.JobIndex = job.SerNo   " +
                   "WHERE " +
                    //"    pcb.StartTime >= '{0}' " +
                   " pcb.StartTime > '{0}' ";
                string strArratySqlSec = " SELECT " +
                            "   bc.ArrayBarCode, " +
                            " bc.ArrayID  " +
                           " FROM " +
                           "    spidb.TBBarCode as bc " +
                           " WHERE " +
                           "  bc.PCBID = '{0}' AND " +
                           "  bc.BarCode = '{1}'";
                string strSqlPadInfoSec = " SELECT  " +
                                            "pad.ABSHeight," +
                                            "pad.ABSArea," +
                                            "pad.ABSVolume," +
                                            "pad.JudgeRes," +
                                            "pad.DefectType," +
                                            " pad.PadID " +
                                            " FROM  " +
                                            " spidb.TBPadMeasure pad " +
                                        " WHERE " +
                                        " pad.PCBID = '{0}'";//And padm.JobIndex = " + jobIndex;
                string strSqlPadInfo = string.Empty;
                string PASS = "pass",NG = "ng";
                //MessageBox.Show("生产信息__start");
                while (true)
                {
                    strSqlForPCBInfo = string.Format(strSqlForPCBInfoSec, ttDateTime);
                    
                    dataTablePCB = getDataTableForZZFox(_strConnectString, strSqlForPCBInfo);
                    if (dataTablePCB != null && dataTablePCB.Rows.Count > 0)
                    {
                        //wait 10s 人员判定
                        System.Threading.Thread.Sleep(5000);
                        dataTablePCB = getDataTableForZZFox(_strConnectString, strSqlForPCBInfo);
                        for (int iBarIndex = 0; iBarIndex < dataTablePCB.Rows.Count; iBarIndex++)
                        {
                            strBarcode = dataTablePCB.Rows[iBarIndex][0].ToString();
                            //MessageBox.Show("条码:" + strBarcode);
                            //strJobName = dataTablePCB.Rows[iBarIndex][1].ToString();
                            strResult = dataTablePCB.Rows[iBarIndex][1].ToString();
                            string strOperator = dataTablePCB.Rows[iBarIndex][2].ToString();
                            //strOP = strOperator;
                            string strPCBID = dataTablePCB.Rows[iBarIndex][3].ToString();   //pcbid
                            //string strPCBstartTime = dataTablePCB.Rows[iBarIndex][4].ToString();   //startTime
                            //string strPCBFilestartTime = ((DateTime)dataTablePCB.Rows[iBarIndex][5]).ToString("yyyyMMddHHmmss");
                            dtEndTime = (DateTime)(dataTablePCB.Rows[iBarIndex][5]);
                            dtStartTime = (DateTime)(dataTablePCB.Rows[iBarIndex][4]);
                            strJobName = dataTablePCB.Rows[iBarIndex][6].ToString();
                            strOpComFirmed = dataTablePCB.Rows[iBarIndex][7].ToString();
                            strOpComFirmedTime = dtStartTime.ToString(RS_FORMAT_DATETIME);//((DateTime)dataTablePCB.Rows[iBarIndex][8]).ToString(RS_FORMAT_DATETIME);
                            if (string.IsNullOrEmpty(strBarcode))
                            {
                                strBarcode = "noread_" + dtStartTime.ToString(RS_Format_DateTimeFileName);
                                //continue;
                            }
                            //MessageBox.Show("时间:" + strOpComFirmedTime);
                            ttDateTime = dtStartTime;
                            if (strResult == "0" || strResult == "2")
                            {
                                strResult = PASS;
                            }
                            else
                            {
                                strResult = NG;
                            }
                            //get the array
                            strArraySql = string.Format(strArratySqlSec, strPCBID, strBarcode);
                            
                            dtArrayPCB = getDataTableForZZFox(_strConnectString, strArraySql);
                            // PCB 数据
                            MdArrayPcbInfo[] mdArrayPcbInfos=null;
                            if (dtArrayPCB != null && dtArrayPCB.Rows.Count > 0)
                            {
                                mdArrayPcbInfos = new MdArrayPcbInfo[dtArrayPCB.Rows.Count];
                                //MessageBox.Show("拼板个数:" + dtArrayPCB.Rows.Count);
                                if (dtArrayPCB.Rows.Count > 1)
                                {
                                    for (int n = 0; n < dtArrayPCB.Rows.Count; n++)
                                    {
                                        string arrbarcode = dtArrayPCB.Rows[n][0].ToString();
                                        if (string.IsNullOrEmpty(arrbarcode) || string.Equals("noread", arrbarcode, StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            arrbarcode = strBarcode;
                                            //空条码不做处理
                                            //continue;
                                        }
                                        //if (string.IsNullOrEmpty(arrbarcode)) { arrbarcode = strBarcode; }
                                        string strarrayID = dtArrayPCB.Rows[n][1].ToString();
                                        string strarrayResult = "true";
                                        strResultArraySql = string.Format(strResultArraySqlSec, strPCBID, strarrayID);
                                        dtArrayResult = getDataTableForZZFox(_strConnectString, strResultArraySql);
                                        if (dtArrayResult != null && dtArrayResult.Rows.Count > 0)
                                        {
                                            if (dtArrayResult.Rows[0][0].ToString() != "0")
                                            {
                                                strarrayResult = PASS;
                                            }
                                            else
                                            {
                                                strarrayResult = NG;
                                            }
                                        }
                                        MdArrayPcbInfo mdArrayPcbInfo = new MdArrayPcbInfo();
                                        mdArrayPcbInfo.arraybarcode = arrbarcode;
                                        mdArrayPcbInfo.arrayResult = strarrayResult;
                                        mdArrayPcbInfos[n] = mdArrayPcbInfo;
                                        
                                    }
                                }
                                else
                                {
                                    MdArrayPcbInfo mdArrayPcbInfo = new MdArrayPcbInfo();
                                    mdArrayPcbInfo.arraybarcode = strBarcode;
                                    mdArrayPcbInfo.arrayResult = strResult;
                                    mdArrayPcbInfos[0] = mdArrayPcbInfo;
                                }
                            }
                            else
                            {
                                mdArrayPcbInfos = new MdArrayPcbInfo[1];
                                MdArrayPcbInfo mdArrayPcbInfo = new MdArrayPcbInfo();
                                mdArrayPcbInfo.arraybarcode = strBarcode;
                                mdArrayPcbInfo.arrayResult = strResult;
                                mdArrayPcbInfos[0] = mdArrayPcbInfo;
                            }
                            strSqlPadInfo = string.Format(strSqlPadInfoSec, strPCBID);
                            dtPadTable = getDataTableForZZFox(_strConnectString, strSqlPadInfo);
                            strErrorContent = string.Empty;
                            //pad 数据
                            MdPadInfo[] MdPadInfos = null;
                            if (dtPadTable != null && dtPadTable.Rows.Count > 0)
                            {
                                MdPadInfos= new MdPadInfo[dtPadTable.Rows.Count];
                                for (int n = 0; n < dtPadTable.Rows.Count; n++)
                                {
                                    MdPadInfo MdPadInfo = new MdPadInfo();
                                    MdPadInfo.barcode = strBarcode;
                                    MdPadInfo.height = dtPadTable.Rows[n][0].ToString();
                                    MdPadInfo.area = dtPadTable.Rows[n][1].ToString();
                                    MdPadInfo.vol = dtPadTable.Rows[n][2].ToString();
                                    MdPadInfo.padResult = dtPadTable.Rows[n][3].ToString() == "1" ? NG : PASS;
                                    MdPadInfo.defaultType = dtPadTable.Rows[n][4].ToString();
                                    MdPadInfo.padId = dtPadTable.Rows[n][5].ToString();
                                    MdPadInfos[n] = MdPadInfo;
                                    if (MdPadInfo.padResult == "1")
                                    {
                                        strErrorContent = MdPadInfo.defaultType;
                                    }
                                }
                            }
                            else
                            {
                                 MdPadInfos = new MdPadInfo[0];
                                MdPadInfo MdPadInfo = new MdPadInfo();
                                MdPadInfos[0] = MdPadInfo;
                            }

                            MdInfomation MdPCBInfo = new AutoAPP.MdInfomation();
                            MdPCBInfo.mdArrayPcbInfos = mdArrayPcbInfos;
                            MdPCBInfo.mdPadInfos = MdPadInfos;
                            MdPCBInfo.endTime = dtEndTime.ToString(RS_FORMAT_DATETIME);
                            MdPCBInfo.startTime = dtStartTime.ToString(RS_FORMAT_DATETIME);
                            MdPCBInfo.errorCode = strErrorContent;
                            MdPCBInfo.jobName = strJobName;
                            MdPCBInfo.opConfirmed = strOpComFirmed;
                            MdPCBInfo.opConfirmedTime = strOpComFirmedTime;
                            MdPCBInfo.user = strOperator;
                            MdPCBInfo.totalSec = (dtEndTime - dtStartTime).TotalSeconds+"";
                            SaveLogsInfo(dtStartTime, _strLogPengPath, MdPCBInfo.endTime, strLineName, "SPI");
                            strJson = JsonConvert.SerializeObject(new JsonConverPRDData
                            {
                                clientId = AclientId,
                                clientSecret = AclientSecret,
                                at = AutoAPP.MainForm._strToken,
                                on = AtbOn,
                                sn = strBarcode,
                                sf = AtbSf,
                                //error = strErrorContent,
                                //msg = strErrorContentEnglish,
                                result = strResult,
                                md = MdPCBInfo
                            });
                            SaveLogsInfo(dtStartTime, _strLogPengPath, strJson, strLineName, "SPI");
                            strHttpResult = WebHttpHelp.HttpPost(AsErrorUrl, strJson);
                            //this.writeLog(this.strFilePath, "uploadError返回结果:" + strHttpResult);
                            //log.WriteLog("http start","LuBangTong");
                            strHttpResult = strHttpResult.Replace("\"{", "{").Replace("\\\"", "\"").Replace("}\"", "}");
                            //log.WriteLog("http end", "LuBangTong");
                            _cStatus = JsonConvert.DeserializeObject<JsonConverStatus>(strHttpResult);
                            if (bool.Parse(_cStatus.success))
                            {
                                //log.WriteLog(_cStatus.message + ";uploadPRD success!", "LuBangTong");
                            }
                            else
                            {
                                //log.WriteLog(_cStatus.message + ";uploadPRD fail!", "LuBangTong");
                            }
                            MdPadInfos = null; mdArrayPcbInfos = null; MdPCBInfo = null;
                           
                        }
                        ClearMemory();
                        System.Threading.Thread.Sleep(100);
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(5000);
                    }
                    
                    strJobName = string.Empty; strBarcode = string.Empty; strResult = string.Empty; 
                }
            }
            catch (Exception exx)
            {
                //log.WriteErr("错误[]" + exx.Message, "LuBangTong");
            }
            finally
            {
            }

        }
        #endregion

        #region "南昌华勤"
        System.Threading.Thread _HqThread;
        System.Threading.Thread _HqPcbThread;
        public void startHuaQinByNanChangRun( InspectConfig.ConfigData Aconfig)
        {
            
            
            this._configData = Aconfig;
            try
            {
                _HqThread = new System.Threading.Thread(new System.Threading.ThreadStart(CountThreadProcHuaQin));
                _HqThread.Priority = System.Threading.ThreadPriority.Lowest;
                _HqThread.IsBackground = true;
                _HqThread.Start();

                _HqPcbThread = new System.Threading.Thread(new System.Threading.ThreadStart(CountThreadProcHqPcb));
                _HqPcbThread.Priority = System.Threading.ThreadPriority.Lowest;
                _HqPcbThread.IsBackground = true;
                _HqPcbThread.Start();
            }
            catch (Exception ex)
            {
                //log.WriteErr("错误!" + ex.Message, "华勤");
            }
        }
        private void CountThreadProcHuaQin()
        {

            
                AppLayerLib.AppSettingHandler appSettingHandle = new AppLayerLib.AppSettingHandler();
                DateTime dtNow = DateTime.Now;
                string strStart = string.Empty, strError = string.Empty,
                                strStop = string.Empty, strRun = string.Empty, strIdle=string.Empty;
                string strTime = string.Empty;
                
                string strFileNameTime = string.Empty;
                string strLineName = string.Empty;
                string strMachine = string.Empty;
                string strAlarm = string.Empty; 
                //test
                //dtNow = Convert.ToDateTime("2019-01-03 16:40:33");
                DateTime dtTMP = dtNow;
                string strSql = "";// strSqlForPCBInfo = "";
                string strSqlSec = "SELECT " +
	                                "stu.Line,"+
	                                "stu.EquipID,"+
	                                "sum(stu.`Stop`) ,"+
	                                "sum(stu.Error),"+
                                    "stu.`Start`,"+
	                                "stu.`Stop` ,"+
	                                "stu.Error, "+
	                                "stu.`Run`, "+
	                                "stu.UpdateTime,"+
                                    "stu.`TowerR`,   " +
                                    "stu.`Idle`  " +
                                "FROM "+
	                                "spidb.tbequipstatus stu "+
                                "WHERE "+
                                    "stu.UpdateTime > '{0}'  and  stu.UpdateTime <= '{1}'  " +
                                "ORDER BY "+
	                                "stu.UpdateTime " ;
                DataTable dataTable = null;
                string strWebType = "type=20&action=SPIMachine&station={0}&machineID={1}&"+
                    "status={2}&runtime={3}&faulttime={4}&"+
                    "loadingtime={5}&stoptime={6}&stoptimes={7}&faulttimes={8}&"+
                    "voltage={9}&current={10}&powerconsum={11}&totalpower={12}";
                string strWebTypeSec = string.Empty;
                string statu = string.Empty, runtime = string.Empty, errorTime = string.Empty,
                    loadingtime = string.Empty, stoptime = string.Empty,
                    voltage = string.Empty, current = string.Empty, powerconsum = string.Empty,totalpower = string.Empty;
                int stoptimes =0,faulttimes=0;
                string path = @"D:\EYSPI\Bin\AutoAPPConfig\";
                DirectoryInfo info = new DirectoryInfo(path);
                FileInfo newestFile = info.GetFiles("*.bin").OrderBy(n => n.LastWriteTime).Last();
                if (newestFile.Exists)
                {
                    System.Threading.Thread.Sleep(300);
                    while (_baseFuc.IsFileInUse(newestFile.FullName)) //在使用
                    {
                        System.Threading.Thread.Sleep(100);
                    };
                    appSettingHandle.Read(newestFile.FullName, _configData);
                }
                else
                {
                    //_log.WriteErr("UI=>" + newestFile + "IS_NOT_EXIST");
                    AppLogHelp.WriteError(LogFileFormate.MES, "UI=>" + newestFile + "IS_NOT_EXIST");
                    return;
                }

                while (true)
                {
                    try
                    {
                        string strWebURL = appSettingHandle._appSettingData.stDataExpVT.strWebAddress;
                        //DBdata获取 设备信息
                        strSql = string.Format(strSqlSec, dtTMP.AddMinutes(-5), dtTMP);
                        // strSql
                        dataTable = getDataTableForZZFox(_strConnectString, strSql);
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            for (int i = 0; i < dataTable.Rows.Count; i++)
                            {
                                if (string.IsNullOrEmpty(dataTable.Rows[i][8].ToString()))
                                {
                                    dtTMP = DateTime.Now;
                                    System.Threading.Thread.Sleep(60000 * 5);
                                    dtTMP = dtTMP.AddMinutes(5);//
                                    continue;
                                }
                                strLineName = dataTable.Rows[i][0].ToString();
                                strMachine = dataTable.Rows[i][1].ToString();
                                DateTime dtTMPInfo = (DateTime)(dataTable.Rows[i][8]);
                                stoptimes = int.Parse(dataTable.Rows[i][2].ToString());
                                faulttimes = int.Parse(dataTable.Rows[i][3].ToString());

                                strStart = dataTable.Rows[i][4].ToString();
                                strRun = dataTable.Rows[i][7].ToString();
                                strStop = dataTable.Rows[i][5].ToString();
                                strAlarm = dataTable.Rows[i][9].ToString();
                                strIdle = dataTable.Rows[i][10].ToString();

                                strError = dataTable.Rows[i][6].ToString();

                                //error
                                //run or stop or alarm or fault
                                if (strError == "True")
                                {
                                    errorTime = dtTMPInfo.ToString(RS_FORMAT_DATETIME_SM);
                                    statu = "fault";
                                }
                                //软件点开始
                                else if (strStart == "True" && strRun == "True")
                                {
                                    runtime = dtTMPInfo.ToString(RS_FORMAT_DATETIME_SM);
                                    statu = "run";
                                }
                                else if (strAlarm == "True")
                                {
                                    runtime = dtTMPInfo.ToString(RS_FORMAT_DATETIME_SM);
                                    statu = "alarm";
                                }
                                else if (strStop == "True")
                                {
                                    stoptime = dtTMPInfo.ToString(RS_FORMAT_DATETIME_SM);
                                    statu = "stop";
                                }
                                else if (strIdle == "True" && strStart == "True")
                                {
                                    runtime = dtTMPInfo.ToString(RS_FORMAT_DATETIME_SM);
                                    statu = "run";
                                }
                                else
                                {
                                    stoptime = dtTMPInfo.ToString(RS_FORMAT_DATETIME_SM);
                                    statu = "stop";
                                }
                                strWebTypeSec = string.Format(strWebType, strLineName, strMachine, statu,
                                    runtime, errorTime, loadingtime, stoptime,
                                    stoptimes, faulttimes,
                                    voltage, current, powerconsum, totalpower
                                    );
                                dtTMP = dtTMP.AddMinutes(5);
                                string strMsg = WebHttpHelp.HttpGet(strWebURL, strWebTypeSec);
                                if (string.IsNullOrEmpty(strMsg) == false && strMsg.Length > 0 && strMsg.Substring(0, 1) == "1")
                                {
                                    //_log.WriteLog("上抛成功:" + dtTMPInfo.ToString(RS_FORMAT_DATETIME_SM) + "   statu:" + statu + "   " + strMsg, "ThreadName:HuaQin");
                                    AppLogHelp.WriteError(LogFileFormate.MES, "ThreadName:HuaQin上抛成功:" + dtTMPInfo.ToString(RS_FORMAT_DATETIME_SM) + "   statu:" + statu + "   " + strMsg);
                                    strMsg = string.Empty;
                                    System.Threading.Thread.Sleep(60000 * 5);
                                }
                                else
                                {
                                   // _log.WriteErr("上抛错误:" + dtTMPInfo.ToString(RS_FORMAT_DATETIME_SM) + "   statu:" + statu + "   " + strMsg, "ThreadName:HuaQin");
                                    AppLogHelp.WriteError(LogFileFormate.MES, dtTMPInfo.ToString(RS_FORMAT_DATETIME_SM) + "ThreadName:HuaQin   statu:" + statu + "   " + strMsg);
                                    System.Threading.Thread.Sleep(60000 * 5);
                                }
                                //break;
                                ClearMemory();
                            }
                            System.Threading.Thread.Sleep(50);
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(300);
                        }
                        //dtTMP.AddMinutes(5);
                        //strSql = string.Empty; strSqlForPCBInfo = string.Empty;
                    }
                    catch (Exception e)
                    {
                        //_log.WriteErr("错误:" + e.Message, "ThreadName:HuaQin");
                        AppLogHelp.WriteError(LogFileFormate.MES,"ThreadName:HuaQin"+ e.Message);
                                   
                        continue;
                    }
                }
                
            
        }
        private void CountThreadProcHqPcb()
        {

            
                //本地读取存储上一片pcb的ID号；
                string strLastPcbPath = @"D:\EYSPI\Bin\AutoAPPConfig\PCB_TMP";
                if (!Directory.Exists(strLastPcbPath))
                {
                    Directory.CreateDirectory(strLastPcbPath);
                }
                //本地记录的appsetting 的文件名;
                string strLastAppsettingPath = @"D:\EYSPI\Bin\AutoAPPConfig\APPsetting_TMP";
                if (!Directory.Exists(strLastAppsettingPath))
                {
                    Directory.CreateDirectory(strLastAppsettingPath);
                }
                //string sfrPcbID = "0";
                //若存在则读取此文件内容去读取；
                string strLastPcbFile = strLastPcbPath + "\\" + "pcbBufer.txt";
                //appsetting 的临时文件
                string strLastAppsettingTmpFile = strLastAppsettingPath + "\\" + "appsetPcbID.txt";
                string strAppsettingPath = @"D:\EYSPI\Bin\AutoAPPConfig";
                
                //实例化appSettingHandler
                AppLayerLib.AppSettingHandler appSettingHandle = new AppLayerLib.AppSettingHandler();
                //用来记录appseting文件名的临时文件; (PCBID)
                string path = strAppsettingPath;
                DirectoryInfo info = new DirectoryInfo(path);
                //var files = Directory.GetFiles(path, "*.bin");
                //int max = int.MinValue, min = int.MaxValue;
                //foreach (var file in files)
                //{
                //    if (!string.IsNullOrEmpty(file))
                //    {
                //        var vv = Path.GetFileNameWithoutExtension(file);
                //        System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"^\d+$");
                //        if (re.IsMatch(vv))
                //        {
                //            int value = int.Parse(vv);
                //            if (value < min)
                //                min = value;
                //            if (value > max)
                //                max = value;
                //        }
                //    }
                //}
                

                string nextPcbId = string.Empty;
                //用来记录上一块PCB的临时文件; ;
                FileInfo newestFile = info.GetFiles("*.bin").OrderBy(n => n.LastWriteTime).Last();
                int value = 0;
                if (newestFile.Exists)
                {
                    System.Threading.Thread.Sleep(200);
                    if (_baseFuc.IsFileInUse(newestFile.FullName)) //在使用
                    {
                        System.Threading.Thread.Sleep(1000);
                    };
                    var vv = Path.GetFileNameWithoutExtension(newestFile.FullName);
                    System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"^\d+$");
                    if (re.IsMatch(vv))
                    {
                        value = int.Parse(vv);

                    }
                }
                if (File.Exists(strLastPcbFile))
                {
                    File.WriteAllText(strLastPcbFile, (value-1) + "", Encoding.Default);
                    //_log.WriteLog(value+"","value");
                }
                else
                {
                    //如果不存在PCBfile 则说明此时系统第一次开启用，则用appsetting中pcbID; 
                    File.WriteAllText(strLastPcbFile, (value-2) + "", Encoding.Default);
                }
                string strMaxPcbSql = "SELECT MAX(pcb.PCBID) FROM spidb.TBBoard pcb";
                DataTable dtMaxPcb = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, strMaxPcbSql);
                if (dtMaxPcb != null && dtMaxPcb.Rows.Count > 0)
                {
                    if (dtMaxPcb.Rows[0][0].ToString() != "0")
                    {
                        File.WriteAllText(strLastPcbFile, dtMaxPcb.Rows[0][0].ToString(), Encoding.Default);
                    }
                }
                //AppLayerLib.AppSettingHandler appSettingHandle = new AppLayerLib.AppSettingHandler();
                //string path = @"D:\EYSPI\Bin\AutoAPPConfig\";
                
                string strWebType = "type=20&action=SPIComplete&sn={0}&station={1}&uid={2}&pwd={3}&CODE={4}&snlist={5}";
                string strWebTypeSEC = string.Empty;
                string sqlSec = "SELECT t.PCBID,t.StartTime,t.PCBBarcode,t.JobIndex,t.Result,t.lineNo FROM spidb.TBBoard t  WHERE t.PCBID >  '{0}'" + "AND t.OPConfirmed = '1'   ORDER BY t.PCBID ASC ";
                string sql = string.Empty;
                string strArraySql = string.Empty;
                string strArraySqlSec = " SELECT " +
                                 "   bc.ArrayBarCode, " +
                                 " bc.ArrayID  " +
                                " FROM " +
                                "    spidb.TBBarCode as bc " +
                                " WHERE " +
                                "  bc.PCBID = '{0}'"; //AND " +
                                //"  bc.BarCode = '{1}'";
                string strResultArraySqlSec = "SELECT COUNT(PCBID) FROM spidb.TBPadMeasure as pad WHERE pad.PCBID = '{0}' AND pad.ArrayIDIndex ='{1}' AND pad.JudgeRes = '1'";//string.Format(strResultArraySql, strarrayID);
                string strResultArraySql = string.Empty;   
                string sqlPadInfo = string.Empty;

                string sqlPadInfoSec = " SELECT  " +
                                                            "padm.ComponentID," +
                                                            "pad.PerArea," +
                                                            "pad.ABSHeight," +
                                                            "pad.PerVolume," +
                                                            "pad.ShiftX," +
                                                            "pad.ShiftY," +
                                                            "pad.JudgeRes," +
                                                            "pad.DefectType," +
                                                            "padm.ArrayID " +
                                                            "  FROM  " +
                                                            " spidb.TBPadMeasure pad " +
                                                        " INNER JOIN spidb.TBSimplePad padm ON pad.PadID = padm.PadID AND padm.JobIndex = pad.JobIndex  " +
                                                        " WHERE " +
                                                        " pad.PCBID = '{0}'" + " And pad.ArrayIDIndex = '{1}' And pad.JudgeRes='1'  ";
               
                string sqlPadInfoNoArraySec = " SELECT  " +
                                                            "padm.ComponentID," +
                                                            "pad.PerArea," +
                                                            "pad.ABSHeight," +
                                                            "pad.PerVolume," +
                                                            "pad.ShiftX," +
                                                            "pad.ShiftY," +
                                                            "pad.JudgeRes," +
                                                            "pad.DefectType," +
                                                            "padm.ArrayID " +
                                                            "  FROM  " +
                                                            " spidb.TBPadMeasure pad " +
                                                        " INNER JOIN spidb.TBSimplePad padm ON pad.PadID = padm.PadID AND padm.JobIndex = pad.JobIndex  " +
                                                        " WHERE " +
                                                        " pad.PCBID = '{0}'"  + " And pad.JudgeRes='1'  ";
                string sqlPadInfoNoArray = string.Empty;
                string strTmpResult = "返回结果=>";
                DataTable dsqlDefult=null;
                DataTable dt=null,dtArrayPCB=null, dtArrayResult=null;
                
                while (true)
                {
                    try
                    {
                        newestFile = info.GetFiles("*.bin").OrderBy(n => n.LastWriteTime).Last();
                        if (newestFile.Exists)
                        {
                            System.Threading.Thread.Sleep(100);
                            while (_baseFuc.IsFileInUse(newestFile.FullName)) //在使用
                            {
                                System.Threading.Thread.Sleep(100);
                            };
                            appSettingHandle.Read(newestFile.FullName, _configData);
                        }
                        else
                        {
                            //_log.WriteErr("UI=>" + newestFile + "IS_NOT_EXIST");
                            return;
                        }
                        string nextPcb = string.Empty;
                        if (File.Exists(strLastPcbFile))
                        {
                            using (StreamReader srMain = new StreamReader(strLastPcbFile))
                            {
                                string str = "";
                                if ((str = srMain.ReadLine()) != null)
                                {
                                    nextPcb = str;
                                }
                            }
                        }
                        //_log.WriteLog(nextPcb + "", "nextPcb:");
                        string strWebURL = appSettingHandle._appSettingData.stDataExpVT.strWebAddress;
                        sql = string.Format(sqlSec, nextPcb);
                        dt = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            //wait the review station confirm
                            System.Threading.Thread.Sleep(5000);
                            dt = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, sql);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i][0] != null)
                                {
                                    string jobIndex = dt.Rows[i][3].ToString();
                                    string resultPCB = dt.Rows[i][4].ToString();
                                    if (resultPCB == "1") resultPCB = "FAIL";
                                    else if (resultPCB == "0") resultPCB = "PASS";
                                    else if (resultPCB == "2") resultPCB = "PASS";
                                    else resultPCB = "PASS";
                                    //_log.WriteLog(sql + "", "resultPCB:");
                                    string lineName = dt.Rows[i][5].ToString();
                                    nextPcb = dt.Rows[i][0].ToString();
                                    //_log.WriteLog(resultPCB + "", "resultPCB:");
                                    //_log.WriteLog(nextPcb + "", "nextPcb:");
                                    string strBarcode = dt.Rows[i][2].ToString();
                                    if (string.IsNullOrEmpty(strBarcode) || string.Equals("noread", strBarcode, StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        strBarcode = "NOREAD";
                                    }
                                    string snList = string.Empty; string code = string.Empty;
                                    if (resultPCB == "FAIL")
                                    {
                                        //get the array
                                        strArraySql = string.Format(strArraySqlSec, nextPcb);

                                        //string strErrorMessage = "";
                                        dtArrayPCB = getDataTableForZZFox(_strConnectString, strArraySql);
                                        //_log.WriteLog(strArraySql + "", "arrayPCB Count");
                                        if (dtArrayPCB != null && dtArrayPCB.Rows.Count > 0)
                                        {
                                            //_log.WriteErr(dtArrayPCB.Rows.Count + "", "拼板数量arrayPCB Count");
                                            if (dtArrayPCB.Rows.Count > 1)
                                            {
                                                //_log.WriteErr(dtArrayPCB.Rows.Count+"","拼板数量arrayPCB Count");
                                                for (int n = 0; n < dtArrayPCB.Rows.Count; n++)
                                                {
                                                    string arrbarcode = dtArrayPCB.Rows[n][0].ToString();
                                                    string strarrayID = dtArrayPCB.Rows[n][1].ToString();
                                                    if (string.IsNullOrEmpty(arrbarcode) || string.Equals("noread", arrbarcode, StringComparison.CurrentCultureIgnoreCase))
                                                    {
                                                        arrbarcode = strBarcode + "_" + strarrayID;
                                                    }
                                                    //if (string.IsNullOrEmpty(arrbarcode)) { arrbarcode = strBarcode; }
                                                    string strarrayResult = "PASS";
                                                    strResultArraySql = string.Format(strResultArraySqlSec, nextPcb, int.Parse(strarrayID)-1);
                                                    //    "SELECT COUNT(PCBID) FROM spidb.TBPadMeasure as pad WHERE pad.PCBID = '" + nextPcb + "' AND pad.ArrayIDIndex ='" + strarrayID + "' AND pad.JudgeRes = '1'";//string.Format(strResultArraySql, strarrayID);
                                                    dtArrayResult = getDataTableForZZFox(_strConnectString, strResultArraySql);
                                                    //_log.WriteLog(strResultArraySql, "strResultArraySql:");
                                                    if (dtArrayResult != null && dtArrayResult.Rows.Count > 0)
                                                    {
                                                        if (dtArrayResult.Rows[0][0].ToString() != "0")
                                                        {
                                                            strarrayResult = "FAIL";
                                                        }
                                                        else
                                                        {
                                                            strarrayResult = "PASS";
                                                        }
                                                    }
                                                    //_log.WriteLog( strarrayResult ,"arrayResult:");
                                                    sqlPadInfo = string.Format(sqlPadInfoSec, nextPcb, int.Parse(strarrayID)-1);
                                                    //_log.WriteLog(sqlPadInfo, "sqlPadInfo:");
                                                    dsqlDefult = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, sqlPadInfo);
                                                    if (dsqlDefult != null && dsqlDefult.Rows.Count > 0)
                                                    {
                                                        for (int y = 0; y < dsqlDefult.Rows.Count; y++)
                                                        {
                                                            //string code = dsqlDefult.Rows[y][7].ToString();
                                                            string ComponentID = dsqlDefult.Rows[y][0].ToString();
                                                            string PerArea = (double.Parse(dsqlDefult.Rows[y][1].ToString()) * 100).ToString("0.00");
                                                            string ABSHeight = dsqlDefult.Rows[y][2].ToString();
                                                            string PerVolume = (double.Parse(dsqlDefult.Rows[y][3].ToString()) * 100).ToString("0.00");
                                                            string ShiftX = dsqlDefult.Rows[y][4].ToString();
                                                            string ShiftY = dsqlDefult.Rows[y][5].ToString();
                                                            //string JudgeRes = dsqlDefult.Rows[y][6].ToString();
                                                            code = dsqlDefult.Rows[y][7].ToString();

                                                            snList += arrbarcode + RS_CommaSplit +
                                                                    strarrayResult + RS_CommaSplit +
                                                                    ComponentID + RS_CommaSplit +
                                                                    PerArea + RS_CommaSplit +
                                                                    ABSHeight + RS_CommaSplit +
                                                                    PerVolume + RS_CommaSplit +
                                                                    ShiftX + RS_CommaSplit +
                                                                    ShiftY + RS_SemicolonSplit;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                sqlPadInfoNoArray = string.Format(sqlPadInfoNoArraySec, nextPcb);

                                                dsqlDefult = bu_Peng.getDataTableForZZFox(CDFOX.strConnectString, sqlPadInfoNoArray);
                                                if (dsqlDefult != null && dsqlDefult.Rows.Count > 0)
                                                {
                                                    for (int y = 0; y < dsqlDefult.Rows.Count; y++)
                                                    {
                                                        //string code = dsqlDefult.Rows[y][7].ToString();
                                                        string ComponentID = dsqlDefult.Rows[y][0].ToString();
                                                        string PerArea = (double.Parse(dsqlDefult.Rows[y][1].ToString()) * 100).ToString("0.00");
                                                        string ABSHeight = dsqlDefult.Rows[y][2].ToString();
                                                        string PerVolume = (double.Parse(dsqlDefult.Rows[y][3].ToString()) * 100).ToString("0.00");
                                                        string ShiftX = dsqlDefult.Rows[y][4].ToString();
                                                        string ShiftY = dsqlDefult.Rows[y][5].ToString();
                                                        //string JudgeRes = dsqlDefult.Rows[y][6].ToString();
                                                        code = dsqlDefult.Rows[y][7].ToString();
                                                        snList += strBarcode + RS_CommaSplit +
                                                                resultPCB + RS_CommaSplit +
                                                                ComponentID + RS_CommaSplit +
                                                                PerArea + RS_CommaSplit +
                                                                ABSHeight + RS_CommaSplit +
                                                                PerVolume + RS_CommaSplit +
                                                                ShiftX + RS_CommaSplit +
                                                                ShiftY + RS_SemicolonSplit;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    switch (resultPCB)
                                    {
                                        case "PASS":
                                            {

                                                code = string.Empty;
                                                snList = strBarcode + RS_CommaSplit + resultPCB;
                                                strWebTypeSEC = string.Format(strWebType,
                                                    strBarcode, lineName,
                                                    appSettingHandle._appSettingData.strDataExpOperator,
                                                    appSettingHandle._appSettingData.stDataExpVT.strOperatorPassword,
                                                    code,
                                                    snList
                                                    );
                                                SaveLogsInfo(DateTime.Now, _strLogPengPath, strWebTypeSEC, lineName, "Sinictek");
                                                //_log.WriteLog(strWebTypeSEC, "strWebTypeSEC");
                                                strMsg = WebHttpHelp.HttpGet(strWebURL, strWebTypeSEC);
                                                SaveLogsInfo(DateTime.Now, _strLogPengPath, strTmpResult + strMsg, lineName, "Sinictek");
                                                if (string.IsNullOrEmpty(strMsg) == false && strMsg.Length > 0 && strMsg.Substring(0, 1) == "1")
                                                {
                                                    strMsg = string.Empty;
                                                }
                                                break;
                                            }
                                        case "FAIL":
                                            {
                                                strWebTypeSEC = string.Format(strWebType,
                                                    strBarcode, lineName,
                                                    appSettingHandle._appSettingData.strDataExpOperator,
                                                    appSettingHandle._appSettingData.stDataExpVT.strOperatorPassword,
                                                    code,
                                                    snList
                                                    );
                                                SaveLogsInfo(DateTime.Now, _strLogPengPath, strWebTypeSEC, lineName, "Sinictek");
                                                strMsg = WebHttpHelp.HttpGet(strWebURL, strWebTypeSEC);
                                                if (string.IsNullOrEmpty(strMsg) == false && strMsg.Length > 0 && strMsg.Substring(0, 1) == "1")
                                                {
                                                    strMsg = string.Empty;
                                                }
                                                SaveLogsInfo(DateTime.Now, _strLogPengPath, strTmpResult + strMsg, lineName, "Sinictek");
                                                break;
                                            }
                                    }
                                    //储存本次pcbID至 strLastPcbFile 以便下次读取

                                    File.WriteAllText(strLastPcbFile, nextPcb, Encoding.Default);
                                }
                            }
                            GC.Collect();
                        }
                        else
                        {

                            System.Threading.Thread.Sleep(100);
                            //break;
                        }

                    }
                    catch (Exception ex)
                    {
                        string error = "  Function: (CountThreadProcHqPcb)  数据导出出错 ";
                        //_log.WriteErr("错误 ! " + error + ex.ToString());
                        continue;
                    }
                }
        }
        #endregion
        private  readonly string RS_CommaSplit = ",";
        private  readonly string RS_SemicolonSplit = ";";
        private readonly string RS_UnderLine = "_";
        #region "设备状态default"
        private void SaveStatusFile()
        {
            try
            {
                //TmpDir

                //object[]  parm = (object[])Aparms;
                string AstrFilePath = this._strStatusExportLogPath;

                _strLogPengPath = Path.Combine(_strLogPengPath, "Status");
                //string strLogDataPath = Path.Combine(_strLogPengPath, "StatusToData");
                if (Directory.Exists(_strLogPengPath) == false)
                {
                    Directory.CreateDirectory(_strLogPengPath);
                }
               
                if(string.IsNullOrEmpty(AstrFilePath) | Directory.Exists(AstrFilePath) == false)
                {
                    AstrFilePath = Path.Combine(_strLogPengPath, "RealPath");
                    if(Directory.Exists(AstrFilePath) == false)
                    {
                        Directory.CreateDirectory(AstrFilePath);
                    }
                }
                string strRenBaoFileByStart = Path.Combine(_strLogPengPath, "renbao_start.txt");
                string strRenBaoFileByStop = Path.Combine(_strLogPengPath, "renbao_stop.txt");
                string strRenBaoFileByError = Path.Combine(_strLogPengPath, "renbao_error.txt");
                string strRenBaoFileByErrorReset = Path.Combine(_strLogPengPath, "renbao_errorReset.txt");

               
                string strTmpReset = Path.Combine(_strLogPengPath, "TmpReset.txt");
                System.Data.DataTable dataTable = null; System.Data.DataTable dataTablePCB = null; DataTable dtArrayResult = null;
                //初始化
                IniFileRenBaoReset(strRenBaoFileByStart, strRenBaoFileByStop, strRenBaoFileByError, strTmpReset, strRenBaoFileByErrorReset);
                
                DateTime dtNow = DateTime.Now;
                string strStart = string.Empty, strError = string.Empty,
                                strStop = string.Empty, strRun = string.Empty;
               
                DateTime dtFileTime = new DateTime();
                string strLineName = string.Empty;
                string strMachine = string.Empty;
                string strErrorContent = string.Empty;

                string strFactory = string.Empty;
                string strCustor = string.Empty;
                string strFool = string.Empty;
                string strEquipName = string.Empty;
                string strSide = string.Empty;
                //string strEquipID = dataTable.Rows[i][17].ToString();
                string strModule = string.Empty;
                string strErrorContentEnglish = string.Empty;
                //GetErrorCodeForStrEquipStatus(strErrorContent, ref strErrorContentEnglish);
                string strFile = string.Empty;//Path.Combine(_strCsvPath, "Status_SPI_" + strFileNameTime);
                string strOP = string.Empty;
                string strFileContent = string.Empty;
                Random rand = new Random();
                
                
                DateTime dtTMP = dtNow;
                //dtTMP = Convert.ToDateTime( "2019-01-03 16:40:35");
                string strSql = "", strSqlForPCBInfo = "", Idle = "";
                
                string strSqlSec = "SELECT " +
                                    "stu.`Start`," +
                                    "stu.`Idle`," +
                                    "stu.`Stop`," +
                                    "stu.Error," +
                                    "stu.`Run`, " +
                                    "stu.ErrContent," +
                                    "stu.UpdateTime, " +
                                    "stu.Line," +
                                    "stu.EquipID  " +
                    
                                "FROM " +
                                    "spidb.tbequipstatus stu " +
                                "WHERE " +
                                    "stu.UpdateTime >= '{0}' ORDER BY stu.UpdateTime;";
                while (true)
                {
                    if (AutoAPP.MainForm.bEnEquipStatus ==false)
                    {
                        break;
                    }
                    //DBdata获取 设备信息
                    strSql = string.Format(strSqlSec, dtTMP);
                    // strSql

                    dataTable = getDataTableForZZFox(_strConnectString, strSql);

                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        //
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            strStart = dataTable.Rows[i][0].ToString();
                            Idle = dataTable.Rows[i][1].ToString();
                            strStop = dataTable.Rows[i][2].ToString();
                            strError = dataTable.Rows[i][3].ToString();
                            strRun = dataTable.Rows[i][4].ToString();

                            dtFileTime = (DateTime)dataTable.Rows[i][6];
                            strLineName = dataTable.Rows[i][7].ToString();
                            strMachine = dataTable.Rows[i][8].ToString();
                            strErrorContent = dataTable.Rows[i][5].ToString();
                            strErrorContentEnglish = "";
                            //GetErrorCodeForStrEquipStatus(strErrorContent, ref strErrorContentEnglish);
                            strFile = "";//Path.Combine(_strCsvPath, "Status_SPI_" + strFileNameTime);
                            strOP = "";
                            strFileContent = _strRenBao_Title + _strEndLine;
                            dtTMP = (DateTime)dataTable.Rows[i][6];
                            //string strErrormessage = "";
                            //error
                            if (strError == "True")
                            {
                                if (File.ReadAllText(strTmpReset) != Em_EquipStatus.error.ToString())
                                {
                                    SaveLogsInfo(dtFileTime,AstrFilePath, Em_EquipStatus.error.ToString(), strLineName, strEquipName);
                                    File.WriteAllText(strTmpReset, Em_EquipStatus.error.ToString(), Encoding.Default);
                                }
                            }
                            //软件点开始
                            else if (strStart == "True" && strRun == "True")
                            {
                                if (File.ReadAllText(strTmpReset) != Em_EquipStatus.run.ToString())
                                {
                                    SaveLogsInfo(dtFileTime,AstrFilePath, Em_EquipStatus.run.ToString(), strLineName, strEquipName);
                                    File.WriteAllText(strTmpReset, Em_EquipStatus.run.ToString(), Encoding.Default);
                                }
                            }
                            else if (strStop == "True")
                            {
                                if (File.ReadAllText(strTmpReset) != Em_EquipStatus.stop.ToString())
                                {
                                    SaveLogsInfo(dtFileTime,AstrFilePath, Em_EquipStatus.stop.ToString(), strLineName, strEquipName);
                                    File.WriteAllText(strTmpReset, Em_EquipStatus.stop.ToString(), Encoding.Default);
                                }

                            }
                            else if (Idle == "True")
                            {
                                if (File.ReadAllText(strTmpReset) != Em_EquipStatus.idle.ToString())
                                {
                                    SaveLogsInfo(dtFileTime,AstrFilePath, Em_EquipStatus.idle.ToString(), strLineName, strEquipName);
                                    File.WriteAllText(strTmpReset, Em_EquipStatus.idle.ToString(), Encoding.Default);
                                }
                            }
                            ClearMemory();
                            strFileContent = string.Empty;
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(5000);
                        //_log.WriteLog(dtTMP+":wait Status...","RenBao");
                    }
                    strSql = string.Empty; strSqlForPCBInfo = string.Empty;
                }
            }
            catch (Exception e)
            {
                //_log.WriteErr("错误:" + e.Message, "ThreadName:RenBao");
            }

        }
        public System.Threading.Thread _StatusThread;
        private void SaveLogsInfo(DateTime dtTime, string AstrFilePath , string Avalue, string AstrLineName, string AstrEquipName)
        {
            string str = string.Empty;
            //string strFilePath = Application.StartupPath;
            if (Directory.Exists(AstrFilePath) == false)
            {
                Directory.CreateDirectory(AstrFilePath);
            }
            string strFile = Path.Combine(AstrFilePath, dtTime.ToString("yyyyMMdd") + ".log");
            string strDateTime = dtTime.ToString("yyyy/MM/dd HH:mm:ss");
            string strLog = "";
            try
            {
                if (!Directory.Exists(AstrFilePath))
                {
                    Directory.CreateDirectory(AstrFilePath);
                }
                if (!File.Exists(strFile))
                {
                    using (FileStream fs = new FileStream(strFile, FileMode.Create))
                    {
                        StreamWriter sw = new StreamWriter(fs, Encoding.Default);

                        strLog += "LineName:" + AstrLineName + "\r\nEquipName:" + AstrEquipName + "\r\n";
                        strLog += "[    " + strDateTime + "   ] :" + Avalue  + "\r\n";
                        sw.Write(strLog);
                        sw.Close();
                    }
                }
                else
                {
                    while (_baseFuc.IsFileInUse(strFile))
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    using (FileStream fs = new FileStream(strFile, FileMode.Append))
                    {
                        StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                        strLog += "[    " + strDateTime + "   ] :" + Avalue  + "\r\n";
                        sw.Write(strLog);
                        sw.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string _strStatusExportLogPath = "";
        public void startStatusRun( string AstrFilePath)
        {
            
            
            this._strStatusExportLogPath = AstrFilePath;
            AutoAPP.MainForm.bu_peng = this;
            try
            {
                //object obParam = AstrFilePath;

                _StatusThread = new System.Threading.Thread(new System.Threading.ThreadStart(SaveStatusFile));
                _StatusThread.Priority = System.Threading.ThreadPriority.Lowest;
                _StatusThread.IsBackground = true;
                _StatusThread.Start();
            }
            catch (Exception ex)
            {
                
            }
        }

        public void stopThreadStatus()
        {

            
            while (this._StatusThread != null && (this._StatusThread.ThreadState == System.Threading.ThreadState.Background
                | this._StatusThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin
                | this._StatusThread.ThreadState == System.Threading.ThreadState.Running))
            {
                System.Threading.Thread.Sleep(100);
                this._StatusThread.Abort();
                ///_log.WriteLog("进程关闭", ThreadNameStatus);
                AppLogHelp.WriteLog(LogFileFormate.EquipStatus, "进程关闭");
            }

            //_log.WriteLog("进程关闭完成", ThreadNameStatus);
        }
        #endregion

        #region "Boe"
        public System.Threading.Thread _BoeThread;
        public void startBoeRun()
        {
            
            AutoAPP.MainForm.bu_peng = this;
            try
            {
                _BoeThread = new System.Threading.Thread(new System.Threading.ThreadStart(SaveMesDataWithBoe));
                _BoeThread.Priority = System.Threading.ThreadPriority.Lowest;
                _BoeThread.IsBackground = true;
                _BoeThread.Start();
            }
            catch (Exception ex)
            {
               
            }
        }
        private void SaveMesDataWithBoe()
        {
            string strBoeCheckFile = Path.Combine(_strLogPengPath, "boe.check");
            string strBoeUploadFile = Path.Combine(_strLogPengPath, "boe.upload");
            string strBoeUrlFile = Path.Combine(_strLogPengPath,"Boe_ServiceURL.log");
            string strFmAutoappCheckFile = Path.Combine(_strLogPengPath, "FmAutoAPPboe.check");
            string strFmAutoappUploadFile = Path.Combine(_strLogPengPath, "FmAutoAPPboe.upload");
            string strUrl = @"http://110.90.119.97:7006/?singleWsdl";
        
            if (File.Exists(strBoeUrlFile))
            {
                strUrl = File.ReadAllText(strBoeUrlFile, Encoding.Default);

            }
            else
            {
                File.WriteAllText(strBoeUrlFile,strUrl, Encoding.Default);
            }
            
            string strCheckFile = "CheckFile";
            string strUploadFile = "UploadFile";
            string strContext = string.Empty;
            string strCheckObjectID = "MES_DEVICE_OFFER_PROGRAM";
            string strUploadObjectID = "ic_pqc_record_multi";
            XmlNodeList nodeList;
            XmlNodeList nodeJobFile;
            XmlNodeList nodeJobVersion;
            WebRefJDF_HEFEI.BOEService ss = new WebRefJDF_HEFEI.BOEService();
            ss.Url = strUrl;
            XmlDocument doc = new XmlDocument();
            while (true)
            {
                if (AutoAPP.MainForm.bEnDataBoe == false)
                {
                    break;
                }
                //checkFile
                if (File.Exists(strBoeCheckFile))
                {
                    Thread.Sleep(100);
                    strContext = File.ReadAllText(strBoeCheckFile, Encoding.Default);
                    //_log.WriteLog(strBoeCheckFile+":"+strContext, strCheckFile);
                    AppLogHelp.WriteLog(LogFileFormate.MES, "BOE "+strCheckFile + strBoeCheckFile + ":" + strContext);
                    if (string.IsNullOrEmpty(strContext) == false)
                    {
                        try
                        {
                            doc.LoadXml(ss.DataService(strCheckObjectID, strContext));
                            nodeList = doc.SelectNodes("/STD_OUT/Status");
                            strContext = nodeList[0].InnerText;
                            if (strContext == "0")
                            {
                                //_log.WriteLog("check success.", strCheckFile);
                                nodeJobFile = doc.SelectNodes("/STD_OUT/DATA/FILE_NAME");
                                nodeJobVersion = doc.SelectNodes("/STD_OUT/DATA/FILE_VERSION");
                                File.WriteAllText(strFmAutoappCheckFile, nodeJobFile[0].InnerText.Trim() + "," + nodeJobVersion[0].InnerText.Trim(),Encoding.Default);
                                AppLogHelp.WriteLog(LogFileFormate.MES, "BOE " + strCheckFile + " check success.");
                            }
                            else
                            {
                                nodeList = doc.SelectNodes("/STD_OUT/Error");
                                strContext = nodeList[0].InnerText;
                               // _log.WriteErr("check fail." + strContext, strCheckFile);
                                File.WriteAllText(strFmAutoappCheckFile,"error,"+strContext,Encoding.Default);
                                AppLogHelp.WriteLog(LogFileFormate.MES, "BOE " + strContext + " check fail.");
                            }
                        }
                        catch (Exception ex)
                        {
                           // _log("check exception." + ex.Message, strCheckFile);
                            File.WriteAllText(strFmAutoappCheckFile, "error,server.." + strContext, Encoding.Default);
                            AppLogHelp.WriteError(LogFileFormate.MES, "BOE " + strCheckFile + ex.Message);
                            continue;
                        }
                        finally
                        {
                            File.Delete(strBoeCheckFile);
                        }
                    }
                }

                //uploadFile
                if (File.Exists(strBoeUploadFile))
                {
                    Thread.Sleep(300);

                    strContext = File.ReadAllText(strBoeUploadFile, Encoding.Default);
                    //_log.WriteLog(strBoeUploadFile + ":" + strContext, strUploadFile);
                    AppLogHelp.WriteLog(LogFileFormate.MES, "BOE " + strUploadFile  + strBoeUploadFile + ":" + strContext);
                    if (string.IsNullOrEmpty(strContext) == false)
                    {
                        try
                        {
                            //ss.Url = "";
                            doc.LoadXml(ss.DataService(strUploadObjectID, strContext));
                            nodeList = doc.SelectNodes("/STD_OUT/Status");
                            strContext = nodeList[0].InnerText;
                            if (strContext == "0")
                            {
                                //_log.WriteLog("check success.", strUploadFile);
                                File.WriteAllText(strFmAutoappUploadFile, string.Empty, Encoding.Default);
                                AppLogHelp.WriteLog(LogFileFormate.MES, "BOE " + strUploadFile + " Upload success.");
                            }
                            else
                            {
                                nodeList = doc.SelectNodes("/STD_OUT/Error");
                                strContext = nodeList[0].InnerText;
                                //_log.WriteErr("check fail." + strContext, strUploadFile);
                                File.WriteAllText(strFmAutoappUploadFile, "MesError," + strContext, Encoding.Default);
                                AppLogHelp.WriteError(LogFileFormate.MES, "BOE " + strContext + " Upload fail.");
                            }
                        }
                        catch (Exception ex)
                        {
                            //_log.WriteErr("check exception." + ex.Message, strUploadFile);
                            File.WriteAllText(strFmAutoappUploadFile, "MesError,strUploadFile " + strContext + ex.Message, Encoding.Default);
                            AppLogHelp.WriteError(LogFileFormate.MES, "BOE check exception." + strUploadFile + ex.Message);
                            continue;
                        }
                        finally
                        {
                            File.Delete(strBoeUploadFile);
                        }
                    }
                }


                System.Threading.Thread.Sleep(100);
            }

        }
        public void stopThreadBoe()
        {


            while (this._BoeThread != null && (this._StatusThread.ThreadState == System.Threading.ThreadState.Background
                | this._BoeThread.ThreadState == System.Threading.ThreadState.Running)
                | this._BoeThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
            {
                System.Threading.Thread.Sleep(100);
                this._BoeThread.Abort();
                //_log.WriteLog("进程关闭", "Boe");
                AppLogHelp.WriteLog(LogFileFormate.MES, "BOE " + "进程关闭");
            }

            //_log.WriteLog("进程关闭完成", ThreadNameStatus);
        }
        #endregion

        #region "SPCDATA 上传"
        // AutoAPP.bu_Peng
        public Thread _SPCDataThread;
        public void AutoStartSPC()
        {
            
            MainForm.bu_peng = this;
            try
            {
                this._SPCDataThread = new Thread(new ThreadStart(this.CountThreadProcSPCData));
                this._SPCDataThread.Priority = ThreadPriority.Lowest ;
                this._SPCDataThread.IsBackground = true;
                this._SPCDataThread.Start();
            }
            catch (Exception ex)
            {
                //log.WriteErr("错误!" + ex.Message, "SPCData");
            }
        }
        //// AutoAPP.bu_Peng
        public void CountThreadProcSPCData()
        {
            WSClnt.DataExportDP Dp = new WSClnt.DataExportDP();
            string strMsg = string.Empty;
            string SaveCSV_Path = Dp._strSPCDataFolder;
            WSClnt.DataExportLin Exlin = new WSClnt.DataExportLin();
            //WSClnt.DataExportDP Exdp = new WSClnt.DataExportDP();

            
            if (string.IsNullOrEmpty(SaveCSV_Path))
            {
                SaveCSV_Path = @"D:\EYSPI\CSVData_Temp";
            }
            string strFrUICsvDirPath = SaveCSV_Path;
            //string strFrUIBackUpPath = "D:\\EYSPI\\SPCData_CSV_Backup\\";
            //int iMaxBackupDay = 7;
            while (true)
            {
                if (AutoAPP.MainForm.bEnDataSpcData == false)
                {
                    break;
                }
                try
                {
                    if (strFrUICsvDirPath != null && Directory.Exists(strFrUICsvDirPath))
                    {
                        string[] arrPcbIDDir = Directory.GetDirectories(strFrUICsvDirPath, "*_Finish");
                        if (arrPcbIDDir != null && arrPcbIDDir.Length > 0)
                        {
                            string[] array = arrPcbIDDir;
                            for (int i = 0; i < array.Length; i++)
                            {
                                string PcbIDDir = array[i];
                                //this._log.WriteLog("pcbIDDir:" + PcbIDDir, "SpcData");
                                AppLogHelp.WriteLog(LogFileFormate.SPCDataUpload, "pcbIDDir:" + PcbIDDir);
                                if (Directory.Exists(PcbIDDir))
                                {
                                    strMsg = Exlin.UploadToDBFromCsv(PcbIDDir);
                                    //this._log.WriteLog("Exlin=>UploadToDBFromCsv=>result:" + strMsg, "SpcData");
                                    AppLogHelp.WriteLog(LogFileFormate.SPCDataUpload, "Exlin=>UploadToDBFromCsv=>result:" + strMsg);
                                }
                                Thread.Sleep(100);
                            }
                        }
                    }

                    //lin 20190726  
                    Exlin.DeleteBackupPCBFolder();//strFrUIBackUpPath, iMaxBackupDay);
                    
                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    //this._log.WriteErr(ex.Message, "SPCData");
                    AppLogHelp.WriteError(LogFileFormate.SPCDataUpload, ex.Message);
                }
            }
        }
        // AutoAPP.bu_Peng
        public void stopThreadSpc()
        {
            try
            {
                while (this._SPCDataThread != null
                    && (this._SPCDataThread.ThreadState == System.Threading.ThreadState.Background
                    | this._SPCDataThread.ThreadState == System.Threading.ThreadState.Running
                    | this._SPCDataThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin))
                {
                    Thread.Sleep(100);
                    this._SPCDataThread.Abort();
                    //this._log.WriteLog("进程关闭", "SPCData");
                    AppLogHelp.WriteError(LogFileFormate.SPCDataUpload, "进程关闭");
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        
        #endregion

        #region "兆驰三星_双段"
        public Thread _ZHAOCHI_SamsungThread;
        private string strZHAOCHITmpPath = string.Empty;
        private string strZHAOCHISavePath = string.Empty;
        private bool _bIsKeepRunningAll = true;
        private int _iWaitMinSeconds = 1000;
        private readonly string RS_ENDLINE = "\r\n";
        private string _ZHAOCHI_TITLE = "Equipment ID,Operator ID,Carrier ID,Module ID,Start Time,End Time,Recipe,Result,POSITION";
        public void AutoZHAOCHI_Samsung(string ATmpPath,string ASavePath)
        {
            strZHAOCHITmpPath = ATmpPath;
            strZHAOCHISavePath = ASavePath;
            //if (this._ZHAOCHI_SamsungThread != null)
            //{
            //    this._ZHAOCHI_SamsungThread.Interrupt();
            //    this._ZHAOCHI_SamsungThread = new Thread(new ThreadStart(this.CountThreadSamsungThread));
            //    this._ZHAOCHI_SamsungThread.Priority = ThreadPriority.Lowest;
            //    this._ZHAOCHI_SamsungThread.IsBackground = true;
            //    this._ZHAOCHI_SamsungThread.Start();
            //}
            //else
            //{
                this._ZHAOCHI_SamsungThread = new Thread(new ThreadStart(this.CountThreadSamsungThread));
                this._ZHAOCHI_SamsungThread.Priority = ThreadPriority.Lowest;
                this._ZHAOCHI_SamsungThread.IsBackground = true;
                this._ZHAOCHI_SamsungThread.Start();
            //}
            
            
        }
        private void CountThreadSamsungThread()
        {
            string[] arrYearPath=null,arrMonthPath=null,arrDayPath=null,arrRealFilePath=null,arrRealFiles = null;
            DateTime dtTime = DateTime.Now;
            if (Directory.Exists(strZHAOCHISavePath) ==false)
            {
                Directory.CreateDirectory(strZHAOCHISavePath);
            }
            //String[] arrStrContent = null;
            System.Collections.Generic.List<String> lst = new List<string>();
            while (_bIsKeepRunningAll)
            {
                if (AutoAPP.MainForm.bEnDataZHAOCHISamsung == false)
                {
                    break;
                }
                try
                {
                    if (Directory.Exists(strZHAOCHITmpPath))
                    {
                        arrRealFilePath = Directory.GetDirectories(strZHAOCHITmpPath);
                        if (arrRealFilePath != null && arrRealFilePath.Length > 0)
                        {
                            foreach (string realPath in arrRealFilePath)
                            {
                                if (string.IsNullOrEmpty(realPath) == false && Directory.Exists(realPath))
                                {
                                    arrRealFiles = Directory.GetFiles(realPath, "*.csv");
                                    dtTime = DateTime.Now;

                                    //while (_bIsKeepRunningAll)
                                    //{
                                        if (arrRealFiles != null && arrRealFiles.Length > 1)
                                        {
                                            Thread.Sleep(_iWaitMinSeconds);
                                            string strContent = string.Empty,strContentSec = string.Empty;
                                            string strBarcode = string.Empty;
                                            string strSpit = string.Empty;
                                            for (int i = 0; i < arrRealFiles.Length; i++)
                                            {
                                                if (File.Exists(arrRealFiles[i]))
                                                {
                                                    strSpit = Path.GetFileNameWithoutExtension(arrRealFiles[i]).Split('_')[0];
                                                    if (string.IsNullOrEmpty(strSpit) == false
                                                        && string.Equals("noread", strSpit, StringComparison.CurrentCultureIgnoreCase) == false
                                                        )
                                                    {
                                                        strBarcode = strSpit;
                                                    }
                                                    //strBarcode = Path.GetFileNameWithoutExtension(arrRealFiles[i]).Split('_')[0];
                                                    if (i == 0)
                                                    {
                                                        strContent = File.ReadAllText(arrRealFiles[i], Encoding.Default);
                                                        lst = File.ReadAllLines(arrRealFiles[i], Encoding.Default).ToList<String>();
                                                        if (lst != null && lst.Count > 0)
                                                        {
                                                            //lst[1] = null;
                                                            //foreach (string str in lst)
                                                            //{
                                                            //    strContentSec += str;
                                                            //}
                                                        }
                                                        strContent = strContent.Replace(lst[1],"@@@@@@");
                                                        //arrStrContent.ToList();
                                                    }
                                                    else
                                                    {
                                                        System.IO.StreamReader sr = new System.IO.StreamReader(arrRealFiles[i]);
                                                        string lineStr; int iSread = 0;
                                                        while ((lineStr = sr.ReadLine()) != null)
                                                        {
                                                            iSread++;
                                                            if (lineStr.Contains(_ZHAOCHI_TITLE)&&iSread==0) continue;
                                                            if (iSread == 1) continue;
                                                            string strPadResult = lineStr.Split(',')[7];
                                                            string strPosition = lineStr.Split(',')[8];
                                                            //lineStr.Split(',')[7]+","+lineStr.Split(',')[8]
                                                            if (string.Equals("NG", strPadResult, StringComparison.CurrentCultureIgnoreCase)
                                                                | string.Equals("FAIL", strPadResult, StringComparison.CurrentCultureIgnoreCase))
                                                            {
                                                                if (strContent.Contains(",PASS," + strPosition) )
                                                                {
                                                                    strContent = strContent.Replace(",PASS," + strPosition, strPadResult == "NG" ? ",NG," + strPosition : ",FAIL," + strPosition);
                                                                }
                                                                //else if (strContent.Contains("RPASS," + strPosition))
                                                                //{
                                                                //    strContent = strContent.Replace("RPASS," + strPosition, strPadResult == "NG" ? "NG," + strPosition : "FAIL," + strPosition);

                                                                //}
                                                                else
                                                                {
                                                                    strContent += lineStr + RS_ENDLINE;
                                                                }
                                                                if (lst[1].Contains(",PASS,1"))
                                                                {
                                                                    lst[1] = lst[1].Replace(",PASS,1", strPadResult == "NG" ? ",NG,1" : ",FAIL,1");
                                                                }
                                                                if (lst[1].Contains(",RPASS,1"))
                                                                {
                                                                    lst[1] = lst[1].Replace(",RPASS,1", strPadResult == "NG" ? ",NG,1" : ",FAIL,1");
                                                                }
                                                            }
                                                            else if (string.Equals("PASS", strPadResult, StringComparison.CurrentCultureIgnoreCase)
                                                                | string.Equals("RPASS", strPadResult, StringComparison.CurrentCultureIgnoreCase)
                                                               | string.Equals("OK", strPadResult, StringComparison.CurrentCultureIgnoreCase))
                                                            {
                                                                if (strContent.Contains(",PASS," + strPosition))
                                                                {
                                                                    //strContent = strContent.Replace("PASS," + strPosition, "NG," + strPosition);
                                                                    continue;
                                                                }
                                                                else if (strContent.Contains(",RPASS," + strPosition))
                                                                {
                                                                    //strContent = strContent.Replace("REPASS," + strPosition, "NG," + strPosition);
                                                                    continue;
                                                                }
                                                                else
                                                                {
                                                                    strContent += lineStr + RS_ENDLINE;
                                                                }
                                                            }
                                                           
                                                        }
                                                        sr.Close();
                                                        sr.Dispose();

                                                    }
                                                }
                                            }
                                            if (string.IsNullOrEmpty(strBarcode))
                                            {
                                                strBarcode = DateTime.Now.ToString(RS_Format_DateTimeFileName);
                                            }
                                            if (string.IsNullOrEmpty(strContent) == false)
                                            {
                                                strContent = strContent.Replace("NOREAD", strBarcode);
                                                strContent = strContent.Replace("noread", strBarcode);
                                                strContent = strContent.Replace("@@@@@@", lst[1]);
                                                File.WriteAllText(Path.Combine(strZHAOCHISavePath, strBarcode + ".csv"), strContent, Encoding.Default);
                                                Directory.Delete(realPath, true);
                                                AppLogHelp.WriteLog(LogFileFormate.MES, "save success..=>" + Path.Combine(strZHAOCHISavePath, strBarcode + ".csv")); ;
                                            }
                                            //break;
                                        }
                                        
                                    //}
                                }
                            }
                        }
                    }
                    #region "other method"
                    //if (Directory.Exists(strZHAOCHITmpPath))
                    //{
                    //    arrYearPath = Directory.GetDirectories(strZHAOCHITmpPath);
                    //    if (arrYearPath != null && arrYearPath.Length > 0)
                    //    {
                    //        //months
                    //        foreach (string year in arrYearPath)
                    //        {
                    //            arrMonthPath = Directory.GetDirectories(year);
                    //            //days
                    //            if (arrMonthPath != null && arrMonthPath.Length > 0)
                    //            {
                    //                foreach (string month in arrMonthPath)
                    //                {
                    //                    arrDayPath = Directory.GetDirectories(month);
                    //                    if (arrDayPath != null && arrDayPath.Length > 0)
                    //                    {
                    //                        foreach (string day in arrDayPath)
                    //                        {
                    //                            arrRealFilePath = Directory.GetDirectories(day);
                    //                            if (arrRealFilePath != null && arrRealFilePath.Length > 0)
                    //                            {
                    //                                foreach (string realPath in arrRealFilePath)
                    //                                {
                    //                                    if (string.IsNullOrEmpty(realPath) == false && Directory.Exists(realPath))
                    //                                    {
                    //                                        arrRealFiles = Directory.GetFiles(realPath, "*.csv");
                    //                                        dtTime = DateTime.Now;

                    //                                        while (_bIsKeepRunningAll)
                    //                                        {
                    //                                            if ((DateTime.Now - dtTime).TotalSeconds > 5)
                    //                                            {
                    //                                                break;
                    //                                            }
                    //                                            if (arrRealFiles != null && arrRealFiles.Length > 1)
                    //                                            {
                    //                                                Thread.Sleep(_iWaitMinSeconds);
                    //                                                string strContent = string.Empty;
                    //                                                string strBarcode = string.Empty;
                    //                                                for (int i = 0; i < arrRealFiles.Length; i++)
                    //                                                {
                    //                                                    if (File.Exists(arrRealFiles[i]))
                    //                                                    {
                    //                                                        strBarcode = Path.GetFileNameWithoutExtension(arrRealFiles[i]).Split('_')[0];
                    //                                                        if (i == 0)
                    //                                                        {
                    //                                                            strContent += File.ReadAllText(arrRealFiles[i], Encoding.Default);
                    //                                                        }
                    //                                                        else
                    //                                                        {
                    //                                                            System.IO.StreamReader sr = new System.IO.StreamReader(arrRealFiles[i]);
                    //                                                            string lineStr;
                    //                                                            while ((lineStr = sr.ReadLine()) != null)
                    //                                                            {
                    //                                                                if (lineStr.Contains(_ZHAOCHI_TITLE)) continue;
                    //                                                                strContent += lineStr + RS_ENDLINE;
                    //                                                            }
                    //                                                            sr.Close();
                    //                                                            sr.Dispose();

                    //                                                        }
                    //                                                    }
                    //                                                }
                    //                                                if (string.IsNullOrEmpty(strBarcode))
                    //                                                {
                    //                                                    strBarcode = DateTime.Now.ToString(RS_Format_DateTimeFileName);
                    //                                                }
                    //                                                if (string.IsNullOrEmpty(strContent) == false)
                    //                                                {
                    //                                                    File.WriteAllText(Path.Combine(strZHAOCHISavePath, strBarcode + ".csv"), strContent, Encoding.Default);
                    //                                                    Directory.Delete(realPath, true);
                    //                                                }
                    //                                                break;
                    //                                            }
                    //                                        }
                    //                                    }
                    //                                }
                    //                            }
                    //                        }
                    //                    }

                    //                }

                    //            }

                    //        }
                    //    }

                    //}
                    #endregion
                    //if (AutoAPP.MainForm.bEnDataForPCB == false)
                    //{
                    //    break;
                    //}
                    GC.Collect();
                    Thread.Sleep(_iWaitMinSeconds);
                }
                catch (Exception ex)
                {
                    AppLogHelp.WriteError(LogFileFormate.MES, "CountThreadSamsungThread=>  " + ex.Message);
                    continue;
                }
                
            }
        }
        public void stopThreadZHAOCHISamsungThread()
        {
            try
            {
                while (this._ZHAOCHI_SamsungThread != null
                    && (this._ZHAOCHI_SamsungThread.ThreadState == System.Threading.ThreadState.Background
                    | this._ZHAOCHI_SamsungThread.ThreadState == System.Threading.ThreadState.Running
                    | this._ZHAOCHI_SamsungThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin))
                {
                    Thread.Sleep(100);
                    this._ZHAOCHI_SamsungThread.Abort();
                    //this._log.WriteLog("进程关闭", "SPCData");
                    AppLogHelp.WriteError(LogFileFormate.MES, "_ZHAOCHI_SamsungThread进程关闭");
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        

        #endregion

        #region "skyWorth"
        public System.Threading.Thread _SkyWorthThread;
        public void startSkyWorthRun()
        {

            AutoAPP.MainForm.bu_peng = this;
            try
            {
                _SkyWorthThread = new System.Threading.Thread(new System.Threading.ThreadStart(SaveMesDataWithSkyWorth));
                _SkyWorthThread.Priority = System.Threading.ThreadPriority.Lowest;
                _SkyWorthThread.IsBackground = true;
                _SkyWorthThread.Start();
            }
            catch (Exception ex)
            {

            }
        }
        private void SaveMesDataWithSkyWorth()
        {
            //业务逻辑
            string strPcbSqlsec = "SELECT " +
                "pcb.PCBID,pcb.JobIndex " +
                "FROM " +
                "spidb.TBBoard pcb " +
                "WHERE " +
                "pcb.StartTime >= '{0}' " +
                "AND pcb.EndTime <= '{1}'  ";
            string strPcbSql = "";

            string strPadSqlSec = "SELECT " +
                "pad.ABSHeight,pad.PerArea,pad.PerVolume,pad.ShiftX,pad.ShiftY,pad.JobIndex " +
                "FROM  " +
                "spidb.TBPadMeasure pad " +
                "WHERE " +
                "pad.PCBID ='{0}'";
            //"SELECT " +
            //"pcb.PCBID " +
            //"FROM " +
            //"spidb.TBBoard pcb " +
            //"WHERE " +
            //"pcb.StartTime >= '{0}' " +
            //"AND pcb.EndTime <= '{1}' ) ";
            string strJobInfoSec = "SELECT  " +
                "job.JobName  " +
                "FROM  " +
                "spidb.TBJobInfo AS job " +
                "WHERE  " +
                "job.SerNo ='{0}'";
            string strTitle = "HeightCPK,AreaCPK,VolCPK,ShiftXCPK,ShiftYCPK";
            string strJobInfo = string.Empty;
            string strPadSql = string.Empty;

            string strAutoAPPIniFile = @"D:\EYSPI\Bin\Config\autoApp.ini";
            double dHeightU = 0, dHeightL = 0, dAreaU = 0, dAreaL = 0, dVolU = 0, dVolL = 0, dShiftXU = 0, dShiftXL = 0, dShiftYU = 0, dShiftYL = 0
                , Height = 0, Area = 0, Vol = 0, ShiftX = 0, ShiftY = 0;

            double dHeightCPK = 0, dAreaCPK = 0, dVolCPK = 0, dShiftXCPK = 0, dShiftYCPK = 0;
            string SaveSkyWorthPath = string.Empty;
            string strJobIndex = string.Empty, strJobName = string.Empty;

            DateTime dtRealTime;
            DataTable dataTable = new DataTable();
            DataTable dataTablePad = new DataTable();

            while (true)
            {
                if (AutoAPP.MainForm.bEnDataSkyWorth == false)
                {
                    break;
                }
                dtRealTime = DateTime.Now;
                try
                {
                    if (dtRealTime.Minute == 59 & dtRealTime.Second == 30)
                    {
                        Dictionary<string, List<double>> dicHeight = new Dictionary<string, List<double>>();
                        Dictionary<string, List<double>> dicArea = new Dictionary<string, List<double>>();
                        Dictionary<string, List<double>> dicVol = new Dictionary<string, List<double>>();
                        Dictionary<string, List<double>> dicShiftX = new Dictionary<string, List<double>>();
                        Dictionary<string, List<double>> dicShiftY = new Dictionary<string, List<double>>();
                        //ArrayList lst = new ArrayList();
                        //DirectoryInfo(string, string> dir = new DirectoryInfo<string, string>();
                        List<double> lstAmountsHeight = new List<double>();
                        //ArrayList lstAmountsHeight = new ArrayList();
                        List<double> lstAmountsArea = new List<double>();
                        List<double> lstAmountsVol = new List<double>();
                        List<double> lstAmountsShiftX = new List<double>();
                        List<double> lstAmountsShiftY = new List<double>();
                        if (File.Exists(strAutoAPPIniFile))
                        {
                            Thread.Sleep(100);
                            dHeightU = INIFileHelper.ReadIniData("SkyWorth", "tbHeightU", string.Empty, strAutoAPPIniFile) == "" ? 0 : double.Parse(INIFileHelper.ReadIniData("SkyWorth", "tbHeightU", string.Empty, strAutoAPPIniFile));
                            dHeightL = INIFileHelper.ReadIniData("SkyWorth", "tbHeightL", string.Empty, strAutoAPPIniFile) == "" ? 0 : double.Parse(INIFileHelper.ReadIniData("SkyWorth", "tbHeightL", string.Empty, strAutoAPPIniFile));
                            dAreaU = INIFileHelper.ReadIniData("SkyWorth", "tbAreaU", string.Empty, strAutoAPPIniFile) == "" ? 0 : double.Parse(INIFileHelper.ReadIniData("SkyWorth", "tbAreaU", string.Empty, strAutoAPPIniFile));
                            dAreaL = INIFileHelper.ReadIniData("SkyWorth", "tbAreaL", string.Empty, strAutoAPPIniFile) == "" ? 0 : double.Parse(INIFileHelper.ReadIniData("SkyWorth", "tbAreaL", string.Empty, strAutoAPPIniFile));
                            dVolU = INIFileHelper.ReadIniData("SkyWorth", "tbVolU", string.Empty, strAutoAPPIniFile) == "" ? 0 : double.Parse(INIFileHelper.ReadIniData("SkyWorth", "tbVolU", string.Empty, strAutoAPPIniFile));
                            dVolL = INIFileHelper.ReadIniData("SkyWorth", "tbVolL", string.Empty, strAutoAPPIniFile) == "" ? 0 : double.Parse(INIFileHelper.ReadIniData("SkyWorth", "tbVolL", string.Empty, strAutoAPPIniFile));
                            dShiftXU = INIFileHelper.ReadIniData("SkyWorth", "tbShiftXU", string.Empty, strAutoAPPIniFile) == "" ? 0 : double.Parse(INIFileHelper.ReadIniData("SkyWorth", "tbShiftXU", string.Empty, strAutoAPPIniFile));
                            dShiftXL = INIFileHelper.ReadIniData("SkyWorth", "tbShiftXL", string.Empty, strAutoAPPIniFile) == "" ? 0 : double.Parse(INIFileHelper.ReadIniData("SkyWorth", "tbShiftXL", string.Empty, strAutoAPPIniFile));
                            dShiftYU = INIFileHelper.ReadIniData("SkyWorth", "tbShiftYU", string.Empty, strAutoAPPIniFile) == "" ? 0 : double.Parse(INIFileHelper.ReadIniData("SkyWorth", "tbShiftYU", string.Empty, strAutoAPPIniFile));
                            dShiftYL = INIFileHelper.ReadIniData("SkyWorth", "tbShiftYL", string.Empty, strAutoAPPIniFile) == "" ? 0 : double.Parse(INIFileHelper.ReadIniData("SkyWorth", "tbShiftYL", string.Empty, strAutoAPPIniFile));
                            SaveSkyWorthPath = INIFileHelper.ReadIniData("SkyWorth", "txSaveSkyWorthPath", string.Empty, strAutoAPPIniFile);

                            if (string.IsNullOrEmpty(SaveSkyWorthPath)) SaveSkyWorthPath = _strLogPengPath;
                            if (Directory.Exists(SaveSkyWorthPath) == false) Directory.CreateDirectory(SaveSkyWorthPath);
                        }

                        strPcbSql = string.Format(strPcbSqlsec, dtRealTime.AddMinutes(-59), dtRealTime);

                        dataTable = getDataTableForZZFox(_strConnectString, strPcbSql);

                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            //int iProcessorCount = Environment.ProcessorCount - 1;

                            //if (iProcessorCount <= 0)
                            //    iProcessorCount = 1;
                            //int KK = ((int)(dataTable.Rows.Count / iProcessorCount) + 1);
                            //KK = 100;
                            string strPcbID = string.Empty;
                            //Parallel.ForEach(Partitioner.Create(0, dataTable.Rows.Count, KK), range =>
                            //{

                            //for (int a = range.Item1; a < range.Item2; a++)
                            //{
                            for (int a = 0; a < dataTable.Rows.Count; a++)
                            {
                                strPcbID = dataTable.Rows[a][0].ToString();

                                if (string.IsNullOrEmpty(dataTable.Rows[a][1].ToString()) == false)
                                    strJobIndex = dataTable.Rows[a][1].ToString();
                                //pad sql
                                if (string.IsNullOrEmpty(strPcbID) == false)
                                {

                                    strPadSql = string.Format(strPadSqlSec, strPcbID);
                                    dataTablePad = getDataTableForZZFox(_strConnectString, strPadSql);
                                    if (dataTablePad != null && dataTablePad.Rows.Count > 0)
                                    {
                                        for (int j = 0; j < dataTablePad.Rows.Count; j++)
                                        {
                                            //if (Height == 0) continue;

                                            Height = (double.Parse(dataTablePad.Rows[j][0].ToString()));
                                            Area = (double.Parse(dataTablePad.Rows[j][1].ToString())) * 100;
                                            Vol = (double.Parse(dataTablePad.Rows[j][2].ToString())) * 100;
                                            ShiftX = (double.Parse(dataTablePad.Rows[j][3].ToString()));
                                            ShiftY = (double.Parse(dataTablePad.Rows[j][4].ToString()));
                                            string strJobIndexPad = dataTablePad.Rows[j][5].ToString();
                                            if (dicHeight.ContainsKey(strJobIndexPad))
                                            {
                                                dicHeight[strJobIndexPad].Add(Height);
                                                dicArea[strJobIndexPad].Add(Area);
                                                dicVol[strJobIndexPad].Add(Vol);
                                                dicShiftX[strJobIndexPad].Add(ShiftX);
                                                dicShiftY[strJobIndexPad].Add(ShiftY);
                                            }
                                            else
                                            {
                                                dicHeight.Add(strJobIndexPad, new List<double>());//lstAmountsHeight);
                                                dicArea.Add(strJobIndexPad, new List<double>());//lstAmountsArea);
                                                dicVol.Add(strJobIndexPad, new List<double>());//lstAmountsVol);
                                                dicShiftX.Add(strJobIndexPad, new List<double>());//lstAmountsShiftX);
                                                dicShiftY.Add(strJobIndexPad, new List<double>());//lstAmountsShiftY);
                                            }
                                            //lstAmountsHeight.Add(Height);
                                            //lstAmountsArea.Add(Area);
                                            //lstAmountsVol.Add(Vol);
                                            //lstAmountsShiftX.Add(ShiftX);
                                            //lstAmountsShiftY.Add(ShiftY);
                                        }
                                    }

                                }
                            }
                            // });
                            //for (int i = 0; i < dataTable.Rows.Count; i++)
                            //{

                            //}

                            if (dicHeight != null && dicHeight.Count > 0)
                            {
                                foreach (string str in dicHeight.Keys)
                                {
                                    lstAmountsHeight.Clear();
                                    lstAmountsArea.Clear();
                                    lstAmountsVol.Clear();
                                    lstAmountsShiftX.Clear();
                                    lstAmountsShiftY.Clear();
                                    strJobIndex = str;
                                    dicHeight.TryGetValue(str, out lstAmountsHeight);
                                    dicArea.TryGetValue(str, out lstAmountsArea);
                                    dicVol.TryGetValue(str, out lstAmountsVol);
                                    dicShiftX.TryGetValue(str, out lstAmountsShiftX);
                                    dicShiftY.TryGetValue(str, out lstAmountsShiftY);

                                    dHeightCPK = CalculateCPK(lstAmountsHeight, dHeightU, dHeightL);
                                    dAreaCPK = CalculateCPK(lstAmountsArea, dAreaU, dAreaL);
                                    dVolCPK = CalculateCPK(lstAmountsVol, dVolU, dVolL);
                                    dShiftXCPK = CalculateCPK(lstAmountsShiftX, dShiftXU, dShiftXL);
                                    dShiftYCPK = CalculateCPK(lstAmountsShiftY, dShiftYU, dShiftYL);
                                    //jobInfo
                                    strJobInfo = string.Format(strJobInfoSec, strJobIndex);
                                    dataTable = getDataTableForZZFox(_strConnectString, strJobInfo);
                                    if (dataTable != null && dataTable.Rows.Count > 0)
                                    {
                                        strJobName = dataTable.Rows[0][0].ToString();
                                    }
                                    File.WriteAllText(
                                        Path.Combine(SaveSkyWorthPath, strJobName + RS_UnderLine + dtRealTime.ToString(RS_Format_DateTimeFileName) + ".txt"),
                                               strTitle + RS_ENDLINE +
                                               dHeightCPK.ToString("0.000") + RS_CommaSplit +
                                               dAreaCPK.ToString("0.000") + RS_CommaSplit +
                                               dVolCPK.ToString("0.000") + RS_CommaSplit +
                                               (dShiftXCPK * 0.5).ToString("0.000") + RS_CommaSplit +
                                               (dShiftYCPK * 0.5).ToString("0.000") + RS_CommaSplit,
                                               Encoding.Default);
                                    AppLogHelp.WriteLog(LogFileFormate.MES, EM_MES_FORMAT._SkyWorth + Path.Combine(SaveSkyWorthPath, strJobName + RS_UnderLine + dtRealTime.ToString(RS_Format_DateTimeFileName) + RS_UnderLine + ".txt") + " success..");

                                }
                            }
                            ClearMemory();

                        }

                    }
                }
                catch (Exception exx)
                {
                    AppLogHelp.WriteError(LogFileFormate.MES, EM_MES_FORMAT._SkyWorth + exx.Message);
                    continue;
                }
                Thread.Sleep(100);
            }

        }
        public void stopThreadSkyWorth()
        {


            while (this._SkyWorthThread != null && (this._StatusThread.ThreadState == System.Threading.ThreadState.Background
                | this._SkyWorthThread.ThreadState == System.Threading.ThreadState.Running)
                | this._SkyWorthThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
            {
                System.Threading.Thread.Sleep(100);
                this._SkyWorthThread.Abort();
                //_log.WriteLog("进程关闭", "Boe");
                AppLogHelp.WriteLog(LogFileFormate.MES, "SkyWorth " + "进程关闭");
            }

            //_log.WriteLog("进程关闭完成", ThreadNameStatus);
        }
        private double CalculateCPK(List<double> lstAmounts, double AdUpperLimit, double AdLowLimit)
        {
            double amount = 0;
            try
            {
                if (lstAmounts != null && lstAmounts.Count > 0)
                {
                    double average = lstAmounts.Average();
                    double totalS = 0;
                    foreach (var amt in lstAmounts)
                    {
                        totalS += Math.Pow((amt - average), 2);
                    }
                    if (lstAmounts.Count > 1)
                        totalS = totalS / (lstAmounts.Count - 1);
                    totalS = Math.Pow(totalS, 0.5);
                    double USL = (AdUpperLimit - average) / (3 * totalS);
                    double LSL = (average - AdLowLimit) / (3 * totalS);
                    amount = Math.Min(USL, LSL);
                }
            }
            catch (Exception ex)
            {
                AppLogHelp.WriteLog(LogFileFormate.MES, "CPK " + ex.Message);
                throw ex;
            }
            return amount;
        }
        #endregion
    }
    public enum Em_EquipStatus
    {
        //start,// 软件开启
        run,  // 机器运行
        idle, //闲置
        stop,// 停止
        error //故障

    }
}

