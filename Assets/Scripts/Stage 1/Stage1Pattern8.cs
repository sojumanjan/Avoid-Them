using UnityEngine;
using System.Collections;

public class Stage1Pattern8 : BasePattern
{
    [Header("Settings")]
    public GameObject bombPrefab; // GridBomb이 붙은 프리팹
    public float gridSpacing = 1f; // 그리드 간격 (Manager와 동일하게)

    // Pattern2의 변수들
    public GameObject projectilePrefab;
    public int ProjectileSpeed; // 투사체 속도
    private string Pos; // 투사체 출발 위치
    private Vector2 direction;
    private Vector3[] LToR; // 발사 방향 L->R
    private Vector3[] RToL;
    private Vector3[] UToD;
    private Vector3[] DToU;

    protected override IEnumerator ProcessPattern()
    {
        LToR = new Vector3[4];
        RToL = new Vector3[4];
        UToD = new Vector3[4];
        DToU = new Vector3[4];
        for (int i = 0; i < 4; i++)
        {
            LToR[i] = new Vector3(-10f, 1.5f - i, 0);
            RToL[i] = new Vector3(10f, 1.5f - i, 0);
            UToD[i] = new Vector3(-1.5f + i, 6f, 0);
            DToU[i] = new Vector3(-1.5f + i, -6f, 0);
        }

        SpawnBombAtIntersection(-1, 1);
        yield return new WaitForSeconds(0.3f);
        SpawnBombAtIntersection(0, 0);
        yield return new WaitForSeconds(0.3f);
        SpawnBombAtIntersection(1, -1);
        yield return new WaitForSeconds(0.3f);
        SpawnBombAtIntersection(1, 1);
        yield return new WaitForSeconds(0.3f);
        SpawnBombAtIntersection(0, 0);
        yield return new WaitForSeconds(0.3f);
        SpawnBombAtIntersection(-1, -1);
        yield return new WaitForSeconds(2f);

        for(int i=0; i<4; i++)
        {
            SpawnBombAtIntersection(-1, 1);
            yield return new WaitForSeconds(0.3f);
            SpawnBombAtIntersection(1, 1);
            yield return new WaitForSeconds(0.3f);
            SpawnBombAtIntersection(1, -1);
            yield return new WaitForSeconds(0.3f);
            SpawnBombAtIntersection(-1, -1);
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < 4; i++)
        {
            SpawnBombAtIntersection(1, 1);
            yield return new WaitForSeconds(0.3f);
            SpawnBombAtIntersection(-1, 1);
            yield return new WaitForSeconds(0.3f);
            SpawnBombAtIntersection(-1, -1);
            yield return new WaitForSeconds(0.3f);
            SpawnBombAtIntersection(1, -1);
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(2f);



        FinishPattern();
    }

    void SpawnBombAtIntersection(int x, int y)
    {
        Vector3 spawnPos = new Vector3(x, y, 0);

        GameObject bombObj = Instantiate(bombPrefab, spawnPos, Quaternion.identity);

        // 폭탄 설정 (좌표 전달)
        GridBomb bombScript = bombObj.GetComponent<GridBomb>();
        if (bombScript != null)
        {
            bombScript.Setup(x, y);
        }
    }
    void SpawnProjectile(Vector3 pos, Vector2 dir, int ProjectileSpeed)
    {
        if (projectilePrefab == null) return;

        GameObject p = Instantiate(projectilePrefab, pos, Quaternion.identity);
        Projectile projScript = p.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.Setup(dir, ProjectileSpeed); // 방향, 속도 설정
        }
    }
}