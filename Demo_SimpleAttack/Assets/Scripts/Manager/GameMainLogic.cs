using UnityEngine;
using System.Collections;

public class GameMainLogic : MonoBehaviour {

    void Awake()
    {
        GameSetting.Instance.Init();
    }

    void OnApplicationPause()
    { 
    
    }

    void OnApplicationQuit()
    { 
        
    }
}
