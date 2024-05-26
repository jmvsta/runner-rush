using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    private Vector3 _offset;
    
    void Start()
    {
        _offset = transform.position - _player.position;
    }

    
    void FixedUpdate()
    {
        Vector3 _newPosition = new Vector3(transform.position.y, transform.position.y, transform.position.z);
        transform.position = _newPosition;
    }
}
