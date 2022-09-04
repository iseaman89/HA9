using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreLabel : MonoBehaviour
{
    private TextMeshProUGUI label;
    private int score;

    public int Score
    {
        get
        {
            label.text = score.ToString();
            return score;
        }
        set => score = value;
    }

    private void Start()
    {
        label = gameObject.GetComponent<TextMeshProUGUI>();
    }


}
