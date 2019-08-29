using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Data;
using System.Management;
using System.Diagnostics;
using System.IO.Compression;
using System.Drawing.Imaging;
//using InspectMainLib;
//using AppLayerLib;
using System.Net.NetworkInformation;
using System.Xml;
//using Microsoft.Office.Interop;
//using Microsoft.Office.Core;
using ImgCSCoreIM;
/// <summary>
/// Create By Xu In 2018_09_29
/// Edit By Xu In  2018_10_23 //Add BitMap.Dispose();
/// Edit By Xu In 2018_10_26 //Add Func To Load And Save FileName.imgdat
/// Edit By Xu In 2018_10_29 //FileName.imgdat  The number of Width and Height occupied In DataHead 2->3 ;LoadImadat( , ref )Add ref.
///                          //strFileName -> strFilePath ,Meaning You Need't Add ".FileFormat" In "strFilePath" But Must Assign ST_SaveOrLoadImgData.iImgFormat To Figure Out File Format;
/// Edit By Xu In 2018.10.31 // The Interface Params of " int []AiParams " Change To" float [] AfParams" In " ResizeImgByOpcv() ";
/// </summary>
namespace AutoAPP
{
    /// <summary>
    /// Create By Xu In 2018_09_29
    /// Edit By Xu In  2018_10_23 //Add BitMap.Dispose();
    /// Edit By Xu In 2018_10_26 //Add Func To Load And Save FileName.imgdat
    /// </summary>
    public partial class Basefunction
    {
        private ocvMngCPPDLL.CvFunctionInterface _cvFunc;//// Add By Q.F In 2018.10.30 ocvMngCPPDLL.CvFunctionInterface
        private byte[] byHeadData = new byte[20];
        public Basefunction()// Add By Q.F In 2018.10.30
        {
            try
            {
                _cvFunc = new ocvMngCPPDLL.CvFunctionInterface();
                _cvFunc.Init();
            }
            catch (System.Exception ex)
            {

            }


        }
        /// <summary>
        /// Create By Xu In 2018.09.29
        /// Edit By Xu In 2018.10.23 //Add BitMap.Dispose();
        /// Edit By Xu In 2018_10_29 //strFileName -> strFilePath ,Meaning You Need't Add ".FileFormat" In "strFilePath" But Must Assign ST_SaveOrLoadImgData.iImgFormat To Figure Out File Format;
        /// Function: Save Img To Native.         
        /// </summary>
        /// <param name="strFileName">FilePath+FileName</param>
        /// <param name="stReadImgData">ST_SaveOrLoadImgData</param>
        /// <param name="iImgQuality">Low 0->100 High</param>
        /// <returns></returns>
        public string SaveImg(string strFilePath, ST_SaveOrLoadImgData stSaveImgData, int iImgQuality)
        {
            string strMsg = string.Empty;
            string strFileName = string.Empty;
            try
            {
                if (strFilePath == null)
                    return "FilePath_Is_Null";

                switch (stSaveImgData.iImgFormat)//Edit By Xu In 2018_10_30
                {
                    case 0:
                        if (
                               (strFilePath[strFilePath.Length - 4] == 46)//"."
                            && (strFilePath[strFilePath.Length - 3] == 98 || strFilePath[strFilePath.Length - 3] == 66)//"bB"
                            && (strFilePath[strFilePath.Length - 2] == 109 || strFilePath[strFilePath.Length - 2] == 77)//"mM"
                            && (strFilePath[strFilePath.Length - 1] == 112 || strFilePath[strFilePath.Length - 1] == 80)//"pP"
                            )
                            strFileName = strFilePath;
                        else
                            strFileName = strFilePath + ".bmp";

                        break;
                    case 1:
                        if (
                               (strFilePath[strFilePath.Length - 4] == 46)
                            && (strFilePath[strFilePath.Length - 3] == 106 || strFilePath[strFilePath.Length - 3] == 74)//"jJ"
                            && (strFilePath[strFilePath.Length - 2] == 112 || strFilePath[strFilePath.Length - 2] == 80)//"pP"
                            && (strFilePath[strFilePath.Length - 1] == 103 || strFilePath[strFilePath.Length - 1] == 71)//"gG"
                            )
                            strFileName = strFilePath;
                        else
                            strFileName = strFilePath + ".jpg";
                        break;
                    case 2:
                        SaveImg(strFilePath, stSaveImgData);//.imgdat
                        return "Format_Is_.imgdat";
                    default:
                        return "File_Format_Params_Err";

                }

                if (stSaveImgData.iImgFormat == 0 && stSaveImgData.bUseOpcv == true)
                {
                    byte[] byFileName = System.Text.Encoding.Default.GetBytes(strFileName);
                    float[] AfarrParams = new float[3];
                    AfarrParams[0] = (float)strFileName.Length;
                    AfarrParams[1] = (float)stSaveImgData.iWidth;
                    AfarrParams[2] = (float)stSaveImgData.iHeight;

                    _cvFunc.SaveImgByOpcv(byFileName, stSaveImgData.byImgDataR, stSaveImgData.byImgDataG, stSaveImgData.byImgDataB, AfarrParams);
                    return strMsg;
                }
                Bitmap bmp = new Bitmap(stSaveImgData.iWidth, stSaveImgData.iHeight, PixelFormat.Format24bppRgb);

                int iWidth  = stSaveImgData.iWidth;
                int iHeight = stSaveImgData.iHeight;
                int iLength = iWidth * iHeight;
                //获取图像的BitmapData对像 
                System.Drawing.Imaging.BitmapData data = bmp.LockBits(new Rectangle(0, 0, iWidth, iHeight), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                //循环处理 
                unsafe
                {
                    int iOffset = data.Stride - data.Width * 3;
                    byte* ptr = (byte*)(data.Scan0);
                    byte* ptr_b;
                    byte* ptr_g;
                    byte* ptr_r;
                    ptr_b = (byte*)System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(stSaveImgData.byImgDataB, 0);
                    ptr_g = (byte*)System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(stSaveImgData.byImgDataG, 0);
                    ptr_r = (byte*)System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(stSaveImgData.byImgDataR, 0);
                    if (iOffset == 0)
                    {
                        for (int i = 0; i < iLength; i++)
                        {
                            *ptr++ = *ptr_b++;
                            *ptr++ = *ptr_g++;
                            *ptr++ = *ptr_r++;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < iHeight; i++)
                        {
                            for (int j = 0; j < iWidth; j++)
                            {
                                *ptr++ = *ptr_b++;
                                *ptr++ = *ptr_g++;
                                *ptr++ = *ptr_r++;
                            }
                            ptr += iOffset;
                        }
                    }
                }

                switch (stSaveImgData.iImgFormat)
                {
                    case 0:
                        bmp.Save(strFileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 1:
                        EncoderParameter p;
                        EncoderParameters ps;

                        ps = new EncoderParameters(1);
                        if (iImgQuality > 100)
                            iImgQuality = 100;
                        else if (iImgQuality < 0)
                            iImgQuality = 80;
                        p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, iImgQuality);
                        ps.Param[0] = p;
                        bmp.Save(strFileName, GetCodecInfo("image/jpeg"), ps);
                        break;

                    default:
                        bmp.Save(strFileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                }
                bmp.Dispose();//Edit By Xu In 2018.10.23
            }

            catch (Exception e)
            {
                strMsg += "SaveImgErr" + e.Message.ToString();
            }
            return strMsg;
        }

        //private  Bitmap _bmp = null;
        public string SaveImg(string strFilePath, ST_SaveOrLoadImgData stSaveImgData, int iImgQuality, bool bIsFovPcb, Bitmap bmp)
        {
            string strMsg = string.Empty;
            string strFileName = string.Empty;
            try
            {
                if (strFilePath == null)
                    return "FilePath_Is_Null";

                switch (stSaveImgData.iImgFormat)//Edit By Xu In 2018_10_30
                {
                    case 0:
                        if (
                               (strFilePath[strFilePath.Length - 4] == 46)//"."
                            && (strFilePath[strFilePath.Length - 3] == 98 || strFilePath[strFilePath.Length - 3] == 66)//"bB"
                            && (strFilePath[strFilePath.Length - 2] == 109 || strFilePath[strFilePath.Length - 2] == 77)//"mM"
                            && (strFilePath[strFilePath.Length - 1] == 112 || strFilePath[strFilePath.Length - 1] == 80)//"pP"
                            )
                            strFileName = strFilePath;
                        else
                            strFileName = strFilePath + ".bmp";

                        break;
                    case 1:
                        if (
                               (strFilePath[strFilePath.Length - 4] == 46)
                            && (strFilePath[strFilePath.Length - 3] == 106 || strFilePath[strFilePath.Length - 3] == 74)//"jJ"
                            && (strFilePath[strFilePath.Length - 2] == 112 || strFilePath[strFilePath.Length - 2] == 80)//"pP"
                            && (strFilePath[strFilePath.Length - 1] == 103 || strFilePath[strFilePath.Length - 1] == 71)//"gG"
                            )
                            strFileName = strFilePath;
                        else
                            strFileName = strFilePath + ".jpg";
                        break;
                    case 2:
                        SaveImg(strFilePath, stSaveImgData);//.imgdat
                        return "Format_Is_.imgdat";
                    default:
                        return "File_Format_Params_Err";

                }

                if (stSaveImgData.iImgFormat == 0 && stSaveImgData.bUseOpcv == true)
                {
                    byte[] byFileName = System.Text.Encoding.Default.GetBytes(strFileName);
                    float[] AfarrParams = new float[3];
                    AfarrParams[0] = (float)strFileName.Length;
                    AfarrParams[1] = (float)stSaveImgData.iWidth;
                    AfarrParams[2] = (float)stSaveImgData.iHeight;

                    _cvFunc.SaveImgByOpcv(byFileName, stSaveImgData.byImgDataR, stSaveImgData.byImgDataG, stSaveImgData.byImgDataB, AfarrParams);
                    return strMsg;
                }
                //Bitmap bmp = new Bitmap(stSaveImgData.iWidth, stSaveImgData.iHeight, PixelFormat.Format24bppRgb);
                
                int iWidth = stSaveImgData.iWidth;
                int iHeight = stSaveImgData.iHeight;
                int iLength = iWidth * iHeight;
                //if (_bmp == null || _bmp.Width * _bmp.Height != iLength)
                //{
                //    if (_bmp != null && _bmp.Width * _bmp.Height != iLength)
                //    {
                //        _bmp.Dispose();
                //    }
                //_bmp = new Bitmap(stSaveImgData.iWidth, stSaveImgData.iHeight, PixelFormat.Format24bppRgb);
                //}
                //获取图像的BitmapData对像 
                //BitmapRegionDecoder bt = BitmapRegionDecoder.
                System.Drawing.Imaging.BitmapData data = bmp.LockBits(new Rectangle(0, 0, iWidth, iHeight), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                ////循环处理 
                unsafe
                {
                    int iOffset = data.Stride - data.Width * 3;
                    byte* ptr = (byte*)(data.Scan0);
                    byte* ptr_b;
                    byte* ptr_g;
                    byte* ptr_r;
                    ptr_b = (byte*)System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(stSaveImgData.byImgDataB, 0);
                    ptr_g = (byte*)System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(stSaveImgData.byImgDataG, 0);
                    ptr_r = (byte*)System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(stSaveImgData.byImgDataR, 0);
                    if (iOffset == 0)
                    {
                        for (int i = 0; i < iLength; i++)
                        {
                            *ptr++ = *ptr_b++;
                            *ptr++ = *ptr_g++;
                            *ptr++ = *ptr_r++;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < iHeight; i++)
                        {
                            for (int j = 0; j < iWidth; j++)
                            {
                                *ptr++ = *ptr_b++;
                                *ptr++ = *ptr_g++;
                                *ptr++ = *ptr_r++;
                            }
                            ptr += iOffset;
                        }
                    }
                }

                switch (stSaveImgData.iImgFormat)
                {
                    case 0:
                        bmp.Save(strFileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 1:
                        EncoderParameter p;
                        EncoderParameters ps;

                        ps = new EncoderParameters(1);
                        if (iImgQuality > 100)
                            iImgQuality = 100;
                        else if (iImgQuality < 0)
                            iImgQuality = 80;
                        p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, iImgQuality);
                        ps.Param[0] = p;
                        bmp.Save(strFileName, GetCodecInfo("image/jpeg"), ps);
                        break;

                    default:
                        bmp.Save(strFileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                }
                bmp.UnlockBits(data);
                //bmp.Dispose();//Edit By Xu In 2018.10.23
            }

            catch (Exception e)
            {
                strMsg += "SaveImgErr" + e.Message.ToString();
            }
            return strMsg;
        }
        

        /// <summary>
        /// Create By Xu In 2018_10_26
        /// Save Image Data To  ..FileName.imgdat
        /// Edit By Xu In 2018_10_29 长宽所占的位数由2->3
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="stSaveImgData"></param>
        /// <returns></returns>
        public string SaveImg(string strFilePath, ST_SaveOrLoadImgData stSaveImgData)
        {
            string strMsg = string.Empty;
            Stream stream = null;
            if (stSaveImgData.iImgFormat != 2)
                return strMsg + "SaveImgParamsErr";
            string strFileName = string.Empty;
            if (
                  (strFilePath[strFilePath.Length - 7] == 46)//"."
               && (strFilePath[strFilePath.Length - 6] == 105 || strFilePath[strFilePath.Length - 5] == 109)//"im"
               && (strFilePath[strFilePath.Length - 4] == 103 || strFilePath[strFilePath.Length - 3] == 100)//"gd"
               && (strFilePath[strFilePath.Length - 2] == 97  || strFilePath[strFilePath.Length - 1] == 116)//"at"
              )
                strFileName = strFilePath;
            else
                strFileName = strFilePath + ".imgdat";
            try
            {
                int iLength = stSaveImgData.iHeight * stSaveImgData.iWidth;
                stream = new MemoryStream(stSaveImgData.byImgDataB);
                byte[] byDataHead = new byte[20];
                byDataHead[0] = 20;//HeadLength
                byDataHead[1] = 0;//是否加密
                byDataHead[2] = 3;//Width Height 所占的byte位数  Edit By Xu In 2018_10_29 : 位数：2->3

                byDataHead[3] = (byte)(stSaveImgData.iWidth / 65536);//高位
                byDataHead[4] = (byte)(stSaveImgData.iWidth % 65536 / 256);
                byDataHead[5] = (byte)(stSaveImgData.iWidth % 256);//低位

                byDataHead[6] = (byte)(stSaveImgData.iHeight / 65536);//高位
                byDataHead[7] = (byte)(stSaveImgData.iHeight % 65536 / 256);
                byDataHead[8] = (byte)(stSaveImgData.iHeight % 256);//低位

                //byDataHead[5] = (byte)(stSaveImgData.iHeight / 256);//高位
                // byDataHead[6] = (byte)(stSaveImgData.iHeight % 256);//低位

                FileStream fs = new FileStream(strFileName, FileMode.OpenOrCreate);
                fs.Write(byDataHead, 0, 20);
                fs.Write(stSaveImgData.byImgDataR, 0, iLength);
                fs.Write(stSaveImgData.byImgDataG, 0, iLength);
                fs.Write(stSaveImgData.byImgDataB, 0, iLength);

                fs.Close();
            }
            catch (Exception e)
            {
                strMsg += "SaveImgErr" + e.Message.ToString();
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }
            return strMsg;
        }

        /// <summary>
        /// Create By Xu In 2018_10_26
        /// Load Data From ..FileName.imgdat
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="stSaveImgData"></param>
        /// <returns></returns>
        public string LoadImadat(string strFilePath, ref ST_SaveOrLoadImgData stSaveImgData)//Edit By Xu In 2018_10_29 Add ref 
        {
            string strMsg = string.Empty;
            Stream stream = null;
            FileStream fs = null;
            if (stSaveImgData.iImgFormat != 2)
                return strMsg + "LoadImgParamsErr";

            string strFileName = string.Empty;
            if (
                  (strFilePath[strFilePath.Length - 7] == 46)//"."
               && (strFilePath[strFilePath.Length - 6] == 105 || strFilePath[strFilePath.Length - 5] == 109)//"im"
               && (strFilePath[strFilePath.Length - 4] == 103 || strFilePath[strFilePath.Length - 3] == 100)//"gd"
               && (strFilePath[strFilePath.Length - 2] == 97 || strFilePath[strFilePath.Length - 1] == 116)//"at"
              )
                strFileName = strFilePath;
            else
                strFileName = strFilePath + ".imgdat";

            try
            {
                fs = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.Read);
                byHeadData.Initialize();
                fs.Read(byHeadData, 0, 20);
                if (byHeadData[0] != 20 || (byHeadData[2] != 3 && byHeadData[2] != 2))//Edit By Xu In 2018_10_29 : 位数：2->3
                    return "Not_Surpported_Head_Data_Format.imgdat";
                if (3 == byHeadData[2])
                {
                    stSaveImgData.iWidth = byHeadData[3] * 65536 + byHeadData[4] * 256 + byHeadData[5];
                    stSaveImgData.iHeight = byHeadData[6] * 65536 + byHeadData[7] * 256 + byHeadData[8];
                }
                else
                    if (2 == byHeadData[2])
                    {
                        stSaveImgData.iWidth = byHeadData[3] * 256 + byHeadData[4];
                        stSaveImgData.iHeight = byHeadData[5] * 256 + byHeadData[6];
                    }
                    else
                        return "Not_Surpported_Head_Data_Format.imgdat";
                int iLength = stSaveImgData.iHeight * stSaveImgData.iWidth;
                if (stSaveImgData.byImgDataR == null || stSaveImgData.byImgDataR.Length != iLength)
                {
                    stSaveImgData.byImgDataR = new byte[iLength];
                    stSaveImgData.byImgDataG = new byte[iLength];
                    stSaveImgData.byImgDataB = new byte[iLength];
                }

                fs.Position = 20;
                fs.Read(stSaveImgData.byImgDataR, 0, iLength);
                fs.Position = 20 + iLength;
                fs.Read(stSaveImgData.byImgDataG, 0, iLength);
                fs.Position = 20 + iLength * 2;
                fs.Read(stSaveImgData.byImgDataB, 0, iLength);

            }
            catch (Exception e)
            {
                strMsg += "LoadImgErr" + e.Message.ToString();
            }
            finally
            {
                if (fs != null)
                    fs.Close();

                if (stream != null)
                    stream.Dispose();
            }
            return strMsg;
        }

        /// <summary>
        /// Create By Xu In 2018.09.21
        /// Edit By Xu In 2018_10_29 :strFileName -> strFilePath ,Meaning You Need't Add ".FileFormat" In "strFilePath" But Must Assign ST_SaveOrLoadImgData.iImgFormat To Figure Out File Format;
        /// Function: Load And Split Img To 3 Channels.
        /// Warming : 1.The Picture Bit Depth Must Be 24(Format24bppRgb) Now.
        ///           2.Only Support Bmp And Jpg/Jpeg;
        /// </summary>
        /// <param name="strFileName">FilePath+FileName</param>
        /// <param name="stReadImgData">ST_SaveOrLoadImgData</param>
        /// <returns></returns>
        public string LoadImg(string strFilePath, ref ST_SaveOrLoadImgData stReadImgData)
        {
            string strMsg = string.Empty;
            string strFileName = string.Empty;
            try
            {
                if (strFileName == null)
                    return "FilePath_Is_Null";

                switch (stReadImgData.iImgFormat)//Edit By Xu In 2018_10_30
                {
                    case 0:
                        if (
                                 (strFilePath[strFilePath.Length - 4] == 46)//"."
                              && (strFilePath[strFilePath.Length - 3] == 98  || strFilePath[strFilePath.Length - 3] == 66)//"bB"
                              && (strFilePath[strFilePath.Length - 2] == 109 || strFilePath[strFilePath.Length - 2] == 77)//"mM"
                              && (strFilePath[strFilePath.Length - 1] == 112 || strFilePath[strFilePath.Length - 1] == 80)//"pP"
                           )
                            strFileName = strFilePath;
                        else
                            strFileName = strFilePath + ".bmp";

                        break;
                    case 1:
                        if (
                               (strFilePath[strFilePath.Length - 4] == 46)
                            && (strFilePath[strFilePath.Length - 3] == 106 || strFilePath[strFilePath.Length - 3] == 74)//"jJ"
                            && (strFilePath[strFilePath.Length - 2] == 112 || strFilePath[strFilePath.Length - 2] == 80)//"pP"
                            && (strFilePath[strFilePath.Length - 1] == 103 || strFilePath[strFilePath.Length - 1] == 71)//"gG"
                           )
                            strFileName = strFilePath;
                        else
                            strFileName = strFilePath + ".jpg";
                        break;
                    case 2:
                        LoadImadat(strFilePath, ref stReadImgData);//.imgdat
                        return "Format_Is_.imgdat";
                    default:
                        return "File_Format_Params_Err";

                }

                if (stReadImgData.iImgFormat == 0 && stReadImgData.bUseOpcv == true)
                {
                    byte[] FilePath = System.Text.Encoding.Default.GetBytes(strFileName);
                    float[] AfarrParams = new float[3];//Edit By Xu In 2018_10_31 int->float
                    AfarrParams[0] = (float)FilePath.Length;

                   // ST_SaveOrLoadImgData stReadImgdata = new ST_SaveOrLoadImgData();

                    _cvFunc.LoadImgByOpcv(FilePath, ref stReadImgData.byImgDataR, ref stReadImgData.byImgDataG, ref stReadImgData.byImgDataB, AfarrParams);
                    stReadImgData.iWidth = (int)AfarrParams[1];
                    stReadImgData.iHeight = (int)AfarrParams[2];
                }
                else if (stReadImgData.iImgFormat == 0 && stReadImgData.bUseFastLoadBmp == true)
                    FastLoadSplitBmp(strFileName, ref stReadImgData);

                else
                    LoadSplitImg(strFileName, ref stReadImgData);
            }
            catch (Exception e)
            {
                strMsg += "LoadImgErr" + e.Message.ToString();
            }
            return strMsg;
        }

        public Bitmap ImageResize(ref Bitmap ASrcBitmap, int AiWidthNew, int AiHeightNew)
        {
            try
            {
                Bitmap resultBitMap = new Bitmap(AiWidthNew, AiHeightNew, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(resultBitMap);
                g.Clear(Color.White);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(ASrcBitmap, new Rectangle(0, 0, AiWidthNew, AiHeightNew), new Rectangle(0, 0, ASrcBitmap.Width, ASrcBitmap.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return resultBitMap;
            }
            catch (System.Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// Create By Xu In 2018.09.21
        /// Edit By Xu In 2018.10.31 // The Interface Params of " int []AiParams " Change To" float [] AfParams" 
        /// Function : Resize Img By Opencv
        /// Warming  ：__AiParams__ Has 5 input parameters: src_Width, src_Height, new_Width, new_Height, Method 
        ///            Now Provide 2 Methods: 
        ///                                  0  : INTER_LINEAR (Less Time ,Less Accuracy)
        ///                                  1  : INTER_CUBIC  (More Time ,More Accuracy)
        /// </summary>
        /// <param name="AbySrcData_R"></param>
        /// <param name="AbySrcData_G"></param>
        /// <param name="AbySrcData_B"></param>
        /// <param name="AbyDstData_R"></param>
        /// <param name="AbyDstData_G"></param>
        /// <param name="AbyDstData_B"></param>
        /// <param name="AiParams">5 input parameters: src_Width, src_Height, new_Width, new_Height, Method </param>
        /// <returns></returns>
        public string ResizeImgByOpcv(byte[] AbySrcData_R, byte[] AbySrcData_G, byte[] AbySrcData_B,
                                      byte[] AbyDstData_R, byte[] AbyDstData_G, byte[] AbyDstData_B, float[] AfParams)//Edit By Xu In 2018_10_31 int->float;
        {
            string strMsg = string.Empty;
            try
            {
                if (AbySrcData_R == null || AbySrcData_G == null || AbySrcData_B == null || AfParams == null)
                {
                    AbyDstData_R = null;
                    AbyDstData_G = null;
                    AbyDstData_B = null;
                    return "SrcData_Is_Null";
                }

                _cvFunc.ResizeImgByOpcv(AbySrcData_R, AbySrcData_G, AbySrcData_B,
                                        AbyDstData_R, AbyDstData_G, AbyDstData_B, AfParams);
            }
            catch (Exception e)
            {
                strMsg += "ResizeImgErr" + e.Message.ToString();
            }
            return strMsg;
        }


        /// <summary>
        /// Create By Xu In 2018.09.21
        /// Function : Split Bmp File To Three Byte[] Of RGB,Fastest;
        /// Warming  ：1.May Exit Bug,If Err,Use Normal Method.
        ///            2.The Picture' Width And Height Must Less Than 256* 256 * 256.If Need Longer Length,You Can Change stReadImgData.iWidth(iHeight) Value;
        ///            3.The Picture Bit Depth Must Be 24(Format24bppRgb) Now.
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="stReadImgData"></param>
        private void FastLoadSplitBmp(string strFileName, ref ST_SaveOrLoadImgData stReadImgData)
        {
            try
            {
                if (strFileName == null || stReadImgData.iImgFormat != 0)
                    return;

                Stream stream = File.OpenRead(strFileName);
                byte[] buffer = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(buffer, 0, buffer.Length);
                stReadImgData.iWidth = buffer[20] * 256 * 256 + buffer[19] * 256 + buffer[18];
                stReadImgData.iHeight = buffer[24] * 256 * 256 + buffer[23] * 256 + buffer[22];
                int iLength = stReadImgData.iWidth * stReadImgData.iHeight;
                stReadImgData.byImgDataR = new byte[iLength];
                stReadImgData.byImgDataG = new byte[iLength];
                stReadImgData.byImgDataB = new byte[iLength];
                int index2 = 0;
                int j = 0;
                int i = 0, ii = stReadImgData.iHeight - 1;
                int iSingleLength = stReadImgData.iWidth * 3;
                int iOffset = (iSingleLength / 4 + 1) * 4 - iSingleLength;
                if (iOffset % 4 == 0)
                    iOffset = 0;
                unsafe
                {
                    byte* ptr_b;
                    byte* ptr_g;
                    byte* ptr_r;
                    ptr_b = (byte*)System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(stReadImgData.byImgDataB, 0);
                    ptr_g = (byte*)System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(stReadImgData.byImgDataG, 0);
                    ptr_r = (byte*)System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(stReadImgData.byImgDataR, 0);
                    for (; i < stReadImgData.iHeight; i++, ii--)
                    {
                        index2 = ii * (iSingleLength + iOffset) + 54;
                        for (j = 0; j < stReadImgData.iWidth; j++)
                        {
                            *ptr_b++ = buffer[index2++];
                            *ptr_g++ = buffer[index2++];
                            *ptr_r++ = buffer[index2++];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        /// <summary>
        /// Create By Xu In 2018.09.21
        /// Edit By Xu In 2018.10.23 // Add BitMap.Dispose();
        /// Function : Split Img To 3 Channals 
        /// Warming  ：The Picture Bit Depth Must Be 24(Format24bppRgb) Now.
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="stReadImgData"></param>
        private void LoadSplitImg(string strFileName, ref ST_SaveOrLoadImgData stReadImgData)
        {
            if (strFileName == null && stReadImgData.bUseOpcv == true)
                return;
            Bitmap image = new Bitmap(strFileName);
            stReadImgData.iWidth = image.Width;
            stReadImgData.iHeight = image.Height;
            int iLength = stReadImgData.iWidth * stReadImgData.iHeight;
            System.Drawing.Imaging.BitmapData data = image.LockBits(new Rectangle(0, 0, stReadImgData.iWidth, stReadImgData.iHeight), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* ptr = (byte*)(data.Scan0);
                stReadImgData.byImgDataR = new byte[iLength];
                stReadImgData.byImgDataG = new byte[iLength];
                stReadImgData.byImgDataB = new byte[iLength];
                byte* ptr_b;
                byte* ptr_g;
                byte* ptr_r;
                ptr_b = (byte*)System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(stReadImgData.byImgDataB, 0);
                ptr_g = (byte*)System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(stReadImgData.byImgDataG, 0);
                ptr_r = (byte*)System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(stReadImgData.byImgDataR, 0);
                int index = 0;
                int iOffset = data.Stride - data.Width * 3;
                if (iOffset == 0)
                {
                    for (int i = 0; i < stReadImgData.iHeight; i++)
                    {
                        for (int j = 0; j < stReadImgData.iWidth; j++)
                        {
                            index = i * stReadImgData.iWidth + j;
                            *ptr_b++ = *ptr++;
                            *ptr_g++ = *ptr++;
                            *ptr_r++ = *ptr++;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < stReadImgData.iHeight; i++)
                    {
                        for (int j = 0; j < stReadImgData.iWidth; j++)
                        {
                            index = i * stReadImgData.iWidth + j;
                            *ptr_b++ = *ptr++;
                            *ptr_g++ = *ptr++;
                            *ptr_r++ = *ptr++;
                        }
                        ptr += iOffset;
                    }
                }
            }
            image.Dispose();//Edit By Xu In 2018.10.23
        }

        private static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType)
                    return ici;
            }
            return null;
        }
        public String[] GetFilesXu(String AstrDir, String AstrExt)// Add By Q.F In 2018.10.30
        {
            return Directory.GetFiles(AstrDir, "*" + AstrExt).Where(t => t.ToLower().EndsWith(AstrExt.ToLower())).ToArray();

        }
    }
}

