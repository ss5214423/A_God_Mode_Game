using UnityEngine;
using System.Collections;

public enum eLogLevel
{ 
    Log = 0,
    Warning = 1,
    Error = 2,
    None = 3,
}

public class Log 
{
    private static Log m_instance;
    public static Log Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new Log();
            }
            return m_instance;
        }
    }

    private eLogLevel m_currentLogLevel;

    public static void SetLogLevel(eLogLevel logLevel)
    {
        Instance.m_currentLogLevel = logLevel;
    }

    public static void LogInfo(string strLog)
    {
        if (Instance.m_currentLogLevel <= eLogLevel.Log)
        {
            Debug.Log(strLog);
        }
    }

    public static void LogWarning(string strLog)
    {
        if (Instance.m_currentLogLevel <= eLogLevel.Warning)
        {
            Debug.LogWarning(strLog);
        }
    }

    public static void LogError(string strLog)
    {
        if (Instance.m_currentLogLevel <= eLogLevel.Error)
        {
            Debug.LogWarning(strLog);
        }
    }
}
