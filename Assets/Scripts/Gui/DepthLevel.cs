using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DepthLevel : MonoBehaviour
{
    public GameObject plr;
    private void Start()
    {
        StartCoroutine(render());
    }

    IEnumerator render()
    {
        int i = 20;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            i--;
            if (i == 0)
            {
                i = 20;
                gameObject.GetComponent<TMP_Text>().text = "Глубина: " + Mathf.Floor(plr.transform.position.y).ToString();
            }
        }
    }
}
