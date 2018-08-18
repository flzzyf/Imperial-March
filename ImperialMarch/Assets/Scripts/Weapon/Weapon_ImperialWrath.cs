using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_ImperialWrath : GeneralWeapon
{
    public string prefab_missile;

    public override void Launch()
    {
        base.Launch();

        GameObject missile = ObjectPoolManager.Instance().SpawnObject(prefab_missile, launchPos[0].position, launchPos[0].rotation);

        StartCoroutine(WaitUntilHit(missile));
    }

    IEnumerator WaitUntilHit(GameObject _missile)
    {
        while(_missile.GetComponent<Missile>().isActive)
        {
            yield return null;
        }

        GameObject target = _missile.GetComponent<Missile>().hitTarget;
        if (target != null)
        {
            //命中
            CreateParticle(particle_impact, _missile.transform);
            SoundManager.Instance().PlayAtPoint(sound_impact, _missile.transform.position);

            if (target.tag == "Enemy")
            {
                Vector3 dir = _missile.GetComponent<Missile>().direction;
                target.GetComponent<Rigidbody>().AddForce(-dir * forceAmount, ForceMode.Impulse);
            }
               
        }
    }

}
