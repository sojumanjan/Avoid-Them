using System.Collections;
using System.ComponentModel;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MainPattern2 : BasePattern
{
    // 레이저 스폰 포인트와 회전을 묶어 구조체화
    [System.Serializable]
    public struct SpawnData
    {
        public Vector2 position;
        public float[] angle;
    }
    // 인스펙터 창에서 스폰 포인트와 회전각, 레이저 오브젝트 할당.
    public SpawnData[] spawndata;
    public GameObject[] lasers;
    // 레이저가 다 길어지는 데까지 걸리는 시간
    public float expandTime;
    //레이저가 굵어지는 데 걸리는 시간
    public float explodeTime;
    // 레이저 길이
    public float laserLength;
    // 레이저 굵기
    public float laserWidth;
    protected override void OnEnable()
    {
        StartCoroutine(ProcessPattern());
    }

    protected override IEnumerator ProcessPattern()
    {
        Debug.Log("MainPattern2 실행됨.");
        //
        for (int i = 0; i < lasers.Length; i++)
        {
            SpawnData randData = spawndata[Random.Range(0, spawndata.Length)];
            float randAngle = randData.angle[Random.Range(0, randData.angle.Length)];
            lasers[i].transform.localPosition = randData.position;
            lasers[i].transform.rotation = Quaternion.Euler(0, 0, randAngle);
            lasers[i].transform.localScale = new Vector3(0f, 0.04f, 0f);
            lasers[i].SetActive(true);
            StartCoroutine(Expand(lasers[i]));
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(5f);
        FinishPattern();
    }

    //레이저 길이를 확장.
    public IEnumerator Expand(GameObject laser)
    {
        float elapsedTime = 0f;

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
        StartCoroutine(Explode(laser));
    }

    // 레이저 길이 확장 이후 굵기를 자연스럽게 애니메이션화
    public IEnumerator Explode(GameObject laser)
    {
        float originWidth = laser.transform.localScale.y;
        //길이를 laserWidth까지 늘렸다가 원상복구 시켰다가 다시 laserWidth까지 늘렸다가 원상복구 시켰다가 비활성화
        yield return StartCoroutine(SetLaserScale(laser, laserWidth));
        yield return StartCoroutine(SetLaserScale(laser, originWidth));
        yield return StartCoroutine(SetLaserScale(laser, laserWidth));
        yield return StartCoroutine(SetLaserScale(laser, 0));
        laser.SetActive(false);
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
}
