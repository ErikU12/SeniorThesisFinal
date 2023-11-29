using UnityEngine;

namespace Scenes.Scripts
{
    public class CameraController : MonoBehaviour
    {
        public Transform target;   
        public float smoothSpeed = 0.125f;  
        public Vector3 offset;      
        
        private void LateUpdate()
        {
            if (target != null)
            {
                Vector3 desiredPosition = target.position + offset;
                desiredPosition.z = transform.position.z;
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
                transform.position = smoothedPosition;
            }
        }

    }
}