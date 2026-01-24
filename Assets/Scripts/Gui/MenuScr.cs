using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScr : MonoBehaviour
{
    public GameObject menu;
    public GameObject anMenu;
    public AudioSource snd;
    public void onClick()
    {
        if (menu.activeSelf == true)
        {
            menu.SetActive(false);
            AudioSource.PlayClipAtPoint(snd.clip, new Vector3(0, 2, -7));
        }
        else if (menu.activeSelf == false)
        {
            menu.SetActive(true);
            anMenu.SetActive(false);
            AudioSource.PlayClipAtPoint(snd.clip, new Vector3(0, 2, -7));
        }
    }
}
