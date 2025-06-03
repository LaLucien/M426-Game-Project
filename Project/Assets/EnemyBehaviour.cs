using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, IDamagable
{
    [SerializeField] private float m_attackRange;
    [SerializeField] LayerMask m_hurtableMask;
    [SerializeField] private float m_damageDealTimeout;
    private float m_timeSinceLastDamageDealtSec = 0f;
    public void Damage(float damageMultiplier = 1)
    {
        Debug.Log("Hit Enemy");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
            if (obj.TryGetComponent(out IHurtable hit))
            {
                Debug.Log($"Attackedby {this.transform.position.x}, {this.transform.position.y}");
                hit.TakeDamage();
            }
        }
    }
}
