using System;
using System.Data;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;


namespace AutoAPP
{
    public class ThreadProcessFov
    {
        private Boolean running = false;
        public Thread threadProcess;
        private static Mutex mut = new Mutex();
        private string strConnectString = string.Empty;

        private string ThreadName = "Save  File For Fov";
        private InspectConfig.ConfigData Aconfig =null;
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
        public ThreadProcessFov( InspectConfig.ConfigData Aconfig, string ExToFovImagePath, AppLayerLib.AppSettingHandler AappSettingHandle)
        {
            //lin 20190813
            //this.strConnectString = Properties.Settings.Default.MySQLConnect;         
            this.strConnectString =  WSClnt.PubStaticParam._strSPIdbConnectionString;         
            this.Aconfig = Aconfig;
            this._ExToFovImagePath = ExToFovImagePath;
            this._appSettingHandle = AappSettingHandle;
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
                if (this.threadProcess != null )
                {
                    this.threadProcess =null;
                    this.threadProcess = new Thread(CountThreadProc);
                    this.threadProcess.IsBackground = true;
                    this.threadProcess.Priority = ThreadPriority.Lowest;
                    this.threadProcess.Start();
                }
                else
                {
                    this.threadProcess = new Thread(CountThreadProc);
                    this.threadProcess.IsBackground = true;
                    this.threadProcess.Priority = ThreadPriority.Lowest;
                    this.threadProcess.Start();
                }
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
                try
                {
                    while (this.threadProcess != null && (this.threadProcess.ThreadState == System.Threading.ThreadState.Background
                        | this.threadProcess.ThreadState == System.Threading.ThreadState.Running |
                        this.threadProcess.ThreadState == System.Threading.ThreadState.WaitSleepJoin))
                    {
                        Thread.Sleep(300);
                        this.threadProcess.Abort();
                        //log.WriteLog("进程关闭完成", ThreadName);
                        AppLogHelp.WriteLog(LogFileFormate.FOVPCB, "进程关闭完成");

                    }
                }
                catch (Exception exx)
                {
                    //log.WriteLog("进程关闭完成", ThreadName);
                    return;
                }
                //log.WriteLog("进程关闭完成", ThreadName);
                AppLogHelp.WriteLog(LogFileFormate.FOVPCB, ThreadName+"进程关闭完成");
            
        }
        public void CountThreadProc()
        {
            string strLog = string.Empty;
            //bu_Peng bu_p = new bu_Peng();
            try
            {
                AutoAPP.MainForm._runnerFov = this;
                if (this.running == true)
                {
                    //BoundCPUX();
                    int iCore = Environment.ProcessorCount;
                    //iCore--;
                    iCore--;
                    SetThreadAffinityMask(GetCurrentThread(), new UIntPtr(SetCpuID(iCore)));
                    //bu_Peng.SaveDataForLHfoxNG(strConnectString, strLastPcbPath, strLastPcbFile, strLastAppsettingTmpFile, strAppsettingPath, appSettingHandle, timer, log);
                    bu_Peng bp = new bu_Peng();
                    //bp._configData = _configData;
                    bp.ReadFovImages( Aconfig, this._ExToFovImagePath, this._appSettingHandle);
                }

            }
            catch (Exception ex)
            {
               // log.WriteErr("警告 ! " + ex.ToString(), ThreadName);
                AppLogHelp.WriteWarning(LogFileFormate.FOVPCB,ThreadName+ ex.Message);
                return;
            }
            finally
            {
                //log.WriteLog("线程Fov结束!", ThreadName);
                //AutoAPP.MainForm.bFov = true;
                AppLogHelp.WriteLog(LogFileFormate.FOVPCB, ThreadName + "线程Fov结束!");
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
