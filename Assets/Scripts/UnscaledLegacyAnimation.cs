using UnityEngine;

public class UnscaledLegacyAnimation : MonoBehaviour
{
    // Time.timeScale이 0이 되어도 재생되어야하는 애니메이션이 있는 오브젝트에 추가하면 강제로 실행시켜줌.
    private Animation anim;

    void Awake()
    {
        anim = GetComponent<Animation>();
    }

    void Update()
    {
        // 게임 시간이 멈췄을 때(0)만 작동!
        if (Time.timeScale == 0f && anim != null)
        {
            // 현재 재생 중인 애니메이션이 있다면
            if (anim.isPlaying)
            {
                // 모든 애니메이션 상태를 돌면서
                foreach (AnimationState state in anim)
                {
                    if (state.enabled)
                    {
                        state.time += Time.unscaledDeltaTime;
                    }
                }
                anim.Sample();
            }
        }
    }
}