using UnityEngine;
using System.Collections;

public enum eSceneType
{ 
    Login,
    Main,
    Battle,
}

public class Scene : MonoBehaviour
{

    private static Scene m_instance;
    public static Scene Instance
    {
        get
        {
            if (m_instance == null)
            {
                Log.LogWarning("Scene Script Is Missing");
            }
            return m_instance;
        }
    }

    private eSceneType m_currentScene;
    public eSceneType CurrentScene
    {
        get
        {
            return m_currentScene;
        }
    }
    // Use this for initialization
    void Awake()
    {
        m_instance = this;
    }

    public void SetCurrentScene(eSceneType sceneType)
    {
        m_currentScene = sceneType;
    }
}
