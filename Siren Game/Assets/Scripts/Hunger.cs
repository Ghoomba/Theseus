using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hunger : MonoBehaviour
{
    public float hungerMax = 20.0f;
    float hunger;
    public float hungerDrainPerSecond = 1.0f;
    GameObject meter;
    // Start is called before the first frame update
    void Start()
    {
        meter = gameObject;
        if (Manager.Instance.hunger >= 0)
        {
            hunger = Mathf.Min(Manager.Instance.newHunger, hungerMax);
        }
        else
        {
            hunger = hungerMax;
        }

        meter = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        hunger -= hungerDrainPerSecond * Time.deltaTime;

        float hungerPercentage = hunger / hungerMax;
        meter.transform.localScale = new Vector3(hungerPercentage, 1, 1);
        meter.transform.localPosition = new Vector3((hungerPercentage - 1) / 2, 0, -1);

        if (hunger <= 0)
        {
            hunger = 0;
            Manager.Instance.lose();
        }

        Manager.Instance.hunger = hunger;
    }
}
