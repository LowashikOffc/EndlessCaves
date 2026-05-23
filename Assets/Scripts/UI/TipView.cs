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
     
    public void SetKeys(List<KeyCode> keys, Tips tip)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            _keys[i].Object.SetActive(true);
            _keys[i].Text.text = keys[i].ToString();
        }
        _tipText.text = TextChange(tip.ToString());
    }

    private string TextChange(string text)
    {
        return text.Replace("_", " ");
    }

}
