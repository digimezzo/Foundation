using Digimezzo.Foundation.Core.IO;
using Digimezzo.Foundation.Core.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;

namespace Digimezzo.Foundation.Core.Logging
{
    public class LogClient
    {
        private string logfile;
        private string logfolder;
        private static LogClient instance;
        private Queue<LogEntry> logEntries;
        private Object logEntriesLock = new Object();
        private Timer logTimer = new Timer();
        private bool isInitialized;
        private int archiveAboveSize = 5242880; // 5 MB
        private int maxArchiveFiles = 3;

        private LogClient()
        {
            this.logfolder = Path.Combine(SettingsClient.ApplicationFolder(), "Log");
            this.logfile = System.IO.Path.Combine(this.logfolder, ProcessExecutable.Name() + ".log");
            this.logEntries = new Queue<LogEntry>();
            this.logTimer.Interval = 25;
            this.logTimer.Elapsed += LogTimer_Elapsed;
        }

        private void LogTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.logTimer.Stop();

            lock (this.logEntriesLock)
            {
                if (this.logEntries.Count > 0)
                {
                    this.TryWrite(logEntries.Dequeue());
                    this.logTimer.Start();
                }
            }
        }

        public static LogClient Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LogClient();
                }
                return instance;
            }
        }

        private void AddLogEntry(LogLevel level, string message, object[] args, string callerFilePath, string callerMemberName, int lineNumber)
        {
            this.logTimer.Stop();

            try
            {
                if (args != null) message = string.Format(message, args);

                string levelDescription = string.Empty;

                switch (level)
                {
                    case LogLevel.Info:
                        levelDescription = "Info";
                        break;
                    case LogLevel.Warning:
                        levelDescription = "Warning";
                        break;
                    case LogLevel.Error:
                        levelDescription = "Error";
                        break;
                    default:
                        levelDescription = "Error";
                        break;
                }

                lock (logEntriesLock)
                {
                    this.logEntries.Enqueue(new LogEntry
                    {
                        TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        Level = levelDescription,
                        CallerFilePath = callerFilePath,
                        CallerMemberName = callerMemberName,
                        Message = message,
                        CallerLineNumber = lineNumber
                    });
                }
            }
            catch (Exception)
            {
                // A failure to should never crash the application
            }

            this.logTimer.Start();
        }

        private void TryWrite(LogEntry entry)
        {
            try
            {
                // If the log directory doesn't exist, create it.
                if (!Directory.Exists(this.logfolder)) Directory.CreateDirectory(this.logfolder);

                // If the log file doesn't exist, this also creates it.
                bool isWriteSuccess = false;

                while (!isWriteSuccess)
                {
                    try
                    {
                        using (StreamWriter sw = File.AppendText(this.logfile))
                        {
                            sw.WriteLine($"{entry.TimeStamp}|{entry.Level}|{Path.GetFileNameWithoutExtension(entry.CallerFilePath)}.{entry.CallerMemberName}|{entry.CallerLineNumber}|{entry.Message}");
                        }
                        isWriteSuccess = true;
                    }
                    catch (Exception)
                    {
                    }
                }

                // Rotate the log file
                try
                {
                    this.RotateLogfile();
                }
                catch (Exception)
                {
                }

                // Delete archived log files
                try
                {
                    this.DeleteArchives();
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }
        }

        private void RotateLogfile()
        {
            var fi = new FileInfo(this.logfile);

            if (fi.Length >= this.archiveAboveSize)
            {
                string archivedLogfile = Path.Combine(Path.GetDirectoryName(this.logfile), Path.GetFileNameWithoutExtension(this.logfile) + DateTime.Now.ToString(" (yyyy-MM-dd HH.mm.ss.fff)") + ".log");
                File.Move(this.logfile, archivedLogfile);
            }
        }

        private void DeleteArchives()
        {
            var di = new DirectoryInfo(this.logfolder);

            FileInfo[] files = di.GetFiles().OrderBy(p => p.CreationTime).ToArray();

            while (files.Length > this.maxArchiveFiles + 1)
            {
                FileInfo fi = files.FirstOrDefault();
                if (fi != null) File.Delete(fi.FullName);
                files = di.GetFiles().OrderBy(p => p.CreationTime).ToArray();
            }
        }

        public static void Initialize(int archiveAboveSize, int maxArchiveFiles)
        {
            if (LogClient.Instance.isInitialized) return;
            LogClient.Instance.isInitialized = true;
            LogClient.Instance.archiveAboveSize = archiveAboveSize;
            LogClient.Instance.maxArchiveFiles = maxArchiveFiles;
        }

        public static string Logfile()
        {
            return LogClient.Instance.logfile;
        }

        public static string GetAllExceptions(Exception ex)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Exception:");
            sb.AppendLine(ex.ToString());
            sb.AppendLine("");
            sb.AppendLine("Stack trace:");
            sb.AppendLine(ex.StackTrace);

            int innerExceptionCounter = 0;

            while (ex.InnerException != null)
            {
                innerExceptionCounter += 1;
                sb.AppendLine("Inner Exception " + innerExceptionCounter + ":");
                sb.AppendLine(ex.InnerException.ToString());
                ex = ex.InnerException;
            }

            return sb.ToString();
        }

        public static void Info(string message, object arg1 = null, object arg2 = null, object arg3 = null, object arg4 = null, object arg5 = null, object arg6 = null, object arg7 = null, object arg8 = null, [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
        {
            LogClient.Instance.AddLogEntry(LogLevel.Info, message, new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 }, sourceFilePath, memberName, lineNumber);
        }

        public static void Warning(string message, object arg1 = null, object arg2 = null, object arg3 = null, object arg4 = null, object arg5 = null, object arg6 = null, object arg7 = null, object arg8 = null, [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
        {
            LogClient.Instance.AddLogEntry(LogLevel.Warning, message, new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 }, sourceFilePath, memberName, lineNumber);
        }

        public static void Error(string message, object arg1 = null, object arg2 = null, object arg3 = null, object arg4 = null, object arg5 = null, object arg6 = null, object arg7 = null, object arg8 = null, [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
        {
            LogClient.Instance.AddLogEntry(LogLevel.Error, message, new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 }, sourceFilePath, memberName, lineNumber);
        }
    }
}
