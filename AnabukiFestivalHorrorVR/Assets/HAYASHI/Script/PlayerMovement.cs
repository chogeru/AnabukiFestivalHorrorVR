using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField,Header("�ړ����x")]
    private float m_Speed = 2.0f;
    [SerializeField,Header("��]�X�s�[�h")]
    private float m_RotationSpeed = 100.0f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Input�f�o�C�X���擾
        InputDevice rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        InputDevice leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        // �E�X�e�B�b�N�̓��͂��擾
        Vector2 inputAxisRight;
        if (rightHandDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxisRight))
        {
            // �X�e�B�b�N�̓��͂Ɋ�Â��Ĉړ��x�N�g�����v�Z
            Vector3 move = new Vector3(inputAxisRight.x, 0, inputAxisRight.y) * m_Speed;

            // �x���V�e�B��ݒ肵�ĕ����ړ�
            rb.velocity = transform.TransformDirection(move);
        }

        // ���X�e�B�b�N�̓��͂��擾
        Vector2 inputAxisLeft;
        if (leftHandDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxisLeft))
        {
            // ���X�e�B�b�N�̓��͂Ɋ�Â��ĉ���]���v�Z
            float rotation = inputAxisLeft.x * m_RotationSpeed * Time.deltaTime;

            // Rigidbody���g������]����
            rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0, rotation, 0)));
        }
    }
}
