using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 이동 감지용

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("BGM")]
    public AudioSource bgmSource; // 배경음용 스피커 (Loop 체크 필수)
    public AudioClip[] bgms;
    private List<AudioClip> playlist = new List<AudioClip>();

    [Header("SFX")]
    public AudioSource sfxSource; // 효과음용 스피커
    public AudioSource loopSource; // 효과음 유지용 스피커

    // 같은 소리가 다시 재생되기 위한 최소 대기 시간
    [Header("설정")]
    public float sfxCooldown = 0.05f;

    private Dictionary<AudioClip, float> sfxTimer = new Dictionary<AudioClip, float>();
    public AudioClip clickSFX;

    public float SFXVolume;
    public float BGMVolume;


    private void Awake()
    {
        // 수직 동기화 끄기 (이걸 꺼야 targetFrameRate가 먹힘)
        QualitySettings.vSyncCount = 0;

        // 목표 프레임을 60으로 고정 (모바일/PC 국룰)
        Application.targetFrameRate = 60;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadVolumePrefs();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bgmSource.Stop();
        playlist.Clear();
        PlayNextTrack();
    }

    void Update()
    {
        // 노래가 끝났는지 체크 (재생 중이 아니고 + 오디오 소스가 켜져있다면)
        if (!bgmSource.isPlaying && bgmSource.clip != null)
        {
            if (bgmSource.time == 0)
            {
                PlayNextTrack();
            }
        }
    }

    // 배경음 재생
    public void PlayNextTrack()
    {
        // 리스트가 비었으면 리필하고 섞기
        if (playlist.Count == 0)
        {
            RefillAndShuffle();
        }
        // 안비었으면 맨 앞에꺼 플레이하고 삭제하기
        AudioClip nextClip = playlist[0];
        playlist.RemoveAt(0);
        bgmSource.clip = nextClip;
        bgmSource.Play();
    }

    // 리스트 비우고 원본에서 다시 채워넣기
    void RefillAndShuffle()
    {
        playlist.Clear();
        playlist.AddRange(bgms);
        int n = playlist.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            AudioClip value = playlist[k];
            playlist[k] = playlist[n];
            playlist[n] = value;
        }
    }

    // 효과음 재생
    public void PlaySFX(AudioClip clip, float volume = 1f, bool isUISound = false)
    {
        if (clip == null) return;

        if (sfxTimer.ContainsKey(clip))
        {
            float lastPlayTime = sfxTimer[clip];
            if (Time.unscaledTime - lastPlayTime < sfxCooldown)
                return; // 쿨타임 안 지났으면 무시
            sfxTimer[clip] = Time.unscaledTime;
        }
        else
        {
            sfxTimer.Add(clip, Time.unscaledTime);
        }
        if (isUISound) sfxSource.pitch = 1f;
        else sfxSource.pitch = Random.Range(0.95f, 1.05f);
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayLoop(AudioClip clip, float volume = 1f)
    {
        if (loopSource.isPlaying && loopSource.clip == clip) return;

        loopSource.clip = clip;
        loopSource.volume = volume;
        loopSource.loop = true;  //무한 반복 켜기
        loopSource.Play();
    }

    // 2. 반복 재생 정지 함수
    public void StopLoop()
    {
        loopSource.Stop();
        loopSource.clip = null; // 클립 비워주기
    }
    public void PlayClickSFX()
    {
        sfxSource.pitch = 1f;
        PlaySFX(clickSFX, 1f, true);
    }

    public void SetBGMVolume(float value)
    {
        BGMVolume = value;
        bgmSource.volume = BGMVolume;
        PlayerPrefs.SetFloat("BGMVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        SFXVolume = value;
        sfxSource.volume = SFXVolume;
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void PauseLoop()
    {
        loopSource.Pause();
    }

    public void ResumeLoop()
    {
        loopSource.UnPause();
    }

    void LoadVolumePrefs()
    {
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        BGMVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        bgmSource.volume = BGMVolume;
        sfxSource.volume = SFXVolume;
        if (loopSource != null) loopSource.volume = SFXVolume;
    }
}