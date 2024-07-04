using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRabbitMove : MonoBehaviour
{
    public float detectionRadius = 5.0f; // 距離の閾値
    public float moveSpeed = 15.0f; // 移動速度

    private bool shouldMove = false; // 移動フラグ

    void Update()
    {
        // "Player"タグが付いているオブジェクトを取得
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            // プレイヤーとの距離を計算
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= detectionRadius)
            {
                // 5m以内にプレイヤーがいる場合、移動フラグを立てる
                shouldMove = true;
                break;
            }
        }

        // 移動フラグが立っている場合、ローカル座標で左に移動
        if (shouldMove)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.Self);
        }
    }
}
