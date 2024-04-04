using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace nosenfield.Logging
{
    public class DefaultLogger : ILogger
    {
        private static DefaultLogger instance;
        public static DefaultLogger Instance
        {
            get
            {
                if (DefaultLogger.instance == null)
                {
                    DefaultLogger.instance = new DefaultLogger();
                }

                return DefaultLogger.instance;
            }
        }
        public LogLevel _sensitivity = LogLevel.DEBUG;
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

        private DefaultLogger()
        {
#if !UNITY_EDITOR
            _sensitivity = LogLevel.SILENT;
#endif
        }
        private static string GetTrace(StackTrace stackTrace)
        {
            // Resolve caller class and method via reflection
            StackFrame frame = stackTrace.GetFrames()[1];
            MethodInfo methodBase = (System.Reflection.MethodInfo)frame.GetMethod();

            string timestamp = DateTime.UtcNow.ToString().Replace("/", "-");
            string classname = methodBase.DeclaringType.Name;
            string methodname = methodBase.Name;
            return String.Concat(timestamp, " ", classname, ".", methodname);
        }

        public void LogTrace()
        {
            if (this.sensitivity < LogLevel.DEBUG) return;

            StackTrace stackTrace = new System.Diagnostics.StackTrace();
            string trace = DefaultLogger.GetTrace(stackTrace);
            string logMessage = String.Concat(trace, " – ", LogLevel.DEBUG, " : ", "Trace");
            UnityEngine.Debug.Log(logMessage);
        }

        public void Log(LogLevel logLevel, string message)
        {
            if (this.sensitivity < logLevel) return;

            StackTrace stackTrace = new System.Diagnostics.StackTrace();
            string trace = DefaultLogger.GetTrace(stackTrace);
            string logMessage = String.Concat(trace, " – ", logLevel, " : ", message);

            switch (logLevel)
            {
                case LogLevel.INFO:
                    UnityEngine.Debug.Log(logMessage);
                    break;
                case LogLevel.DEBUG:
                    UnityEngine.Debug.Log(logMessage);
                    break;
                case LogLevel.WARN:
                    UnityEngine.Debug.LogWarning(logMessage);
                    break;
                case LogLevel.ERROR:
                    UnityEngine.Debug.LogError(logMessage);
                    break;
            }
        }
    }
}