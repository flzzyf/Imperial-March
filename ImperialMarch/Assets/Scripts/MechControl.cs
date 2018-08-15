using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechControl : MonoBehaviour
{
    public float speed = 3;
    public float lowerPartRotationSpeed = 3;



    public Transform lowerPart;
    Animator lowerPartAnimator;
    public Transform upperPart;

    public GameObject[] weapons;

    void Start ()
    {
        lowerPartAnimator = lowerPart.GetComponent<Animator>();
	}
	
	void Update ()
    {
        LowerPart_Movement();

        UpperPart_Rotate();

        SearchTarget();
	}

    void LowerPart_Movement()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(-inputH, 0, -inputV).normalized;
        if (dir != Vector3.zero)
        {
            lowerPartAnimator.SetBool("walking", true);

            Vector3 movement = dir * speed * Time.deltaTime;
            transform.Translate(movement, Space.World);

            LookAtTarget(lowerPart, dir);
        }
        else
        {
            lowerPartAnimator.SetBool("walking", false);
        }
    }

    void UpperPart_Rotate()
    {
        LookAtTarget(upperPart, zyf.GetMouseWorldPoint() - transform.position);
    }

    void SearchTarget()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].GetComponent<GeneralWeapon>().SearchTarget();
        }
    }

    public void FootStep()
    {

    }

    public void EndOfWalk()
    {

    }

    void LookAtTarget(Transform _transform, Vector3 _dir)
    {
        Quaternion lookRotation = Quaternion.LookRotation(_dir);
        Vector3 rotation = Quaternion.Lerp(_transform.rotation, lookRotation, lowerPartRotationSpeed * Time.deltaTime).eulerAngles;
        _transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

}
