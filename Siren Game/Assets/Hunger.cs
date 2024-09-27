using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hunger : MonoBehaviour
{
    public double hungerMax = 60.0f;
    double hunger;
    public double hungerDrainPerSecond = 1.0f;
    double hungerDrainPerFrame;
    // Start is called before the first frame update
    void Start()
    {
        hungerDrainPerFrame = hungerDrainPerSecond / 60.0f;
        hunger = hungerMax;
    }

    // Update is called once per frame
    void Update()
    {
        hunger -= hungerDrainPerFrame;
    }
}
