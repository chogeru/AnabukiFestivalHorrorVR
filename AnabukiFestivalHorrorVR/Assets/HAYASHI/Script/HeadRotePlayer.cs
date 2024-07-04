using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotePlayer : MonoBehaviour
{
    private Transform playerTransform;
    public float rotationSpeed = 5f; // ��]���x

    private void Start()
    {
        // �^�O�����I�u�W�F�N�g��T����Transform���擾
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // �v���C���[�̕������v�Z
        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0; // Y���̉�]�𖳎�

        if (direction != Vector3.zero) // direction���[���x�N�g���łȂ��ꍇ�̂݉�]����
        {
            // �ڕW�̉�]���v�Z
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // ���݂̉�]����ڕW�̉�]�֕�Ԃ���
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // ��]�̐���iy���̂݉�]����悤�ɏC���j
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }
}