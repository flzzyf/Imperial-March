using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_ImperialWrath : GeneralWeapon
{

    public override void Launch()
    {
        base.Trigger();

        StartCoroutine(IELaunch());
    }

    IEnumerator IELaunch()
    {
        Vector3 dir = zyf.GetMouseWorldPoint() - transform.position;

        Vector3 origin = transform.position;
        origin.y = 1;
        dir.y = 1;

        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit))
        {
            //命中
            if (hit.collider.tag == "Enemy")
            {
                hit.collider.GetComponent<Rigidbody>().AddForce(dir.normalized * Time.deltaTime * 20, ForceMode.Impulse);

                for (int i = 0; i < launchPos.Length; i++)
                {
                    GameObject go = Instantiate(particle_muzzleFlash, launchPos[i].position, launchPos[i].rotation);
                    float lifetime2 = go.GetComponent<ParticleSystem>().main.duration;
                    Destroy(go, lifetime2);
                }

                GameObject go2 = Instantiate(particle_impact, hit.point, Quaternion.LookRotation(hit.normal));
                float lifetime = go2.GetComponent<ParticleSystem>().main.duration;
                Destroy(go2, lifetime);

                SoundManager.Instance().PlayAtPoint("ImperialWrath_Launch", transform.position);

                for (int i = 0; i < 3; i++)
                {
                    yield return new WaitForSeconds(0.1f);
                    SoundManager.Instance().PlayAtPoint("ImperialWrath_Impact", hit.point);
                }
            }
        }

        
    }

}
