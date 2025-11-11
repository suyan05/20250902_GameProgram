using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float walkSpeed = 5f;
    public float runSpeed = 11f;
    public float jumpPower = 4.5f;
    public float gravity = 9.8f;

    [Header("체력")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI")]
    public Slider hpSlider;
    public Image crosshairImage;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isRunning;

    [Header("카메라")]
    public CinemachineVirtualCamera thirdPersonCam;
    public CinemachineVirtualCamera firstPersonCam;
    public CinemachineFreeLook freeLookCam;
    public float rotateSpeed = 10f;

    private CinemachinePOV thirdPersonPOV;
    private CinemachinePOV firstPersonPOV;

    [Header("카메라 스위처")]
    public CinemachineSwitcher cameraSwitcher;


    private PlayerHarvester harvester;
    private PlayerShooting shooter;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        thirdPersonPOV = thirdPersonCam.GetCinemachineComponent<CinemachinePOV>();
        firstPersonPOV = firstPersonCam.GetCinemachineComponent<CinemachinePOV>();


        harvester = GetComponent<PlayerHarvester>();
        shooter = GetComponent<PlayerShooting>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentHealth = maxHealth;
        hpSlider.value = 1f;
    }

    void Update()
    {
        if (crosshairImage != null) crosshairImage.enabled = cameraSwitcher.currentMode == CinemachineSwitcher.CameraMode.FirstPerson;

        if (harvester != null) harvester.enabled = cameraSwitcher.currentMode == CinemachineSwitcher.CameraMode.FirstPerson;
        if (shooter != null) shooter.enabled = cameraSwitcher.currentMode == CinemachineSwitcher.CameraMode.ThirdPerson;

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 moveDirection = Vector3.zero;
        float cameraYaw = 0f;

        switch (cameraSwitcher.currentMode)
        {
            case CinemachineSwitcher.CameraMode.ThirdPerson:
                moveDirection = GetCameraRelativeMove(thirdPersonCam.transform, h, v);
                cameraYaw = thirdPersonPOV.m_HorizontalAxis.Value;
                SetPOVSpeed(thirdPersonPOV);
                break;

            case CinemachineSwitcher.CameraMode.FreeLook:
                moveDirection = GetCameraRelativeMove(freeLookCam.transform, h, v);
                break;

            case CinemachineSwitcher.CameraMode.FirstPerson:
                moveDirection = GetCameraRelativeMove(firstPersonCam.transform, h, v);
                cameraYaw = firstPersonPOV.m_HorizontalAxis.Value;
                SetPOVSpeed(firstPersonPOV);
                break;
        }

        Vector3 movement = moveDirection * currentSpeed;

        if (cameraSwitcher.currentMode != CinemachineSwitcher.CameraMode.FreeLook)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, cameraYaw, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpPower;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        controller.Move((movement + velocity) * Time.deltaTime);
    }

    private void SetPOVSpeed(CinemachinePOV pov)
    {
        float speed = isRunning ? 300f : 150f;
        pov.m_HorizontalAxis.m_MaxSpeed = speed;
        pov.m_VerticalAxis.m_MaxSpeed = speed;
    }

    private Vector3 GetCameraRelativeMove(Transform camTransform, float h, float v)
    {
        Vector3 forward = camTransform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = camTransform.right;
        right.y = 0;
        right.Normalize();

        return (forward * v + right * h).normalized;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        hpSlider.value = currentHealth / maxHealth;
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        controller.enabled = false;
        GameManager.Instance.RestartScene(3f);
        Destroy(gameObject);
    }
}


//내가 만든 코드
/*public float Speed = 5f;
public float JumpPower = 4.5f;
public float Gravity = 9.8f;

private CharacterController controller;
private float verticalVelocity = 0f;

public Transform cameraTransform;

void Start()
{
    controller = GetComponent<CharacterController>();
    Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 고정
}

void Update()
{
    float h = Input.GetAxis("Horizontal");
    float v = Input.GetAxis("Vertical");

    Vector3 inputDir = new Vector3(h, 0, v);
    inputDir = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * inputDir; // 카메라 방향 기준 이동

    if (controller.isGrounded)
    {
        verticalVelocity = -1f;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            verticalVelocity = JumpPower;
        }
    }
    else
    {
        verticalVelocity -= Gravity * Time.deltaTime;
    }

    Vector3 moveDir = inputDir.normalized * Speed;
    moveDir.y = verticalVelocity;

    controller.Move(moveDir * Time.deltaTime);

    // 플레이어가 카메라 방향으로 회전
    if (inputDir.magnitude > 0.1f)
    {
        Quaternion targetRotation = Quaternion.LookRotation(inputDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }
}*/