using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OPO_Particle : IPooledObject
{
    public GameObject mainParticle
    {
        get
        {
            if (GetComponent<ParticleSystem>() != null)
                return gameObject;
            else
                return transform.GetChild(0).gameObject;
        }
    }

    public override void OnObjectSpawned()
    {
        base.OnObjectSpawned();

        foreach (var item in GetComponentsInChildren<ParticleSystem>())
        {
            item.Play();
        }
    }
}
