using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject[] bulletPrefabs; // ���� ���� �������� �迭�� ����
    public Transform firePoint;

    private Camera cam;
    private int currentWeaponIndex = 0; // ���� ���õ� ���� �ε���

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        // ���콺 Ŭ�� �� �߻�
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        // Z Ű�� ������ ���� ��ȯ
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