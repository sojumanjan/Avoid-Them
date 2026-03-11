using DG.Tweening;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("설정")]
    public float hoverScale = 1.1f;     // 얼마나 커질지 (1.1배)
    public float hoverDuration;              // 버튼 커지는 시간
    public AnimationCurve scaleCurve;

    private Vector3 defaultScale;      // 원래 크기 저장용
    private Vector3 targetScale;       // 목표 크기

    Button btn;
    void Awake()
    {
        defaultScale = transform.localScale;
        targetScale = defaultScale;
    }
    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => AudioManager.instance.PlayClickSFX());
    }

    // 마우스가 버튼 위로 올라왔을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!btn.interactable) return; // 버튼 꺼져있으면 반응 X

        transform.DOKill();
        transform.DOScale(defaultScale * hoverScale, hoverDuration).SetUpdate(true);
    }

    // 마우스가 버튼 밖으로 나갔을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!btn.interactable) return;

        // 다시 원래 크기로 복구
        transform.DOKill();
        transform.DOScale(defaultScale, hoverDuration).SetUpdate(true);
    }

    // 스테이지 버튼을 클릭했을 때만 한정 실행
    public void OnStageClick(int sceneIndex)
    {
        Button btn = GetComponent<Button>();
        // 중복 클릭 방지
        if (btn != null) btn.interactable = false;
        

        transform.DOKill(); // 호버링 중이던 거 멈추고
        transform.DOScale(Vector3.zero, 1.3f)
            .SetEase(scaleCurve) // 님이 만든 애니메이션 커브 적용!
            .SetUpdate(true);

        if (UIManager.instance != null)
        {
            UIManager.instance.LoadStageScene(sceneIndex);
        }
    }

    // 버튼이 클릭 등으로 비활성화 되었다가 켜질 때 크기 초기화
    void OnEnable()
    {
        transform.localScale = defaultScale;
        transform.DOKill();
        GetComponent<Button>().interactable = true;
    }
    void OnDisable()
    {
        transform.DOKill();
    }
}