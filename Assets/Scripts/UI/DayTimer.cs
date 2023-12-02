using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayTimer : MonoBehaviour
{
    public TextMeshProUGUI txtDays;
    public TextMeshProUGUI txtHours;

    public float secondsPerHours = 60.0f;
    private int days = 0;
    private float timer = 0.0f;

    void Update()
    {
        timer += Time.deltaTime;

        days = Mathf.FloorToInt(timer / (secondsPerHours * 24.0f));
        int horas = Mathf.FloorToInt((timer % (secondsPerHours * 24.0f)) / secondsPerHours);

        txtDays.text = days.ToString();

        if (horas < 10)
            txtHours.text = "0" + horas.ToString();
        else
            txtHours.text = horas.ToString();
    }
}