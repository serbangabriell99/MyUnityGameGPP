using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform_trigger_script : MonoBehaviour
{
    public MovingPlatform platform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           platform.NextPlatform(); 
        }
        
    }
}
