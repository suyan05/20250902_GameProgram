using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;       // ������ �� ������
    public Transform player;             // �÷��̾� ��ġ
    public float spawnRadius = 10f;      // �÷��̾� ���� ���� �ݰ�
    public float spawnInterval = 4.5f;     // ���� ���� (��)

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (player == null || enemyPrefab == null) return;

        // ���� ��ġ ���
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(
            player.position.x + randomCircle.x,
            player.position.y,
            player.position.z + randomCircle.y
        );

        // �� ����
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}