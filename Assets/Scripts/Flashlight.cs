using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    public Light l1;
    public TMP_Text text;
    public AudioSource snd;
    public float energy = 1000;
    public bool CanEn = true;

    void Start()
    {
        StartCoroutine(FlEnergyDown());
    }
    void Update()
    {
        text.text = "Fl: " + energy / 10;
        if (Input.GetKeyDown(KeyCode.F)&&l1.enabled == false && energy > 0)
        {
            if (CanEn == true)
            {
                l1.enabled = true;
                snd.Play();
            }
        }
        else if (Input.GetKeyDown(KeyCode.F) && l1.enabled == true)
        {
            l1.enabled = false;
            snd.Play();
        }
    }

    IEnumerator FlEnergyDown()
    {
        while (true)
        {
            if (l1.enabled == true && energy > 0)
            {
                energy -= 1f;
            }
            else if (l1.enabled == true && energy == 0)
            {
                if (CanEn == true)
                {
                    CanEn = false;
                    Off();
                }
            }
            yield return new WaitForSeconds(0.3f);
        }    
    }

    void Off()
    {
        l1.enabled = false;
        snd.Play();
    }
}
