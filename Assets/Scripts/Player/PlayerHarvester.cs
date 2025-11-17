using UnityEngine;

public class PlayerHarvester : MonoBehaviour
{
    public float rayDistnace = 5f;
    public LayerMask hitMask = ~0;
    public int toolDamage = 1;
    public float hitCooldown = 0.15f;

    private float _nextHitTimer;
    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= _nextHitTimer)
        {
            _nextHitTimer = Time.time + hitCooldown;

            Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out var hit, rayDistnace, hitMask))
            {
                var block = hit.collider.GetComponent<Blocks>();
                if (block != null)
                {
                    block.Hit(toolDamage);
                }
            }
        }
    }
}