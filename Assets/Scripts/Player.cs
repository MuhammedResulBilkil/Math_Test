using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform testCube;
    public float rotateSpeed;
    public float rotateAngle = 60f;

    [SerializeField] private bool _dotProduct;
    [SerializeField] private bool _crossProduct;
    [SerializeField] private bool _reflection;
    [SerializeField] private bool _projectOnPlane;

    private Quaternion _targetRotation;

        // Start is called before the first frame update
    void Start()
    {
        _targetRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _targetRotation *= Quaternion.AngleAxis(rotateAngle, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, rotateSpeed * Time.deltaTime);
        
        
        #region DOT Product

        if (_dotProduct)
        {
            Vector3 direction = testCube.position - transform.position;
            direction.Normalize();
        
            float dot1 = Vector3.Dot(transform.forward, direction);
            float dot2 = transform.forward.x * direction.x + transform.forward.y * direction.y +
                         transform.forward.z * direction.z; 
            float dotProductAngle = Mathf.Acos(dot1 / transform.forward.magnitude * direction.magnitude);

            Debug.Log("Dot 1 = " + dot1);
            Debug.Log("Dot 2 = " + dot2);
            Debug.Log("Angle = " + dotProductAngle * Mathf.Rad2Deg);
        }
        
        #endregion
        
        #region Cross Product

        if (_crossProduct)
        {
            Vector3 crossProduct = Vector3.Cross(transform.forward, transform.right);
            float crossProductAngle = Mathf.Asin(crossProduct.magnitude / -transform.forward.magnitude * transform.right.magnitude);
        
            Debug.Log("Cross Product = " + crossProduct);
            Debug.Log("Cross Product Angle = " + crossProductAngle * Mathf.Rad2Deg);
        }
        
        #endregion

        #region Get Normal and Reflection

        if (_reflection)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 100f))
            {
                Vector3 dir = hitInfo.point - transform.position;
                dir.Normalize();

                Vector3 reflection = Vector3.Reflect(dir, hitInfo.normal);
            
                Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.red);
                Debug.DrawRay(hitInfo.point, -dir, Color.blue);
                Debug.DrawRay(hitInfo.point, reflection, Color.green);
            }
        }

        #endregion

        #region Project On Plane

        if (_projectOnPlane)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 100f))
            {
                Vector3 dir = transform.position - hitInfo.point;
                dir.Normalize();

                Vector3 projectOnPlane = Vector3.ProjectOnPlane(-dir, hitInfo.normal);
            
                Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.red);
                Debug.DrawRay(hitInfo.point, dir, Color.blue);
                Debug.DrawRay(hitInfo.point, projectOnPlane, Color.green);
            }
        }
        
        #endregion
    }
}
