using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private RoadSpawner _generator;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravity;
    [SerializeField] private float _lineDistanse = 3;
    private CharacterController _characterController;
    private Vector3 _dir;
    private int _lineToMove = 1;

    void Start()
    {
        _losePanel.SetActive(false);
        Time.timeScale = 1;
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (SwipeController.SwipeRight)
        {
            if (_lineToMove < 2)
            {
                _lineToMove++;
            }
        }

        if (SwipeController.SwipeLeft)
        {
            if (_lineToMove > 0)
            {
                _lineToMove--;
            }
        }

        if (SwipeController.SwipeUp)
        {
            if (_characterController.isGrounded)
            {
                Jump();
            }
        }

        Vector3 _targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (_lineToMove == 0)
        {
            _targetPosition += Vector3.left * _lineDistanse;
        }
        else if (_lineToMove == 2)
        {
            _targetPosition += Vector3.right * _lineDistanse;
        }

        if (transform.position == _targetPosition)
        {
            return;
        }

        Vector3 _dif = _targetPosition - transform.position;
        Vector3 _moveDir = _dif.normalized * 25 * Time.deltaTime;
        if (_moveDir.sqrMagnitude > _dif.sqrMagnitude)
        {
            _characterController.Move(_moveDir);
        }
        else
        {
            _characterController.Move(_dif);
        }
    }

    void FixedUpdate()
    {
    //    _dir.z = _speed;
        _dir.y += _gravity * Time.fixedDeltaTime;
        _characterController.Move(_dir * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        _dir.y = _jumpForce;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Respawn"))
        {
            _generator.DeactivateRoad(other.transform.parent.gameObject);
            _generator.ActivateRoad();
        }
    
        if (other.gameObject.CompareTag("Died"))
        {
            _losePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
