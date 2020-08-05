using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCollect : MonoBehaviour
{
    public bool isMagnet = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magnet"))
        { 
            isMagnet = true;
        }
    }
}
