using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppLayerLib;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Drawing;
using ImgCSCoreIM;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
namespace AutoAPP
{
    class SavePCBImages
    {

        private const float F_MM2UM = 1000;
        private const float F_UM2MM = 0.001f;
        private static int _iProcessorCount = Environment.ProcessorCount;
        private bool _bHasGetBoardBackGroudColor = false;
        private byte[] _arrBoardBackGroudColor = null;
        private String _strCurrentJobName = String.Empty;
        private byte[] _imgByte2DR = null;//whole image rgb
        private byte[] _imgByte2DG = null;
        private byte[] _imgByte2DB = null;
        private ocvMngCPPDLL.CvFunctionInterface CvFunction = new ocvMngCPPDLL.CvFunctionInterface();
        private byte[] _imgByte2DRFov = null;//whole image rgb
        private byte[] _imgByte2DGFov = null;
        private byte[] _imgByte2DBFov = null;

        private byte[] _imgByte2DRReSize = null;//whole image rgb
        private byte[] _imgByte2DGReSize = null;
        private byte[] _imgByte2DBReSize = null;

        //private Basefunction _baseFuc = new Basefunction();
        private float[] _fParams = new float[100];
        public SavePCBImages()
        {
            _arrBoardBackGroudColor = new byte[3];
            CvFunction.Init();
        }

        public void SaveWholePCBImage(
            string strBarcode,
            string strDateTime,
            String strJobName,
            List<APP_SaveOrLoadImgDataInFo> AFovs,
           ref AppSettingData AappSettingData,
           ref InspectConfig.ConfigData Aconfig,
            DateTime AdtStartTime,
            string AstrPerWidthHeight,

            Basefunction _baseFuc,
            ImgCSCoreIM.ST_SaveOrLoadImgData stSave,
            string ExToFovImagePath,string ip
           )
        {
            if (AappSettingData.stDataExpVT.stSavePCBImageParams.bEnabled == false)
                return;

            String strPre = "SaveWholePCBImage:";
            String strMsg = SaveWholePCBImagePrivate(strBarcode, strDateTime, strJobName,
                AFovs,
                ref AappSettingData,
           
                ref Aconfig, AdtStartTime, AstrPerWidthHeight, _baseFuc,stSave,ip);
            if (String.IsNullOrEmpty(strMsg)
                   || String.Equals(strMsg.ToLower().Trim(), "true"))//Q.F.2017.04.06
            {
                strMsg = strPre + "Success!";


            }
            else
            {
                strMsg = strPre + strMsg;
            }
            strMsg += "\tOpID:" + AappSettingData.stDataExpVT.strOperatorID;
            //if (_csBaseF != null)
            //    _csBaseF.LogRecord(strMsg, false);
        }
        private String SaveWholePCBImagePrivate(string strBarcode, string strDateTime, String strJobName,
            List<APP_SaveOrLoadImgDataInFo> AFovs,

            ref AppSettingData AappSettingData,
            ref InspectConfig.ConfigData Aconfig,
            DateTime AdtStartTime,
            string AstrPerWidthHeight,
            Basefunction _baseFuc,
             ImgCSCoreIM.ST_SaveOrLoadImgData stSave,string ip
            )
        {
            //String strPre = "SaveWholePCBImage:";
            String strMsg = string.Empty;
            try
            {
                if (String.IsNullOrEmpty(_strCurrentJobName) || !String.Equals(_strCurrentJobName, strJobName))
                {
                    _strCurrentJobName = strJobName;
                    _bHasGetBoardBackGroudColor = false;
                    Array.Clear(_arrBoardBackGroudColor, 0, _arrBoardBackGroudColor.Length);
                }
                strJobName = Path.GetFileNameWithoutExtension(strJobName);
                if (AFovs == null)
                {
                    strMsg = "Fov Img is Null";
                    return strMsg;

                }


                //if (ABrdRes.jugResult == JudgeRes.Unmeasured)
                //    return (strMsg = ABrdRes.jugResult.ToString());
                int AiFOVWidth = 0, AiFOVHeight = 0;
                float AfPixelSizeX0um = Aconfig._pixelSizeX0, AfPixelSizeY0um = Aconfig._pixelSizeY0;
                AfPixelSizeX0um = 19.95233f; AfPixelSizeY0um = 19.9552f;
                int index = 0;
                int iFovCount = AFovs.Count;
                unsafe
                {
                    for (index = 0; index < iFovCount; ++index)
                    {
                        if (AFovs[index].stImgData.byImgDataR == null)
                        {
                            strMsg = "Fov Img is Null";
                            return strMsg;
                        }
                    }
                }
                String strDayTime = strDateTime;

                string strPath = AappSettingData.stDataExpVT.stSavePCBImageParams.strSaveImagePath;
                if (string.IsNullOrEmpty(strPath) == false)
                {
                    strPath = strPath + "\\" + ip;
                }
                if (_baseFuc.DirCheck(ref strPath) == -1)
                {
                    strMsg = strPath + "Could not be created;";
                    return strMsg;
                }
                //get board barcode
                String strboardBarcode = strBarcode;
                //    AappSettingData, (byte)ABrdRes.LaneNo);//Q.F.2017.04.13
                String strTime = AdtStartTime.ToString("yyyyMMddHHmmss");
                if (string.Equals(strboardBarcode.ToUpper(), "NOREAD"))
                {
                    strboardBarcode = strTime;
                }
                string strFileName = "";
                if (AappSettingData.stDataExpVT.stSavePCBImageParams.bEnabled == true)
                {
                    switch ((EM_SavePCBImageCustomer)AappSettingData.stDataExpVT.stSavePCBImageParams.Customer)
                    {
                        case EM_SavePCBImageCustomer.HuaWei:
                            {
                                if (!string.IsNullOrEmpty(AappSettingData.stDataExpVT.stSavePCBImageParams.TaskOrder))
                                {
                                    strPath = Path.Combine(strPath, AappSettingData.stDataExpVT.stSavePCBImageParams.TaskOrder);
                                    if (_baseFuc.DirCheck(ref strPath) == -1)
                                    {
                                        strMsg = strPath + "Could not be created;";
                                        return strMsg;
                                    }
                                    //strPath = Path.Combine(strPath, strboardBarcode);
                                    //if (_baseFuc.DirCheck(ref strPath) == -1)
                                    //{
                                    //    strMsg = strPath + "Could not be created;";
                                    //    return strMsg;
                                    //}
                                }
                                else
                                {
                                    strPath = Path.Combine(strPath, strDayTime);
                                    if (_baseFuc.DirCheck(ref strPath) == -1)
                                    {
                                        strMsg = strPath + "Could not be created;";
                                        return strMsg;
                                    }

                                }
                                strFileName = strboardBarcode + PubStaticParam.RS_UnderLineSplit + strTime + PubStaticParam.RS_UnderLineSplit + strJobName;
                                strFileName = Path.Combine(strPath, strFileName);
                                break;
                            }
                        case EM_SavePCBImageCustomer.JobBarcodePerDay:
                            {
                                strPath = Path.Combine(strPath, strDayTime);
                                if (_baseFuc.DirCheck(ref strPath) == -1)
                                {
                                    strMsg = strPath + "Could not be created;";
                                    return strMsg;
                                }
                                strFileName = Path.Combine(strPath, strJobName + PubStaticParam.RS_UnderLineSplit + strboardBarcode + PubStaticParam.RS_UnderLineSplit + strTime);
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
                AiFOVWidth = AFovs[0].stImgData.iWidth;
                AiFOVHeight = AFovs[0].stImgData.iHeight;
                int intRange = (int)(iFovCount / _iProcessorCount) + 1;
                int lImageLength = AiFOVWidth * AiFOVHeight;
                ImgCSCoreIM.ColorFactorParams stColorFactors = AappSettingData.stColorFactorParams;

                //cal Hue for backgroud
                int hue = 0, saturation = 0, value = 0;

                Color rgb = Color.Black;
                //cal larger image size              
                int WholeImageWidth = 0, WholeImageHeight = 0;

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
                WholeImageWidth = (int)Math.Ceiling((MaxXMM - MinXMM) * F_MM2UM / AfPixelSizeX0um);
                WholeImageHeight = (int)Math.Ceiling((MaxYMM - MinYMM) * F_MM2UM / AfPixelSizeY0um);
                WholeImageWidth += AiFOVWidth;
                WholeImageHeight += AiFOVHeight;
                int lLength = WholeImageWidth * WholeImageHeight;
                if (_imgByte2DR == null || _imgByte2DR.Length != lLength)
                {
                    _imgByte2DR = new byte[lLength];
                    _imgByte2DG = new byte[lLength];
                    _imgByte2DB = new byte[lLength];
                }
                Array.Clear(_imgByte2DR, 0, lLength);
                Array.Clear(_imgByte2DG, 0, lLength);
                Array.Clear(_imgByte2DB, 0, lLength);


                if (_imgByte2DRFov == null || _imgByte2DRFov.Length != lLength)
                {
                    _imgByte2DRFov = new byte[lImageLength];
                    _imgByte2DGFov = new byte[lImageLength];
                    _imgByte2DBFov = new byte[lImageLength];
                }
                Array.Clear(_imgByte2DRFov, 0, lImageLength);
                Array.Clear(_imgByte2DGFov, 0, lImageLength);
                Array.Clear(_imgByte2DBFov, 0, lImageLength);


                float fFovWMM = AiFOVWidth * AfPixelSizeX0um * 0.001f;
                float fFovHMM = AiFOVHeight * AfPixelSizeY0um * 0.001f;
                double LeftXMM = MinXMM;// -fFovWMM * 0.5;
                double LeftYMM = MinYMM;// -fFovHMM * 0.5;
                //for (index = 0; index < lLength; ++index)
                //{
                //    _imgByte2DG[index] = rgb.G;
                //    _imgByte2DB[index] = rgb.B;
                //    _imgByte2DB[index] = rgb.R;
                //}
                int i = 0, j = 0;
                float fFactor = 1.0f;
                short shColorPer = 0;
                //Parallel.ForEach(Partitioner.Create(0, iFovCount, intRange), range =>
                //{
                //for (int a = range.Item1; a < range.Item2; ++a)
                for (index = 0; index < iFovCount; ++index)
                {
                    unsafe
                    {
                        // recal rgb value
                        if (stColorFactors.byRGB2DFactorR != 100)
                        {
                            shColorPer = (short)(stColorFactors.byRGB2DFactorR * 1.28);
                            for (i = 0; i < lImageLength; ++i)
                            {
                                _imgByte2DRFov[i] = (byte)((AFovs[index].stImgData.byImgDataR[i] * shColorPer) >> 7);
                            }
                        }
                        else
                        {
                            Array.Copy(AFovs[index].stImgData.byImgDataR, _imgByte2DRFov, lImageLength);
                        }
                        if (stColorFactors.byRGB2DFactorG != 100)
                        {
                            //fFactor = stColorFactors.byRGB2DFactorR * 0.01f;
                            shColorPer = (short)(stColorFactors.byRGB2DFactorG * 1.28);
                            for (i = 0; i < lImageLength; ++i)
                            {
                                // _imgByte2DRFov[i] = (byte)(AFovs[a]._rgbImg[0][i] * fFactor);
                                _imgByte2DGFov[i] = (byte)((AFovs[index].stImgData.byImgDataG[i] * shColorPer) >> 7);
                            }
                        }
                        else
                        {
                            Array.Copy(AFovs[index].stImgData.byImgDataG, _imgByte2DGFov, lImageLength);
                        }
                        if (stColorFactors.byRGB2DFactorB != 100)
                        {
                            //fFactor = stColorFactors.byRGB2DFactorR * 0.01f;
                            shColorPer = (short)(stColorFactors.byRGB2DFactorB * 1.28);
                            for (i = 0; i < lImageLength; ++i)
                            {
                                // _imgByte2DRFov[i] = (byte)(AFovs[a]._rgbImg[0][i] * fFactor);
                                _imgByte2DBFov[i] = (byte)((AFovs[index].stImgData.byImgDataB[i] * shColorPer) >> 7);
                            }
                        }
                        else
                        {
                            Array.Copy(AFovs[index].stImgData.byImgDataB, _imgByte2DBFov, lImageLength);
                        }
                    }
                    int iShiftX = (int)Math.Ceiling((AFovs[index].fPosXmm - LeftXMM) / AfPixelSizeX0um * F_MM2UM);
                    int iShiftY = (int)Math.Ceiling((AFovs[index].fPosYmm - LeftYMM) / AfPixelSizeY0um * F_MM2UM);//Q.F.2018.01.04
                    int iStartX = 0;
                    int iStartY = 0;
                    if (iShiftX < 0)
                    {
                        iStartX = 0 - iShiftX;
                    }
                    if (iShiftY < 0)
                    {
                        iStartY = 0 - iShiftY;
                    }
                    for (i = iStartY; i < AiFOVHeight; ++i)
                    {
                        Array.Copy(_imgByte2DRFov, i * AiFOVWidth + iStartX,
                            _imgByte2DR, (i + iShiftY) * WholeImageWidth + (0 + iShiftX),
                            AiFOVWidth - iStartX);
                        Array.Copy(_imgByte2DGFov, i * AiFOVWidth + iStartX,
                          _imgByte2DG, (i + iShiftY) * WholeImageWidth + (0 + iShiftX),
                          AiFOVWidth - iStartX);
                        Array.Copy(_imgByte2DBFov, i * AiFOVWidth + iStartX,
                          _imgByte2DB, (i + iShiftY) * WholeImageWidth + (0 + iShiftX),
                          AiFOVWidth - iStartX);
                    }

                }
                //});

                Rectangle rect = new Rectangle(0, 0, WholeImageWidth, WholeImageHeight);
                int iWidthNew = 0;
                int iHeightNew = 0;
                bool bAdjustWIdth = false;
                AappSettingData.stDataExpVT.stSavePCBImageParams.byImageAdjustPercent = 30;
                
                if (AappSettingData.stDataExpVT.stSavePCBImageParams.byImageAdjustPercent < 100)
                {
                    bAdjustWIdth = true;
                    iWidthNew = (int)System.Math.Ceiling(WholeImageWidth * AappSettingData.stDataExpVT.stSavePCBImageParams.byImageAdjustPercent * 0.01);
                    iHeightNew = (int)System.Math.Ceiling(WholeImageHeight * AappSettingData.stDataExpVT.stSavePCBImageParams.byImageAdjustPercent * 0.01);
                }
                //else
                //{
                //    bAdjustWIdth = true;
                //    iWidthNew = int.Parse(AstrPerWidthHeight.Split('@')[0]);
                //    iHeightNew = int.Parse(AstrPerWidthHeight.Split('@')[1]);
                //}
                //if (string.IsNullOrEmpty(AstrPerWidthHeight) == false)
                //{
                //    bAdjustWIdth = true;
                //    iWidthNew = int.Parse(AstrPerWidthHeight.Split('@')[0]);
                //    iHeightNew = int.Parse(AstrPerWidthHeight.Split('@')[1]);
                //}
                //Q.F.2018.08.29
                AappSettingData.stDataExpVT.stSavePCBImageParams.bFillBlack = true;
                if (AappSettingData.stDataExpVT.stSavePCBImageParams.bFillBlack == true
                    && _bHasGetBoardBackGroudColor == false)
                {
                    Array.Clear(_fParams, 0, _fParams.Length);
                    _fParams[0] = WholeImageWidth;
                    _fParams[1] = WholeImageHeight;
                    _bHasGetBoardBackGroudColor = true;
                    CvFunction.GetPcbBcgrdColor(_imgByte2DR, _imgByte2DG, _imgByte2DB, _fParams, _arrBoardBackGroudColor);

                }
                int iSaveImageMode = 1;
                //strboardBarcode = Path.GetFileNameWithoutExtension(strJobName) + "_" + strboardBarcode + "_" + strTime;
               // string strFileName = "";//Path.Combine(strPath, strboardBarcode);
                switch ((ImgCSCoreIM.EM_ImageFormat)AappSettingData.stDataExpVT.stSavePCBImageParams.ImageFormat)
                {
                    case EM_ImageFormat.bmp:
                        {
                            strFileName += ".bmp";
                            iSaveImageMode = 2;
                            //resultBitMap.Save(strFileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;
                        }
                    default:
                        {
                            strFileName += ".jpg";
                            if (iSaveImageMode >= 2)
                            {
                                iSaveImageMode = 1;
                            }
                            //resultBitMap.Save(strFileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;
                        }
                }
                Bitmap resultBitMap = null;
                //Stopwatch stopWatch = new Stopwatch();
                //stopWatch.Reset();
                //stopWatch.Start();

                if (bAdjustWIdth == false)
                {

                    if (AappSettingData.stDataExpVT.stSavePCBImageParams.bFillBlack == true)
                    {
                        Array.Clear(_fParams, 0, _fParams.Length);
                        _fParams[0] = WholeImageWidth;
                        _fParams[1] = WholeImageHeight;
                        _fParams[2] = _arrBoardBackGroudColor[0];
                        _fParams[3] = _arrBoardBackGroudColor[1];
                        _fParams[4] = _arrBoardBackGroudColor[2];
                        _fParams[5] = 1;
                        byte[] imgByte2DTmp = new byte[_imgByte2DB.Length];
                        CvFunction.FillPcbBlackHole(_imgByte2DR, _imgByte2DG, _imgByte2DB,
                            _fParams, _imgByte2DR, _imgByte2DG, _imgByte2DB);
                    }

                    switch (iSaveImageMode)
                    {
                        case 0:
                            {
                                unsafe
                                {

                                    resultBitMap = new Bitmap(WholeImageWidth, WholeImageHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                                    System.Drawing.Imaging.BitmapData bmpData = resultBitMap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, resultBitMap.PixelFormat);
                                    IntPtr iPtr = bmpData.Scan0;
                                    int iStride = bmpData.Stride;
                                    int iBytes = iStride * WholeImageHeight;
                                    byte[] PixelValues = new byte[iBytes];
                                    int iPoint = 0;
                                    int offSet = iStride - WholeImageWidth * 3;
                                    byte byG = 0, byB = 0, byR = 0;
                                    //for (i = 0; i < WholeImageHeight; ++i)
                                    //{
                                    //    for (j = 0; j < WholeImageWidth; ++j)
                                    //    {
                                    for (index = 0; index < lLength; ++index)
                                    {
                                        //index = i * WholeImageWidth + j;


                                        byG = _imgByte2DG[index];
                                        byB = _imgByte2DB[index];
                                        byR = _imgByte2DR[index];


                                        PixelValues[iPoint++] = byB;
                                        PixelValues[iPoint++] = byG;
                                        PixelValues[iPoint++] = byR;
                                        // }

                                        // iPoint += 3;               
                                        iPoint += offSet;
                                    }
                                    //
                                    System.Runtime.InteropServices.Marshal.Copy(PixelValues, 0, iPtr, iBytes);
                                    resultBitMap.UnlockBits(bmpData);
                                    resultBitMap.Save(strFileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                                }
                                break;
                            }
                        case 1:
                            {
                                //ImgCSCoreIM.ST_SaveOrLoadImgData stSave = new ImgCSCoreIM.ST_SaveOrLoadImgData();
                                stSave.iImgFormat = 1;
                                stSave.bUseOpcv = false;
                                stSave.byImgDataR = _imgByte2DR;
                                stSave.byImgDataG = _imgByte2DG;
                                stSave.byImgDataB = _imgByte2DB;
                                stSave.iWidth = WholeImageWidth;
                                stSave.iHeight = WholeImageHeight;
                                _baseFuc.SaveImg(strFileName, stSave, 80);

                                stSave.byImgDataR = null;
                                stSave.byImgDataG = null;
                                stSave.byImgDataB = null;
                                break;
                            }
                        case 2:
                            {
                                //ImgCSCoreIM.ST_SaveOrLoadImgData stSave = new ImgCSCoreIM.ST_SaveOrLoadImgData();
                                stSave.iImgFormat = 0;
                                stSave.bUseOpcv = true;
                                stSave.byImgDataR = _imgByte2DR;
                                stSave.byImgDataG = _imgByte2DG;
                                stSave.byImgDataB = _imgByte2DB;
                                stSave.iWidth = WholeImageWidth;
                                stSave.iHeight = WholeImageHeight;
                                _baseFuc.SaveImg(strFileName, stSave, 100);

                                stSave.byImgDataR = null;
                                stSave.byImgDataG = null;
                                stSave.byImgDataB = null;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }






                }
                else
                {
                    unsafe
                    {
                        int lImageLengthNew = iWidthNew * iHeightNew;

                        if (_imgByte2DRReSize == null || _imgByte2DRReSize.Length != lImageLengthNew)
                        {
                            _imgByte2DRReSize = new byte[lImageLengthNew];
                            _imgByte2DGReSize = new byte[lImageLengthNew];
                            _imgByte2DBReSize = new byte[lImageLengthNew];
                        }
                        Array.Clear(_imgByte2DRReSize, 0, lImageLengthNew);
                        Array.Clear(_imgByte2DGReSize, 0, lImageLengthNew);
                        Array.Clear(_imgByte2DBReSize, 0, lImageLengthNew);


                        //_imgBi[0].RGBImageScale(_imgByte2DB,
                        //             _imgByte2DG,
                        //             _imgByte2DB,
                        //             WholeImageWidth, WholeImageHeight,
                        //             r, g, b, iWidthNew, iHeightNew);                     
                        Array.Clear(_fParams, 0, _fParams.Length);
                        _fParams[0] = WholeImageWidth;
                        _fParams[1] = WholeImageHeight;
                        _fParams[2] = iWidthNew;
                        _fParams[3] = iHeightNew;
                        _fParams[4] = 0;
                        _baseFuc.ResizeImgByOpcv(_imgByte2DR,
                            _imgByte2DG,
                            _imgByte2DB,
                            _imgByte2DRReSize, _imgByte2DGReSize, _imgByte2DBReSize, _fParams);

                        AappSettingData.stDataExpVT.stSavePCBImageParams.bFillBlack = true;
                        if (AappSettingData.stDataExpVT.stSavePCBImageParams.bFillBlack == true)
                        {
                            Array.Clear(_fParams, 0, _fParams.Length);
                            _fParams[0] = iWidthNew;
                            _fParams[1] = iHeightNew;
                            _fParams[2] = _arrBoardBackGroudColor[0];
                            _fParams[3] = _arrBoardBackGroudColor[1];
                            _fParams[4] = _arrBoardBackGroudColor[2];
                            _fParams[5] = 1;
                            //byte[] imgByte2DTmp = new byte[r.Length];
                            CvFunction.FillPcbBlackHole(_imgByte2DRReSize, _imgByte2DGReSize, _imgByte2DBReSize,
                                _fParams, _imgByte2DRReSize, _imgByte2DGReSize, _imgByte2DBReSize);
                        }

                        rect = new Rectangle(0, 0, iWidthNew, iHeightNew);


                        switch (iSaveImageMode)
                        {
                            case 0:
                                {
                                    unsafe
                                    {

                                        resultBitMap = new Bitmap(iWidthNew, iHeightNew, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                                        //Rectangle rect = new Rectangle(0, 0, AiFOVWidth, AiFOVHeight);
                                        // Rectangle rect = new Rectangle(0, 0, WholeImageWidth, WholeImageHeight);
                                        System.Drawing.Imaging.BitmapData bmpData = resultBitMap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, resultBitMap.PixelFormat);
                                        IntPtr iPtr = bmpData.Scan0;
                                        int iStride = bmpData.Stride;
                                        int iBytes = iStride * iHeightNew;
                                        byte[] PixelValues = new byte[iBytes];
                                        int iPoint = 0;
                                        int offSet = iStride - iWidthNew * 3;
                                        byte byG = 0, byB = 0, byR = 0;

                                        for (i = 0; i < iHeightNew; ++i)
                                        {
                                            for (j = 0; j < iWidthNew; ++j)
                                            {

                                                index = i * iWidthNew + j;
                                                byG = _imgByte2DGReSize[index];
                                                byB = _imgByte2DBReSize[index];
                                                byR = _imgByte2DRReSize[index];


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
                                        resultBitMap.Save(strFileName, System.Drawing.Imaging.ImageFormat.Jpeg);

                                        PixelValues = null;
                                    }
                                    break;
                                }
                            case 1:
                                {
                                    //ImgCSCoreIM.ST_SaveOrLoadImgData stSave = new ImgCSCoreIM.ST_SaveOrLoadImgData();
                                    stSave.iImgFormat = 1;
                                    stSave.bUseOpcv = false;
                                    stSave.byImgDataR = _imgByte2DRReSize;
                                    stSave.byImgDataG = _imgByte2DGReSize;
                                    stSave.byImgDataB = _imgByte2DBReSize;
                                    stSave.iWidth =  iWidthNew;
                                    stSave.iHeight = iHeightNew;
                                    _baseFuc.SaveImg(strFileName, stSave, 80);

                                    stSave.byImgDataR = null;
                                    stSave.byImgDataG = null;
                                    stSave.byImgDataB = null;
                                    break;
                                }
                            case 2:
                                {
                                    //ImgCSCoreIM.ST_SaveOrLoadImgData stSave = new ImgCSCoreIM.ST_SaveOrLoadImgData();
                                    stSave.iImgFormat = 0;
                                    stSave.bUseOpcv = true;
                                    stSave.byImgDataR = _imgByte2DRReSize;
                                    stSave.byImgDataG = _imgByte2DGReSize;
                                    stSave.byImgDataB = _imgByte2DBReSize;
                                    stSave.iWidth = iWidthNew;
                                    stSave.iHeight = iHeightNew;
                                    _baseFuc.SaveImg(strFileName, stSave, 100);

                                    stSave.byImgDataR = null;
                                    stSave.byImgDataG = null;
                                    stSave.byImgDataB = null;
                                    
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }





                        //strboardBarcode = Path.GetFileNameWithoutExtension(strJobName) + "_" + strboardBarcode + "_" + strTime;
                        //string strFileName = Path.Combine(strPath, strboardBarcode);
                        //switch ((ImgCSCoreIM.EM_ImageFormat)AappSettingData.stDataExpVT.stSavePCBImageParams.ImageFormat)
                        //{
                        //    default:
                        //        {
                        //            strFileName += ".jpg";
                        //            resultBitMap.Save(strFileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        //            break;
                        //        }
                        //}


                    }
                }
                if (resultBitMap != null)
                {
                    resultBitMap.Dispose();
                }
                
                //stopWatch.Stop();
                //MessageBox.Show("iSaveImageMode:" + iSaveImageMode + "\t" + stopWatch.ElapsedMilliseconds);


            }
            catch (System.Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                strMsg += ex.ToString();
                AppLogHelp.WriteError(LogFileFormate.FOVPCB, "SaveWholePCBImagePrivate " + strMsg);
            }
            finally
            {
                //_arrBoardBackGroudColor = null;

                _imgByte2DR = null;
                _imgByte2DG = null;
                _imgByte2DB = null;

                _imgByte2DRFov = null;
                _imgByte2DGFov = null;
                _imgByte2DBFov = null;

                _imgByte2DRReSize = null;
                _imgByte2DGReSize = null;
                _imgByte2DBReSize = null;
                GC.Collect();
            }
            
            return strMsg;
        }


    }
}
