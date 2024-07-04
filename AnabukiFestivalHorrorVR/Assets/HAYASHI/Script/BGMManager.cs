using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    private AudioSource m_AudioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        m_AudioSource = GetComponent<AudioSource>();
    }

    public void PlayBGMByScene(string bgmFileName, float volume)
    {
        try
        {
            // ResourcesフォルダからBGMファイルをロード
            AudioClip clip = Resources.Load<AudioClip>("BGM/" + bgmFileName);

            if (clip != null)
            {
                // クリップを設定
                m_AudioSource.clip = clip;
                // 音量を設定
                m_AudioSource.volume = volume;
                // 再生
                m_AudioSource.Play();
            }
            else
            {
                DebugUtility.Log("BGMファイルが見つからない: " + bgmFileName);
            }
        }
        catch (Exception ex)
        {
            DebugUtility.LogError("BGMファイルのロード時にエラー発生: " + ex.Message);
        }
    }
}