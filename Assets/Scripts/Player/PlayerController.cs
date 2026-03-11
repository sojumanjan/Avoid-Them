using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [Header("Settings")]
    public float moveSpeed;// 이동 애니메이션 속도
    public float gridSpacing = 1.0f; // 한 번 이동 시 움직일 거리
    public int gridWidth = 4; // 그리드 가로 크기
    public int gridHeight = 4; // 그리드 세로 크기

    //게임 시작 시 포지션, 입력 받았을 때 목적지 포지션
    private Vector2 startPos;
    public Vector2 nextPos;

    //플레이어의 그리드 포지션 (맨 왼쪽 아래칸 위치 시 (0, 0) ~ (3, 3))
    private int curX;
    private int curY;

    // 게임이 정지된 상황인가에 대한 불린
    public bool isStopped = false;

    // 참조
    private BombPattern gameManager;
    PlayerHP playerHP;

    // 효과음
    [Header("할당")]
    public AudioClip moveSFX;
    public GameObject diedParticle;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        playerHP = PlayerHP.instance;
        curX = 0;
        curY = 0;
        // 그리드 크기에 의해 시작 지점 자동 계산 (0, 0) 위치.
        float startX = -(float)gridWidth / 2 + 0.5f;
        float startY = -(float)gridHeight / 2 + 0.5f;
        startPos = new Vector2(startX, startY);
        nextPos = startPos;

        gameManager = BombPattern.instance;
        playerHP.onHealthChanged = Damaged;
        playerHP.onDie = Die;
    }

    void Update()
    {
        if (isStopped) return;

        // 상하좌우 입력받기
        HandleInput();
        transform.position = Vector3.Lerp(transform.position, nextPos, Time.deltaTime * moveSpeed);
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            Move(0, 1);
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            Move(0, -1);
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            Move(-1, 0);
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            Move(1, 0);
    }

    // 그리드 좌표와 nextPos를 입력에 따라 지정.
    void Move(int x, int y)
    {
        AudioManager.instance.PlaySFX(moveSFX, 1f);
        // 현재 x, y 그리드를 보고 넘어가면 반대편으로 순간이동 하도록
        curX += x;
        curY += y;
        if (curX >= gridWidth) curX = 0;
        if (curX < 0) curX = gridWidth - 1;
        if (curY >= gridHeight) curY = 0;
        if (curY < 0) curY = gridHeight - 1;
        nextPos = new Vector2(startPos.x + curX * gridSpacing, startPos.y + curY * gridSpacing);
    }

    // 대미지 받았을 때 스프라이트 애니메이션 발동
    void Damaged(int a, int b)
    {
        GetComponent<Animation>().Play();
    }

    void Die()
    {
        GameObject particle = Instantiate(diedParticle, nextPos, Quaternion.identity);
        CameraController.instance.ZoomInOnTarget(nextPos);
        isStopped = true;
    }
}