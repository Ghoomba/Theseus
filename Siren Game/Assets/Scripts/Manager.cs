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
        Overworld,
        Battle,
        Gameover
    }

    State state = State.Menu;
    public float hunger = -1;
    public float newHunger = -1;

    public enum Songs
    {
        Null,
        Test,
        Test2,
        SoundTest
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
        
    }

    public void startGame()
    {
        if (state != State.Overworld)
        {
            state = State.Overworld;
            newHunger = 20;
            SceneManager.LoadScene("SirenGame");
        }
    }

    public void enterBattle()
    {
        if (state != State.Battle)
        {
            if (UnityEngine.Random.value < 0.5f)
            {
                song = Songs.Test;
            }
            else
            {
                song = Songs.Test2;
            }
            song = Songs.SoundTest;
            state = State.Battle;
            newHunger = hunger;
            SceneManager.LoadScene("Battle");
        }
    }

    public void lose()
    {
        if (state != State.Gameover)
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
    }

    public void toTitle()
    {
        if (state != State.Menu)
        {
            state = State.Menu;
            SceneManager.LoadScene("Menu");
        }
    }
}
