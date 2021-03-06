﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechControl : MonoBehaviour
{
    public float speed = 3;
    public float lowerPartRotationSpeed = 3;
    public float upperPartRotationSpeed = 2;
    public float weaponRotationSpeed = 1;

    public Transform lowerPart;
    Animator lowerPartAnimator;
    public Transform upperPart;

    public GameObject[] weapons;

    public Transform[] weapon_main;

    bool walking;

    void Start ()
    {
        lowerPartAnimator = lowerPart.GetComponent<Animator>();
	}
	
	void Update ()
    {
        LowerPart_Movement();

        UpperPart_Rotate();

        SearchTarget();

        if(Input.GetMouseButton(0))
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].GetComponent<GeneralWeapon>().Trigger();
            }
        }
	}
    //下身移动
    void LowerPart_Movement()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(-inputH, 0, -inputV).normalized;
        if (dir != Vector3.zero)
        {
            walking = true;
            lowerPartAnimator.SetBool("walking", true);

            Vector3 movement = dir * speed * Time.deltaTime;
            transform.Translate(movement, Space.World);

            LookAtTarget(lowerPart, dir, lowerPartRotationSpeed);
        }
        else
        {
            walking = false;
            lowerPartAnimator.SetBool("walking", false);
        }
    }
    //上身旋转
    void UpperPart_Rotate()
    {
        //主武器旋转
        for (int i = 0; i < 2; i++)
        {
            LookAtTargetPos(weapon_main[i], zyf.GetMouseWorldPoint(), weaponRotationSpeed);
        }
        LookAtTargetPos(upperPart, zyf.GetMouseWorldPoint(), upperPartRotationSpeed);
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

    void LookAtTarget(Transform _origin, Vector3 _dir, float _rotationSpeed, bool _2d = false)
    {
        Quaternion lookRotation = Quaternion.LookRotation(_dir);
        Vector3 rotation = Quaternion.Lerp(_origin.rotation, lookRotation, _rotationSpeed * Time.deltaTime).eulerAngles;
        if(_2d)
        {
            _origin.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
        else
        {
            _origin.rotation = Quaternion.Euler(rotation);
        }
    }

    void LookAtTargetPos(Transform _origin, Vector3 _pos, float _rotationSpeed, bool _2d = false)
    {
        Vector3 dir = _pos - _origin.position;

        LookAtTarget(_origin, dir, _rotationSpeed, _2d);
    }
}
