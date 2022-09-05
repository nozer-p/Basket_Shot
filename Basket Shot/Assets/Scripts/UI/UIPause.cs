using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPause : MonoBehaviour
{
    public void Pause()
    {
        Time.timeScale = 0f;
    }
}
