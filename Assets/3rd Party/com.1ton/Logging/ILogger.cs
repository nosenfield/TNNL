using System;
using System.Diagnostics;
using System.Reflection;
using Debug = UnityEngine.Debug;

namespace nosenfield.Logging
{
    public enum LogLevel : int
    {
        SILENT,
        ERROR,
        WARN,
        DEBUG,
        INFO
    }

    public interface ILogger
    {
        LogLevel sensitivity
        {
            get;
            set;
        }
        void Log(LogLevel logLevel, string message);
    }

    public sealed class Logger : ILogger
    {
        private static Logger instance;
        public static Logger Instance
        {
            get
            {
                if (Logger.instance == null)
                {
                    Logger.instance = new Logger();
                }

                return Logger.instance;
            }
        }
        public LogLevel _sensitivity = LogLevel.INFO;
        public LogLevel sensitivity
        {
            get
            {
                return _sensitivity;
            }
            set
            {
                _sensitivity = value;
            }
        }

        private Logger() { }

        public void Log(LogLevel logLevel, string message)
        {
            if (this.sensitivity < logLevel) return;

            // Resolve caller class and method via reflection
            StackTrace stackTrace = new System.Diagnostics.StackTrace();
            StackFrame frame = stackTrace.GetFrames()[1];
            MethodInfo methodBase = (System.Reflection.MethodInfo)frame.GetMethod();

            string timestamp = DateTime.UtcNow.ToString().Replace("/", "-");
            string classname = methodBase.DeclaringType.Name;
            string methodname = methodBase.Name;
            string logmessage = String.Concat(timestamp, " ", classname, ".", methodname, " – ", logLevel, " : ", message);

            switch (logLevel)
            {
                case LogLevel.INFO:
                    Debug.Log(logmessage);
                    break;
                case LogLevel.DEBUG:
                    Debug.Log(logmessage);
                    break;
                case LogLevel.WARN:
                    Debug.LogWarning(logmessage);
                    break;
                case LogLevel.ERROR:
                    Debug.LogError(logmessage);
                    break;
                default:
                    throw new Exception("log level not defined");
            }

#if UNITY_EDITOR
            string date = timestamp.Substring(0, timestamp.IndexOf(" "));
            System.IO.Directory.CreateDirectory(@"logs/" + date);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"logs/" + date + "/log.log", true))
            {
                file.WriteLine(logmessage);
            }
#endif

        }
    }
}