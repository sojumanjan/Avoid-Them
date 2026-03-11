using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    [Header("설정")]
    public Slider slider;
    // BGM인지 SFX인지 인스펙터에서 설정. 값이 바뀌지 않음.
    public bool isBGM;
    bool isMute = false;

    [Header("연결 대상")]
    public Image targetImage; // 그림이 바뀔 본체 (보통 자기 자신)

    [Header("아이콘 설정")]
    public Sprite onSprite;   // 켜졌을 때 그림 (소리 아이콘)
    public Sprite offSprite;  // 꺼졌을 때 그림 (X 아이콘)

    float originSound;

    private void Start()
    {
        // 오디오 매니저 값 불러와 소리 세팅
        slider.value = isBGM ? AudioManager.instance.BGMVolume : AudioManager.instance.SFXVolume;
        originSound = slider.value;
        UpdateImage();
    }

    private void OnDisable()
    {
        // 저장을 버튼이 비활성화 될때만
        PlayerPrefs.Save();
    }

    public void ToggleMute()
    {
        // 원래 뮤트상태였다면 원래 사운드 크기로 돌아가기
        isMute = !isMute;
        if (isBGM)
        {
            AudioManager.instance.SetBGMVolume(isMute ? 0f : originSound);
        }
        else
        {
            AudioManager.instance.SetSFXVolume(isMute ? 0f : originSound);
        }
        UpdateImage();
    }

    void UpdateImage()
    {
        if (targetImage != null)
        {
            targetImage.sprite = isMute ? offSprite : onSprite;
        }
    }

    // 소리 슬라이더가 움직이면 호출되는 함수
    public void SetVolume()
    {
        if (isMute)
        {
            isMute = !isMute;
            UpdateImage();
        }
        if (isBGM)
        {
            AudioManager.instance.SetBGMVolume(slider.value);
        }
        else
        {
            AudioManager.instance.SetSFXVolume(slider.value);
        }
        originSound = slider.value;
    }

    public void PlayClickSound()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.PlayClickSFX();
    }
}
