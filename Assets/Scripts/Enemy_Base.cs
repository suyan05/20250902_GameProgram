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

    [Header("RunAway ����")]
    private float runAwayDistance = 20f;
    private float idleStartTime = -1f;

    [Header("źȯ")]
    public GameObject[] projectilePrefab;

    public Transform firePoint;

    [Header("UI")]
    public GameObject HpSlider;
    private EnemyHP_S healthUI;

    public float lastAttackTime;

    [Header("ä��")]
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

                // RunAway ���� ���� �ð� ������ �����
                if (idleStartTime > 0 && Time.time - idleStartTime >= 5f)
                {
                    Die();
                }
                break;


            case EnemyState.Trace:
                // �÷��̾ ���� �̵�
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
                // ���� ��Ÿ�� üũ �� �߻�
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    lastAttackTime = Time.time;
                    AttackPlayer();
                }

                // ���� ������ ����� �ٽ� ����
                if (distanceToPlayer > attackRange)
                {
                    state = EnemyState.Trace;
                }
                break;

            case EnemyState.RunAway:
                Vector3 awayDirection = (transform.position - player.position).normalized;
                transform.Translate(awayDirection * speed * Time.deltaTime, Space.World);
                transform.LookAt(transform.position + awayDirection);

                // ���� �Ÿ� �̻� �����ƴ��� Ȯ��
                if (Vector3.Distance(transform.position, player.position) > runAwayDistance)
                {
                    state = EnemyState.Idel;
                    idleStartTime = Time.time; // Idel ���� ���� �ð� ���
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
            transform.LookAt(player); // �÷��̾ �ٶ󺸵��� �߰�

            int index = Random.Range(0, projectilePrefab.Length);
            GameObject bullet = Instantiate(projectilePrefab[index], firePoint.position, firePoint.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.owner = BulletOwner.Enemy; // �Ѿ� ������ ����
            }
        }
    }


    public void TakeDamage(float damage)
    {
        helth -= damage;
        Debug.Log("���� �� ä��: " + helth);

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