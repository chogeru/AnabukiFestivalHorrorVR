using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotePlayer : MonoBehaviour
{
    private Transform playerTransform;
    public float rotationSpeed = 5f; // 回転速度

    private void Start()
    {
        // タグを持つオブジェクトを探してTransformを取得
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // プレイヤーの方向を計算
        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0; // Y軸の回転を無視

        if (direction != Vector3.zero) // directionがゼロベクトルでない場合のみ回転する
        {
            // 目標の回転を計算
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // 現在の回転から目標の回転へ補間する
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // 回転の制御（y軸のみ回転するように修正）
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }
}