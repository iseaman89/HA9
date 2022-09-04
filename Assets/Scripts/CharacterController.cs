using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : Character
{
    private Rigidbody _rigidbody;

    [SerializeField] private float movingForce = 20.0f;
    [SerializeField] private float jumpForce = 80f;
    [SerializeField] private float maxSlope = 30f;
    [SerializeField] private float maxSpeed;
    
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

    private void Destroyed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        LookAtTarget();
        Shoot();
        UpdateTimer();
    }

    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 shootDirection = transform.forward;
            shootDirection = GetShootDirection(shootDirection, Gun.position);
            ShootBullet(shootDirection);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            Vector3 shootDirection = transform.forward;
            shootDirection = GetShootDirection(shootDirection, Gun.position);
            ShootRocket(shootDirection);
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
            else
            {
                _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, maxSpeed);
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

    private Vector3 GetShootDirection(Vector3 shootDirection, Vector3 gunPosition)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetVector = hit.point - gunPosition;

            if (Vector3.Angle(shootDirection, targetVector) < 45)
            {
                shootDirection = targetVector;
            }

        }
        return shootDirection;
    }

    private void LookAtTarget()
    {
        float distance;
        Plane plane = new Plane(Vector3.up, transform.position);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out distance))
        {
            Vector3 position = ray.GetPoint(distance);
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
