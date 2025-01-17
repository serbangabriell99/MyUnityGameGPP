﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpline : MonoBehaviour
{

    public GameObject text;

    public Transform teleportTarget;
    public GameObject thePlayer;
    
    public static bool on_station = false;
    public static bool key_pressed = false;

    public static bool onBoat = false;
    public static bool onBoatNotMoving = false;

    public static bool on_station_and_can_move = false;
    // Start is called before the first frame update
    void Start()
    {
        text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (key_pressed)
        {
            CameraCont.dstFromTarget = 10;
        }
        else
        {
            CameraCont.dstFromTarget = 5; 
        }
        if (Input.GetKeyDown(KeyCode.H) && on_station)
        {
            key_pressed = true;
            onBoat = true;
            thePlayer.transform.position = teleportTarget.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.H) && !on_station_and_can_move )
        {
            gameObject.transform.parent = null;
            key_pressed = false;
            onBoat = false; 
        }
        
         if (Input.GetKeyDown(KeyCode.H) && !on_station && onBoat)
        {
            gameObject.transform.parent = null;
            key_pressed = false;
            onBoat = false;
        }

         if (Input.GetKeyDown(KeyCode.H)  && !on_station && onBoatNotMoving)
         {
             key_pressed = true;
             onBoat = true;
             thePlayer.transform.position = teleportTarget.transform.position;  
         }

        if (key_pressed && GetComponent<PlayerController>().enabled )
        {
            transform.parent = GameObject.FindWithTag("PathCamera").transform;
            GetComponent<PlayerController>().enabled = false; 
        }else if (!key_pressed && !GetComponent<PlayerController>().enabled)
        {
            GetComponent<PlayerController>().enabled = true;  
        }

        if (on_station || (!on_station && onBoatNotMoving) || (!on_station && onBoat))
        {
            text.SetActive(true);
        }
        else
        {
            text.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
         if (other.CompareTag("SplineStation") )
         {
             on_station = true;

             if (GetComponent<PlayerController>().enabled)
             {
                 on_station_and_can_move = true;
             }else if (!GetComponent<PlayerController>().enabled)
             {
                 on_station_and_can_move = false; 
             }
         }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PathCamera"))
        {
           if (GetComponent<PlayerController>().enabled)
           {
               onBoatNotMoving = true;
           }else if (!GetComponent<PlayerController>().enabled)
           {
               onBoatNotMoving = false;
           } 
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SplineStation"))
        {
            on_station = false;
        }
        
        if(other.CompareTag("PathCamera"))
        {
            onBoatNotMoving = false;
        }

    }
    
}
