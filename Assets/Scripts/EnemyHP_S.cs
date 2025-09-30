using UnityEngine;
using UnityEngine.UI;

public class EnemyHP_S : MonoBehaviour
{
    public Enemy_Base enemy; // 연결된 적
    public Slider healthSlider; // Slider 참조
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (enemy == null) return;

        // 체력 비율 업데이트
        healthSlider.value = enemy.GetHealthPercent();

        // 적 위치를 화면 좌표로 변환
        Vector3 screenPos = mainCamera.WorldToScreenPoint(enemy.transform.position + Vector3.up * 2f);
        transform.position = screenPos;
    }
}
