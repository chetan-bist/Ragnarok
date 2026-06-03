using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [Header("Aspect Ratio Settings")]
    public float targetWidth = 19.2f; 

    [Header("Follow Settings")]
    public Transform target;          
    public float smoothTime = 0.2f;   
    public Vector3 offset = new Vector3(0f, 0f, -10f); 

    [Header("Level Bounds (Camera Limit)")]
    public bool useBounds = true;
    // Inspector bata yo values milayera camera lai left jana bata roknu parcha
    public float minX = 0f;           // Camera space ko minimum X position (Left Limit)
    public float maxX = 100f;         // Camera space ko maximum X position (Right Limit)
    public float minY = 0f;           // Bottom limit
    public float maxY = 10f;          // Top limit

    private Camera cam;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (cam != null)
        {
            cam.orthographicSize = targetWidth / cam.aspect / 2f;
        }

        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;

            // Use bounds to stop camera from showing the blue empty space
            if (useBounds)
            {
                targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
                targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
            }

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}