using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject[] bulletPrefabs; // 여러 무기 프리팹을 배열로 관리
    public Transform firePoint;

    private Camera cam;
    private int currentWeaponIndex = 0; // 현재 선택된 무기 인덱스

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        // 마우스 클릭 시 발사
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        // Z 키를 누르면 무기 전환
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SwitchWeapon();
        }
    }

    private void Shoot()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint = ray.GetPoint(50f);
        Vector3 direction = (targetPoint - firePoint.position).normalized;

        Instantiate(bulletPrefabs[currentWeaponIndex], firePoint.position, Quaternion.LookRotation(direction));
    }

    private void SwitchWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % bulletPrefabs.Length;
    }
}