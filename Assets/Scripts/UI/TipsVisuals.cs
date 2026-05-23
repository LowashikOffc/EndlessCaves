using System.Collections.Generic;
using UnityEngine;

public class TipsVisuals : MonoBehaviour
{
    [SerializeField] InputConfig _inputConfig;
    [SerializeField] private TipView _tipView;
    [SerializeField] private Transform _parent;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        foreach (var i in _inputConfig.KeysBindings)
        {
            TipView tip = Instantiate(_tipView, _parent);
            tip.gameObject.SetActive(true);
            List<KeyCode> newKeys = ChangeKeys(i.keys);
            tip.SetKeys(newKeys, i.tip);
        }
    }

    private List<KeyCode> ChangeKeys(List<KeyCode> old)
    {
        var keys = new List<KeyCode>();

        foreach (var i in old)  // итерируемся по старому списку
        {
            //Debug.Log(i);
            switch (i)
            {
                case KeyCode.F:
                    keys.Add(InputReceiver.Instance.GetKey(settingsEnum.flashlightKey));
                    break;
                case KeyCode.E:
                    keys.Add(InputReceiver.Instance.GetKey(settingsEnum.actionKey));
                    break;
                case KeyCode.Q:
                    keys.Add(InputReceiver.Instance.GetKey(settingsEnum.dropKey));
                    break;
                case KeyCode.Z:
                    keys.Add(InputReceiver.Instance.GetKey(settingsEnum.zoomKey));
                    break;
                case KeyCode.LeftControl:
                    keys.Add(InputReceiver.Instance.GetKey(settingsEnum.crouchKey));
                    break;
                default:
                    keys.Add(i); // если клавиша не требует замены, добавляем оригинал
                    break;
            }
        }

        //if (keys.Count > 0)
            //Debug.Log(keys[0]);

        return keys;
    }

}
