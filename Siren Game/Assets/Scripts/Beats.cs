using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Beats : MonoBehaviour
{
    List<(float, int, GameObject)> timings = null;
    float offset = 0f;
    public GameObject noteBasis;
    public GameObject staffObject;
    float bpm = 60f;

    const float noteStart = 0.5f;
    const float staffPos = -0.5f/3;
    // Start is called before the first frame update
    void Start()
    {
        timings = new List<(float, int, GameObject)> ();

        switch(Manager.Instance.song)
        {
            case Manager.Songs.Test:
                bpm = 162f;
                queueBeat(3, bpm, 0);
                queueBeat(4, bpm, 0);
                queueBeat(5, bpm, 0);
                queueBeat(6, bpm, 0);
                queueBeat(6.5f, bpm, 0);
                queueBeat(7.5f, bpm, 0);
                queueBeat(8.5f, bpm, 0);
                queueBeat(9.5f, bpm, 0);
                queueBeat(10.5f, bpm, 0);
                queueBeat(11.5f, bpm, 0);
                queueBeat(12.5f, bpm, 0);
                queueBeat(13f, bpm, 0);
                queueBeat(14f, bpm, 0);
                queueBeat(15f, bpm, -1);
                break;
            case Manager.Songs.Test2:
                bpm = 162f;
                queueBeat(3, bpm, 0);
                queueBeat(3 + 2f / 3, bpm, 0);
                queueBeat(5, bpm, 0);
                queueBeat(5 + 2f / 3, bpm, 0);
                queueBeat(7, bpm, 0);
                queueBeat(7 + 2f / 3, bpm, 0);
                queueBeat(7 + 4f / 3, bpm, 0);
                queueBeat(9, bpm, 0);
                queueBeat(9 + 2f / 3, bpm, 0);
                queueBeat(9 + 4f / 3, bpm, 0);
                queueBeat(11, bpm, 0);
                queueBeat(11 + 2f / 3, bpm, 0);
                queueBeat(13, bpm, 0);
                queueBeat(13 + 2f / 3, bpm, 0);
                queueBeat(15f, bpm, -1);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float hit = float.NaN;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < timings.Count; i++)
            {
                if (Mathf.Abs(timings[i].Item1) < 15 / bpm) //if right within a quarter beat
                {
                    hit = timings[i].Item1; //set hit to the offset
                    Object.Destroy(timings[i].Item3);
                    timings.RemoveAt(i);
                    break;
                }
                if (timings[i].Item1 > 15 / bpm) //if there are no beats yet and we've already reached a quarter beat in the future, we've missed
                {
                    break;
                }
            }
        } //if float is NaN it's a miss

        for (int i = 0; i < timings.Count; i++)
        {
            timings[i] = (timings[i].Item1 - Time.deltaTime, timings[i].Item2, timings[i].Item3);
            if (timings[i].Item1 < 120/bpm)
            {
                if (timings[i].Item2 == 0)
                {
                    timings[i].Item3.SetActive(true);
                    timings[i].Item3.transform.localPosition = new Vector3(staffPos + (timings[i].Item1 / 60 * bpm / 2) * (noteStart - staffPos), 0, 0);
                }
            }
            if (timings[i].Item2 == -1)
            {
                if (timings[i].Item1 < 0)
                {
                    Manager.Instance.exitBattle(10.0f);
                }
            }
        }
        for (int i = timings.Count-1; i >= 0; i--)
        {
            if (timings[i].Item1 < -60/bpm)
            {
                Object.Destroy(timings[i].Item3);
                timings.RemoveAt(i);
            }
        }
    }

    public void queueBeat(float beatCount, float bpm, int noteType)
    { //beats per minute times minutes = beats; beatCount/bpm * 60
        timings.Add((
            (beatCount / bpm * 60) - offset,
            noteType,
            Object.Instantiate(noteBasis, staffObject.transform)
        ));
    }
}
