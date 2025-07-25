﻿using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HeroKnight : MonoBehaviour
{

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;
    [SerializeField] bool       m_noBlood = false;
    [SerializeField] GameObject m_slideDust;

    [Header("Attack Properties")]
    [SerializeField] private Transform m_attackPoint;
    [SerializeField] private float m_attackRange;
    [SerializeField] private LayerMask m_attackMask;
    [SerializeField] private int m_attackDamage = 10;
    private StorageManager m_storageManager;
    private ScoreDisplay m_scoreDisplay;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private Health              m_health;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private bool                m_isDead = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;
    private PlayerData m_playerData;
    //private int m_score = 0;


    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_health = GetComponent<Health>();
        m_storageManager = GetComponent<StorageManager>();
        m_scoreDisplay = GetComponent<ScoreDisplay>();
        Debug.Log($"Player {StaticClass.Player}");
        m_playerData = m_storageManager.ReadData(StaticClass.Player);
        Debug.Log($"Highscore {m_playerData.Highscore}");
        m_playerData.Score = 0;
        m_scoreDisplay.UpdateText(m_playerData);
        if (m_health != null)
        {
            m_health.OnHealthChanged += HandleHealthChanged;
            m_health.OnDied += HandleDeath;
        }
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();


    }

    private void Attack()
    {
        Vector2 actualAttackPosition = m_attackPoint.position;
        if (m_facingDirection == 1)
        {
            actualAttackPosition.x = m_attackPoint.position.x + 2 * (Mathf.Abs(actualAttackPosition.x - this.transform.position.x));
        }

        Debug.Log($"Attack position {actualAttackPosition.x}, {actualAttackPosition.y}");

        Collider2D[] hits = Physics2D.OverlapCircleAll(actualAttackPosition, m_attackRange, m_attackMask);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out IDamageable target))
            {
                target.TakeDamage(m_attackDamage);
                m_playerData.Score += 1;
                if (m_playerData.Score > m_playerData.Highscore)
                {
                    m_playerData.Highscore = m_playerData.Score;
                }
            }
        }
    }

    private void HandleHealthChanged(int current, int max)
    {
        Debug.Log($"Hp: {current}/{max}");
    }

    private void HandleDeath()
    {
        if (!m_isDead)
        {
            m_isDead = true;
            Debug.Log("Hero Knight has died!");

            // Death animation is handled in the health script
            // I think it makes sense, but may be worth considering moving it here if we want more control.

            // Disable player movement and physics interactions
            m_body2d.linearVelocity = Vector2.zero;
            m_body2d.bodyType = RigidbodyType2D.Kinematic;

            // TODO: Handle game over logic, e.g., show game over screen, reset level, etc.
            // GameManager.Instance.GameOver();
            m_storageManager.WritePlayerData(StaticClass.Player, m_playerData);
            SceneManager.LoadScene("GameOver");
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (m_isDead)
            return; // Do not update if the hero is dead

        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if(m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }
        
        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        //Mouse position
        Vector2 screenPositionMouse = Mouse.current.position.ReadValue();
        Vector2 worldPositionMouse = Camera.main.ScreenToWorldPoint(screenPositionMouse);

        float playerPositionX = m_body2d.position.x;

        // Swap direction of sprite depending on mouse x position
        if (worldPositionMouse.x > playerPositionX)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
            
        else if (worldPositionMouse.x < playerPositionX)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        float InputY = Input.GetAxis("Vertical");


        // Move
        /*if (!m_rolling )
            m_body2d.linearVelocity = new Vector2(inputX * m_speed, m_body2d.linearVelocity.y);*/
        if (!m_rolling)
        {
            m_body2d.linearVelocity = new Vector2(inputX * m_speed, InputY * m_speed);
        }

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.linearVelocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);

        //Death
        if (Input.GetKeyDown("e") && !m_rolling)
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
        }
            
        //Hurt
        else if (Input.GetKeyDown("q") && !m_rolling)
            m_animator.SetTrigger("Hurt");

        //Attack
        else if(Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
        {
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // Reset timer
            m_timeSinceAttack = 0.0f;

            Attack();
        }

        // Block
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1))
            m_animator.SetBool("IdleBlock", false);

        // Roll
        else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.linearVelocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.linearVelocity.y);
        }
            

        //Jump
        else if (Input.GetKeyDown("space") && m_grounded && !m_rolling)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.linearVelocity = new Vector2(m_body2d.linearVelocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
        }

        // Update Score
        m_scoreDisplay.UpdateText(m_playerData);

    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
}
