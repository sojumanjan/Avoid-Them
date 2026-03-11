using System.Collections;
using UnityEngine;

public class MainPattern1 : BasePattern
{
    
    public float rotationTime; //돌아가는 시간
    public GameObject choo;
    public Vector3 targetRotate; //목표 로테이션 값
    public Vector2[] spawnPosition;
    protected override void OnEnable()
    {
        StartCoroutine(ProcessPattern());

        //회전 값 및 스폰위치 값 초기화
        choo.transform.rotation = Quaternion.Euler(0, 0, 0);
        choo.transform.position = spawnPosition[Random.Range(0, spawnPosition.Length)];
    }
    public void StartRotation(float duration)
    {
        StartCoroutine(RotateRoutine(duration, targetRotate));
    }

    IEnumerator RotateRoutine(float duration, Vector3 targetRot)
    {
        float timeElapsed = 0f;

        // 현재 회전값
        Quaternion startRotation = choo.transform.rotation;

        // 목표 회전값 (예: Y축 180도)
        Quaternion targetRotation = Quaternion.Euler(targetRot);

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;

            // 0 ~ 1 사이의 진행도 (t)
            float t = timeElapsed / duration;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            // 보간(Lerp) 적용
            choo.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, smoothT);

            yield return null; // 한 프레임 대기
        }

        // 오차 보정 (마지막에 정확히 180도로 딱 맞춰줌)
        choo.transform.rotation = targetRotation;
        Vector3 nextLotation = new Vector3(0, 0, 0);
        StartCoroutine(RotateRoutine(rotationTime, nextLotation));
    }

    protected override IEnumerator ProcessPattern()
    {
        Debug.Log("MainPattern1 실행됨.");
        StartCoroutine(RotateRoutine(rotationTime, targetRotate));
        yield return new WaitForSeconds(rotationTime * 2f);
        FinishPattern();
    }
}
