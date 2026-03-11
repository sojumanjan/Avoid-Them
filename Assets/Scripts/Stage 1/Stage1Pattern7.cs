using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

// BasePattern ���
public class Stage1Pattern7 : BasePattern
{
    public GameObject projectilePrefab;
    private Vector3[] Pos; // 중력받는 구체의 스폰위치
    public float gravityStrength; // 중력 세기 (조절 가능)
    private BombPattern gameManager;
    private void Awake()
    {
        gameManager = FindFirstObjectByType<BombPattern>();
    }

    protected override IEnumerator ProcessPattern()
    {
        Pos = new Vector3[4];
        for (int i = 0; i < 4; i++)
        {
            Pos[i] = new Vector3(-1.5f + i, 7f, 0);
        }

        SpawnProjectile(Pos[0], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[2], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[1], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[3], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnBombAtIntersection(-1, 1);
        SpawnBombAtIntersection(1, 1);
        for (int i = 0; i < 4; i++)
        {
            StartCoroutine(gameManager.TriggerSingleTile(i, 3));
        }
        SpawnProjectile(Pos[3], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[2], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[0], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[1], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[2], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[0], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[3], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[0], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnBombAtIntersection(-1, 1);
        SpawnBombAtIntersection(1, 1);
        for (int i = 0; i < 4; i++)
        {
            StartCoroutine(gameManager.TriggerSingleTile(i, 2));
        }
        SpawnProjectile(Pos[3], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[2], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[0], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[1], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[2], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[0], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[3], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[0], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnBombAtIntersection(-1, 1);
        SpawnBombAtIntersection(1, 1);
        for (int i = 0; i < 4; i++)
        {
            StartCoroutine(gameManager.TriggerSingleTile(i, 3));
        }
        SpawnProjectile(Pos[3], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[2], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[0], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[1], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[2], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[0], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[3], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[0], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnBombAtIntersection(-1, 1);
        SpawnBombAtIntersection(1, 1);
        for (int i = 0; i < 4; i++)
        {
            StartCoroutine(gameManager.TriggerSingleTile(i, 2));
        }
        SpawnProjectile(Pos[3], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[2], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[0], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[1], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[2], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[0], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[3], gravityStrength);
        yield return new WaitForSeconds(0.25f);
        SpawnProjectile(Pos[0], gravityStrength);
        yield return new WaitForSeconds(2f);



        yield return null;
        FinishPattern();
    }
    void SpawnProjectile(Vector3 pos, float gravityStrength)
    {
        if (projectilePrefab == null) return;

        GameObject p = Instantiate(projectilePrefab, pos, Quaternion.identity);
        FallingProjectile projScript = p.GetComponent<FallingProjectile>();
        if (projScript != null)
        {
            projScript.Setup(pos, gravityStrength);
        }
    }
    void SpawnBombAtIntersection(int x, int y)
    {
        Vector3 spawnPos = new Vector3(x, y, 0);

        GameObject bombObj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        // 폭탄 설정 (좌표 전달)
        GridBomb bombScript = bombObj.GetComponent<GridBomb>();
        if (bombScript != null)
        {
            bombScript.Setup(x, y);
        }
    }

}
