using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;
    public float JP = 4.5f;
    public float Gv = 9.8f;
    public float RS = 4.0f;

    private float h, v;
    private Vector3 moveDir;
    private CharacterController CC;
    private float verticalVelocity = 0f;

    private void Start()
    {
        CC = GetComponent<CharacterController>();
    }

    private void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(h, 0, v);
        inputDir = transform.TransformDirection(inputDir);

        if (CC.isGrounded)
        {
            verticalVelocity = -1f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = JP;
            }
        }
        else
        {
            verticalVelocity -= Gv * Time.deltaTime; // 중력 적용
        }

        moveDir = inputDir * Speed;
        moveDir.y = verticalVelocity;

        CC.Move(moveDir * Time.deltaTime);

        // 회전 처리
        if (inputDir.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * RS);
        }
    }
}