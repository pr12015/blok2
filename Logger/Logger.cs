using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EventLogger
{
    public enum EventType
    {
        AuthenticationSuccess,
        AuthenticationFailure,
        AuthorizationSuccess,
        AuthorizationFailure
    }

    public class Logger
    {
        private string _source;
        private string _logName;

        public Logger() { }

        public Logger(string source, string logName)
        {
            _source = source;
            _logName = logName;

            if (!EventLog.SourceExists(_source))
            {
                EventLog.CreateEventSource(_source, _logName);
            }
        }

        /// <summary>
        /// Logs authentication outcome.
        /// </summary>
        /// <param name="user">username</param>
        /// <param name="type"></param>
        public void Log(string user, EventType type)
        {
            using (var log = new EventLog(_logName))
            {
                log.MachineName = Environment.MachineName;
                log.Source = _source;
                switch (type)
                {
                    case EventType.AuthenticationSuccess:
                        log.WriteEntry($"User {user} is successfully authenticated.");
                        break;
                    case EventType.AuthenticationFailure:
                        log.WriteEntry($"User {user} failed to authenticate.");
                        break;
                }
            }
        }

        /// <summary>
        /// Logs authorization outcome.
        /// </summary>
        /// <param name="user">username</param>
        /// <param name="method"></param>
        /// <param name="permission"></param>
        /// <param name="type"></param>
        public void Log(string user, string method, string permission, EventType type)
        {
            using (var log = new EventLog(_logName))
            {
                log.MachineName = Environment.MachineName;
                log.Source = _source;
                switch (type)
                {
                    case EventType.AuthorizationSuccess:
                        log.WriteEntry($"User {user} successfully accessed {method}().");
                        break;
                    case EventType.AuthorizationFailure:
                        log.WriteEntry($"User {user} failed to access {method}(). Reason: Missing {permission} permission");
                        break;
                }
            }
        }
    }
}
