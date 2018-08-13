using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform cam;
    public Transform target;

    public Vector3 offset;

	void Update ()
    {
        cam.position = target.position + offset;
	}
}
