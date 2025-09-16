using UnityEngine;
using Cinemachine;

public class CinemacineSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCam;
    public CinemachineFreeLook freeLookCam;
    public bool usingFreeLook = false;

    private void Start()
    {
        virtualCam.Priority = 10;
        freeLookCam.Priority = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            usingFreeLook = !usingFreeLook;
            if (usingFreeLook)
            {
                virtualCam.Priority = 0;
                freeLookCam.Priority = 20;
            }
            else
            {
                virtualCam.Priority = 20;
                freeLookCam.Priority = 0;
            }
        }
    }
}