using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UIElements;

public class MainPatternManager : MonoBehaviour
{
    public GameObject[] patterns;
    public List<GameObject> tempPatterns;

    // Main Pattern들 전부 비활성화 한 상태로 게임 시작
    private void Awake()
    {
        for (int i = 0; i < patterns.Length; i++)
        {
            patterns[i].SetActive(false);
        }
    }
    void Start()
    {
        RefillPatterns();
        // 게임 시작 5초 후부터 Main Pattern들 시작
        StartCoroutine(WaitAndLoad(5f));
    }

    // Main Pattern들 중 랜덤으로 하나를 뽑아 백그라운드에 재생시키는 함수.
    // 중복 최소화를 위해 List에서 하나씩 제거하고, Empty하면 다시 Refill해서 랜덤화.
    public void  LoadPattern()
    {
        if (tempPatterns.Count <= 0) RefillPatterns();

        int randomIndex = UnityEngine.Random.Range(0, tempPatterns.Count);

        BasePattern patternScript = tempPatterns[randomIndex].GetComponent<BasePattern>();
        if (patternScript != null)
        {
            patternScript.onFinished = () => StartCoroutine(WaitAndLoad(0.1f));
        }
        else
        {
            Debug.Log("해당 패턴에 스크립트가 안붙어있음.");
        }
        tempPatterns[randomIndex].SetActive(true);
        tempPatterns.RemoveAt(randomIndex);
    }

    //패턴 싸이클 이후 리스트가 비면 다시 꽉채우는 함수
    public void RefillPatterns()
    {
        for (int i = 0; i < patterns.Length; i++)
        {
            tempPatterns.Add(patterns[i]);
        }
    }

    //waitTime 만큼 기다린 후 LoadPattern() 호출
    IEnumerator WaitAndLoad(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        LoadPattern();
    }
}
