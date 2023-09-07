using System;

namespace Witchpixels.Tanks.Logging;

public interface ILogger
{
    /// <summary>
    /// Logs a string with information.
    ///
    /// A Info is defined as a log line that informs about something for adding context when debugging but is
    /// non-critical. Actionable items can go here if they are not considered problematic.
    /// </summary>
    public void Info(string format, params object[] arguments);
    
    /// <summary>
    /// Logs a warning.
    ///
    /// Warning level is for things that are non-critical misbehaviour that should be noted but will not cause a
    /// catastrophic failure later down the line. Things like position drift, unexpected number of collisions, exceeded
    /// time boxes for pathing, ect.
    ///
    /// Warnings should all eventually be crushed before shipping a production build as either the warning is incorrect
    /// and the unexpected behaviour is permissible, or the unexpected behaviour has been removed.
    /// </summary>
    public void Warning(string format, params object[] arguments);
    
    /// <summary>
    /// Logs an Error.
    ///
    /// An error is an always-actionable event that something has gone terribly wrong. Errors should be used to
    /// highlight an issue that must be fixed and, naturally, errors should never exhibit themselves on production
    /// builds. It is safe and sensible on release builds to stop the process to put these into a remove error tracker
    /// as, again, these should never happen. 
    /// </summary>
    public void Error(string format, params object[] arguments);
    
    /// <summary>
    /// Logs an exception. This is a helper method for Error, but with standardized formatting.
    ///
    /// See remarks from Error.
    /// </summary>
    public void Exception(Exception exception);
}