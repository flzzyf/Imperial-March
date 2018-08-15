using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralWeapon : MonoBehaviour
{
    public float cooldownTime = 0.2f;

    public float splashRadius = 1;
    public float range = 10;

    float lastFireTime = 0;

    public GameObject particle_muzzleFlash;
    public GameObject particle_impact;

    public Transform[] launchPos;

    public virtual void Trigger()
    {
        if(Time.time - lastFireTime > cooldownTime || lastFireTime == 0)
        {
            lastFireTime = Time.time;
            Launch();
        }
    }

    public virtual void Launch()
    {

    }

    void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
        origin.y = 0;

        Vector3 target = origin + Vector3.forward * range;

        Gizmos.color = Color.green;
        for (int i = 0; i < 3; i++)
        {
            Gizmos.DrawLine(origin, target + Vector3.right * splashRadius);
            Gizmos.DrawLine(origin, target + -Vector3.right * splashRadius);
            Gizmos.DrawLine(target + Vector3.right * splashRadius, target + -Vector3.right * splashRadius);
        }
        
    }
}
