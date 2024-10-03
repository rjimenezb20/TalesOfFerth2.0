using UnityEngine;
public class CameraController : MonoBehaviour
{
    private Camera MainCamera;

    [Header("MOVE")]
    public float CameraSpeed = .5f;
    public float CameraSmooth = .5f;

    [Header("LIMITS")]
    public float minX;
    public float minZ;
    public float maxX;
    public float maxZ;


    [Header("ZOOM")]
    public float ZoomSpeed = .5f;
    public float MaxZoom = 10f;
    public float MinZoom = 28f;

    private void Start()
    {      
        MainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        CameraMove();
        CameraZoom();
    }

    private void CameraMove()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 screenSize = new Vector3(Screen.width, Screen.height, 0);
        Vector3 movement = Vector3.zero;

        // Left
        if (/*mousePosition.x < Boundaries ||*/ Input.GetKey("a"))
        {
            movement.x -= CameraSpeed;
            movement.z -= CameraSpeed;
        }

        // Right
        if (/*mousePosition.x > screenSize.x - Boundaries ||*/ Input.GetKey("d"))
        {
            movement.x += CameraSpeed;
            movement.z += CameraSpeed;
        }

        // Up
        if (/*mousePosition.y > screenSize.y - Boundaries ||*/ Input.GetKey("w"))
        {
            movement.x -= CameraSpeed;
            movement.z += CameraSpeed;
        }

        // Down
        if (/*mousePosition.y < Boundaries ||*/ Input.GetKey("s"))
        {
            movement.x += CameraSpeed;
            movement.z -= CameraSpeed;
        }

        Vector3 desiredPosition = MainCamera.transform.localPosition + movement;

        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        desiredPosition.z = Mathf.Clamp(desiredPosition.z, minZ, maxZ);

        MainCamera.transform.localPosition = Vector3.Lerp(MainCamera.transform.localPosition, desiredPosition, CameraSmooth);
    }

    private void CameraZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f && MainCamera.orthographicSize - ZoomSpeed >= MaxZoom)
        {
            MainCamera.orthographicSize -= ZoomSpeed;
        }

        if (scroll < 0f && MainCamera.orthographicSize + ZoomSpeed <= MinZoom)
        {
            MainCamera.orthographicSize += ZoomSpeed;
        }
    }
}