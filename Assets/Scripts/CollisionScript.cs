using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    public GameObject Player;
    public PlayerController PC;
    public bool FirstPlatform;
    public bool GotPoint = false;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        PC = Player.GetComponent<PlayerController>();
    }

    
}