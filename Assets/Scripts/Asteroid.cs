using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 3f;

    [SerializeField]
    private GameObject _explosionAnim;

    private SpawnManager _spawnManager;

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is null");
        }
    }

    void Update()
    {
        this.transform.Rotate((Vector3.forward * _rotationSpeed) * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            _explosionAnim.transform.localScale = new Vector3(1.5f, 1.5f, 0f);
            GameObject explosion = Instantiate(_explosionAnim, transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();

            Destroy(this.gameObject, 0.25f);
            Destroy(collision.gameObject);
            Destroy(explosion.gameObject, 3f);
        }
    }
}
