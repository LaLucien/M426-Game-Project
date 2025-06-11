using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public Rigidbody2D HeroKnight;
    public Rigidbody2D Enemy;
    public float Speed;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Enemy.transform.position = Vector3.MoveTowards(Enemy.transform.position, HeroKnight.transform.position, Speed);
    }
}
