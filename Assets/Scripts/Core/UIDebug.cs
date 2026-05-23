#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class UiDebug : MonoBehaviour
{
    [SerializeField]
    private string zonesTag = "UIZone"; // Тег для поиска

    [SerializeField]
    private bool enabled;

    private GameObject[] zones;

    public bool Enabled
    {
        get => enabled;
        set
        {
            if (enabled != value)
            {
                enabled = value;
                FindZonesByTag();
                ApplyToZones();
            }
        }
    }

    private void FindZonesByTag()
    {
        // Находим все объекты с указанным тегом
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag(zonesTag);
        zones = foundObjects;
    }

    private void ApplyToZones()
    {
        if (zones == null)
            return;

        foreach (GameObject zone in zones)
        {
            if (zone != null)
                zone.GetComponent<Image>().enabled = enabled;
        }

        //Debug.Log($"Applied {enabled} to {zones.Length} zones with tag '{zonesTag}'");
    }

    private void OnValidate()
    {
        FindZonesByTag();
        ApplyToZones();
    }

    [ContextMenu("Refresh Zones")]
    private void RefreshZones()
    {
        FindZonesByTag();
        ApplyToZones();
        Debug.Log($"Refreshed zones. Found {zones?.Length ?? 0} zones with tag '{zonesTag}'");
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(UiDebug))]
    public class UiDebugEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            UiDebug script = (UiDebug)target;

            // Отображаем текущий тег
            EditorGUILayout.LabelField("Zone Tag Settings", EditorStyles.boldLabel);

            // Поле для ввода тега
            EditorGUI.BeginChangeCheck();
            string newTag = EditorGUILayout.TagField("Zones Tag", script.GetType()
                .GetField("zonesTag", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(script) as string ?? "UIZones");

            if (EditorGUI.EndChangeCheck())
            {
                var field = script.GetType().GetField("zonesTag",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                field?.SetValue(script, newTag);
                script.RefreshZones();
            }

            EditorGUILayout.Space();

            // Поле Enabled
            EditorGUI.BeginChangeCheck();
            bool enabled = EditorGUILayout.Toggle("Enabled", script.Enabled);
            if (EditorGUI.EndChangeCheck())
            {
                script.Enabled = enabled;
            }

            EditorGUILayout.Space();

            // Информация о найденных зонах
            var zonesField = script.GetType().GetField("zones",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var zones = zonesField?.GetValue(script) as GameObject[];

            EditorGUILayout.LabelField($"Found Zones: {zones?.Length ?? 0}", EditorStyles.boldLabel);

            if (zones != null && zones.Length > 0)
            {
                foreach (var zone in zones)
                {
                    if (zone != null)
                        EditorGUILayout.LabelField("  • " + zone.name);
                }
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Refresh Zones"))
            {
                script.RefreshZones();
            }
        }
    }
#endif
}
#endif