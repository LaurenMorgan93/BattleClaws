using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Collectables : MonoBehaviour
{
    public TextMeshProUGUI P1ScoreText;
    public int currentScore;
    public int basicPoints;

    private void Start()
    {
         P1ScoreText.text = currentScore.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("P1Score"))
        {

            currentScore += basicPoints;
            P1ScoreText.text = currentScore.ToString();
            
            // You can also play a sound or add visual effects here if needed.

            // Destroy the collectable object if needed
            Destroy(gameObject);
        }
    }
}

