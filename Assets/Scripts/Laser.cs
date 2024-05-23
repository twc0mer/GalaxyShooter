using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;

    [SerializeField]
    private bool _isEnemyLaser = false;

    void Update()
    {
        if (!_isEnemyLaser)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && _isEnemyLaser)
        {
            Player player = collision.GetComponent<Player>();
            player.Damage();
        }
    }

    private void MoveUp()
    {
        transform.Translate((Vector3.up * _speed) * Time.deltaTime);

        if (transform.position.y >= 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    private void MoveDown()
    {
        transform.Translate((Vector3.down * _speed) * Time.deltaTime);

        if (transform.position.y <= -8f)
        {
            if (transform.parent != null)
            {
                 Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }
}
