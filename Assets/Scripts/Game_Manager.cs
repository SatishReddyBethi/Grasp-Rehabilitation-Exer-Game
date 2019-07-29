﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Game_Manager : MonoBehaviour
{
    
    public Dropdown Game_;
    public int Game_num;
    public GameObject[] Games;
    public GameObject[] Levels;    
    public GameObject player;
    public PlayerController PC;
    public GameObject Gameover;
    public GameObject Win;
    public GameObject Beat_Game;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }    
    
    public void Game()
    {
        Game_num = Game_.value;        
        switch (Game_num)
        {
            
            case 0:
                Levels[0].SetActive(true);
                Levels[1].SetActive(false);
                Levels[2].SetActive(false);
                break;
            case 1:
                Levels[0].SetActive(false);
                Levels[1].SetActive(true);
                Levels[2].SetActive(false);
                break;
            case 2:
                Levels[0].SetActive(false);
                Levels[1].SetActive(false);
                Levels[2].SetActive(true);
                break;
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }
    
    public void Play()
    {
        Time.timeScale = 1;
    }
    public void GameOver()
    {
        if (!Win.activeInHierarchy)
        {
            Gameover.SetActive(true);
            PC.GameEnd();
            player.SetActive(true);
            Move();
        }        
    }
    public void Finish()
    {
        PC.GameEnd();
        if (Game_.value < 2)
        {
            Win.SetActive(true);
            Invoke("Move", 1); 
        }

        else
        {
            Beat_Game.SetActive(true);
            Invoke("Move", 1);
        }                
    }
    public void Move()
    {
        
        player.transform.position = new Vector3(-10.0f, 3.0f, 0f);
        PC = player.GetComponent<PlayerController>();
        PC.score = 0;
        PC.Score.text = "Score: 0";
    }
    public void NextLevel()
    {
        Game_.value = Game_.value + 1;
        
        Game();
    }
    
}