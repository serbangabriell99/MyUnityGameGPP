using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;
    void Update()
    {
        scoreText.text = PlayerController.coinsCollected.ToString();
    }
}
