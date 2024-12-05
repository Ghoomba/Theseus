using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
//using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public enum State
    {
        Menu,
        Controls,
        Overworld,
        Battle,
        Gameover,
        Soundtest,
        Close
    }

    State state = State.Menu;



    public float hunger = -1;
    public float newHunger = -1;

    public enum Songs
    {
        Null,
        Test,
        Test2,
        SoundTest,
        Incedious,
        Pride,
        Wrath,
        Lust
    }
    public Songs song = Songs.Null;

    public List<Sprite> shipSprites;
    public Sprite ship = null;

    public static Manager Instance = null;
    // Start is called before the first frame update
    void Start()
    {
        shipSprites = new List<Sprite> { Resources.Load<Sprite>("s1"), Resources.Load<Sprite>("s2"), Resources.Load<Sprite>("s3"), Resources.Load<Sprite>("s4") };
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        //Debug.Log(Instance);

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(hunger + ", " + newHunger);
    }

    public void startGame()
    {
        if (state == State.Menu || state == State.Gameover)
        {
            Destroy(RetainAudioPoint.instance.gameObject);
            state = State.Overworld;
            newHunger = 20;
            SceneManager.LoadScene("SirenGame");
        }
    }

    public void enterBattle(Songs song)
    {
        if (state == State.Overworld)
        {
            /*if (UnityEngine.Random.value < 0.5f)
            {
                song = Songs.Test;
            }
            else
            {
                song = Songs.Test2;
            }*/
            //song = Songs.SoundTest;
            //song = Songs.Pride;

            this.song = song;
            state = State.Battle;
            newHunger = hunger;
            SceneManager.LoadScene("Battle");
            
        }
    }

    public void lose()
    {
        if (state == State.Overworld || state == State.Battle)
        {
            song = Songs.Null;
            state = State.Gameover;
            SceneManager.LoadScene("EndGame");
        }
    }

    public void exitBattle(float restoration)
    {
        if (state == State.Battle)
        {
            song = Songs.Null;
            state = State.Overworld;
            newHunger = hunger + restoration;
            SceneManager.LoadScene("SirenGame");
        }
        if (state == State.Soundtest)
        {
            song = Songs.Null;
            PlayerPrefs.SetFloat("Offset", restoration);
            state = State.Controls;
            SceneManager.LoadScene("Controls");
        }
    }

    public void toTitle()
    {
        if (state != State.Menu)
        {
            state = State.Menu;
            SceneManager.LoadScene("Menu");
        }
    }

    public void toControlsScreen()
    {
        if (state != State.Controls)
        {
            state = State.Controls;
            SceneManager.LoadScene("Controls");
        }
    }

    public void soundTest()
    {
        if (state == State.Controls)
        {
            Destroy(RetainAudioPoint.instance.gameObject);
            state = State.Soundtest;
            song = Songs.SoundTest;
            SceneManager.LoadScene("Battle");
        }
    }

    public void btnState(State state)
    {
        switch(state)
        {
            case State.Menu:
                Manager.Instance.toTitle();
                break;
            case State.Controls:
                Manager.Instance.toControlsScreen();
                break;
            case State.Soundtest:
                Manager.Instance.soundTest();
                break;
            case State.Overworld:
                Manager.Instance.startGame();
                break;
            case State.Close:
                Application.Quit();
                Debug.Log("Quitting!");
                break;
        }
    }
}
