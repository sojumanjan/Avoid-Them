// CameraController.cs
using DG.Tweening;
using UnityEngine;
using static UnityEditorInternal.ReorderableList;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    [Header("줌 설정")]
    public float zoomDuration;
    public float targetOrthoSize = 2f;
    public Ease zoomEase = Ease.OutExpo;
    
    private float defaultSize;
    private Vector3 defaultPos;
    private Camera cam;

    private void Awake()
    {
        instance = this;
        cam = GetComponent<Camera>();
    }
    private void Start()
    {
        defaultSize = cam.orthographicSize;
        defaultPos = transform.position;
    }

    public void ShakeCamera()
    {
        Camera.main.transform.DOKill();
        Camera.main.transform.DOShakePosition(0.2f, new Vector3(0.2f, 0.2f, 0), 20, 90, false, true)
               .SetUpdate(true);
    }

    public void ZoomInOnTarget(Vector3 targetPos)
    {
        transform.DOKill();
        Vector3 finalPos = new Vector3(targetPos.x, targetPos.y, -10f);

        cam.transform.DOMove(finalPos, zoomDuration)
            .SetEase(zoomEase)
            .SetUpdate(true);

        cam.DOOrthoSize(targetOrthoSize, zoomDuration)
            .SetEase(zoomEase)
            .SetUpdate(true);
    }

    // 게임 재시작할 때 카메라 원상복구용
    public void ResetCamera()
    {
        transform.DOKill();
        transform.position = defaultPos;
        cam.orthographicSize = defaultSize;
    }
}