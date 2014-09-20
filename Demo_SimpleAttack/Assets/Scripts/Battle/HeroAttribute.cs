using UnityEngine;
using System.Collections;

public class HeroAttribute : BaseAttribute,IMove {
    public override void AttackOn(GameObject targetObj)
    {
        throw new System.NotImplementedException();
    }

    public override void AttackOn(Vector3 targetPosition)
    {
        throw new System.NotImplementedException();
    }

    public void MoveTo(Vector3 targetPosition)
    {
        throw new System.NotImplementedException();
    }
}
