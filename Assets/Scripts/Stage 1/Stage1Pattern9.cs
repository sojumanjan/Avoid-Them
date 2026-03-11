using UnityEngine;
using System.Collections;

public class Stage1Pattern9 : BasePattern
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
    private BombPattern gameManager;
    private void Awake()
    {
        gameManager = FindFirstObjectByType<BombPattern>();
    }
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

        SpawnProjectile(LToR[1], Vector2.right, ProjectileSpeed);
        SpawnProjectile(LToR[2], Vector2.right, ProjectileSpeed);
        yield return new WaitForSeconds(0.1f);
        SpawnProjectile(UToD[1], Vector2.down, ProjectileSpeed);
        SpawnProjectile(UToD[2], Vector2.down, ProjectileSpeed);
        yield return new WaitForSeconds(2f);

        SpawnBombAtIntersection(0, 0);
        for(int i=0;i<4;i++)
        {
            StartCoroutine(gameManager.TriggerSingleTile(0, i));
            StartCoroutine(gameManager.TriggerSingleTile(3, 3-i));
        }
        yield return new WaitForSeconds(2f);
        SpawnBombAtIntersection(0, 0);
        for (int i = 0; i < 4; i++)
        {
            StartCoroutine(gameManager.TriggerSingleTile(i, 0));
            StartCoroutine(gameManager.TriggerSingleTile(3-i, 3));
        }
        yield return new WaitForSeconds(2f);
        SpawnBombAtIntersection(-1, 1);
        SpawnBombAtIntersection(1, -1);
        StartCoroutine(gameManager.TriggerSingleTile(0, 2));
        StartCoroutine(gameManager.TriggerSingleTile(2, 0));
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(gameManager.TriggerSingleTile(1, 2));
        StartCoroutine(gameManager.TriggerSingleTile(3, 0));
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(gameManager.TriggerSingleTile(1, 3));
        StartCoroutine(gameManager.TriggerSingleTile(3, 1));
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(gameManager.TriggerSingleTile(0, 3));
        StartCoroutine(gameManager.TriggerSingleTile(2, 1));
        yield return new WaitForSeconds(1f);


        SpawnProjectile(LToR[0], Vector2.right, ProjectileSpeed);
        yield return new WaitForSeconds(0.3f);

        // 2. 아래 세번째
        SpawnProjectile(DToU[2], Vector2.up, ProjectileSpeed);
        yield return new WaitForSeconds(0.3f);

        // 3. 오른쪽 두번째
        SpawnProjectile(RToL[1], Vector2.left, ProjectileSpeed);
        yield return new WaitForSeconds(0.3f);

        // 4. 위 네번째
        SpawnProjectile(UToD[3], Vector2.down, ProjectileSpeed);
        yield return new WaitForSeconds(0.3f);

        // 5. 왼쪽 세번째
        SpawnProjectile(LToR[2], Vector2.right, ProjectileSpeed);
        yield return new WaitForSeconds(0.3f);

        // 6. 오른쪽 첫번째
        SpawnProjectile(RToL[0], Vector2.left, ProjectileSpeed);
        yield return new WaitForSeconds(0.3f);

        // 7. 위 두번째
        SpawnProjectile(UToD[1], Vector2.down, ProjectileSpeed);
        yield return new WaitForSeconds(0.3f);

        // 8. 아래 첫번째
        SpawnProjectile(DToU[0], Vector2.up, ProjectileSpeed);
        yield return new WaitForSeconds(0.3f);

        // 9. 왼쪽 네번째
        SpawnProjectile(LToR[3], Vector2.right, ProjectileSpeed);
        yield return new WaitForSeconds(0.3f);

        // 10. 오른쪽 네번째
        SpawnProjectile(RToL[3], Vector2.left, ProjectileSpeed);
        yield return new WaitForSeconds(0.3f);

        // 11. 아래 두번째
        SpawnProjectile(DToU[1], Vector2.up, ProjectileSpeed);
        yield return new WaitForSeconds(0.3f);

        // 12. 위 세번째
        SpawnProjectile(UToD[2], Vector2.down, ProjectileSpeed);
        yield return new WaitForSeconds(0.3f);

        // 13. 왼쪽 두번째
        SpawnProjectile(LToR[1], Vector2.right, ProjectileSpeed);
        yield return new WaitForSeconds(0.3f);

        // 14. 아래 네번째
        SpawnProjectile(DToU[3], Vector2.up, ProjectileSpeed);
        yield return new WaitForSeconds(0.3f);

        // 15. 오른쪽 세번째
        SpawnProjectile(RToL[2], Vector2.left, ProjectileSpeed);
        yield return new WaitForSeconds(0.3f);

        // 16. 위 첫번째
        SpawnProjectile(UToD[0], Vector2.down, ProjectileSpeed);
        yield return new WaitForSeconds(0.3f);

        // 17. 왼쪽 첫번째 (다시 반복 시작이나 위치 다름)
        SpawnProjectile(LToR[0], Vector2.right, ProjectileSpeed);
        yield return new WaitForSeconds(0.3f);

        // 18. 오른쪽 두번째
        SpawnProjectile(RToL[1], Vector2.left, ProjectileSpeed);
        yield return new WaitForSeconds(0.3f);

        // 19. 위 네번째
        SpawnProjectile(UToD[3], Vector2.down, ProjectileSpeed);
        yield return new WaitForSeconds(0.3f);

        // 20. 아래 세번째
        SpawnProjectile(DToU[2], Vector2.up, ProjectileSpeed);

        yield return new WaitForSeconds(3f);

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