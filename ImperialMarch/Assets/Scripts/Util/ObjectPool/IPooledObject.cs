using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPooledObject : MonoBehaviour
{
    public virtual void OnObjectSpawned()
    {
        gameObject.SetActive(true);
    }
	
    public virtual void OnObjectPutback()
    {
        gameObject.SetActive(false);
    }
}
