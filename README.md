# C#中的Logging
## 使用Trace
Trace有两种方式，一种是直接用Trace类的静态方法，还有一种是用TraceSource。两者可以共用Listener。

## 使用log4net
此部分参照了log4net的官方手册，其中大部分的实例都来自官方手册，略有改动。
### log4net简介
log4net是Apache主导的基于微软的.NET的Log系统，与Java中的log4j对应。
log在软件的开发过程中是很重要的一个方面。log可以提供软件执行的日志，设计良好的log可以帮助开发人员更加明确软件执行的过程，软件出BUG时可以更快的定位问题所在等。笔者认为，设计良好的log首先要能做到不同的log要有所区分，一般的信息，是警告信息，还是出错信息。根据这些不同的级别可以在不同的地方进行log的输出。log4net就提供了log的级别划分。而且，log4net还有一个非常棒的特性，定制log输出只需要配置就可以了。

 - ALL
 - DEBUG
 - INFO
 - WARN
 - ERROR
 - FATAL
 - OFF


### loggers和appenders
log4net主要包含三个部分：logger、appender以及layout。logger是log输出的定义实体，通过指定不同的appender给logger即可在不同的介质中输出log；appender即定义好的输出log的方式，而layout是跟appender紧密相连的，是指appender输出的格式。
#### loggers
在同一个软件中，可以定义多个不同的logger以完成不同类型的log输出。这些不同的logger之间可以有一定的父子关系。
其中最特殊的一个logger是root，是所有其他logger的根。父子关系是通过logger的名称确定的，其规则与C#中的命名空间类似。例如：名称为“A”的logger是名称为“A.X”的logger的父logger。一般地，每个logger都是一个类，其名称就是类的全名(包括命名空间部分)。
根据父子关系，若子logger未指定输出级别，则会继承父logger的级别。`在log4net中，logger由ILog接口表示。`
#### appenders
appender会指定log最终要输出的地方，比如是文件、控制台、还是通过网络传输等。要自定义log输出的地方，需要继承log4net中的appender类。

 * log4net.Appender.ConsoleAppender
 * log4net.Appender.FileAppender
 * log4net.Appender.RollingFileAppender
 * log4net.Appender.TraceAppender
 * log4net.Appender.EventLogAppender
 * ...

### log4net的配置（Configuration）
为了有一个感性的认识，先来看一个简单的例子：
	
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

上面的例子是配置了一个控制台的logger进行输出，logger的名字叫：MyApp，Appender采用的是ConsoleAppender。下面给出一个有两个logger的例子。第一个文件：MyApp.cs：
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

第二个文件：Bar.cs

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


上面的两个例子都是按照默认的控制台配置给出的，是log4net自带的配置。如何自定义配置了，答案是通过XML格式配置。下面给出一个配置的例子。主程序如下：

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

XML配置文件相关内容如下：

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

上面是采用外部文件进行配置，可否按照应用程序配置来做呢？答案是可以的。只需要对程序集声明一个Attribute并在app.config中加入相应的配置内容即可。

	[assembly: log4net.Config.XmlConfigurator(Watch=true)]

app.config内容如下：

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


下面给出一种结合了外部配置文件和app.config的配置方式。
首先准备好外部配置文件，比如：config.xml。
在app.config的appSettings中添加：

    <add key="log4net.Config" value="config.xml"/>
    <add key="log4net.Config.Watch" value="True"/>

在程序集中声明：

	//此处的false将不起作用，最终看log4net.Config.Watch的配置的值
	[assembly: log4net.Config.XmlConfigurator(Watch = false)]


## 参考
* http://www.daveoncsharp.com/2009/09/create-a-logger-using-the-trace-listener-in-csharp/
* http://www.cnblogs.com/luminji/archive/2010/10/26/1861316.html
* https://msdn.microsoft.com/en-us/library/system.diagnostics.tracesource.aspx
* http://blog.stephencleary.com/2010/12/simple-and-easy-tracing-in-net.html
* http://stackoverflow.com/questions/21781510/loading-configuration-for-system-diagnostics-tracesource-from-an-xml-file
* http://stackoverflow.com/questions/24172868/tracesource-set-autoflush-to-true-without-config-file
* http://logging.apache.org/log4net/