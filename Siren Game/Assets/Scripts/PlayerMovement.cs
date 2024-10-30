//using System;
//using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerMovement : MonoBehaviour
{
    float moveSpeed;
    float rotationSpeed;
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;
    GameObject[] obstacles;
    HashSet<GameObject> trackedObjs;
    GameObject pointer;
    List<GameObject> pointerClones;
    public float pointerDist;
    // Start is called before the first frame update
    void Start()
    {

        moveSpeed = 6.0f;
        rotationSpeed = 500.0f;

        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        trackedObjs = new HashSet<GameObject>();
        pointer = GameObject.Find("pointer");
        pointerClones = new List<GameObject>();
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

        transform.position = new Vector3(
            Mathf.Max(minX, Mathf.Min(transform.position.x, maxX)),
            Mathf.Max(minY, Mathf.Min(transform.position.y, maxY)),
        0
        );


        foreach (GameObject obstacle in  obstacles)
        {
            BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
            BoxCollider2D obstacleCollider = obstacle.GetComponent<BoxCollider2D>();
            if (collider.bounds.Intersects(obstacleCollider.bounds))
            {
                Vector3 axis1 = new Vector3(-Mathf.Sin(transform.rotation.eulerAngles.z), Mathf.Cos(transform.rotation.eulerAngles.z), 0);
                Vector3 axis2 = new Vector3(Mathf.Cos(transform.rotation.eulerAngles.z), Mathf.Sin(transform.rotation.eulerAngles.z), 0);
                Vector3 axis3 = new Vector3(Mathf.Cos(obstacle.transform.rotation.eulerAngles.z), Mathf.Sin(obstacle.transform.rotation.eulerAngles.z), 0);
                Vector3 axis4 = new Vector3(-Mathf.Sin(obstacle.transform.rotation.eulerAngles.z), Mathf.Cos(obstacle.transform.rotation.eulerAngles.z), 0);

                Vector3[] axes = { axis1, axis2, axis3, axis4 };

                Vector3 corner11 = transform.TransformPoint(new Vector3(collider.size.x / 2, collider.size.y / 2, 0));
                Vector3 corner12 = transform.TransformPoint(new Vector3(-collider.size.x / 2, collider.size.y / 2, 0));
                Vector3 corner13 = transform.TransformPoint(new Vector3(collider.size.x / 2, -collider.size.y / 2, 0));
                Vector3 corner14 = transform.TransformPoint(new Vector3(-collider.size.x / 2, -collider.size.y / 2, 0));

                Vector3[] corner1 = { corner11, corner12, corner13, corner14 };

                Vector3 corner21 = obstacle.transform.TransformPoint(new Vector3(obstacleCollider.size.x / 2, obstacleCollider.size.y / 2, 0));
                Vector3 corner22 = obstacle.transform.TransformPoint(new Vector3(-obstacleCollider.size.x / 2, obstacleCollider.size.y / 2, 0));
                Vector3 corner23 = obstacle.transform.TransformPoint(new Vector3(obstacleCollider.size.x / 2, -obstacleCollider.size.y / 2, 0));
                Vector3 corner24 = obstacle.transform.TransformPoint(new Vector3(-obstacleCollider.size.x / 2, -obstacleCollider.size.y / 2, 0));

                Vector3[] corner2 = { corner21, corner22, corner23, corner24 };

                float minimumPenetration = float.PositiveInfinity;
                Vector3 penetrationAxis = Vector3.zero;
                foreach (Vector3 axis in axes)
                {
                    float obj1Minimum = float.PositiveInfinity;
                    float obj1Maximum = float.NegativeInfinity;
                    float obj2Minimum = float.PositiveInfinity;
                    float obj2Maximum = float.NegativeInfinity;

                    foreach (Vector3 corner in corner1)
                    {
                        float proj = Vector3.Dot(corner, axis);
                        obj1Maximum = Mathf.Max(obj1Maximum, proj);
                        obj1Minimum = Mathf.Min(obj1Minimum, proj);
                    }

                    foreach (Vector3 corner in corner2)
                    {
                        float proj = Vector3.Dot(corner, axis);
                        obj2Maximum = Mathf.Max(obj2Maximum, proj);
                        obj2Minimum = Mathf.Min(obj2Minimum, proj);
                    }

                    //if either of the following two is true, there is a separating axis
                    if (obj1Minimum > obj2Maximum)
                    {
                        minimumPenetration = float.PositiveInfinity;
                        penetrationAxis = Vector3.zero;
                        break;
                    }
                    if (obj2Minimum > obj1Maximum)
                    {
                        minimumPenetration = float.PositiveInfinity;
                        penetrationAxis = Vector3.zero;
                        break;
                    }

                    float penetration;
                    //there is a collision. determine how deep the collision is. find the smallest collision
                    if (obj1Maximum + obj1Minimum > obj2Maximum + obj2Minimum)
                    {
                        //the middle of obj1 is further along the axis than obj2
                        penetration = obj2Maximum - obj1Minimum;
                    }
                    else
                    {
                        //the middle of obj2 is further along the axis than obj1
                        penetration = obj2Minimum - obj1Maximum; //note: it is negative. this is intentional.
                    }
                    if (Mathf.Abs(penetration) < Mathf.Abs(minimumPenetration))
                    {
                        minimumPenetration = penetration;
                        penetrationAxis = axis;
                    }
                }
                //check collision
                if (!float.IsInfinity(minimumPenetration))
                {
                    //process collision
                    transform.position += penetrationAxis * minimumPenetration;
                    Debug.Log(minimumPenetration);
                    Debug.Log(penetrationAxis);
                }
            }
        }
        
        HashSet<GameObject> toRemove = new HashSet<GameObject>();
        int objNum = 0;

        foreach (GameObject pointerClone in pointerClones) {
            pointerClone.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
        foreach (GameObject obj in trackedObjs)
        {
            Debug.Log("ship");
            float totalRadii = gameObject.GetComponent<CircleCollider2D>().radius + obj.GetComponent<CircleCollider2D>().radius;
            float dist = Vector3.Distance(gameObject.transform.position, obj.transform.position);

            
            if (totalRadii < dist)
            {
                toRemove.Add(obj);
            }
            else
            {
                if (pointerClones.Count <= objNum)
                {
                    pointerClones.Add(Object.Instantiate(pointer, transform));
                }
                float factor = 1 - dist / totalRadii;
                pointerClones[objNum].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, factor);
                pointerClones[objNum].transform.rotation = Quaternion.LookRotation(Vector3.forward, obj.transform.position - gameObject.transform.position);
                pointerClones[objNum].transform.position = transform.position + Vector3.Normalize(obj.transform.position - gameObject.transform.position) * pointerDist;
                objNum++;
            }
        }

        foreach (GameObject obj in toRemove)
        {
            trackedObjs.Remove(obj);
        }
    }

    public void Alert(GameObject obj)
    {
        trackedObjs.Add(obj);
    }


}
