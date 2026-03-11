using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHP : MonoBehaviour
{
    public int maxHp = 5;
    private int currentHp;
    public bool isInvincible = false;
    public float invincibleTime;

    // 맞았을 때 현재 체력과 함께 방송
    public Action<int, int> onHealthChanged;

    // 죽었다는거 방송
    public Action onDie;

    // 참조
    public static PlayerHP instance;
    public AudioClip damagedSFX;
    public AudioClip damagedLastSFX;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    void Start()
    {
        currentHp = maxHp;
        isInvincible = false;
    }

    // 장애물과 충돌 시 무적 고려해서 대미지 주기.
    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        // 맞는 순간 무적판정이 아니라면 체력 깎고 무적 On
        currentHp -= damage;
        isInvincible = true;
        Debug.Log("플레이어 체력: " + currentHp);

        // 한대 맞았다고 알리고 무적시간 이후 무적 해제하기
        StartCoroutine(ChangeInvinciblity());
        onHealthChanged?.Invoke(maxHp, currentHp);
        // 알 수 없는 이유로 방송 연결이 안되어 수동 호출.
        LevelManager.instance.UpdateHP(maxHp, currentHp, isDamaged : true);
        CameraController.instance.ShakeCamera();

        // 체력이 다 달면 파괴 후 죽었다고 알리고 맞는 소리내기.
        if (currentHp <= 0)
        {
            AudioManager.instance.PlaySFX(damagedLastSFX, 1f);
            onDie?.Invoke();
            LevelManager.instance.ProcessDie();
        }
        else
        {
            AudioManager.instance.PlaySFX(damagedSFX, 1f);
        }
    }
    
    // 무적시간 기다린 후 무적판정 off하기
    IEnumerator ChangeInvinciblity()
    {
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
}