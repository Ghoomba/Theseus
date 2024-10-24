using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    float speed;
    float rotationSpeed;
    Vector2 originalPosition;
    GameObject player;

    [SerializeField] Transform[] MovePoints;

    private float distance;
    private int pointsIndex;

    // Start is called before the first frame update
    void Start()
    {
        speed = 2.0f;
        rotationSpeed = 60.0f;
        transform.position = MovePoints[pointsIndex].transform.position;
        player = GameObject.Find("Siren");
    }

    // Update is called once per frame
    void Update()
    {
        //distance = Vector2.Distance(transform.position, player.transform.position);
        //
        //Vector2 direction = player.transform.position - transform.position;
        //direction.Normalize();
        //
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //
        //angle += -90f;
        //
        //
        //if (distance < 4)
        //{
        //    transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        //
        //    Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        //
        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //}
        //else
        //{
        //    transform.position = Vector2.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
        //}

        if (MovePoints.Length == 0)
        {
         
        }
        else
        {
            Debug.Log("WORKSSSSSSSSSSSSSSSSSSSSS");
            if (pointsIndex <= MovePoints.Length - 1)
            {
                this.transform.position = Vector2.MoveTowards(transform.position, MovePoints[pointsIndex].transform.position, speed * Time.deltaTime);
                Debug.Log(pointsIndex);
                if (transform.position == MovePoints[pointsIndex].transform.position)
                {
                    pointsIndex += 1;
                }

                if (pointsIndex == MovePoints.Length)
                {
                    pointsIndex = 0;
                }
            }
        }

    }
}
