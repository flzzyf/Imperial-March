using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : IPooledObject
{
    public float speed = 5;

    public GameObject trail;

    public float lifetime = 2;

    [HideInInspector]
    public GameObject hitTarget;

    public Vector3 direction;
	
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

        hitTarget = null;
    }

    IEnumerator AutomaticPutback()
    {
        yield return new WaitForSeconds(lifetime);

        ObjectPoolManager.Instance().PutbackObject(gameObject);
    }
}
