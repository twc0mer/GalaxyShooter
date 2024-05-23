using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _speed = 4f;

    [SerializeField]
    private int _scoreValue = 10;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float _fireRate = 3f;

    private float _canFire = -1;
    private Player _player1;
    private Player _player2;
    private Animator _anim;

    private bool _isDestroyed = false;

    [SerializeField]
    private GameObject _explosion;

    private GameManager _gameManager;

    private AudioManager _audio;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Enemy.gameManager is null");
        }

        _player1 = _gameManager.IsCoop() ? GameObject.Find("Player_1").GetComponent<Player>() : GameObject.Find("Player").GetComponent<Player>(); ;
        if (_player1 == null)
        {
            Debug.LogError("Enemy._player1 is null");
        }

        if (_gameManager.IsCoop())
        {
            _player2 = GameObject.Find("Player_2").GetComponent<Player>();
            if (_player2 == null)
            {
                Debug.LogError("Enemy._player2 is null");
            }
        }

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Enemy.anim is null");
        }

        _audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audio == null)
        {
            Debug.LogError("Enemy.audio is null");
        }
    }

    private void Update()
    {
        CalculateMovement();
        
        if (Time.time > _canFire && !_isDestroyed)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;

            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position + new Vector3(0, -2f, 0), Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            foreach (Laser laser in lasers)
            {
                laser.AssignEnemyLaser();
            }
        }
    }

    private void CalculateMovement()
    {
        transform.Translate((Vector3.down * _speed) * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !_isDestroyed)
        {
            collision.transform.GetComponent<Player>()?.Damage();
            _speed = 0;
            _isDestroyed = true;

            if (_explosion != null && _audio != null)
            {
                _audio.PlayEnemyDestroyed();
                Instantiate(_explosion, transform.position, Quaternion.identity);
            }

            Destroy(this.gameObject);
        }

        if (collision.tag == "Laser" && !_isDestroyed)
        {
            Destroy(collision.gameObject);

            if (_player1 != null)
            {
                _player1.AddScore(_scoreValue);
            }

            _speed = 0;
            _isDestroyed = true;

            if (_explosion != null && _audio != null)
            {
                _audio.PlayEnemyDestroyed();
                Instantiate(_explosion, transform.position, Quaternion.identity);
            }

            Destroy(this.gameObject);
        }
    }
}
