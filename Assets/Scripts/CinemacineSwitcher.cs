using UnityEngine;
using Cinemachine;

public class CinemachineSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera thirdPersonCam;
    public CinemachineFreeLook freeLookCam;
    public CinemachineVirtualCamera firstPersonCam;

    public enum CameraMode { ThirdPerson, FreeLook, FirstPerson }
    public CameraMode currentMode = CameraMode.ThirdPerson;

    private void Start()
    {
        SetCameraPriority(CameraMode.ThirdPerson);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            currentMode = CameraMode.ThirdPerson;
            SetCameraPriority(currentMode);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            currentMode = CameraMode.FreeLook;
            SetCameraPriority(currentMode);
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            currentMode = CameraMode.FirstPerson;
            SetCameraPriority(currentMode);
        }
    }

    private void SetCameraPriority(CameraMode mode)
    {
        thirdPersonCam.Priority = (mode == CameraMode.ThirdPerson) ? 20 : 0;
        freeLookCam.Priority = (mode == CameraMode.FreeLook) ? 20 : 0;
        firstPersonCam.Priority = (mode == CameraMode.FirstPerson) ? 20 : 0;
    }
}