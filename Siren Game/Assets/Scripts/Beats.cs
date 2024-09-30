using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Beats : MonoBehaviour
{
    List<(float, int, GameObject)> timings = null;
    float offset = 0f;
    public GameObject noteBasis;
    public GameObject staffObject;

    const float noteStart = 0.5f;
    const float staffPos = -0.3f;
    // Start is called before the first frame update
    void Start()
    {
        timings = new List<(float, int, GameObject)> ();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < timings.Count; i++)
        {
            timings[i] = (timings[i].Item1 - Time.deltaTime, timings[i].Item2, timings[i].Item3);
            if (timings[i].Item1 < 1.0f)
            {
                timings[i].Item3.SetActive(true);
                timings[i].Item3.transform.localPosition = new Vector3(staffPos + timings[i].Item1 * (noteStart - staffPos), 0, 0);
            }
        }
        for (int i = timings.Count-1; i >= 0; i--)
        {
            if (timings[i].Item1 < -1.0f)
            {
                Object.Destroy(timings[i].Item3);
                timings.RemoveAt(i);
            }
        }
        if (Input.GetKey(KeyCode.Space)) //DEBUG FUNCTIONALITY: PRESSING SPACE CUES A NOTE IN 3 BEATS AT 90 BPM
        {
            queueBeat(3, 90, 0);
        }
    }

    void queueBeat(float beatCount, float bpm, int noteType)
    { //beats per minute times minutes = beats; beatCount/bpm * 60
        timings.Add((
            (beatCount / bpm * 60) - offset,
            noteType,
            Object.Instantiate(noteBasis, staffObject.transform)
        ));
    }
}
