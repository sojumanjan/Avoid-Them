using UnityEngine;
using TMPro; // 텍스트용
using UnityEngine.UI;
using JetBrains.Annotations; // 슬라이더(게이지)용

public class GameStageTimer : MonoBehaviour
{
    public static GameStageTimer instance;

    [Header("설정")]
    public float maxStageTime; // 전체 스테이지 시간 (초 단위)

    [Header("할당")]
    TextMeshProUGUI percentText; // "50%" 표시할 텍스트

    public float currentTime = 0f;
    public int progress;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        percentText = GetComponent<TextMeshProUGUI>();
        maxStageTime = LevelManager.instance.curStageTotalTime;
    }
    private bool isPlaying = true;

    void Update()
    {
        if (isPlaying && LevelManager.instance.currentState != LevelManager.GameState.Paused)
        {
            currentTime += Time.deltaTime;

            // 비율 계산 (0.0 ~ 1.0)
            float ratio = currentTime / maxStageTime;
            progress = (int)(ratio * 100);
            // 100% 넘어가면 100%로 고정하고 게임 클리어 처리
            if (ratio >= 1f)
            {
                ratio = 1f;
                isPlaying = false; // 시간 멈춤
                LevelManager.instance.ProcessClear();
            }

            // 텍스트 갱신 (소수점 없이 정수로: F0)
            if (percentText != null)
            {
                // ratio가 0.5면 -> 50%
                percentText.text = string.Format("{0:F1}%", ratio * 100);
            }
        }
    }

    // 랜덤 타임을 쓰는 패턴의 경우 오차를 보정해주기 위해 쓰는 함수. (Stage2Pattern1)
    public void UpdateMaxStageTime(float time)
    {
        maxStageTime += time;
    }
}