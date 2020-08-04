using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetTrigger : MonoBehaviour
{
    private Transform Player;
    private PlayerController collectScript;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("RPG-Character-Blender").GetComponent<Transform>();
        collectScript = Player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (collectScript.MagnetCollected == true)
        {
            if (Vector3.Distance(transform.position, Player.position) < 5)
            {
              transform.position = Vector3.MoveTowards(transform.position, Player.position, .3f);  
            }

            if (transform.position == Player.position)
            {
                Destroy(gameObject);
            }
            
        }
    }
}
