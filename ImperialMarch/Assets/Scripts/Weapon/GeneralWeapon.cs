using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralWeapon : MonoBehaviour
{
    public float cooldownTime = 0.2f;

    public float splashRadius = 1;
    public float range = 10;
    //攻击次数
    public int attackCount = 1;
    public float attackInterval;

    float lastFireTime = 0;

    public GameObject particle_launch;
    public GameObject particle_impact;

    public Transform[] launchPos;

    public string sound_launch;
    public string sound_impact;

    public float scanningCount = 5;

    public LineRenderer lineRenderer;

    public virtual void SearchTarget()
    {
        Vector3 origin = transform.position;
        origin.y = 0;

        Vector3 lineOrigin = transform.position;
        lineOrigin.y = 0.1f;
        //设置扫描线位置
        //lineRenderer.transform.position = lineOrigin;

        //lineRenderer.SetPosition(0, lineOrigin);
        //lineRenderer.SetPosition(1, origin + transform.forward * range);

        RaycastHit hit;
        
        for (int j = 0; j < scanningCount; j++)
        {
            origin.y += 1f;

            if (Physics.Raycast(origin, transform.forward, out hit, range))
            {
                if (hit.collider.tag == "Enemy")
                {
                    Trigger();
                }
            }
        }
    }

    public virtual void Trigger()
    {
        if(Time.time - lastFireTime > cooldownTime || lastFireTime == 0)
        {
            lastFireTime = Time.time;

            StartCoroutine(MultiAttack());
        }
    }
    //多次攻击
    IEnumerator MultiAttack()
    {
        SoundManager.Instance().PlayAtPoint(sound_launch, transform.position);

        for (int i = 0; i < attackCount; i++)
        {
            Launch();

            yield return new WaitForSeconds(attackInterval);
        }
    }

    public virtual void Launch()
    {
    }

    void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
        origin.y = 0;

        Vector3 target = origin + transform.forward * range;

        Gizmos.color = Color.green;
        for (int i = 0; i < 3; i++)
        {
            DrawRayEnhanced(origin, target + transform.right * splashRadius);
            DrawRayEnhanced(origin, target + -transform.right * splashRadius);
            DrawRayEnhanced(target + transform.right * splashRadius, target + -transform.right * splashRadius);
        }

        for (int j = 0; j < scanningCount; j++)
        {
            origin.y += 1f;
            target.y += 1f;

            Gizmos.color = Color.red;
            DrawRayEnhanced(origin, target);
        }
    }

    void DrawRayEnhanced(Vector3 _origin, Vector3 _target)
    {
        for (int i = 0; i < 5; i++)
        {
            Gizmos.DrawLine(_origin, _target);
        }
    }
}
