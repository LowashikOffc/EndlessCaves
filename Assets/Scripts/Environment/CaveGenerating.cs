using System.Collections.Generic;
using UnityEngine;

public class CaveGenerating : MonoBehaviour
{
    public static CaveGenerating Instance { get; private set; }

    public List<GameObject> _rooms;

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
    public void Generate()
    {
        int rand = Random.Range(0, _rooms.Count);
        GameObject newRoom = _rooms[rand];
        GameObject newRoomInst = Instantiate(newRoom);
    }

}
