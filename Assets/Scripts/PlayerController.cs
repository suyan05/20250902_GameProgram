using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 11f;
    public float jumpPower = 4.5f;
    public float gravity = 9.8f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isRunning;

    [Header("ī�޶�")]
    public CinemachineVirtualCamera virtualCamera;
    public float rotateSpeed = 10f;
    private CinemachinePOV pov;

    [Header("ī�޶� ����ó")]
    public CinemacineSwitcher cameraSwitcher;

    void Start()
    {
        pov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // FreeLook ����� �� �÷��̾� �Է� ����
        if (cameraSwitcher != null && cameraSwitcher.usingFreeLook)
        {
            velocity.y -= gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
            return;
        }

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // �޸��� �Է� ó��
        isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // ī�޶� ���� ��� �̵�
        Vector3 camForward = virtualCamera.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = virtualCamera.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 move = (camForward * v + camRight * h).normalized;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // ĳ���� ȸ��
        float cameraYaw = pov.m_HorizontalAxis.Value;
        Quaternion targetRotation = Quaternion.Euler(0f, cameraYaw, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        // ����
        if (isGrounded)
        {
            Debug.Log("Grounded");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Jumped");
                velocity.y = jumpPower;
            }
        }
        else
        {
            Debug.Log("Not Grounded");
            velocity.y -= gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);

        // POV FOV ���� (�޸��� ȿ��)
        pov.m_VerticalAxis.m_MaxSpeed = isRunning ? 300f : 150f;
        pov.m_HorizontalAxis.m_MaxSpeed = isRunning ? 300f : 150f;
    }
}


    //���� ���� �ڵ�
    /*public float Speed = 5f;
    public float JumpPower = 4.5f;
    public float Gravity = 9.8f;

    private CharacterController controller;
    private float verticalVelocity = 0f;

    public Transform cameraTransform;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ�� ����
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(h, 0, v);
        inputDir = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * inputDir; // ī�޶� ���� ���� �̵�

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

        // �÷��̾ ī�޶� �������� ȸ��
        if (inputDir.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }*/