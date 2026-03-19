using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHP : MonoBehaviour
{
    public int maxHp = 3;
    public int curHP;
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
        maxHp += UpgradeManager.instance.hpState * 2; // hp 업글 활성화 시 최대 체력 2만큼 늘어남.
        curHP = maxHp;
    }
    void Start()
    {
        isInvincible = false;
    }

    // 장애물과 충돌 시 무적 고려해서 대미지 주기.
    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        // 맞는 순간 무적판정이 아니라면 체력 깎고 무적 On
        curHP -= damage;
        isInvincible = true;
        Debug.Log("플레이어 체력: " + curHP);

        // 한대 맞았다고 알리고 무적시간 이후 무적 해제하기
        StartCoroutine(ChangeInvinciblity());
        onHealthChanged?.Invoke(maxHp, curHP);
        // 알 수 없는 이유로 방송 연결이 안되어 수동 호출.
        LevelManager.instance.UpdateHP(curHP, isDamaged : true);
        CameraController.instance.ShakeCamera();

        // 체력이 다 달면 파괴 후 죽었다고 알리고 맞는 소리내기.
        if (curHP <= 0)
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