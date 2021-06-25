using UnityEngine;

public class EnemyDestructionDelegate : MonoBehaviour
{
    public delegate void EnemyDelegate(GameObject enemy);
    public EnemyDelegate enemyDelegate = null;

    void OnDestroy()
    {
        if (enemyDelegate != null)
        {
            enemyDelegate(gameObject);
        }
    }
}
