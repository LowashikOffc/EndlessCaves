using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class Prams : MonoBehaviour
{
    public float hp = 100f;
    public TMP_Text txt;
    public AudioSource lowHp;
    public GameObject Hook;
    public GameObject dieScreen;
    public AudioSource dieSound;
    private void Start()
    {
        StartCoroutine(RegenHp());
    }

    IEnumerator RegenHp()
    {
        while (true)
        {
            if (hp < 100 && hp > 0)
            {
                hp += 1;
                txt.text = "Hp: " + hp.ToString();
            }
            yield return new WaitForSeconds(1.213456f);
        }

    }
    void Update()
    {
        if (hp > 0f)
        {
            txt.text = "Hp: " + hp.ToString();
            lowHp.volume = (0.005f - hp / 20000);
        }
        if (hp == 0f)
        {
            hp = -100;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<CharacterController>().canMove = false;
            gameObject.GetComponent<CharacterController>().canCrouch = false;
            gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            Destroy(Hook);
            dieSound.Play();
            dieScreen.SetActive(true);
        }
        else if (hp <= 0f)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}