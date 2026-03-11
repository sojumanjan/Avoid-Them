using System.Collections;
using UnityEngine;

// 1자 레이저 두개가 동시에 출현하기도 하는 패턴. 패턴 1번을 상속받음.
public class Stage2Pattern2 : Stage2Pattern1
{
    // 스폰 포인트가 4개 추가되어 대각 레이저를 동시에 발사 하도록 함. (대각 스폰 인덱스는 4 ~ 7)

    // 패턴 다양화를 위한 중복 방지 차원에서 이 전 스폰 지점은 선택하지 않도록.
    int prevDiag = -1;
    int spawnIndex1;
    int spawnIndex2;
    protected override IEnumerator ProcessPattern()
    {
        // 레이저 스폰 포인트 2개와 각 게임오브젝트 2개

        // 대각 레이저 하나 발사
        spawnIndex1 = GetDiagIndex();
        FireDiagonal();
        yield return new WaitForSeconds(0.7f);

        FireVertical();
        FireVertical();
        yield return new WaitForSeconds(1f);

        FireVertical();
        FireDiagonal();
        yield return new WaitForSeconds(1f);

        FireHorizontal();
        FireHorizontal();
        yield return new WaitForSeconds(1f);

        SpawnLaser(4);
        SpawnLaser(5);
        yield return new WaitForSeconds(1f);

        SpawnLaser(0, 0);
        SpawnLaser(0, 2);
        SpawnLaser(2, 0);
        SpawnLaser(2, 2);
        yield return new WaitForSeconds(1.3f);

        SpawnLaser(1, 1);
        SpawnLaser(1, 3);
        SpawnLaser(3, 1);
        SpawnLaser(3, 3);
        yield return new WaitForSeconds(1.3f);

        FireDiagonal();
        FireVertical();
        FireVertical();
        yield return new WaitForSeconds(1f);

        FireDiagonal();
        FireHorizontal();
        FireVertical();
        yield return new WaitForSeconds(1f);

        FireDiagonal();
        FireHorizontal();
        FireVertical();
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 4; i++)
        {
            SpawnLaser(0, i);
            yield return new WaitForSeconds(0.4f);
        }
        yield return new WaitForSeconds(0.5f);

        SpawnLaser(2, 0, 1.5f);
        yield return new WaitForSeconds(0.5f);
        SpawnLaser(2, 2, 1.5f);
        yield return new WaitForSeconds(0.5f);
        SpawnLaser(2, 1, 1.5f);
        yield return new WaitForSeconds(0.5f);
        SpawnLaser(2, 3, 1.5f);
        yield return new WaitForSeconds(2.5f);

        FireVertical(0.2f);
        FireVertical(0.2f);
        FireHorizontal(0.2f);
        FireHorizontal(0.2f);
        yield return new WaitForSeconds(1.5f);

        FireVertical(0.2f);
        FireVertical(0.2f);
        FireHorizontal(0.2f);
        FireHorizontal(0.2f);
        yield return new WaitForSeconds(1.5f);

        FireHorizontal();
        FireHorizontal();
        yield return new WaitForSeconds(0.5f);

        FireVertical();
        FireVertical();
        yield return new WaitForSeconds(1f);

        FireHorizontal();
        FireHorizontal();
        yield return new WaitForSeconds(0.5f);

        FireVertical();
        FireVertical();
        yield return new WaitForSeconds(0.5f);
        FireDiagonal();
        yield return new WaitForSeconds(0.5f);

        FireHorizontal();
        FireHorizontal();
        yield return new WaitForSeconds(0.5f);

        FireVertical();
        FireVertical();
        FireDiagonal();
        yield return new WaitForSeconds(0.5f);

        yield return new WaitForSeconds(2f);
        FinishPattern();
    }

    int GetDiagIndex()
    {
        int index = Random.Range(4, 8);
        while (index == prevDiag)
        {
            index = Random.Range(4, 8);
        }
        prevDiag = index;
        return index;
    }

    // 확장 후 explodeWaitTime만큼 대기한 뒤 터트리는 래퍼 함수. Vertical 레이저에 해당.
    void FireVertical(float explodeWaitTime = 0.3f)
    {
        int idx = Random.Range(2, 4);
        int space = getVerticalSpacing();
        SpawnLaser(idx, space, explodeWaitTime);
    }

    void FireHorizontal(float explodeWaitTime = 0.3f)
    {
        int idx = Random.Range(0, 2);
        int space = getHorizontalSpacing();
        SpawnLaser(idx, space, explodeWaitTime);
    }

    void FireDiagonal(float explodeWaitTime = 0.3f)
    {
        int idx = GetDiagIndex();
        SpawnLaser(idx, 0, explodeWaitTime); // 대각선은 spacing 없다고 가정
    }
}
