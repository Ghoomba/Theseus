//using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //public double maxAccel = 2.0f;
    //public double maxVelocity = 3.0f;
    public GameObject target;
    //public Vector3 initialVelocity = Vector3.zero;
    //public Vector3 acceleration = Vector3.zero;
    // Start is called before the first frame update
    GameObject[] obstacles;
    void Start()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        //target = GameObject.Find("Siren");
    }

    // Update is called once per frame
    void Update()
    {
        /*Vector3 targetPosition = target.transform.position;
        Vector3 pos = this.gameObject.transform.position;
        Vector3 targetPosRelative = targetPosition - pos;
        Vector3 targetPosBisectorDir = new Vector3(targetPosRelative.y, -targetPosRelative.x, targetPosRelative.z);
        Vector3 velocityDirection = initialVelocity.normalized;
        Vector3 velocityPerp = new Vector3(velocityDirection.y, -velocityDirection.x, velocityDirection.z);*/
        //perpendicular bisector is tPR/2 + targetPosBisectorDir_b
        //when do tPR/2 + targetPosBisectorDir*t and velocityPerp*u intersect

        float oldX = transform.position.x;
        float oldY = transform.position.y;
        foreach (GameObject obstacle in obstacles)
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

                Vector3[] corner1 = {corner11, corner12, corner13, corner14};

                Vector3 corner21 = obstacle.transform.TransformPoint(new Vector3(obstacleCollider.size.x / 2, obstacleCollider.size.y / 2, 0));
                Vector3 corner22 = obstacle.transform.TransformPoint(new Vector3(-obstacleCollider.size.x / 2, obstacleCollider.size.y / 2, 0));
                Vector3 corner23 = obstacle.transform.TransformPoint(new Vector3(obstacleCollider.size.x / 2, -obstacleCollider.size.y / 2, 0));
                Vector3 corner24 = obstacle.transform.TransformPoint(new Vector3(-obstacleCollider.size.x / 2, -obstacleCollider.size.y / 2, 0));

                Vector3[] corner2 = {corner21, corner22, corner23, corner24};

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


        float totalRadii = gameObject.GetComponent<CircleCollider2D>().radius * transform.lossyScale.magnitude / Mathf.Sqrt(3) + target.GetComponent<CircleCollider2D>().radius * target.transform.lossyScale.magnitude / Mathf.Sqrt(3);
        float dist = Vector3.Distance(gameObject.transform.position, target.transform.position);
        //ok heres the actual collision part
        if (dist < totalRadii)
        {
            target.GetComponent<PlayerMovement>().Alert(gameObject);
        }
        if (gameObject.GetComponent<BoxCollider2D>().bounds.Intersects(target.GetComponent<BoxCollider2D>().bounds))
        {
            Manager.Instance.enterBattle();
        }

        
    }
}
