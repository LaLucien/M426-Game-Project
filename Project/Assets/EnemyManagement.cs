using UnityEngine;

public class EnemyManagement : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int initialEnemyCount = 5;
    [SerializeField] private float sizeX = 5f;
    [SerializeField] private float sizeY = 3f;



    private void Start()
    {
        for (int i = 0; i < initialEnemyCount; i++)
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy Prefab is missing!");
            return;
        }
        float x = (Random.value - 0.5f) * 2 * sizeX + transform.position.x;
        float y = (Random.value - 0.5f) * 2 * sizeY + transform.position.y;

        GameObject enemy = Instantiate(enemyPrefab, new Vector3(x, y, 0), Quaternion.identity);

        EnemyBehaviour behaviour = enemy.GetComponent<EnemyBehaviour>();
        if (behaviour != null)
        {
            behaviour.SetManager(this);  // <<< DAS HIER IST WICHTIG!

        }
        else
        {
            Debug.LogError("Enemy prefab has no EnemyBehaviour component!");
        }
    }
}