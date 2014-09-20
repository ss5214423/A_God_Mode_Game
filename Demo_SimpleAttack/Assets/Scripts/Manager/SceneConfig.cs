using UnityEngine;
using System.Collections;

public class SceneConfig : MonoBehaviour
{
    public eSceneType m_currentScene;

    // Use this for initialization
    void Awake()
    {
        Scene.Instance.SetCurrentScene(m_currentScene);
    }
}
