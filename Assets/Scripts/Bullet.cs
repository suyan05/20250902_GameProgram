using UnityEngine;

public enum BulletOwner
{
    Player,
    Enemy
}

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 2f;
    public float Bulletdamage = 1f;
    public BulletOwner owner; // 외부에서 설정할 총알의 소유자

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (owner)
        {
            case BulletOwner.Player:
                if (other.CompareTag("Enemy"))
                {
                    Enemy_Base enemy = other.GetComponent<Enemy_Base>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(Bulletdamage);
                    }
                    Destroy(gameObject);
                }
                break;

            case BulletOwner.Enemy:
                if (other.CompareTag("Player"))
                {
                    PlayerController player = other.GetComponent<PlayerController>();
                    if (player != null)
                    {
                        player.InDamage(Bulletdamage);
                    }
                    Destroy(gameObject);
                }
                break;
        }
    }
}