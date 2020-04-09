using UnityEngine;

/// <summary>
/// A logger that wraps Unity's internal logger.
/// Calls to its methods are stripped in case the LOGGER_SYMBOL is not defined.
/// </summary>
public sealed class Logger
{
	public const string LOGGER_SYMBOL = "ENABLE_LOG";

	[System.Diagnostics.Conditional(LOGGER_SYMBOL)]
	public static void Log(object message)
	{
		UnityEngine.Debug.Log(message);
	}
	
	[System.Diagnostics.Conditional(LOGGER_SYMBOL)]
	public static void Log(object message, Object context)
	{
		UnityEngine.Debug.Log(message, context);
	}

	[System.Diagnostics.Conditional(LOGGER_SYMBOL)]
	public static void LogFormat(string message, params object[] args)
	{
		UnityEngine.Debug.LogFormat(message, args);
	}
	
	[System.Diagnostics.Conditional(LOGGER_SYMBOL)]
	public static void LogFormat(Object context, string message, params object[] args)
	{
		UnityEngine.Debug.LogFormat(context, message, args);
	}

	[System.Diagnostics.Conditional(LOGGER_SYMBOL)]
	public static void LogWarning(object message)
	{
		UnityEngine.Debug.LogWarning(message);
	}
	
	[System.Diagnostics.Conditional(LOGGER_SYMBOL)]
	public static void LogWarning(object message, Object context)
	{
		UnityEngine.Debug.LogWarning(message, context);
	}

	[System.Diagnostics.Conditional(LOGGER_SYMBOL)]
	public static void LogWarningFormat(string message, params object[] args)
	{
		UnityEngine.Debug.LogWarningFormat(message, args);
	}
	
	[System.Diagnostics.Conditional(LOGGER_SYMBOL)]
	public static void LogWarningFormat(Object context, string message, params object[] args)
	{
		UnityEngine.Debug.LogWarningFormat(context, message, args);
	}

	[System.Diagnostics.Conditional(LOGGER_SYMBOL)]
	public static void LogError(object message)
	{
		UnityEngine.Debug.LogError(message);
	}

	[System.Diagnostics.Conditional(LOGGER_SYMBOL)]
	public static void LogError(object message, Object context)
	{
		UnityEngine.Debug.LogError(message, context);
	}

	[System.Diagnostics.Conditional(LOGGER_SYMBOL)]
	public static void LogErrorFormat(string message, params object[] args)
	{
		UnityEngine.Debug.LogErrorFormat(message, args);
	}
	
	[System.Diagnostics.Conditional(LOGGER_SYMBOL)]
	public static void LogErrorFormat(Object context, string message, params object[] args)
	{
		UnityEngine.Debug.LogErrorFormat(context, message, args);
	}

	[System.Diagnostics.Conditional(LOGGER_SYMBOL)]
	public static void LogException(System.Exception exception)
	{
		UnityEngine.Debug.LogException(exception);
	}
	
	[System.Diagnostics.Conditional(LOGGER_SYMBOL)]
	public static void LogException(System.Exception exception, Object context)
	{
		UnityEngine.Debug.LogException(exception, context);
	}

	[System.Diagnostics.Conditional(LOGGER_SYMBOL)]
    public static void DrawLine(Vector3 start, Vector3 end, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
        UnityEngine.UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
    }

    [System.Diagnostics.Conditional(LOGGER_SYMBOL)]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
        UnityEngine.UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
    }

    [System.Diagnostics.Conditional(LOGGER_SYMBOL)]
    public static void Assert(bool condition)
    {
        if (!condition) throw new Exception();
    }
}