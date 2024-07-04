using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField,Header("移動速度")]
    private float m_Speed = 2.0f;
    [SerializeField,Header("回転スピード")]
    private float m_RotationSpeed = 100.0f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Inputデバイスを取得
        InputDevice rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        InputDevice leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        // 右スティックの入力を取得
        Vector2 inputAxisRight;
        if (rightHandDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxisRight))
        {
            // スティックの入力に基づいて移動ベクトルを計算
            Vector3 move = new Vector3(inputAxisRight.x, 0, inputAxisRight.y) * m_Speed;

            // ベロシティを設定して物理移動
            rb.velocity = transform.TransformDirection(move);
        }

        // 左スティックの入力を取得
        Vector2 inputAxisLeft;
        if (leftHandDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxisLeft))
        {
            // 左スティックの入力に基づいて横回転を計算
            float rotation = inputAxisLeft.x * m_RotationSpeed * Time.deltaTime;

            // Rigidbodyを使った回転処理
            rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0, rotation, 0)));
        }
    }
}
