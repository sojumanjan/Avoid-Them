using UnityEngine;
using System.Collections;

public class GridBomb : MonoBehaviour
{
    [Header("Settings")]
    public float fuseTime; // Æø¹ß±îÁö °É¸®´Â ½Ã°£
    public float blinkSpeed; // ±ôºýÀÌ´Â ¼Óµµ
    public AudioClip timerSFX;
    public AudioClip bombSFX;

    private SpriteRenderer spriteRenderer;
    private BombPattern gameManager;
    private int targetGridX, targetGridY; // Æø¹ßÇÒ ±âÁØ ÁÂÇ¥ (¿ÞÂÊ ¾Æ·¡)

    public void Setup(int gridX, int gridY)
    {
        targetGridX = gridX;
        targetGridY = gridY;

        // 2ÃÊ µÚ Æø¹ß ½ÃÀÛ
        StartCoroutine(BombRoutine());
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindFirstObjectByType<BombPattern>();
    }

    IEnumerator BombRoutine()
    {
        float timer = 0;
        AudioManager.instance.PlaySFX(timerSFX, 1f);

        // 1. Ä«¿îÆ®´Ù¿î (±ôºý°Å¸² È¿°ú)
        while (timer < fuseTime)
        {
            timer += Time.deltaTime;

            // ±ôºýÀÓ: »¡°£»ö <-> Èò»ö ¿Ô´Ù°¬´Ù
            float lerp = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            spriteRenderer.color = Color.Lerp(Color.white, Color.red, lerp);

            yield return null;
        }

        // 2. Æø¹ß! (GameManager¿¡°Ô À§ÀÓ)
        spriteRenderer.enabled = false; // ÆøÅº º»Ã¼´Â ¼û±è
        AudioManager.instance.PlaySFX(bombSFX, 1f);

        // ¸Å´ÏÀú¿¡°Ô 2x2 ¿µ¿ª Æø¹ß ¿äÃ»
        yield return StartCoroutine(FindFirstObjectByType<BombPattern>().TriggerSpecificBomb(targetGridX, targetGridY));

        // 3. Á¦°Å
        Destroy(gameObject);
    }
}