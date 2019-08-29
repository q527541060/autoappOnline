﻿using System;
using System.Data;
using System.Threading;
using System.IO;


namespace AutoAPP
{
    class ThreadProcessDefaultTools
    {
        private Boolean running = false;
        //private DateTime dtStartTime = new DateTime(2011, 11, 11);
        //private DateTime dtEndTime = new DateTime(2011, 11, 11);
        private string strCDPath = string.Empty;
        //private int intSectionTime = 10;
        private Thread threadProcess;
        private string configPath = string.Empty;
        private static Mutex mut = new Mutex();
        private string strConnectString = string.Empty;
 
        private string ThreadName = "Save File For DefaultTools";
        private string _strRealConfigPath = "";

        public ThreadProcessDefaultTools(string strConfigPath, string AstrRealConfigPath)
        {
            //this.dtStartTime = dtStartTime;
            //this.dtEndTime = dtEndTime;
            //lin 20190813
           // this.strConnectString = "server=127.0.0.1;user id=root;database=spidb;Charset=utf8";//Properties.Settings.Default.MySQLConnect;
            this.strConnectString = WSClnt.PubStaticParam._strSPIdbConnectionString;
            //this.strCDPath = AstrCDPath;          
             this.configPath = strConfigPath;
            _strRealConfigPath = AstrRealConfigPath;
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
                            else
                            {
                                File.Delete(file);
                            }
                        }
                    }
                    //add Peng 20180523  系统第一次载入或者UI升级  如果存在多个bin文件   delete appsetting.bin; 
                    //if (min == 1  && files.Length > 1)
                    //{
                    //    foreach (var file in files)
                    //    {
                    //        var vv = Path.GetFileNameWithoutExtension(file);
                    //        if (vv != "1")
                    //        {
                    //            File.Delete(file);
                    //        }
                    //    }
                    //    if (File.Exists(strLastAppsettingTmpFile))
                    //    {
                    //        File.Delete(strLastAppsettingTmpFile);
                    //    }
                    //    if (File.Exists(strLastPcbFile))
                    //    {
                    //        File.Delete(strLastPcbFile);
                    //    }
                    //}

                    if (!File.Exists(strLastAppsettingTmpFile))
                    {
                        FileStream fsAppsetting = new FileStream(strLastAppsettingTmpFile, FileMode.Create);
                        string zeroAPPfile = @"D:\EYSPI\Bin\AutoAPPConfig\" + min + ".bin";
                        if (File.Exists(zeroAPPfile))
                        {
                            appSettingHandle.Read(zeroAPPfile, _strRealConfigPath);
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
                            appSettingHandle.Read(nestAPPfile, _strRealConfigPath);
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
                        //using (StreamReader srPcbFile = new StreamReader(strLastPcbFile))
                        //{
                        //    string str = "";
                        //    while ((str = srPcbFile.ReadLine()) != null)
                        //    {
                        //        nextPcbId = str;
                        //    }
                        //}
                    }
                    else
                    {
                        //如果不存在PCBfile 则说明此时系统第一次开启用，则用appsetting中pcbID;                       
                        using (FileStream fsPcbFile = new FileStream(strLastPcbFile, FileMode.Create))
                        {                                                       
                             StreamWriter swPcbFile = new StreamWriter(fsPcbFile, System.Text.Encoding.Default);
                             swPcbFile.Write(min-1);                          
                             swPcbFile.Close();
                        }
                    }                    
                    //需要临时加时间---
                    int timer = appSettingHandle._appSettingData.stDataExpVT.IntervalSecond;
                    bu_Peng bp = new bu_Peng();
                    bp.SaveFileForDefaultTools(strConnectString, strLastPcbPath, strLastPcbFile, strLastAppsettingTmpFile, strAppsettingPath, appSettingHandle, timer, this.configPath, appSettingHandle._appSettingData.strDataExpPath, _strRealConfigPath);                    
                }
            }
            catch (Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
            }
            finally
            {
                //log.WriteLog("线程DefaultTools结束!", ThreadName);
            }
        }

    }
}
