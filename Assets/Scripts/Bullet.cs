using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _forceAmount;
    [SerializeField] private bool _reflection;
    [SerializeField] private bool _projectOnPlane;

    private Rigidbody _rigidbody;
    private Vector3 _moveForwardVector;
    private bool _isCollided;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _moveForwardVector = transform.forward;
    }

    private void Start()
    {
        //_rigidbody.AddForce(Vector3.forward * _forceAmount, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        if(!other.gameObject.CompareTag("Wall"))
            return;
        
        #region Reflect
        
        if (_reflection)
        {
            _isCollided = true;

            _moveForwardVector = Vector3.Reflect(transform.forward, other.contacts[0].normal);
            /*float angle = GetAngleFromDotProduct(-inDirection, _moveForwardVector);
            Vector3 rotationEuler = transform.rotation.eulerAngles;
            rotationEuler.y = 180f - angle;*/
            //transform.position = hitInfo.point; ///////
            Quaternion rotation = Quaternion.LookRotation(_moveForwardVector);
            transform.rotation = rotation;

            Debug.Log("Reflect = " + _moveForwardVector);
            Debug.DrawRay(other.contacts[0].point, _moveForwardVector * 20f, Color.blue, 20f);
            Debug.DrawRay(other.contacts[0].point, other.contacts[0].normal * 20f, Color.red, 20f);
            Debug.DrawRay(other.contacts[0].point, transform.forward * 20f, Color.magenta, 20f);
            
            /*if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 100f))
            {
                Vector3 inDirection = hitInfo.point - transform.position;
                inDirection.Normalize();

                _moveForwardVector = Vector3.Reflect(inDirection, hitInfo.normal);
                /*float angle = GetAngleFromDotProduct(-inDirection, _moveForwardVector);
                Vector3 rotationEuler = transform.rotation.eulerAngles;
                rotationEuler.y = 180f - angle;#1#
                transform.position = hitInfo.point; ///////
                Quaternion rotation = Quaternion.LookRotation(_moveForwardVector);
                transform.rotation = rotation;

                Debug.Log("Reflect = " + _moveForwardVector);
                Debug.DrawRay(hitInfo.point, _moveForwardVector * 20f, Color.blue, 20f);
                Debug.DrawRay(hitInfo.point, hitInfo.normal * 20f, Color.red, 20f);
                Debug.DrawRay(hitInfo.point, -inDirection * 20f, Color.magenta, 20f);

                
            }*/
        }

        #endregion

        #region Project

        if (_projectOnPlane)
        {
            _isCollided = true;
            
            _moveForwardVector = Vector3.ProjectOnPlane(transform.forward, other.contacts[0].normal);
            /*float angle = GetAngleFromDotProduct(-inDirection, _moveForwardVector);
            Vector3 rotationEuler = transform.rotation.eulerAngles;
            rotationEuler.y = 180f - angle;
            transform.rotation = Quaternion.Euler(rotationEuler);*/
            Quaternion rotation = Quaternion.LookRotation(_moveForwardVector);
            transform.rotation = rotation;
                
            Debug.Log("Project On Plane = " + _moveForwardVector);
            Debug.DrawRay(other.contacts[0].point, _moveForwardVector * 20f, Color.blue, 20f);
            Debug.DrawRay(other.contacts[0].point, other.contacts[0].normal * 20f, Color.red, 20f);
            Debug.DrawRay(other.contacts[0].point, transform.forward * 20f, Color.magenta, 20f);
            
            /*if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 100f))
            {
                Vector3 inDirection = hitInfo.point - transform.position;
                inDirection.Normalize();

                _moveForwardVector = Vector3.ProjectOnPlane(inDirection, hitInfo.normal);
                /*float angle = GetAngleFromDotProduct(-inDirection, _moveForwardVector);
                Vector3 rotationEuler = transform.rotation.eulerAngles;
                rotationEuler.y = 180f - angle;
                transform.rotation = Quaternion.Euler(rotationEuler);#1#
                Quaternion rotation = Quaternion.LookRotation(_moveForwardVector);
                transform.rotation = rotation;
                
                Debug.Log("Project On Plane = " + _moveForwardVector);
                Debug.DrawRay(hitInfo.point, _moveForwardVector * 20f, Color.blue, 20f);
                Debug.DrawRay(hitInfo.point, hitInfo.normal * 20f, Color.red, 20f);
                Debug.DrawRay(hitInfo.point, -inDirection * 20f, Color.magenta, 20f);
                
                //transform.position = hitInfo.point;
            }*/
        }

        #endregion
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * _forceAmount * Time.deltaTime);

        if (!_isCollided)
        {
            if (_reflection)
            {
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 100f))
                {
                    Vector3 inDirection = hitInfo.point - transform.position;
                    inDirection.Normalize();

                    //_moveForwardVector = Vector3.Reflect(inDirection, hitInfo.normal);

                    Debug.DrawRay(hitInfo.point, Vector3.Reflect(inDirection, hitInfo.normal) * 20f, Color.yellow, 20f);
                    Debug.DrawRay(hitInfo.point, hitInfo.normal * 20f, Color.cyan, 20f);
                    Debug.DrawRay(hitInfo.point, -inDirection * 20f, Color.green, 20f);
                }
            }

            if (_projectOnPlane)
            {
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 100f))
                {
                    Vector3 inDirection = hitInfo.point - transform.position;
                    inDirection.Normalize();

                    //_moveForwardVector = Vector3.ProjectOnPlane(inDirection, hitInfo.normal);

                    Debug.DrawRay(hitInfo.point, Vector3.ProjectOnPlane(inDirection, hitInfo.normal) * 20f, Color.yellow, 20f);
                    Debug.DrawRay(hitInfo.point, hitInfo.normal * 20f, Color.cyan, 20f);
                    Debug.DrawRay(hitInfo.point, -inDirection * 20f, Color.green, 20f);
                }
            }
        }
    }

    private float GetAngleFromDotProduct(Vector3 a, Vector3 b)
    {
        float dotProduct = Vector3.Dot(a.normalized, b.normalized);
        float dotProductAngle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
        
        Debug.Log("Dot Product Angle = " + dotProductAngle);
        
        return dotProductAngle;
    }
}