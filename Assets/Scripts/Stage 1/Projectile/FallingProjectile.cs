using UnityEngine;

public class FallingProjectile : MonoBehaviour
{
    // 중력 가속도 (값이 클수록 빨리 떨어짐)
    private float gravity;
    private float speed=0;

    public void Setup(Vector2 startVelocity, float gravityScale)
    {
        gravity = gravityScale;

        // 5초 뒤 삭제
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        speed += gravity * Time.deltaTime;
        // 2. 이동 적용: 계산된 속도만큼 이동
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

}