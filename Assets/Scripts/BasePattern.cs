using UnityEngine;
using System;
using System.Collections;

public class BasePattern : MonoBehaviour
{
    // 매니저가 여기에 "끝나면 실행할 함수"를 넣어줄 겁니다.
    public Action onFinished;

    // 1. 켜지자마자 패턴 시작 (매니저가 SetActive(true) 하면 자동 실행)
    protected virtual void OnEnable()
    {
        StartCoroutine(ProcessPattern());
    }
    //
    //패턴이 끝나고 비활성화 될 때 실행되는 함수. 더 많은 기능이 필요하면 자식에서 구현.
    protected virtual void OnDisable()
    {
        onFinished = null;
        StopAllCoroutines();
    }

    // 자식들이 내용을 채워넣을 함수 (가상 함수)
    protected virtual IEnumerator ProcessPattern()
    {
        yield return null;
    }

    // 패턴이 끝났을 때 호출하는 함수
    protected void FinishPattern()
    {
        // 매니저한테 "저 끝났어요!" 보고
        onFinished?.Invoke();

        // (선택 사항) 스스로 꺼지기
        gameObject.SetActive(false);
    }
}