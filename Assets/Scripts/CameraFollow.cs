using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 5.0f;
    public float rotationSpeed = 5.0f;
    public Vector3 offset = new Vector3(0, 5, -5); // Default camera offset

    [Header("Zoom Settings")]
    public float zoomSpeed = 15.0f;
    public float minZoom = 2.0f;
    public float maxZoom = 10.0f;

    public float heightOffset = 1.5f; // Adjusted to keep camera focus on upper body

    private float currentZoom;
    private Camera cam; // Reference to Camera component

    void Start()
    {
        cam = GetComponent<Camera>(); 
        cam.nearClipPlane = 1.3f;


        currentZoom = offset.magnitude;


        if (player != null)
        {
            // Instantly set the camera position and rotation at the start
            Vector3 zoomedOffset = offset.normalized * currentZoom;
            Vector3 targetPosition = player.position + zoomedOffset + Vector3.up * heightOffset;
            transform.position = targetPosition;

            Vector3 lookAtTarget = player.position + Vector3.up * heightOffset;
            transform.rotation = Quaternion.LookRotation(lookAtTarget - transform.position);
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Handle zoom with scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scroll * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        // Adjust zoom keeping original offset direction
        Vector3 zoomedOffset = offset.normalized * currentZoom;
        Vector3 targetPosition = player.position + zoomedOffset + Vector3.up * heightOffset;

        // Smooth follow movement
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Smooth rotation to focus on the player
        Vector3 lookAtTarget = player.position + Vector3.up * heightOffset;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAtTarget - transform.position), rotationSpeed * Time.deltaTime);
    }
}