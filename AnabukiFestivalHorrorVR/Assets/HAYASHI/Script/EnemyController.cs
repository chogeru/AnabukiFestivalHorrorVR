using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform player;  // �v���C���[��Transform
    public float detectionRange = 10f;  // �G���v���C���[�����m����͈�
    public Camera playerCamera;  // �v���C���[�����F����J����
    private NavMeshAgent navMeshAgent;
    private Animation animationComponent;
    private bool playerInCameraView = false;  // �v���C���[���J�����̎��E�ɂ��邩�ǂ���
    [SerializeField, Header("�W�����v�X�P�A�pmodel")]
    private GameObject m_JumpScareModel;
    [SerializeField, Header("�V�[���}�l�[�W���[")]
    private FadeSceneManager m_FadeSceneManager;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animationComponent = GetComponent<Animation>();
        m_JumpScareModel.SetActive(false);
    }

    void Update()
    {
        bool enemyInCameraView = IsEnemyInCameraView(); // �G���g���J�����̎��E���ɂ��邩�ǂ������m�F
        playerInCameraView = IsPlayerInCameraView(); // �v���C���[���J�����̎��E���ɂ��邩�ǂ������m�F
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        Debug.Log($"�G�̃r���[�|�[�g���W: {playerCamera.WorldToViewportPoint(transform.position)}, �J�����ɉf���Ă���: {enemyInCameraView}");
        Debug.Log($"�v���C���[�̃r���[�|�[�g���W: {playerCamera.WorldToViewportPoint(player.position)}, �J�����ɉf���Ă���: {playerInCameraView}");
        Debug.Log($"�v���C���[�Ƃ̋���: {distanceToPlayer}");

        if (enemyInCameraView || distanceToPlayer > detectionRange)
        {
            StopMovement();
            Debug.Log("��~");
        }
        else
        {
            MoveTowardsPlayer();
            Debug.Log("�v���C���[�Ɍ������Ĉړ�");
        }

        // �f�o�b�O�p�̎��F��Ԃ̕\��
        Debug.Log($"�v���C���[���J�����ɉf���Ă���: {playerInCameraView}");
        Debug.Log($"�v���C���[�����m�͈͓��ɂ���: {distanceToPlayer <= detectionRange}");
    }

    bool IsPlayerInDetectionRange()
    {
        return Vector3.Distance(transform.position, player.position) <= detectionRange;
    }

    bool IsEnemyInCameraView()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("�v���C���[�J�������ݒ肳��Ă��܂���B");
            return false;
        }

        Vector3 viewportPoint = playerCamera.WorldToViewportPoint(transform.position);
        bool inView = viewportPoint.z > 0 && viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1;
        Debug.Log($"�G�̃r���[�|�[�g���W: {viewportPoint}, �J�����ɉf���Ă���: {inView}");
        return inView;
    }

    bool IsPlayerInCameraView()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("�v���C���[�J�������ݒ肳��Ă��܂���B");
            return false;
        }

        Vector3 viewportPoint = playerCamera.WorldToViewportPoint(player.position);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // �v���C���[�Ƃ̋������߂�����ꍇ�́A�J�����̎��E�O�Ƃ݂Ȃ�
        bool inView = viewportPoint.z > 0 && viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1 && distanceToPlayer > 1f;
        Debug.Log($"�v���C���[�̃r���[�|�[�g���W: {viewportPoint}, �J�����ɉf���Ă���: {inView}");
        return inView;
    }

    void MoveTowardsPlayer()
    {
        navMeshAgent.isStopped = false;  // navMeshAgent�̒�~������
        navMeshAgent.SetDestination(player.position);
        RotateTowardsPlayer();
        if (animationComponent != null)
        {
            animationComponent.Play();  // �A�j���[�V�������Đ�
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
        navMeshAgent.isStopped = true;  // navMeshAgent���~
        navMeshAgent.velocity = Vector3.zero;
        if (animationComponent != null)
        {
            animationComponent.Stop();  // �A�j���[�V�������~
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