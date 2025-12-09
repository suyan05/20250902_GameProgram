using UnityEngine;

public class PlayerHarvester : MonoBehaviour
{
    public float rayDistance = 5f;
    public LayerMask hitMask = ~0;
    public int toolDamage = 1;
    public float hitCooldown = 0.15f;

    private float _nextHitTimer;
    private Camera _cam;
    private bool buildMode = false;

    public void SetBuildMode(bool active)
    {
        buildMode = active;
    }

    private GameObject previewObject;

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
            if (!buildMode && previewObject != null)
            {
                Destroy(previewObject);
            }
        }

        // 설치 모드일 때 프리뷰 표시
        if (buildMode)
        {
            ShowPreview();
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

    private void ShowPreview()
    {
        var slot = InventoryManager.Instance.hotbarSlots[InventoryManager.Instance.selectedHotbarIndex];
        if (slot == null || slot.type == ItemType.Empty || slot.count <= 0) return;

        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out var hit, rayDistance, hitMask))
        {
            Vector3 placePos = hit.point + hit.normal * 0.5f;
            placePos = new Vector3(Mathf.Round(placePos.x), Mathf.Round(placePos.y), Mathf.Round(placePos.z));

            if (previewObject == null)
            {
                GameObject prefab = NoiseVoxelMap.Instance.blockPrefabs[(int)slot.type];
                previewObject = Instantiate(prefab, placePos, Quaternion.identity);
                SetTransparent(previewObject, 0.1f);
            }
            else
            {
                previewObject.transform.position = placePos;
            }
        }
    }

    private void PlaceBlock(RaycastHit hit)
    {
        var slot = InventoryManager.Instance.hotbarSlots[InventoryManager.Instance.selectedHotbarIndex];
        if (slot == null || slot.type == ItemType.Empty || slot.count <= 0) return;

        Vector3 placePos = hit.point + hit.normal * 0.5f;
        placePos = new Vector3(Mathf.Round(placePos.x), Mathf.Round(placePos.y), Mathf.Round(placePos.z));

        GameObject prefab = NoiseVoxelMap.Instance.blockPrefabs[(int)slot.type];
        if (prefab != null)
        {
            Instantiate(prefab, placePos, Quaternion.identity, NoiseVoxelMap.Instance.transform);

            slot.count--;
            if (slot.count <= 0) slot.Clear();
            else
            {
                slot.countText.text = slot.count > 1 ? slot.count.ToString() : "";
                slot.countText.enabled = slot.count > 1;
            }
        }
    }

    private void SetTransparent(GameObject obj, float alpha)
    {
        foreach (var renderer in obj.GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in renderer.materials)
            {
                Color c = mat.color;
                c.a = alpha;
                mat.color = c;
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.renderQueue = 3000;
            }
        }
    }
}