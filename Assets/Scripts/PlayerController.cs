using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _lossPanel;
    [SerializeField] private GameObject _continuePanel;
    [SerializeField] private GameObject _player;
    private ExplosionsSpawner _explosionsSpawner;
    //[SerializeField] private GameObject _shield;
    //[SerializeField] private GameObject _hit;
    //[SerializeField] private GameObject _shooting;
    [SerializeField] private RoadSpawner _roadSpawner;
    [SerializeField] private EnemiesSpawner _enemiesSpawner;
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
    [SerializeField] private int _live;
    private bool _isHit;

    private bool _isShield;
    //private bool _isShooting;

    void Start()
    {
        _explosionsSpawner = GameObject.Find("ExplosionsSpawner").GetComponent<ExplosionsSpawner>();
        _lossPanel.SetActive(false);
        Time.timeScale = 1;
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _live = _maxLive;
    }

    private void Update()
    {
        if (_lossPanel.activeSelf || _continuePanel.activeSelf) return;

        if ((SwipeController.SwipeRight || Input.GetKeyDown(KeyCode.RightArrow)) && _lineToMove < 2)
        {
            _lineToMove++;
        }

        if ((SwipeController.SwipeLeft || Input.GetKeyDown(KeyCode.LeftArrow)) && _lineToMove > 0)
        {
            _lineToMove--;
        }

        if ((SwipeController.SwipeUp || Input.GetKeyDown(KeyCode.UpArrow)) && _characterController.isGrounded)
        {
            Jump();
        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (_lineToMove == 0)
        {
            targetPosition += Vector3.left * _lineDistanse;
        }
        else if (_lineToMove == 2)
        {
            targetPosition += Vector3.right * _lineDistanse;
        }

        if (transform.position == targetPosition)
        {
            return;
        }

        Vector3 dif = targetPosition - transform.position;
        Vector3 moveDir = dif.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude > dif.sqrMagnitude)
        {
            _characterController.Move(moveDir);
        }
        else
        {
            _characterController.Move(dif);
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
                _roadSpawner.ProcessRoad(other);
                break;

            case "Died":
                if (_isShield)
                {
                    _enemiesSpawner.KillEnemy(other);
                }
                else if (_live > 0)
                {
                    StartCoroutine(Freeze());
                    _continuePanel.SetActive(true);
                    _enemiesSpawner.KillEnemy(other);
                    _live--;
                }
                else
                {
                    _lossPanel.SetActive(true);
                    var place = new Vector3(_player.transform.position.x, 2, 0);
                    var explosion = _explosionsSpawner.GenerateExplosion(place, ExplosionsSpawner.ExplosionType.Enemy);
                    StartCoroutine(ExplosionsSpawner.DisableExplosionDelay(explosion));
                    // _player.SetActive(false);
                    StartCoroutine(Freeze1());
                }
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

                    _lossPanel.SetActive(true);
                    Time.timeScale = 0;
                    _isHit = false;
                    break;
                }

                _isHit = true;
                //_hit.SetActive(true);
                StartCoroutine(Hit(_timeHit));
                break;

            case "Coin":
                _coins++;
                _coinsText.text = _coins.ToString();
                Destroy(other.gameObject);
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

    IEnumerator Freeze()
    {
        yield return new WaitForSeconds(1);
        Time.timeScale = 0;
    }
    IEnumerator Freeze1()
    {
        _animator.SetTrigger("Died");
        yield return new WaitForSeconds(2);
        Time.timeScale = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Block":
                if (_isShield)
                {
                    collision.gameObject.SetActive(false);
                }

                break;
        }
    }

    IEnumerator Hit(float time)
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