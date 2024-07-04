using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRotation : MonoBehaviour
{
    public string playerTag = "Player"; // プレイヤータグ
    public float rotationSpeed = 1f;    // 回転速度
    public float rotationAngle = 90f;   // 回転角度
    public float detectionRange = 2f;   // 検出範囲
    public AnimationCurve easingCurve;  // イージングカーブ
    public string m_ClipName;
    public float m_Volume;
    private bool hasRotated = false;    // 回転済みかどうか
    private float rotationProgress = 0f; // 回転進捗
    private Quaternion initialRotation;  // 初期の回転
    private Quaternion targetRotation;   // 目標の回転


    void Start()
    {
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        if (!hasRotated)
        {
            // プレイヤーとの距離をチェック
            GameObject player = GameObject.FindGameObjectWithTag(playerTag);
            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance <= detectionRange)
                {
                    // 回転開始
                    targetRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y + rotationAngle, transform.localEulerAngles.z);
                    hasRotated = true;
                    SEManager.instance.PlaySound(m_ClipName, m_Volume);
                }
            }
        }

        if (hasRotated && rotationProgress < 1f)
        {
            // 回転処理
            rotationProgress += Time.deltaTime * rotationSpeed;
            float easedProgress = easingCurve.Evaluate(rotationProgress);
            transform.localRotation = Quaternion.Lerp(initialRotation, targetRotation, easedProgress);

            // 回転が完了したかチェック
            if (rotationProgress >= 1f)
            {
                transform.localRotation = targetRotation;
            }
        }
    }
}
