using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureDetection : MonoBehaviour
{
    [SerializeField,Header("����̋���")]
    public float m_ViewDistance = 10f;
    [SerializeField,Header("����̊p�x")]
    public float m_ViewAngle = 120f;
    [SerializeField,Header("�N���[�`���[���m���C���[")]
    public LayerMask m_CreatureLayer;
    [SerializeField,Header("��Q���̃��C���[")]
    public LayerMask m_ObstacleLayer;
    public bool isTrigger = false;

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
                //���C�L���X�g�𔭎˂��N���[�`���[�Ƀq�b�g���邩�`�F�b�N
                if (RaycastToCreature(creature))
                {
                    //�������Ă�����trigger��true��
                    isTrigger = true;
                    return;
                }
            }
        }
        //������ɋ��Ȃ����false��
        isTrigger = false;
    }
    bool RaycastToCreature(Collider creature)
    {
        Vector3 directionToCreature = (creature.transform.position - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, directionToCreature, out hit, m_ViewDistance, ~m_ObstacleLayer))
        {
            //�N���[�`���[�^�O�ɖ����������ǂ���
            if (hit.collider.CompareTag("Creature"))
            {
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
