using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ResPath("AudioManager")]
public class AudioManager : MonoBehaviour
{
    private static AudioManager mInstance = null;
    public static AudioManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                ResPathAttribute attr = typeof(AudioManager).GetAttribute<ResPathAttribute>();
                if (attr == null) { return null; }

                var prefab = Resources.Load(attr.ResourcePath);
                if (prefab == null) { return null; }

                var gameObj = GameObject.Instantiate(prefab);
                mInstance = gameObj.GetComponent<AudioManager>();

            }
            return mInstance;
        }
    }

    [Header("音量設定-基礎")]
    [Range(0f, 3f)] public float basicBgmVolume = 1f;
    [Range(0f, 3f)] public float basicSfxVolume = 1f;

    [Header("音量設定")]
    [ReadOnly, Range(0f, 1f)] public float bgmVolume = 1f;
    [ReadOnly, Range(0f, 1f)] public float sfxVolume = 1f;

    

    private AudioSource bgmSource;
    private List<AudioSource> sfxSources = new List<AudioSource>();

    private void Awake()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        bgmSource.volume = bgmVolume;
    }

    // 播放背景音樂
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        bgmSource.clip = clip;
        bgmSource.volume = bgmVolume * basicBgmVolume;
        bgmSource.Play();
    }

    public void PlayBGM(string path)
    {
        if (path == null) return;
        AudioClip clip = Resources.Load<AudioClip>(path);
        if (clip == null)
        {
            Debug.LogError($"無法加載背景音樂: {path}");
            return;
        }

        PlayBGM(clip);
    }

// 停止背景音樂
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // 播放音效
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        AudioSource sfx = gameObject.AddComponent<AudioSource>();
        sfx.clip = clip;
        sfx.volume = sfxVolume * basicSfxVolume;
        sfx.Play();
        sfxSources.Add(sfx);
        StartCoroutine(RemoveSFXSourceWhenDone(sfx));
    }

    public void PlaySFX(string path)
    {
        AudioClip clip = Resources.Load<AudioClip>(path);
        if (clip == null)
        {
            Debug.LogError($"無法加載音效: {path}");
            return;
        }
        PlaySFX(clip);
    }

    // 靜音/取消靜音
    public void SetMute(bool mute)
    {
        bgmSource.mute = mute;
        foreach (var sfx in sfxSources)
        {
            if (sfx != null) sfx.mute = mute;
        }
    }

    // 調整音量
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        bgmSource.volume = bgmVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        foreach (var sfx in sfxSources)
        {
            if (sfx != null) sfx.volume = sfxVolume;
        }
    }

    private System.Collections.IEnumerator RemoveSFXSourceWhenDone(AudioSource sfx)
    {
        yield return new WaitUntil(() => !sfx.isPlaying);
        sfxSources.Remove(sfx);
        Destroy(sfx);
    }
}
