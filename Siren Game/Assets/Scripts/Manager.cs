using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    enum State
    {
        Menu,
        Controls,
        Overworld,
        Battle,
        Gameover,
        Soundtest
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
        Incedious
    }
    public Songs song = Songs.Null;

    public static Manager Instance
    {
        get;
        set;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(hunger + ", " + newHunger);
    }

    public void startGame()
    {
        if (state == State.Menu)
        {
            state = State.Overworld;
            newHunger = 20;
            SceneManager.LoadScene("SirenGame");
        }
    }

    public void enterBattle()
    {
        if (state == State.Overworld)
        {
            if (UnityEngine.Random.value < 0.5f)
            {
                song = Songs.Test;
            }
            else
            {
                song = Songs.Test2;
            }
            //song = Songs.SoundTest;
            song = Songs.Incedious;
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
            state = State.Soundtest;
            song = Songs.SoundTest;
            SceneManager.LoadScene("Battle");
        }
    }
}
