using UnityEngine;
using UnityEngine.UI;

public class UIClickSound : MonoBehaviour
{
    public AudioClip clickSFX;

    private void Start()
    {
        Button btn = GetComponent<Button>();

        // 클릭 리스너에 소리 재생 추가
    }
    public void PlayClickSFX()
    {
        AudioManager.instance.PlaySFX(clickSFX, 1f);
    }
}
