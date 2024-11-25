using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBtn : MonoBehaviour
{
    // Start is called before the first frame update

    public Manager.State state;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider == gameObject.GetComponent<BoxCollider2D>())
            {
                Manager.Instance.btnState(state);
            }
        }
    }
}
