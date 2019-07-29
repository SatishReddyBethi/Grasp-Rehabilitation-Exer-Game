using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public Game_Manager manager;
    void OnTriggerEnter2D()
    {
        manager.Finish();
    }
}
