#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class Bootstrapper
{
    static Bootstrapper()
    {
        // 1. Найди свою сцену Boot в окне Project
        // 2. Нажми на неё правой кнопкой мыши -> Copy Path
        // 3. Вставь этот путь вместо того, что в кавычках ниже:
        string pathToBootScene = "Assets/Scenes/Bootstrap.unity"; 

        SceneAsset bootScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathToBootScene);

        if (bootScene != null)
        {
            // Устанавливаем эту сцену как стартовую для режима Play в редакторе
            EditorSceneManager.playModeStartScene = bootScene;
            Debug.Log("<color=green>Загрузочная сцена установлена: </color>" + pathToBootScene);
        }
        else
        {
            Debug.LogError("Не удалось найти сцену Boot по пути: " + pathToBootScene);
        }
    }
}
#endif