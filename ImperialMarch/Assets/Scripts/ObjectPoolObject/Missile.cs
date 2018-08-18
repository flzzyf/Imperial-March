using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : IPooledObject
{
    public float speed = 5;

    public GameObject trail;
    public ParticleSystem particle;

    public GameObject gfx;

    public float lifetime = 2;

    [HideInInspector]
    public GameObject hitTarget;
    [HideInInspector]
    public Vector3 direction;
    [HideInInspector]
    public bool isActive;
	
	void Update () 
	{
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, speed * Time.deltaTime))
        {
            direction = hit.normal.normalized;

            hitTarget = hit.collider.gameObject;

            ObjectPoolManager.Instance().PutbackObject(gameObject);

            if (hit.collider.tag == "Enemy")
            {
                
            }
        }
        if(hitTarget == null)
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
	}

    public override void OnObjectPutback()
    {
        gfx.SetActive(false);

        isActive = false;

        if(trail != null)
            trail.GetComponent<TrailRenderer>().Clear();

        if (particle != null)
            particle.enableEmission = false;

        StopAllCoroutines();
    }

    public override void OnObjectSpawned()
    {
        base.OnObjectSpawned();

        isActive = true;

        gfx.SetActive(true);

        StartCoroutine(AutomaticPutback());

        hitTarget = null;
    }

    IEnumerator AutomaticPutback()
    {
        yield return new WaitForSeconds(lifetime);

        OnObjectPutback();
    }
}
