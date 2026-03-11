using System.Collections;
using UnityEngine;

public class Stage2Pattern5 : BasePattern
{
    // 오른쪽에서 레이저가 위, 아래에 적절한 크기로 다가오는 패턴
    [Header("설정")]
    public float laserSpeed;
    // 레이저 스폰 후 제거까지 걸리는 시간
    public float laserMoveTime;
    // 위에서 아래로 쬐는 레이저 스폰 포인트
    public Vector2 upSpawnPoint;
    public Vector2 downSpawnPoint;

    [Header("할당")]
    public GameObject laser;
    protected override IEnumerator ProcessPattern()
    {
        // 위에서 아래로 쬐는 그리드 3칸을 먹는 레이저 스폰 한다는 뜻
        StartCoroutine(SpawnLaser("up", 2f));
        yield return new WaitForSeconds(1f);

        StartCoroutine(SpawnLaser("down", 1f));
        yield return new WaitForSeconds(1.3f);

        StartCoroutine(SpawnLaser("up", 3f));
        yield return new WaitForSeconds(1f);

        StartCoroutine(SpawnLaser("down", 3f));
        yield return new WaitForSeconds(0.8f);

        StartCoroutine(SpawnLaser("up", 2f));
        yield return new WaitForSeconds(0.8f);

        StartCoroutine(SpawnLaser("down", 3f));
        yield return new WaitForSeconds(0.8f);

        StartCoroutine(SpawnLaser("up", 2f));
        StartCoroutine(SpawnLaser("down", 1f));
        yield return new WaitForSeconds(0.8f);

        StartCoroutine(SpawnLaser("down", 3f));
        yield return new WaitForSeconds(0.8f);

        StartCoroutine(SpawnLaser("up", 3f));
        yield return new WaitForSeconds(1f);

        // ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
        StartCoroutine(SpawnLaser("down", 3f));
        yield return new WaitForSeconds(0.6f);

        StartCoroutine(SpawnLaser("up", 2f));
        yield return new WaitForSeconds(0.6f);

        StartCoroutine(SpawnLaser("down", 3f));
        yield return new WaitForSeconds(0.6f);

        StartCoroutine(SpawnLaser("down", 1f));
        StartCoroutine(SpawnLaser("up", 2f));
        yield return new WaitForSeconds(0.6f);

        StartCoroutine(SpawnLaser("down", 2f));
        yield return new WaitForSeconds(0.6f);

        StartCoroutine(SpawnLaser("up", 2f));
        yield return new WaitForSeconds(0.6f);

        StartCoroutine(SpawnLaser("down", 2f));
        yield return new WaitForSeconds(0.6f);

        StartCoroutine(SpawnLaser("down", 0f));
        StartCoroutine(SpawnLaser("up", 3f));
        yield return new WaitForSeconds(0.6f);

        StartCoroutine(SpawnLaser("down", 3f));
        StartCoroutine(SpawnLaser("up", 0f));
        yield return new WaitForSeconds(0.6f);

        StartCoroutine(SpawnLaser("up", 3f));
        yield return new WaitForSeconds(0.6f);

        StartCoroutine(SpawnLaser("up", 1f));
        StartCoroutine(SpawnLaser("down", 2f));
        yield return new WaitForSeconds(0.6f);

        StartCoroutine(SpawnLaser("up", 3f));
        yield return new WaitForSeconds(2f);

        // ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
        StartCoroutine(SpawnLaser("up", 1f));
        StartCoroutine(SpawnLaser("down", 2f));
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(SpawnLaser("up", 2f));
        StartCoroutine(SpawnLaser("down", 1f));
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(SpawnLaser("up", 1f));
        StartCoroutine(SpawnLaser("down", 2f));
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(SpawnLaser("up", 0f));
        StartCoroutine(SpawnLaser("down", 3f));
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(SpawnLaser("up", 1f));
        StartCoroutine(SpawnLaser("down", 2f));
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(SpawnLaser("up", 2f));
        StartCoroutine(SpawnLaser("down", 1f));
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(SpawnLaser("up", 1f));
        StartCoroutine(SpawnLaser("down", 2f));
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(SpawnLaser("up", 2f));
        StartCoroutine(SpawnLaser("down", 1f));
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(SpawnLaser("up", 3f));
        StartCoroutine(SpawnLaser("down", 0f));
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(SpawnLaser("up", 2f));
        StartCoroutine(SpawnLaser("down", 1f));
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(SpawnLaser("up", 3f));
        StartCoroutine(SpawnLaser("down", 0f));
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(SpawnLaser("up", 3f));
        StartCoroutine(SpawnLaser("down", 0f));
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(SpawnLaser("up", 2f));
        StartCoroutine(SpawnLaser("down", 1f));
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(SpawnLaser("up", 1f));
        StartCoroutine(SpawnLaser("down", 2f));
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(SpawnLaser("up", 0f));
        StartCoroutine(SpawnLaser("down", 3f));
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(SpawnLaser("up", 1f));
        StartCoroutine(SpawnLaser("down", 2f));
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(SpawnLaser("up", 0f));
        StartCoroutine(SpawnLaser("down", 3f));
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(SpawnLaser("up", 1f));
        StartCoroutine(SpawnLaser("down", 2f));
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(SpawnLaser("up", 2f));
        StartCoroutine(SpawnLaser("down", 1f));
        yield return new WaitForSeconds(0.8f);

        StartCoroutine(SpawnLaser("up", 2f));
        StartCoroutine(SpawnLaser("down", 2f));
        yield return new WaitForSeconds(3f);

        FinishPattern();
    }

    IEnumerator SpawnLaser(string upDown, float length)
    {
        // 위, 아래에 따라 
        float rotAngle;
        Vector2 spawnPoint;
        if (upDown == "up") 
        {
            rotAngle = -90f;
            spawnPoint = upSpawnPoint;
            spawnPoint = new Vector2(spawnPoint.x, spawnPoint.y - length);
        }
        else 
        {
            rotAngle = 90f;
            spawnPoint = downSpawnPoint;
            spawnPoint = new Vector2(spawnPoint.x, spawnPoint.y + length);
        }
        GameObject laserObj = Instantiate(laser, spawnPoint, Quaternion.Euler(0, 0, rotAngle));

        float timer = 0f;
        while (timer < laserMoveTime)
        {
            timer += Time.deltaTime;
            laserObj.transform.position += (Vector3)(new Vector2(-1, 0) * laserSpeed * Time.deltaTime);
            yield return null;
        }
        // 5. 시간 다 되면 삭제
        Destroy(laserObj);
    }
}
