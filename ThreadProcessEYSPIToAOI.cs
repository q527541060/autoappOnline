using System;
using System.Data;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;


namespace AutoAPP
{
    public class ThreadProcessFovEYSPIToAOI
    {
        private Boolean running = false;
        public Thread threadProcess;
        private static Mutex mut = new Mutex();
        private string strConnectString = string.Empty;
        
        private string ThreadName = "Save  File For Fov";
        private InspectConfig.ConfigData Aconfig =null;
        private int _iLimitFileNum  =0;
        //public InspectConfig.ConfigData _configData = null;
        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentThread();

        //[DllImport("kernel32.dll")]       
        //static extern IntPtr SetThreadAffinityMask(IntPtr hThread, IntPtr dw);

        //[DllImport("kernel32.dll")]
        //static extern IntPtr GetCurrentThread();
        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentProcessorNumberEx();
        [DllImport("kernel32.dll")]
        static extern IntPtr SetThreadAffinityMask(IntPtr hThread, IntPtr dwThreadAffinityMask);


        [DllImport("kernel32.dll")]
        static extern UIntPtr SetThreadAffinityMask(IntPtr hThread, UIntPtr dwThreadAffinityMask);
        private AppLayerLib.AppSettingHandler _appSettingHandle = new AppLayerLib.AppSettingHandler();
        public ThreadProcessFovEYSPIToAOI( InspectConfig.ConfigData Aconfig, int iLimitFileNum, string ExToFovImagePath, AppLayerLib.AppSettingHandler appSettingHandle)
        {
            //lin 20190813
            //this.strConnectString = Properties.Settings.Default.MySQLConnect;         
            this.strConnectString = WSClnt.PubStaticParam._strSPIdbConnectionString;        
            this.Aconfig = Aconfig;
            this._iLimitFileNum = iLimitFileNum;
            this._ExToFovImagePath = ExToFovImagePath;
            this._appSettingHandle = appSettingHandle;
        }
        private string _ExToFovImagePath = "";
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
                this.threadProcess.IsBackground = true;
                this.threadProcess.Priority = ThreadPriority.Lowest;
                this.threadProcess.Start();
            }
            catch (Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
                return;
            }
        }
        public void Stop()
        {
            
                this.running = false;
                //log.WriteLog("进程关闭开始", ThreadName);
                AppLogHelp.WriteLog(LogFileFormate.EYspiToAOI, "进程关闭开始");
                try
                {
                    while (this.threadProcess != null && (//this.threadProcess.ThreadState != System.Threading.ThreadState.Aborted|
                        this.threadProcess.ThreadState == System.Threading.ThreadState.Background
                        | this.threadProcess.ThreadState == System.Threading.ThreadState.Running
                        | this.threadProcess.ThreadState == System.Threading.ThreadState.WaitSleepJoin))
                    {
                        Thread.Sleep(300);
                        this.threadProcess.Abort();
                    }
                }
                catch (Exception)
                {
                    return;
                }
                //log.WriteLog("进程关闭完成", ThreadName);
                AppLogHelp.WriteLog(LogFileFormate.EYspiToAOI, "进程关闭完成");
            
        }
        public void CountThreadProc()
        {
            string strLog = string.Empty;
            //bu_Peng bu_p = new bu_Peng();
            try
            {
                AutoAPP.MainForm._runnerFovThreadProcessFovEYSPIToAOI = this;
                if (this.running == true)
                {
                    //BoundCPUX();
                    int iCore = Environment.ProcessorCount;
                    iCore--;
                    //iCore--;
                    SetThreadAffinityMask(GetCurrentThread(), new UIntPtr(SetCpuID(iCore)));
                    //bu_Peng.SaveDataForLHfoxNG(strConnectString, strLastPcbPath, strLastPcbFile, strLastAppsettingTmpFile, strAppsettingPath, appSettingHandle, timer, log);
                    bu_Peng bp = new bu_Peng();
                    //bp._configData = _configData;
                    bp.ReadFovImagesForEYSPIToAOI( Aconfig, this._iLimitFileNum, this._ExToFovImagePath, this._appSettingHandle);
                }

            }
            catch (Exception ex)
            {
                //log.WriteErr("警告 ! " + ex.ToString(), ThreadName);
                AppLogHelp.WriteWarning(LogFileFormate.EYspiToAOI, ex.ToString());
                return;
            }
            finally
            {
                //log.WriteLog("线程Fov结束!", ThreadName);
                //AutoAPP.MainForm.bFov = true;
                
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
        [DllImport("psapi.dll")]
        static extern int EmptyWorkingSet(IntPtr hwProc);
    }
}
