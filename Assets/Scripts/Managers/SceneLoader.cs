using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private bool _loopToFirstScene = true;
    public static SceneLoader Instance { get; private set; }

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
    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            if (!_loopToFirstScene)
            {
                Debug.LogWarning("Next scene is NOT configured in Build Settings.");
            }

            nextIndex = 0;
        }
        SceneManager.LoadScene(nextIndex);
    }
    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
}