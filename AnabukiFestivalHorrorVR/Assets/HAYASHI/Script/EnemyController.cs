using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform player;  // プレイヤーのTransform
    public float detectionRange = 10f;  // 敵がプレイヤーを検知する範囲
    public Camera playerCamera;  // プレイヤーを視認するカメラ
    private NavMeshAgent navMeshAgent;
    private Animation animationComponent;
    private bool playerInCameraView = false;  // プレイヤーがカメラの視界にいるかどうか
    [SerializeField, Header("ジャンプスケア用model")]
    private GameObject m_JumpScareModel;
    [SerializeField, Header("シーンマネージャー")]
    private FadeSceneManager m_FadeSceneManager;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animationComponent = GetComponent<Animation>();
        m_JumpScareModel.SetActive(false);
    }

    void Update()
    {
        bool enemyInCameraView = IsEnemyInCameraView(); // 敵自身がカメラの視界内にいるかどうかを確認
        playerInCameraView = IsPlayerInCameraView(); // プレイヤーがカメラの視界内にいるかどうかを確認
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        Debug.Log($"敵のビューポート座標: {playerCamera.WorldToViewportPoint(transform.position)}, カメラに映っている: {enemyInCameraView}");
        Debug.Log($"プレイヤーのビューポート座標: {playerCamera.WorldToViewportPoint(player.position)}, カメラに映っている: {playerInCameraView}");
        Debug.Log($"プレイヤーとの距離: {distanceToPlayer}");

        if (enemyInCameraView || distanceToPlayer > detectionRange)
        {
            StopMovement();
            Debug.Log("停止");
        }
        else
        {
            MoveTowardsPlayer();
            Debug.Log("プレイヤーに向かって移動");
        }

        // デバッグ用の視認状態の表示
        Debug.Log($"プレイヤーがカメラに映っている: {playerInCameraView}");
        Debug.Log($"プレイヤーが検知範囲内にいる: {distanceToPlayer <= detectionRange}");
    }

    bool IsPlayerInDetectionRange()
    {
        return Vector3.Distance(transform.position, player.position) <= detectionRange;
    }

    bool IsEnemyInCameraView()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("プレイヤーカメラが設定されていません。");
            return false;
        }

        Vector3 viewportPoint = playerCamera.WorldToViewportPoint(transform.position);
        bool inView = viewportPoint.z > 0 && viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1;
        Debug.Log($"敵のビューポート座標: {viewportPoint}, カメラに映っている: {inView}");
        return inView;
    }

    bool IsPlayerInCameraView()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("プレイヤーカメラが設定されていません。");
            return false;
        }

        Vector3 viewportPoint = playerCamera.WorldToViewportPoint(player.position);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // プレイヤーとの距離が近すぎる場合は、カメラの視界外とみなす
        bool inView = viewportPoint.z > 0 && viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1 && distanceToPlayer > 1f;
        Debug.Log($"プレイヤーのビューポート座標: {viewportPoint}, カメラに映っている: {inView}");
        return inView;
    }

    void MoveTowardsPlayer()
    {
        navMeshAgent.isStopped = false;  // navMeshAgentの停止を解除
        navMeshAgent.SetDestination(player.position);
        RotateTowardsPlayer();
        if (animationComponent != null)
        {
            animationComponent.Play();  // アニメーションを再生
        }
    }

    void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(-directionToPlayer.x, 0, -directionToPlayer.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * navMeshAgent.angularSpeed);
    }

    void StopMovement()
    {
        navMeshAgent.isStopped = true;  // navMeshAgentを停止
        navMeshAgent.velocity = Vector3.zero;
        if (animationComponent != null)
        {
            animationComponent.Stop();  // アニメーションを停止
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
            m_JumpScareModel.SetActive(true);
            m_FadeSceneManager.FadeSceneChange();
        }
    }
}