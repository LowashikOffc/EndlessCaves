using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TipDef
{
    public Tips tip;
    public List<settingsEnum> actions;
    public bool enabledOnStart = false;
}

public class TipsVisuals : MonoBehaviour
{
    [SerializeField] private List<TipDef> _tips;
    [SerializeField] private TipView _tipView;
    [SerializeField] private Transform _parent;

    private readonly List<TipView> _spawnedTips = new List<TipView>();

    private void Start()
    {
        TipManager.Instance.Add += EnableTip;
        TipManager.Instance.Delete += DisableTip;
        TipsCreate();
        Refresh();  
        InputReceiver.Instance.OnRebind += Refresh;
    }

    private void OnDestroy()
    {
        if (InputReceiver.Instance != null)
            InputReceiver.Instance.OnRebind -= Refresh;
    }

    private void EnableTip(Tips tip)
    {
        //Debug.Log($"Add {tip}");

        foreach (TipView v in _spawnedTips)
        {
            if (v.GetTip() == tip)
            {
                v.gameObject.SetActive(true);
                SoundService.Instance.PlaySound(SoundID.tip);
            }
        }
    }

    private void DisableTip(Tips tip)
    {
        //Debug.Log($"Delete {tip}");

        foreach (TipView v in _spawnedTips)
        {
            if (v.GetTip() == tip) v.gameObject.SetActive(false);
        }
    }

    private void TipsCreate()
    {
        for (int i = 0; i < _tips.Count; i++)
        {
            TipView tip = Instantiate(_tipView, _parent);
            tip.gameObject.SetActive(_tips[i].enabledOnStart);
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
