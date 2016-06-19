using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;
using log4net.Config;

[assembly: log4net.Config.XmlConfigurator(Watch = false)]

class Program
{
    //System.Reflection.MethodBase.GetCurrentMethod().DeclaringType
    private static readonly ILog log =
        LogManager.GetLogger(typeof(Program));

    static void Main(string[] args)
    {
        //BasicConfigurator.Configure();
        //XmlConfigurator.Configure(new System.IO.FileInfo("config.xml"));

        log.Info("Entering application...");
        var f = new Log4netSamples.Foo();
        f.DoIt();
        var b = new Log4netSamples.Bar();
        b.DoIt();
        log.Info("Exiting application...");
        Console.ReadKey();
    }

}

namespace Log4netSamples
{
    

    class Foo
    {
        static readonly ILog log = LogManager.GetLogger(typeof(Foo));

        internal void DoIt()
        {
            log.Warn("Foo Did it again...");
        }
    }

    class Bar
    {
        static readonly ILog log = LogManager.GetLogger(typeof(Bar));

        internal void DoIt()
        {
            log.Warn("Bar Did it again...");
        }
    }
}
