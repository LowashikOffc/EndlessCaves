using System.Collections;
using TMPro;
using UnityEngine;

public class Prams : MonoBehaviour
{
    [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private float _hp = 100f;
    [SerializeField] private TMP_Text _txt;
    [SerializeField] private AudioSource _lowHp;
    [SerializeField] private GameObject _hook;
    [SerializeField] private GameObject _dieScreen;
    [SerializeField] private AudioSource _dieSound;
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
            if (_hp < 100 && _hp > 0)
            {
                _hp += 1;
                _txt.text = "Hp: " + _hp.ToString();
            }
            yield return new WaitForSeconds(1.213456f);
        }

    }
    void Update()
    {
        if (_hp > 0f)
        {
            //_txt.text = "Hp: " + _hp.ToString();
            //_lowHp.volume = (0.005f - _hp / 20000);
        }
        if (_hp <= 0f)
        {
            _hp = -100;
            _rigidbody.constraints = RigidbodyConstraints.None;
            _rigidbody.useGravity = true;
            _characterController.canMove = false;
            _characterController.canCrouch = false;
            gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            Destroy(_hook);
            _dieSound.Play();
            _dieScreen.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                _sceneLoader.LoadSceneByIndex(1);
            }
        }
    }
}