using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public float speed;
    float rotationSpeed;
    Vector2 originalPosition;
    GameObject player;

    [SerializeField] Transform[] Points;

    private float distance;
    private int pointsIndex;

    // Start is called before the first frame update
    void Start()
    {
        //speed = 1.0f;
        rotationSpeed = 80.0f;
        transform.position = Points[0].transform.position;
        player = GameObject.Find("Siren");

        pointsIndex = 0;

        //originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Points.Length == 0)
        {

        }
        else
        {

            if (pointsIndex <= Points.Length - 1)
            {

                distance = Vector2.Distance(transform.position, Points[pointsIndex].transform.position);

                Vector2 direction = Points[pointsIndex].transform.position - transform.position;
                direction.Normalize();

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                angle += -90f;

                transform.position = Vector2.MoveTowards(this.transform.position, Points[pointsIndex].transform.position, speed * Time.deltaTime);

                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


                if (transform.position == Points[pointsIndex].transform.position)
                {
                    pointsIndex += 1;
                }

                if (pointsIndex == Points.Length)
                {
                    pointsIndex = 0;
                }
            }
        }

    }
}
