using UnityEngine;
using System.Collections;

public class GameSetting
{
    private static GameSetting m_instance;
    public static GameSetting Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameSetting();
            }
            return m_instance;
        }
    }

    public void Init()
    {
        Log.SetLogLevel(eLogLevel.Log);
    }
}
