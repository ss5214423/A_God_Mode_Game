using UnityEngine;
using System.Collections;

public class BattleData {

    private static BattleData m_instance;
    public static BattleData Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new BattleData();
            }
            return m_instance;
        }
    }
}
