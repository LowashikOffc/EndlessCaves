using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private AudioSource _snd;
    [SerializeField] private AudioSource _del;
    [SerializeField] private SceneLoader _sceneLoader;
    void Start()
    {
        StartCoroutine(logo());
    }

    private void LoadScene()
    {
        _sceneLoader.LoadSceneByIndex(1);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            LoadScene();
        }
    }

    IEnumerator logo()
    {
        yield return new WaitForSeconds(1);
        AudioSource.PlayClipAtPoint(_snd.clip, Camera.main.transform.position);
        _text.text = "L";
        yield return new WaitForSeconds(0.4f);
        AudioSource.PlayClipAtPoint(_snd.clip, Camera.main.transform.position);
        _text.text = "Lo";
        yield return new WaitForSeconds(0.1f);
        AudioSource.PlayClipAtPoint(_snd.clip, Camera.main.transform.position);
        _text.text = "Low";
        yield return new WaitForSeconds(0.1f);
        AudioSource.PlayClipAtPoint(_snd.clip, Camera.main.transform.position);
        _text.text = "Lowa";
        yield return new WaitForSeconds(0.1f);
        AudioSource.PlayClipAtPoint(_snd.clip, Camera.main.transform.position);
        _text.text = "Lowas";
        yield return new WaitForSeconds(0.05f);
        AudioSource.PlayClipAtPoint(_snd.clip, Camera.main.transform.position);
        _text.text = "Lowash";
        yield return new WaitForSeconds(0.1f);
        AudioSource.PlayClipAtPoint(_snd.clip, Camera.main.transform.position);
        _text.text = "Lowashi";
        yield return new WaitForSeconds(0.15f);
        AudioSource.PlayClipAtPoint(_snd.clip, Camera.main.transform.position);
        _text.text = "Lowashik";
        yield return new WaitForSeconds(0.05f);
        AudioSource.PlayClipAtPoint(_snd.clip, Camera.main.transform.position);
        _text.text = "Lowashik ";
        yield return new WaitForSeconds(0.1f);
        AudioSource.PlayClipAtPoint(_snd.clip, Camera.main.transform.position);
        _text.text = "Lowashik p";
        yield return new WaitForSeconds(0.1f);
        AudioSource.PlayClipAtPoint(_snd.clip, Camera.main.transform.position);
        _text.text = "Lowashik pr";
        yield return new WaitForSeconds(0.05f);
        AudioSource.PlayClipAtPoint(_snd.clip, Camera.main.transform.position);
        _text.text = "Lowashik pre";
        yield return new WaitForSeconds(0.1f);
        AudioSource.PlayClipAtPoint(_snd.clip, Camera.main.transform.position);
        _text.text = "Lowashik pres";
        yield return new WaitForSeconds(0.1f);
        AudioSource.PlayClipAtPoint(_snd.clip, Camera.main.transform.position);
        _text.text = "Lowashik prese";
        yield return new WaitForSeconds(0.2f);
        AudioSource.PlayClipAtPoint(_snd.clip, Camera.main.transform.position);
        _text.text = "Lowashik presen";
        yield return new WaitForSeconds(0.1f);
        AudioSource.PlayClipAtPoint(_snd.clip, Camera.main.transform.position);
        _text.text = "Lowashik present";
        yield return new WaitForSeconds(0.2f);
        AudioSource.PlayClipAtPoint(_snd.clip, Camera.main.transform.position);
        _text.text = "Lowashik presents";
        yield return new WaitForSeconds(1f);
        AudioSource.PlayClipAtPoint(_del.clip, Camera.main.transform.position);
        yield return new WaitForSeconds(0.4f);
        _text.text = "Lowashik present";
        yield return new WaitForSeconds(0.04f);
        _text.text = "Lowashik presen";
        yield return new WaitForSeconds(0.04f);
        _text.text = "Lowashik prese";
        yield return new WaitForSeconds(0.04f);
        _text.text = "Lowashik pres";
        yield return new WaitForSeconds(0.04f);
        _text.text = "Lowashik pre";
        yield return new WaitForSeconds(0.04f);
        _text.text = "Lowashik pr";
        yield return new WaitForSeconds(0.04f);
        _text.text = "Lowashik p";
        yield return new WaitForSeconds(0.04f);
        _text.text = "Lowashik ";
        yield return new WaitForSeconds(0.04f);
        _text.text = "Lowashik";
        yield return new WaitForSeconds(0.04f);
        _text.text = "Lowashi";
        yield return new WaitForSeconds(0.04f);
        _text.text = "Lowash";
        yield return new WaitForSeconds(0.04f);
        _text.text = "Lowas";
        yield return new WaitForSeconds(0.04f);
        _text.text = "Lowa";
        yield return new WaitForSeconds(0.04f);
        _text.text = "Low";
        yield return new WaitForSeconds(0.04f);
        _text.text = "Lo";
        yield return new WaitForSeconds(0.04f);
        _text.text = "L";
        yield return new WaitForSeconds(0.04f);
        _text.text = "";
        yield return new WaitForSeconds(0.5f);
        LoadScene();
    }
}
