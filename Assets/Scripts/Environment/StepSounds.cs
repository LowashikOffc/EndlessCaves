using System.Collections;
using UnityEngine;

public class StepSounds : MonoBehaviour
{
    [SerializeField] private float _dist;
    [SerializeField, Range(0f, 1f)] private float _successChance;
    private GameObject _player;
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            float rand = Random.value;
            if (rand <= _successChance)
            {
                SoundService.Instance.PlaySound3D(SoundID.horrosSteps, _player.transform.position
                    + _player.transform.forward * -_dist
                    + _player.transform.right * Random.Range(-_dist, _dist)
                    , 0.7f);
            }
        }
    }
}
