using UnityEngine;
using System.Collections;

// BasePattern ???
public class Stage1Pattern1 : BasePattern
{
    [Header("Pattern Settings")]
    public GameObject projectilePrefab; // 공 프리팹 연결
    protected override IEnumerator ProcessPattern()
    {
        // === Wave 1: 왼쪽 벽에서 오른쪽으로 발사 ===
        for (int i = 0; i < 4; i++)
        {
            // (왼쪽 벽 밖, i번째 줄) 위치 생성
            Vector3 spawnPos = new Vector3(-10f, -1.5f + i, 0);
            SpawnProjectile(spawnPos, Vector2.right);

            // 0.3초 간격으로 발사
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1.0f); // 잠시 휴식

        // === Wave 2: 위쪽 벽에서 아래로 발사 ===
        for (int i = 0; i < 4; i++)
        {
            // (i번째 줄, 위쪽 벽 밖) 위치 생성
            Vector3 spawnPos = new Vector3(-1.5f + i, 6f, 0);
            SpawnProjectile(spawnPos, Vector2.down);

            yield return new WaitForSeconds(0.3f);
        }

        FinishPattern();

        // 투사체 생성 도우미 함수
        void SpawnProjectile(Vector3 pos, Vector2 dir)
        {
            if (projectilePrefab == null) return;

            GameObject p = Instantiate(projectilePrefab, pos, Quaternion.identity);
            Projectile projScript = p.GetComponent<Projectile>();
            if (projScript != null)
            {
                projScript.Setup(dir, 6f); // 방향, 속도 설정
            }
        }

        // ?????? ????????? ???? ?????? ??? ????.


        // ?????? ?????? ?????? ?? ??? ???
        yield return null;
        FinishPattern();
    }

}
