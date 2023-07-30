using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public GameObject Target;
    public float Yaxis;

    void Update()
    {
        TargetPointCalculate();
    }


    private void TargetPointCalculate()
    {
        gameObject.transform.position = new Vector3(Target.transform.position.x, Yaxis, Target.transform.position.z);
    }
}
