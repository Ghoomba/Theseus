using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public double maxAccel = 2.0f;
    public double maxVelocity = 3.0f;
    public GameObject target;
    public Vector3 initialVelocity = Vector3.zero;
    public Vector3 acceleration = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = target.transform.position;
        Vector3 pos = this.gameObject.transform.position;
        Vector3 targetPosRelative = targetPosition - pos;
        Vector3 targetPosBisectorDir = new Vector3(targetPosRelative.y, -targetPosRelative.x, targetPosRelative.z);
        Vector3 velocityDirection = initialVelocity.normalized;
        Vector3 velocityPerp = new Vector3(velocityDirection.y, -velocityDirection.x, velocityDirection.z);
        //perpendicular bisector is tPR/2 + targetPosBisectorDir_b
        //when do tPR/2 + targetPosBisectorDir*t and velocityPerp*u intersect

    }
}
