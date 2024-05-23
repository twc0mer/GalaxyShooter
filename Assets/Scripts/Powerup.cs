using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _fallSpeed = 3.5f;

    [SerializeField]
    private int _powerupID;

    [SerializeField]
    private AudioClip _audioClip;

    void Start()
    {
        if (_audioClip == null)
        {
            Debug.LogError("AudioClip on Powerup is null");
        }
    }

    void Update()
    {
        transform.Translate((Vector3.down * _fallSpeed) * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_audioClip, transform.position);

            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.SetTripleShotActive(true);
                        break;
                    case 1:
                        player.SetSpeedBoostActive(true);
                        break;
                    case 2:
                        Debug.Log("Collided With Shield Power-up");
                        player.SetShieldActive(true);
                        break;
                }
            }

            Destroy(this.gameObject);
        }
    }
}
