using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FpsCount : MonoBehaviour
{
    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    float m_refreshTime = 0.1f;

    public TMP_Text text;

    void Update()
    {
        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            m_lastFramerate = (float)m_frameCounter / m_timeCounter;
            m_frameCounter = 0;
            m_timeCounter = 0.0f;
            text.text = "Fps: " + Mathf.Floor(m_lastFramerate);
        }
    }
}
