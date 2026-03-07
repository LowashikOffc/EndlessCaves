using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuRandEffect : MonoBehaviour
{
    public AudioSource menuSong;
    public AudioDistortionFilter dist;
    public TMP_Text txt;

    public Light l1;
    public Light l2;
    public Light l3;
    public Light l4;

    public Image fade;
    private void Start()
    {
        fade.color = new Color(0, 0, 0, 1);
        StartCoroutine(wait());
        int rand = Random.Range(0, 50);
        //Debug.Log(rand);
        if (rand == 0)
        {
            l1.color = Color.red;
            l2.color = l1.color;
            l3.color = l1.color;
            l4.color = l1.color;
            menuSong.pitch = 0.4f;
            dist.distortionLevel = 0.92f;
            txt.color = Color.red;
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
        while (fade.color.a > 0)
        {
            fade.color -= new Color(0,0,0,0.05f);
            yield return new WaitForSeconds(0.05f);
        }

    }
}
