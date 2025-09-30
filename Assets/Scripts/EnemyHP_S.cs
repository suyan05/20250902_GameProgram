using UnityEngine;
using UnityEngine.UI;

public class EnemyHP_S : MonoBehaviour
{
    public Enemy_Base enemy; // ����� ��
    public Slider healthSlider; // Slider ����
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (enemy == null) return;

        // ü�� ���� ������Ʈ
        healthSlider.value = enemy.GetHealthPercent();

        // �� ��ġ�� ȭ�� ��ǥ�� ��ȯ
        Vector3 screenPos = mainCamera.WorldToScreenPoint(enemy.transform.position + Vector3.up * 2f);
        transform.position = screenPos;
    }
}
