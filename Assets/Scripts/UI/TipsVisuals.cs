using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TipDef
{
    public Tips tip;
    public List<settingsEnum> actions;
}

public class TipsVisuals : MonoBehaviour
{
    [SerializeField] private List<TipDef> _tips;
    [SerializeField] private TipView _tipView;
    [SerializeField] private Transform _parent;

    private readonly List<TipView> _spawnedTips = new List<TipView>();

    private void Start()
    {
        TipManager.Instance.Add += AddTip;
        TipManager.Instance.Delete += DeleteTip;
        SpawnTips();
        Refresh();
        InputReceiver.Instance.OnRebind += Refresh;
    }

    private void OnDestroy()
    {
        if (InputReceiver.Instance != null)
            InputReceiver.Instance.OnRebind -= Refresh;
    }

    private void AddTip(Tips tip, settingsEnum settingsEnum)
    {
        Debug.Log($"Add {tip}");

        // Пытаемся найти существующую
        TipDef def = _tips.Find(t => t.tip == tip);

        // Если не нашли - создаем новую
        if (def == null)
        {
            def = new TipDef();
            def.tip = tip;
            def.actions = new List<settingsEnum>(); // Создаем список
            def.actions.Add(settingsEnum);
            _tips.Add(def); // Добавляем в общий список
        }

        // Создаем визуальную подсказку
        TipView newTip = Instantiate(_tipView, _parent);
        newTip.gameObject.SetActive(true);

        List<KeyCode> keys = ResolveKeys(def.actions);
        newTip.SetKeys(keys, def.tip);

        _spawnedTips.Add(newTip);
    }

    private void DeleteTip(Tips tip)
    {
        Debug.Log($"Delete {tip}");

        // Находим и удаляем подсказку для этого tip
        TipView tipToRemove = _spawnedTips.Find(t =>
            t != null && t.GetTip() == tip); // предпологая, что у TipView есть метод GetTip()

        if (tipToRemove != null)
        {
            _spawnedTips.Remove(tipToRemove);
            Destroy(tipToRemove.gameObject);
        }
    }

    private void SpawnTips()
    {
        for (int i = 0; i < _tips.Count; i++)
        {
            TipView tip = Instantiate(_tipView, _parent);
            tip.gameObject.SetActive(true);
            _spawnedTips.Add(tip);
        }
    }

    private void Refresh()
    {
        for (int i = 0; i < _tips.Count; i++)
        {
            TipDef def = _tips[i];
            List<KeyCode> keys = ResolveKeys(def.actions);
            _spawnedTips[i].SetKeys(keys, def.tip);
        }
    }

    private List<KeyCode> ResolveKeys(List<settingsEnum> actions)
    {
        var keys = new List<KeyCode>();
        foreach (var action in actions)
            keys.Add(InputReceiver.Instance.GetKey(action));
        return keys;
    }
}
