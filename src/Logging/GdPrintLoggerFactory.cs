using System;
using Godot;

namespace Witchpixels.Tanks.Logging;

public class GdPrintLoggerFactory : ILoggerFactory
{
    private readonly ErrorSeverity _minimumSeverity;

    private class GdPrintLogger : ILogger
    {
        private readonly string _loggerSlug;
        private readonly ErrorSeverity _minimumSeverity;

        public GdPrintLogger(string loggerSlug, ErrorSeverity minimumSeverity)
        {
            _loggerSlug = loggerSlug;
            _minimumSeverity = minimumSeverity;
        }

        public void Info(string format, params object[] arguments)
        {
            if (_minimumSeverity <= ErrorSeverity.Info)
            {
                GD.Print($"{DateTime.Now:u} INF [{_loggerSlug}]: {string.Format(format, arguments)}");
            }
        }

        public void Warning(string format, params object[] arguments)
        {
            if (_minimumSeverity <= ErrorSeverity.Warning)
            {
                GD.PushWarning($"{DateTime.Now:u} WRN [{_loggerSlug}]: {string.Format(format, arguments)}");
            }
        }

        public void Error(string format, params object[] arguments)
        {
            if (_minimumSeverity <= ErrorSeverity.Error)
            {
                GD.PushError($"{DateTime.Now:u} ERR [{_loggerSlug}]: {string.Format(format, arguments)}");
            }
        }

        public void Exception(Exception exception)
        {
            if (_minimumSeverity <= ErrorSeverity.Error)
            {
                GD.PushError($"{DateTime.Now:u} ERR [{_loggerSlug}]: THROWN {exception.GetType().Name} {exception.Message}\n{exception}");
            }
        }
    }
    
    public GdPrintLoggerFactory(ErrorSeverity minimumSeverity = ErrorSeverity.Info)
    {
        _minimumSeverity = minimumSeverity;
    }
    
    public ILogger CreateServiceLogger<TService>()
    {
        return new GdPrintLogger(typeof(TService).Name, _minimumSeverity);
    }

    public ILogger CreateInstanceLogger<T>(string instanceId)
    {
        return new GdPrintLogger($"{typeof(T).Name}:{instanceId}", _minimumSeverity);
    }
}