using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private List<Radiation> _rad = new List<Radiation>();
    public void GetRadiation(Radiation scr)
    {
        _rad.Add(scr);
    }
    public void SendDosimeter(GameObject dosimeter)
    {
        foreach (Radiation scr in _rad)
        {
            scr.SetDosimeter(dosimeter);
        }
    }
}
