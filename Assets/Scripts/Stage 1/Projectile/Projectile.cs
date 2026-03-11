using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed = 5f;
    private Vector2 moveDirection;

    public void Setup(Vector2 direction, float speedVal)
    {
        this.moveDirection = direction;
        this.speed = speedVal;

        // 5초 뒤 자동 삭제 (메모리 관리)
        Destroy(gameObject, 10f);
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}