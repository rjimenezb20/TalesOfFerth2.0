using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayTimer : MonoBehaviour
{
    public TextMeshProUGUI txtDays;
    public TextMeshProUGUI txtHours;

    public int WinningDay;
    public float secondsPerHours = 60.0f;
    private int days = 0;
    private int daysAux = 0;
    private float timer = 0f;
    private WavesController WC;
    private UIManager UIM;

    private void Start()
    {
        WC = FindAnyObjectByType<WavesController>();
        UIM = FindAnyObjectByType<UIManager>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        days = Mathf.FloorToInt(timer / (secondsPerHours * 24.0f));
        int horas = Mathf.FloorToInt((timer % (secondsPerHours * 24.0f)) / secondsPerHours);

        CheckDayChange();

        txtDays.text = days.ToString();

        if (horas < 10)
            txtHours.text = "0" + horas.ToString() + ":00";
        else
            txtHours.text = horas.ToString() + ":00";

        if (WinningDay == days)
            UIM.Win();
    }

    private void CheckDayChange()
    {
        if (daysAux != days)
        {
            WC.WaveManagement(days);
            daysAux = days;
        }
    }
}