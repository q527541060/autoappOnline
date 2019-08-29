using System;
using System.Data;
using System.Threading;
using System.IO;


namespace AutoAPP
{
    class ThreadProcessPCB
    {
        private Boolean running = false;
        private Thread threadProcess;
        private static Mutex mut = new Mutex();
        private string strConnectString = string.Empty;
        private Log log;
        private string ThreadName = "Save  File For PCB";

        public ThreadProcessPCB(Log log)
        {
            //lin 20190813
            //this.strConnectString = Properties.Settings.Default.MySQLConnect;
            this.strConnectString = WSClnt.PubStaticParam._strSPIdbConnectionString;
            this.log = log;
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
                this.threadProcess.Priority = ThreadPriority.Lowest;
                this.threadProcess.Start();
            }
            catch (Exception ex)
            {
                log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
            }
        }
        public void Stop()
        {
            this.running = false;
            log.WriteLog("进程关闭开始", ThreadName);
            while (this.threadProcess != null && this.threadProcess.IsAlive)
            {
                //Thread.Sleep(300);
                this.threadProcess.Abort();
            }
            log.WriteLog("进程关闭完成", ThreadName);
        }
        public void CountThreadProc()
        {
            string strLog = string.Empty;
            //bu_Peng bu_p = new bu_Peng();
            try
            {
                if (this.running == true)
                {
                    //bu_Peng bp = new bu_Peng();
                    //bp.JoinFovToPcbImage(log);

                }

            }
            catch (Exception ex)
            {
                log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
            }
            finally
            {
                log.WriteLog("线程Pcb结束!", ThreadName);
            }
        }

    }
}
