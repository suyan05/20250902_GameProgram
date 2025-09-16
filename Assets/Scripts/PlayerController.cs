using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;
    public float JumpPower = 4.5f;
    public float Gravity = 9.8f;

    private CharacterController controller;

    [Header("카메라")]
    public CinemachineVirtualCamera virtualCamera;
    public float rotateSpeed = 10f;
    private CinemachinePOV Pov;
    private Vector3 velocity;
    private bool isGrounded;
    void Start()
    {
        Pov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 고정
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 땅에 닿았을 때 약간의 음수 값을 줘서 안정적으로 땅에 붙도록 함
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //카메라 기본 설정

        Vector3 camForward = virtualCamera.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = virtualCamera.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 move = (camForward * v + camRight * h).normalized;
        controller.Move(move * Speed * Time.deltaTime);

        float cameraYaw = Pov.m_HorizontalAxis.Value;
        Quaternion targetRotation = Quaternion.Euler(0f, cameraYaw, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = JumpPower;
        }
        else
        {
            velocity.y -= Gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
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
}