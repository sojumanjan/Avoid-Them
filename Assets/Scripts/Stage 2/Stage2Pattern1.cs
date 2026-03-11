using System.Collections;
using UnityEngine;

// 1자 레이저가 수직, 수평을 번갈아가며 랜덤 라인 하나를 폭발시키는 패턴
public class Stage2Pattern1 : BasePattern
{
    [Header("설정")]
    // 스폰 포인트에 대한 회전값과 위치 설정. 같은 인덱스끼리 묶임.
    public Vector2[] spawnPoints;
    public float[] laserRotation;
    public float spacing;
    // 레이저가 다 길어지는 데까지 걸리는 시간
    public float expandTime;
    //레이저가 굵어지는 데 걸리는 시간
    public float explodeTime;
    // 목표 레이저 길이
    public float laserLength;
    // 목표 레이저 굵기
    public float laserWidth;
    // 총 스폰 레이저 개수
    public int laserCount;
    public float minSpawnInterval;
    public float maxSpawnInterval;

    int prevVertical = -1;
    int prevHorizontal = -1;
    bool isVertical = false;
    float startTime;

    [Header("할당")]
    // 레이저 프리펩 할당
    public GameObject laser;
    public AudioClip laserExpandSFX;
    public AudioClip laserExplodeSFX;

    protected override void OnEnable()
    {
        StartCoroutine(ProcessPattern());
    }

    protected override IEnumerator ProcessPattern()
    {
        startTime = Time.time;
        Debug.Log("Stage2Pattern1 실행됨.");
        // 레이저를 랜덤 스폰 포인트 4군데에서 랜덤 Spacing을 줘 스폰시킴.
        for (int i = 0; i < laserCount; i++)
        {
            int randomSpacing = isVertical ? getVerticalSpacing() : getHorizontalSpacing();
            int randomIndex = isVertical ? Random.Range(2, 4) : Random.Range(0, 2);

            // 랜덤 스폰 포인트와 스페이싱에 따라 레이저 스폰.
            SpawnLaser(randomIndex, randomSpacing);

            isVertical = !isVertical;

            // 레이저 스폰 인터벌 랜덤화 후 다음 레이저 소환으로 넘어가기.
            float randomInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(randomInterval);
        }
        yield return new WaitForSeconds(2f);
        float expectDuration = ((minSpawnInterval + maxSpawnInterval) / 2) * laserCount + 2f;
        float actualDuration = Time.time - startTime;
        Debug.Log($"[시간 보정] 예상: {expectDuration} / 실제: {actualDuration} / 보정값: {actualDuration - expectDuration}");
        GameStageTimer.instance.UpdateMaxStageTime(actualDuration - expectDuration);
        FinishPattern();
    }
    protected void SpawnLaser(int spawnIndex, int randomSpacing = 0, float explodeWaitTime = 0.3f)
    {
        // 일단 스폰 지점에 생성
        GameObject laserObj = Instantiate(laser, spawnPoints[spawnIndex], Quaternion.Euler(0, 0, laserRotation[spawnIndex]));
        // 이후 spacing 만큼 이동. 수직이면 x만, 수평이면 y만 이동
        // 스폰 인덱스 0, 1은 수평 레이저
        if (spawnIndex == 0 || spawnIndex == 1)
        {
            laserObj.transform.position = new Vector2(laserObj.transform.position.x, laserObj.transform.position.y + randomSpacing * spacing);
        }
        else
        {
            laserObj.transform.position = new Vector2(laserObj.transform.position.x + randomSpacing * spacing, laserObj.transform.position.y);
        }
        // 색 변경 후 확장 시작
        laserObj.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        StartCoroutine(Expand(laserObj, explodeWaitTime));
    }

    //레이저 길이를 확장.
    public IEnumerator Expand(GameObject laser, float explodeWaitTime)
    {
        // 확장 중에는 타격 판정 Off
        laser.GetComponentInChildren<BoxCollider2D>().enabled = false;
        float elapsedTime = 0f;
        AudioManager.instance.PlaySFX(laserExpandSFX, 0.7f);

        // 현재 크기 저장 (시작점)
        Vector3 startScale = laser.transform.localScale;

        while (elapsedTime < expandTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / expandTime;
            float newX = Mathf.Lerp(startScale.x, laserLength, t);
            laser.transform.localScale = new Vector3(newX, startScale.y, startScale.z);
            yield return null;
        }
        laser.transform.localScale = new Vector3(laserLength, startScale.y, startScale.z);

        // 길이 전부 확장 후 폭발까지 잠깐 대기 후 폭발
        yield return new WaitForSeconds(explodeWaitTime);
        StartCoroutine(Explode(laser));
    }

    // 레이저 길이 확장 이후 굵기를 자연스럽게 애니메이션화
    public IEnumerator Explode(GameObject laser)
    {
        AudioManager.instance.PlaySFX(laserExplodeSFX, 0.7f);
        laser.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        float originWidth = laser.transform.localScale.y;
        // 충돌 판정 On
        laser.GetComponentInChildren<BoxCollider2D>().enabled = true;
        //길이를 laserWidth까지 늘렸다가 원상복구 시켰다가 다시 laserWidth까지 늘렸다가 원상복구 시켰다가 비활성화
        yield return StartCoroutine(SetLaserScale(laser, laserWidth));
        yield return StartCoroutine(SetLaserScale(laser, originWidth));
        yield return StartCoroutine(SetLaserScale(laser, laserWidth));
        yield return StartCoroutine(SetLaserScale(laser, 0));
        Destroy(laser);
    }

    // 레이저의 Y축 길이, 즉 굵기를 변화시키는 부분.
    public IEnumerator SetLaserScale(GameObject laser, float targetWidth)
    {
        float elapsedTime = 0f;
        Vector3 startScale = laser.transform.localScale;
        while (elapsedTime < explodeTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / explodeTime;
            float newY = Mathf.Lerp(startScale.y, targetWidth, t);
            laser.transform.localScale = new Vector3(startScale.x, newY, startScale.z);

            yield return null;
        }
        laser.transform.localScale = new Vector3(startScale.x, targetWidth, startScale.z);
    }

    protected int getVerticalSpacing()
    {
        int index = Random.Range(0, 4);
        while (index == prevVertical)
        {
            index = Random.Range(0, 4);
        }
        prevVertical = index;
        return index;
    }

    protected int getHorizontalSpacing()
    {
        int index = Random.Range(0, 4);
        while (index == prevHorizontal)
        {
            index = Random.Range(0, 4);
        }
        prevHorizontal = index;
        return index;
    }
}
