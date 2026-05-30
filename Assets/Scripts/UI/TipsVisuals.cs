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
        SpawnTips();
        Refresh();
        InputReceiver.Instance.OnRebind += Refresh;
    }

    private void OnDestroy()
    {
        if (InputReceiver.Instance != null)
            InputReceiver.Instance.OnRebind -= Refresh;
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
