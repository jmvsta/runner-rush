using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private RoadSpawner _generator;
    [SerializeField] private Text _coinsText;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravity;
    [SerializeField] private float _lineDistanse = 3;
    [SerializeField] private int _coins;
    [SerializeField] private int _live;
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
        if (SwipeController.SwipeRight || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_lineToMove < 2)
            {
                _lineToMove++;
            }
        }

        if (SwipeController.SwipeLeft || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_lineToMove > 0)
            {
                _lineToMove--;
            }
        }

        if (SwipeController.SwipeUp || Input.GetKeyDown(KeyCode.UpArrow))
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
        switch (other.gameObject.tag)
        {
            case "Respawn":
                Debug.Log("respawn");
                _generator.ProcessRoad(other.transform.parent.gameObject);
                break;

            case "Died":
                _losePanel.SetActive(true);
                Time.timeScale = 0;
                break;

            case "Coin":
                _coins++;
                _coinsText.text = _coins.ToString();
                Destroy(other.gameObject);
                break;
            default:
                break;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    switch (collision.gameObject.tag)
    //    {
    //        case "Hit":
    //            _live++;
    //            Debug.Log(_live);
    //            break;
    //    }
    //}
}
