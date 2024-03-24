using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _continuePanel;
    //[SerializeField] private GameObject _shield;
    //[SerializeField] private GameObject _hit;
    //[SerializeField] private GameObject _shooting;
    [SerializeField] private RoadSpawner _generator;
    [SerializeField] private Text _coinsText;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravity;
    [SerializeField] private float _lineDistanse = 3;
    [SerializeField] private int _coins;
    [SerializeField] private float _timeHit = 10f;
    [SerializeField] private float _timeShield = 10f;
    [SerializeField] private float _timeShooting = 10f;
    private Animator _animator;
    private CharacterController _characterController;
    private Vector3 _dir;
    private int _lineToMove = 1;
    private int _maxLive = 2;
    private int _live;
    private bool _isHit;
    private bool _isShield;
    //private bool _isShooting;

    void Start()
    {
        _losePanel.SetActive(false);
        Time.timeScale = 1;
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _live = _maxLive;
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
                    Debug.Log("В дайде с шиелдом");
                    // Сделать норм столкновение с врагом
                    break;
                }

                if (_live > 0)
                {
                    Time.timeScale = 0;
                    _continuePanel.SetActive(true);
                    _live--;
                    break;
                }

                Debug.Log("В дайде бэз шиелда");
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
                    if (_live > 0)
                    {
                        Time.timeScale = 0;
                        _continuePanel.SetActive(true);
                        _live--;
                        _isHit = false;
                        break;
                    }

                    _losePanel.SetActive(true);
                    Time.timeScale = 0;
                    _isHit = false;
                    break;
                }
                _isHit = true;
                //_hit.SetActive(true);
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
                other.gameObject.SetActive(false);
                _isShield = true;
                StartCoroutine(Shielded(_timeShield));
                break;

            case "Shooting":
                other.gameObject.SetActive(false);
                //_shooting.SetActive(true);
                //_isShooting = true;
                StartCoroutine(Shooting(_timeShooting));
                break;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("До свитче");

        switch (collision.gameObject.tag)
        {
            case "Block":
                Debug.Log("В кейсе");
;
                if (_isShield)
                {
                    Debug.Log("В ифе");
                    collision.gameObject.SetActive(false);
                    break;
                }
                break;
        }
    }

    IEnumerator Hited(float time)
    {
        _animator.SetTrigger("startHited");

        yield return new WaitForSeconds(time - 2f);
        
        _animator.SetTrigger("isHit");

        yield return new WaitForSeconds(2f);
        _isHit = false;
        //_hit.SetActive(false);
    }

    IEnumerator Shielded(float time)
    {
        _animator.SetTrigger("startShielded");

        yield return new WaitForSeconds(time - 2f);
        _animator.SetTrigger("isShielded");

        yield return new WaitForSeconds(2f);
        _isShield = false;
        //_shield.SetActive(false);

    }

    IEnumerator Shooting(float time)
    {
        _animator.SetTrigger("startShooting");

        yield return new WaitForSeconds(time - 2f);
        _animator.SetTrigger("isShooting");

        yield return new WaitForSeconds(2f);
       //_isShooting = false;
        //_shooting.SetActive(false);
    }
}
