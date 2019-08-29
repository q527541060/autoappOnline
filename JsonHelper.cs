using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoAPP
{
    public static class JsonHelper
    {
        ///<summary>
        /// 解析Object到json字符
        ///</summary>
        ///<param name="obj"></param>
        ///<returns></returns>
        public static string SerializeObject2String(Object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }


        }
        ///<summary>
        /// 解析Json格式文本内容到Ojbect
        ///</summary>
        ///<param name="strContent"></param>
        ///<returns></returns>
        public static object DeserializeString2Object(string strContent)
        {
            try
            {
                return JsonConvert.DeserializeObject(strContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }


        }
        /// <summary>
        /// 解析Json格式文本内容到T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strContent"></param>
        /// <returns></returns>
        public static T DeserializeString2Object<T>(string strContent)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(strContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }


        }

        ///<summary>
        /// 保存Object到Json文本文件
        ///</summary>
        ///<param name="obj"></param>
        ///<param name="strFullPath"></param>
        ///<returns></returns>
        public static bool SaveObject2JsonFile(object obj, string strJsonFullPath)
        {
            bool blnReturn = false;
            try
            {
                string strContent = SerializeObject2String(obj);
                File.WriteAllText(strJsonFullPath, strContent);
                blnReturn = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return blnReturn;
        }

        ///<summary>
        /// 从文件中读取文本解析成object  json格式
        ///</summary>
        ///<param name="strFullPath"></param>
        ///<returns></returns>
        public static object RealJsonObject2File(string strFullPath)
        {
            try
            {
                string strContent = File.ReadAllText(strFullPath);
                return DeserializeString2Object(strContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }


        }


    }
}
