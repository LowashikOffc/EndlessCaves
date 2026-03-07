using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagersCheck : MonoBehaviour
{
    void Start()
    {
        if (GameObject.Find("SceneLoader"))
        {
            //Debug.Log("SceneLoader обнаружен.");
        }
        else
        {
            Debug.LogWarning("SceneLoader не обнаружен!");
            SceneManager.LoadScene(0);
        }
    }
}
