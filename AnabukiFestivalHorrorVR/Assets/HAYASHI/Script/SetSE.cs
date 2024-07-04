using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSE : MonoBehaviour
{
    [SerializeField, Header("再生するSEの名前")]
    private string m_SEClipName;
    private float m_Volume;
    public void PlaySE()
    {
        SEManager.instance.PlaySound(m_SEClipName,m_Volume);
    }

    public void PlaySEButton(string SEClipName)
    {
        SEManager.instance.PlaySound(SEClipName, m_Volume);
    }
}
