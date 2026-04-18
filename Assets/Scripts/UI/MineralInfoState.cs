using UnityEngine;

public class MineralInfoState : MonoBehaviour
{
    [SerializeField] private Minerals _type;
    [SerializeField] private string _description;
    [SerializeField] private MineralInfoVisual _visual;
    private bool _currentState = false;
    void Start()
    {
        _visual.Rename(_type.ToString(), _description);
        ChangeState(false);

        InputReceiver.Instance.MouseR += TEST;
    }

    private void OnDestroy()
    {

        InputReceiver.Instance.MouseR -= TEST;
    }

    private void TEST(bool a)
    {
        ChangeState(a);
    }

    public void ChangeState(bool state)
    {
        _currentState = state;
        _visual.ChangeVisuals(state);
    }
}
