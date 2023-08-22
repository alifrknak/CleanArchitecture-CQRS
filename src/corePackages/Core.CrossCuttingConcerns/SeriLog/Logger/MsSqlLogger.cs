﻿using Core.CrossCuttingConcerns.SeriLog.ConfigurationModels;
using Core.CrossCuttingConcerns.SeriLog.Messages;
using Microsoft.Extensions.Configuration;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.SeriLog.Logger;

public class MsSqlLogger : LoggerServiceBase
{
	public MsSqlLogger(IConfiguration configuration)
	{
		MsSqlConfiguration logConfiguration =
			configuration.GetSection("SeriLogConfigurations:MsSqlConfiguration").Get<MsSqlConfiguration>()
			?? throw new Exception(SerilogMessages.NullOptionsMessage);

		MSSqlServerSinkOptions sinkOptions = new()
		{
			TableName = logConfiguration.TableName,
			AutoCreateSqlDatabase = logConfiguration.AutoCreateSqlTable
		};

		ColumnOptions columnOptions = new();

		Serilog.Core.Logger seriLogConfig = new LoggerConfiguration().WriteTo
			.MSSqlServer(logConfiguration.ConnectionString, sinkOptions, columnOptions: columnOptions)
			.CreateLogger();

		Logger = seriLogConfig;
	}
}
