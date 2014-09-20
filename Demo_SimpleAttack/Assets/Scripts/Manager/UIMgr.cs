using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EUIPanelID
{ 
    Null=0,
    SoldierUnion,//佣兵工会
    WishPool,//许愿池
    PlayerInfo,//人物信息
    BattleFormation,//阵法
    SingleChallenge,//个人竞技
    MagicHunt,//魔能
    MagicEnergy,//魔能背包
    EverydayTask,//日常任务
    FurySkill,//怒气天赋
    DungeonPanel,//副本
    Nuli,//奴隶税收
    Practice,//修炼塔
    StrengthenEquip,//装备强化
    Divine,//占卜
    HellPanel,//地狱试练
    Chat,//聊天
    PrivateChat,//私信
    Union,//联盟
    AllUnion,//所有联盟
    TeamFight,
    Task,//任务
    Package,//背包
    XiaoFei,//消费
    HeroSkill,//人物技能
    HeroClass,
    Qiandao,//签到
    FighterRank,//排行榜
    JewelleryPanel,
    FightMessage,//战报
    GameSet,//设置
    BuyGold,//购买黄金
    BuySoul,//购买战魂
    Charge,//充能
    UnionSkill,//联盟技能
    EverydayAward,//每日奖励
    HelpPanel,//帮助信息
    JinkuangzhanPanel,//金矿战
    BuildingManage,//建筑管理
    Activities,//活动
    HeartDevil,//挑战心魔
    Reward,//通用奖励面板
    GodTree,//神树
    GodAnimalAttribute,//神兽属性
    GodAnimalEgg,//神兽砸蛋
    GodAnimalFormation,//神兽阵法
    WagerBear,//熊熊向前冲
    PracticeReward,//修炼塔收益
    HelpDesPanel,//通用帮助面板
    HelpLeaguePanel,//联盟军衔和爵位面板
    UnionMap,//联盟地图
    SlaveMine,//奴隶挖宝
    Triones,//七星阵
    RuinsSeizeTreasure,//遗迹夺宝
    EveryDaySignIn,//每日签到
    LeagueWar_FightMsg,//盟战战报
    LeagueWar_GetAward,//盟战领奖
    LeagueWar,//盟战
    SoldierInherit,//佣兵传承
    UnionNotice,//联盟公告
    OKCancel,//确定取消面板
    LocalRank,//本服排行
    StrideRank,//跨服排行
    MyRank,//我的排行
    CollectSpirit,//聚灵阵
    CityDefense,//守卫格劳城
    UnionMember,//联盟成员
    UnionLog,//联盟日志
    UnionAskFor,//申请联盟
    MagicUnderstand,//魔法领悟
    InterestQuestion,//趣味问答
    Notice,//公告面板
    HelpSkip,//帮助跳转
    //==============常用面板结束==============
    CommonPanelsEnd,//常用面板计数
    //接下来是动态加载的面板喔~
    AllPanelsEnd,
}

public class UIMgr : MonoBehaviour
{
    /// <summary>
    /// 动态面板属性
    /// </summary>
    private class StruUncommonPrefab
    {
        public EUIPanelID panelID;
        public GameObject panelObj;
        public float countDownTimer;
    }

    private struct DialogInfo
    {
        public string content;
        public ClickOKDelegate del;
    }

    public CallbackDelegateInt OnClosePanel;
    public CallbackDelegateInt OnOpenPanel;
    public CallbackDelegateInt OnReadyOpenPanel;

    private static UIMgr m_instance = null;
    public static UIMgr Instance
    {
        get
        {
            if (m_instance == null)
            {
                Debug.LogWarning("UIMgr Script Is Not Instantiate Or Missing");
            }
            return m_instance;
        }
    }

    public GameObject m_targetTopNode;

    //存放动态加载的资源，这部分的资源需要参与计时，如果超出时间，销毁
    private List<StruUncommonPrefab> m_panelsList_DynamicLoading;
    //用来计时
    private float m_nTimeInterval;
    //修改最大倒计时
    private float m_nSeconds;
    //注册事件啥的
    private EventMgr m_eventMgrScript;
    //消息提示框
    private UILabel m_okDialogLabelScript;
    //底板
    private GameObject m_mainPanelGameObj;
    //确认
    private GameObject m_okPanelGameObj;
    //购买
    private GameObject m_buyPanelObj;
    //提示
    private GameObject m_hintObj;
    //当前显示面板的ID们~
    private EUIPanelID[] m_shownPanelIDArr;
    //当前显示的面板数量
    private int m_nShownPanelCount;
    //使用底板的面板们~
    private bool[] m_bUseMainPanelArr;

    private EUIPanelID m_priorPanelID;
    //上一个打开的面板ID
    public EUIPanelID PriorPanelID
    {
        get
        {
            return m_priorPanelID;
        }
    }
    private object m_priorPanelParam;

    private EUIPanelID m_morePriorPanelID;
    //更早的面板ID
    public EUIPanelID MorePriorPanelID
    {
        get
        {
            return m_morePriorPanelID;
        }
    }
    private object m_morePriorPanelParam;

    /// <summary>
    /// 各种初始化
    /// </summary>
    void Awake()
    {
        m_instance = this;

        m_panelsList_DynamicLoading = new List<StruUncommonPrefab>();
        //时间间隔，用来判断动态加载的面板是否可以销毁
        m_nTimeInterval = 0;
        //倒计时总数，600秒
        m_nSeconds = 600;
        m_nShownPanelCount = 0;
        m_priorPanelID = EUIPanelID.Null;
        m_priorPanelParam = null;
        m_morePriorPanelID = EUIPanelID.Null;
        m_morePriorPanelParam = null;
        m_shownPanelIDArr = new EUIPanelID[10];
        m_bUseMainPanelArr = new bool[(int)EUIPanelID.AllPanelsEnd];

        m_eventMgrScript = new EventMgr();

        InitCommonPanels();

        for (int i = 0; i < m_bUseMainPanelArr.Length; ++i)
        {
            m_bUseMainPanelArr[i] = true;
        }
        m_hintObj = Resources.Load("Prefabs/Tooltip") as GameObject;
    }

    /// <summary>
    /// 初始化常用的面板
    /// </summary>
    private void InitCommonPanels()
    {
        for (int i = 0; i < (int)EUIPanelID.CommonPanelsEnd; ++i)
        {
            SetPanel_DynamicLoading((EUIPanelID)i);
        }
    }

    /// <summary>
    /// 每帧调用一次，主要就是检查是否有面板可以销毁
    /// </summary>
    void Update()
    {
        CheckUncommonPanels();
    }

    /// <summary>
    /// 关闭所有面板
    /// </summary>
    /// <param name="bOnlyPanelWithMainPanel"></param>
    public void HideAll(bool bOnlyPanelWithMainPanel)
    {
        for (int nIndex = 0; nIndex < m_nShownPanelCount; ++nIndex)
        {
                for (int i = 0; i < m_panelsList_DynamicLoading.Count; ++i)
                {
                    if (m_panelsList_DynamicLoading[i].panelID == m_shownPanelIDArr[nIndex])
                    {
                        m_panelsList_DynamicLoading[i].panelObj.SendMessage("DestroyUIData");
                        m_panelsList_DynamicLoading[i].panelObj.SetActive(false);
                        if (OnClosePanel != null)
                        {
                            OnClosePanel((int)m_shownPanelIDArr[nIndex]);
                        }
                    }
                }
        }
        m_nShownPanelCount = 0;
        m_priorPanelID = EUIPanelID.Null;
        m_priorPanelParam = null;
        m_morePriorPanelID = EUIPanelID.Null;
        m_morePriorPanelParam = null;
        m_mainPanelGameObj.SetActive(false);
    }

    /// <summary>
    /// 检查底板是否关闭
    /// </summary>
    private void CheckBottomFrame()
    {
        if (m_nShownPanelCount > 0)
        {
            bool bActive = false;
            for (int i = 0; i < m_nShownPanelCount; ++i)
            {
                if (true == m_bUseMainPanelArr[(int)m_shownPanelIDArr[i]])
                {
                    bActive = true;
                    break;
                }
            }
            if (m_mainPanelGameObj.activeSelf != bActive)
            {
                m_mainPanelGameObj.SetActive(bActive);
            }
        }
        else
        {
            m_priorPanelID = EUIPanelID.Null;
            m_priorPanelParam = null;
            m_morePriorPanelID = EUIPanelID.Null;
            m_morePriorPanelParam = null;
            m_mainPanelGameObj.SetActive(false);
        }
    }

    /// <summary>
    /// 设置是否使用底板
    /// 这个主要是用来设置不用底板的
    /// </summary>
    /// <param name="panelID">面板ID</param>
    /// <param name="bUseMainPanel">是否使用底板</param>
    public void SetUseMainPanel(EUIPanelID panelID, bool bUseMainPanel)
    {
        m_bUseMainPanelArr[(int)panelID] = bUseMainPanel;
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <param name="panelID">面板ID</param>
    /// <param name="bHideOther">是否隐藏其他的面板</param>
    /// <param name="param"></param>
    public void ShowPanel(EUIPanelID panelID, bool bHideOther, object param = null)
    {
        int nIndex = (int)panelID;
        if (bHideOther)
        {
            if (m_nShownPanelCount > 0)
            {
                for (int i = 0; i < m_nShownPanelCount; ++i)
                {
                    if (panelID != m_shownPanelIDArr[i])
                    {
                        HidePanelGameObject(m_shownPanelIDArr[i]);
                    }
                }
            }
        }

        ShowPanelGameObject(panelID, param);
        CheckBottomFrame();

        m_priorPanelID = panelID;
        m_priorPanelParam = param;
        m_morePriorPanelID = m_priorPanelID;
        m_morePriorPanelParam = m_priorPanelParam;
    }

    /// <summary>
    /// 显示面板的GameObject
    /// </summary>
    /// <param name="panelID"></param>
    /// <param name="param"></param>
    private void ShowPanelGameObject(EUIPanelID panelID, object param)
    {
        //开始设置面板

        //遍历动态面板的字典~
        //找到panelID，然后再把时间重置一下~
        for (int i = 0; i < m_panelsList_DynamicLoading.Count; ++i)
        {
            if (m_panelsList_DynamicLoading[i].panelID == panelID)
            {
                SetPanel(i, panelID, param);
                return;
            }
        }

        //如果在动态面板列表中没有找到这个面板，就动态加载一个
        SetPanel_DynamicLoading(panelID);
        //然后再设置
        if (m_panelsList_DynamicLoading[m_panelsList_DynamicLoading.Count - 1].panelID == panelID && m_panelsList_DynamicLoading[m_panelsList_DynamicLoading.Count - 1].panelObj != null)
        {
            SetPanel(m_panelsList_DynamicLoading.Count - 1, panelID, param);
        }
    }

    /// <summary>
    /// 设置面板
    /// </summary>
    /// <param name="nIndex">该面板在列表中的索引</param>
    /// <param name="panelID">面板ID</param>
    /// <param name="param"></param>
    private void SetPanel(int nIndex, EUIPanelID panelID, object param)
    {
        //重置时间，主要是针对普通面板
        if(m_panelsList_DynamicLoading[nIndex].countDownTimer < m_nSeconds)
        {
            m_panelsList_DynamicLoading[nIndex].countDownTimer = m_nSeconds;
        }
        if (m_panelsList_DynamicLoading[nIndex].panelObj != null)
        {
            Debug.Log("Panel : " + panelID);
            if (!m_panelsList_DynamicLoading[nIndex].panelObj.activeSelf)
            {
                ++m_nShownPanelCount;
            }
            m_panelsList_DynamicLoading[nIndex].panelObj.SetActive(true);
            m_shownPanelIDArr[m_nShownPanelCount - 1] = panelID;
            PanelScript tmpScript = m_panelsList_DynamicLoading[nIndex].panelObj.GetComponent<PanelScript>();
            tmpScript.UIParam = param;
            tmpScript.InitUIData();
            if (panelID == EUIPanelID.GameSet)
            {
                if ((int)param == 1)
                {
                    tmpScript.transform.localPosition = Vector3.forward * -1000;
                }
                else
                {
                    tmpScript.transform.localPosition = Vector3.forward * -1600;
                }
            }
        }

        if (m_nShownPanelCount > m_panelsList_DynamicLoading.Count)
        {
            CorrectShowPanelCount();
        }

        if (OnOpenPanel != null)
        {
            OnOpenPanel((int)panelID);
        }
    }

    /// <summary>
    /// 设置动态加载的面板
    /// [外部调用]
    /// </summary>
    /// <param name="panelID">面板ID</param>
    private void SetPanel_DynamicLoading(EUIPanelID panelID)
    {
        //初始化一个结构体
        StruUncommonPrefab tmpUncommonObj = new StruUncommonPrefab();
        //将结构体存放到列表中
        m_panelsList_DynamicLoading.Add(tmpUncommonObj);
        //主要就是加个ID，GameObject和一个倒计时时间
        //并且对GameObject进行一些设置
        m_panelsList_DynamicLoading[m_panelsList_DynamicLoading.Count - 1].panelID = panelID;
        //给需要长时间保持的面板，或者不通过UIMgr显示的面板一个足够长的时间
        if (panelID == EUIPanelID.HelpDesPanel || panelID == EUIPanelID.HelpLeaguePanel)
        {
            m_panelsList_DynamicLoading[m_panelsList_DynamicLoading.Count - 1].countDownTimer = 999999;
        }
        else
        {
            //否则就十分钟一次~
            m_panelsList_DynamicLoading[m_panelsList_DynamicLoading.Count - 1].countDownTimer = m_nSeconds;
        }
        m_panelsList_DynamicLoading[m_panelsList_DynamicLoading.Count - 1].panelObj = GameObject.Instantiate(Resources.Load("Panels/" + panelID + "/Panel")) as GameObject;
        m_panelsList_DynamicLoading[m_panelsList_DynamicLoading.Count - 1].panelObj.name = panelID.ToString();
        m_panelsList_DynamicLoading[m_panelsList_DynamicLoading.Count - 1].panelObj.transform.parent = m_targetTopNode.transform;
        m_panelsList_DynamicLoading[m_panelsList_DynamicLoading.Count - 1].panelObj.transform.localScale = Vector3.one;
        m_panelsList_DynamicLoading[m_panelsList_DynamicLoading.Count - 1].panelObj.transform.localPosition = new Vector3(0, 0, m_panelsList_DynamicLoading[m_panelsList_DynamicLoading.Count - 1].panelObj.transform.localPosition.z);

        //简单的初始化
        PanelScript tmpScript = m_panelsList_DynamicLoading[m_panelsList_DynamicLoading.Count - 1].panelObj.GetComponent<PanelScript>();
        tmpScript.AppInit();
        m_panelsList_DynamicLoading[m_panelsList_DynamicLoading.Count - 1].panelObj.SetActive(true);
        //注册这个面板的事件
        m_eventMgrScript.RegisterEvent(panelID);
    }

    /// <summary>
    /// 检查动态加载的面板
    /// [是否需要销毁]
    /// </summary>
    private void CheckUncommonPanels()
    {
        m_nTimeInterval += Time.deltaTime;
        //每过一段时间【目前是15秒】，需要判断面板是否需要销毁~~~
        if (m_nTimeInterval > 15)
        {
            for (int i = 0; i < m_panelsList_DynamicLoading.Count; ++i)
            {
                m_panelsList_DynamicLoading[i].countDownTimer -= m_nTimeInterval;
                //如果超时，移除这个家伙
                if (m_panelsList_DynamicLoading[i].countDownTimer < 0)
                {
                    m_panelsList_DynamicLoading[i].panelObj = null;
                    m_panelsList_DynamicLoading[i] = null;
                    m_panelsList_DynamicLoading.RemoveAt(i);
                    //注销事件
                    m_eventMgrScript.UnRegisterEvent(m_panelsList_DynamicLoading[i].panelID);
                }
            }
            m_nTimeInterval = 0;
        }
    }

    /// <summary>
    /// 销毁所有面板资源
    /// 只在切换场景时使用
    /// </summary>
    void OnDestroy()
    {
        //销毁所有动态加载的，还没有被释放的面板
        for (int i = 0; i < m_panelsList_DynamicLoading.Count; ++i)
        {
            m_panelsList_DynamicLoading[i].panelObj = null;
            m_panelsList_DynamicLoading[i] = null;
            m_eventMgrScript.UnRegisterEvent(m_panelsList_DynamicLoading[i].panelID);
        }
        m_panelsList_DynamicLoading.Clear();

        //m_bottomFramePanelScr = null;
        //m_buyPanelScr = null;
        m_okDialogLabelScript = null;
        m_buyPanelObj = null;
        m_mainPanelGameObj = null;
        m_okPanelGameObj = null;
        m_hintObj = null;

        m_eventMgrScript.UnRegisterSpecialEvent();

        //最后再把事件干掉
        m_eventMgrScript = null;
    }

    /// <summary>
    /// 关闭单个面板
    /// </summary>
    /// <param name="panelID">面板ID</param>
    private void HidePanelGameObject(EUIPanelID panelID)
    {
        for (int i = 0; i < m_panelsList_DynamicLoading.Count; ++i)
        {
            if (m_panelsList_DynamicLoading[i].panelID == panelID && m_panelsList_DynamicLoading[i].panelObj != null && m_panelsList_DynamicLoading[i].panelObj.activeSelf)
            {
                m_panelsList_DynamicLoading[i].panelObj.SendMessage("DestroyUIData");
                m_panelsList_DynamicLoading[i].panelObj.SetActive(false);
                for (int index = 0; i < m_nShownPanelCount; ++index)
                {
                    if (m_shownPanelIDArr[index] == panelID)
                    {
                        m_shownPanelIDArr[index] = m_shownPanelIDArr[m_nShownPanelCount - 1];
                        m_shownPanelIDArr[m_nShownPanelCount - 1] = EUIPanelID.CommonPanelsEnd;
                    }
                }
                --m_nShownPanelCount;
                if (m_nShownPanelCount < 0)
                {
                    CorrectShowPanelCount();
                }

                if (OnClosePanel != null)
                {
                    OnClosePanel((int)panelID);
                }
            }
        }
    }

    /// <summary>
    /// 据说是用来矫正ShowPanelCount
    /// </summary>
    private void CorrectShowPanelCount()
    { 
        //m_nShownPanelCount = 0;
        //for()
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="panelID">面板ID</param>
    public void HidePanel(EUIPanelID panelID,int nType = 0)
    {
        if (nType == 0)
        {
            HidePanelGameObject(panelID);
        }
        else if(nType == 1)
        {
            HidePanelGameObject(panelID);
            ShowPanelGameObject(m_priorPanelID,m_priorPanelParam);
        }
        CheckBottomFrame();
        m_morePriorPanelID = EUIPanelID.Null;
        m_morePriorPanelParam = null;
        m_priorPanelID = m_morePriorPanelID;
        m_priorPanelParam = m_morePriorPanelParam;
    }

    /// <summary>
    /// 判断当前是否有面板打开
    /// </summary>
    /// <returns></returns>
    public bool IsSomePanelShown()
    {
        return m_nShownPanelCount > 0;
    }

    /// <summary>
    /// 判断面板是否已经打开
    /// </summary>
    /// <param name="panelID"></param>
    /// <returns></returns>
    public bool IsPanelsShown(EUIPanelID panelID)
    {
        for (int i = 0; i < m_panelsList_DynamicLoading.Count; ++i)
        {
            if (m_panelsList_DynamicLoading[i].panelID == panelID && m_panelsList_DynamicLoading[i].panelObj.activeSelf)
            {
                return true;
            }
        }
        return false;
    }
}
