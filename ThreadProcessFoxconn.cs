using System;
using System.Data;
using System.Threading;


namespace AutoAPP
{
    class ThreadProcessFoxconn
    {
        private Boolean running = false;
        private DateTime dtStartTime = new DateTime(2011, 11, 11);
        private DateTime dtEndTime = new DateTime(2011, 11, 11);
        private string strCSVFilePath = string.Empty;
        private int intSectionTime = 10;
        private Thread threadProcess;
        private static Mutex mut = new Mutex();
        private string strConnectString = string.Empty;
        //private Log log;
        private string ThreadName = "Save CSV File For Foxconn";
   
        public ThreadProcessFoxconn(DateTime dtStartTime, DateTime dtEndTime,string AstrCSVFilePath, int AintSectionTime)
        {
            this.dtStartTime = dtStartTime;
            this.dtEndTime = dtEndTime;
            //lin 20190813
            //this.strConnectString = Properties.Settings.Default.MySQLConnect;
            this.strConnectString = WSClnt.PubStaticParam._strSPIdbConnectionString;
            this.strCSVFilePath = AstrCSVFilePath;
            intSectionTime = AintSectionTime;
            //this.log = log;
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
                this.threadProcess.Priority = ThreadPriority.Normal;
                this.threadProcess.Start();
            }
            catch (Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
                AppLogHelp.WriteError(LogFileFormate.MES, ThreadName + ex.ToString());
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
             try
            {

                if (this.running == true)
                {
                    //Console.WriteLine(strCSVFilePath);
                    
                    bu_Peng.ExportZZFoxconnInformat(this.strConnectString,strCSVFilePath, dtStartTime, dtEndTime, intSectionTime);                    
                }

            }
            catch (Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
            }
            finally
            {
                //log.WriteLog("线程FoxConn结束!", ThreadName);
            }
        }

    }
}
