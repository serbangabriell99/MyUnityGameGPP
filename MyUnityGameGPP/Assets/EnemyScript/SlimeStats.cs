using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStats : MonoBehaviour
{
    public bool spawnsMoreOnDeath;
    public int numberToSpawnOnDeath;
    public float spawnSpreadDistanceOnDeath;

    [SerializeField] private GameObject slimeToSpawnOnDeath;

    void Start()
    {
    }

    public void spawnMoreOnDeath()
    {
        if (spawnsMoreOnDeath)
        {
            for (int i = 0; i < numberToSpawnOnDeath; i++)
            {
                Vector3 spawnPos = transform.position +
                                   transform.right * ((-0.50f * (numberToSpawnOnDeath - 1)) + i) * spawnSpreadDistanceOnDeath;

                Instantiate(slimeToSpawnOnDeath, spawnPos, transform.rotation);
            }
        }
    }
}
