using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 2f;
    public int Bulletdamage = 1;

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
        if (other.CompareTag("Enemy"))
        {
            Enemy_Base enemy = other.GetComponent<Enemy_Base>();
            if (enemy != null)
            {
                enemy.TakeDamage(Bulletdamage); // 체력 1 감소
            }
            Destroy(gameObject); // 총알 제거
        }
    }
}