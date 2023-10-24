using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PullScoreData : MonoBehaviour
{

    private string WinnerTag;
    public TextMeshProUGUI winnerTextBox;
    
    // Start is called before the first frame update
    void Start()
    {
        WinnerTag = PlayerPrefs.GetString("RemainingPlayers");
        winnerTextBox.text = WinnerTag;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
