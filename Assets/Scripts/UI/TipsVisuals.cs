using System.Collections.Generic;
using UnityEngine;

public class TipsVisuals : MonoBehaviour
{
    [SerializeField] InputConfig _inputConfig;
    [SerializeField] private TipView _tipView;
    [SerializeField] private GameObject _parent;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        foreach (var i in _inputConfig.KeysBindings)
        {
            TipView tip = Instantiate(_tipView, _parent.transform);
            List<KeyCode> newKeys = ChangeKeys();
            tip.SetKeys(i.keys, i.tip);
            tip.gameObject.SetActive(true);
        }
    }

    private List<KeyCode> ChangeKeys()
    {
        var keys = new List<KeyCode>();
        foreach (var i in keys)
        {
            switch (i)
            {
                case KeyCode.F:
                    keys.Add(InputReceiver.Instance.GetKey(settingsEnum.flashlightKey));
                    break;
            }
        }
        return keys;
    }

}
