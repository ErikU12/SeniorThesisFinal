using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;   
    public float smoothSpeed = 0.125f;  
    public Vector3 offset;

    private void Start()
    {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            var position = transform.position;
            desiredPosition.z = position.z;
            Vector3 smoothedPosition = Vector3.Lerp(position, desiredPosition, smoothSpeed * Time.deltaTime);
            position = smoothedPosition;
            transform.position = position;
        }
    }

}