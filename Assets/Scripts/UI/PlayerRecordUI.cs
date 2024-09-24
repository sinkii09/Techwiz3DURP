using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerRecordUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI scoreText;

    public void Setup(string name, string score)
    {
        nameText.text = name;
        scoreText.text = score;
    }
}
