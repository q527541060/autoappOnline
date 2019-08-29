using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppLayerLib;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Drawing.Imaging;

namespace AutoAPP
{
    class SaveFovImages
    {
        
        public static int _iProcessorCount = Environment.ProcessorCount;
        private readonly string RS_DATAEXPORTImageExt = ".jpg";
        private readonly string RS_DATAEXPORTFovInfoExt = ".txt";
        private float[] _fParams = new float[100];
        private const int INT_FOV_Size = 8;
        private const float F_MM2UM = 1000;
        private const float F_UM2MM = 0.001f;

        private byte[] _imgByte2DRFov = null;//whole image rgb
        private byte[] _imgByte2DGFov = null;
        private byte[] _imgByte2DBFov = null;
        private byte[] _r = null;
        private byte[] _g = null;
        private byte[] _b = null;

        private string _strDataLinkFrAOIDir = "";
        private string _strDataLinkToAOIDir = "";

        public String SaveFovImagesInfo(string strFovDataFileLoadOverByUI,string AstFirstFovPath,string strBarcode,string strDate,DateTime dtStartTime,
            List<APP_SaveOrLoadImgDataInFo> AFovs,
            ref AppSettingData AappSettingData,
            ref InspectConfig.ConfigData Aconfig,
            bool bNeedZoom, Basefunction _csBaseF,
            string AstrPerWidthHeight, string AtrDataExportSaveFovImagePath,
            ImgCSCoreIM.ST_SaveOrLoadImgData stSave,
            String ExToFOVImagePath, int AiCount,
            string strIp
            )
        {
            string strMsg = string.Empty;
            try
            {
                //if (ABrdRes.jugResult == JudgeRes.Unmeasured)
                //    return;
                
                int index = 0;
                int iFovCount = AiCount;//AFovs.Count;
                unsafe
                {
                    //for (index = 0; index < iFovCount; ++index)
                    //{
                    //    if (AFovs[index].stImgData.byImgDataR == null)
                    //    {
                    //        strMsg = "Fov Img is Null";
                    //        return strMsg;
                    //    }
                    //}
                }
                if (iFovCount>0)
                {
                    _csBaseF.LoadImadat(AstFirstFovPath + "\\" + 0, ref AFovs[0].stImgData);
                }
                int AiFOVWidth = AFovs[0].stImgData.iWidth;
                int AiFOVHeight = AFovs[0].stImgData.iHeight;
                float AfPixelSizeX0um = Aconfig._pixelSizeX0, AfPixelSizeY0um = Aconfig._pixelSizeY0;
                //AappSettingData.stDataExpVT.strSaveFovImagePath = "D:\\FOV";
                //AappSettingData.stDataExpVT.bEnSaveFovImage = true;
                if (
                    AappSettingData.stDataExpVT.bEnSaveFovImage == false)//AappSettingData.bEnDataExp == false
                {
                    strMsg = "bEnSaveFovImage is false!";
                    return strMsg;
                }
                ////Q.F.2017.04.24
                // _csBaseF.GetSaveFovImagePath(ref ABrdRes, AstrDir, ref AappSettingData);
                if (string.IsNullOrEmpty(AappSettingData.stDataExpVT.strSaveFovImagePath))
                    return "SaveFovImagePath is null!";
                //string strFovImageDir = AstrDir;
                //if(!string.IsNullOrEmpty(AappSettingData.stDataExpVT.strSaveFovImagePath))
                //{
                //    strFovImageDir = AappSettingData.stDataExpVT.strSaveFovImagePath;
                //    if (_csBaseF.DirCheck(ref strFovImageDir) == -1)
                //    {
                //        strFovImageDir = AstrDir;                        
                //    }
                //}


                //if (_csBaseF.DirCheck(ref strFovImageDir) == -1)
                //    return;
                //get board barcode
                String strboardBarcode = strBarcode;
                bool bUse1DImage = false;
                if (Aconfig._VAHMsrPara_bFastMode == true && Aconfig._checkGage == false)
                    bUse1DImage = true;


                //save fov images
                //strFovImageDir = strFovImageDir + RS_DATAEXPORTFovImageSubDir;
                //String strFovImageDirDay = RS_EMPTY;
                String strFovImageDirPCB = string.Empty;
                if (AappSettingData.stDataExpVT.bEnSaveFovImage == true)
                {
                    //Q.F.2017.04.19
                    strFovImageDirPCB = AtrDataExportSaveFovImagePath;//// strFovImageDirDay + strboardBarcode + RS_UnderLine + ABrdRes.pcbID;
                    if (string.IsNullOrEmpty(strIp))
                    {

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(AtrDataExportSaveFovImagePath) == false)
                        {
                            string strBaseDirName = AtrDataExportSaveFovImagePath.Split(':').First();

                            string strLastDirName = AtrDataExportSaveFovImagePath.Split(':').Last();

                            strFovImageDirPCB = strBaseDirName + ":\\"+ strIp+ strLastDirName;
                        }

                    }
                    //strFovImageDirPCB = Path.Combine(strFovImageDirPCB, strDate + "_" + strBarcode);
                    //如果UI勾选 AappSettingData.bEnGenAOIFile 
                    //if (AappSettingData.bEnGenAOIFile || AappSettingData.bEnLinkToAOIStation)
                    //{
                    //    strFovImageDirPCB = AappSettingData.stDataExpVT.strSaveFovImagePath;
                    //    if (string.IsNullOrEmpty(strFovImageDirPCB))
                    //    {
                    //        strFovImageDirPCB = Path.Combine(ExToFOVImagePath, "FOVImage");
                    //        strFovImageDirPCB = Path.Combine(strFovImageDirPCB, strboardBarcode + RS_strUnderLine + dtStartTime.ToString(RS_strTimeFormat));
                    //    }
                    //    else
                    //    {
                    //        strFovImageDirPCB = Path.Combine(strFovImageDirPCB, "FOVImage");
                    //        strFovImageDirPCB = Path.Combine(strFovImageDirPCB, strboardBarcode + RS_strUnderLine + dtStartTime.ToString(RS_strTimeFormat));
                    //    }
                    //    if (Directory.Exists(strFovImageDirPCB) == false)
                    //    {
                    //        Directory.CreateDirectory(strFovImageDirPCB);
                    //    }
                    //}
                    if (_csBaseF.DirCheck(ref strFovImageDirPCB) == -1)
                        return strFovImageDirPCB + "Could not be created;"; ;
                    // _csCrImg.SaveFovImages(strFovImageDir, AFovs,AiFOVWidth,AiFOVHeight,AappSettingData);
                    //_csBaseF.LogRecord("SaveFovImage Path:" + strFovImageDirPCB, false);

                    int iProcessorCount = Environment.ProcessorCount - 1;
                    if (iProcessorCount <= 0)
                        iProcessorCount = 1;


                    int iCount = AiCount;//AFovs.Count;
                    String[] strFovComponentInfo = new String[iCount];//Q.F.2017.06.06
                    Stopwatch stop = new Stopwatch();
                    stop.Start();

                    Rectangle rect = new Rectangle(0, 0, AiFOVWidth, AiFOVHeight);
                    int iWidthNew = 0;
                    int iHeightNew = 0;
                    bool bAdjustWIdth = false;
                    if (AappSettingData.stDataExpVT.byFovImageAdjustPercent < 100)
                    {
                        bAdjustWIdth = true;
                        iWidthNew = (int)System.Math.Ceiling(AiFOVWidth * AappSettingData.stDataExpVT.byFovImageAdjustPercent * 0.01);
                        iHeightNew = (int)System.Math.Ceiling(AiFOVHeight * AappSettingData.stDataExpVT.byFovImageAdjustPercent * 0.01);
                    }
                    else if (bNeedZoom && Aconfig._bEnGenROiCADImg == false)//Q.F.2017.08.30
                    {
                        bAdjustWIdth = true;
                        iWidthNew = int.Parse(AstrPerWidthHeight.Split('@')[0]);
                        iHeightNew = int.Parse(AstrPerWidthHeight.Split('@')[1]);
                    }
                    //iWidthNew = AiFOVWidth; iHeightNew = AiFOVHeight;
                    //byte[] aTmp = new byte[256];

                    int intRange = (int)(iCount / _iProcessorCount) + 1;

                    Rectangle rectNew = new Rectangle(0, 0, iWidthNew, iHeightNew);
                    int lImageLength = AiFOVWidth * AiFOVHeight;
                    int lImageLengthNew = iWidthNew * iHeightNew;
                    ImgCSCoreIM.ColorFactorParams stColorFactors = AappSettingData.stColorFactorParams;
                    bool bGenROICADImg = Aconfig._bEnGenROiCADImg;
                    double MinXMM = AFovs[0].fPosXmm;
                    double MaxXMM = AFovs[0].fPosXmm;
                    double MinYMM = AFovs[0].fPosYmm;
                    double MaxYMM = AFovs[0].fPosYmm;

                    for (index = 1; index < iFovCount; ++index)
                    {
                        if (MinXMM > AFovs[index].fPosXmm)//correctedXmm
                        {
                            MinXMM = AFovs[index].fPosXmm;
                        }
                        if (MaxXMM < AFovs[index].fPosXmm)
                        {
                            MaxXMM = AFovs[index].fPosXmm;
                        }
                        if (MinYMM > AFovs[index].fPosYmm)//correctedYmm
                        {
                            MinYMM = AFovs[index].fPosYmm;
                        }
                        if (MaxYMM < AFovs[index].fPosYmm)
                        {
                            MaxYMM = AFovs[index].fPosYmm;
                        }
                    }
                    
                    
                    int lLength = AiFOVWidth * AiFOVHeight;
                    //if (_imgByte2DR == null || _imgByte2DR.Length != lLength)
                    //{
                    //    _imgByte2DR = new byte[lLength];
                    //    _imgByte2DG = new byte[lLength];
                    //    _imgByte2DB = new byte[lLength];
                    //}
                    //Array.Clear(_imgByte2DR, 0, lLength);
                    //Array.Clear(_imgByte2DG, 0, lLength);
                    //Array.Clear(_imgByte2DB, 0, lLength);

                    if (_imgByte2DRFov == null || _imgByte2DRFov.Length != lLength)
                    {
                        _imgByte2DRFov = new byte[lImageLength];
                        _imgByte2DGFov = new byte[lImageLength];
                        _imgByte2DBFov = new byte[lImageLength];
                    }
                    Array.Clear(_imgByte2DRFov, 0, lImageLength);
                    Array.Clear(_imgByte2DGFov, 0, lImageLength);
                    Array.Clear(_imgByte2DBFov, 0, lImageLength);

                    //Parallel.ForEach(Partitioner.Create(0, iCount, intRange), range =>
                    //{
                    //Parallel.ForEach(Partitioner.Create(0, iCount, (int)(iCount / _iProcessorCount) + 1), range =>
                    //{
                    //for(int iCount=0,)
                        if (_r == null || _r.Length != lLength)
                        {
                            _r = new byte[lImageLengthNew];
                            _g = new byte[lImageLengthNew];
                            _b = new byte[lImageLengthNew];
                        }
                        //for (int iIndex = range.Item1; iIndex < range.Item2; ++iIndex)
                        //{
                        for (int iIndex = 0; iIndex < iCount; ++iIndex)
                        {
                            strFovComponentInfo[iIndex] = String.Empty;
                            string strFileName = String.Empty;
                            //int iIndex = a / intRange;//a - range.Item1;
                            int iIndexRGB = 0;
                            string strFovImgNo = (iIndex + 1).ToString();

                            //Q.F.2017.04.19
                            //swap color
                            //int capType = AFovs[a].capType;
                            _csBaseF.LoadImadat(AstFirstFovPath + "\\" + iIndex.ToString(), ref AFovs[0].stImgData);
                            
                            float fFactor = 1.0f;
                            int i = 0, j = 0;
                            short shColorPer = 0;
                            unsafe
                            {
                                // recal rgb value
                                if (stColorFactors.byRGB2DFactorR != 100)
                                {
                                    //fFactor = stColorFactors.byRGB2DFactorR * 0.01f;
                                    shColorPer = (short)(stColorFactors.byRGB2DFactorR * 1.28);
                                    for (i = 0; i < lImageLength; ++i)
                                    {
                                        //R1[i] = (byte)(AFovs[a]._rgbImg[0][i] * fFactor);
                                        _imgByte2DRFov[i] = (byte)((AFovs[0].stImgData.byImgDataR[i] * shColorPer) >> 7);
                                        // R1[i] = aTmp[AFovs[a]._rgbImg[0][i]];
                                    }
                                }
                                else
                                {
                                    //Marshal.Copy((IntPtr)((void*)AFovs[a]._rgbImg[0]), R1, 0,
                                    //   lImageLength);
                                    Array.Copy(AFovs[0].stImgData.byImgDataR, _imgByte2DRFov, lImageLength);
                                }

                                if (stColorFactors.byRGB2DFactorG != 100)
                                {
                                    //fFactor = stColorFactors.byRGB2DFactorR * 0.01f;
                                    shColorPer = (short)(stColorFactors.byRGB2DFactorG * 1.28);
                                    for (i = 0; i < lImageLength; ++i)
                                    {
                                        // R1[i] = (byte)(AFovs[a]._rgbImg[0][i] * fFactor);
                                        _imgByte2DGFov[i] = (byte)((AFovs[0].stImgData.byImgDataG[i] * shColorPer) >> 7);
                                    }
                                }
                                else
                                {
                                    //Marshal.Copy((IntPtr)((void*)AFovs[a]._rgbImg[1]), G1, 0,
                                    //   lImageLength);
                                    Array.Copy(AFovs[0].stImgData.byImgDataG, _imgByte2DGFov, lImageLength);
                                }
                                if (stColorFactors.byRGB2DFactorB != 100)
                                {
                                    //fFactor = stColorFactors.byRGB2DFactorR * 0.01f;
                                    shColorPer = (short)(stColorFactors.byRGB2DFactorB * 1.28);
                                    for (i = 0; i < lImageLength; ++i)
                                    {
                                        // R1[i] = (byte)(AFovs[a]._rgbImg[0][i] * fFactor);
                                        _imgByte2DBFov[i] = (byte)((AFovs[0].stImgData.byImgDataB[i] * shColorPer) >> 7);
                                    }
                                }
                                else
                                {
                                    //Marshal.Copy((IntPtr)((void*)AFovs[a]._rgbImg[2]), B1, 0,
                                    //   lImageLength);
                                    Array.Copy(AFovs[0].stImgData.byImgDataB, _imgByte2DBFov, lImageLength);
                                }
                            }
                            int iSaveImageMode = 1;
                            if (bGenROICADImg == false)
                            {
                                if (iSaveImageMode >= 2)
                                {
                                    iSaveImageMode = 1;
                                }
                                strFileName = strFovImageDirPCB + strFovImgNo + RS_DATAEXPORTImageExt;// ".Jpeg";
                            }
                            else
                            {
                                iSaveImageMode = 2;
                                strFileName = strFovImageDirPCB + strFovImgNo + ".bmp";// ".Jpeg";
                            }

                            if (bAdjustWIdth == false)
                            {

                                switch (iSaveImageMode)
                                {
                                    case 0:
                                        {
                                            unsafe
                                            {
                                                Bitmap resultBitMap = new Bitmap(AiFOVWidth, AiFOVHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                                                //Rectangle rect = new Rectangle(0, 0, AiFOVWidth, AiFOVHeight);
                                                System.Drawing.Imaging.BitmapData bmpData = resultBitMap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, resultBitMap.PixelFormat);
                                                IntPtr iPtr = bmpData.Scan0;
                                                int iStride = bmpData.Stride;
                                                int iBytes = iStride * AiFOVHeight;
                                                byte[] PixelValues = new byte[iBytes];
                                                int iPoint = 0;
                                                int offSet = iStride - AiFOVWidth * 3;
                                                byte byG = 0, byB = 0, byR = 0;
                                                short value;
                                                //
                                                for (i = 0; i < AiFOVHeight; ++i)
                                                {
                                                    for (j = 0; j < AiFOVWidth; ++j)
                                                    {
                                                        iIndexRGB = i * AiFOVWidth + j;
                                                        //byG = AFovs[a]._rgbImg[1][iIndexRGB];
                                                        //byB = AFovs[a]._rgbImg[2][iIndexRGB];
                                                        //byR = AFovs[a]._rgbImg[0][iIndexRGB];
                                                        byG = _imgByte2DGFov[iIndexRGB];
                                                        byB = _imgByte2DBFov[iIndexRGB];
                                                        byR = _imgByte2DRFov[iIndexRGB];

                                                        PixelValues[iPoint++] = byB;
                                                        PixelValues[iPoint++] = byG;
                                                        PixelValues[iPoint++] = byR;
                                                    }

                                                    // iPoint += 3;               
                                                    iPoint += offSet;
                                                }
                                                //
                                                System.Runtime.InteropServices.Marshal.Copy(PixelValues, 0, iPtr, iBytes);
                                                resultBitMap.UnlockBits(bmpData);
                                                if (bGenROICADImg == false)//Q.F.2017.08.30
                                                {
                                                    //strFileName = strFovImageDirPCB + strFovImgNo + RS_DATAEXPORTImageExt;// ".Jpeg";
                                                    resultBitMap.Save(strFileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                                                }
                                                else
                                                {
                                                    //strFileName = strFovImageDirPCB + strFovImgNo + ".bmp";// ".Jpeg";
                                                    resultBitMap.Save(strFileName, System.Drawing.Imaging.ImageFormat.Bmp);
                                                }
                                                if (resultBitMap != null)
                                                {
                                                    resultBitMap.Dispose();
                                                }
                                            }
                                            break;
                                        }
                                    case 1:
                                        {
                                            //ImgCSCoreIM.ST_SaveOrLoadImgData stSave = new ImgCSCoreIM.ST_SaveOrLoadImgData();
                                            stSave.iImgFormat = 1;
                                            stSave.bUseOpcv = false;
                                            stSave.byImgDataR = _imgByte2DRFov;
                                            stSave.byImgDataG = _imgByte2DGFov;
                                            stSave.byImgDataB = _imgByte2DBFov;
                                            stSave.iWidth = AiFOVWidth;
                                            stSave.iHeight = AiFOVHeight;
                                            _csBaseF.SaveImg(strFileName, stSave, 100);
                                            break;

                                        }
                                    case 2:
                                        {
                                            //ImgCSCoreIM.ST_SaveOrLoadImgData stSave = new ImgCSCoreIM.ST_SaveOrLoadImgData();
                                            stSave.iImgFormat = 0;
                                            stSave.bUseOpcv = true;
                                            stSave.byImgDataR = _imgByte2DRFov;
                                            stSave.byImgDataG = _imgByte2DGFov;
                                            stSave.byImgDataB = _imgByte2DBFov;
                                            stSave.iWidth = AiFOVWidth;
                                            stSave.iHeight = AiFOVHeight;
                                            _csBaseF.SaveImg(strFileName, stSave, 0);
                                            break;

                                        }
                                    default:
                                        break;


                                }

                            }
                            else
                            {
                                unsafe
                                {
                                    Array.Clear(_r, 0, lImageLengthNew);
                                    Array.Clear(_g, 0, lImageLengthNew);
                                    Array.Clear(_b, 0, lImageLengthNew);

                                    Array.Clear(_fParams, 0, _fParams.Length);
                                    _fParams[0] = AiFOVWidth;
                                    _fParams[1] = AiFOVHeight;
                                    _fParams[2] = iWidthNew;
                                    _fParams[3] = iHeightNew;
                                    _fParams[4] = 0;
                                    _csBaseF.ResizeImgByOpcv(_imgByte2DRFov,
                                        _imgByte2DGFov,
                                        _imgByte2DBFov,
                                        _r, _g, _b, _fParams);

                                    switch (iSaveImageMode)
                                    {
                                        case 0:
                                            {
                                                Bitmap resultBitMap = new Bitmap(iWidthNew, iHeightNew, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                                                //Rectangle rect = new Rectangle(0, 0, AiFOVWidth, AiFOVHeight);
                                                System.Drawing.Imaging.BitmapData bmpData = resultBitMap.LockBits(rectNew, System.Drawing.Imaging.ImageLockMode.ReadWrite, resultBitMap.PixelFormat);
                                                IntPtr iPtr = bmpData.Scan0;
                                                int iStride = bmpData.Stride;
                                                int iBytes = iStride * iHeightNew;
                                                byte[] PixelValues = new byte[iBytes];
                                                int iPoint = 0;
                                                int offSet = iStride - iWidthNew * 3;
                                                byte byG = 0, byB = 0, byR = 0;
                                                short value;
                                                //
                                                for (i = 0; i < iHeightNew; ++i)
                                                {
                                                    for (j = 0; j < iWidthNew; ++j)
                                                    {
                                                        iIndexRGB = i * iWidthNew + j;
                                                        byG = _g[iIndexRGB];
                                                        byB = _b[iIndexRGB];
                                                        byR = _r[iIndexRGB];

                                                        PixelValues[iPoint++] = byB;
                                                        PixelValues[iPoint++] = byG;
                                                        PixelValues[iPoint++] = byR;
                                                    }


                                                    // iPoint += 3;               
                                                    iPoint += offSet;
                                                }
                                                //
                                                System.Runtime.InteropServices.Marshal.Copy(PixelValues, 0, iPtr, iBytes);
                                                resultBitMap.UnlockBits(bmpData);

                                                // strFileName = strFovImageDirPCB + (a + 1).ToString() + ".Jpeg";// ".Jpeg";
                                                //strFileName = strFovImageDirPCB + strFovImgNo + RS_DATAEXPORTImageExt;// ".Jpeg";
                                                //strFileName = Path.Combine(strFovImageDirPCB,strD);
                                                resultBitMap.Save(strFileName, System.Drawing.Imaging.ImageFormat.Jpeg);

                                                if (resultBitMap != null)
                                                {
                                                    resultBitMap.Dispose();
                                                }
                                                PixelValues = null;
                                                break;
                                            }
                                        case 1:
                                            {
                                                //ImgCSCoreIM.ST_SaveOrLoadImgData stSave = new ImgCSCoreIM.ST_SaveOrLoadImgData();
                                                stSave.iImgFormat = 1;
                                                stSave.bUseOpcv = false;
                                                stSave.byImgDataR = _r;
                                                stSave.byImgDataG = _g;
                                                stSave.byImgDataB = _b;
                                                stSave.iWidth = iWidthNew;
                                                stSave.iHeight = iHeightNew;
                                                if (_bmp == null || _bmp.Width * _bmp.Height != iWidthNew * iHeightNew)
                                                {
                                                    if (_bmp != null && _bmp.Width * _bmp.Height != iWidthNew * iHeightNew)
                                                    {
                                                        _bmp.Dispose();
                                                    }
                                                    _bmp = new Bitmap(iWidthNew, iHeightNew, PixelFormat.Format24bppRgb);
                                                }
                                                
                                                _csBaseF.SaveImg(strFileName, stSave, 70, true, _bmp);
                                                break;
                                            }
                                        case 2:
                                            {
                                                //ImgCSCoreIM.ST_SaveOrLoadImgData stSave = new ImgCSCoreIM.ST_SaveOrLoadImgData();
                                                stSave.iImgFormat = 0;
                                                stSave.bUseOpcv = true;
                                                stSave.byImgDataR = _r;
                                                stSave.byImgDataG = _g;
                                                stSave.byImgDataB = _b;
                                                stSave.iWidth = iWidthNew;
                                                stSave.iHeight = iHeightNew;
                                                if (_bmp == null || _bmp.Width * _bmp.Height != iWidthNew * iHeightNew)
                                                {
                                                    if (_bmp != null && _bmp.Width * _bmp.Height != iWidthNew * iHeightNew)
                                                    {
                                                        _bmp.Dispose();
                                                    }
                                                    _bmp = new Bitmap(iWidthNew, iHeightNew, PixelFormat.Format24bppRgb); 
                                                }
                                                _csBaseF.SaveImg(strFileName, stSave, 0,true,_bmp);
                                                break;
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }
                            }
                            ////Q.F.2017.06.07
                            ////record the fov info
                            //if (AFovs[a].stFovInfo.ComponentArrayName != null)
                            //{
                            //    for (i = 0, j = AFovs[a].stFovInfo.ComponentArrayName.Length; i < j; ++i)
                            //    {
                            //        AFovs[a].stFovInfo.ComponentArrayName[a] += AFovs[a].stFovInfo.ComponentArrayName[i]
                            //            + RS_LineEnd;
                            //    }
                            //}

                            }
                        //});
                        ////Q.F.2017.06.07
                        ////record the fov info
                        //StringBuilder strFovInfo = new StringBuilder();
                        //strFovInfo.Append("COMPONENTID_ARRAYID" + ",");
                        //strFovInfo.Append("FovNo" + "\r\n");
                        //int n = 0, iLength = 0;
                        //int m = 0, iCLength = 0;
                        //for (n = 0, iLength = AFovs.Length; n < iLength; ++n)
                        //{
                        //    if (AFovs[n].stFovInfo.ComponentArrayName != null)
                        //    {
                        //        for (m = 0, iCLength = AFovs[n].stFovInfo.ComponentArrayName.Length; m < iCLength; ++m)
                        //        {
                        //            strFovInfo.Append(AFovs[n].stFovInfo.ComponentArrayName[m] + RS_SPLIT);
                        //            strFovInfo.Append((n + 1) + RS_LineEnd);
                        //        }

                        //    }
                        //}
                        //string strFileNameFovInfo = strFovImageDirPCB + strboardBarcode + RS_CSV_EXT;// ".Jpeg";
                        //FileStream fs = new FileStream(strFileNameFovInfo, FileMode.OpenOrCreate);
                        //System.IO.StreamWriter streamWrt = new StreamWriter(fs, Encoding.Default);
                        //streamWrt.Write(strFovInfo);
                        //streamWrt.Close();
                        //stop.Stop();

                        //   MessageBox.Show(stop.ElapsedMilliseconds.ToString());
                    //});

                    //copy 三点照合需要的txt.csv文件
                    //if (AappSettingData.bEnGenAOIFile == true)
                    //{
                    //    string strFile = Path.Combine(AstFirstFovPath, strBarcode + "_" + dtStartTime.ToString(RS_strTimeFormat) + ".csv");

                    //    //ToAOI
                    //    if (File.Exists(strFile))
                    //    {
                    //        File.Copy(strFile, Path.Combine(AappSettingData.stDataExpVT.strSaveFovImagePath, strBarcode + "_" + dtStartTime.ToString(RS_strTimeFormat) + ".csv"), true);
                    //        System.Threading.Thread.Sleep(300);
                    //        //File.Copy(strFile, Path.Combine(AappSettingData.stDataExpVT.strSaveFovImagePath, "FOVImage", strBarcode + "_" + dtStartTime.ToString(RS_strTimeFormat) + ".csv"), true);
                    //        //System.Threading.Thread.Sleep(100);
                    //    }
                    //    strFile = Path.Combine(AstFirstFovPath, strBarcode + "_" + dtStartTime.ToString(RS_strTimeFormat) + ".txt");
                    //    // 模拟来自FrAoi
                    //    if (File.Exists(strFile))
                    //    {
                    //        File.Copy(strFile, Path.Combine(@"D:\EYSPI\DataExport\LinkFrAOI", strBarcode + "_" + dtStartTime.ToString(RS_strTimeFormat) + ".txt"), true);
                    //        System.Threading.Thread.Sleep(100);
                    //    }
                    //}
                        if (File.Exists(strFovDataFileLoadOverByUI))
                        {
                            string strPosFile = Path.Combine(strFovImageDirPCB,Path.GetFileName(strFovDataFileLoadOverByUI));
                            File.Copy(strFovDataFileLoadOverByUI,strPosFile,true);
                        }

                }


            }
            catch (System.Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                strMsg = ex.Message;
            }
            finally
            {
            }
           _imgByte2DRFov = null;
           _imgByte2DGFov = null;
           _imgByte2DBFov = null;
           _r = null;
           _g = null;
           _b = null;

            return strMsg;
        }

        Bitmap _bmp ;
        private string RS_strTimeFormat = "yyyyMMddHHmmss";
        private string RS_strUnderLine = "_";
    }
    
}
