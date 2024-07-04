using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class SEManager : MonoBehaviour
{
    public static SEManager instance;
    private AudioSource audioSource;

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

        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string clipName, float volume)
    {
        try
        {
            // Resourcesフォルダからサウンドファイルをロード
            AudioClip clip = Resources.Load<AudioClip>("SE/" + clipName);

            if (clip != null)
            {
                audioSource.volume = volume;
                // サウンドを再生
                audioSource.PlayOneShot(clip);
            }
            else
            {
                DebugUtility.Log("サウンドファイルが見つからない: " + clipName);
            }
        }
        catch (Exception ex)
        {
            DebugUtility.LogError("サウンドファイルのロード時にエラー発生: " + ex.Message);
        }
    }
}