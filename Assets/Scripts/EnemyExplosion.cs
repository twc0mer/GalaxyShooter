using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    void Start()
    {
        Destroy(this.gameObject, 2.8f);
    }
}
