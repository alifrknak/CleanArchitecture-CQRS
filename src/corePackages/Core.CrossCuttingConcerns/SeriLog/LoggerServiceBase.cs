using Serilog;

namespace Core.CrossCuttingConcerns.SeriLog;

public abstract  class LoggerServiceBase
{
	protected ILogger Logger { get; set; }

	protected LoggerServiceBase()
	{
		Logger = null;
	}
	protected LoggerServiceBase(ILogger logger)
	{
		Logger = logger;
	}


	public virtual  void Verbose(string message) => Logger.Verbose(message);

	public virtual void Fatal(string message) => Logger.Fatal(message);

	public virtual void Info(string message) => Logger.Information(message);

	public virtual void Warn(string message) => Logger.Warning(message);

	public virtual void Debug(string message) => Logger.Debug(message);

	public virtual void Error(string message) => Logger.Error(message);
}
