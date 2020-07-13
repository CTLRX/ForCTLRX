using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omipay.Core
{
    public class FileLog
    {
        /// <summary>
        /// 多线程 lock对象
        /// </summary>
        private static object lockobj = new object();

        public static string CurrentApplicationPath
        {
            get
            {
                string appPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                return FormatFilePath(appPath);
            }
        }

        public static string FormatFilePath(string filePath)
        {
            if (!filePath.EndsWith("\\"))
            {
                filePath += "\\";
            }
            return filePath;
        }

        #region 日志文件夹路径
        /// <summary>
        /// 日志文件夹路径
        /// </summary>
        private static string LogPath
        {
            get
            {
                return Path.Combine(CurrentApplicationPath, "Log");
            }
        }
        #endregion

        #region 错误日志的保存路径
        /// <summary>
        /// 错误日志的保存路径
        /// </summary>
        private static string ErrorLogPath
        {
            get
            {
                return Path.Combine(LogPath, "ErrorLog");
            }
        }
        #endregion

        #region 调试日志保存路径
        /// <summary>
        /// 调试日志保存路径
        /// </summary>
        private static string DebugLogPath
        {
            get
            {
                return Path.Combine(LogPath, "InfoLog");
            }
        }
        #endregion

        #region 记录日志信息
        /// <summary>
        /// 记录日志信息
        /// </summary>
        /// <param name="logPath">日志路径</param>
        /// <param name="message">日志信息</param>
        private static void LogInfo(string logPath, string message)
        {
            try
            {
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }

                string logFileName = Path.Combine(logPath, DateTime.Now.ToString("yyyy-MM-dd") + ".log");

                string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

                //关键点：防止多线程同时访问文件
                lock (lockobj)
                {
                    if (File.Exists(logFileName))
                    {
                        StreamWriter logWriter = File.AppendText(logFileName);
                        try
                        {
                            logWriter.WriteLine(nowTime + "   " + message);
                            logWriter.WriteLine(System.Environment.NewLine);
                        }
                        finally
                        {
                            logWriter.Close();
                        }
                    }
                    else
                    {
                        StreamWriter logWriter = File.CreateText(logFileName);
                        try
                        {
                            logWriter.WriteLine(nowTime + "   " + message);
                            logWriter.WriteLine(System.Environment.NewLine);
                        }
                        finally
                        {
                            logWriter.Close();
                        }
                    }
                }

            }
            catch
            {
            }
        }
        #endregion

        #region 记录错误日志信息
        /// <summary>
        /// 记录错误日志信息
        /// </summary>
        /// <param name="logPath">记录错误日志的文件夹</param>
        /// <param name="message">错误日志的信息</param>
        public static void Error(string logPath, string message)
        {
            LogInfo(logPath, message);
        }

        /// <summary>
        /// 记录错误日志信息
        /// </summary>
        /// <param name="logPath">记录错误日志的文件夹</param>
        /// <param name="ex">异常信息</param>
        public static void Error(string logPath, Exception ex)
        {
            string message = "";

            if (ex == null)
            {
                message = "Exception is null";
            }
            else
            {
                if (ex.Source != null)
                {
                    message += ex.Source + "  ";
                }
                if (ex.StackTrace != null)
                {
                    message += ex.StackTrace + " : ";
                }
                message += ex.Message;
            }

            Error(logPath, message);
        }

        /// <summary>
        /// 记录错误日志信息
        /// </summary>
        /// <param name="message">错误日志信息</param>
        public static void Error(string message)
        {
            Error(ErrorLogPath, message);
        }

        /// <summary>
        /// 记录错误日志信息
        /// </summary>
        /// <param name="ex">异常信息</param>
        public static void Error(Exception ex)
        {
            Error(ErrorLogPath, ex);
        }
        #endregion

        #region 记录信息
        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="logPath">日志路径</param>
        /// <param name="message">调试信息</param>
        public static void Info(string logPath, string message)
        {
            LogInfo(logPath, message);
        }

        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="message">调试信息</param>
        public static void Info(string message)
        {
            LogInfo(DebugLogPath, message);
        }
        #endregion
    }
}
