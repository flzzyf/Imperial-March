using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechControl : MonoBehaviour
{
    public float speed = 3;

    public Transform lowerPart;
    public float lowerPartRotationSpeed = 3;
    Animator lowerPartAnimator;
    public Transform upperPart;

    Vector3 mouseWorldPoint;

    public GameObject particle_muzzleFlash;
    public GameObject particle_impact;

    public Transform[] launchPos;

    void Start ()
    {
        lowerPartAnimator = lowerPart.GetComponent<Animator>();
	}
	
	void Update ()
    {
        mouseWorldPoint = GetMouseWorldPoint();

        LowerPart_Movement();

        UpperPart_Rotate();

        SearchTarget();
	}

    void LowerPart_Movement()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(inputH, 0, inputV).normalized;
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
        Vector3 dir = mouseWorldPoint - transform.position;

        LookAtTarget(upperPart, dir);
    }

    void SearchTarget()
    {
        Vector3 dir = mouseWorldPoint - transform.position;

        Vector3 origin = transform.position;
        origin.y = 1;
        dir.y = 1;

        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit))
        {
            if(hit.collider.tag == "Enemy")
            {
                hit.collider.GetComponent<Rigidbody>().AddForce(dir.normalized * Time.deltaTime * 20, ForceMode.Impulse);

                for (int i = 0; i < launchPos.Length; i++)
                {
                    GameObject go = Instantiate(particle_muzzleFlash, launchPos[i].position, launchPos[i].rotation);
                    float lifetime2 = go.GetComponent<ParticleSystem>().main.duration;
                    Destroy(go, lifetime2);
                }

                GameObject go2 = Instantiate(particle_impact, hit.point, Quaternion.LookRotation(hit.normal));
                float lifetime = go2.GetComponent<ParticleSystem>().main.duration;
                Destroy(go2, lifetime);
            }
        }

        Debug.DrawRay(origin, dir);
    }

    //获取鼠标在世界中的位置
    Vector3 GetMouseWorldPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }

        return Vector3.zero;
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
