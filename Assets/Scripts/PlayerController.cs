using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Spawn;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _lossPanel;
    [SerializeField] private GameObject _continuePanel;
    [SerializeField] private SpawnController _spawnController;
    [SerializeField] private EnemiesSpawner _enemiesSpawner;
    [SerializeField] private Text _coinsText;
    [SerializeField] private float _gravity;
    [SerializeField] private float _lineDistanse = 3;
    [SerializeField] private int _coins;
    [SerializeField] private float _timeHit = 10f;
    [SerializeField] private float _timeShield = 10f;
    [SerializeField] private float _timeShooting = 10f;
    [SerializeField] private int _live;
    [SerializeField] private Transform bulletSpawnPosition;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 10;
    private RoadSpawner _roadSpawner;
    private Animator _animator;
    private CharacterController _characterController;
    private Vector3 _dir;
    private int _lineToMove = 1;
    private bool _highJump;
    private bool _isHit;
    private bool _isShield;
    private static readonly int IsHit = Animator.StringToHash("isHit");
    private static readonly int StartHited = Animator.StringToHash("startHited");
    private static readonly int StartShielded = Animator.StringToHash("startShielded");
    private static readonly int IsShielded = Animator.StringToHash("isShielded");
    private static readonly int StartShooting = Animator.StringToHash("startShooting");
    private static readonly int IsShooting = Animator.StringToHash("isShooting");
    private float _airDensity = 1.225f;
    private float _dragCoefficient = 1.1f;
    private float _crossSectionalArea = 0.5f;
    private float _mass = 60;
    private float _cachedSpeed;
    private Vector2 _touchStartPos;
    private Vector2 _touchEndPos;
    private AudioSource _jumpAudioSource;
    
    void Start()
    {
        _lossPanel.SetActive(false);
        Time.timeScale = 1;
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _spawnController = GameObject.Find("SpawnController").GetComponent<SpawnController>();
        _roadSpawner = GameObject.Find("RoadSpawner").GetComponent<RoadSpawner>();
        _cachedSpeed = _roadSpawner.Speed;
        _jumpAudioSource = GameObject.Find("JumpAudioSource").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_lossPanel.activeSelf || _continuePanel.activeSelf) return;

        switch (SwipeController.CurrentSwipe)
        {
            case SwipeController.Swipe.SwipeLeft:
                if (_lineToMove > 0) _lineToMove--;
                break;
            case SwipeController.Swipe.SwipeRight:
                if (_lineToMove < 2)  _lineToMove++;
                break;
            case SwipeController.Swipe.SwipeUp:
                if (_characterController.isGrounded)
                {
                    _highJump = true;
                    Jump(15);
                }
                else if (_highJump)
                {
                    _highJump = false;
                    Jump(20);
                }
                break;
            case SwipeController.Swipe.SwipeDown:
                break;
            case SwipeController.Swipe.None:
                break;
            default:
                if (Input.touchCount > 0 && _characterController.isGrounded && _roadSpawner.Speed >= _cachedSpeed)
                {
                    Jump(SwipeController.SwipeValue <= 500 ? 15 : 20);
                }
                break;
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
        _characterController.Move(moveDir.sqrMagnitude > dif.sqrMagnitude ? moveDir : dif);
    }

    void FixedUpdate()
    {
        if (!_characterController.isGrounded)
        {
            var accelerationDueToDrag = 0.5f * _airDensity * _roadSpawner.Speed * _roadSpawner.Speed *
                _dragCoefficient * _crossSectionalArea / _mass;
            _roadSpawner.Speed -= accelerationDueToDrag * Time.fixedDeltaTime;
        }
        else if (_roadSpawner.Speed < _cachedSpeed)
        {
            _roadSpawner.Speed++;
        }

        _dir.y += _gravity * Time.fixedDeltaTime;
        _characterController.Move(_dir * Time.fixedDeltaTime);
    }

    private void Jump(float jumpForce)
    {
        _jumpAudioSource.Play();
        _dir.y = jumpForce;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Respawn":
                _spawnController.GenerateNext(other);
                break;

            case "Died":
                if (_isShield)
                {
                    _enemiesSpawner.KillEnemy(other);
                }
                else if (_live > 0)
                {
                    StartCoroutine(ActivatePanel(_continuePanel));
                    _enemiesSpawner.KillEnemy(other);
                    _live--;
                }
                else
                {
                    _enemiesSpawner.KillEnemy(other);
                    StartCoroutine(ActivatePanel(_lossPanel));
                }

                break;

            case "Hit":
                if (_isShield)
                {
                    other.gameObject.SetActive(false);
                    break;
                }

                if (_isHit)
                {
                    if (_live > 0)
                    {
                        Time.timeScale = 0;
                        // _enemiesSpawner.KillEnemy(other);
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
                StartCoroutine(Hit(_timeHit));
                break;

            case "Coin":
                _coins++;
                _coinsText.text = _coins.ToString();
                other.gameObject.SetActive(false);
                break;

            case "Shield":
                other.gameObject.SetActive(false);
                _isShield = true;
                StartCoroutine(Shielded(_timeShield));
                break;

            case "Shooting":
                other.gameObject.SetActive(false);
                StartCoroutine(Shooting(_timeShooting));
                break;
        }
    }

    private IEnumerator ActivatePanel(GameObject panel)
    {
        yield return new WaitForSeconds(1);
        panel.SetActive(true);
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
        _animator.SetTrigger(StartHited);

        yield return new WaitForSeconds(time - 2f);

        _animator.SetTrigger(IsHit);

        yield return new WaitForSeconds(2f);
        _isHit = false;
    }

    IEnumerator Shielded(float time)
    {
        _animator.SetTrigger(StartShielded);

        yield return new WaitForSeconds(time - 2f);
        _animator.SetTrigger(IsShielded);

        yield return new WaitForSeconds(2f);
        _isShield = false;
    }

    IEnumerator Shooting(float time)
    {
        while (time > 0)
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawnPosition.position, bulletSpawnPosition.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPosition.forward * bulletSpeed;
            time -= 0.7f;
            yield return new WaitForSeconds(0.7f);
        }
    }
}