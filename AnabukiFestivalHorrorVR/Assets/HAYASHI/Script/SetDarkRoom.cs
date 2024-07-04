using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDarkRoom : MonoBehaviour
{
    [SerializeField, Header("�ύX�p�Ȗ�")]
    private string m_BGMName;
    [SerializeField, Header("����")]
    private float m_Volume;
    [SerializeField, Header("�ł̃v���n�u")]
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
