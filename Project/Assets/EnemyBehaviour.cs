using UnityEngine;


[RequireComponent(typeof(Health))]
public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float m_speed = 0.004f;
    [SerializeField] private float m_attackRange;
    [SerializeField] LayerMask m_hurtableMask;
    [SerializeField] private float m_attackInterval = 3;
    [SerializeField] private int m_attackDamage = 10;

    private float m_timeSinceLastAttack = 0f;
    private Transform m_heroTransform;
    private Health m_health;
    private EnemyManagement m_manager;

    // Start is called before the first frame update
    private void Start()
    {
        m_heroTransform = Object.FindFirstObjectByType<HeroKnight>()?.transform;
        if (m_heroTransform == null)
            Debug.LogError("No HeroKnight in scene!");

        m_health = GetComponent<Health>();
        m_health.OnDied += HandleDeath;
    }

    // Update is called once per frame
    void Update()
    {
        // Move *this* enemy toward the hero
        transform.position = Vector3.MoveTowards(
            transform.position,
            m_heroTransform.position,
            m_speed * Time.deltaTime);

        // Individual cooldown
        m_timeSinceLastAttack += Time.deltaTime;
        if (m_timeSinceLastAttack >= m_attackInterval)
            TryAttack();
    }

    private void TryAttack()
    {
        var hits = Physics2D.OverlapCircleAll(
        transform.position,
        m_attackRange,
        m_hurtableMask);

        bool didHit = false;
        foreach (var col in hits)
        {
            if (col.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(m_attackDamage);
                didHit = true;
            }
        }

        if (didHit)
            m_timeSinceLastAttack = 0f;
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
