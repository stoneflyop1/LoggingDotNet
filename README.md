# C#�е�Logging
## ʹ��Trace
Trace�����ַ�ʽ��һ����ֱ����Trace��ľ�̬����������һ������TraceSource�����߿��Թ���Listener��

## ʹ��log4net
�˲��ֲ�����log4net�Ĺٷ��ֲᣬ���д󲿷ֵ�ʵ�������Թٷ��ֲᣬ���иĶ���
### log4net���
log4net��Apache�����Ļ���΢���.NET��Logϵͳ����Java�е�log4j��Ӧ��
log������Ŀ����������Ǻ���Ҫ��һ�����档log�����ṩ���ִ�е���־��������õ�log���԰���������Ա������ȷ���ִ�еĹ��̣������BUGʱ���Ը���Ķ�λ�������ڵȡ�������Ϊ��������õ�log����Ҫ��������ͬ��logҪ�������֣�һ�����Ϣ���Ǿ�����Ϣ�����ǳ�����Ϣ��������Щ��ͬ�ļ�������ڲ�ͬ�ĵط�����log�������log4net���ṩ��log�ļ��𻮷֡����ң�log4net����һ���ǳ��������ԣ�����log���ֻ��Ҫ���þͿ����ˡ�

 - ALL
 - DEBUG
 - INFO
 - WARN
 - ERROR
 - FATAL
 - OFF


### loggers��appenders
log4net��Ҫ�����������֣�logger��appender�Լ�layout��logger��log����Ķ���ʵ�壬ͨ��ָ����ͬ��appender��logger�����ڲ�ͬ�Ľ��������log��appender������õ����log�ķ�ʽ����layout�Ǹ�appender���������ģ���ָappender����ĸ�ʽ��
#### loggers
��ͬһ������У����Զ�������ͬ��logger����ɲ�ͬ���͵�log�������Щ��ͬ��logger֮�������һ���ĸ��ӹ�ϵ��
�����������һ��logger��root������������logger�ĸ������ӹ�ϵ��ͨ��logger������ȷ���ģ��������C#�е������ռ����ơ����磺����Ϊ��A����logger������Ϊ��A.X����logger�ĸ�logger��һ��أ�ÿ��logger����һ���࣬�����ƾ������ȫ��(���������ռ䲿��)��
���ݸ��ӹ�ϵ������loggerδָ������������̳и�logger�ļ���`��log4net�У�logger��ILog�ӿڱ�ʾ��`
#### appenders
appender��ָ��log����Ҫ����ĵط����������ļ�������̨������ͨ�����紫��ȡ�Ҫ�Զ���log����ĵط�����Ҫ�̳�log4net�е�appender�ࡣ

 * log4net.Appender.ConsoleAppender
 * log4net.Appender.FileAppender
 * log4net.Appender.RollingFileAppender
 * log4net.Appender.TraceAppender
 * log4net.Appender.EventLogAppender
 * ...

### log4net�����ã�Configuration��
Ϊ����һ�����Ե���ʶ��������һ���򵥵����ӣ�
	
	using log4net;
	using log4net.Config;	

	public class MyApp 
	{
    	// Define a static logger variable so that it references the Logger instance named "MyApp".
    	private static readonly ILog log = LogManager.GetLogger(typeof(MyApp));

    	static void Main(string[] args) 
    	{
        	// Set up a simple configuration that logs on the console.
        	BasicConfigurator.Configure();
        	log.Info("Entering application.");
        	System.Threading.Thread.Sleep(1000);
        	log.Info("Exiting application.");
    	}
	}

�����������������һ������̨��logger���������logger�����ֽУ�MyApp��Appender���õ���ConsoleAppender���������һ��������logger�����ӡ���һ���ļ���MyApp.cs��
	//MyApp.cs
	using Com.Foo;
	using log4net;
	using log4net.Config;

	public class MyApp 
	{
    	private static readonly ILog log = LogManager.GetLogger(typeof(MyApp));

    	static void Main(string[] args) 
    	{
        	BasicConfigurator.Configure();

        	log.Info("Entering application.");
        	Bar bar = new Bar();
        	bar.DoIt();
        	log.Info("Exiting application.");
    	}
	}

�ڶ����ļ���Bar.cs

	// Bar.cs
	using log4net;

	namespace Com.Foo
	{
    	public class Bar 
    	{
        	private static readonly ILog log = LogManager.GetLogger(typeof(Bar));

        	public void DoIt()
        	{
            	log.Debug("Did it again!");
        	}
    	}
	}


������������Ӷ��ǰ���Ĭ�ϵĿ���̨���ø����ģ���log4net�Դ������á�����Զ��������ˣ�����ͨ��XML��ʽ���á��������һ�����õ����ӡ����������£�

	using Com.Foo;

	// Import log4net classes.
	using log4net;
	using log4net.Config;

	public class MyApp 
	{
    	private static readonly ILog log = LogManager.GetLogger(typeof(MyApp));

    	static void Main(string[] args) 
    	{
        	// BasicConfigurator replaced with XmlConfigurator.
        	XmlConfigurator.Configure(new System.IO.FileInfo(args[0]));

        	log.Info("Entering application.");
        	Bar bar = new Bar();
        	bar.DoIt();
        	log.Info("Exiting application.");
    	}
	}

XML�����ļ�����������£�

	<log4net>
    <!-- A1 is set to be a ConsoleAppender -->
    <appender name="A1" type="log4net.Appender.ConsoleAppender">
        <!-- A1 uses PatternLayout -->
        <layout type="log4net.Layout.PatternLayout">
            <conversionPattern value="%-4timestamp [%thread] %-5level %logger %ndc - %message%newline" />
        </layout>
    </appender>    
    <!-- Set root logger level to DEBUG and its only appender to A1 -->
    <root>
        <level value="DEBUG" />
        <appender-ref ref="A1" />
    </root>
    <!-- Print only messages of level WARN or above in the package Com.Foo -->
    <logger name="Com.Foo">
        <level value="WARN" />
    </logger>
	</log4net>

�����ǲ����ⲿ�ļ��������ã��ɷ���Ӧ�ó������������أ����ǿ��Եġ�ֻ��Ҫ�Գ�������һ��Attribute����app.config�м�����Ӧ���������ݼ��ɡ�

	[assembly: log4net.Config.XmlConfigurator(Watch=true)]

app.config�������£�

	<?xml version="1.0" encoding="utf-8" ?>
	<configuration>
    <configSections>
        <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
    </configSections>
    <log4net>
        <appender name="ConsoleAppender" type="log4net.Appender.ConsoleAppender" >
            <layout type="log4net.Layout.PatternLayout">
                <conversionPattern value="%date [%thread] %-5level %logger [%ndc] - %message%newline" />
            </layout>
        </appender>
        <root>
            <level value="INFO" />
            <appender-ref ref="ConsoleAppender" />
        </root>
    </log4net>
	</configuration>


�������һ�ֽ�����ⲿ�����ļ���app.config�����÷�ʽ��
����׼�����ⲿ�����ļ������磺config.xml��
��app.config��appSettings����ӣ�

    <add key="log4net.Config" value="config.xml"/>
    <add key="log4net.Config.Watch" value="True"/>

�ڳ�����������

	//�˴���false���������ã����տ�log4net.Config.Watch�����õ�ֵ
	[assembly: log4net.Config.XmlConfigurator(Watch = false)]


## �ο�
* http://www.daveoncsharp.com/2009/09/create-a-logger-using-the-trace-listener-in-csharp/
* http://www.cnblogs.com/luminji/archive/2010/10/26/1861316.html
* https://msdn.microsoft.com/en-us/library/system.diagnostics.tracesource.aspx
* http://blog.stephencleary.com/2010/12/simple-and-easy-tracing-in-net.html
* http://stackoverflow.com/questions/21781510/loading-configuration-for-system-diagnostics-tracesource-from-an-xml-file
* http://stackoverflow.com/questions/24172868/tracesource-set-autoflush-to-true-without-config-file
* http://logging.apache.org/log4net/