using System.Collections;
using TMPro;
using UnityEngine;

public class Prams : MonoBehaviour
{
    [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private float hp = 100f;
    [SerializeField] private TMP_Text txt;
    [SerializeField] private AudioSource lowHp;
    [SerializeField] private GameObject Hook;
    [SerializeField] private GameObject dieScreen;
    [SerializeField] private AudioSource dieSound;
    private Rigidbody _rigidbody;
    private CharacterController _characterController;
    private void Start()
    {
        StartCoroutine(RegenHp());
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _characterController = gameObject.GetComponent<CharacterController>();
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
            _rigidbody.constraints = RigidbodyConstraints.None;
            _rigidbody.useGravity = true;
            _characterController.canMove = false;
            _characterController.canCrouch = false;
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
                _sceneLoader.LoadSceneByIndex(1);
            }
        }
    }
}