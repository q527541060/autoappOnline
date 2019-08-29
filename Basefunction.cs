using AppLayerLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text; 
using System.Threading;
using System.Windows.Forms;
using System.Data;
using ImgCSCoreIM;
namespace AutoAPP
{  
    //do some processing for data export
    // bPadIsSkip must be written by to Q.F
    public partial class Basefunction
    {

        public bool IsFileInUse(string fileName)
        {
            bool inUse = true;

            FileStream fs = null;
            try
            {
                if (File.Exists(fileName) == true)
                {
                    fs = new FileStream(fileName, FileMode.Open, FileAccess.Read,

                    FileShare.None);
                }

                inUse = false;
            }
            catch(Exception ex)
            {
                //continue;
                return inUse;
            }
            finally
            {
                if (fs != null)

                    fs.Close();
            }
            return inUse;//true表示正在使用,false没有使用  
        }

        public String[] GetFiles(String AstrDir, String AstrExt)
        {
            return Directory.GetFiles(AstrDir, "*" + AstrExt).Where(t => t.ToLower().EndsWith(AstrExt.ToLower())).ToArray();

        }
        public int DirCheck(ref String AstrDir)
        {
            //InspectMainLib.InspectAlgCore.DirChkAndCrt(_logRecordDir);
            try
            {
                if (String.IsNullOrEmpty(AstrDir))
                    return -1;
                if (System.IO.Directory.Exists(AstrDir) == false)
                    System.IO.Directory.CreateDirectory(AstrDir);

                if (!String.Equals(AstrDir.Substring(AstrDir.Length - 1, 1), "\\"))
                    AstrDir += "\\";

                return 0;
            }
            catch (System.Exception ex)
            {
                return -1;
            }


        }
        
        // delete  days data
        public void DeleteDataGenDays(string AstrDirPath, int AiCount, int AiUIDay)
        {
            //string strMsg = string.Empty;
            int iTmp = 0;
            if (string.IsNullOrEmpty(AstrDirPath) == false)
            {
                if (Directory.Exists(AstrDirPath))
                {
                    //get files
                    try
                    {
                        IEnumerable<string> lstDir = Directory.GetDirectories(AstrDirPath).Where(p => Directory.GetCreationTime(p) < DateTime.Now.AddDays(-AiUIDay));
                        if (lstDir != null && lstDir.Count() > 0)
                        {
                            //form spc tmpFile
                            string strTmpFile = @"D:\EYSPI\DataExport\SpcAutoAppFolder";
                            string strFovPath = string.Empty;
                            if (Directory.Exists(strTmpFile))
                            {
                                DirectoryInfo info = new DirectoryInfo(strTmpFile);
                                FileInfo newestFile = info.GetFiles("*.txt").OrderBy(n => n.LastWriteTime).First();
                                if (newestFile != null)
                                {
                                    if (newestFile.Name.Contains('-') && newestFile.Name.Contains('+'))
                                    {
                                        strFovPath = newestFile.Name.Replace("+", ":").Replace("-", "\\").Replace(".txt", "");
                                    }
                                }
                            }


                            if (lstDir.Count() < AiCount)
                            {
                                foreach (string strImagePath in lstDir)
                                {
                                    if (!string.IsNullOrEmpty(strImagePath))
                                    {
                                        if (strImagePath == strFovPath)
                                        {
                                            continue;
                                        }
                                        Directory.Delete(strImagePath, true);
                                    }
                                }
                            }
                            else
                            {
                                foreach (string strImagePath in lstDir)
                                {
                                    iTmp++;
                                    if (!string.IsNullOrEmpty(strImagePath))
                                    {
                                        if (strImagePath == strFovPath)
                                        {
                                            continue;
                                        }
                                        if (iTmp <= AiCount)
                                        {
                                            Directory.Delete(strImagePath, true);
                                        }
                                    }
                                }
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        //return strMsg;
                        return;
                    }
                }


            }
            //return strMsg;
        }


        //delete autoAPP config
        public void DeleteDataAutoAPPConfigBin(string AstrDirPath, int AiSencond)
        {
            if (string.IsNullOrEmpty(AstrDirPath) == false)
            {
                if (Directory.Exists(AstrDirPath))
                {
                    //get files
                    try
                    {
                        //Where(p => File.GetCreationTime(p) < DateTime.Now.AddSeconds(-AiSencond) )
                        IEnumerable<string> lstDir = Directory.GetFiles(AstrDirPath).Where(p=> p.Contains(".bin")).OrderBy(k => File.GetCreationTime(k));
                        if (lstDir != null && lstDir.Count() > 0)
                        {
                            int i = 0;
                            foreach(string str in lstDir)
                            {
                                i++;
                                if (File.Exists(str))
                                {
                                    if (i == lstDir.Count())
                                    {
                                        continue;
                                    }
                                    File.Delete(str);
                                }
                                
                            }
                        }
                        IEnumerable<string> lstDirFolder = Directory.GetDirectories(AstrDirPath);
                        if (lstDirFolder != null && lstDirFolder.Count() > 0)
                        {
                            foreach (string dir in lstDirFolder)
                            {
                                if (Directory.Exists(dir))
                                {
                                    Directory.Delete(dir,true);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return;
                    }
                }
            }
        }
    }
}
