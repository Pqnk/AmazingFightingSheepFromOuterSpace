using System.Collections.Generic;
using UnityEngine;

public class GravityAreaPoint : GravityArea
{
    [SerializeField] private Vector3 _center;
    [SerializeField] private GameObject _centerObject;

    public override Vector3 GetGravityDirection(GravityBody _gravityBody)
    {
        if(_centerObject == null )
        {
            return (_center - _gravityBody.transform.position).normalized;
        }
        else
        {
            return (_centerObject.transform.position - _gravityBody.transform.position).normalized;
        }
    }
}
