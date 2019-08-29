using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace AutoAPP
{
    public struct PCBFOVConfig
    {
        //InspectConfig.ConfigData Aconfig, string ExToFovImagePath, AppLayerLib.AppSettingHandler AappSettingHandle
        //[Serializable]
        public InspectConfig.ConfigData Aconfig;
        public string ExToFovImagePath;
        public AppLayerLib.AppSettingHandler AappSettingHandle;
        public string IP;
        public string LineName;

    }
    class AppLogHelp
    {
        
            private static string _logPath = string.Empty;
            private static string RS_LOG_PATHNAME = "log";
            private static string RS_DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss:fff:      ";
            private static string RS_DATE_FORMAT = "yyyyMMdd";
            public static string LogFielPrefix = "\\";
            private static string RS_FILE_FIX = ".log";
            private static string RS_R = "]   ";
            private static string RS_L = "[";
            public static string LogPath
            {
                get
                {
                    if (_logPath == string.Empty)
                    {
                        _logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory+RS_LOG_PATHNAME);
                        if (Directory.Exists(_logPath) == false)
                        {
                            Directory.CreateDirectory(_logPath);
                        }
                    }
                    return _logPath;
                }
                set { _logPath = value; }
            }

            private static string logFielPrefix = string.Empty;
           
            

            
            public static void WriteLog(string logFile, string msg)
            {
                try
                {
                    System.IO.StreamWriter sw = System.IO.File.AppendText(
                        LogPath + LogFielPrefix + logFile + " " +
                        DateTime.Now.ToString(RS_DATE_FORMAT) + RS_FILE_FIX
                        );
                    sw.WriteLine(DateTime.Now.ToString(RS_DATETIME_FORMAT) + msg);
                    sw.Close();
                }
                catch
                { }
            }

        
            public static void WriteLog(LogFileFormate logFile, string msg)
            {
                WriteLog(logFile.ToString(), RS_L + LogFileStatus.Trace + RS_R + msg);
            }
            public static void WriteError(LogFileFormate logFile,  string msg)
            {
                WriteLog(logFile.ToString(), RS_L + LogFileStatus.Error + RS_R + msg);
            }
            public static void WriteWarning(LogFileFormate logFile, string msg)
            {
                WriteLog(logFile.ToString(), RS_L + LogFileStatus.Warning + RS_R + msg);
            }
        }

        public enum LogFileStatus
        {
            Trace,
            Warning,
            Error 
        }

        public enum LogFileFormate
        {
            AppMain,
            SQL,
            Delete,
            FOVPCB,
            EYspiToAOI,
            MES,
            SPCDataUpload,
            EquipStatus

        }

        public enum EM_MES_FORMAT
        {
            _EXFO,// "-EX富士康",
            _SaveCSVFileForFoxconn,// "-郑州富士康",
            _JUFEI,// "-聚飞",
            _CDFOX ,// "-成都富士康",
            _LHFox ,// "-龙华富士康",
            _DefaultTools ,// "-基础格式工具",
            _CaiHuang ,// "-彩煌",
            _LeiNeng ,// "-雷能",
            _RenBao,// "-仁宝",
            _TianJinWeiYe,// "-天津天地伟业",
            _WeiXin_suzhou ,// "-苏州维信",
            _ZheJaingDaHua ,// "-浙江大华",
            _DataLuBangTong,//"-鲁帮通",
            _NanChangHuaQin,//"-南昌华勤",
            _Boe,//"-京东方",
            _ZHAOCHISamsung,// "-兆驰三星"
            _SkyWorth, // 创维数字



        }
    
}
