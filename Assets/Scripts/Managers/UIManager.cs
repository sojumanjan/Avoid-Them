using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

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
}
