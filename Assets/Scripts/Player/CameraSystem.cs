using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraSystem : MonoBehaviour
{
    [Header("General")]
    public float worldBounds = 100f;
    public CinemachineVirtualCamera cinemachineVirtualCamera;

    [Header("Movement")]
    public float moveSpeed = 20f;
    public bool useEdgeScroll = true;
    public int edgeScrollSize = 20;

    [Header("Rotation")]
    public float rotationSpeed = 200f;
    public bool usePanRot = true;

    [Header("Zoom")]
    public float zoomSpeed = 10f;
    public float zoomAmout = 3f;
    public float followOffsetMaxY = 50f;
    public float followOffsetMinY = 5f;
        
    private bool dragpanRotationActive = false;
    private Vector2 lastMousePosition;
    private Vector3 followOffset;
    private void Start()
    {
        followOffset = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
    }

    // Update is called once per frame
    void Update()
    {
        HandleCameraMove(out Vector3 inputDir);
        HandleEdgeScroll(out Vector3 finalInputDir, inputDir);



        HandleCameraRotation();
        HandleCameraZoom_LowerY();

        Vector3 moveDir = transform.forward * finalInputDir.z + transform.right * finalInputDir.x;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float clampedX = Mathf.Clamp(transform.position.x, -worldBounds, worldBounds);
        float clampedZ = Mathf.Clamp(transform.position.z, -worldBounds, worldBounds);
        transform.position = new Vector3(clampedX, transform.position.y, clampedZ);

    }


    private void HandleCameraMove(out Vector3 inputDir)
    {
        inputDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) inputDir.z = 1f;
        if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) inputDir.x = 1f;
    }

    private void HandleEdgeScroll(out Vector3 inputDir,Vector3 startInputDir)
    {
        inputDir = startInputDir;
        if (useEdgeScroll)
        {
            if (Input.mousePosition.x < edgeScrollSize) inputDir.x = -1f;
            if (Input.mousePosition.y < edgeScrollSize) inputDir.z = -1f;
            if (Input.mousePosition.x > Screen.width - edgeScrollSize) inputDir.x = 1f;
            if (Input.mousePosition.y > Screen.height - edgeScrollSize) inputDir.z = 1f;
        }
    }

    private void HandleCameraRotation()
    {
        float rotateDir = 0f;

        if (usePanRot)
        {
            if (Input.GetMouseButtonDown(2))
            {
                dragpanRotationActive = true;
                lastMousePosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(2))
            {
                dragpanRotationActive = false;
            }
            if (dragpanRotationActive)
            {
                Vector2 mouseMovDelta = (Vector2)Input.mousePosition - lastMousePosition;

                rotateDir = mouseMovDelta.x;

                lastMousePosition = Input.mousePosition;
            }
        }

        if (Input.GetKey(KeyCode.Q)) rotateDir = 1f;
        if (Input.GetKey(KeyCode.E)) rotateDir = -1f;

        transform.eulerAngles += new Vector3(0, rotateDir * rotationSpeed * Time.deltaTime, 0);

    }

    private void HandleCameraZoom_LowerY()
    {

        if (Input.mouseScrollDelta.y > 0)
            followOffset.y -= zoomAmout;

        if (Input.mouseScrollDelta.y < 0)
            followOffset.y += zoomAmout;

        followOffset.y = Mathf.Clamp(followOffset.y, followOffsetMinY, followOffsetMaxY);


        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
            Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset,
            zoomSpeed * Time.deltaTime);
    }

}
