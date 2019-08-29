using System;
using System.Data;
using System.Threading;
using System.IO;


namespace AutoAPP
{
    class ThreadProcessEXFO
    {
        private Boolean running = false;
        private DateTime dtStartTime = new DateTime(2011, 11, 11);
        private DateTime dtEndTime = new DateTime(2011, 11, 11);
        private Thread threadProcess;
        private static Mutex mut = new Mutex();
        private string strConnectString = string.Empty;
        private string strDatatableName = string.Empty;
        private string strConfigIniPath = @"D:\EYSPI\Bin\Config";
        private string _strIniAutoAPPConfigFileName = "autoApp.ini";
       
        private string ThreadName = "Update Data To EXFO";
   
        public ThreadProcessEXFO(DateTime dtStartTime, DateTime dtEndTime)
        {
            this.dtStartTime = dtStartTime;
            this.dtEndTime = dtEndTime;
            string strEXiniConecction = string.Empty, strEXiniAutoTime = string.Empty;
            
            string strEXIniFile = Path.Combine(strConfigIniPath, _strIniAutoAPPConfigFileName);
            if (File.Exists(strEXIniFile))
            {
                strEXiniConecction = WSClnt.INIFileHelper.ReadIniData("autoAPP", "EXFOConnectionString", "", strEXIniFile);
            }
            this.strConnectString = strEXiniConecction;
            strDatatableName =  Properties.Settings.Default.EXFOTableName; 
            
        }
 
        public bool Running
        {
            get { return running; }
        }


        public void Run()
        {
            try
            {
                this.running = true;
                this.threadProcess = new Thread(new ThreadStart(CountThreadProc));
                this.threadProcess.Priority = ThreadPriority.BelowNormal;
                this.threadProcess.Start();
            }
            catch (Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
            }
        }

        public void Stop()
        {
            this.running = false;
            //log.WriteLog("进程关闭开始", ThreadName);
            while (this.threadProcess != null && this.threadProcess.IsAlive)
            {
                Thread.Sleep(300);
            }
            //log.WriteLog("进程关闭完成", ThreadName);

        }

        public void CountThreadProc()
        {
            string strLog = string.Empty;
                bu_Joch bu = new bu_Joch();
            DataTable dt = new DataTable();
            try
            {
                int intRowNo = 0;

                if (this.running == true)
                {
                    dt = bu.GetDataTableFromEXFO(this.strConnectString, this.strDatatableName);

                    strLog = string.Format("GetSPCData开始,{0}", dtStartTime.ToString("yyyy-MM-dd HH:mm:ss") + "~" + dtEndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    //log.WriteLog(strLog, ThreadName);
                    AppLogHelp.WriteLog(LogFileFormate.MES, ThreadName + strLog);

                    dt = bu.GetSPCDataForEXFO(this.strConnectString, this.strDatatableName, dtStartTime, dtEndTime);
                    if (dt != null)
                    {
                        intRowNo = dt.Rows.Count;
                        strLog = string.Format("GetSPCData结束，共获取{1}日期之前下的{0}条记录", intRowNo, dtStartTime.ToString("yyyy-MM-dd HH:mm:ss") + "~" + dtEndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        //log.WriteLog(strLog, ThreadName);
                        AppLogHelp.WriteLog(LogFileFormate.MES, ThreadName + strLog);
                        if (intRowNo > 0)
                        {
                            bu.BulkToDB(this.strConnectString, dt, null);
                            strLog = string.Format("Insert  SPCData结束，共同步{1}日期之前下的{0}条记录", intRowNo, dtStartTime.ToString("yyyy-MM-dd HH:mm:ss") + "~" + dtEndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                            //log.WriteLog(strLog, ThreadName);
                            AppLogHelp.WriteLog(LogFileFormate.MES, ThreadName + strLog);
                        }
                    }
                    else
                    {
                        strLog =  "GetSPCData结束, NULL ";
                        //log.WriteLog(strLog, ThreadName);
                        AppLogHelp.WriteLog(LogFileFormate.MES, ThreadName + strLog);
                    }
                }

            }
            catch (Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
                AppLogHelp.WriteLog(LogFileFormate.MES, ThreadName + "错误 ! " + ex.ToString());
            }
            finally
            {
                //log.WriteLog("线程EXFO结束!", ThreadName);
                AppLogHelp.WriteLog(LogFileFormate.MES, ThreadName + strLog);
            }
        }

    }
}
