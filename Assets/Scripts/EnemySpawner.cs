using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;       // 생성할 적 프리팹
    public Transform player;             // 플레이어 위치
    public float spawnRadius = 10f;      // 플레이어 기준 생성 반경
    public float spawnInterval = 4.5f;     // 생성 간격 (초)

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (player == null || enemyPrefab == null) return;

        // 랜덤 위치 계산
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(
            player.position.x + randomCircle.x,
            player.position.y,
            player.position.z + randomCircle.y
        );

        // 적 생성
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}