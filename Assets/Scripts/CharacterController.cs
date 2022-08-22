using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField] private float movingForce = 20.0f;
    [SerializeField] private float jumpForce = 80f;
    [SerializeField] private float maxSlope = 30f;
    [SerializeField] private float shootPower;
    [SerializeField] private float rocketPower;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private Transform gun;
    [SerializeField] private Transform rocketGun;
    private float damping = 0.3f;

    private bool onGround = false;

    private void Awake()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {
        onGround = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        onGround = CheckIsOnGround(collision);
    }

    private void Update()
    {
        LookAtTarget();
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject newBullet = Instantiate(bulletPrefab, gun.position, gun.rotation) as GameObject;
            newBullet.GetComponent<Rigidbody>().AddForce(gun.forward * shootPower);
            Destroy(newBullet, 5);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            GameObject newRocket = Instantiate(rocketPrefab, rocketGun.position, rocketGun.rotation) as GameObject;
            newRocket.AddComponent<ConstantForce>();
            Destroy(newRocket, 5);
        }
    }

    private void FixedUpdate()
    {
        if (onGround)
        {
            ApplyMovingForce();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rigidbody.AddForce(Vector3.up * jumpForce);
            }
        }
    }

    private void ApplyMovingForce()
    {
        Vector3 xAxisForce = transform.right * Input.GetAxis("Horizontal");
        Vector3 zAxisForce = transform.forward * Input.GetAxis("Vertical");

        Vector3 resultXZForce = xAxisForce + zAxisForce;

        resultXZForce.Normalize();

        if (resultXZForce.magnitude > 0)
        {
            resultXZForce *= movingForce;
            _rigidbody.AddForce(resultXZForce);

        }
        else
        {
            Vector3 dampedVelocity = _rigidbody.velocity * damping;
            dampedVelocity.y = _rigidbody.velocity.y;
            _rigidbody.velocity = dampedVelocity;
        }


    }

    private void LookAtTarget()
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 position = ray.GetPoint(hit.distance);
            position.y = transform.position.y;
            transform.LookAt(position);
        }
    }

    private bool CheckIsOnGround(Collision collision)
    {
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            if (collision.contacts[i].point.y < transform.position.y)
            {
                if (Vector3.Angle(collision.contacts[i].normal, Vector3.up) < maxSlope)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
