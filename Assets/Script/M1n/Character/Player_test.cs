using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_test : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float detectionRange = 20.0f; // 시야 범위 변수 추가

    // Update is called once per frame
    void Update()
    {
        // 간단한 키보드 이동 로직
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        // 필요하다면 여기에 감지 로직 추가 가능
    }

    // Gizmos로 시야 범위 시각화 (옵션)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
