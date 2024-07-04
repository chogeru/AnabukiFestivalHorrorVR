using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRotation : MonoBehaviour
{
    public string playerTag = "Player"; // �v���C���[�^�O
    public float rotationSpeed = 1f;    // ��]���x
    public float rotationAngle = 90f;   // ��]�p�x
    public float detectionRange = 2f;   // ���o�͈�
    public AnimationCurve easingCurve;  // �C�[�W���O�J�[�u
    public string m_ClipName;
    public float m_Volume;
    private bool hasRotated = false;    // ��]�ς݂��ǂ���
    private float rotationProgress = 0f; // ��]�i��
    private Quaternion initialRotation;  // �����̉�]
    private Quaternion targetRotation;   // �ڕW�̉�]


    void Start()
    {
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        if (!hasRotated)
        {
            // �v���C���[�Ƃ̋������`�F�b�N
            GameObject player = GameObject.FindGameObjectWithTag(playerTag);
            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance <= detectionRange)
                {
                    // ��]�J�n
                    targetRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y + rotationAngle, transform.localEulerAngles.z);
                    hasRotated = true;
                    SEManager.instance.PlaySound(m_ClipName, m_Volume);
                }
            }
        }

        if (hasRotated && rotationProgress < 1f)
        {
            // ��]����
            rotationProgress += Time.deltaTime * rotationSpeed;
            float easedProgress = easingCurve.Evaluate(rotationProgress);
            transform.localRotation = Quaternion.Lerp(initialRotation, targetRotation, easedProgress);

            // ��]�������������`�F�b�N
            if (rotationProgress >= 1f)
            {
                transform.localRotation = targetRotation;
            }
        }
    }
}
