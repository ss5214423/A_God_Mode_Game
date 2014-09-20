using UnityEngine;
using System.Collections;

/// <summary>
/// 在事件注册这个类里，使用switch……case……，效率比if else高~
/// </summary>
public class EventMgr{

    /// <summary>
    /// 特殊事件
    /// 这些事件不会被注销
    /// </summary>
    public void RegisterSpecialEvent()
    {
    }

    /// <summary>
    /// 注销事件
    /// 只有在手动调用时才会被注销
    /// </summary>
    public void UnRegisterSpecialEvent()
    {
    }

    /// <summary>
    /// 注册事件
    /// Controller.Instance.Register();
    /// </summary>
    /// <param name="panelID">面板ID</param>
    public void RegisterEvent(EUIPanelID panelID)
    {
    }

    /// <summary>
    /// 注销事件
    /// Controller.Instance.Unregister();
    /// </summary>
    /// <param name="panelID">面板ID</param>
    public void UnRegisterEvent(EUIPanelID panelID)
    { 
    }
}
