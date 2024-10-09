using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float moveSpeed;
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;

    // Start is called before the first frame update
    void Start()
    {

        moveSpeed = 6.0f;


    }

    // Update is called once per frame
    void Update()
    {
        // Up/Down Movement
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.up * -moveSpeed * Time.deltaTime;
        }

        // Left/Right Movement
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.right * -moveSpeed * Time.deltaTime;

        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }


        transform.position = new Vector3(
            Mathf.Max(minX, Mathf.Min(transform.position.x, maxX)),
            Mathf.Max(minY, Mathf.Min(transform.position.y, maxY)),
            0
        );
    }


}
