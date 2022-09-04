using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private TextMeshProUGUI score;

    private void Start()
    {
        score = GetComponent<TextMeshProUGUI>();        
    }
}