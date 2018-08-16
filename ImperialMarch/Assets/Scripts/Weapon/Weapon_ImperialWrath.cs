using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_ImperialWrath : GeneralWeapon
{
    public GameObject missile;

    public override void Launch()
    {
        base.Launch();

        GameObject missile = ObjectPoolManager.Instance().SpawnObject("missile", launchPos[0].position, launchPos[0].rotation);
        //StartCoroutine(IELaunch());

        //创建开火粒子
        for (int i = 0; i < launchPos.Length; i++)
        {
            CreateParticle(particle_launch, launchPos[i]);
        }

        StartCoroutine(WaitUntilHit(missile));
    }

    IEnumerator WaitUntilHit(GameObject _missile)
    {
        while(_missile.activeSelf)
        {
            yield return null;

        }

        if(_missile.GetComponent<Missile>().hitTarget)
        {
            //命中
            print("命中");
            CreateParticle(particle_impact, _missile.transform);
            SoundManager.Instance().PlayAtPoint(sound_impact, _missile.transform.position);
        }
    }

    IEnumerator IELaunch()
    {
        Vector3 dir = zyf.GetMouseWorldPoint() - transform.position;

        Vector3 origin = transform.position;
        origin.y = 1;
        dir.y = 1;

        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit, range))
        {
            //命中
            if (hit.collider.tag == "Enemy")
            {
                hit.collider.GetComponent<Rigidbody>().AddForce(dir.normalized * Time.deltaTime * 150, ForceMode.Impulse);
                

                GameObject go2 = Instantiate(particle_impact, hit.point, Quaternion.LookRotation(hit.normal));
                float lifetime = go2.GetComponent<ParticleSystem>().main.duration;
                Destroy(go2, lifetime);


                SoundManager.Instance().PlayAtPoint(sound_impact, hit.point);

            }
        }

        yield return null;
        
    }

}
