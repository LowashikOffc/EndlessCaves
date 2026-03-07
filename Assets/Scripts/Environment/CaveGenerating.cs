using System.Collections.Generic;
using UnityEngine;

public class CaveGenerating : MonoBehaviour
{
    public GameObject root;
    public GameObject pl;

    public List<GameObject> rooms;
    public GameObject currentRoom;

    private void Start()
    {
    }
    private void Generate()
    {
        int rand = UnityEngine.Random.Range(0, rooms.Count);
        GameObject newRoom = rooms[rand];
        GameObject newRoomInst = Instantiate(rooms[rand]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            Generate();
        }
    }

}
