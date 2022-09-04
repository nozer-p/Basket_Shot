using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI score;
    static Plane plane;
    [SerializeField] private GameObject wall;

    private void Start()
    {
        score = GetComponent<TextMeshProUGUI>();
        plane = new Plane(transform.forward, transform.position);
        float borderX = Mathf.Abs(CalcPosition(new Vector2(0f, 0f)).x);
        score.text = borderX.ToString();
        wall.transform.position = new Vector3(borderX + wall.transform.localScale.x / 2, 0f, 0f);
    }

    public Vector3 CalcPosition(Vector2 screenPos)
    {
        //Ray ray = UICamera.currentCamera.ScreenPointToRay(screenPos);    // для NGUI
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        float dist = 0f;
        Vector3 pos = Vector3.zero;

        if (plane.Raycast(ray, out dist))
            pos = ray.GetPoint(dist);

        return pos;
    }
}
