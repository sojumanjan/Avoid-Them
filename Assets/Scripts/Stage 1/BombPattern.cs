using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class BombPattern : MonoBehaviour
{
    public bool isGameOver = false;
    public static BombPattern instance;

    public AudioClip timerSFX;
    public AudioClip bombSFX;

    public float beatInterval = 1.5f;
    public Color basicColor = Color.yellow;     // 기본 색상
    public Color dangerColor = Color.red;     // 폭발 색상

    // 4x4 타일 배열
    private GameObject[,] tiles = new GameObject[4, 4];

    private PlayerController player;
    private UIManager uiManager;

    public bool IsGameOver { get; private set; } = false;
    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        uiManager = FindFirstObjectByType<UIManager>();

        LinkExistingTiles();

        // 패턴 매니저가 따로 돌므로 여기서는 GameLoop를 돌리지 않거나, 
        // 필요한 경우에만 실행합니다. (질문하신 내용에 맞춰 패턴 매니저 사용 권장)
    }

    // ★ 수정된 핵심 부분: 이름으로 타일 찾아서 배열에 넣기
    void LinkExistingTiles()
    {
        // 배열 메모리 할당 (이거 안 하면 NullReference 뜸)
        tiles = new GameObject[4, 4];

        // 1. 부모 오브젝트 찾기 (Hierarchy 이름과 똑같아야 함)
        GameObject parentObj = GameObject.Find("Tiles_Manager");

        if (parentObj == null)
        {
            Debug.LogError("오류: Hierarchy에 'Tiles_Manager' 오브젝트가 없습니다!");
            return;
        }

        // 2. 이중 반복문으로 Tile00 ~ Tile33 찾기
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                // 찾을 이름 만들기: "Tile" + 0 + 0 => "Tile00"
                string targetName = "Tile" + x + y;

                // Tiles_Manager의 자식 중에서 이름으로 찾기
                Transform foundTile = parentObj.transform.Find(targetName);

                if (foundTile != null)
                {
                    tiles[x, y] = foundTile.gameObject;

                    // 초기화: 게임 시작 시 안 보이게 꺼두기
                    tiles[x, y].SetActive(false);
                }
                else
                {
                    Debug.LogError($"오류: '{targetName}'을 Tiles_Manager 아래에서 찾을 수 없습니다.");
                }
            }
        }
    }

    // ★ 수정된 폭발 함수: 태그 변경 없이 활성화/비활성화로 제어
    public IEnumerator TriggerSpecificBomb(int x, int y)
    {
        if (tiles == null) yield break;
        List<GameObject> targetTiles = new List<GameObject>();

        // 1. 5가지 경우의 수에 따라 타일 선택 (기존과 동일)
        if (x == 0 && y == 0) // 중앙
        {
            targetTiles.Add(tiles[1, 1]); targetTiles.Add(tiles[1, 2]);
            targetTiles.Add(tiles[2, 1]); targetTiles.Add(tiles[2, 2]);
        }
        else if (x == -1 && y == 1) // 좌상단
        {
            targetTiles.Add(tiles[0, 0]); targetTiles.Add(tiles[0, 1]);
            targetTiles.Add(tiles[1, 0]); targetTiles.Add(tiles[1, 1]);
        }
        else if (x == 1 && y == 1) // 우상단
        {
            targetTiles.Add(tiles[0, 2]); targetTiles.Add(tiles[1, 2]);
            targetTiles.Add(tiles[0, 3]); targetTiles.Add(tiles[1, 3]);
        }
        else if (x == -1 && y == -1) // 좌하단
        {
            targetTiles.Add(tiles[2, 0]); targetTiles.Add(tiles[3, 0]);
            targetTiles.Add(tiles[2, 1]); targetTiles.Add(tiles[3, 1]);
        }
        else if (x == 1 && y == -1) // 우하단
        {
            targetTiles.Add(tiles[2, 2]); targetTiles.Add(tiles[2, 3]);
            targetTiles.Add(tiles[3, 2]); targetTiles.Add(tiles[3, 3]);
        }
        else if (x == 0 && y == 1)
        {
            targetTiles.Add(tiles[0, 1]); targetTiles.Add(tiles[0, 2]);
            targetTiles.Add(tiles[1, 1]); targetTiles.Add(tiles[1, 2]);
        }
        else if (x == 1 && y == 0)
        {
            targetTiles.Add(tiles[1, 2]); targetTiles.Add(tiles[1, 3]);
            targetTiles.Add(tiles[2, 2]); targetTiles.Add(tiles[2, 3]);
        }
        else if (x == 0 && y == -1)
        {
            targetTiles.Add(tiles[2, 1]); targetTiles.Add(tiles[2, 2]);
            targetTiles.Add(tiles[3, 1]); targetTiles.Add(tiles[3, 2]);
        }
        else if (x == -1 && y == 0)
        {
            targetTiles.Add(tiles[1, 0]); targetTiles.Add(tiles[1, 1]);
            targetTiles.Add(tiles[2, 0]); targetTiles.Add(tiles[2, 1]);
        }
        else
        {
            yield break;
        }



        // [공격 단계] 빨간색으로 변경
        foreach (var t in targetTiles)
        {
            dangerColor.a = 0.1f;
            t.GetComponent<SpriteRenderer>().color = dangerColor; // 빨간색
            t.SetActive(true);


        }

        // 공격 판정 시간
        yield return new WaitForSeconds(0.5f);


        // 4. [종료 단계] 오브젝트 끄기 + 노란색으로 복구
        foreach (var tile in targetTiles)
        {
            tile.SetActive(false); // 비활성화
        }
    }

    public IEnumerator TriggerSingleTile(int x, int y)
    {
        // 1. 유효성 검사
        if (tiles == null || x < 0 || x >= 4 || y < 0 || y >= 4) yield break;

        GameObject targetTile = tiles[y, x];
        if (targetTile == null) yield break;

        SpriteRenderer sr = targetTile.GetComponent<SpriteRenderer>();
        var col = targetTile.GetComponent<Collider2D>();

        // ---------------------------------------------------
        // 2. [경고 단계] 깜빡임 효과 (PingPong)
        // ---------------------------------------------------
        targetTile.SetActive(true);
        AudioManager.instance.PlaySFX(timerSFX, 1f);

        // 안전장치: 노란색일 때는 충돌 끄기
        if (col != null) col.enabled = false;

        float warningDuration = 1f; // 경고 시간 (1초)
        float timer = 0f;
        float blinkSpeed = 10f; // 깜빡이는 속도 (클수록 빠름)

        while (timer < warningDuration)
        {
            timer += Time.deltaTime;

            // 0(투명) ~ 1(불투명) 사이를 계속 왕복(PingPong)
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);

            // 색상 적용 (기본 노란색 + 계산된 알파값)
            Color c = basicColor;
            c.a = alpha;
            sr.color = c;

            yield return null;
        }

        // 깜빡임이 끝나면 확실하게 불투명하게 설정 (빨간색 전환 전)
        Color solidColor = basicColor;
        solidColor.a = 1f;
        sr.color = solidColor;


        // ---------------------------------------------------
        // 3. [공격 단계] 빨간색 + 충돌 켜기
        // ---------------------------------------------------
        dangerColor.a = 0.1f;
        AudioManager.instance.PlaySFX(bombSFX, 1f);
        sr.color = dangerColor; // 빨간색 (알파 1)
        if (col != null) col.enabled = true; // 이제 닿으면 데미지

        // 공격 지속 시간 (0.5초)
        yield return new WaitForSeconds(0.5f);


        // ---------------------------------------------------
        // 4. [종료] 비활성화 + 색상 초기화
        // ---------------------------------------------------
        targetTile.SetActive(false);
        sr.color = basicColor; // 다음을 위해 색상 복구
        if (col != null) col.enabled = false; // 안전하게 끄기
    }
}
