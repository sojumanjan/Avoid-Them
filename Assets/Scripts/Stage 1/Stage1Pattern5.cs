using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

// BasePattern ���
public class Stage1Pattern5 : BasePattern
{
    private BombPattern gameManager;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<BombPattern>();
    }
    protected override IEnumerator ProcessPattern()
    {
        StartCoroutine(gameManager.TriggerSingleTile(1, 2));
        StartCoroutine(gameManager.TriggerSingleTile(1, 1));
        StartCoroutine(gameManager.TriggerSingleTile(2, 2));
        StartCoroutine(gameManager.TriggerSingleTile(2, 1));

        yield return new WaitForSeconds(2.0f);

        StartCoroutine(gameManager.TriggerSingleTile(0, 0));
        StartCoroutine(gameManager.TriggerSingleTile(0, 2));
        StartCoroutine(gameManager.TriggerSingleTile(1, 1));
        StartCoroutine(gameManager.TriggerSingleTile(1, 3));
        StartCoroutine(gameManager.TriggerSingleTile(2, 0));
        StartCoroutine(gameManager.TriggerSingleTile(2, 2));
        StartCoroutine(gameManager.TriggerSingleTile(3, 1));
        StartCoroutine(gameManager.TriggerSingleTile(3, 3));

        yield return new WaitForSeconds(2.0f);

        for(int i = 0; i < 4; i++)
        {
            StartCoroutine(gameManager.TriggerSingleTile(i, 0));
            StartCoroutine(gameManager.TriggerSingleTile(i, 1));
            StartCoroutine(gameManager.TriggerSingleTile(i, 2));
            StartCoroutine(gameManager.TriggerSingleTile(i, 3));
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(2.0f);

        for (int i = 0; i < 4; i++)
        {
            StartCoroutine(gameManager.TriggerSingleTile(i, 0));
            StartCoroutine(gameManager.TriggerSingleTile(i, 1));

            StartCoroutine(gameManager.TriggerSingleTile(3-i, 2));
            StartCoroutine(gameManager.TriggerSingleTile(3-i, 3));
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(2f);


        FinishPattern();

    }
}
