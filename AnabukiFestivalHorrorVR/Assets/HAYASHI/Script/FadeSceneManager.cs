using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeSceneManager : MonoBehaviour
{
    public static FadeSceneManager Instance;
    [SerializeField, Header("VR用フェード処理")]
    private OVRScreenFade m_OVRScreenFade;
    [SerializeField, Header("フェードアウトしているかどうか")]
    private bool isFadeOut=false;
    private float m_CurrentTime=0.0f;
    //シングルトンパターン
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Update()
    {
        if (isFadeOut)
        {
   
            m_CurrentTime += Time.deltaTime;
            if(m_OVRScreenFade.fadeTime<m_CurrentTime)
            {
                SceneManager.LoadSceneAsync("HMainScene", LoadSceneMode.Single);
            }
        }

    }
    public void FadeSceneChange()
    {
       m_OVRScreenFade.FadeOut();
        isFadeOut = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            FadeSceneChange();
        }
    }
}
