using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 10;
    public float knockbackUpForce = 0;
    public float knockbackBackForce = 0;

    private Rigidbody rb;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Death();

           if (GetComponent<SlimeStats>() != null)
            {
                SlimeStats stats = GetComponent<SlimeStats>();
                stats.spawnMoreOnDeath();
            }
        }

        Knockback();
    }

    public void Knockback()
    {
        Vector3 dir = transform.up * knockbackUpForce + player.forward * knockbackBackForce;

        if (GetComponent<Rigidbody>() != null)
        {
            rb.AddForce(dir, ForceMode.Impulse);
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {

        if ( player.GetComponent<Animator>().GetBool("attack") && other.CompareTag("Hand"))
        {
            TakeDamage(4);
        }
    }
}