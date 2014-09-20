using UnityEngine;
using System.Collections;

public enum eSkillType
{
    //直接伤害
    DirectDamage,
    //持续伤害
    DamageOverTime,
    //范围技能
    AreaOfEffect,
    //增益效果
    Buff,
    //减益效果
    Debuff,
    //直接治疗
    DirectTreatment,
    //持续治疗
    HealOverTime,
    //复活
    ReSpawn,
}

public enum eAttackType
{ 
    //近战
    MeleeAttack,
    //远程
    RangedAttack,
}