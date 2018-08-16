using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : IPooledObject
{
    public float speed = 5;

    public GameObject trail;

    public float lifetime = 2;

    [HideInInspector]
    public bool hitTarget = false;
	
	void Update () 
	{
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, speed * Time.deltaTime))
        {
            hitTarget = true;

            ObjectPoolManager.Instance().PutbackObject(gameObject);
        }

        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
	}

    public override void OnObjectPutback()
    {
        base.OnObjectPutback();

        trail.GetComponent<TrailRenderer>().Clear();

        StopAllCoroutines();
    }

    public override void OnObjectSpawned()
    {
        base.OnObjectSpawned();

        StartCoroutine(AutomaticPutback());

        hitTarget = false;
    }

    IEnumerator AutomaticPutback()
    {
        yield return new WaitForSeconds(lifetime);

        ObjectPoolManager.Instance().PutbackObject(gameObject);
    }
}
