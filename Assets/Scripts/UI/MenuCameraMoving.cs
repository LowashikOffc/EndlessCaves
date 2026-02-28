using UnityEngine;

public class MenuCameraMoving : MonoBehaviour
{
    private Vector3 _savedPosition;
    private Quaternion _savedRotation;
    private Quaternion _startRotation;
    private Vector3 _startPosition;
    [SerializeField] private float _moveScale;
    [SerializeField] private float _moveSmooth;
    [SerializeField] private float _rotateSmooth;

    private void Start()
    {
        _startRotation = transform.rotation;
        _startPosition = transform.position;
    }
    void Update()
    {
        _savedPosition = Camera.main.transform.position;
        _savedRotation = Camera.main.transform.rotation;
        float H = Input.mousePosition.x;
        float V = Input.mousePosition.y;
        Camera.main.transform.position = Vector3.Lerp(_savedPosition, new Vector3(H * _moveScale,V * _moveScale, 0) + _startPosition, Time.deltaTime * _moveSmooth);
        Camera.main.transform.rotation = Quaternion.Lerp(_savedRotation, _startRotation * Quaternion.Euler((V / -200)+20,(H / 200) - 10, _savedRotation.z), Time.deltaTime * _rotateSmooth);
    }
}
