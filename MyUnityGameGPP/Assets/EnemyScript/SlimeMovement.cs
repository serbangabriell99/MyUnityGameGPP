using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    public float jumpForce;
    public float detectionRange;
    public float jump_rate;
    
    private Rigidbody rb;
    private Transform player;
    private float jumpTimer;
    private bool justLanded;
    private float jumpRateRandom;

    private bool isgrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        jumpTimer = 0;
        justLanded = false;
        jumpRateRandom = Random.Range(jump_rate - 1.5f, jump_rate + 1.5f);
    }

    void Update()
    {
        if (isgrounded && !justLanded)
        {
            justLanded = true;
            jumpTimer = 0;
            jumpRateRandom = Random.Range(jump_rate - 1.5f, jump_rate + 1.5f);

        }
        if (!isgrounded)
        {
            justLanded = false;
        }

        jumpTimer += Time.deltaTime;

        if (jumpTimer >= jumpRateRandom)
        {
            jumpTimer = 0;

            Vector3 jumpDir = Vector3.zero;
            
            if (Vector3.Distance(transform.position, player.position) <= detectionRange)
            {
                Vector3 playerDir = (player.position - transform.position).normalized;
                playerDir.y = 0;

                Quaternion lookAtRot = Quaternion.LookRotation(playerDir);
                transform.rotation = lookAtRot;

                jumpDir = transform.up * jumpForce + playerDir * jumpForce * 0.53f;
            }
            else
            {
                transform.rotation = Quaternion.AngleAxis(Random.Range(0, 359), Vector3.up);

                jumpDir = transform.up * jumpForce + transform.forward * jumpForce * 0.53f;
            }
            
            rb.AddForce(jumpDir, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision theCollision)
   {
       if (theCollision.gameObject.name == "floor")
       {
           isgrounded = true;
       }
   }
    void OnCollisionExit(Collision theCollision)
   {
       if (theCollision.gameObject.name == "floor")
       {
           isgrounded = false;
       }
   }
}
