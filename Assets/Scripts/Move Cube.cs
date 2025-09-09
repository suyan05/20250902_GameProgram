using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 lastPos;

    private bool isRight;

    public float maxMovingDistance;
    public float movingSpeed;

    public bool RL = true;

    public GameObject playerOnPlatform;

    private void Start()
    {
        startPos = transform.position;
        lastPos = transform.position;
        isRight = true;
    }

    private void Update()
    {
        Moving();
        MovePlayerWithPlatform();
        lastPos = transform.position;
    }

    void Moving()
    {
        Vector3 newPos = transform.position;

        if (RL)
        {
            if (isRight)
            {
                newPos.x -= movingSpeed;
                if (startPos.x - newPos.x >= maxMovingDistance)
                    isRight = false;
            }
            else
            {
                newPos.x += movingSpeed;
                if (startPos.x <= newPos.x)
                    isRight = true;
            }
        }
        else
        {
            if (isRight)
            {
                newPos.z -= movingSpeed;
                if (startPos.z - newPos.z >= maxMovingDistance)
                    isRight = false;
            }
            else
            {
                newPos.z += movingSpeed;
                if (startPos.z <= newPos.z)
                    isRight = true;
            }
        }

        transform.position = newPos;
    }

    void MovePlayerWithPlatform()
    {
        if (playerOnPlatform != null)
        {
            Vector3 delta = transform.position - lastPos;
            playerOnPlatform.transform.position += delta;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = null;
        }
    }
}