using System.Collections;
using UnityEngine;

public class TutorialLookAtPlayer : MonoBehaviour
{
    private GameObject _player;
    private Quaternion _quaternion;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(LookAtPlayer());
    }

    IEnumerator LookAtPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            _quaternion = Quaternion.LookRotation(-(_player.transform.position + (_player.transform.up * _player.transform.localScale.y/2) - transform.position));
        }
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, _quaternion, Time.deltaTime * 10);
    }
}
