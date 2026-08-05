using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EffType
{
    Select,
    Button,
    Match,
}

/// <summary>
/// 音乐管理器
/// </summary>
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    // 音效片段
    public AudioClip[] clips;
    // 背景音乐片段
    public AudioClip bgmClip;
    // 音效播放
    private AudioSource effSource;
    // 背景音乐播放
    private AudioSource bgmSource;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    public void PlayEff(EffType type, float volume = 1f)
    {
        if (effSource == null)
        {
            GameObject obj = new GameObject();
            obj.name = "Eff";
            effSource = obj.AddComponent<AudioSource>();
        }
        effSource.Stop();

        int typeInt = (int)type;
        effSource.clip = this.clips[typeInt];
        effSource.volume = volume;
        effSource.Play();
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    public void PlayBgm(float volume = 1f)
    {
        if (bgmSource == null)
        {
            GameObject obj = new GameObject();
            obj.name = "Bgm";
            bgmSource = obj.AddComponent<AudioSource>();
        }
        bgmSource.Stop();
        bgmSource.clip = bgmClip;
        bgmSource.volume = volume;
        bgmSource.loop = true;
        bgmSource.Play();
    }
}
