using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Camera MainCamera;

    [Header("MOVE")]
    public float CameraSpeed = .5f;

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
        //Left
        if(Input.mousePosition.x <= 5 || Input.GetKey("a"))
        {
            MainCamera.transform.localPosition = MainCamera.transform.localPosition + new Vector3(-CameraSpeed / 2, 0, -CameraSpeed / 2);
        }

        //Right
        if (Input.mousePosition.x >= Screen.width - 5 || Input.GetKey("d"))
        {
            MainCamera.transform.localPosition = MainCamera.transform.localPosition + new Vector3(CameraSpeed / 2, 0, CameraSpeed / 2);
        }

        //Up
        if (Input.mousePosition.y >= Screen.height - 5 || Input.GetKey("w"))
        {
            MainCamera.transform.localPosition = MainCamera.transform.localPosition + new Vector3(-CameraSpeed, 0, CameraSpeed);
        }

        //Down
        if (Input.mousePosition.y <= 50 || Input.GetKey("s"))
        {
            MainCamera.transform.localPosition = MainCamera.transform.localPosition + new Vector3(CameraSpeed, 0, -CameraSpeed);
        }
    }

    private void CameraZoom()
    {
        //Forward 
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if(MainCamera.orthographicSize - ZoomSpeed >= MaxZoom)
            {
                MainCamera.orthographicSize -= ZoomSpeed;
            }
        }

        //Backwards 
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (MainCamera.orthographicSize + ZoomSpeed <= MinZoom)
            {
                MainCamera.orthographicSize += ZoomSpeed;
            }        
        }
    }
}