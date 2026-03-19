using DG.Tweening;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [Header("윈도우")]
    public GameObject mainMenuWindow;
    public GameObject stageSelectWindow;
    public GameObject optionWindow;
    public GameObject upgradeWindow;
    [Header("페이드")]
    public Image fadeImage;
    public float fadeTime = 1f;
    [Header("텍스트 및 사운드")]
    public AudioClip stageClickSFX;
    public TextMeshProUGUI stage1Text;
    public TextMeshProUGUI stage2Text;
    public TextMeshProUGUI curPointsText;
    public Button hpBtn;
    public Button shieldBtn;
    public Button invincibilityBtn;
    public Button reviveBtn;
    public Button timestopBtn;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    private void Start()
    {
        OpenMainMenuWindow();
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(0f, fadeTime)
                .OnComplete(() => fadeImage.gameObject.SetActive(false));
        Time.timeScale = 1f;
    }

    // 메인메뉴 윈도우 오픈
    public void OpenMainMenuWindow()
    {
        stageSelectWindow.SetActive(false);
        optionWindow.SetActive(false);
        upgradeWindow.SetActive(false);
        mainMenuWindow.SetActive(true);
    }

    // 스테이지 선택 윈도우 오픈
    public void OpenStageSelectWindow()
    {
        UpdateProgress();
        mainMenuWindow.SetActive(false);
        stageSelectWindow.SetActive(true);
    }

    // 옵션 윈도우 오픈
    public void OpenOptionWindow()
    {
        mainMenuWindow.SetActive(false);
        optionWindow.SetActive(true);
    }

    // 업그레이드 윈도우 오픈

    public void OpenUpgradeWindow()
    {
        mainMenuWindow.SetActive(false);
        upgradeWindow.SetActive(true);
        UpdateUpgradeState();
    }
    public void LoadStageScene(int sceneIndex)
    {
        StartCoroutine(LoadStageSceneWithFade(sceneIndex));
    }
    // 스테이지 씬 로드
    IEnumerator LoadStageSceneWithFade(int sceneIndex)
    {
        fadeImage.gameObject.SetActive(true);
        AudioManager.instance.PlaySFX(stageClickSFX, 1f);
        // 버튼 페이드 대기 시간 0.3초
        yield return new WaitForSeconds(0.3f);
        fadeImage.DOFade(1f, fadeTime)
            .OnComplete(() => SceneManager.LoadScene(sceneIndex));
    }

    // 게임 종료
    public void ExitGame()
    {
        Application.Quit();
    }
    
    void UpdateProgress()
    {
        int stage1Progress = PlayerPrefs.GetInt("Stage1Progress", 0);
        int stage2Progress = PlayerPrefs.GetInt("Stage2Progress", 0);
        stage1Text.text = stage1Progress + "%";
        stage2Text.text = stage2Progress + "%";
    }

    public void UpdateUpgradeState()
    {
        // 보유 포인트 텍스트 갱신
        int currentPoints = PlayerPrefs.GetInt("curPoints", 0);
        curPointsText.text = $"보유 포인트\n{currentPoints}pts";

        // 각 버튼별 상태 업데이트
        SetButtonAlpha(hpBtn, "hpState");
        SetButtonAlpha(shieldBtn, "shieldState");
        SetButtonAlpha(invincibilityBtn, "invincibilityState");
        SetButtonAlpha(reviveBtn, "reviveState");
        SetButtonAlpha(timestopBtn, "timestopState");
    }

    // 버튼과 자식 텍스트의 투명도를 한 번에 조절
    private void SetButtonAlpha(Button btn, string prefsKey)
    {
        // PlayerPrefs 값이 1 이상이면 구매한 것으로 간주
        bool isPurchased = PlayerPrefs.GetInt(prefsKey, 0) >= 1;
        float targetAlpha = isPurchased ? 0.2f : 1f;

        // 버튼 이미지 투명도 조절
        Image btnImg = btn.GetComponent<Image>();
        if (btnImg != null)
        {
            Color c = btnImg.color;
            c.a = targetAlpha;
            btnImg.color = c;
        }

        // 자식 텍스트 투명도 조절
        TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
        if (btnText != null)
        {
            Color tc = btnText.color;
            tc.a = targetAlpha;
            btnText.color = tc;

            // 구매했으면 텍스트 변경
            if (isPurchased) btnText.text = "구매 완료";
            else btnText.text = "3pts";
        }

        // 이미 샀다면 버튼 클릭 비활성화
        btn.interactable = !isPurchased;
    }
}
