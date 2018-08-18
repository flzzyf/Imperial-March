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

    public string particle_launch;
    public string particle_impact;

    public Transform[] launchPos;

    public string sound_launch;
    public string sound_impact;

    public float forceAmount = 1;

    public virtual void SearchTarget()
    {
        Vector3 origin = transform.position;

        RaycastHit hit;
        if (Physics.Raycast(origin, transform.forward, out hit, range))
        {
            if (hit.collider.tag == "Enemy")
            {
                Trigger();
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
        //创建开火粒子
        for (int i = 0; i < launchPos.Length; i++)
        {
            CreateParticle(particle_launch, launchPos[i]);
        }
    }

    void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
        origin.y = 0;

        Vector3 dir = transform.forward;
        dir.y = 0;
        Vector3 target = origin + dir.normalized * range;

        Gizmos.color = Color.green;
        for (int i = 0; i < 3; i++)
        {
            DrawRayEnhanced(origin, target + transform.right * splashRadius);
            DrawRayEnhanced(origin, target + -transform.right * splashRadius);
            DrawRayEnhanced(target + transform.right * splashRadius, target + -transform.right * splashRadius);
        }
    }

    void DrawRayEnhanced(Vector3 _origin, Vector3 _target)
    {
        for (int i = 0; i < 5; i++)
        {
            Gizmos.DrawLine(_origin, _target);
        }
    }

    //从对象池创建粒子
    public void CreateParticle(string _particle, Transform _transform)
    {
        GameObject go = ObjectPoolManager.Instance().SpawnObject(_particle, _transform.position, _transform.rotation);
        float lifetime = go.GetComponent<OPO_Particle>().mainParticle.GetComponent<ParticleSystem>().main.duration;

        StartCoroutine(PutbackParticle(go, lifetime));
    }

    IEnumerator PutbackParticle(GameObject _go, float _lifetime)
    {
        yield return new WaitForSeconds(_lifetime);
        ObjectPoolManager.Instance().PutbackObject(_go);
    }
}
