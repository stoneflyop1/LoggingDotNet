using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoggingTrace.Listeners;
using System.Threading;
using System.Reflection;

namespace LoggingTrace
{
    class Program
    {
        static void Main(string[] args)
        {
            //SimpleTrace();
            //SimpleTraceWithConfig();
            TraceWithSource();
            Console.ReadKey();
        }

        static void SimpleTrace()
        {
            Trace.AutoFlush = true;
            Trace.Listeners.Add(new ConsoleTraceListener());
            Trace.Listeners.Add(new CustomTextListener("simpletrace.log"));
            Trace.WriteLine("Start SimpleTrace...", "Info"); // Info: Start SimpleTrace...
            Trace.Indent();
            Trace.WriteLine("Indent Text...", "Info");
            Trace.TraceInformation("Info..."); // exeName Information: 0 : Info...
            Trace.Unindent();
            Trace.WriteLine("End SimpleTrace...", "Info");
        }
        /// <summary>
        /// Uncomment [system.diagnostics].[trace] configSection to use
        /// </summary>
        static void SimpleTraceWithConfig()
        {
            Trace.WriteLine("Start SimpleTraceWithConfig...", "Info"); // Info: Start SimpleTrace...
            Thread.Sleep(1000);
            Trace.Indent();
            Trace.WriteLine("Indent Text1...", "Info");
            Trace.WriteLine("Indent Text2...", "Info");
            Thread.Sleep(1000);
            Trace.WriteLine("Logging Without Category...");
            //Trace.WriteLine(DateTime.Now);
            //Trace.WriteLine(DateTime.Now, "Info");
            //Trace.TraceInformation("Info..."); // exeName Information: 0 : Info...
            //Trace.Unindent();
            //Trace.WriteLine("End SimpleTraceWithConfig...", "Info");
        }

        static void TraceWithSource()
        {
            var source = new TraceSource(MethodBase.GetCurrentMethod().DeclaringType.FullName, SourceLevels.Information);
            source.Listeners.Add(new ConsoleTraceListener());
            source.Listeners.Add(new CustomTextListener("customtracesource.log"));
            source.TraceInformation("Trace With Source...");
            source.TraceData(TraceEventType.Warning, 0, "Trace Warning...");
            source.Close();
            Utils.TraceWithSource();
        }
    }

    class Utils
    {
        internal static void TraceWithSource()
        {
            var source = new TraceSource(MethodBase.GetCurrentMethod().DeclaringType.FullName, SourceLevels.Information);
            source.Listeners.AddRange(Trace.Listeners); //If trace config has listeners, use them; or use listeners below
            //source.Listeners.Add(new ConsoleTraceListener());
            //source.Listeners.Add(new CustomTextListener("customtracesource.log"));
            source.TraceInformation("Trace With Source...");
            source.TraceData(TraceEventType.Warning, 0, "Trace Warning...");
            source.Close();
        }
    }
}
