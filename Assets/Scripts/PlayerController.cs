using System.Collections;
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
    [SerializeField] private float _timeHit = 5f;
    [SerializeField] private float _timeShield = 5f;
    //[SerializeField] private float _timeShooting = 5f;
    private Animator _animator;
    private CharacterController _characterController;
    private Vector3 _dir;
    private int _lineToMove = 1;
    private bool _isHit;
    private bool _isShield;
    //private bool _isShooting;

    void Start()
    {
        _losePanel.SetActive(false);
        Time.timeScale = 1;
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
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
                if (_isShield)
                {
                    // Сделать норм столкновение с врагом
                    break;
                }
                _losePanel.SetActive(true);
                Time.timeScale = 0;
                break;

            case "Hit":
                if (_isShield)
                {
                    break;
                }

                if (_isHit)
                {
                    _losePanel.SetActive(true);
                    Time.timeScale = 0;
                    _isHit = false;
                    break;
                }
                _isHit = true;
                _animator.SetTrigger("isHit");
                StartCoroutine(Hited(_timeHit));
                break;

            case "Coin":
                _coins++;
                _coinsText.text = _coins.ToString();
                Destroy(other.gameObject);
                break;
            default:
                break;

            case "Shield":
                _animator.SetTrigger("isShielded");
                other.gameObject.SetActive(false);
                _isShield = true;
                StartCoroutine(Shielded(_timeShield));
                break;

            //case "Shooting":
            //    other.gameObject.SetActive(false);
            //    _animator.SetTrigger("isShooting");
            //    var hit = Physics.Raycast(transform.position, transform.forward, 500);
            //    Debug.DrawRay(transform.position, Vector3.forward, Color.red);

            //    if (hit)
            //    {
            //        Debug.Log("Вдыщьььь");
            //    }
            //    //_isShooting = true;
            //    //StartCoroutine(Shooting(_timeShooting));
            //    break;
        }
    }

    IEnumerator Hited(float time)
    {
        yield return new WaitForSeconds(time);
        _isHit = false;
    }

    IEnumerator Shielded(float time)
    {
        yield return new WaitForSeconds(time);
        _isShield = false;
    }

    //IEnumerator Shooting(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    _isShooting = false;
    //}
}
