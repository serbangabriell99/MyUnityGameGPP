using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiamondsScore : MonoBehaviour
{
    public Text scoreText;
    void Update()
    {
        scoreText.text = PlayerController.diamondsCollected.ToString();
    }
}
