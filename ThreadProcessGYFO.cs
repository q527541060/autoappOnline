using System;
using System.Data;
using System.Threading;
using System.IO;


namespace AutoAPP
{
    class ThreadProcessGYFO
    {
        private Boolean running = false;
        private DateTime lastTime = new DateTime(2011, 11, 11);
        private DateTime nowTime = new DateTime(2011, 11, 11);
        //private string strCDPath = string.Empty;
        //private int intSectionTime = 10;
        private Thread threadProcess;
        //private string configPath = string.Empty;
        private static Mutex mut = new Mutex();
        private string strConnectString = string.Empty;

        private string ThreadName = "Save Data For GYFO";

        public ThreadProcessGYFO(DateTime lastTime,DateTime nowTime)
        {
            this.lastTime = lastTime;
            this.nowTime = nowTime;
            //lin 20190813
            //this.strConnectString = "server=127.0.0.1;user id=root;database=spidb;Charset=utf8";//Properties.Settings.Default.MySQLConnect;
            this.strConnectString =  WSClnt.PubStaticParam._strSPIdbConnectionString;
            //this.strCDPath = AstrCDPath;          
          
            //this.configPath = strConfigPath;
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
            }
        }

        public  void Stop()
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
            //bu_Peng bu_p = new bu_Peng();
            bu_Peng bp = new bu_Peng();
            try
            {
                if (this.running == true)
                {               
                                      
                    bp.SaveDataForGYFox(strConnectString, lastTime, nowTime);                    
                }
            }
            catch (Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
            }
            finally
            {
                //log.WriteLog("线程GYFOX结束!", ThreadName);
            }
        }

    }
}
