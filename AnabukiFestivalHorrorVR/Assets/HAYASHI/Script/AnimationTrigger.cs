using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    private Animator m_Animator;

    [SerializeField, Header("�A�j���[�V�����p�����[�^�[��")]
    private string m_ParametaName;
    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventManager.OnKillerPranEvent += TriggerAnimation;
    }

    private void OnDisable()
    {
        EventManager.OnKillerPranEvent -= TriggerAnimation;
    }

    public void TriggerAnimation()
    {
        //�w��̃A�j���[�V�����p�����[�^�[��true�ɐݒ�
        m_Animator.SetBool(m_ParametaName, true);
    }
}
