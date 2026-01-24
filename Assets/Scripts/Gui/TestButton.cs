using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestButton : MonoBehaviour
{
    public AudioSource snd;
    public void PlayButton()
    {

        snd.Play();
        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Testing");
    }
}
