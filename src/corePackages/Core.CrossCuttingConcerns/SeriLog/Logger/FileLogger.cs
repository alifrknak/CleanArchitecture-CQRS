using Core.CrossCuttingConcerns.SeriLog.ConfigurationModels;
using Core.CrossCuttingConcerns.SeriLog.Messages;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;

namespace Core.CrossCuttingConcerns.SeriLog.Logger;

public class FileLogger : LoggerServiceBase
{
	private readonly IConfiguration _configuration;
	private readonly FileLogConfiguration _fileLogConfiguration;

	public FileLogger(IConfiguration configuration)
	{
		_configuration = configuration;

		_fileLogConfiguration = configuration.GetSection("SeriLogConfigurations:FileLogConfiguration").Get<FileLogConfiguration>()
			?? throw new Exception(SerilogMessages.NullOptionsMessage);

		string logFilePath = string.Format(format: "{0}{1}", arg0: Directory.GetCurrentDirectory() + _fileLogConfiguration.FolderPath, arg1: ".txt");

		Logger = new LoggerConfiguration().WriteTo.File(
		  logFilePath, rollingInterval: RollingInterval.Day,
		  retainedFileCountLimit: null,
		  fileSizeLimitBytes: 5000000,
		  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}"
		  ).CreateLogger();
	}

	
}
