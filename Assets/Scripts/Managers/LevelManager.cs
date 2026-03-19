using System.Collections;
using System.IO.IsolatedStorage;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    public enum GameState { Playing, Paused, Option, Countdown, GameOver, Clear, Fading }
    public GameState currentState = GameState.Playing;

    [Header("설정")]
    // 현재 스테이지 번호와 총 패턴 시간 설정해주기. 스테이지마다 다르므로.
    public int curStageNum;
    public float curStageTotalTime;

    [Header("윈도우")]
    // 각종 윈도우들
    public GameObject pauseWindow;
    public GameObject optionWindow;
    public GameObject dieWindow;
    public GameObject stageClearWindow;

    [Header("UI 이미지")]
    public GameObject[] countDownTexts;
    public GameObject[] hps;
    public GameObject fadeIn;
    public GameObject fadeOut;

    [Header("텍스트")]
    public TextMeshProUGUI clearLivingTimeText;
    public TextMeshProUGUI dieLivingTimeText;
    public TextMeshProUGUI hitCountText;
    public TextMeshProUGUI stageClearText;

    [Header("사운드")]
    public AudioClip stageClearSFX;
    public AudioClip DieSFX;

    int hitCount = 0;
    float fadeTime = 1f;

    // PlayerPref 저장을 위해 해당 스테이지 번호에 따라 생성되는 Key.
    string curStageNumStr;

    // 참조
    public static LevelManager instance;


    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        // 윈도우들 초기화
        pauseWindow.SetActive(false);
        optionWindow.SetActive(false);
        stageClearWindow.SetActive(false);
        dieWindow.SetActive(false);
        fadeIn.SetActive(true);

        Time.timeScale = 1f;
        curStageNumStr = "Stage" + curStageNum + "Progress";
        UpdateHP(PlayerHP.instance.curHP, isDamaged : false);
    }

    private void Update()
    {
        // ESC를 눌렀을 때 (카운트 다운 상태가 아니어야함.)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleInput();
        }
    }
    void HandleInput()
    {
        switch (currentState)
        {
            case GameState.Playing:
                PauseGame();
                break;
            case GameState.Paused:
                ResumeGame();
                break;
            case GameState.Option:
                CloseOption();
                break;
            // 카운트다운, 게임오버, 페이드 중일 때는 ESC 무시
        }
    }

    //----------------------- 게임 흐름 제어 ------------------------
    // 일시 정지
    public void PauseGame()
    {
        currentState = GameState.Paused;
        Time.timeScale = 0f;
        AudioManager.instance.PauseLoop();
        if (PlayerController.instance != null) PlayerController.instance.isStopped = true;
        pauseWindow.SetActive(true);
    }
    // 게임 재개
    public void ResumeGame()
    {
        pauseWindow.SetActive(false);
        StartCoroutine(ProcessCountDown());
    }
    public void OpenOption()
    {
        currentState = GameState.Option;
        pauseWindow.SetActive(false);
        optionWindow.SetActive(true);
    }

    public void CloseOption()
    {
        currentState = GameState.Paused;
        pauseWindow.SetActive(true);
        optionWindow.SetActive(false);
    }

    // 카운트다운 3초 이후 게임 재개
    IEnumerator ProcessCountDown()
    {
        currentState = GameState.Countdown;

        foreach (var text in countDownTexts)
        {
            text.gameObject.SetActive(true);
            text.transform.localScale = Vector3.one * 5f;
            text.transform.DOScale(0.3f, 1f).SetUpdate(true); // 1초동안 크기 0으로 만들어라
            yield return new WaitForSecondsRealtime(1f);
            text.gameObject.SetActive(false);
        }

        // 게임 재개
        Time.timeScale = 1f;
        currentState = GameState.Playing;
        AudioManager.instance.ResumeLoop();
        if (PlayerController.instance != null) PlayerController.instance.isStopped = false;
    }

    // ----------------- 씬 전환 관련 -----------------
    IEnumerator ProcessSceneChange(int sceneIndex)
    {
        currentState = GameState.Fading;
        SavePrefs(GameStageTimer.instance.progress);
        if (fadeOut != null) fadeOut.SetActive(true);
        if (PlayerHP.instance != null) PlayerHP.instance.isInvincible = true;
        Time.timeScale = 1f;
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneIndex);
    }

    // 목표 씬 번호를 매개변수로 메인메뉴 or 재시작
    public void LoadMainMenu() => StartCoroutine(ProcessSceneChange(0));

    public void Restart() => StartCoroutine(ProcessSceneChange(curStageNum));

    //----------------- 체력 변동 UI -----------------
    // PlayerHP에서 체력 변동 시 아래 함수 실행.
    // Action 활용하려 했으나 왜인지 발동이 안돼 수동 실행.
    public void UpdateHP(int curHP, bool isDamaged = true)
    {
        if (isDamaged) hitCount++;
        for (int i = 0; i < hps.Length; i++)
        {
            Image img = hps[i].GetComponent<Image>();
            if (i < curHP)
            {
                if (img.color.a <= 0.01f)
                    hps[i].GetComponent<Image>()?.DOFade(1f, 0.3f);
            }
            else if (hps[i].activeSelf) // 켜져있던 걸 끌 때만 연출 실행
            {
                if (img.color.a >= 0.01)
                    hps[i].GetComponent<Image>()?.DOFade(0f, 0.3f);
            }
        }
    }

    // ----------------- 캐릭터 사망 시 -----------------
    // PlayerHP에서 체력 0이 되면 아래 함수 호출
    public void ProcessDie() => StartCoroutine(ProcessDie1());
    IEnumerator ProcessDie1()
    {
        // 죽는 순간 해당 스테이지의 Prefs 저장.
        SavePrefs(GameStageTimer.instance.progress);

        // 슬로우 모션 발동 후 게임 정지
        Time.timeScale = 0.3f;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 0f;
        AudioManager.instance.PlaySFX(DieSFX, 1f);
        dieWindow.SetActive(true);
        UpgradeManager.instance.AddOnePoints();

        // 생존 시간 텍스트 업데이트
        int min = (int)GameStageTimer.instance.currentTime / 60;
        int sec = (int)GameStageTimer.instance.currentTime % 60;
        dieLivingTimeText.text = $"{min}분 {sec:D02}초";
    }

    // ----------------- 스테이지 클리어 시 -----------------
    // GameStageTimer에서 100% 달성 시 해당 함수를 실행.
    public void ProcessClear() => StartCoroutine(ProcessClear1());
    IEnumerator ProcessClear1()
    {
        // 플레이어 Pref 저장하기
        SavePrefs(100);

        // 2초간 슬로우 모션 후 무적 키고 사운드 재생하고 윈도우 키기
        Time.timeScale = 0.3f;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 0f;
        AudioManager.instance.PlaySFX(stageClearSFX, 1f);
        stageClearWindow.SetActive(true);

        // 게임 클리어 텍스트 업데이트
        int min = (int)curStageTotalTime / 60;
        int sec = (int)curStageTotalTime % 60;
        clearLivingTimeText.text = $"{min}분 {sec:D2}초";
        hitCountText.text = hitCount + "회";
        stageClearText.text = "Stage " + curStageNum + " Clear!!";
    }
    void SavePrefs(int prog)
    {
        // 최고 기록보다 현재 저장하려는 기록이 더 높아야만 저장.
        if (PlayerPrefs.GetInt(curStageNumStr, 0) < prog)
        {
            PlayerPrefs.SetInt(curStageNumStr, prog);
            PlayerPrefs.Save();
        }
    }
}