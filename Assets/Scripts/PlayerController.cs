using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;
    public float JumpPower = 4.5f;
    public float Gravity = 9.8f;

    private CharacterController controller;

    [Header("ī�޶�")]
    public CinemachineVirtualCamera virtualCamera;
    public float rotateSpeed = 10f;
    private CinemachinePOV Pov;
    private Vector3 velocity;
    private bool isGrounded;
    void Start()
    {
        Pov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ�� ����
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // ���� ����� �� �ణ�� ���� ���� �༭ ���������� ���� �ٵ��� ��
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //ī�޶� �⺻ ����

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
}