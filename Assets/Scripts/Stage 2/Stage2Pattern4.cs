using System.Collections;
using UnityEngine;

public class Stage2Pattern4 : BasePattern
{
    /*레이저가 일자로 맵을 싹 훑는 패턴.
    4방향으로 움직이는 레이저 스폰 타이밍만 조절하면 된다.
    방향 인덱스  0 : 좌 -> 우
                1 : 우 -> 좌
                2 : 위 -> 아래
                3 : 아래 -> 위
    */
    [Header("설정")]
    // 레이저 스폰 지점과 인덱스에 따른 이동 방향 벡터.
    public Vector2[] spawnPoints;
    // 생성 후 반대편 끝까지 가서 삭제되기까지의 시간
    public float laserMoveTime;
    public float laserSpeed;

    [Header("할당")]
    public GameObject laser;
    public AudioClip laserLoopSound;


    private Vector2[] direction = { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(0, 1) };
    private float[] rotation = { 90, -90, 180, 0 };
    protected override IEnumerator ProcessPattern()
    {

        StartCoroutine(SpawnLaser(1));
        yield return new WaitForSeconds(2f);
        StartCoroutine(SpawnLaser(3));
        yield return new WaitForSeconds(2f);
        StartCoroutine(SpawnLaser(0));
        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnLaser(1));
        yield return new WaitForSeconds(1.3f);
        StartCoroutine(SpawnLaser(3));
        yield return new WaitForSeconds(2f);
        StartCoroutine(SpawnLaser(2));
        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnLaser(0));
        yield return new WaitForSeconds(1.6f);
        StartCoroutine(SpawnLaser(0));
        yield return new WaitForSeconds(1.6f);
        StartCoroutine(SpawnLaser(3));
        yield return new WaitForSeconds(1.6f);
        StartCoroutine(SpawnLaser(1));
        StartCoroutine(SpawnLaser(2));
        yield return new WaitForSeconds(2f);
        StartCoroutine(SpawnLaser(1));
        StartCoroutine(SpawnLaser(3));
        yield return new WaitForSeconds(1.4f);
        StartCoroutine(SpawnLaser(0));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SpawnLaser(2));
        yield return new WaitForSeconds(1.6f);

        for (int i = 0; i < 4; i++)
        {
            StartCoroutine(SpawnLaser(1));
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < 4; i++)
        {
            StartCoroutine(SpawnLaser(2));
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1.4f);
        StartCoroutine(SpawnLaser(0));
        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnLaser(2));
        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnLaser(1));
        yield return new WaitForSeconds(1.3f);
        StartCoroutine(SpawnLaser(3));
        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnLaser(2));
        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnLaser(2));
        yield return new WaitForSeconds(5f);
        FinishPattern();
    }

    IEnumerator SpawnLaser(int dirIndex)
    {
        // 인덱스에 맞게 레이저 스폰.
        GameObject laserObj = Instantiate(laser, spawnPoints[dirIndex], Quaternion.Euler(0, 0, rotation[dirIndex]));
        AudioManager.instance.PlaySFX(laserLoopSound, 0.3f);

        // 인덱스에 맞게 쭉 이동해줌.
        float timer = 0f;
        while (timer < laserMoveTime)
        {
            timer += Time.deltaTime;
            laserObj.transform.position += (Vector3)(direction[dirIndex] * laserSpeed * Time.deltaTime);
            yield return null;
        }
        // 5. 시간 다 되면 삭제
        Destroy(laserObj);
    }
}
