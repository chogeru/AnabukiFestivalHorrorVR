using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRabbitMove : MonoBehaviour
{
    public float detectionRadius = 5.0f; // ������臒l
    public float moveSpeed = 15.0f; // �ړ����x

    private bool shouldMove = false; // �ړ��t���O

    void Update()
    {
        // "Player"�^�O���t���Ă���I�u�W�F�N�g���擾
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            // �v���C���[�Ƃ̋������v�Z
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= detectionRadius)
            {
                // 5m�ȓ��Ƀv���C���[������ꍇ�A�ړ��t���O�𗧂Ă�
                shouldMove = true;
                break;
            }
        }

        // �ړ��t���O�������Ă���ꍇ�A���[�J�����W�ō��Ɉړ�
        if (shouldMove)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.Self);
        }
    }
}
