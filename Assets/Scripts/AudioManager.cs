using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyDestroyed;

    void Start()
    {
        if (_enemyDestroyed == null)
        {
            Debug.LogError("AudioManager.enemyDestroyed is null");
        }
    }

    public void PlayEnemyDestroyed()
    {
        if (_enemyDestroyed != null)
        {
            _enemyDestroyed.gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
