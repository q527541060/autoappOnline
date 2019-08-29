using System;
using System.Threading;


namespace AutoAPP
{
    public class ThreadProcess
    {
        private Boolean running = false;
        private DateTime dtStartTime = new DateTime(2011, 11, 11);
        private DateTime dtEndTime = new DateTime(2011, 11, 11);
        public Thread threadProcess;
        private string strConnectString = string.Empty;
        
        private string ThreadName = "DeleteData";
        private bool blnBackupData = false;
        private string strBackFilePath = string.Empty;
        private bool blnDeleteEveryHour = false;
        private int _iPCBLimit = 0;
        public ThreadProcess(DateTime dtEndTime, bool AblnBackupData, string AstrBackFilePath,bool blnDeleteDataEveryHour, int iPCBLimit)
        {
            this.dtEndTime = dtEndTime;
            //lin 20190813
            //this.strConnectString = Properties.Settings.Default.MySQLConnect;
            this.strConnectString = WSClnt.PubStaticParam._strSPIdbConnectionString;
            blnBackupData = AblnBackupData;
            strBackFilePath = AstrBackFilePath;
            blnDeleteEveryHour = blnDeleteDataEveryHour;
            _iPCBLimit = iPCBLimit;
            
        }

      
        public ThreadProcess(DateTime dtStartTime, DateTime dtEndTime,bool AblnBackupData, string AstrBackFilePath, bool blnDeleteDataEveryHour, int iPCBLimit)
        {
            this.dtStartTime = dtStartTime;
            this.dtEndTime = dtEndTime;
            //lin 20190813
            //this.strConnectString = Properties.Settings.Default.MySQLConnect;
            this.strConnectString = WSClnt.PubStaticParam._strSPIdbConnectionString;
            blnBackupData = AblnBackupData;
            blnDeleteEveryHour = blnDeleteDataEveryHour;
            strBackFilePath = AstrBackFilePath;
            _iPCBLimit = iPCBLimit;
            
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
                this.threadProcess = new Thread(CountThreadProc);
               // this.threadProcess.Priority = ThreadPriority.BelowNormal;
                this.threadProcess.Priority = ThreadPriority.Lowest;
                this.threadProcess.IsBackground = true;
                this.threadProcess.Start();
            }
            catch (Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
            }
        }

        public void Stop()
        {
            try
            {
                this.running = false;
                //log.WriteLog("进程关闭开始", ThreadName);
                //while (this.threadProcess != null && this.threadProcess.IsAlive)
                while (this.threadProcess != null
                            & (this.threadProcess.ThreadState == System.Threading.ThreadState.Running
                            | this.threadProcess.ThreadState == System.Threading.ThreadState.Background
                            | threadProcess.ThreadState == System.Threading.ThreadState.WaitSleepJoin
                            ))
                {
                    Thread.Sleep(300);
                    this.threadProcess.Abort();
                }
                //log.WriteLog("进程关闭完成", ThreadName);
            }
            catch (Exception ex)
            {
                //log.WriteLog("进程关闭:", ThreadName);
                return;
            }
          

        }

        public void CountThreadProc()
        {
            string strLog = string.Empty;
            try
            {
                bu_Joch bu = new bu_Joch();
                int intRowNo = 0;
                AutoAPP.MainForm._runnerDeleteProcess = this;
                if (this.running == true)
                {
                    intRowNo = bu.DeleteAndBackupDataByDays(dtStartTime, dtEndTime, 2, strConnectString, blnBackupData, strBackFilePath, blnDeleteEveryHour, _iPCBLimit);
                    strLog = string.Format("删除结束，共删除{1}日期之前下的{0}条记录", intRowNo, dtStartTime.ToString("yyyy-MM-dd HH:mm:ss") + "~" + dtEndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (blnBackupData)
                    {
                        strLog += "\r\n  数据备份目录:" + strBackFilePath;
                    }
                    else
                    {
                        strLog += "\r\n 无数据备份!";
                    }

                    //log.WriteLog(strLog, ThreadName);
                    AppLogHelp.WriteLog(LogFileFormate.Delete, ThreadName + " :" + strLog);
                }

            }
            catch (Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
                AppLogHelp.WriteError(LogFileFormate.Delete, ThreadName + "错误 ! " + ex.ToString());
                return;
            }
            finally
            {
                AppLogHelp.WriteLog(LogFileFormate.Delete, ThreadName + " :结束" );
                this.running = false;
            }
        }

    }
}
