using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UIElements;

public class Stage1PatternManager : MonoBehaviour
{
    public GameObject[] patterns;
    private int currentPatternIndex = 0;

    private void Awake()
    {
        for (int i = 0; i < patterns.Length; i++)
        {
            patterns[i].SetActive(false);
        }
    }
    void Start()
    {
        // ���� ���� 5�� �ĺ��� Main Pattern�� ����
        StartCoroutine(WaitAndLoad(1f));
    }

    public void LoadPattern()
    {
        // 더 이상 실행할 패턴이 없으면 종료
        if (currentPatternIndex >= patterns.Length)
        {
            Debug.Log("🎉 스테이지 1 클리어! (모든 패턴 종료)");
            return;
        }

        GameObject patternObj = patterns[currentPatternIndex];

        // 패턴 오브젝트가 비어있지 않은지 확인
        if (patternObj != null)
        {
            BasePattern patternScript = patternObj.GetComponent<BasePattern>();

            if (patternScript != null)
            {
                Debug.Log($"▶ 패턴 {currentPatternIndex + 1} 시작: {patternObj.name}");

                // 패턴이 끝났을 때 실행할 행동(콜백) 정의
                patternScript.onFinished = () =>
                {
                    currentPatternIndex++; // 다음 순번으로 넘어감
                    // 패턴 사이의 휴식 시간 (예: 2초) 후 다음 패턴 로드
                    StartCoroutine(WaitAndLoad(1.0f));
                };

                // ★ 핵심: 오브젝트를 켜면 BasePattern의 OnEnable이 돌면서 자동 시작됨
                patternObj.SetActive(true);
            }
            else
            {
                Debug.LogError($"[오류] {patternObj.name}에 BasePattern 스크립트가 없습니다!");
            }
        }
        else
        {
            Debug.LogError($"[오류] {currentPatternIndex}번 패턴 슬롯이 비어있습니다!");
        }
    }


    IEnumerator WaitAndLoad(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        LoadPattern();
    }
}
