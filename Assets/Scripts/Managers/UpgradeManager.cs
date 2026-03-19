using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    [Header("현재 보유 포인트")]
    public int curPoints = 0;

    [Header("업그레이드 상태 (현재 레벨)")]
    public int hpState = 0;
    public int shieldState = 0;
    public int invincibilityState = 0;
    public int reviveState = 0;
    public int timestopState = 0;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            LoadUpgradeData();
        }
        else Destroy(gameObject);
    }

    // 데이터를 로드하는 함수
    public void LoadUpgradeData()
    {
        hpState = PlayerPrefs.GetInt("hpState", 0);
        shieldState = PlayerPrefs.GetInt("shieldState", 0);
        invincibilityState = PlayerPrefs.GetInt("invincibilityState", 0);
        reviveState = PlayerPrefs.GetInt("reviveState", 0);
        timestopState = PlayerPrefs.GetInt("timestopState", 0);
        curPoints = PlayerPrefs.GetInt("curPoints", 0);

    }

    // 업그레이드 구매 함수 (업그레이드 버튼 클릭 시 호출)
    public void Upgrade(string upgradeState)
    {
        // 이미 구매했다면
        if (PlayerPrefs.GetInt(upgradeState, 0) == 1)
        {
            Debug.Log("이미 구매한 업그레이드입니다.");
            return;
        }
        // 포인트가 충분하다면
        if (PlayerPrefs.GetInt("curPoints", 0) >= 3)
        {
            curPoints -= 3;
            PlayerPrefs.SetInt("curPoints", curPoints);
            PlayerPrefs.SetInt(upgradeState, 1);
            PlayerPrefs.Save();
            LoadUpgradeData();
            UIManager.instance.UpdateUpgradeState();
            Debug.Log("구매 완료했습니다.");
        }
        // 포인트가 부족하다면
        else
        {
            Debug.Log("포인트가 부족하여 구매하지 못합니다.");
        }
    }

    public void AddOnePoints()
    {
        curPoints++;
        PlayerPrefs.SetInt("curPoints", curPoints);
        PlayerPrefs.Save();
        LoadUpgradeData();
        UIManager.instance.UpdateUpgradeState();
    }

    public void MinusOnePoints()
    {
        curPoints--;
        PlayerPrefs.SetInt("curPoints", curPoints);
        PlayerPrefs.Save();
        LoadUpgradeData();
        UIManager.instance.UpdateUpgradeState();
    }

    public void ResetAllState()
    {
        PlayerPrefs.SetInt("hpState", 0);
        PlayerPrefs.SetInt("shieldState", 0);
        PlayerPrefs.SetInt("invincibilityState", 0);
        PlayerPrefs.SetInt("reviveState", 0);
        PlayerPrefs.SetInt("timestopState", 0);
        PlayerPrefs.SetInt("curPoints", 0);
        LoadUpgradeData();
        UIManager.instance.UpdateUpgradeState();
    }
}