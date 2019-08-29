using System;
using System.Data;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;


namespace AutoAPP
{
    public class ThreadProcessLuBangTongPRD
    {
        private Boolean running = false;
        public Thread threadProcess;
        private static Mutex mut = new Mutex();
        private string strConnectString = string.Empty;
        
        private string _AStatusURL; 
        private string _AsErrorUrl;
        private string _AtbUSERNO;
        private string _AtbMO;
        private string _AtbMachineNo;
        private string _AtbOn;
        private string _AtbSf;
        private string _AclientId;
        private string _AclientSecret;
        private string _AtbITEMVALUE;
        private string ThreadName = "Save  File For LuBangTong";
        private InspectConfig.ConfigData Aconfig =null;
        //public InspectConfig.ConfigData _configData = null;
        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentThread();

        [DllImport("kernel32.dll")]       
        static extern IntPtr SetThreadAffinityMask(IntPtr hThread, IntPtr dw);
        public ThreadProcessLuBangTongPRD(string AtbUSERNO,
                            string AtbMO,
                            string AtbITEMVALUE,
                            string AtbMachineNo,
                            string AtbOn,
                            string AtbSf,
                            string AclientId,string AclientSecret, string AStatusURL, string AsErrorUrl)
        {
            //lin 20190813
            //this.strConnectString = Properties.Settings.Default.MySQLConnect;         
            this.strConnectString = WSClnt.PubStaticParam._strSPIdbConnectionString;         
            this._AsErrorUrl = AsErrorUrl;
            this._AStatusURL = AStatusURL;
            _AtbUSERNO = AtbUSERNO;
            _AtbMO = AtbMO;
            _AtbMachineNo = AtbMachineNo;
            _AtbOn = AtbOn;
            _AtbSf = AtbSf;
            _AclientId = AclientId;
            _AclientSecret = AclientSecret;
            _AtbITEMVALUE = AtbITEMVALUE;
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
                //log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
                //return;
            }
        }
        public void Stop()
        {
            
                this.running = false;
                //log.WriteLog("进程关闭开始", ThreadName);

                while (this.threadProcess != null && (this.threadProcess.ThreadState == System.Threading.ThreadState.Background
                    || this.threadProcess.ThreadState == System.Threading.ThreadState.Running))
                {
                    Thread.Sleep(300);
                    this.threadProcess.Abort();
                }

                //log.WriteLog("进程关闭完成", ThreadName);
            
            
        }
        public void CountThreadProc()
        {
            string strLog = string.Empty;
            //bu_Peng bu_p = new bu_Peng();
            try
            {
                AutoAPP.MainForm._runnerLuBangTongPRD = this;
                if (this.running == true)
                {
                    BoundCPUX();
                    //bu_Peng.SaveDataForLHfoxNG(strConnectString, strLastPcbPath, strLastPcbFile, strLastAppsettingTmpFile, strAppsettingPath, appSettingHandle, timer, log);
                    bu_Peng bp = new bu_Peng();
                    //bp._configData = _configData;
                    bp.SaveDataLuBangTongPRD(_AtbUSERNO,
                    _AtbMO,
                    _AtbITEMVALUE,
                    _AtbMachineNo,
                    _AtbOn,
                    _AtbSf,
                    _AclientId,
                    _AclientSecret, _AStatusURL, _AsErrorUrl);
                }

            }
            catch (Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
                //throw ex;
            }
            finally
            {
                //log.WriteLog("线程ThreadProcessLuBangTong结束!", ThreadName);
            }
        }
        private void BoundCPUX()
				 {
					 //	Thread CPU bounding
					 int cpuQty=0;
					 int mask=0;
					// if(_bCapUseMotionInPos==false)
					 //{
					
					// if(_configData->_algUseFirstCPUCore==false)
					 {
						 cpuQty=System.Environment.ProcessorCount;
						 mask=(int)Math.Pow(2,cpuQty);
						 mask=mask-1-1;
                         SetThreadAffinityMask(GetCurrentThread(), new IntPtr(mask));
				     }
					// }
				 }

    }
}
