using UnityEngine;


[RequireComponent(typeof(Health))]
public class EnemyBehaviour : MonoBehaviour
{
    public Rigidbody2D m_heroKnight;
    [SerializeField] private GameObject m_enemy;
    private float m_speed = 0.004f;

    private EnemyManagement m_manager;
    private Health m_health;

    [SerializeField] private float m_attackRange;
    [SerializeField] LayerMask m_hurtableMask;
    [SerializeField] private float m_damageDealTimeout;
    [SerializeField] private int m_attackDamage = 10;
    private float m_timeSinceLastDamageDealtSec = 0f;

    private void Awake()
    {
        m_health = GetComponent<Health>();
        m_health.OnDied += HandleDeath;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (m_heroKnight == null)
        {
            m_heroKnight = Object.FindFirstObjectByType<HeroKnight>()?.GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_enemy.transform.position = Vector3.MoveTowards(m_enemy.transform.position, m_heroKnight.transform.position, m_speed);
        m_timeSinceLastDamageDealtSec += Time.deltaTime;
        if (m_timeSinceLastDamageDealtSec > m_damageDealTimeout)
        {
            DealDamage();
            m_timeSinceLastDamageDealtSec = 0f;
        }
    }

    private void DealDamage()
    {
        Collider2D[] objs = Physics2D.OverlapCircleAll(this.transform.position, m_attackRange, m_hurtableMask);

        foreach (Collider2D obj in objs)
        {
            if (obj.TryGetComponent(out IDamageable hit))
            {
                Debug.Log($"Attackedby {this.transform.position.x}, {this.transform.position.y}");
                hit.TakeDamage(m_attackDamage);
            }
        }
    }

    private void HandleDeath()
    {
        m_manager?.SpawnEnemy();
        Destroy(gameObject);
    }

    public void SetManager(EnemyManagement mgr)
    {
        m_manager = mgr;
    }
}
