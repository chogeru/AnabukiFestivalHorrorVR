using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDarkRoom : MonoBehaviour
{
    [SerializeField, Header("変更用曲名")]
    private string m_BGMName;
    [SerializeField, Header("音量")]
    private float m_Volume;
    [SerializeField, Header("闇のプレハブ")]
    private GameObject m_DrakPefabs;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BGMManager.instance.PlayBGMByScene(m_BGMName, m_Volume);
            m_DrakPefabs.SetActive(true);
        }

    }
}
