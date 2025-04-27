using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float pushDistance = 0.5f; // 움찔할 거리
    public float pushDuration = 0.2f; // 움찔 애니메이션 시간
    public float checkRadius = 0.1f; // 플레이어가 있는지 확인할 반지름

    private Vector3 originalPosition;
    private bool hasPushed = false; // 첫 번째 밀림이 일어났는지 여부
    private bool isPushedBack = false; // 원위치로 복귀한 상태인지 여부

    private void Start()
    {
        // 게임 시작 시 오브젝트의 원래 위치 저장
        originalPosition = transform.position;
    }

    private void Update()
    {
        // 플레이어가 원위치에 있으면 다시 밀림을 시작할 수 있도록 상태 초기화
        if (!CheckPlayer() && hasPushed && isPushedBack)
        {
            hasPushed = false;
            isPushedBack = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 첫 번째 충돌 시만 밀리도록 처리
            if (!hasPushed)
            {
                hasPushed = true;

                // 플레이어와 충돌 시 밀리기
                HandlePushReaction(other);
            }
        }
    }

    private void HandlePushReaction(Collider playerCollider)
    {
        // 플레이어 위치와 오브젝트 위치를 기준으로 반대 방향 계산
        Vector3 pushDirection = (transform.position - playerCollider.transform.position).normalized;

        // 움찔 애니메이션
        StartCoroutine(PushAndReturn(pushDirection));
    }

    private IEnumerator PushAndReturn(Vector3 direction)
    {
        // 움찔 방향으로 이동
        Vector3 targetPosition = transform.position + direction * pushDistance;
        float elapsedTime = 0f;

        // 밀기 애니메이션
        while (elapsedTime < pushDuration)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, elapsedTime / pushDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        // 원위치로 돌아가려고 할 때, 그 자리에 플레이어가 있으면 돌아가지 않음
        if (!isPushedBack)
        {
            elapsedTime = 0f;
            // 원위치로 돌아가기 전에 플레이어가 있는지 확인
            while (elapsedTime < pushDuration)
            {
                // 원위치에 플레이어가 없을 경우에만 돌아옴
                if (!CheckPlayer())
                {
                    transform.position = Vector3.Lerp(transform.position, originalPosition, elapsedTime / pushDuration);
                    elapsedTime += Time.deltaTime;
                }
                yield return null;
            }

            transform.position = originalPosition;
            isPushedBack = true;
        }
    }

    bool CheckPlayer()
    {
        return Physics.CheckSphere(originalPosition, checkRadius, LayerMask.GetMask("Player"));
    }
}
