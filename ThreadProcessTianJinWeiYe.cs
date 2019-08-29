using System;
using System.Data;
using System.Threading;
using System.IO;


namespace AutoAPP
{
    class ThreadProcessTianJinWeiYe
    {
        private Boolean running = false;
        private Thread threadProcess;
        private static Mutex mut = new Mutex();
        private string strConnectString = string.Empty;
      
        private string ThreadName = "Save  File For TianJinWeiYe";
        private string _strConfigNamePath = "";
        private InspectConfig.ConfigData _configData;
        public ThreadProcessTianJinWeiYe(InspectConfig.ConfigData ConfigData)
        {
            //lin 20190813
            //this.strConnectString = "server=127.0.0.1;user id=root;database=spidb;Charset=utf8";//Properties.Settings.Default.MySQLConnect;         
            this.strConnectString =  WSClnt.PubStaticParam._strSPIdbConnectionString;
            this._configData = ConfigData;
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
            //bu_Peng bu_p = new bu_Peng();
            try
            {
                if (this.running == true)
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
                    if (!File.Exists(strLastAppsettingTmpFile))
                    {
                        FileStream fsAppsetting = new FileStream(strLastAppsettingTmpFile, FileMode.Create);
                        string zeroAPPfile = @"D:\EYSPI\Bin\AutoAPPConfig\" + min + ".bin";
                        if (File.Exists(zeroAPPfile))
                        {
                            appSettingHandle.Read(zeroAPPfile, _strConfigNamePath);
                        }
                        StreamWriter swAppsetting = new StreamWriter(fsAppsetting, System.Text.Encoding.Default);
                        swAppsetting.Write(min);
                        swAppsetting.Close();
                        fsAppsetting.Close();
                    }
                    else
                    {
                        FileStream fsAppsetting = new FileStream(strLastAppsettingTmpFile, FileMode.Create);
                        string nestAPPfile = @"D:\EYSPI\Bin\AutoAPPConfig\" + max + ".bin";
                        if (File.Exists(nestAPPfile))
                        {
                            appSettingHandle.Read(nestAPPfile,_strConfigNamePath);
                        }
                        StreamWriter swAppsetting = new StreamWriter(fsAppsetting, System.Text.Encoding.Default);
                        swAppsetting.Write(max);
                        swAppsetting.Close();
                        fsAppsetting.Close();
                    }
                    string nextPcbId = string.Empty;
                    //用来记录上一块PCB的临时文件; ;
                    if (File.Exists(strLastPcbFile))
                    {

                    }
                    else
                    {
                        //如果不存在PCBfile 则说明此时系统第一次开启用，则用appsetting中pcbID; 
                        using (FileStream fsPcbFile = new FileStream(strLastPcbFile, FileMode.Create))
                        {
                            StreamWriter swPcbFile = new StreamWriter(fsPcbFile, System.Text.Encoding.Default);
                            swPcbFile.Write(min - 1);
                            swPcbFile.Close();
                        }
                    }
                    //需要临时加时间---
                    int timer = appSettingHandle._appSettingData.stDataExpVT.IntervalSecond;
                    bu_Peng.SaveDataForTianJinWeiYeMes(strConnectString, strLastPcbPath, strLastPcbFile, strLastAppsettingTmpFile, strAppsettingPath, appSettingHandle, timer, this._strConfigNamePath);
                }

            }
            catch (Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
            }
            finally
            {
                //log.WriteLog("线程TianJinWeiYe结束!", ThreadName);
            }
        }

    }
}
