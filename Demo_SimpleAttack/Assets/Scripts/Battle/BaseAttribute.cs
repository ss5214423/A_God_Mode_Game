using UnityEngine;
using System.Collections;

public abstract class BaseAttribute : MonoBehaviour {

    [SerializeField]
    protected float m_moveSpeed;
    [SerializeField]
    protected float m_attackSpeed;
    [SerializeField]
    protected float m_attackValue;
    [SerializeField]
    protected float m_attackRange;

    public abstract void AttackOn(GameObject targetObj);
    public abstract void AttackOn(Vector3 targetPosition);
}
