using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureDetection : MonoBehaviour
{
    [SerializeField,Header("����̋���")]
    public float m_ViewDistance = 10f;
    [SerializeField,Header("����̊p�x")]
    public float m_ViewAngle = 120f;
    public bool isTrigger = false;
    [SerializeField, Header("�A�j���[�V�����g���K�[")]
    private AnimationTrigger m_AnimationTrigger;
    [SerializeField, Header("�N���[�`���[���C���[")]
    private LayerMask m_CreatureLayer;
    private void Update()
    {
        CheckForCreatures();
    }

    void CheckForCreatures()
    {
        Collider[] creaturesInView = Physics.OverlapSphere(transform.position, m_ViewDistance,m_CreatureLayer);

        foreach(Collider creature in creaturesInView)
        {
            Vector3 directionToCreature = (creature.transform.position - transform.position).normalized;
            float angleToCreature = Vector3.Angle(transform.forward, directionToCreature);

            //�N���[�`���[��������ɂ��邩�`�F�b�N
            if (angleToCreature < m_ViewAngle / 2)
            {
                if (creature != null)
                {
                    //���C�L���X�g�𔭎˂��N���[�`���[�Ƀq�b�g���邩�`�F�b�N
                    if (RaycastToCreature(creature))
                    {
                        //�������Ă�����trigger��true��
                        isTrigger = true;
                        m_AnimationTrigger.TriggerAnimation();
                        return;
                    }
                }
            }
        }
        //������ɋ��Ȃ����false��
        isTrigger = false;
    }
    bool RaycastToCreature(Collider creature)
    {
        Vector3 rayOrigin = transform.position + Vector3.up;  // �����v���X1�̈ʒu���甭��
        Vector3 directionToCreature = (creature.transform.position - rayOrigin).normalized;
        Ray ray = new Ray(rayOrigin, directionToCreature);
        RaycastHit hit;

        // Debug.DrawRay���g�p���ă��C�L���X�g������
        Debug.DrawRay(rayOrigin, directionToCreature * m_ViewDistance, Color.red);

        // Raycast�𔭎˂��A�q�b�g�������̂��N���[�`���[���`�F�b�N
        if (Physics.Raycast(ray, out hit, m_ViewDistance))
        {
            // �q�b�g�����I�u�W�F�N�g���N���[�`���[�^�O�������Ă��邩�m�F
            if (hit.collider == creature)
            {
                m_AnimationTrigger=hit.collider.GetComponent<AnimationTrigger>();
                return true;
            }
        }
        return false;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_ViewDistance);

        Vector3 leftBoundary = Quaternion.Euler(0, -m_ViewAngle / 2, 0) * transform.forward * m_ViewDistance;
        Vector3 rightBoundary = Quaternion.Euler(0, m_ViewAngle / 2, 0) * transform.forward * m_ViewDistance;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }
#endif
}
