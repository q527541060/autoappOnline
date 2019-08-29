using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Windows.Forms;
using EYSPIToAOI;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using AppLayerLib;

namespace AutoAPP
{
    class EYSPIToAOIHelp
    {
        List<APP_SaveOrLoadImgDataInFo> _arrAPP_SaveOrLoadImgDataInFo = new List<APP_SaveOrLoadImgDataInFo>();
        ImgCSCoreIM.ST_SaveOrLoadImgData _stImgData = new ImgCSCoreIM.ST_SaveOrLoadImgData();
        private static String _logRecordDir = @"D:\EYSPI\Bin\SPILogs\Peng\EYSPIToAOI\";
        private String _strDataLinkToAOIDir = "D:\\EYSPI\\DataExport\\LinkToAOI";
        private String _strDataLinkToAOIDirProcessed = "\\Processed";
        private String _strFovImgLinkToAOIDir = "\\FOVImage";

        private String _strDataLinkFrAOIDir = "D:\\EYSPI\\DataExport\\LinkFrAOI";
        private String _strDataLinkFrAOIDirProcessed = "\\Processed";
        private String _strDataExportToAOIDir = "D:\\EYSPI\\DataExport\\ExportToAOI";
        private String _strDataExUIDir = @"D:\EYSPI\DataExport\ExToEyspiToAOI";
        public string _strEYSPIAoiFrSPIDataFilePath = "";

        private const string RS_CONN_X32 = @"Provider=Microsoft.Jet.OleDb.4.0; Data Source = {0};"; //Extended Properties = MaxBufferSize = 2048; "
        private const string RS_CONN_X64 = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source = {0};";
        //private const string RS_CONN_X32 = @"Provider=Microsoft.Jet.OleDb.4.0; Data Source = {0};Jet OLEDB:Database HDR = {1};Extended Properties=\"text;HDR=No;FMT=Delimited\""; //Extended Properties = MaxBufferSize = 2048; "
        //private const string RS_CONN_X64 = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source = {0};Jet OLEDB:Database HDR = {1};";

        private string _strConnectionDB = @"Provider=Microsoft.Jet.OleDb.4.0; Data Source = {0};Jet OLEDB:Database HDR = {1};";
        private bool _bIsOS64Bit = false;
        private const string RS_SQL_SELECT = @" SELECT * FROM [{0}] ";

        //private DataTable _dtFrAOI;
        //private DataTable _dtFrSPI;
        private bool _bReadFirstLine;//whether the first line is column name
        private int _iArrayIDColumnIndex;
        private int _iComponentIDColumnIndex;
        private static int _iProcessorCount = Environment.ProcessorCount;

        public static CThread cthreadProcess;

        private EYSPIToAOI.Basefunction _baseFuc = new EYSPIToAOI.Basefunction();
        private AutoAPP.Basefunction _baseFucXU = new Basefunction();
        private EYSPI.Basefunction _csBaseF = new EYSPI.Basefunction();

        private NumberStyles _style;
        private CultureInfo _culture;
        //private Stopwatch _stopWatch;

        private bool _bStopPressed;
        private bool _bProcessing;
        private FileSystemWatcher _watcher;

        private String RS_SPLIT = ",";
        private String RS_LineEnd = "\r\n";

        //static Api _cmApi = new Api();
        private bool _bUICanStart = false;

        private static bool _bTest = true;

        private String _strSaveExtension;


        private AppMultiLanForEYSPI.AppLanForEYSPI _dict;
        public AppLayerLibForEYSPI.AppSettingDataForEYSPI _appSetting;

        private String _strDateTime;

        private const string RS_ColumnTypeString = "System.String";
        private const string RS_ColumnTypeINT = "System.Int32";
        private const string RS_ColumnTypeDouble = "System.Double";
        private const string RS_FovImgdatFormat = ".imgdat";
        private const string RS_FovImageFormatJPG = ".jpg";


        private readonly string RS_PROJECTNAME = "EYSPIToAOI";
        private readonly string RS_PROJECTNAME_PROCESSED_FILENAME = "EYSPIToAOI_FileName";
        private readonly string RS_PROJECTNAME_REMOVED_FILENAME = "EYSPIToAOI_RemoveFileName";

        private byte[] _arrByUsedFovFlag = new byte[1000];
        private AutoAPP.Basefunction _baseFucc = new Basefunction();
        public void StartProcess( AppSettingData AappSettingData, string ExToEyspiToAOI)
        {
            
                ReadAppSetting();
                DirectoryInfo dirFrAOI = new DirectoryInfo(_strDataLinkFrAOIDir);
                FileInfo[] lstFilesFrAOI = dirFrAOI.GetFiles();
                bool ok = false;
                _strDataExUIDir = ExToEyspiToAOI;
                if (lstFilesFrAOI != null && lstFilesFrAOI.Length > 0)
                {
                    try
                    {
                        System.Threading.Thread.Sleep(100);
                        //log.WriteLog("start:");
                        AppLogHelp.WriteLog(LogFileFormate.EYspiToAOI, " :start");
                        int iCount = lstFilesFrAOI.Length;

                        for (int i = 0; i < iCount; i++)
                        //for (int i = range.Item1; i < range.Item2; i++)
                        {
                            String strFileNameFrAOI = lstFilesFrAOI[i].Name;

                            //String strBarcode = strFileNameFrAOI.Replace(lstFilesFrAOI[i].Extension, "");
                            String strBarcode = GetBarcodeFrFileInfo(lstFilesFrAOI[i]);

                            DirectoryInfo dirFrSPI = new DirectoryInfo(_strDataLinkToAOIDir);
                            FileInfo[] lstFilesFrSPI = null;
                            //if (_appSetting.bAOIFileFrSPI == false)
                            lstFilesFrSPI = dirFrSPI.GetFiles(strBarcode + "_*.csv", SearchOption.TopDirectoryOnly);
                            //else //Q.F.2016.12.09
                            //{
                            //    lstFilesFrSPI = dirFrSPI.GetFiles(strBarcode + ".csv");
                            //    strBarcode = strBarcode.Substring(0, strBarcode.LastIndexOf("_"));
                            //}
                            FileInfo selectedFileInfo = null;
                            if (lstFilesFrSPI != null && lstFilesFrSPI.Length != 0)
                            {
                                String FileName = "";

                                if (_appSetting.bAOIFileFrSPI == false)
                                {
                                    long time = -1;
                                    long tmp = 0;
                                    foreach (FileInfo fileInfo in lstFilesFrSPI)
                                    {
                                        FileName = fileInfo.Name;
                                        FileName = FileName.Replace(fileInfo.Extension, "");
                                        FileName = FileName.Substring(strBarcode.Length + 1);

                                        ok = long.TryParse(FileName, _style, _culture, out tmp);
                                        if (ok == false)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            if (tmp > time)
                                            {
                                                time = tmp;
                                                selectedFileInfo = fileInfo;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    String strTmpFileName = lstFilesFrAOI[i].Name;
                                    strTmpFileName = strTmpFileName.Replace(lstFilesFrAOI[i].Extension, ".csv");
                                    // JUZI
                                    if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                                    {
                                        strTmpFileName = strTmpFileName.Replace(".csv", "");
                                        if (string.IsNullOrEmpty(strTmpFileName) == false)
                                        {
                                            if (strTmpFileName.Split('_').Length > 2)
                                            {
                                                //strTmpFileName = strTmpFileName.Split('_')[2];// +"_" + strTmpFileName.Split('_')[1] + ".csv";
                                            }
                                            else
                                            {
                                                if (File.Exists(lstFilesFrAOI[i].FullName))
                                                {
                                                    string strRealFile = Path.Combine(_appSetting.strToAOIErrorNGPath, lstFilesFrAOI[i].Name);
                                                    File.Copy(lstFilesFrAOI[i].FullName, strRealFile, true);
                                                    Thread.Sleep(200);
                                                    File.Delete(lstFilesFrAOI[i].FullName);
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                    selectedFileInfo = lstFilesFrSPI[0]; //new FileInfo(Path.Combine(_strDataLinkToAOIDir, lstFilesFrSPI[0].FullName));// lstFilesFrSPI[0];
                                }
                                if (IsFileInUse(lstFilesFrAOI[i].FullName) == true)
                                {
                                    //LogRecord(RS_PROJECTNAME, "IsFileInUse : True,File:" + strFileNameFrAOI, false);
                                    continue;
                                }
                                if (Processing(strFileNameFrAOI, lstFilesFrAOI[i], strBarcode, selectedFileInfo, AappSettingData) == -1)
                                {
                                    //StopPress("Start Process Prcessing return -1");
                                    //log.WriteErr("Start Process Prcessing return -1","EYSPIToAOI");
                                    AppLogHelp.WriteLog(LogFileFormate.EYspiToAOI, "Start Process Prcessing return -1  FAILE.. strBarcode=>" + strBarcode);
                                    if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                                    {
                                        string tmpDir = selectedFileInfo.Name.Split('_')[1].Substring(0, 8);
                                        string strFrSpiDataFileDir = string.Empty;
                                        if (Directory.GetDirectories(Path.Combine(_strDataExUIDir), "*_" + strBarcode + "_*") != null
                                            && Directory.GetDirectories(Path.Combine(_strDataExUIDir), "*_" + strBarcode + "_*").Length > 0)
                                        {
                                            string[] arrDir = Directory.GetDirectories(Path.Combine(_strDataExUIDir), "*_" + strBarcode + "_*");
                                            foreach (string dir in arrDir)
                                            {
                                                if (Directory.Exists(@dir))
                                                {
                                                    strFrSpiDataFileDir = dir;
                                                    _strEYSPIAoiFrSPIDataFilePath = strFrSpiDataFileDir;
                                                    Directory.Delete(dir, true);
                                                }
                                            }
                                        }

                                        if (File.Exists(lstFilesFrAOI[i].FullName))
                                        {
                                            string strRealFile = Path.Combine(_appSetting.strToAOIErrorNGPath, lstFilesFrAOI[i].Name);
                                            File.Copy(lstFilesFrAOI[i].FullName, strRealFile, true);
                                            Thread.Sleep(200);
                                            File.Delete(lstFilesFrAOI[i].FullName);
                                        }
                                    }

                                }
                                if (selectedFileInfo != null && File.Exists(selectedFileInfo.FullName))
                                {
                                    string tmpDir = selectedFileInfo.Name.Split('_')[1].Substring(0, 8);
                                    string strFrSpiDataFileDir = string.Empty;
                                    if (Directory.GetDirectories(Path.Combine(_strDataExUIDir), "*_" + strBarcode + "_*") != null
                                        && Directory.GetDirectories(Path.Combine(_strDataExUIDir), "*_" + strBarcode + "_*").Length > 0)
                                    {
                                        string[] arrDir = Directory.GetDirectories(Path.Combine(_strDataExUIDir), "*_" + strBarcode + "_*");
                                        foreach (string dir in arrDir)
                                        {
                                            if (Directory.Exists(@dir))
                                            {
                                                strFrSpiDataFileDir = dir;
                                                _strEYSPIAoiFrSPIDataFilePath = strFrSpiDataFileDir;
                                                Directory.Delete(dir, true);
                                            }
                                        }
                                    }
                                    //log.WriteLog("success.. ", "EYSPIToAOI");
                                    AppLogHelp.WriteLog(LogFileFormate.EYspiToAOI, "success..=>" + selectedFileInfo.FullName);
                                }
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                                GC.Collect();
                            }
                            else
                            {
                                if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                                {
                                    if (File.Exists(lstFilesFrAOI[i].FullName))
                                    {
                                        string strRealFile = Path.Combine(_appSetting.strToAOIErrorNGPath, lstFilesFrAOI[i].Name);
                                        File.Copy(lstFilesFrAOI[i].FullName, strRealFile, true);
                                        Thread.Sleep(200);
                                        File.Delete(lstFilesFrAOI[i].FullName);
                                    }

                                }
                                if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.HOllY1)
                                {
                                    if (File.Exists(lstFilesFrAOI[i].FullName))
                                    {
                                        string strRealFile = Path.Combine(lstFilesFrAOI[i].DirectoryName, "Processed", lstFilesFrAOI[i].Name);
                                        File.Copy(lstFilesFrAOI[i].FullName, strRealFile, true);
                                        Thread.Sleep(200);
                                        File.Delete(lstFilesFrAOI[i].FullName);
                                        //continue;
                                    }
                                }
                                if (selectedFileInfo != null && File.Exists(selectedFileInfo.FullName))
                                {
                                    string tmpDir = selectedFileInfo.Name.Split('_')[1].Substring(0, 8);
                                    string strFrSpiDataFileDir = string.Empty;
                                    if (Directory.GetDirectories(Path.Combine(_strDataExUIDir), tmpDir + "_" + strBarcode + "_*") != null
                                        && Directory.GetDirectories(Path.Combine(_strDataExUIDir), tmpDir + "_" + strBarcode + "_*").Length > 0)
                                    {
                                        string[] arrDir = Directory.GetDirectories(Path.Combine(_strDataExUIDir), tmpDir + "_" + strBarcode + "_*");
                                        foreach (string dir in arrDir)
                                        {
                                            if (Directory.Exists(dir))
                                            {
                                                strFrSpiDataFileDir = dir;
                                                _strEYSPIAoiFrSPIDataFilePath = strFrSpiDataFileDir;
                                                Directory.Delete(dir, true);

                                            }

                                        }
                                    }
                                }
                                continue;
                                //MessageBox.Show(_dict.GetTranslate("No SPI Data") + "!");
                                //LogRecord(RS_PROJECTNAME, "No SPI Data", false);
                                //StopPress("Start Process No SPI Data");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //LogRecord(RS_PROJECTNAME, "Start Process Error:" + ex.ToString(), false);
                        //MessageBox.Show(ex.ToString());
                        //log.WriteErr("请检查 linkFrAOI linkToAOI 文件是否正确! ;  或者请三点照合初始化..处理异常"+ex.Message,"EYSPIToAOI");
                        AppLogHelp.WriteError(LogFileFormate.EYspiToAOI, "请检查 linkFrAOI linkToAOI 文件是否正确! ;  或者请三点照合初始化..处理异常" + ex.Message );
                        //log.WriteErr("错误:" + e.Message, "FOV线程 ReadFovImages");
                        //System.Windows.Forms.MessageBox.Show("FOV线程 ReadFovImages异常__请手动检查ExToFovImage文件夹是否转换成功.." + e.Message);
                        MessageBox.Show("EYSPIToAOI 请检查 linkFrAOI linkToAOI  文件 以及条码是否正确 是否正确! ;  或者请三点照合初始化..处理异常" + ex.Message, "系统提示",
                                                                MessageBoxButtons.OK, MessageBoxIcon.Warning,
                                                                MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        //MessageBox.Show(ex.Message);
                        //StopPress("Start Process Error");
                        return;
                    }
                    finally
                    {
                        _bProcessing = false;
                    }
                    //});

                    //String str = "";
                    //String strFileName = "";
                    //String barcode = "";
                    //foreach (FileInfo fileInfo in lstFilesFrAOI)
                    //{
                    //for (int i = 0; i < iCount; i++)
                    //{
                    //    if (str != "")
                    //        str += " + ";
                    //    strFileName = lstFilesFrAOI[i].Name;
                    //    barcode = strFileName.Replace(lstFilesFrAOI[i].Extension, "");
                    //    str += barcode;


                    //}
                    
                }
                else
                {
                    //MessageBox.Show("No Data From AOI To Process" + "!");
                    // return;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                //GC.Collect();
                //Thread.Sleep(300);

                //  StartWatchOrProcess();

            
            //MessageBox.Show("Processed" + "!");
        }
        private DataTable getCsvData(string pCsvpath, string pCsvname, bool bISFrAOI)
        {
            DataTable dttbRtn = new DataTable();
            StreamReader sr = null;
            String strTmpFileBarcode = string.Empty;
            try
            {
                String line;
                String[] split = null;
                //DataTable table = new DataTable("auto");
                DataRow row = null;
                sr = new StreamReader(pCsvpath + "\\" + pCsvname, System.Text.Encoding.Default);

                if (bISFrAOI == true && _appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                {
                    strTmpFileBarcode = pCsvname.Split('_')[2];
                }
                //创建与数据源对应的数据列 
                if (bISFrAOI == false || _bReadFirstLine == false)
                {
                    line = sr.ReadLine();
                    split = line.Split(',');
                    foreach (String colname in split)
                    {
                        if (String.Equals(colname, "XinFov")
                            || String.Equals(colname, "YinFov")
                            || String.Equals(colname, "PadW")
                            || String.Equals(colname, "PadH")
                          )
                        {
                            dttbRtn.Columns.Add(colname, System.Type.GetType(RS_ColumnTypeINT));
                        }
                        else if (
                            String.Equals(colname, "XinCADmm")
                          || String.Equals(colname, "YinCADmm")
                        )
                        {
                            dttbRtn.Columns.Add(colname, System.Type.GetType(RS_ColumnTypeDouble));
                        }
                        else
                        {
                            dttbRtn.Columns.Add(colname, System.Type.GetType(RS_ColumnTypeString));
                        }
                    }
                }
                else
                {
                    dttbRtn.Columns.Add("ArrayID", System.Type.GetType("System.String"));
                    dttbRtn.Columns.Add("ComponentID", System.Type.GetType("System.String"));
                }
                //将数据填入数据表 
                int j = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    j = 0;
                    row = dttbRtn.NewRow();
                    split = line.Split(',');
                    // add Peng 20181105
                    if (bISFrAOI == true && _appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                    {
                        //Pad Id，Component ID,Result,Volume(%)，Area(%),Height(um)，OffsetX(mm)， OffsetY(mm)，PosX(Pixel) ,PosY(Pixel),SizeX(Pixel),SizeY(Pixel),
                        if (split[1] == strTmpFileBarcode)
                        {
                            continue;
                        }
                        row[0] = split[0];
                        row[1] = split[1];
                        dttbRtn.Rows.Add(row);

                    }
                    else
                    {
                        foreach (String colname in split)
                        {
                            row[j] = @colname;
                            j++;
                        }
                        dttbRtn.Rows.Add(row);
                    }
                }
                //return dsCsvData.Tables[0];
                if (dttbRtn != null && dttbRtn.Rows.Count > 0)
                    return dttbRtn;
                else
                    return null;
            }
            catch (Exception ex)
            {
                //connOleDb.Close();
                //OleCmd.Dispose();
                //OleDa.Dispose();
                //connOleDb.Dispose();
                LogRecord(RS_PROJECTNAME, pCsvname + "\tGetCSVData Error:" + ex.ToString(), false);
                //MessageBox.Show(ex.ToString());

                return null;
            }
            finally
            {
                if (sr != null)
                    sr.Close();

            }
        }

        private int UpdateExportToAOIPath(DataTable AdtFrAOI)
        {
            String strTmp = _strDataExportToAOIDir;
            if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.HOllY)
            {
                DataRow dr = AdtFrAOI.Rows[0];
                strTmp = dr[2].ToString();
            }
            if (System.IO.Directory.Exists(strTmp) == false)
            {
                MessageBox.Show(_dict.GetTranslate("Export Path To AOI Is Error") + "!");
                return -1;
            }
            _strDataExportToAOIDir = strTmp;
            return 0;
        }

        private String GetBarcodeFrFileInfo(FileInfo AFileInfo)
        {
            String strFileNameFrAOI = AFileInfo.Name;
            if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.MingRui)
            {
                return strFileNameFrAOI.Replace(AFileInfo.Extension, "");
            }
            if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.HOllY)
            {
                int iIndex = strFileNameFrAOI.LastIndexOf("_");
                if (iIndex != -1)
                {
                    strFileNameFrAOI = strFileNameFrAOI.Substring(iIndex + 1);
                    return strFileNameFrAOI.Replace(AFileInfo.Extension, "");
                }
                else
                    return "";
            }
            if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.HOllY1)
            {
                int iIndex = strFileNameFrAOI.LastIndexOf("_");
                if (iIndex != -1)
                {
                    strFileNameFrAOI = strFileNameFrAOI.Substring(0, iIndex);
                    return strFileNameFrAOI;
                }
                else
                    return "";
            }
            if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
            {
                int iIndex = strFileNameFrAOI.LastIndexOf("_");
                if (iIndex != -1)
                {
                    strFileNameFrAOI = strFileNameFrAOI.Split('.')[0];
                    strFileNameFrAOI = strFileNameFrAOI.Split('_')[2];
                    return strFileNameFrAOI;
                }
            }
            return "";

        }
       

        private int Processing(String AstrFileNameFrAOI, FileInfo AAOIFileInfo, String AstrBarcode, FileInfo ASPIFileInfo,AppSettingData AappSettingData)
        {
            //Bitmap[] bitMaps = null;
            try
            {
                //LogRecord(RS_PROJECTNAME_PROCESSED_FILENAME, "FileNameFrAOI:" + AstrFileNameFrAOI.ToString() + "\tSPIFile:" + ASPIFileInfo.FullName, false);
                //get data from AOI
                DataTable dtFrAOI = new DataTable();
                dtFrAOI = getCsvData(_strDataLinkFrAOIDir, AstrFileNameFrAOI, true);

                if (dtFrAOI != null)
                {
                    string strAoiTmpTimeName = "";
                    //2015.03.31
                    int iRet = UpdateExportToAOIPath(dtFrAOI);
                    if (iRet == -1)
                    {
                        //log.WriteErr("Function(UpdateExportToAOIPath)");
                        AppLogHelp.WriteError(LogFileFormate.EYspiToAOI, "Function(UpdateExportToAOIPath)");
                        return -1;
                    }
                    String strFileName = ASPIFileInfo.Name;
                    strFileName = strFileName.Replace(ASPIFileInfo.Extension, "");

                    String dir = strFileName;
                    //Q.F.2016.12.09
                    if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.HOllY1)
                        dir = AstrBarcode;
                    String strTime = "";
                    if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                    {
                        if (string.IsNullOrEmpty(strFileName) == false)
                        {
                            strTime = strFileName.Split('_')[1];
                        }
                        if (string.IsNullOrEmpty(AAOIFileInfo.Name) == false)
                        {
                            strAoiTmpTimeName = AAOIFileInfo.Name.Split('_')[1];
                        }
                        dir = AstrFileNameFrAOI;
                    }
                    else
                    {
                        strTime = strFileName.Replace(AstrBarcode + "_", "");
                    }
                    //get fov image
                    int AiFovWidth = 0;
                    int AiFovHeight = 0;
                    string tmpDir = "";
                    if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                    {
                        tmpDir = strAoiTmpTimeName.Substring(0, 8);
                    }
                    else
                    {
                        tmpDir = strTime.Substring(0, 8);
                    }
                    string strFrSpiDataFileDir = string.Empty;
                    if (Directory.GetDirectories(Path.Combine(_strDataExUIDir), "*_" + AstrBarcode.Trim() + "_*") != null
                        && Directory.GetDirectories(Path.Combine(_strDataExUIDir), "*_" + AstrBarcode.Trim() + "_*").Length > 0)
                    {
                        string[] arrStr = Directory.GetDirectories(Path.Combine(_strDataExUIDir), "*_" + AstrBarcode.Trim() + "_*");
                        foreach (string str in arrStr)
                        {
                            strFrSpiDataFileDir = str;
                            _strEYSPIAoiFrSPIDataFilePath = strFrSpiDataFileDir;
                        }
                      
                    }
                    else
                    {
                        //log.WriteErr("GetDirectories  :" + _strDataExUIDir+" null");
                        AppLogHelp.WriteError(LogFileFormate.EYspiToAOI, "GetDirectories  :" + _strDataExUIDir + " null");
                        return -1;
                    }

                   
                    strFileName = ASPIFileInfo.Name;
                    int iBitmapWidth = AiFovWidth;
                    //get data from SPI
                    DataTable dtFrSPI = new DataTable();

                    if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                    {
                        DirectoryInfo info = new DirectoryInfo(_strDataLinkToAOIDir);
                        FileInfo[] files = info.GetFiles(AstrBarcode + "_*.csv");
                        if (files.Length > 0) strFileName = files[0].Name;
                    }
                    dtFrSPI = getCsvData(_strDataLinkToAOIDir, strFileName, false);
                    //ChangeINTtoString(ref dtFrSPI);
                    if (dtFrAOI == null)
                    {
                        //log.WriteErr("dtFrAOI is null");
                        AppLogHelp.WriteError(LogFileFormate.EYspiToAOI, "dtFrAOI is null");
                        return -1;
                    }
                    //dir = _strDataExportToAOIDir + "\\" + AstrBarcode;

                    if (bGenSubDir() == true)
                    {
                        dir = _strDataExportToAOIDir + "\\" + dir;
                        DirChkAndCrt(dir);
                    }
                    else
                    {
                        dir = _strDataExportToAOIDir;
                    }
                    int iCount = dtFrAOI.Rows.Count;
                    bool[] bComponentPass = new bool[iCount];
                    String[] strImgPath = new String[iCount];

                    DataRow dr1 = dtFrAOI.Rows[iCount - 1];
                    if (dr1.IsNull(_iArrayIDColumnIndex))
                        iCount--;

                    if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.HOllY)
                    {
                        System.DateTime dttmNow = System.DateTime.Now;
                        _strDateTime = GetNowDateTimeStr(dttmNow, 0);
                    }
                    //DeleteDir(dir, true);
 
                        //foreach (DataRow dr in dtFrAOI.Rows)
                        DirChkAndCrt(dir);
                        for (int i = 0; i < iCount; i++)
                        {
                            DataRow dr = dtFrAOI.Rows[i];
                            string strComp = dr[_iComponentIDColumnIndex].ToString();
                            if (string.IsNullOrEmpty(strComp) || Char.IsNumber(strComp, 0))//Q.F.2016.12.12 ;;//String.IsNullOrEmpty(strComp) ||
                            {
                                continue;
                            }
                            if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                            {
                                if (i == 0)
                                {
                                    continue;
                                }
                            }
                            //read join data and sort x,y
                            //DataRow[] dr_selected = dtFrSPI.Select("ArrayID = '" + dr[_iArrayIDColumnIndex]
                            //+ "'AND ComponentID '" + dr[_iComponentIDColumnIndex] + "'", "XinCADmm asc , YinCADmm asc");//desc

                            //DataRow[] dr_selected = dtFrSPI.Select("ArrayID = '" + dr[_iArrayIDColumnIndex]
                            //        + "'AND ComponentIDStr = '" + strComp + "'", "XinCADmm asc , YinCADmm asc");//desc
                            DataRow[] dr_selected = dtFrSPI.Select("ArrayID = '" + dr[_iArrayIDColumnIndex]
                              + "'AND ComponentID = '" + strComp + "'", "XinCADmm asc , YinCADmm asc");//desc
                            ClearALLImgData();//Q.F.2019.04.09
                            if (_appSetting.bFaseMode == false)
                            {
                                GetFovImage(strFrSpiDataFileDir, strFileName, dr_selected, out AiFovWidth, out AiFovHeight);//AColorLst = Q.F.2019.04.09
                            }
                            else
                            {
                                ClearTheLastImgData();
                            }
                            String strComponentImgName = "";
                            if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.HOllY)
                            {
                                strComponentImgName = dr[3].ToString();
                            }
                            //Q.F.2016.12.09
                            if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.HOllY1)
                            {///
                                //Q.F.2016.12.12
                                strComponentImgName = "-" + dr[1].ToString() + "-" + dr[0].ToString();//Barcode/-Comp-ArrayID
                                strComponentImgName += "spi.jpg";
                            }
                            if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                            {
                                strComponentImgName = strAoiTmpTimeName + "-" + dr[1].ToString() + "-" + dr[0].ToString();//Barcode/-Comp-ArrayID
                                strComponentImgName += "spi.jpg";
                                // 如果不匹配到数据;
                                //test
                                //dr_selected = null;
                                if (dr_selected != null && dr_selected.Length > 0)
                                {
                                    if (_appSetting.bFaseMode == false)
                                    {
                                        GenDataToAOI(strFrSpiDataFileDir, strFileName, dr_selected, dir, _arrAPP_SaveOrLoadImgDataInFo, iBitmapWidth, strComponentImgName, out bComponentPass[i], out strImgPath[i], AiFovWidth, AiFovHeight, AappSettingData);
                                    }
                                    else
                                    {
                                        GenDataToAOI(strFrSpiDataFileDir, strFileName, dr_selected, dir, _arrAPP_SaveOrLoadImgDataInFo, iBitmapWidth, strComponentImgName, out bComponentPass[i], out strImgPath[i], AiFovWidth, AiFovHeight, AappSettingData,true);
                                    }
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(_appSetting.strToAOIErrorNGPath) == false)
                                    {
                                        string strNGFilePath = Path.Combine(_appSetting.strToAOIErrorNGPath, AstrFileNameFrAOI);
                                        if (File.Exists(AAOIFileInfo.FullName) && File.Exists(strNGFilePath) == false)
                                        {
                                            File.Copy(AAOIFileInfo.FullName, strNGFilePath, true);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (_appSetting.bFaseMode == false)
                                {
                                    GenDataToAOI(strFrSpiDataFileDir, strFileName, dr_selected, dir, _arrAPP_SaveOrLoadImgDataInFo, iBitmapWidth, strComponentImgName, out bComponentPass[i], out strImgPath[i], AiFovWidth, AiFovHeight, AappSettingData);
                                }
                                else
                                {
                                    GenDataToAOI(strFrSpiDataFileDir, strFileName, dr_selected, dir, _arrAPP_SaveOrLoadImgDataInFo, iBitmapWidth, strComponentImgName, out bComponentPass[i], out strImgPath[i], AiFovWidth, AiFovHeight, AappSettingData, true);
                                }
                            }
                        }
                    
                    if (_appSetting.bExportComponentInfo == true)
                    {
                        StringBuilder sb = new StringBuilder();
                        if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                        {
                            strTime = strAoiTmpTimeName;
                        }
                        sb.Append(
                           strTime + ","
                            + AstrBarcode + ","
                            + iCount + RS_LineEnd
                                        );
                        for (int i = 0; i < iCount; i++)
                        {
                            if (string.IsNullOrEmpty(strImgPath[i]))
                            {
                                continue;
                            }
                            DataRow dr = dtFrAOI.Rows[i];
                            sb.Append(
                                dr[_iComponentIDColumnIndex] + RS_SPLIT
                                + bComponentPass[i] + RS_SPLIT
                                + dr[_iArrayIDColumnIndex] + RS_SPLIT
                                + strImgPath[i] + RS_LineEnd
                                        );
                        }
                        //File.WriteAllText(strFileName + ".csv", sb.ToString());
                        strFileName = strFileName.Replace(".csv", "");
                        File.WriteAllText(_strDataExportToAOIDir + "\\" + strFileName + _strSaveExtension, sb.ToString());
                    }
                    //_arrAPP_SaveOrLoadImgDataInFo = null; ;
                    DeleteEmptyDir(dir);
                    //MessageBox.Show("Processed" + "!");
                }
                //else
                //    MessageBox.Show("No Data From AOI To Process" + "!");
                if (File.Exists(AAOIFileInfo.FullName))
                {
                    if (DirChkAndCrt(_strDataLinkFrAOIDirProcessed) != -1)
                    {
                        String strDestFileName = Path.Combine(_strDataLinkFrAOIDirProcessed, AAOIFileInfo.Name);
                        if (!IsFileInUse(strDestFileName))
                        {
                            if (File.Exists(strDestFileName))
                            {
                                File.Delete(strDestFileName);
                            }
                            // if (!String.Equals(Path.GetFileName(AAOIFileInfo.FullName), Path.GetFileName(strDestFileName)))
                            { File.Copy(AAOIFileInfo.FullName, strDestFileName, true); }
                        }
                        else
                        {
                            //LogRecord(RS_PROJECTNAME, "IsFileInUse:" + strDestFileName, false);
                        }
                        File.Delete(AAOIFileInfo.FullName);
                    }
                    else
                    {
                        //LogRecord(RS_PROJECTNAME, "Could Not Create Folder:" + _strDataLinkFrAOIDirProcessed, false);
                        //StopPress("Processing Error");
                        MessageBox.Show("Could Not Create Folder:" + _strDataLinkFrAOIDirProcessed);
                    }
                }
                return 0;
            }
            catch (System.Exception ex)
            {
                throw ex;
                //LogRecord(RS_PROJECTNAME, "Processing Error:" + ex.ToString(), false);
            }
            finally
            {
                //if (bitMaps != null)
                //{
                //    for (int i = 0; i < bitMaps.Length; ++i)
                //    {
                //        if (bitMaps[i] != null)
                //            bitMaps[i].Dispose();
                //    }
                //}
            }
        }
       
        private String GetNowDateTimeStr(DateTime AdttmNow, int AiMode)
        {
            String str = "";
            if (AiMode == 0)
            {
                str = AdttmNow.Hour.ToString("00")
                    + AdttmNow.Minute.ToString("00")
                    + AdttmNow.Second.ToString("00");
            }
            return str;
        }

        private void DeleteEmptyDir(String AstrDir)
        {
            if (System.IO.Directory.Exists(AstrDir) == false)
                return;
            String[] files = Directory.GetFiles(AstrDir);
            if (files == null || files.Length == 0)
                Directory.Delete(AstrDir);

        }
        private int DirChkAndCrt(String AstrDir)
        {

            try
            {
                if (System.IO.Directory.Exists(AstrDir) == false)
                    System.IO.Directory.CreateDirectory(AstrDir);
            }
            catch (System.Exception ex)
            {
                return -1;
            }
            return 0;

        }

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
            catch
            {

            }
            finally
            {
                if (fs != null)

                    fs.Close();
            }
            return inUse;//true表示正在使用,false没有使用  
        }
        private bool bGenSubDir()
        {
            if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.HOllY)
            {
                return false;
            }

            return true;
        }
        int iRectVCrrX = 0;
        int iRectVCrrY = 0;
        int iRectVCrrW = 0;
        int iRectVCrrH = 0;
        int iRectVX = 0;
        int iRectVY = 0;
        int iRectVW = 0;
        int iRectVH = 0;

        int iRectBX = 0;
        int iRectBY = 0;
        int iRectBW = 0;
        int iRectBH = 0;

        int iRectAX = 0;
        int iRectAY = 0;
        int iRectAW = 0;
        int iRectAH = 0;
        ImgCSCoreIM.ST_SaveOrLoadImgData stSave = new ImgCSCoreIM.ST_SaveOrLoadImgData();
        private void GenDataToAOI(string strFrSpiDataFileDir, string strFileName, DataRow[] Adr, String AstrDirName, List<APP_SaveOrLoadImgDataInFo> AColorLst,
            int AiBitmapWidth,  String AComponentImgName, out bool AbComponentPass, out String AstrImagePath, int AiFovWidth, int AiFovHeight,AppSettingData AappSettingData)//, Bitmap[] AbitMaps)
        {
            AbComponentPass = true;
            AstrImagePath = "";
            string strAstrDirNameForBlueWey = string.Empty;
            if (Adr == null || Adr.Length == 0)
                return;
            try
            {
                //System.Collections.ArrayList list = new System.Collections.ArrayList();
                ImgCSCoreIM.ColorFactorParams stColorFactors = AappSettingData.stColorFactorParams;
                int iCount = Adr.Length;
                float fTmp = 0.1f;
                double dPixelXum = (double)Convert.ToDouble(Adr[0]["PixelXum"]);
                double dPixelYum = (double)Convert.ToDouble(Adr[0]["PixelYum"]);
                int iMargin = (int)Math.Ceiling(dPixelXum * (int.Parse(_appSetting.strtBarImage) * fTmp));
                int[] iXinFovs = new int[iCount];
                int[] iYinFovs = new int[iCount];
                int[] iPadWidths = new int[iCount];
                int[] iPadHeights = new int[iCount];
            
                int iFovNo = 0;
                double dXinCADmm = 0;
                double dYinCADmm = 0;

                double dMinW = 10000;//// (double)Convert.ToDouble(Adr[0]["XinCADmm"]);
                double dMaxW = -10000;// (double)Convert.ToDouble(Adr[iCount - 1]["XinCADmm"]) + ((int)Convert.ToDouble(Adr[iCount - 1]["PadW"]) / 1000.0) * dPixelXum;
                double dMinH = 10000;// (double)Convert.ToDouble(Adr[0]["YinCADmm"]);
                double dMaxH = -100000;
                double dTmpW = 0;
                double dTmpH = 0;
                short shColorPer = 0;
                ////Q.F.2019.04.09
                //for (int i = 0; i < iCount; i++)
                //{
                //    iFovNo = (int)Convert.ToInt32(Adr[i]["FovNo"]);
                //    _arrByUsedFovFlag[iFovNo - 1] = 1;
                //}

                //if (stColorFactors.byRGB2DFactorR != 100)
                //{
                //    //fFactor = stColorFactors.byRGB2DFactorR * 0.01f;
                //    shColorPer = (short)(stColorFactors.byRGB2DFactorR * 1.28);
                //    for (int i = 0; i < AColorLst.Length; ++i)
                //    {
                //        //for (int k = 0; k < ; k++)
                //        //{

                //        //}
                //        AColorLst[i].stImgData.byImgDataR
                //            //R1[i] = (byte)(AFovs[a]._rgbImg[0][i] * fFactor);
                //            _imgByte2DRFov[i] = (byte)((AFovs[iIndex].stImgData.byImgDataR[i] * shColorPer) >> 7);
                //        // R1[i] = aTmp[AFovs[a]._rgbImg[0][i]];
                //    }
                //}
                //else
                //{
                //    //Marshal.Copy((IntPtr)((void*)AFovs[a]._rgbImg[0]), R1, 0,
                //    //   lImageLength);
                //    Array.Copy(AFovs[iIndex].stImgData.byImgDataR, _imgByte2DRFov, lImageLength);
                //}

                float fUM2MM = 0.001f;
                
                //for (int i = 0; i < iCount; i++)
                //{
                //    iFovNo = (int)Convert.ToInt32(Adr[i]["FovNo"]);
                //    //if (list.Contains(iFovNo) == false)
                //    //{
                //    //    list.Add(iFovNo);
                //    //}                    
                //}
               // GetFovImage(strFrSpiDataFileDir, strFileName,1, out AiFovWidth, out AiFovHeight);//AColorLst = Q.F.2019.04.09
                AiBitmapWidth = AiFovWidth;
                for (int i = 0; i < iCount; i++)
                {
                    iXinFovs[i] = (int)Convert.ToInt32(Adr[i]["XinFov"]);
                    iYinFovs[i] = (int)Convert.ToInt32(Adr[i]["YinFov"]);
                    iPadWidths[i] = (int)Convert.ToInt32(Adr[i]["PadW"]);
                    iPadHeights[i] = (int)Convert.ToInt32(Adr[i]["PadH"]);

                    iFovNo = (int)Convert.ToInt32(Adr[i]["FovNo"]);
                    
                    dXinCADmm = (double)Convert.ToDouble(Adr[i]["XinCADmm"]);
                    dYinCADmm = (double)Convert.ToDouble(Adr[i]["YinCADmm"]);

                    iRectVCrrX = (int)Convert.ToInt32(Adr[i]["RectVCrrX"]);
                    iRectVCrrY = (int)Convert.ToInt32(Adr[i]["RectVCrrY"]);
                    iRectVCrrW = (int)Convert.ToInt32(Adr[i]["RectVCrrW"]);
                    iRectVCrrH = (int)Convert.ToInt32(Adr[i]["RectVCrrH"]);
                    iRectVX = (int)Convert.ToInt32(Adr[i]["RectVX"]);
                    iRectVY = (int)Convert.ToInt32(Adr[i]["RectVY"]);
                    iRectVW = (int)Convert.ToInt32(Adr[i]["RectVW"]);
                    iRectVH = (int)Convert.ToInt32(Adr[i]["RectVH"]);

                    iRectBX = (int)Convert.ToInt32(Adr[i]["RectBX"]);
                    iRectBY = (int)Convert.ToInt32(Adr[i]["RectBY"]);
                    iRectBW = (int)Convert.ToInt32(Adr[i]["RectBW"]);
                    iRectBH = (int)Convert.ToInt32(Adr[i]["RectBH"]);

                    iRectAX = (int)Convert.ToInt32(Adr[i]["RectAX"]);
                    iRectAY = (int)Convert.ToInt32(Adr[i]["RectAY"]);
                    iRectAW = (int)Convert.ToInt32(Adr[i]["RectAW"]);
                    iRectAH = (int)Convert.ToInt32(Adr[i]["RectAH"]);
                    int iPadAI = (int)Convert.ToInt32(Adr[i]["PadID"]);

                    iXinFovs[i] = iRectVCrrX - iMargin < 0 ? 0 : iRectVCrrX - iMargin;
                    iYinFovs[i] = iRectVCrrY - iMargin < 0 ? 0 : iRectVCrrY - iMargin;
                    iPadWidths[i] = iRectVCrrW + iMargin * 2;
                    iPadHeights[i] = iRectVCrrH + iMargin * 2;

                    iXinFovs[i] = iRectVX - iMargin < 0 ? 0 : iRectVX - iMargin;
                    iYinFovs[i] = iRectVY - iMargin < 0 ? 0 : iRectVY - iMargin;
                    iPadWidths[i] = iRectVW + iMargin * 2;
                    iPadHeights[i] = iRectVH + iMargin * 2;

                    if (iXinFovs[i] + iPadWidths[i] > AiFovWidth)
                    {
                        iXinFovs[i] -= (AiFovWidth - iPadWidths[i]);
                    }
                    if (iYinFovs[i] + iPadHeights[i] > AiFovHeight)
                    {
                        iYinFovs[i] -= (AiFovHeight - iPadHeights[i]);
                    }
                    dTmpW = (dXinCADmm - Convert.ToDouble(iPadWidths[i]) * 0.5f * fUM2MM * dPixelXum);//Math.Floor
                    if (dTmpW < dMinW)
                        dMinW = dTmpW;
                    dTmpW = Math.Ceiling((double)Convert.ToDouble(dXinCADmm) + Convert.ToDouble(iPadWidths[i]) * 0.5f * fUM2MM * dPixelXum);
                    if (dTmpW > dMaxW)
                        dMaxW = dTmpW;

                    dTmpH = Math.Floor((double)Convert.ToDouble(dYinCADmm) - Convert.ToDouble(iPadHeights[i]) * 0.5f * fUM2MM * dPixelYum);
                    if (dTmpH < dMinH)
                        dMinH = dTmpH;

                    dTmpH = Math.Ceiling((double)Convert.ToDouble(dYinCADmm) + Convert.ToDouble(iPadHeights[i]) * 0.5f * fUM2MM * dPixelYum);
                    if (dTmpH > dMaxH)
                        dMaxH = dTmpH;
                }
                //cal concat image size

                int iWidth = (int)Math.Ceiling((dMaxW - dMinW) / dPixelXum * 1000);
                int iHeight = (int)Math.Ceiling((dMaxH - dMinH) / dPixelYum * 1000);
                //<-;

                int iNewWidth = iWidth * int.Parse(_appSetting.strtBarImage);
                int iNewHeight = iHeight * int.Parse(_appSetting.strtBarImage);
                if (iNewHeight == 0)
                {
                    iNewHeight = iHeight;
                }
                if (iNewWidth == 0)
                {
                    iNewWidth = iWidth;
                }
                //get the Path
                if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                {
                    if (string.IsNullOrEmpty(AstrDirName) == false)
                    {
                        string str = AstrDirName.Split('_')[1];
                        DateTime dtTime = DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);

                        if (dtTime.Month.ToString().Length >= 2)
                        {
                            strAstrDirNameForBlueWey = Path.Combine(_appSetting.strToAOIPath, dtTime.Year + "", dtTime.Month + "", dtTime.ToString("yyyyMMdd"));
                        }
                        else
                        {
                            strAstrDirNameForBlueWey = Path.Combine(_appSetting.strToAOIPath, dtTime.Year + "", "0" + dtTime.Month + "", dtTime.ToString("yyyyMMdd"));
                        }

                        if (Directory.Exists(strAstrDirNameForBlueWey) == false)
                        {
                            Directory.CreateDirectory(strAstrDirNameForBlueWey);
                        }
                    }
                }
                //Bitmap bitMap = null;// = new Bitmap(iWidth, iHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                int iXinBitMap = 0;
                int iYinBitMap = 0;
                Color color;
                int iIndex = 0;
                byte[] r = new byte[iWidth * iHeight];
                byte[] g = new byte[iWidth * iHeight];
                byte[] b = new byte[iWidth * iHeight];
                Array.Clear(r, 0, iWidth * iHeight);
                Array.Clear(g, 0, iWidth * iHeight);
                Array.Clear(b, 0, iWidth * iHeight);
                double dValue = 0;
                //ImgCSCoreIM.ColorFactorParams stColorFactors = AappSettingData.stColorFactorParams;
                int iRGBInFovIndex = 0;
                //bool bPass = true;

                for (int i = 0; i < iCount; i++)
                {
                    iFovNo = (int)Convert.ToInt32(Adr[i]["FovNo"]);
                    //if (list.Contains(iFovNo) == false)
                    //{
                    //    continue;
                    //}
                    //if (_arrByUsedFovFlag[iFovNo - 1] == 1)
                    //{



                        dXinCADmm = (double)Convert.ToDouble(Adr[i]["XinCADmm"]);
                        dYinCADmm = (double)Convert.ToDouble(Adr[i]["YinCADmm"]);

                        int iPadAI = (int)Convert.ToInt32(Adr[i]["PadID"]);

                        dValue = dXinCADmm - iPadWidths[i] * 0.5f * fUM2MM * dPixelXum - dMinW;
                        iXinBitMap = (int)(Math.Floor(dValue * 1000 / dPixelXum));
                        dValue = dYinCADmm - iPadHeights[i] * 0.5f * fUM2MM * dPixelYum - dMinH;
                        iYinBitMap = (int)(Math.Floor(dValue * 1000 / dPixelYum));

                        for (int y = iYinBitMap; y < iYinBitMap + iPadHeights[i]; y++)
                        {


                            iIndex = y * iWidth + iXinBitMap;

                            if (iIndex < 0)
                            {
                                iIndex = 0;
                            }
                            if (iIndex > AColorLst[iFovNo - 1].stImgData.byImgDataR.Length - 1)
                            {
                                iIndex = AColorLst[iFovNo - 1].stImgData.byImgDataR.Length - 1;
                            }

                            //if (AColorLst[iFovNo - 1].stImgData.byImgDataR.Length > 0)
                            //{
                            //    for (int l = 0; l < AColorLst[iFovNo - 1].stImgData.byImgDataR.Length; l++)
                            //    {                                                                           //(byte)((AFovs[iIndex].stImgData.byImgDataR[i] * shColorPer) >> 7)
                            //        AColorLst[iFovNo - 1].stImgData.byImgDataR[l] = (byte)((AColorLst[iFovNo - 1].stImgData.byImgDataR[l] * (short)(30 * 1.28)) >> 7);
                            //        //AColorLst[iFovNo - 1].stImgData.byImgDataG[l] = (byte)((AColorLst[iFovNo - 1].stImgData.byImgDataG[l] * (short)(30 * 1.28)) >> 7);
                            //        //AColorLst[iFovNo - 1].stImgData.byImgDataB[l] = (byte)((AColorLst[iFovNo - 1].stImgData.byImgDataB[l] * (short)(30 * 1.28)) >> 7);


                            //    }
                            //}

                            iRGBInFovIndex = (y - iYinBitMap + iYinFovs[i]) * AiBitmapWidth + (iXinFovs[i]);
                            Array.Copy(AColorLst[iFovNo - 1].stImgData.byImgDataG, iRGBInFovIndex, g, iIndex, iPadWidths[i]);
                            Array.Copy(AColorLst[iFovNo - 1].stImgData.byImgDataR, iRGBInFovIndex, r, iIndex, iPadWidths[i]);
                            Array.Copy(AColorLst[iFovNo - 1].stImgData.byImgDataB, iRGBInFovIndex, b, iIndex, iPadWidths[i]);
                            //Array.Copy(AColorLst[iFovNo - 1].stImgData.byImgDataB, iRGBInFovIndex, b, iIndex, iPadWidths[i]);
                            //if (stColorFactors.byRGB2DFactorR != 100)
                            //{
                            //    //fFactor = stColorFactors.byRGB2DFactorR * 0.01f;
                            //    shColorPer = (short)(stColorFactors.byRGB2DFactorR * 1.28);
                            //    for (i = 0; i < lImageLength; ++i)
                            //    {
                            //        //R1[i] = (byte)(AFovs[a]._rgbImg[0][i] * fFactor);
                            //        _imgByte2DRFov[i] = (byte)((AFovs[iIndex].stImgData.byImgDataR[i] * shColorPer) >> 7);
                            //        // R1[i] = aTmp[AFovs[a]._rgbImg[0][i]];
                            //    }
                            //}
                            //else
                            //{
                            //    //Marshal.Copy((IntPtr)((void*)AFovs[a]._rgbImg[0]), R1, 0,
                            //    //   lImageLength);
                            //    Array.Copy(AFovs[iIndex].stImgData.byImgDataR, _imgByte2DRFov, lImageLength);
                            //}


                        }


                        String str = Adr[i]["JudgeRes"].ToString();
                        if (str.Equals("Pass"))
                            AbComponentPass = false;
                        if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                        {
                            #region
                            string strTmpFile = Path.Combine(strAstrDirNameForBlueWey, AComponentImgName.Split('-')[0] + "spi.log");
                            if (File.Exists(strTmpFile) == false)
                            {
                                StringBuilder strPld = new StringBuilder();
                                // spidb data server
                                string strTmpBarcode = string.Empty;
                                if (File.Exists(AstrDirName))
                                {
                                    strTmpBarcode = Path.GetFileNameWithoutExtension(AstrDirName);
                                }
                                else
                                {
                                    strTmpBarcode = AstrDirName.Split('\\').Last();
                                }
                                //string 
                                //if()
                                if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                                {
                                    strTmpBarcode = strTmpBarcode.Split('_').Last();
                                }
                                else
                                {
                                    strTmpBarcode = strTmpBarcode.Split('_').First();
                                }
                                string strSql = "SELECT job.JobName,pcb.StartTime,pcb.Result FROM spidb.tbboard as pcb,spidb.TBJobInfo as job WHERE pcb.PCBBarcode = '" + strTmpBarcode + "' AND job.SerNo = pcb.JobIndex;";

                                DataBeanEx.MySQLBean mysqlBean = new DataBeanEx.MySQLBean();
                                DataTable dt = mysqlBean.GetDataTableFrSQL(strSql);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    string strTpResult = string.Empty;
                                    if (dt.Rows[0][2].ToString() == "2")
                                    {
                                        strTpResult = "PASS";
                                    }
                                    else if (dt.Rows[0][2].ToString() == "0")
                                    {
                                        strTpResult = "GOOD";
                                    }
                                    else
                                    {
                                        strTpResult = "NG";
                                    }

                                    strPld.Append("SinicTek" + RS_SPLIT +
                                        dt.Rows[0][0].ToString() + RS_SPLIT + RS_SPLIT + RS_SPLIT +
                                        ((DateTime)dt.Rows[0][1]).ToString("yyyyMMddHHmmss") + RS_SPLIT + RS_SPLIT + RS_SPLIT +
                                        strTpResult + RS_SPLIT + "");
                                    File.WriteAllText(strTmpFile, strPld.ToString(), Encoding.Default);
                                }
                            }
                            #endregion
                        }
                    }

                    try
                    {
                        //Q.F.2019.03.29
                        byte byRGB2DFactorR = 30;
                        shColorPer = 0;
                        int iImgLength = r.Length;
                        if (byRGB2DFactorR != 100)
                        {

                            shColorPer = (short)(byRGB2DFactorR * 1.28);
                            for (int i = 0; i < iImgLength; ++i)
                            {
                                r[i] = (byte)((r[i] * shColorPer) >> 7);
                            }
                        }


                        stSave.iImgFormat = 1;
                        stSave.bUseOpcv = false;
                        stSave.byImgDataR = r;
                        stSave.byImgDataG = g;
                        stSave.byImgDataB = b;
                        stSave.iWidth = iWidth;
                        stSave.iHeight = iHeight;

                    }
                    catch (System.Exception ex)
                    {
                        //bitMap = null;
                        //MessageBox.Show("Gen Bitmap Error:" + ex.ToString());
                        return;
                        //StopPress("Gen Bitmap Error:" + ex.ToString());
                    }
                    if (true)
                    {
                        strFileName = "";
                        String strTmp = "";
                        //int iComponentID = (int)Adr[0]["ComponentID"];
                        if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.MingRui)
                        {
                            strFileName = AstrDirName + "\\" + Adr[0]["ArrayID"].ToString() + "_" + Adr[0]["ComponentID"].ToString();
                            AstrImagePath = strFileName + ".Jpeg";
                            //bitMap.Save(AstrImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.HOllY)
                        {
                            AstrImagePath = AstrDirName + "\\" + AComponentImgName;
                            //bitMap.Save(AstrImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.HOllY1)//Q.F.2016.12.09
                        {

                            AstrImagePath = AstrDirName + "\\" + AComponentImgName;
                            //     AstrImagePath = AstrDirName + AComponentImgName ;
                            bool bExist = File.Exists(AstrImagePath);
                            bool bPathExist = Directory.Exists(Path.GetDirectoryName(AstrImagePath));
                            try
                            {
                                //WF.CImageSize.GenerateHighThumbnail(bitMap, AstrImagePath, iNewWidth, iNewHeight);
                                // bitMap.Save(AstrImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                                _csBaseF.SaveImg(AstrImagePath, stSave, 50);
                            }
                            catch (System.Exception ex)
                            {
                                //MessageBox.Show(AstrImagePath + ":" + bExist + "\t" + Path.GetDirectoryName(AstrImagePath) + ":" + bPathExist + "\t" + ex.ToString());
                                return;
                            }
                        }
                        if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)//PD 20181019
                        {
                            // save Jpeg
                            AstrImagePath = strAstrDirNameForBlueWey + "\\" + AComponentImgName;
                            //     AstrImagePath = AstrDirName + AComponentImgName ;
                            bool bExist = File.Exists(AstrImagePath);
                            bool bPathExist = Directory.Exists(Path.GetDirectoryName(AstrImagePath));
                            strFileName = AstrDirName + "\\" + Adr[0]["ArrayID"].ToString() + "_" + Adr[0]["ComponentID"].ToString();
                            try
                            {
                                //WF.CImageSize.GenerateHighThumbnail(bitMap, AstrImagePath, iNewWidth, iNewHeight);
                                //bitMap.Save(AstrImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                _csBaseF.SaveImg(AstrImagePath, stSave, 80);
                            }
                            catch (System.Exception ex)
                            {
                                //MessageBox.Show(AstrImagePath + ":" + bExist + "\t" + Path.GetDirectoryName(AstrImagePath) + ":" + bPathExist + "\t" + ex.ToString());
                                return;
                            }

                        }
                        r = null;
                        g = null;
                        b = null;
                        //bitMap = null;

                        //save to scv
                        if (_appSetting.bExportPadInfo == true
                            && _appSetting.csvFormat != AppLayerLibForEYSPI.CSVFormat.HOllY1
                            && _appSetting.csvFormat != AppLayerLibForEYSPI.CSVFormat.JUZI)
                        {
                            StringBuilder sb = new StringBuilder();

                            sb.Append(
                                "ArrayID" + RS_SPLIT
                                + "ComponentID" + RS_SPLIT
                                + "PadID" + RS_SPLIT
                                + "JudgeRes" + RS_SPLIT
                                + "PerArea" + RS_SPLIT
                                + "PerVolume" + RS_SPLIT
                                + "ABSHeight" + RS_SPLIT
                                + "ShiftX" + RS_SPLIT
                                + "ShiftY" + RS_LineEnd
                                            );
                            
                            foreach (DataRow row in Adr)
                            {

                                sb.Append(
                                   row["ArrayID"].ToString() + RS_SPLIT
                                   + row["ComponentID"].ToString() + RS_SPLIT
                                   + row["PadID"] + RS_SPLIT
                                   + row["JudgeRes"].ToString() + RS_SPLIT
                                   + row["PerArea"] + RS_SPLIT
                                   + row["PerVolume"] + RS_SPLIT
                                   + row["ABSHeight"] + RS_SPLIT
                                   + row["ShiftX"] + RS_SPLIT
                                   + row["ShiftY"] + RS_LineEnd
                                           );

                            }

                            //File.WriteAllText(strFileName + ".csv", sb.ToString());
                            File.WriteAllText(strFileName + _strSaveExtension, sb.ToString());
                            sb.Clear();
                        }
                        else if (_appSetting.bExportPadInfo == true && _appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                        {
                            StringBuilder strContent = new StringBuilder();

                            //Pad Id，Component ID,Result,Volume(%)，Area(%),Height(um)，OffsetX(mm)， OffsetY(mm)，PosX(Pixel) ,PosY(Pixel),SizeX(Pixel),SizeY(Pixel),
                            strContent.Append(
                                "Pad ID" + RS_SPLIT
                                + "Component ID" + RS_SPLIT
                                + "Result" + RS_SPLIT
                                + "Volume(%)" + RS_SPLIT
                                + "Area(%)" + RS_SPLIT
                                + "Height(um)" + RS_SPLIT
                                + "OffsetX(mm)" + RS_SPLIT
                                + "OffsetY(mm)" + RS_SPLIT
                                + "PosX(Pixel)" + RS_SPLIT
                                + "PosY(Pixel)" + RS_SPLIT
                                + "SizeX(Pixel)" + RS_SPLIT
                                + "SizeY(Pixel)" + RS_SPLIT + RS_LineEnd
                                            );

                            foreach (DataRow row in Adr)
                            {

                                strContent.Append(
                                   row["PadID"].ToString() + RS_SPLIT
                                   + row["ComponentID"].ToString() + RS_SPLIT
                                   + row["JudgeRes"] + RS_SPLIT
                                   + row["PerVolume"].ToString() + RS_SPLIT
                                   + row["PerArea"] + RS_SPLIT
                                   + row["ABSHeight"] + RS_SPLIT
                                   + row["ShiftX"] + RS_SPLIT
                                   + row["ShiftY"] + RS_SPLIT
                                   + row["XinFov"] + RS_SPLIT
                                   + row["YinFov"] + RS_SPLIT
                                   + row["XinCADmm"] + RS_SPLIT
                                   + row["YinCADmm"] + RS_SPLIT + RS_LineEnd
                                           );

                            }
                            File.WriteAllText(Path.Combine(strAstrDirNameForBlueWey, AComponentImgName.Replace(Path.GetExtension(AComponentImgName), _strSaveExtension)), strContent.ToString(), Encoding.Default);
                            strContent.Clear();
                        }
                    }
                
                    iCount = 0;
                    dPixelXum = 0;
                    dPixelYum = 0;

                    dMinW = 0;
                    dMaxW = 0;
                    dMinH = 0;
                    dMaxH = 0;
                    dTmpW = 0;
                    dTmpH = 0;
                
            }
            catch (System.Exception ex)
            {
                
                LogRecord(RS_PROJECTNAME, "GenDataToAOI Error:" + Adr[0]["ArrayID"].ToString() + "\t" + Adr[0]["ComponentID"].ToString()+" 异常信息: "+ex.ToString(), false);
                //MessageBox.Show(Adr[0]["ArrayID"].ToString() + "\t" + Adr[0]["ComponentID"].ToString());
                //if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                //{
                //    return;
                //}
                //else
                //{
                //    throw ex;
                    return;
                //}
            }

        }
        private void GenDataToAOI(string strFrSpiDataFileDir, string strFileName, DataRow[] Adr, String AstrDirName, List<APP_SaveOrLoadImgDataInFo> AColorLst,
            int AiBitmapWidth, String AComponentImgName, out bool AbComponentPass, out String AstrImagePath, int AiFovWidth, int AiFovHeight, AppSettingData AappSettingData,bool bIsFast)//, Bitmap[] AbitMaps)
        {
            AbComponentPass = true;
            AstrImagePath = "";
            string strAstrDirNameForBlueWey = string.Empty;
            if (Adr == null || Adr.Length == 0)
                return;
            try
            {
                //System.Collections.ArrayList list = new System.Collections.ArrayList();
                ImgCSCoreIM.ColorFactorParams stColorFactors = AappSettingData.stColorFactorParams;
                int iCount = Adr.Length;
                float fTmp = 0.1f;
                double dPixelXum = (double)Convert.ToDouble(Adr[0]["PixelXum"]);
                double dPixelYum = (double)Convert.ToDouble(Adr[0]["PixelYum"]);
                int iMargin = (int)Math.Ceiling(dPixelXum * (int.Parse(_appSetting.strtBarImage) * fTmp));
                int[] iXinFovs = new int[iCount];
                int[] iYinFovs = new int[iCount];
                int[] iPadWidths = new int[iCount];
                int[] iPadHeights = new int[iCount];

                int iFovNo = 0;
                double dXinCADmm = 0;
                double dYinCADmm = 0;

                double dMinW = 10000;//// (double)Convert.ToDouble(Adr[0]["XinCADmm"]);
                double dMaxW = -10000;// (double)Convert.ToDouble(Adr[iCount - 1]["XinCADmm"]) + ((int)Convert.ToDouble(Adr[iCount - 1]["PadW"]) / 1000.0) * dPixelXum;
                double dMinH = 10000;// (double)Convert.ToDouble(Adr[0]["YinCADmm"]);
                double dMaxH = -100000;
                double dTmpW = 0;
                double dTmpH = 0;
                short shColorPer = 0;
              

                float fUM2MM = 0.001f;


                for (int i = 0; i < iCount; i++)
                {

                    iXinFovs[i] = (int)Convert.ToInt32(Adr[i]["XinFov"]);
                    iYinFovs[i] = (int)Convert.ToInt32(Adr[i]["YinFov"]);
                    iPadWidths[i] = (int)Convert.ToInt32(Adr[i]["PadW"]);
                    iPadHeights[i] = (int)Convert.ToInt32(Adr[i]["PadH"]);

                    iFovNo = (int)Convert.ToInt32(Adr[i]["FovNo"]);
                    if (i < 1)
                    {
                        GetFovImage(strFrSpiDataFileDir, strFileName, out AiFovWidth, out AiFovHeight, iFovNo);
                    }
                    dXinCADmm = (double)Convert.ToDouble(Adr[i]["XinCADmm"]);
                    dYinCADmm = (double)Convert.ToDouble(Adr[i]["YinCADmm"]);

                    iRectVCrrX = (int)Convert.ToInt32(Adr[i]["RectVCrrX"]);
                    iRectVCrrY = (int)Convert.ToInt32(Adr[i]["RectVCrrY"]);
                    iRectVCrrW = (int)Convert.ToInt32(Adr[i]["RectVCrrW"]);
                    iRectVCrrH = (int)Convert.ToInt32(Adr[i]["RectVCrrH"]);
                    iRectVX = (int)Convert.ToInt32(Adr[i]["RectVX"]);
                    iRectVY = (int)Convert.ToInt32(Adr[i]["RectVY"]);
                    iRectVW = (int)Convert.ToInt32(Adr[i]["RectVW"]);
                    iRectVH = (int)Convert.ToInt32(Adr[i]["RectVH"]);

                    iRectBX = (int)Convert.ToInt32(Adr[i]["RectBX"]);
                    iRectBY = (int)Convert.ToInt32(Adr[i]["RectBY"]);
                    iRectBW = (int)Convert.ToInt32(Adr[i]["RectBW"]);
                    iRectBH = (int)Convert.ToInt32(Adr[i]["RectBH"]);

                    iRectAX = (int)Convert.ToInt32(Adr[i]["RectAX"]);
                    iRectAY = (int)Convert.ToInt32(Adr[i]["RectAY"]);
                    iRectAW = (int)Convert.ToInt32(Adr[i]["RectAW"]);
                    iRectAH = (int)Convert.ToInt32(Adr[i]["RectAH"]);
                    int iPadAI = (int)Convert.ToInt32(Adr[i]["PadID"]);

                    iXinFovs[i] = iRectVCrrX - iMargin < 0 ? 0 : iRectVCrrX - iMargin;
                    iYinFovs[i] = iRectVCrrY - iMargin < 0 ? 0 : iRectVCrrY - iMargin;
                    iPadWidths[i] = iRectVCrrW + iMargin * 2;
                    iPadHeights[i] = iRectVCrrH + iMargin * 2;

                    iXinFovs[i] = iRectVX - iMargin < 0 ? 0 : iRectVX - iMargin;
                    iYinFovs[i] = iRectVY - iMargin < 0 ? 0 : iRectVY - iMargin;
                    iPadWidths[i] = iRectVW + iMargin * 2;
                    iPadHeights[i] = iRectVH + iMargin * 2;

                    if (iXinFovs[i] + iPadWidths[i] > AiFovWidth)
                    {
                        iXinFovs[i] -= (AiFovWidth - iPadWidths[i]);
                    }
                    if (iYinFovs[i] + iPadHeights[i] > AiFovHeight)
                    {
                        iYinFovs[i] -= (AiFovHeight - iPadHeights[i]);
                    }
                    dTmpW = (dXinCADmm - Convert.ToDouble(iPadWidths[i]) * 0.5f * fUM2MM * dPixelXum);//Math.Floor
                    if (dTmpW < dMinW)
                        dMinW = dTmpW;
                    dTmpW = Math.Ceiling((double)Convert.ToDouble(dXinCADmm) + Convert.ToDouble(iPadWidths[i]) * 0.5f * fUM2MM * dPixelXum);
                    if (dTmpW > dMaxW)
                        dMaxW = dTmpW;

                    dTmpH = Math.Floor((double)Convert.ToDouble(dYinCADmm) - Convert.ToDouble(iPadHeights[i]) * 0.5f * fUM2MM * dPixelYum);
                    if (dTmpH < dMinH)
                        dMinH = dTmpH;

                    dTmpH = Math.Ceiling((double)Convert.ToDouble(dYinCADmm) + Convert.ToDouble(iPadHeights[i]) * 0.5f * fUM2MM * dPixelYum);
                    if (dTmpH > dMaxH)
                        dMaxH = dTmpH;
                }
                //cal concat image size
                AiBitmapWidth = AiFovWidth;
                int iWidth = (int)Math.Ceiling((dMaxW - dMinW) / dPixelXum * 1000);
                int iHeight = (int)Math.Ceiling((dMaxH - dMinH) / dPixelYum * 1000);
                //<-;

                int iNewWidth = iWidth * int.Parse(_appSetting.strtBarImage);
                int iNewHeight = iHeight * int.Parse(_appSetting.strtBarImage);
                if (iNewHeight == 0)
                {
                    iNewHeight = iHeight;
                }
                if (iNewWidth == 0)
                {
                    iNewWidth = iWidth;
                }
                //get the Path
                if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                {
                    if (string.IsNullOrEmpty(AstrDirName) == false)
                    {
                        string str = AstrDirName.Split('_')[1];
                        DateTime dtTime = DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);

                        if (dtTime.Month.ToString().Length >= 2)
                        {
                            strAstrDirNameForBlueWey = Path.Combine(_appSetting.strToAOIPath, dtTime.Year + "", dtTime.Month + "", dtTime.ToString("yyyyMMdd"));
                        }
                        else
                        {
                            strAstrDirNameForBlueWey = Path.Combine(_appSetting.strToAOIPath, dtTime.Year + "", "0" + dtTime.Month + "", dtTime.ToString("yyyyMMdd"));
                        }

                        if (Directory.Exists(strAstrDirNameForBlueWey) == false)
                        {
                            Directory.CreateDirectory(strAstrDirNameForBlueWey);
                        }
                    }
                }
                //Bitmap bitMap = null;// = new Bitmap(iWidth, iHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                int iXinBitMap = 0;
                int iYinBitMap = 0;
                //Color color;
                int iIndex = 0;
                byte[] r = new byte[iWidth * iHeight];
                byte[] g = new byte[iWidth * iHeight];
                byte[] b = new byte[iWidth * iHeight];
                Array.Clear(r, 0, iWidth * iHeight);
                Array.Clear(g, 0, iWidth * iHeight);
                Array.Clear(b, 0, iWidth * iHeight);
                double dValue = 0;
                //ImgCSCoreIM.ColorFactorParams stColorFactors = AappSettingData.stColorFactorParams;
                int iRGBInFovIndex = 0;
                //bool bPass = true;

                for (int i = 0; i < iCount; i++)
                {
                    iFovNo = (int)Convert.ToInt32(Adr[i]["FovNo"]);
                    


                    GetFovImage(strFrSpiDataFileDir, strFileName, out AiFovWidth, out AiFovHeight, iFovNo);
                    dXinCADmm = (double)Convert.ToDouble(Adr[i]["XinCADmm"]);
                    dYinCADmm = (double)Convert.ToDouble(Adr[i]["YinCADmm"]);

                    int iPadAI = (int)Convert.ToInt32(Adr[i]["PadID"]);

                    dValue = dXinCADmm - iPadWidths[i] * 0.5f * fUM2MM * dPixelXum - dMinW;
                    iXinBitMap = (int)(Math.Floor(dValue * 1000 / dPixelXum));
                    dValue = dYinCADmm - iPadHeights[i] * 0.5f * fUM2MM * dPixelYum - dMinH;
                    iYinBitMap = (int)(Math.Floor(dValue * 1000 / dPixelYum));

                    for (int y = iYinBitMap; y < iYinBitMap + iPadHeights[i]; y++)
                    {


                        iIndex = y * iWidth + iXinBitMap;

                        if (iIndex < 0)
                        {
                            iIndex = 0;
                        }
                        if (iIndex > _arrAPP_SaveOrLoadImgDataInFo[0].stImgData.byImgDataR.Length - 1)
                        {
                            iIndex = _arrAPP_SaveOrLoadImgDataInFo[0].stImgData.byImgDataR.Length - 1;
                        }

                        //if (AColorLst[iFovNo - 1].stImgData.byImgDataR.Length > 0)
                        //{
                        //    for (int l = 0; l < AColorLst[iFovNo - 1].stImgData.byImgDataR.Length; l++)
                        //    {                                                                           //(byte)((AFovs[iIndex].stImgData.byImgDataR[i] * shColorPer) >> 7)
                        //        AColorLst[iFovNo - 1].stImgData.byImgDataR[l] = (byte)((AColorLst[iFovNo - 1].stImgData.byImgDataR[l] * (short)(30 * 1.28)) >> 7);
                        //        //AColorLst[iFovNo - 1].stImgData.byImgDataG[l] = (byte)((AColorLst[iFovNo - 1].stImgData.byImgDataG[l] * (short)(30 * 1.28)) >> 7);
                        //        //AColorLst[iFovNo - 1].stImgData.byImgDataB[l] = (byte)((AColorLst[iFovNo - 1].stImgData.byImgDataB[l] * (short)(30 * 1.28)) >> 7);


                        //    }
                        //}

                        iRGBInFovIndex = (y - iYinBitMap + iYinFovs[i]) * AiBitmapWidth + (iXinFovs[i]);
                        Array.Copy(_arrAPP_SaveOrLoadImgDataInFo[0].stImgData.byImgDataG, iRGBInFovIndex, g, iIndex, iPadWidths[i]);
                        Array.Copy(_arrAPP_SaveOrLoadImgDataInFo[0].stImgData.byImgDataR, iRGBInFovIndex, r, iIndex, iPadWidths[i]);
                        Array.Copy(_arrAPP_SaveOrLoadImgDataInFo[0].stImgData.byImgDataB, iRGBInFovIndex, b, iIndex, iPadWidths[i]);
                        //Array.Copy(AColorLst[iFovNo - 1].stImgData.byImgDataB, iRGBInFovIndex, b, iIndex, iPadWidths[i]);
                        //if (stColorFactors.byRGB2DFactorR != 100)
                        //{
                        //    //fFactor = stColorFactors.byRGB2DFactorR * 0.01f;
                        //    shColorPer = (short)(stColorFactors.byRGB2DFactorR * 1.28);
                        //    for (i = 0; i < lImageLength; ++i)
                        //    {
                        //        //R1[i] = (byte)(AFovs[a]._rgbImg[0][i] * fFactor);
                        //        _imgByte2DRFov[i] = (byte)((AFovs[iIndex].stImgData.byImgDataR[i] * shColorPer) >> 7);
                        //        // R1[i] = aTmp[AFovs[a]._rgbImg[0][i]];
                        //    }
                        //}
                        //else
                        //{
                        //    //Marshal.Copy((IntPtr)((void*)AFovs[a]._rgbImg[0]), R1, 0,
                        //    //   lImageLength);
                        //    Array.Copy(AFovs[iIndex].stImgData.byImgDataR, _imgByte2DRFov, lImageLength);
                        //}


                    }


                    String str = Adr[i]["JudgeRes"].ToString();
                    if (str.Equals("Pass"))
                        AbComponentPass = false;
                    if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                    {
                        #region
                        string strTmpFile = Path.Combine(strAstrDirNameForBlueWey, AComponentImgName.Split('-')[0] + "spi.log");
                        if (File.Exists(strTmpFile) == false)
                        {
                            StringBuilder strPld = new StringBuilder();
                            // spidb data server
                            string strTmpBarcode = string.Empty;
                            if (File.Exists(AstrDirName))
                            {
                                strTmpBarcode = Path.GetFileNameWithoutExtension(AstrDirName);
                            }
                            else
                            {
                                strTmpBarcode = AstrDirName.Split('\\').Last();
                            }
                            //string 
                            //if()
                            if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                            {
                                strTmpBarcode = strTmpBarcode.Split('_').Last();
                            }
                            else
                            {
                                strTmpBarcode = strTmpBarcode.Split('_').First();
                            }
                            string strSql = "SELECT job.JobName,pcb.StartTime,pcb.Result FROM spidb.tbboard as pcb,spidb.TBJobInfo as job WHERE pcb.PCBBarcode = '" + strTmpBarcode + "' AND job.SerNo = pcb.JobIndex;";

                            DataBeanEx.MySQLBean mysqlBean = new DataBeanEx.MySQLBean();
                            DataTable dt = mysqlBean.GetDataTableFrSQL(strSql);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                string strTpResult = string.Empty;
                                if (dt.Rows[0][2].ToString() == "2")
                                {
                                    strTpResult = "PASS";
                                }
                                else if (dt.Rows[0][2].ToString() == "0")
                                {
                                    strTpResult = "GOOD";
                                }
                                else
                                {
                                    strTpResult = "NG";
                                }

                                strPld.Append("SinicTek" + RS_SPLIT +
                                    dt.Rows[0][0].ToString() + RS_SPLIT + RS_SPLIT + RS_SPLIT +
                                    ((DateTime)dt.Rows[0][1]).ToString("yyyyMMddHHmmss") + RS_SPLIT + RS_SPLIT + RS_SPLIT +
                                    strTpResult + RS_SPLIT + "");
                                File.WriteAllText(strTmpFile, strPld.ToString(), Encoding.Default);
                            }
                        }
                        #endregion
                    }
                }

                try
                {
                    //Q.F.2019.03.29
                    byte byRGB2DFactorR = 30;
                    shColorPer = 0;
                    int iImgLength = r.Length;
                    if (byRGB2DFactorR != 100)
                    {

                        shColorPer = (short)(byRGB2DFactorR * 1.28);
                        for (int i = 0; i < iImgLength; ++i)
                        {
                            r[i] = (byte)((r[i] * shColorPer) >> 7);
                        }
                    }


                    stSave.iImgFormat = 1;
                    stSave.bUseOpcv = false;
                    stSave.byImgDataR = r;
                    stSave.byImgDataG = g;
                    stSave.byImgDataB = b;
                    stSave.iWidth = iWidth;
                    stSave.iHeight = iHeight;

                }
                catch (System.Exception ex)
                {
                    //bitMap = null;
                    //MessageBox.Show("Gen Bitmap Error:" + ex.ToString());
                    return;
                    //StopPress("Gen Bitmap Error:" + ex.ToString());
                }
                if (true)
                {
                    strFileName = "";
                    String strTmp = "";
                    //int iComponentID = (int)Adr[0]["ComponentID"];
                    if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.MingRui)
                    {
                        strFileName = AstrDirName + "\\" + Adr[0]["ArrayID"].ToString() + "_" + Adr[0]["ComponentID"].ToString();
                        AstrImagePath = strFileName + ".Jpeg";
                        //bitMap.Save(AstrImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.HOllY)
                    {
                        AstrImagePath = AstrDirName + "\\" + AComponentImgName;
                        //bitMap.Save(AstrImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.HOllY1)//Q.F.2016.12.09
                    {

                        AstrImagePath = AstrDirName + "\\" + AComponentImgName;
                        //     AstrImagePath = AstrDirName + AComponentImgName ;
                        bool bExist = File.Exists(AstrImagePath);
                        bool bPathExist = Directory.Exists(Path.GetDirectoryName(AstrImagePath));
                        try
                        {
                            //WF.CImageSize.GenerateHighThumbnail(bitMap, AstrImagePath, iNewWidth, iNewHeight);
                            // bitMap.Save(AstrImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                            _csBaseF.SaveImg(AstrImagePath, stSave, 80);
                        }
                        catch (System.Exception ex)
                        {
                            //MessageBox.Show(AstrImagePath + ":" + bExist + "\t" + Path.GetDirectoryName(AstrImagePath) + ":" + bPathExist + "\t" + ex.ToString());
                            return;
                        }
                    }
                    if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)//PD 20181019
                    {
                        // save Jpeg
                        AstrImagePath = strAstrDirNameForBlueWey + "\\" + AComponentImgName;
                        //     AstrImagePath = AstrDirName + AComponentImgName ;
                        bool bExist = File.Exists(AstrImagePath);
                        bool bPathExist = Directory.Exists(Path.GetDirectoryName(AstrImagePath));
                        strFileName = AstrDirName + "\\" + Adr[0]["ArrayID"].ToString() + "_" + Adr[0]["ComponentID"].ToString();
                        try
                        {
                            //WF.CImageSize.GenerateHighThumbnail(bitMap, AstrImagePath, iNewWidth, iNewHeight);
                            //bitMap.Save(AstrImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                            _csBaseF.SaveImg(AstrImagePath, stSave, 80);
                        }
                        catch (System.Exception ex)
                        {
                            //MessageBox.Show(AstrImagePath + ":" + bExist + "\t" + Path.GetDirectoryName(AstrImagePath) + ":" + bPathExist + "\t" + ex.ToString());
                            return;
                        }

                    }
                    r = null;
                    g = null;
                    b = null;
                    //bitMap = null;

                    //save to scv
                    if (_appSetting.bExportPadInfo == true
                        && _appSetting.csvFormat != AppLayerLibForEYSPI.CSVFormat.HOllY1
                        && _appSetting.csvFormat != AppLayerLibForEYSPI.CSVFormat.JUZI)
                    {
                        StringBuilder sb = new StringBuilder();

                        sb.Append(
                            "ArrayID" + RS_SPLIT
                            + "ComponentID" + RS_SPLIT
                            + "PadID" + RS_SPLIT
                            + "JudgeRes" + RS_SPLIT
                            + "PerArea" + RS_SPLIT
                            + "PerVolume" + RS_SPLIT
                            + "ABSHeight" + RS_SPLIT
                            + "ShiftX" + RS_SPLIT
                            + "ShiftY" + RS_LineEnd
                                        );

                        foreach (DataRow row in Adr)
                        {

                            sb.Append(
                               row["ArrayID"].ToString() + RS_SPLIT
                               + row["ComponentID"].ToString() + RS_SPLIT
                               + row["PadID"] + RS_SPLIT
                               + row["JudgeRes"].ToString() + RS_SPLIT
                               + row["PerArea"] + RS_SPLIT
                               + row["PerVolume"] + RS_SPLIT
                               + row["ABSHeight"] + RS_SPLIT
                               + row["ShiftX"] + RS_SPLIT
                               + row["ShiftY"] + RS_LineEnd
                                       );

                        }

                        //File.WriteAllText(strFileName + ".csv", sb.ToString());
                        File.WriteAllText(strFileName + _strSaveExtension, sb.ToString());
                        sb.Clear();
                    }
                    else if (_appSetting.bExportPadInfo == true && _appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                    {
                        StringBuilder strContent = new StringBuilder();

                        //Pad Id，Component ID,Result,Volume(%)，Area(%),Height(um)，OffsetX(mm)， OffsetY(mm)，PosX(Pixel) ,PosY(Pixel),SizeX(Pixel),SizeY(Pixel),
                        strContent.Append(
                            "Pad ID" + RS_SPLIT
                            + "Component ID" + RS_SPLIT
                            + "Result" + RS_SPLIT
                            + "Volume(%)" + RS_SPLIT
                            + "Area(%)" + RS_SPLIT
                            + "Height(um)" + RS_SPLIT
                            + "OffsetX(mm)" + RS_SPLIT
                            + "OffsetY(mm)" + RS_SPLIT
                            + "PosX(Pixel)" + RS_SPLIT
                            + "PosY(Pixel)" + RS_SPLIT
                            + "SizeX(Pixel)" + RS_SPLIT
                            + "SizeY(Pixel)" + RS_SPLIT + RS_LineEnd
                                        );

                        foreach (DataRow row in Adr)
                        {

                            strContent.Append(
                               row["PadID"].ToString() + RS_SPLIT
                               + row["ComponentID"].ToString() + RS_SPLIT
                               + row["JudgeRes"] + RS_SPLIT
                               + row["PerVolume"].ToString() + RS_SPLIT
                               + row["PerArea"] + RS_SPLIT
                               + row["ABSHeight"] + RS_SPLIT
                               + row["ShiftX"] + RS_SPLIT
                               + row["ShiftY"] + RS_SPLIT
                               + row["XinFov"] + RS_SPLIT
                               + row["YinFov"] + RS_SPLIT
                               + row["XinCADmm"] + RS_SPLIT
                               + row["YinCADmm"] + RS_SPLIT + RS_LineEnd
                                       );

                        }
                        File.WriteAllText(Path.Combine(strAstrDirNameForBlueWey, AComponentImgName.Replace(Path.GetExtension(AComponentImgName), _strSaveExtension)), strContent.ToString(), Encoding.Default);
                        strContent.Clear();
                    }
                }

                iCount = 0;
                dPixelXum = 0;
                dPixelYum = 0;

                dMinW = 0;
                dMaxW = 0;
                dMinH = 0;
                dMaxH = 0;
                dTmpW = 0;
                dTmpH = 0;

            }
            catch (System.Exception ex)
            {

                LogRecord(RS_PROJECTNAME, "GenDataToAOI Error:" + Adr[0]["ArrayID"].ToString() + "\t" + Adr[0]["ComponentID"].ToString() + " 异常信息: " + ex.ToString(), false);
                //MessageBox.Show(Adr[0]["ArrayID"].ToString() + "\t" + Adr[0]["ComponentID"].ToString() + ex.Message);
                MessageBox.Show(Adr[0]["ArrayID"].ToString() + "\t" + Adr[0]["ComponentID"].ToString() + ex.Message, "系统提示",

                                                        MessageBoxButtons.OK, MessageBoxIcon.Warning,

                                                        MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                //if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                //{
                //    return;
                //}
                //else
                //{
                //    throw ex;
                return;
                //}
            }

        }
        //AbyMode  0, before save component; 1, save component

        private void GetFovImage(string strFrSpiDataFileDir, String AstrDirName,
            //DataRow[] ASelectedDrFrmAOI,
            out int AiFovWidth, out int AiFovHeight, int iFov)//, ref List<Color[]> AColorLst)
        {                      
            AiFovWidth = 0;
            AiFovHeight = 0;
            try
            {
                String strPath = string.Empty;
                if (DirChkAndCrt(strFrSpiDataFileDir) != -1)
                {           
                    strPath = strFrSpiDataFileDir;//Path.Combine(_strFovImgLinkToAOIDir, AstrDirName);
                }
                else
                {
                    return;// null;
                }
                if (_arrAPP_SaveOrLoadImgDataInFo == null || _arrAPP_SaveOrLoadImgDataInFo.Count <= 0)
                {
                    APP_SaveOrLoadImgDataInFo tmp = new APP_SaveOrLoadImgDataInFo();
                    tmp.stImgData.iImgFormat = 2;
                    tmp.stImgData.bUseOpcv = false;
                    //tmp.stImgData.bUseFastLoadBmp = false;
                    _arrAPP_SaveOrLoadImgDataInFo.Add(tmp);
                }
                if (_arrAPP_SaveOrLoadImgDataInFo != null && _arrAPP_SaveOrLoadImgDataInFo.Count > 0)
                {
                    _baseFucXU.LoadImadat(strPath + "\\" + (iFov - 1), ref _arrAPP_SaveOrLoadImgDataInFo[0].stImgData);
                }
                if (_arrAPP_SaveOrLoadImgDataInFo[0].stImgData.byImgDataR != null)
                //&& _arrAPP_SaveOrLoadImgDataInFo[0].stImgData.byImgDataR.Length == _arrAPP_SaveOrLoadImgDataInFo[0].stImgData.iHeight * _arrAPP_SaveOrLoadImgDataInFo[0].stImgData.iWidth)
                {
                    AiFovWidth = _arrAPP_SaveOrLoadImgDataInFo[0].stImgData.iWidth;
                    AiFovHeight = _arrAPP_SaveOrLoadImgDataInFo[0].stImgData.iHeight;
                    
                }
                
            }
            catch (Exception ex)
            {
                LogRecord(RS_PROJECTNAME, " GetFovImage Error:" + ex.ToString(), false);
                return;
            }           
        }   
        //AbyMode  0, before save component; 1, save component
        private void GetFovImage(string strFrSpiDataFileDir, String AstrDirName, 
            DataRow[] ASelectedDrFrmAOI,
            out int AiFovWidth, out int AiFovHeight)//, ref List<Color[]> AColorLst)
        {
            //Q.F.2019.04.09
            int iCount = 0;
            int iFovNo = 0;
            Array.Clear(_arrByUsedFovFlag, 0, _arrByUsedFovFlag.Length);
            if(ASelectedDrFrmAOI != null)
            { 
                iCount = ASelectedDrFrmAOI.Length;
                for (int i = 0; i < iCount; i++)
                {
                    iFovNo = (int)Convert.ToInt32(ASelectedDrFrmAOI[i]["FovNo"]);
                    _arrByUsedFovFlag[iFovNo - 1] = 1;
                }
            }
            //List<Color[]> colorLst = null;
            AiFovWidth = 0;
            AiFovHeight = 0;
            //AOutBitMaps = null;
            try
            {
                String strPath = string.Empty;
                if (DirChkAndCrt(strFrSpiDataFileDir) != -1)
                {
                    //if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                    //{
                    //    if (string.IsNullOrEmpty(AstrDirName) == false)
                    //    {
                    //        AstrDirName = AstrDirName.Split('_')[0];
                    //        DirectoryInfo info = new DirectoryInfo(_strFovImgLinkToAOIDir);
                    //        DirectoryInfo[] dirTmp = info.GetDirectories(AstrDirName + "_*");
                    //        strPath = dirTmp[0].FullName;
                    //    }
                    //}
                    //else
                    //{
                    strPath = strFrSpiDataFileDir;//Path.Combine(_strFovImgLinkToAOIDir, AstrDirName);
                    //}
                }
                else
                {
                    //StopPress("GetFovImage Could Not Create Folder");
                    //MessageBox.Show("Could Not Create Folder:" + _strFovImgLinkToAOIDir);
                    return;// null;
                }

                //if (!Directory.Exists(strPath))//no fov image
                //{
                //    return colorLst;
                //}

                DirectoryInfo dir = new DirectoryInfo(strPath);
                FileInfo[] listfile = dir.GetFiles("*" + RS_FovImgdatFormat);//.imgdat"); //Q.F.2019.04.09

                iCount = listfile.Length;
                //Bitmap[] bitMaps = new Bitmap[iCount];
                //String test = listfile[0].FullName;
                //Q.F.2016.12.12
                String[] aStrFileNames = GetFiles(strPath + "\\", RS_FovImgdatFormat);
                if (aStrFileNames == null || aStrFileNames.Length == 0)
                {
                    aStrFileNames = GetFiles(strPath + "\\", RS_FovImgdatFormat);
                }
                //Q.F.2019.04.09 
                int iOrgCount = _arrAPP_SaveOrLoadImgDataInFo.Count;
                if (aStrFileNames.Length > iOrgCount)
                {
                    for (int i = 0; i < aStrFileNames.Length - iOrgCount; ++i)
                    {
                        APP_SaveOrLoadImgDataInFo tmp = new APP_SaveOrLoadImgDataInFo();                       
                        tmp.stImgData.iImgFormat = 2;
                        tmp.stImgData.bUseOpcv = false;
                        tmp.stImgData.bUseFastLoadBmp = false;
                          
                         _arrAPP_SaveOrLoadImgDataInFo.Add(tmp);
                    }
                    
                }
                //if (AbyMode == 0)
                //{
                //    Array.Clear(_arrByUsedFovFlag, 0, _arrByUsedFovFlag.Length);
                //    return;
                //}

                int iImgLength = 0;
                //ClearALLImgData();
                for (int i = 0; i < iCount; i++)
                {
                    //_arrAPP_SaveOrLoadImgDataInFo[i] = new APP_SaveOrLoadImgDataInFo();
                    //_stImgData = new ImgCSCoreIM.ST_SaveOrLoadImgData();
                    //_stImgData.iImgFormat = 2;
                    //_stImgData.bUseOpcv = false;
                    //_stImgData.bUseFastLoadBmp = false;

                    //if (list.Contains(i + 1) == false)
                    //{
                    //    continue;
                    //}
                    //Q.F.2019.04.09
                    if (_arrByUsedFovFlag[i] > 0
                        && _arrAPP_SaveOrLoadImgDataInFo[i].bHasLoaded== false)
                    {
                        //ClearSingleImgData(i);
                        //MessageBox.Show(strPath + "\\" + i.ToString());
                        _baseFucXU.LoadImadat(strPath + "\\" + i.ToString(), ref _arrAPP_SaveOrLoadImgDataInFo[i].stImgData);
                        if (_arrAPP_SaveOrLoadImgDataInFo[i].stImgData.byImgDataR != null
                            && _arrAPP_SaveOrLoadImgDataInFo[i].stImgData.byImgDataR.Length == _arrAPP_SaveOrLoadImgDataInFo[i].stImgData.iHeight * _arrAPP_SaveOrLoadImgDataInFo[i].stImgData.iWidth)
                        {
                            AiFovWidth = _arrAPP_SaveOrLoadImgDataInFo[i].stImgData.iWidth;
                            AiFovHeight = _arrAPP_SaveOrLoadImgDataInFo[i].stImgData.iHeight;
                            _arrAPP_SaveOrLoadImgDataInFo[i].bHasLoaded = true;
                        }
                      //  _arrAPP_SaveOrLoadImgDataInFo[i].stImgData = _stImgData;
                    }                   
                }
              
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }
            catch (Exception ex)
            {
                LogRecord(RS_PROJECTNAME, " GetFovImage Error:"  + ex.ToString(), false);
                return;
            }
            //return _arrAPP_SaveOrLoadImgDataInFo;

        }
        //[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void LogRecord(string strProjectName, string ARecordEvent, bool bCheckReRecord)
        {

            StreamWriter sw = null;
            try
            {
                String strFileName = DateTime.Now.ToString("yyyy_MM_dd");
                //String strTime = InspectMainLib.InspectAlgCore.GetNowDateTimeStr() + ":  ";
                //String strTime = DateTime.Now.ToLongTimeString() + "  :  ";
                String strTime = DateTime.Now.ToString("HH:mm:ss.fff") + "  :  ";
                strFileName = _logRecordDir + strProjectName + "_" + strFileName + ".txt";
                int iCount = 0;
                if (bCheckReRecord == true)
                {
                    if (File.Exists(strFileName))
                    {
                        String[] strLines = File.ReadAllLines(strFileName);
                        if (strLines != null && strLines.Length >= 1)
                        {
                            iCount = strLines.Length - 1;
                            //if (strLines[strLines.Length - 1] != null && strLines[strLines.Length - 1].Equals("") != true)
                            //{
                            //    if (strLines[strLines.Length - 1].Length > strTime.Length)
                            //    {
                            //        strTmp = strLines[strLines.Length - 1].Substring(strTime.Length, strLines[strLines.Length - 1].Length - strTime.Length);

                            //    }
                            //}
                            if (String.IsNullOrEmpty(strLines[iCount]) == false
                                && strLines[iCount].Length > strTime.Length)
                            {
                                String strTmp = strLines[iCount].Substring(strTime.Length, strLines[iCount].Length - strTime.Length);
                                if (string.Equals(strTmp, ARecordEvent) == false)
                                {
                                    bCheckReRecord = false;
                                }
                            }
                        }
                        //if (strTmp.Equals(ARecordEvent) == false)

                    }
                    else
                    {
                        bCheckReRecord = false;
                    }

                }
                if (bCheckReRecord == false)
                {
                    sw = File.AppendText(strFileName);
                    sw.WriteLine(strTime + ARecordEvent);
                }


            }
            catch
            {
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }
        }

        public String[] GetFiles(String AstrDir, String AstrExt)
        {
            return Directory.GetFiles(AstrDir, "*" + AstrExt).Where(t => t.ToLower().EndsWith(AstrExt)).ToArray();

        }
        private void GetColorValues(Bitmap AbitMap, Color[] Acolors)
        {
            //int iIndex = 0;
            //for (int y = 0; y < AbitMap.Size.Height; y++)
            //{
            //    for (int x = 0; x < AbitMap.Size.Width; x++)
            //    {
            //        iIndex = y * AbitMap.Size.Width + x;
            //        Acolors[iIndex] = AbitMap.GetPixel(x, y);


            //    }
            //}
            try
            {
                int iWidth = AbitMap.Width;
                int iHeight = AbitMap.Height;
                int Depth = System.Drawing.Bitmap.GetPixelFormatSize(AbitMap.PixelFormat);

                Rectangle rect = new Rectangle(0, 0, iWidth, iHeight);
                System.Drawing.Imaging.BitmapData bmpData = AbitMap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, AbitMap.PixelFormat);
                int iStride = bmpData.Stride;
                int iBytes = iStride * iHeight;
                byte[] Pixels = new byte[iBytes];


                unsafe
                {
                    IntPtr iPtr = bmpData.Scan0;
                    byte* ptr = (byte*)iPtr;
                    // Copy data from pointer to array
                    Marshal.Copy(iPtr, Pixels, 0, Pixels.Length);
                    int iIndex = 0;
                    int cCount = 0;
                    //another method
                    cCount = Depth / 8;
                    int i = 0;
                    for (int y = 0; y < iHeight; y++)
                    {
                        for (int x = 0; x < iWidth; x++)
                        {
                            iIndex = y * iWidth + x;


                            // Get start index of the specified pixel
                            i = ((y * iWidth) + x) * cCount;



                            if (Depth == 32) // For 32 bpp get Red, Green, Blue and Alpha
                            {
                                byte b = Pixels[i];
                                byte g = Pixels[i + 1];
                                byte r = Pixels[i + 2];
                                byte a = Pixels[i + 3]; // a
                                Acolors[iIndex] = Color.FromArgb(a, r, g, b);
                            }
                            if (Depth == 24) // For 24 bpp get Red, Green and Blue
                            {
                                byte b = Pixels[i];
                                byte g = Pixels[i + 1];
                                byte r = Pixels[i + 2];
                                Acolors[iIndex] = Color.FromArgb(r, g, b);
                            }
                            if (Depth == 8)
                            // For 8 bpp get color value (Red, Green and Blue values are the same)
                            {
                                byte c = Pixels[i];
                                Acolors[iIndex] = Color.FromArgb(c, c, c);
                            }

                        }
                    }

                }


                AbitMap.UnlockBits(bmpData);
            }
            catch (System.Exception ex)
            {

                //LogRecord(RS_PROJECTNAME, "GetColorVlue Error:" + ex.ToString(), false);
                //AbitMap.UnlockBits(bmpData);
                throw ex;
                return;

            }



        }
        private void SetParams()
        {
            if (_appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.MingRui
                || _appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.HOllY
                || _appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.HOllY1
                || _appSetting.csvFormat == AppLayerLibForEYSPI.CSVFormat.JUZI)
                _strSaveExtension = ".txt";
            _iArrayIDColumnIndex = _appSetting.iArrayIDColumnIndex;
            _iComponentIDColumnIndex = _appSetting.iComponentIDColumnIndex;

            if (_appSetting.strSPIPath != null && !_appSetting.strSPIPath.Equals(""))
            {
                _strDataLinkToAOIDir = _appSetting.strSPIPath;
                _strFovImgLinkToAOIDir = _strDataLinkToAOIDir + "\\FOVImage";
                DirChkAndCrt(_strDataLinkToAOIDir);
                DirChkAndCrt(_strFovImgLinkToAOIDir);
                _strDataLinkToAOIDirProcessed = Path.Combine(_strDataLinkToAOIDir, "Processed");
            }
            if (_appSetting.strFromAOIPath != null && !_appSetting.strFromAOIPath.Equals(""))
            {
                _strDataLinkFrAOIDir = _appSetting.strFromAOIPath;
                _strDataLinkFrAOIDirProcessed = _strDataLinkFrAOIDir + "\\Processed"; ;
                DirChkAndCrt(_strDataLinkFrAOIDir);
                DirChkAndCrt(_strDataLinkFrAOIDirProcessed);
            }
            if (_appSetting.strToAOIPath != null && !_appSetting.strToAOIPath.Equals(""))
            {
                _strDataExportToAOIDir = _appSetting.strToAOIPath;
                DirChkAndCrt(_strDataExportToAOIDir);
            }

            DirChkAndCrt(_logRecordDir);

            _bReadFirstLine = !_appSetting.bHasColumnName;

        }
        private AppLayerLibForEYSPI.AppSettingHandler appSettingHandle = new AppLayerLibForEYSPI.AppSettingHandler();
        public void ReadAppSetting()
        {
            if (_baseFucc.IsFileInUse(AppLayerLibForEYSPI.AppSettingHandler._fileDirName))
            {
                System.Threading.Thread.Sleep(100);
            }
            appSettingHandle.Read();
            _appSetting = appSettingHandle._appSettingData;

            SetParams();
        }
        private void ClearTheLastImgData()
        {
            if (_arrAPP_SaveOrLoadImgDataInFo != null && _arrAPP_SaveOrLoadImgDataInFo.Count > 1)
            {
                int i = 0, iFovLength = _arrAPP_SaveOrLoadImgDataInFo.Count;

                _arrAPP_SaveOrLoadImgDataInFo.RemoveRange(0, iFovLength);
                
            }
        }
        private void ClearALLImgData()
        {
            try
            {
                int i = 0, iFovLength = _arrAPP_SaveOrLoadImgDataInFo.Count, iImgLength = 0;

                for (; i < iFovLength; ++i)
                {
                    if (_arrAPP_SaveOrLoadImgDataInFo[i] != null
                        && _arrAPP_SaveOrLoadImgDataInFo[i].stImgData.byImgDataR != null
                        && _arrByUsedFovFlag[i] > 0)
                    {
                        _arrAPP_SaveOrLoadImgDataInFo[i].bHasLoaded = false;
                        iImgLength = _arrAPP_SaveOrLoadImgDataInFo[i].stImgData.byImgDataR.Length;
                        Array.Clear(_arrAPP_SaveOrLoadImgDataInFo[i].stImgData.byImgDataR, 0, iImgLength);
                        Array.Clear(_arrAPP_SaveOrLoadImgDataInFo[i].stImgData.byImgDataG, 0, iImgLength);
                        Array.Clear(_arrAPP_SaveOrLoadImgDataInFo[i].stImgData.byImgDataB, 0, iImgLength);
                    }
                }
            }
            catch (System.Exception ex)
            {
            	
            }
         
        }
        private void ClearSingleImgData(int AiFovIndex)
        {
            try
            {
                int i = 0, iFovLength = _arrAPP_SaveOrLoadImgDataInFo.Count, iImgLength = 0;

                if (_arrAPP_SaveOrLoadImgDataInFo.Count > 0
                    && _arrAPP_SaveOrLoadImgDataInFo[AiFovIndex] != null
                    && _arrAPP_SaveOrLoadImgDataInFo[AiFovIndex].stImgData.byImgDataR != null)
                {
                    _arrAPP_SaveOrLoadImgDataInFo[i].bHasLoaded = false;
                    iImgLength = _arrAPP_SaveOrLoadImgDataInFo[AiFovIndex].stImgData.byImgDataR.Length;
                    Array.Clear(_arrAPP_SaveOrLoadImgDataInFo[AiFovIndex].stImgData.byImgDataR, 0, iImgLength);
                    Array.Clear(_arrAPP_SaveOrLoadImgDataInFo[AiFovIndex].stImgData.byImgDataG, 0, iImgLength);
                    Array.Clear(_arrAPP_SaveOrLoadImgDataInFo[AiFovIndex].stImgData.byImgDataB, 0, iImgLength);
                }
            }
            catch (System.Exception ex)
            {
            	
            }
          
        }
    }
}
