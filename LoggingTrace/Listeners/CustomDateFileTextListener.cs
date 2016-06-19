using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LoggingTrace.Listeners
{
    /// <summary>
    /// Creating a new log file each second with datetime as filename, only for demo
    /// </summary>
    public class CustomDateFileTextListener : TextWriterTraceListener
    {
        private string _dateStr;

        public CustomDateFileTextListener() : base()
        {
            _dateStr = GetCurrentDateStr();
        }

        private string GetCurrentDateStr()
        {
            return DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        }

        public override void Write(string message)
        {
            ChangeWriterIfNeeded();
            base.Write(message);
        }

        private void ChangeWriterIfNeeded()
        {
            if (Writer == null)
            {
                Writer = new StreamWriter(_dateStr + ".log", true, Encoding.Default);
            }
            else
            {
                var fileName = GetCurrentDateStr();
                if (fileName != _dateStr)
                {
                    _dateStr = fileName;
                    Writer.Flush();
                    Writer.Dispose();
                    Writer = new StreamWriter(_dateStr + ".log", true, Encoding.Default);
                }

            }
        }

        public override void WriteLine(string message)
        {
            ChangeWriterIfNeeded();
            base.WriteLine(message);
        }
    }
}
