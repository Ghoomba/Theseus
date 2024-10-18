using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Beats : MonoBehaviour
{
    List<(float, int, GameObject, bool)> timings = null;
    float offset = 0f;
    public GameObject noteBasis;
    public GameObject staffObject;
    public GameObject hungerMeterScriptObject;
    float bpm = 60f;
    int fails = 0;
    const int FAILS_LIMIT = 2;

    const float TIMING_LENIENCE = 1.0f / 4.0f;

    const float noteStart = 0.5f;
    const float staffPos = -0.5f/3;
    // Start is called before the first frame update
    void Start()
    {
        timings = new List<(float, int, GameObject, bool)> ();

        switch(Manager.Instance.song)
        {
            case Manager.Songs.Test:
                bpm = 162f;
                queueBeat(3, bpm, 1);
                queueBeat(4, bpm, 1);
                queueBeat(5, bpm, 1);
                queueBeat(6, bpm, 1);
                queueBeat(6.5f, bpm, 4);
                queueBeat(7.5f, bpm, 4);
                queueBeat(8.5f, bpm, 4);
                queueBeat(9.5f, bpm, 4);
                queueBeat(10.5f, bpm, 4);
                queueBeat(11.5f, bpm, 4);
                queueBeat(12.5f, bpm, 4);
                queueBeat(13f, bpm, 1);
                queueBeat(14f, bpm, 1);
                queueBeat(15f, bpm, -1);
                break;
            case Manager.Songs.Test2:
                bpm = 162f;
                queueBeat(3, bpm, 2);
                queueBeat(3 + 2f / 3, bpm, 2);
                queueBeat(5, bpm, 2);
                queueBeat(5 + 2f / 3, bpm, 2);
                queueBeat(7, bpm, 3);
                queueBeat(7 + 2f / 3, bpm, 3);
                queueBeat(7 + 4f / 3, bpm, 3);
                queueBeat(9, bpm, 3);
                queueBeat(9 + 2f / 3, bpm, 3);
                queueBeat(9 + 4f / 3, bpm, 3);
                queueBeat(11, bpm, 2);
                queueBeat(11 + 2f / 3, bpm, 2);
                queueBeat(13, bpm, 2);
                queueBeat(13 + 2f / 3, bpm, 2);
                queueBeat(15f, bpm, -1);
                break;
            default:
                bpm = 162f;
                queueBeat(1f, bpm, -1);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float hit = float.NaN;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            hit = float.PositiveInfinity;
            for (int i = 0; i < timings.Count; i++)
            {
                if (!timings[i].Item4)
                {
                    if (Mathf.Abs(timings[i].Item1) < 60 / bpm * TIMING_LENIENCE) //if right within a quarter beat
                    {
                        switch (timings[i].Item2)
                        {
                            case 0:
                                if (!Input.GetKeyDown(KeyCode.Space))
                                { continue; }
                                break;
                            case 1:
                                if (!(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)))
                                { continue; }
                                break;
                            case 2:
                                if (!(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Space)))
                                { continue; }
                                break;
                            case 3:
                                if (!(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Space)))
                                { continue; }
                                break;
                            case 4:
                                if (!(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Space)))
                                { continue; }
                                break;
                        }
                        hit = timings[i].Item1; //set hit to the offset
                        timings[i] = (timings[i].Item1, timings[i].Item2, timings[i].Item3, true);
                        break;
                    }
                    if (timings[i].Item1 > 60 / bpm * TIMING_LENIENCE) //if there are no beats yet and we've already reached a quarter beat in the future, we've missed
                    {
                        break;
                    }
                }
            }
            Debug.Log(hit.ToString());
        } //if float is NaN there's nothing pressed; if float is PositiveInfinity it's a miss.

        for (int i = 0; i < timings.Count; i++)
        {
            if (!timings[i].Item4)
            {
                timings[i] = (timings[i].Item1 - Time.deltaTime, timings[i].Item2, timings[i].Item3, timings[i].Item4);
                if (timings[i].Item1 < 120 / bpm)
                {
                    switch (timings[i].Item2)
                    {
                        case 0:
                            timings[i].Item3.SetActive(true);
                            timings[i].Item3.transform.localPosition = new Vector3(staffPos + (timings[i].Item1 / 60 * bpm / 2) * (noteStart - staffPos), 0, 0);
                            break;
                        case 1:
                            timings[i].Item3.SetActive(true);
                            timings[i].Item3.transform.localPosition = new Vector3(staffPos + (timings[i].Item1 / 60 * bpm / 2) * (noteStart - staffPos), 0.375f, 0);
                            timings[i].Item3.transform.localScale = new Vector3(timings[i].Item3.transform.localScale.x, 0.25f, 1);
                            break;
                        case 2:
                            timings[i].Item3.SetActive(true);
                            timings[i].Item3.transform.localPosition = new Vector3(staffPos + (timings[i].Item1 / 60 * bpm / 2) * (noteStart - staffPos), 0.125f, 0);
                            timings[i].Item3.transform.localScale = new Vector3(timings[i].Item3.transform.localScale.x, 0.25f, 1);
                            break;
                        case 3:
                            timings[i].Item3.SetActive(true);
                            timings[i].Item3.transform.localPosition = new Vector3(staffPos + (timings[i].Item1 / 60 * bpm / 2) * (noteStart - staffPos), -0.125f, 0);
                            timings[i].Item3.transform.localScale = new Vector3(timings[i].Item3.transform.localScale.x, 0.25f, 1);
                            break;
                        case 4:
                            timings[i].Item3.SetActive(true);
                            timings[i].Item3.transform.localPosition = new Vector3(staffPos + (timings[i].Item1 / 60 * bpm / 2) * (noteStart - staffPos), -0.375f, 0);
                            timings[i].Item3.transform.localScale = new Vector3(timings[i].Item3.transform.localScale.x, 0.25f, 1);
                            break;
                    }
                    if (timings[i].Item1 < -60 / bpm * TIMING_LENIENCE)
                    {
                        timings[i].Item3.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 50f / 255f);
                        if (fails >= FAILS_LIMIT)
                        {
                            Manager.Instance.exitBattle(0.0f);
                        }
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
        }
        for (int i = timings.Count-1; i >= 0; i--)
        {
            if (timings[i].Item1 < -60/bpm)
            {
                Object.Destroy(timings[i].Item3);
                timings.RemoveAt(i);
                fails += 1;
            }
            if (timings[i].Item4)
            {
                timings[i].Item3.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, timings[i].Item3.GetComponent<SpriteRenderer>().color.a - Time.deltaTime * 100f / 255f);
                if (timings[i].Item3.GetComponent<SpriteRenderer>().color.a < 0)
                {
                    Object.Destroy(timings[i].Item3);
                    timings.RemoveAt(i);
                }
            }
        }
    }

    public void queueBeat(float beatCount, float bpm, int noteType)
    { //beats per minute times minutes = beats; beatCount/bpm * 60
        timings.Add((
            (beatCount / bpm * 60) - offset,
            noteType,
            Object.Instantiate(noteBasis, staffObject.transform),
            false
        ));
    }
}
