using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dosimeter : MonoBehaviour
{
    public GameObject pl;
    public GameObject dosimeter;
    public LayerMask targetLayer;
    public TMP_Text txt;
    public AudioSource snd;
    public AudioSource snd2;
    public float distance = 0;
    public List<GameObject> rad;
    public float totalRadiation = 0f;

    string language = "Ru";


    private void Update()
    {
        totalRadiation = 0f;
        bool isInRange = false;

        foreach (GameObject obj in rad)
        {
            // Получаем параметры источника
            Radiation source = obj.GetComponent<Radiation>();
            float radScale = source.radiationScale;
            float radDist = source.radiationDist;

            // Расстояние до источника (не ограничиваем clamp, иначе формула не будет работать)
            float distance = Vector3.Distance(dosimeter.transform.position, obj.transform.position);

            // Если источник в зоне действия
            if (distance < radDist)
            {
                isInRange = true;
                // Формула: radScale * (1 - distance/radDist)^2 (можно менять степень для разного затухания)
                float radiationIntensity = radScale * Mathf.Pow(1 - (distance / radDist), 2f);
                totalRadiation += radiationIntensity;
            }
        }

        // Вывод результата
        if (isInRange)
        {
            float displayValue = Mathf.Floor(totalRadiation * 10) / 10;
            txt.text = displayValue.ToString() + " Зв/ч";
        }
        else
        {
            txt.text = "0 Зв/ч";
        }
        dosimeter.transform.Find("Screen").GetComponent<Renderer>().material.color = new Color(Mathf.Clamp(0 + totalRadiation / 2, 0, 0.5f), 0.5f - totalRadiation / 2, 0) / 15;
    }
}