using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isPlayerOne = false;
    public bool isPlayerTwo = false;

    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private float _speedMultiplier = 2.5f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float _fireRate = 0.5f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private bool _isTripleShotActive = false;

    [SerializeField]
    private bool _isSpeedBoostActive = false;

    [SerializeField]
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private GameObject _shield;

    [SerializeField]
    private float _shieldDuration = 8.0f;

    [SerializeField]
    private float _tripleShotDuration = 5.0f;

    [SerializeField]
    private float _speedBoostDuration = 5.0f;

    [SerializeField]
    private int _score = 0;

    [SerializeField]
    private UIManager _uiManager;

    [SerializeField]
    private GameObject _rightEngine, _leftEngine;

    [SerializeField]
    private AudioClip _laserSoundClip;

    private AudioSource _audioSource;

    private float _nextFire = -1f;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;

    [DllImport("__Internal")]
    private static extern bool IsMobile();

    public bool isMobile()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
    return IsMobile();
#endif
        return false;
    }

    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (!_gameManager.IsCoop()) {
            transform.position = new Vector3(0, -2f, 0);
        }

        if (_shield != null) {
            _shield.SetActive(false);
        }

        if (_uiManager == null)
        {
            Debug.LogError("UIManager not found");
        }

        _audioSource = _audioSource == null ? GetComponent<AudioSource>() : _audioSource;
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the player is null");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    void Update()
    {
        if (isPlayerOne)
        {
            CalculatePlayerOneMovement();
            PlayerOneShoot();
        }
        
        if (isPlayerTwo)
        {
            CalculatePlayerTwoMovement();
            PlayerTwoShoot();
        }  
    }

    void CalculatePlayerOneMovement()
    {
        float horizontalInput = !isMobile() ? Input.GetAxis("Horizontal") : Input.GetAxis("Mouse X");
        float verticalInput = !isMobile() ? Input.GetAxis("Vertical") : Input.GetAxis("Mouse Y");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate((direction * (_isSpeedBoostActive ? _speed * _speedMultiplier : _speed)) * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 2f), 0);

        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void CalculatePlayerTwoMovement()
    {
        float horizontalInput = !isMobile() ? Input.GetAxis("Horizontal") : Input.GetAxis("Mouse X");
        float verticalInput = !isMobile() ? Input.GetAxis("Vertical") : Input.GetAxis("Mouse Y");

        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.Keypad8))
        {
            direction = Vector3.up;
        }
        else if (Input.GetKey(KeyCode.Keypad2))
        {
            direction = Vector3.down;
        }
        else if (Input.GetKey(KeyCode.Keypad4))
        {
            direction = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.Keypad6))
        {
            direction = Vector3.right;
        }


        transform.Translate((direction * (_isSpeedBoostActive ? _speed * _speedMultiplier : _speed)) * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 2f), 0);

        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    private void PlayerOneShoot()
    {
        if (!isMobile())
        {
            if (Input.GetKey(KeyCode.Space) && Time.time > _nextFire)
            {
                FirePlayerOneLaser();
            }
        }
        else
        {
            if (Input.touchCount > 0 && Time.time > _nextFire)
            {
                foreach (Touch touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        FirePlayerOneLaser();
                    }
                }
            }
        }
    }

    private void PlayerTwoShoot()
    {
        if (!isMobile())
        {
            if (Input.GetKey(KeyCode.RightShift) && Time.time > _nextFire)
            {
                FirePlayerTwoLaser();
            }
        }
        else
        {
            if (Input.touchCount > 0 && Time.time > _nextFire)
            {
                foreach (Touch touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        FirePlayerTwoLaser();
                    }
                }
            }
        }
    }

    void FirePlayerOneLaser()
    {
        _nextFire = Time.time + _fireRate;

        if (_isTripleShotActive && _tripleShotPrefab != null)
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(-1.35f, 0.28f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        if (!_audioSource.enabled)
        {
            _audioSource.enabled = true;
        }

        _audioSource.Play();
    }

    void FirePlayerTwoLaser()
    {
        _nextFire = Time.time + _fireRate;

        if (_isTripleShotActive && _tripleShotPrefab != null)
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(-1.35f, 0.28f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        if (!_audioSource.enabled)
        {
            _audioSource.enabled = true;
        }

        _audioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            SetShieldActive(false);
            return;
        }

        if (_lives > 0)
        {
            _lives--;

            if (_lives == 2)
            {
                _leftEngine.gameObject.SetActive(true);
            }
            else if (_lives == 1)
            {
                _rightEngine.gameObject.SetActive(true);
            }
        }

        if (_lives == 0)
        {
            if (_spawnManager)
            {
                _spawnManager.OnPlayerDeath();
            }

            Destroy(this.gameObject);
        }

        _uiManager.UpdateLives(_lives);
        _uiManager.CheckForBestScore();
    }

    public void AddScore(int score)
    {
        _score += score;
        _uiManager.UpdateScoreText(_score);
    }

    public void SetTripleShotActive(bool isActive)
    {
        _isTripleShotActive = isActive;

        if (_isTripleShotActive)
        {
            StartCoroutine(TripleShotPowerDownRoutine());
        }
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(_tripleShotDuration);
        SetTripleShotActive(false);
    }

    public void SetSpeedBoostActive(bool isActive)
    {
        _isSpeedBoostActive = isActive;

        if (_isSpeedBoostActive)
        {
            StartCoroutine(SpeedBoostPowerDownRoutine());
        }
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(_speedBoostDuration);
        SetSpeedBoostActive(false);
    }

    public void SetShieldActive(bool isActive)
    {
        _isShieldActive = isActive;
        _shield.SetActive(_isShieldActive);

        if (_isShieldActive)
        {
            StartCoroutine(ShieldPowerDownRoutine());
        }
    }

    IEnumerator ShieldPowerDownRoutine()
    {
        yield return new WaitForSeconds(_shieldDuration);
        SetShieldActive(false);
    }
}
