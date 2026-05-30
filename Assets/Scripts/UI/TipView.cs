using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class TipData
{
    public GameObject Object;
    public TMP_Text Text;
}

public class TipView : MonoBehaviour
{
    [SerializeField] private TipData[] _keys;
    [SerializeField] private TMP_Text _tipText;
     
    private Tips _currentTip;

    public void SetKeys(List<KeyCode> keys, Tips tip)
    {
        _currentTip = tip;
        for (int i = 0; i < keys.Count; i++)
        {
            _keys[i].Object.SetActive(true);
            _keys[i].Text.text = keys[i].ToString();
        }
        _tipText.text = TextChange(tip.ToString());
    }

    public Tips GetTip()
    {
        return _currentTip;
    }

    private string TextChange(string text)
    {
        return text.Replace("_", " ");
    }

}
