using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MineralInfoVisual : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    private string _savedName;
    private string _savedDescription;
    [SerializeField] private Image _value;
    [SerializeField] private Image _light;
    public void Rename(string newName, string newDescription)
    {
        _name.text = newName;
        _description.text = newDescription;
        _savedName = _name.text;
        _savedDescription = _description.text;
    }

    public void ChangeVisuals(bool state)
    {
        if (state) Enable();
        else Disable();
    }

    private void Enable()
    {
        _name.text = _savedName;
        _description.text = _savedDescription;
        _value.color = Color.green;
        _light.color = new Color32(0, 255, 0, 20);
    }

    private void Disable()
    {
        _name.text = "???";
        _description.text = "???";
        _value.color = Color.red;
        _light.color = new Color32(255, 0, 0, 20);
    }
}
