using UnityEngine;
using UnityEngine.UI;

public class Enemy_Base : MonoBehaviour
{
    public enum EnemyState { Idel,Trace,Attack,RunAway }
    public EnemyState state = EnemyState.Idel;

    [Header("Base")]
    public float speed = 3f;
    public float traceRange = 15f;
    public float attackRange = 6.0f;
    public float attackCooldown = 1.5f;

    [Header("RunAway 이후")]
    private float runAwayDistance = 20f;
    private float idleStartTime = -1f;

    [Header("탄환")]
    public GameObject[] projectilePrefab;

    public Transform firePoint;

    [Header("UI")]
    public GameObject HpSlider;
    private EnemyHP_S healthUI;

    public float lastAttackTime;

    [Header("채력")]
    public float MaxHelth=10f;
    private float helth;
    //public GameObject deathEffect;

    private Transform player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastAttackTime = -attackCooldown;
        helth = MaxHelth;

        GameObject canvas = GameObject.Find("Canvas"); // Overlay Canvas
        GameObject bar = Instantiate(HpSlider, canvas.transform);
        healthUI = bar.GetComponent<EnemyHP_S>();
        healthUI.enemy = this;

        healthUI.healthSlider.maxValue = 1f;
        healthUI.healthSlider.value = 1f;
    }
    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (state)
        {
            case EnemyState.Idel:
                if (distanceToPlayer <= traceRange)
                {
                    state = EnemyState.Trace;
                }

                // RunAway 이후 일정 시간 지나면 사라짐
                if (idleStartTime > 0 && Time.time - idleStartTime >= 5f)
                {
                    Die();
                }
                break;


            case EnemyState.Trace:
                // 플레이어를 향해 이동
                if (distanceToPlayer > attackRange)
                {
                    TracePlayer();
                }
                else
                {
                    state = EnemyState.Attack;
                }
                break;

            case EnemyState.Attack:
                // 공격 쿨타임 체크 후 발사
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    lastAttackTime = Time.time;
                    AttackPlayer();
                }

                // 공격 범위를 벗어나면 다시 추적
                if (distanceToPlayer > attackRange)
                {
                    state = EnemyState.Trace;
                }
                break;

            case EnemyState.RunAway:
                Vector3 awayDirection = (transform.position - player.position).normalized;
                transform.Translate(awayDirection * speed * Time.deltaTime, Space.World);
                transform.LookAt(transform.position + awayDirection);

                // 일정 거리 이상 도망쳤는지 확인
                if (Vector3.Distance(transform.position, player.position) > runAwayDistance)
                {
                    state = EnemyState.Idel;
                    idleStartTime = Time.time; // Idel 상태 시작 시간 기록
                }
                break;

        }
    }

    private void TracePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        transform.LookAt(player);
    }

    private void AttackPlayer()
    {
        if (projectilePrefab.Length > 0 && firePoint != null)
        {
            transform.LookAt(player); // 플레이어를 바라보도록 추가

            int index = Random.Range(0, projectilePrefab.Length);
            GameObject bullet = Instantiate(projectilePrefab[index], firePoint.position, firePoint.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.owner = BulletOwner.Enemy; // 총알 소유자 설정
            }
        }
    }


    public void TakeDamage(float damage)
    {
        helth -= damage;
        Debug.Log("현제 적 채력: " + helth);

        if (helth <= MaxHelth * 0.2)
        {
            state = EnemyState.RunAway;
        }

        if (helth <= 0)
        {
            Die();
        }
    }

    public float GetHealthPercent()
    {
        return Mathf.Clamp01(helth / MaxHelth);
    }

    private void Die()
    {
        if (healthUI != null)
        {
            Destroy(healthUI.gameObject);
        }
        Destroy(gameObject);
    }
}