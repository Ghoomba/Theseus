using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    float moveSpeed;
    float rotationSpeed;
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;
    GameObject[] obstacles;
    // Start is called before the first frame update
    void Start()
    {

        moveSpeed = 6.0f;
        rotationSpeed = 500.0f;

        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
    }

    // Update is called once per frame
    void Update()
    {
        float oldX = transform.position.x;
        float oldY = transform.position.y;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movementDirection = new Vector2(horizontalInput, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
        movementDirection.Normalize();

        transform.Translate(movementDirection * moveSpeed * inputMagnitude * Time.deltaTime, Space.World);

        if (movementDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        

        foreach (GameObject obstacle in  obstacles)
        {
            BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
            BoxCollider2D obstacleCollider = obstacle.GetComponent<BoxCollider2D>();
            if (collider.bounds.Intersects(obstacleCollider.bounds))
            {
                //process collision
                Vector3 closestPoint = obstacleCollider.ClosestPoint(transform.position);
                if (Mathf.Abs(transform.position.x - closestPoint.x) > Mathf.Abs(transform.position.y - closestPoint.y))
                {
                    transform.position = new Vector3(oldX, transform.position.y, transform.position.z);
                    if (transform.position.x > obstacle.transform.position.x)
                    {
                        transform.position = new Vector3(obstacle.transform.position.x + obstacle.GetComponent<BoxCollider2D>().bounds.extents.x + gameObject.GetComponent<BoxCollider2D>().bounds.extents.x + .001f, transform.position.y, transform.position.z);
                    }
                    if (transform.position.x < obstacle.transform.position.x)
                    {
                        transform.position = new Vector3(obstacle.transform.position.x - obstacle.GetComponent<BoxCollider2D>().bounds.extents.x - gameObject.GetComponent<BoxCollider2D>().bounds.extents.x - .001f, transform.position.y, transform.position.z);
                    }
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, oldY, transform.position.z);
                    if (transform.position.y > obstacle.transform.position.y)
                    {
                        transform.position = new Vector3(transform.position.x, obstacle.transform.position.y + obstacle.GetComponent<BoxCollider2D>().bounds.extents.y + gameObject.GetComponent<BoxCollider2D>().bounds.extents.y + .001f, transform.position.z);
                    }
                    if (transform.position.y < obstacle.transform.position.y)
                    {
                        transform.position = new Vector3(transform.position.x, obstacle.transform.position.y - obstacle.GetComponent<BoxCollider2D>().bounds.extents.y - gameObject.GetComponent<BoxCollider2D>().bounds.extents.y - .001f, transform.position.z);
                    }
                }
            }
        }
    }


}
