using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hunger : MonoBehaviour
{
    public float hungerMax = 60.0f;
    float hunger;
    public float hungerDrainPerSecond = 1.0f;
    public GameObject meter;
    // Start is called before the first frame update
    void Start()
    {
        hunger = hungerMax;
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
            //todo: implement failure
            hunger = 0;
        }
    }

    void RestoreHunger(float food)
    {
        hunger += food;
        hunger = Mathf.Min(hunger, hungerMax);
    }
}
