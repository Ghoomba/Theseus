using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

//using static TreeEditor.TreeEditorHelper;
using static UnityEngine.GraphicsBuffer;

public class Beats : MonoBehaviour
{
    List<(float, int, GameObject, bool)> timings = null;
    float offset = 0;
    public GameObject noteBasis;
    public GameObject staffObject;
    public GameObject hungerMeterScriptObject;
    private GameObject fakeShipObject;
    private GameObject sailor;
    private Animator sailorAnim;
    float bpm = 60f;
    int fails = 0;
    const int FAILS_LIMIT = 2;

    const float TIMING_LENIENCE = 1.0f / 4.0f;

    const float noteStart = 0.5f;
    const float staffPos = -0.5f / 3;

    public List<AudioClip> music;

    public AudioSource audioSource;

    float totaloffset = 0f;
    int totalhits = 0;
    int beatsinthepast = 0;

    int walkBeatAmt = 0;
    int remainingFails = 0;
    float sailorStartX = 0;

    int songLength = 0;
    float time = 0;

    Hunger hungerMeterComponent;

    public GameObject timeMeter;

    SpriteRenderer spriteRenderer;

    Vector3 scaleChange;
    Vector3 posChange;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        scaleChange = new Vector3(0.1f, 0.1f, 0.0f);
        posChange = new Vector3(-0.1f, -0.01f, 0.0f);
        fakeShipObject = GameObject.Find("FakeShip");
        hungerMeterComponent = hungerMeterScriptObject.GetComponent<Hunger>();

        sailor = GameObject.Find("sailor");
        sailorAnim = sailor.GetComponent<Animator>();
        sailorStartX = sailor.transform.localPosition.x;

        offset = PlayerPrefs.GetFloat("Offset", 0f);
        if (!float.IsNormal(offset))
        {
            offset = 0f;
        }
        timings = new List<(float, int, GameObject, bool)> ();

        switch(Manager.Instance.song)
        {
            case Manager.Songs.SoundTest:
                bpm = 120f;
                queueBeat(0f, bpm, -3);
                for (int i = 4; i < 64; i++)
                {
                    queueBeat(i, bpm, 0);
                }
                songLength = 64;
                queueBeat(64, bpm, -1);
                audioSource.clip = music[0];
                audioSource.Play();
                break;
            case Manager.Songs.Test:
                bpm = 60f;
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
                songLength = 15;
                audioSource.clip = music[0];
                audioSource.loop = true;
                audioSource.Play();
                break;
            case Manager.Songs.Test2:
                bpm = 60f;
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
                songLength = 15;
                audioSource.clip = music[0];
                audioSource.Play();
                break;
            case Manager.Songs.Incedious:
                bpm = 120f;
                queueBeat(0f, bpm, -3);
                queueBeat(4f, bpm, 4);
                queueBeat(5f, bpm, 1);
                queueBeat(5.5f, bpm, 4);
                queueBeat(6f, bpm, 4);
                queueBeat(7f, bpm, 1);
                queueBeat(7.5f, bpm, 4);
                queueBeat(8f, bpm, 4);
                queueBeat(9f, bpm, 1);
                queueBeat(9.5f, bpm, 4);
                queueBeat(10f, bpm, 4);
                queueBeat(11f, bpm, 1);
                queueBeat(11.5f, bpm, 4);
                queueBeat(12f, bpm, 4);
                queueBeat(13f, bpm, 1);
                queueBeat(13.5f, bpm, 4);
                queueBeat(14f, bpm, 4);
                queueBeat(15f, bpm, 1);
                queueBeat(15f, bpm, -4);
                queueBeat(15.5f, bpm, 4);
                queueBeat(16f, bpm, 0);

                //queueBeat(16.5f, bpm, 3);
                queueBeat(17f, bpm, 3);
                queueBeat(18f, bpm, 3);
                queueBeat(18.75f, bpm, 3);
                queueBeat(19.5f, bpm, 3);
                queueBeat(20.5f, bpm, 3);
                queueBeat(21f, bpm, 3);
                queueBeat(22f, bpm, 3);
                queueBeat(22.75f, bpm, 3);
                queueBeat(23.5f, bpm, 3);
                queueBeat(24.5f, bpm, 3);
                queueBeat(25f, bpm, 3);
                queueBeat(26f, bpm, 3);
                queueBeat(26.75f, bpm, 3);
                queueBeat(27.5f, bpm, 3);
                queueBeat(28.5f, bpm, 3);
                queueBeat(29f, bpm, 3);
                queueBeat(30f, bpm, 3);
                queueBeat(30.75f, bpm, 3);
                queueBeat(31f, bpm, -4);
                queueBeat(31.5f, bpm, 3);
                queueBeat(32f, bpm, 0);

                queueBeat(33f, bpm, 2);
                queueBeat(34f, bpm, 1);
                queueBeat(35.5f, bpm, 2);
                queueBeat(36f, bpm, 1);

                queueBeat(37f, bpm, 2);
                queueBeat(38f, bpm, 1);
                queueBeat(38.5f, bpm, 2);
                queueBeat(39.5f, bpm, 2);
                queueBeat(40f, bpm, 1);
                queueBeat(40.5f, bpm, 2);

                queueBeat(41f, bpm, 2);
                queueBeat(42f, bpm, 1);
                queueBeat(43.5f, bpm, 4);
                queueBeat(43.75f, bpm, 4);
                queueBeat(44f, bpm, 1);

                queueBeat(45.5f, bpm, 4);
                queueBeat(45.75f, bpm, 4);
                queueBeat(46f, bpm, 1);
                queueBeat(46.5f, bpm, 4);

                queueBeat(47.5f, bpm, 4);
                queueBeat(48f, bpm, 1);

                queueBeat(49.5f, bpm, 4);
                queueBeat(50f, bpm, 1);

                queueBeat(51.5f, bpm, 4);
                queueBeat(52f, bpm, 1);
                queueBeat(53f, bpm, 4);
                queueBeat(53.5f, bpm, 4);
                queueBeat(54f, bpm, 1);

                queueBeat(55.5f, bpm, 4);
                queueBeat(56f, bpm, 3);
                queueBeat(57f, bpm, 2);
                queueBeat(58f, bpm, 1);
                queueBeat(59f, bpm, 2);
                queueBeat(60f, bpm, 3);
                queueBeat(61f, bpm, 1);
                queueBeat(62f, bpm, 4);
                queueBeat(63f, bpm, 2);
                queueBeat(63f, bpm, -4);
                queueBeat(64f, bpm, 0);
                queueBeat(68f, bpm, -1);

                songLength = 68;
                audioSource.clip = music[1];
                audioSource.loop = true;
                audioSource.Play();
                break;
            case Manager.Songs.Pride:
                bpm = 190f;
                queueBeat(0f, bpm, -3);

                queueBeat(16f, bpm, 0);
                queueBeat(17f, bpm, 0);
                queueBeat(18f, bpm, 0);
                queueBeat(18.5f, bpm, 0);
                queueBeat(19.5f, bpm, 5);
                queueBeat(23f, bpm, 10);

                queueBeat(24f, bpm, 0);
                queueBeat(25f, bpm, 0);
                queueBeat(26f, bpm, 0);
                queueBeat(26.5f, bpm, 0);
                queueBeat(27.5f, bpm, 5);
                queueBeat(31f, bpm, 10);

                queueBeat(32f, bpm, 5);
                queueBeat(33f, bpm, 10);
                queueBeat(34f, bpm, 0);
                queueBeat(34f + 2f / 3, bpm, 0);
                queueBeat(34f + 4f / 3, bpm, 0);
                queueBeat(36f, bpm, 5);
                queueBeat(37f, bpm, 10);
                queueBeat(38f, bpm, 0);
                queueBeat(38f + 2f / 3, bpm, 0);
                queueBeat(38f + 4f / 3, bpm, 0);
                queueBeat(40f, bpm, 5);
                queueBeat(41f, bpm, 10);
                queueBeat(42f, bpm, 5);
                queueBeat(43f, bpm, 10);
                queueBeat(44f, bpm, 5);
                queueBeat(45f, bpm, 10);
                queueBeat(46f, bpm, 5);
                queueBeat(47f, bpm, 10);

                queueBeat(47f, bpm, -4);
                queueBeat(48f, bpm, 0);
                queueBeat(49.5f, bpm, 0);
                queueBeat(50f, bpm, 0);
                queueBeat(50.5f, bpm, 0);
                queueBeat(51.5f, bpm, 5);
                queueBeat(55f, bpm, 10);

                queueBeat(56f, bpm, 0);
                queueBeat(57.5f, bpm, 0);
                queueBeat(58f, bpm, 0);
                queueBeat(59f, bpm, 0);
                queueBeat(60f, bpm, 5);
                queueBeat(61f, bpm, 10);
                queueBeat(62f, bpm, 0);
                queueBeat(62f + 2f / 3, bpm, 0);
                queueBeat(62f + 4f / 3, bpm, 0);
                queueBeat(64f, bpm, 5);
                queueBeat(65f, bpm, 10);
                queueBeat(66f, bpm, 5);
                queueBeat(67f, bpm, 10);
                queueBeat(67.5f, bpm, 0);
                queueBeat(68f, bpm, 5);
                queueBeat(69f, bpm, 10);
                queueBeat(70f, bpm, 0);
                queueBeat(70.5f, bpm, 5);
                queueBeat(71f, bpm, 10);

                queueBeat(72f, bpm, 5);
                queueBeat(73f, bpm, 10);
                queueBeat(74f, bpm, 5);
                queueBeat(75f, bpm, 10);
                queueBeat(76f, bpm, 5);
                queueBeat(77f, bpm, 10);
                queueBeat(78f, bpm, 5);
                queueBeat(79f, bpm, 10);

                queueBeat(79f, bpm, -4);
                queueBeat(80f, bpm, 0);
                queueBeat(81f, bpm, 0);
                queueBeat(82f, bpm, 0);
                queueBeat(83f, bpm, 0);
                queueBeat(84f, bpm, 5);
                queueBeat(86f, bpm, 10);
                queueBeat(87f, bpm, 0);
                queueBeat(88f, bpm, 5);
                queueBeat(90f, bpm, 10);
                queueBeat(91f, bpm, 0);
                queueBeat(91.5f, bpm, 0);
                queueBeat(92f, bpm, 5);
                queueBeat(94f, bpm, 10);
                queueBeat(95f, bpm, 0);

                queueBeat(96f, bpm, 0);
                queueBeat(97.5f, bpm, 0);
                queueBeat(99f, bpm, 0);
                queueBeat(100f, bpm, 5);
                queueBeat(102f, bpm, 10);
                queueBeat(103f, bpm, 0);
                queueBeat(104f, bpm, 0);
                queueBeat(105f, bpm, 0);
                queueBeat(106f, bpm, 0);
                queueBeat(107f, bpm, 0);
                queueBeat(108f, bpm, 0);
                queueBeat(109f, bpm, 0);
                queueBeat(110f, bpm, 0);
                queueBeat(111f, bpm, 0);

                queueBeat(111f, bpm, -4);
                queueBeat(112f, bpm, 0);
                queueBeat(113.5f, bpm, 0);
                queueBeat(115f, bpm, 0);
                queueBeat(116f, bpm, 5);
                queueBeat(118f, bpm, 10);
                queueBeat(119f, bpm, 0);
                queueBeat(119.5f, bpm, 0);
                queueBeat(120f, bpm, 0);
                queueBeat(121.5f, bpm, 0);
                queueBeat(123f, bpm, 0);
                queueBeat(124f, bpm, 5);
                queueBeat(125f, bpm, 10);
                queueBeat(126f, bpm, 0);
                queueBeat(127f, bpm, 0);
                queueBeat(128f, bpm, 5);
                queueBeat(129f, bpm, 10);
                queueBeat(130f, bpm, 5);
                queueBeat(131f, bpm, 10);
                queueBeat(132f, bpm, 5);
                queueBeat(133f, bpm, 10);
                queueBeat(134f, bpm, 5);
                queueBeat(135f, bpm, 10);

                queueBeat(136f + 0f / 3, bpm, 0);
                queueBeat(136f + 2f / 3, bpm, 0);
                queueBeat(136f + 4f / 3, bpm, 0);
                queueBeat(138f + 0f / 3, bpm, 0);
                queueBeat(138f + 2f / 3, bpm, 0);
                queueBeat(138f + 4f / 3, bpm, 0);
                queueBeat(140f + 0f / 3, bpm, 0);
                queueBeat(140f + 2f / 3, bpm, 0);
                queueBeat(140f + 4f / 3, bpm, 0);
                queueBeat(142f + 0f / 3, bpm, 0);
                queueBeat(142f + 2f / 3, bpm, 0);
                queueBeat(142f + 4f / 3, bpm, 0);

                queueBeat(144f, bpm, 5);
                queueBeat(155f, bpm, -4);
                queueBeat(156f, bpm, 10);

                queueBeat(168f, bpm, -1);
                songLength = 168;
                audioSource.clip = music[2];
                audioSource.loop = false;
                audioSource.Play();
                break;
            default:
                bpm = 162f;
                audioSource.clip = music[0];
                audioSource.loop = true;
                audioSource.Play();
                queueBeat(1f, bpm, -1);
                break;
        }

        walkBeatAmt = songLength;
        remainingFails = FAILS_LIMIT+1;
    }

    // Update is called once per frame
    void Update()
    {
        float hit = float.NaN;
        if (Manager.Instance.song != Manager.Songs.SoundTest)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyUp(KeyCode.Space))// || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                bool miss = false;
                if (Input.GetKeyDown(KeyCode.Space)) {
                    hit = float.PositiveInfinity;
                }
                for (int i = 0; i < timings.Count; i++)
                {
                    if (!timings[i].Item4 && timings[i].Item2 >= 0)
                    {
                        if (Mathf.Abs(timings[i].Item1) < 60 / bpm * TIMING_LENIENCE) //if right within a quarter beat
                        {
                            /*switch (timings[i].Item2)
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
                            }*/
                            if (timings[i].Item2 <= 9)
                            {
                                if (!Input.GetKeyDown(KeyCode.Space))
                                { continue; }
                            } else
                            {
                                if (!Input.GetKeyUp(KeyCode.Space))
                                { continue; }
                            }
                            hit = timings[i].Item1; //set hit to the offset
                            timings[i] = (timings[i].Item1, timings[i].Item2, timings[i].Item3, true);
                            break;
                        }
                        if (timings[i].Item1 > 60 / bpm * TIMING_LENIENCE) //if there are no beats yet and we've already reached a quarter beat in the future, we've missed
                        {
                            miss = true;
                            break;
                        }
                    }
                }
                if (hit == float.PositiveInfinity && !miss) //only happens if there's no more normal notes
                {
                    hit = float.NaN;
                }
                //Debug.Log(hit.ToString());
            } //if float is NaN there's nothing pressed; if float is PositiveInfinity it's a miss.
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))// || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                hit = float.PositiveInfinity;
                for (int i = 0; i < timings.Count; i++)
                {
                    if (!timings[i].Item4 && timings[i].Item2 >= 0)
                    {
                        hit = timings[i].Item1; //set hit to the offset
                        timings[i] = (timings[i].Item1, timings[i].Item2, timings[i].Item3, true);
                        break;
                    }
                }
                if (float.IsFinite(hit))
                {
                    totaloffset += hit;
                    totalhits += 1;
                    float realoffset = totaloffset / totalhits + offset;
                    //Debug.Log(realoffset.ToString());
                }
            }
        }

        for (int i = 0; i < timings.Count; i++)
        {
            if (!timings[i].Item4)
            {
                timings[i] = (timings[i].Item1 - Time.deltaTime, timings[i].Item2, timings[i].Item3, timings[i].Item4);
                if (timings[i].Item2 >= 0 && timings[i].Item1 < 120 / bpm)
                {
                    switch (timings[i].Item2 % 5)
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
                    switch (timings[i].Item2 / 5)
                    {
                        case 1:
                            timings[i].Item3.GetComponent<SpriteRenderer>().color = Color.yellow;
                            break;
                        case 2:
                            timings[i].Item3.GetComponent<SpriteRenderer>().color = Color.blue;
                            break;
                    }
                    if (timings[i].Item1 < -60 / bpm * TIMING_LENIENCE)
                    {
                        float sailorCurrentX = sailor.transform.localPosition.x;

                        if (sailor.transform.localPosition.x < 5.82f)
                        {
                            sailor.GetComponent<SpriteRenderer>().flipX = true;
                            sailor.transform.localPosition -= new Vector3((sailorCurrentX - sailorStartX) / remainingFails, 0.0f, 0.0f);
                            sailorAnim.SetTrigger("tWalk");
                        }                        

                        if (Manager.Instance.song != Manager.Songs.SoundTest)
                        {
                            timings[i].Item3.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 200f / 255f);
                        }
                        if (fails >= FAILS_LIMIT)
                        {
                            if ((Manager.Instance.song != Manager.Songs.SoundTest) || (totalhits == 0))
                            {
                                Manager.Instance.exitBattle(0.0f);
                            }
                            else
                            {
                                Manager.Instance.exitBattle(totaloffset / totalhits + offset);
                            }

                            remainingFails = FAILS_LIMIT + 1;
                        }
                    }
                }
                if (timings[i].Item2 == -1)
                {
                    if (timings[i].Item1 < 0)
                    {
                        if (Manager.Instance.song != Manager.Songs.SoundTest)
                        {
                            Manager.Instance.exitBattle(0.0f); //success
                        }
                        else
                        {
                            Manager.Instance.exitBattle(totaloffset / totalhits + offset);
                        }

                        remainingFails = FAILS_LIMIT + 1;
                        walkBeatAmt = songLength;
                    }
                }
                if (timings[i].Item2 == -2 && timings[i].Item1 < 120 / bpm)
                {
                    timings[i].Item3.transform.localPosition = new Vector3(staffPos + (timings[i].Item1 / 60 * bpm / 2) * (noteStart - staffPos), 0, 0);
                    timings[i].Item3.transform.localScale = new Vector3(0.00625f, 1, 1);
                    timings[i].Item3.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 50f/255f);
                    timings[i].Item3.SetActive(true);
                }
                if (timings[i].Item2 == -4)
                {
                    if (timings[i].Item1 < -60 / bpm)
                    {
                        hungerMeterComponent.awardHunger(1.0f);
                    }
                }
            }
        }
        for (int i = timings.Count-1; i >= 0; i--)
        {
            if (timings[i].Item2 == -3)
            {
                continue;
            }
            if (timings[i].Item1 < -60 / bpm)
            {
                Object.Destroy(timings[i].Item3);
                if (timings[i].Item2 >= 0)
                {
                    fails += 1;
                    --walkBeatAmt;
                    --remainingFails;
                    Debug.Log(timings[i].Item2);
                }
                timings.RemoveAt(i);
            }
            if (timings[i].Item4)
            {
                timings[i].Item3.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, timings[i].Item3.GetComponent<SpriteRenderer>().color.a - Time.deltaTime * 400f / 255f);
                if (timings[i].Item3.GetComponent<SpriteRenderer>().color.a < 0)
                {
                    Object.Destroy(timings[i].Item3);
                    timings.RemoveAt(i);
                }
            }
        }

        if (timings[0].Item2 == -3 && timings[0].Item1 < (-beatsinthepast) * 60 / bpm)
        {
            queueBeat((timings[0].Item1 + offset) * bpm / 60 + beatsinthepast + 3, bpm, -2);
            beatsinthepast++;
        }

        if(float.IsNormal(hit)) //hit a note this frame
        {
            float factor = 1 - (hit / (60 / bpm * TIMING_LENIENCE));

            //fakeShipObject.transform.localScale += scaleChange * factor;
            //fakeShipObject.transform.localPosition += posChange * factor;

            //StartCoroutine(Shake(fakeShipObject));
            
            if (sailor.transform.localPosition.x >= 1.36)
            {
                if (Manager.Instance.song != Manager.Songs.SoundTest)
                {
                    if(walkBeatAmt <= 0)
                        walkBeatAmt = 1;

                    sailor.transform.localPosition += new Vector3((0.8f - sailor.transform.localPosition.x) / (walkBeatAmt - 42), 0.0f, 0.0f);
                    sailorAnim.SetTrigger("tWalk");
                }
                else
                {
                    if (walkBeatAmt <= 0)
                        walkBeatAmt = 1;

                    sailor.transform.localPosition += new Vector3((0.8f - sailor.transform.localPosition.x) / walkBeatAmt, 0.0f, 0.0f);
                    sailorAnim.SetTrigger("tWalk");
                }

                sailor.GetComponent<SpriteRenderer>().flipX = false;
            }             

            walkBeatAmt--;
        }

        time += Time.deltaTime;
        float songLengthSecs = songLength * 60 / bpm;

        float timePercentage = time / songLengthSecs;
        timeMeter.transform.localScale = new Vector3(timePercentage, 1, 1);
        timeMeter.transform.localPosition = new Vector3((timePercentage - 1) / 2, 0, -1);

        spriteRenderer.color = new Color(Mathf.Max(0, spriteRenderer.color.r - Time.deltaTime * 400f / 255f), 0, 0, 200f/255f);
        if((Manager.Instance.song != Manager.Songs.SoundTest) && float.IsPositiveInfinity(hit)) //missed a note this frame
        {
            float sailorCurrentX = sailor.transform.localPosition.x;

            if (sailor.transform.localPosition.x < 5.82f)
            {
                sailor.transform.localPosition -= new Vector3((sailorCurrentX - sailorStartX) / remainingFails, 0.0f, 0.0f);
                sailorAnim.SetTrigger("tWalk");
                sailor.GetComponent<SpriteRenderer>().flipX = true;
            }

            if (fails++ >= FAILS_LIMIT)
            {
                Manager.Instance.exitBattle(0.0f);
                remainingFails = FAILS_LIMIT+1;
            }
            Debug.Log("no note");
            spriteRenderer.color = new Color(0.5f, 0, 0, 200f / 255f);

        }
    }

    public IEnumerator Shake(GameObject gameObj)
    {
        float shakeDuration = 0.5f; // Duration of the shake
        float shakeMagnitude = 0.1f; // Magnitude of the shake
        int shakeCount = 10; // Number of shakes

        Vector3 originalPos = gameObj.transform.position;

        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float xOffset = Mathf.Sin(elapsed * shakeCount * Mathf.PI) * shakeMagnitude;
            gameObj.transform.localPosition = new Vector3(originalPos.x + xOffset, originalPos.y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        //for (int i = 0; i < shakeCount; i++)
        //{
        //    Vector3 offset = new Vector3(Random.Range(-shakeMagnitude, shakeMagnitude), 0, 0);
        //    gameObj.transform.localPosition = originalPos + offset;
        //
        //    yield return new WaitForSeconds(shakeDuration / shakeCount);
        //
        //}

        gameObj.transform.position = originalPos;

    }

    public void queueBeat(float beatCount, float bpm, int noteType)
    { //beats per minute times minutes = beats; beatCount/bpm * 60
        timings.Add((
            (beatCount / bpm * 60) - offset,
            noteType,
            Object.Instantiate(noteBasis, staffObject.transform),
            false
        ));
        //'real' types: 0: whole bar. 1: top. 2: middle-top. 3: middle-bottom. 4: bottom. 5-9: hold starts. 10-14: hold ends
        //'command' types: -1: end song. when this note is reached the song ends
        //-2: staff background [the bpm marker]. this note is created by
        //-3: staff background creator. put it at the beginning of a song
        //-4: hunger refill. adds 1 to the hunger bar [after it exits. so a beat after it's queued]
    }
}
