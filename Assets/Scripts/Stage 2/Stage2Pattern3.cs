using System.Collections;
using UnityEngine;

public class Stage2Pattern3 : BasePattern
{
    [Header("할당")]
    public GameObject enemy;
    public AudioClip expandSFX;
    public AudioClip laserLoopSFX;

    [Header("설정")]
    public AnimationCurve moveCurve;
    public float laserLength;
    public float laserExpandTime;

    Transform laserTrans;
    GameObject realEnemy;
    Vector2 originPos;
    float currentRotationSpeed = 0f;

    protected override void OnEnable()
    {
        realEnemy = Instantiate(enemy, new Vector2(0, -10f), Quaternion.identity);
        originPos = realEnemy.transform.position;
        base.OnEnable();
    }

    protected override IEnumerator ProcessPattern()
    {
        laserTrans = realEnemy.transform.Find("Laser");
        yield return StartCoroutine(MoveSmoothly(realEnemy, originPos, Vector2.zero, 2f));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(Expand(laserLength));

        // 적 회전 시작.
        StartCoroutine(ApplyRotationLoop());

        // 회전 속도만 조절해주면 된다.
        AudioManager.instance.PlayLoop(laserLoopSFX, 0.4f);
        yield return StartCoroutine(ChangeRotationSpeed(180, 6f));
        yield return StartCoroutine(ChangeRotationSpeed(360, 3f));
        yield return StartCoroutine(ChangeRotationSpeed(360, 3f));
        yield return StartCoroutine(ChangeRotationSpeed(180, 2f));
        yield return StartCoroutine(ChangeRotationSpeed(0, 3f));
        AudioManager.instance.StopLoop();
        yield return StartCoroutine(Expand(0.05f));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(MoveSmoothly(realEnemy, Vector2.zero, originPos, 2f));
        FinishPattern();
        yield return null;
    }

    public IEnumerator MoveSmoothly(GameObject target, Vector3 startPos, Vector3 endPos, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float smoothT = moveCurve.Evaluate(t);
            target.transform.position = Vector3.LerpUnclamped(startPos, endPos, smoothT);

            yield return null;
        }
        target.transform.position = endPos;
    }

    // enemy의 레이저 확장 및 축소
    public IEnumerator Expand(float targetLength)
    {
        AudioManager.instance.PlaySFX(expandSFX, 0.6f);
        float elapsedTime = 0f;
        Vector3 startScale = laserTrans.localScale;

        while (elapsedTime < laserExpandTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / laserExpandTime;
            float newX = Mathf.Lerp(startScale.x, targetLength, t);
            laserTrans.localScale = new Vector3(newX, startScale.y, startScale.z);
            yield return null;
        }
        laserTrans.localScale = new Vector3(targetLength, startScale.y, startScale.z);
    }

    // 각속도 targetSpeed까지 duration초 동안 속도를 변화시킨다.
    public IEnumerator ChangeRotationSpeed(float targetSpeed, float duration)
    {
        float elapsedTime = 0f;
        float startSpeed = currentRotationSpeed;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            currentRotationSpeed = Mathf.Lerp(startSpeed, targetSpeed, t);
            yield return null;
        }
        currentRotationSpeed = targetSpeed;
    }
    IEnumerator ApplyRotationLoop()
    {
        while (true)
        {
            if (realEnemy != null)
            {
                // 실제 적 회전이 일어나는 곳.
                realEnemy.transform.Rotate(0, 0, -currentRotationSpeed * Time.deltaTime);
            }
            yield return null;
        }
    }
}
