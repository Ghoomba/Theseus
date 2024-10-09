using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OWCameraMovement : MonoBehaviour
{
    public GameObject player;
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(
            Mathf.Max(minX, Mathf.Min(player.transform.position.x, maxX)),
            Mathf.Max(minY, Mathf.Min(player.transform.position.y, maxY)),
            -10
        );
    }
}
