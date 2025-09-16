using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;
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
    }
}