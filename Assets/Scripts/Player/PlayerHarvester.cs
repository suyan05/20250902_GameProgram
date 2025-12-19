using UnityEngine;

public class PlayerHarvester : MonoBehaviour
{
    [Header("Raycast 설정")]
    public float rayDistance = 5f;
    public LayerMask hitMask = ~0;

    [Header("채굴 설정")]
    public int toolDamage = 1;
    public float hitCooldown = 0.15f;

    private float _nextHitTimer;
    private Camera _cam;
    private bool buildMode = false;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        // 우클릭으로 설치 모드 토글
        if (Input.GetMouseButtonDown(1))
        {
            buildMode = !buildMode;
        }

        // 좌클릭 입력 처리
        if (Input.GetMouseButtonDown(0) && Time.time >= _nextHitTimer)
        {
            _nextHitTimer = Time.time + hitCooldown;

            Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out var hit, rayDistance, hitMask))
            {
                if (buildMode)
                {
                    PlaceBlock(hit);
                }
                else
                {
                    var block = hit.collider.GetComponent<Blocks>();
                    if (block != null)
                    {
                        int finalDamage = toolDamage;

                        // 현재 선택된 핫바 슬롯 확인
                        var slot = InventoryManager.Instance.hotbarSlots[InventoryManager.Instance.selectedHotbarIndex];
                        if (slot != null && slot.type != ItemType.Empty)
                        {
                            finalDamage += DamageModifier.Instance.GetExtraDamage(slot.type);
                        }

                        block.Hit(finalDamage);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 외부에서 설치 모드 제어 (예: 인벤토리 열릴 때)
    /// </summary>
    public void SetBuildMode(bool active)
    {
        buildMode = active;
    }

    /// <summary>
    /// 실제 블록 설치 (기존 블록 위/옆에 붙여서)
    /// </summary>
    private void PlaceBlock(RaycastHit hit)
    {
        var slot = InventoryManager.Instance.hotbarSlots[InventoryManager.Instance.selectedHotbarIndex];
        if (slot == null || slot.type == ItemType.Empty || slot.count <= 0) return;

        // 충돌 지점 + 법선 방향으로 약간 밀어줌
        Vector3 placePos = hit.point + hit.normal * 0.5f;

        // 격자에 맞게 반올림
        placePos = new Vector3(
            Mathf.RoundToInt(placePos.x),
            Mathf.RoundToInt(placePos.y),
            Mathf.RoundToInt(placePos.z)
        );

        placePos -= new Vector3(0.0f, 1f, 0.0f);

        GameObject prefab = NoiseVoxelMap.Instance.blockPrefabs[(int)slot.type];
        if (prefab != null)
        {
            Instantiate(prefab, placePos, Quaternion.identity, NoiseVoxelMap.Instance.transform);

            // 인벤토리에서 아이템 소모
            InventoryManager.Instance.Remove(slot.type, 1);
        }
    }
}