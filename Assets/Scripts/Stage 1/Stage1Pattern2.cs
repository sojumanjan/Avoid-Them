using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

// BasePattern ���
public class Stage1Pattern2 : BasePattern
{
    [Header("Pattern Settings")]
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

        SpawnProjectile(LToR[0], Vector2.right, ProjectileSpeed);
        SpawnProjectile(LToR[1], Vector2.right, ProjectileSpeed);
        SpawnProjectile(RToL[2], Vector2.left, ProjectileSpeed);
        SpawnProjectile(RToL[3], Vector2.left, ProjectileSpeed);

        yield return new WaitForSeconds(2f);

        SpawnProjectile(UToD[0], Vector2.down, ProjectileSpeed);
        SpawnProjectile(UToD[1], Vector2.down, ProjectileSpeed);
        SpawnProjectile(DToU[2], Vector2.up, ProjectileSpeed);
        SpawnProjectile(DToU[3], Vector2.up, ProjectileSpeed);

        yield return new WaitForSeconds(2f);

        SpawnProjectile(LToR[0], Vector2.right, ProjectileSpeed);
        yield return new WaitForSeconds(0.5f);

        SpawnProjectile(DToU[3], Vector2.up, ProjectileSpeed);
        yield return new WaitForSeconds(0.5f);

        SpawnProjectile(LToR[1], Vector2.right, ProjectileSpeed);
        yield return new WaitForSeconds(0.5f);

        SpawnProjectile(DToU[2], Vector2.up, ProjectileSpeed);
        yield return new WaitForSeconds(0.5f);

        SpawnProjectile(LToR[2], Vector2.right, ProjectileSpeed);
        yield return new WaitForSeconds(0.5f);

        SpawnProjectile(DToU[1], Vector2.up, ProjectileSpeed);
        yield return new WaitForSeconds(0.5f);

        SpawnProjectile(LToR[3], Vector2.right, ProjectileSpeed);
        yield return new WaitForSeconds(0.5f);

        SpawnProjectile(DToU[0], Vector2.up, ProjectileSpeed);
        yield return new WaitForSeconds(2f);


        SpawnProjectile(UToD[0], Vector2.down, ProjectileSpeed);
        yield return new WaitForSeconds(0.5f);

        SpawnProjectile(RToL[3], Vector2.left, ProjectileSpeed);
        yield return new WaitForSeconds(0.5f);

        SpawnProjectile(UToD[1], Vector2.down, ProjectileSpeed);
        yield return new WaitForSeconds(0.5f);

        SpawnProjectile(RToL[2], Vector2.left, ProjectileSpeed);
        yield return new WaitForSeconds(0.5f);

        SpawnProjectile(UToD[2], Vector2.down, ProjectileSpeed);
        yield return new WaitForSeconds(0.5f);

        SpawnProjectile(RToL[1], Vector2.left, ProjectileSpeed);
        yield return new WaitForSeconds(0.5f);

        SpawnProjectile(UToD[3], Vector2.down, ProjectileSpeed);
        yield return new WaitForSeconds(0.5f);

        SpawnProjectile(RToL[0], Vector2.left, ProjectileSpeed);


        yield return null;
        FinishPattern();
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
