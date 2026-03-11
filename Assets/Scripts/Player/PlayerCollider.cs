using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    void Update()
    {
        transform.position = PlayerController.instance.nextPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 현재 무적 상태가 아니고 장애물에 부딫힌다면 1대미지 줬다고 알리기
        if (collision.transform.CompareTag("Obstacle"))
        {
            PlayerHP.instance.TakeDamage(1);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            PlayerHP.instance.TakeDamage(1);
        }
    }
}
