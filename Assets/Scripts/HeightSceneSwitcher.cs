using UnityEngine;
using UnityEngine.SceneManagement;

public class HeightSceneSwitcher : MonoBehaviour
{
    [Header("전환 조건")]
    public float heightThreshold = 100f;      // 이 높이 이상 올라가면 전환
    public string targetSceneName = "NextScene"; // 이동할 씬 이름

    [Header("안전 설정")]
    public bool onlyOnce = true;              // 한 번만 전환할지
    private bool _switched;

    private void Update()
    {
        if (_switched && onlyOnce) return;

        if (transform.position.y >= heightThreshold)
        {
            _switched = true;
            SceneManager.LoadScene(targetSceneName);
        }
    }
}