using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    public TMP_Text text;
    public AudioSource snd;
    public AudioSource del;
    void Start()
    {
        StartCoroutine(logo());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    IEnumerator logo()
    {
        yield return new WaitForSeconds(1);
        AudioSource.PlayClipAtPoint(snd.clip, Camera.main.transform.position);
        text.text = "L";
        yield return new WaitForSeconds(0.4f);
        AudioSource.PlayClipAtPoint(snd.clip, Camera.main.transform.position);
        text.text = "Lo";
        yield return new WaitForSeconds(0.1f);
        AudioSource.PlayClipAtPoint(snd.clip, Camera.main.transform.position);
        text.text = "Low";
        yield return new WaitForSeconds(0.1f);
        AudioSource.PlayClipAtPoint(snd.clip, Camera.main.transform.position);
        text.text = "Lowa";
        yield return new WaitForSeconds(0.1f);
        AudioSource.PlayClipAtPoint(snd.clip, Camera.main.transform.position);
        text.text = "Lowas";
        yield return new WaitForSeconds(0.05f);
        AudioSource.PlayClipAtPoint(snd.clip, Camera.main.transform.position);
        text.text = "Lowash";
        yield return new WaitForSeconds(0.1f);
        AudioSource.PlayClipAtPoint(snd.clip, Camera.main.transform.position);
        text.text = "Lowashi";
        yield return new WaitForSeconds(0.15f);
        AudioSource.PlayClipAtPoint(snd.clip, Camera.main.transform.position);
        text.text = "Lowashik";
        yield return new WaitForSeconds(0.05f);
        AudioSource.PlayClipAtPoint(snd.clip, Camera.main.transform.position);
        text.text = "Lowashik ";
        yield return new WaitForSeconds(0.1f);
        AudioSource.PlayClipAtPoint(snd.clip, Camera.main.transform.position);
        text.text = "Lowashik p";
        yield return new WaitForSeconds(0.1f);
        AudioSource.PlayClipAtPoint(snd.clip, Camera.main.transform.position);
        text.text = "Lowashik pr";
        yield return new WaitForSeconds(0.05f);
        AudioSource.PlayClipAtPoint(snd.clip, Camera.main.transform.position);
        text.text = "Lowashik pre";
        yield return new WaitForSeconds(0.1f);
        AudioSource.PlayClipAtPoint(snd.clip, Camera.main.transform.position);
        text.text = "Lowashik pres";
        yield return new WaitForSeconds(0.1f);
        AudioSource.PlayClipAtPoint(snd.clip, Camera.main.transform.position);
        text.text = "Lowashik prese";
        yield return new WaitForSeconds(0.2f);
        AudioSource.PlayClipAtPoint(snd.clip, Camera.main.transform.position);
        text.text = "Lowashik presen";
        yield return new WaitForSeconds(0.1f);
        AudioSource.PlayClipAtPoint(snd.clip, Camera.main.transform.position);
        text.text = "Lowashik present";
        yield return new WaitForSeconds(0.2f);
        AudioSource.PlayClipAtPoint(snd.clip, Camera.main.transform.position);
        text.text = "Lowashik presents";
        yield return new WaitForSeconds(1f);
        AudioSource.PlayClipAtPoint(del.clip, Camera.main.transform.position);
        yield return new WaitForSeconds(0.4f);
        text.text = "Lowashik present";
        yield return new WaitForSeconds(0.04f);
        text.text = "Lowashik presen";
        yield return new WaitForSeconds(0.04f);
        text.text = "Lowashik prese";
        yield return new WaitForSeconds(0.04f);
        text.text = "Lowashik pres";
        yield return new WaitForSeconds(0.04f);
        text.text = "Lowashik pre";
        yield return new WaitForSeconds(0.04f);
        text.text = "Lowashik pr";
        yield return new WaitForSeconds(0.04f);
        text.text = "Lowashik p";
        yield return new WaitForSeconds(0.04f);
        text.text = "Lowashik ";
        yield return new WaitForSeconds(0.04f);
        text.text = "Lowashik";
        yield return new WaitForSeconds(0.04f);
        text.text = "Lowashi";
        yield return new WaitForSeconds(0.04f);
        text.text = "Lowash";
        yield return new WaitForSeconds(0.04f);
        text.text = "Lowas";
        yield return new WaitForSeconds(0.04f);
        text.text = "Lowa";
        yield return new WaitForSeconds(0.04f);
        text.text = "Low";
        yield return new WaitForSeconds(0.04f);
        text.text = "Lo";
        yield return new WaitForSeconds(0.04f);
        text.text = "L";
        yield return new WaitForSeconds(0.04f);
        text.text = "";
        SceneManager.LoadScene("MainMenu");
    }
}
