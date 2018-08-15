using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPooledObject : MonoBehaviour
{
    public virtual void OnObjectSpawned() { }
	
    public virtual void OnObjectPutback() { }
}
