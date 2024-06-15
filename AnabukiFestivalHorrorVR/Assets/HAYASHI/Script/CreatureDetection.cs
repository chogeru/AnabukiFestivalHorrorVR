using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureDetection : MonoBehaviour
{
    [SerializeField,Header("視野の距離")]
    public float m_ViewDistance = 10f;
    [SerializeField,Header("視野の角度")]
    public float m_ViewAngle = 120f;
    [SerializeField,Header("クリーチャー検知レイヤー")]
    public LayerMask m_CreatureLayer;
    [SerializeField,Header("障害物のレイヤー")]
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

            //クリーチャーが視野内にいるかチェック
            if (angleToCreature < m_ViewAngle / 2)
            {
                //レイキャストを発射しクリーチャーにヒットするかチェック
                if (RaycastToCreature(creature))
                {
                    //命中していたらtriggerをtrueに
                    isTrigger = true;
                    return;
                }
            }
        }
        //視野内に居なければfalseに
        isTrigger = false;
    }
    bool RaycastToCreature(Collider creature)
    {
        Vector3 directionToCreature = (creature.transform.position - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, directionToCreature, out hit, m_ViewDistance, ~m_ObstacleLayer))
        {
            //クリーチャータグに命中したかどうか
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
