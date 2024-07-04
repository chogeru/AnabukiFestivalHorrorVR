using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureDetection : MonoBehaviour
{
    [SerializeField,Header("視野の距離")]
    public float m_ViewDistance = 10f;
    [SerializeField,Header("視野の角度")]
    public float m_ViewAngle = 120f;
    public bool isTrigger = false;
    [SerializeField, Header("アニメーショントリガー")]
    private AnimationTrigger m_AnimationTrigger;
    [SerializeField, Header("クリーチャーレイヤー")]
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

            //クリーチャーが視野内にいるかチェック
            if (angleToCreature < m_ViewAngle / 2)
            {
                if (creature != null)
                {
                    //レイキャストを発射しクリーチャーにヒットするかチェック
                    if (RaycastToCreature(creature))
                    {
                        //命中していたらtriggerをtrueに
                        isTrigger = true;
                        m_AnimationTrigger.TriggerAnimation();
                        return;
                    }
                }
            }
        }
        //視野内に居なければfalseに
        isTrigger = false;
    }
    bool RaycastToCreature(Collider creature)
    {
        Vector3 rayOrigin = transform.position + Vector3.up;  // 高さプラス1の位置から発射
        Vector3 directionToCreature = (creature.transform.position - rayOrigin).normalized;
        Ray ray = new Ray(rayOrigin, directionToCreature);
        RaycastHit hit;

        // Debug.DrawRayを使用してレイキャストを可視化
        Debug.DrawRay(rayOrigin, directionToCreature * m_ViewDistance, Color.red);

        // Raycastを発射し、ヒットしたものがクリーチャーかチェック
        if (Physics.Raycast(ray, out hit, m_ViewDistance))
        {
            // ヒットしたオブジェクトがクリーチャータグを持っているか確認
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
