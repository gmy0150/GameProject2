using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : StorageItem
{
    float maxThrowDistance;
    float maxThrowForce;
    LineRenderer lineRenderer;
    float gravity = -9.81f;
    GameObject CoinPrefab;
     bool shoot = false;
     bool hasCoin = false;


    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
        GetCoin(); // 🪙 아이템 확인 코드 - 아이템 획득 처리
        
        maxThrowDistance = character.maxThrowDistance;
        maxThrowForce = character.maxThrowForce;
        lineRenderer = character.lineRenderer;
        CoinPrefab = character.Prefab;
    }

    public override bool CanInteract()
    {
        return !shoot && !hasCoin;
    }

    public override void InteractAgain()
    {
        base.InteractAgain();
        if (!shoot)
        {
            filter.mesh = Base;
            gameObject.SetActive(false);
            UseCoin();
        }
        else
        {
            ShootCoin();
        }
    }

    public void UseCoin()
    {
        ShootCoin();
        shoot = false;
    }

    public void ShootCoin()
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0;
        }
        hasCoin = false;
    }

        public void GetCoin()
    {
        hasCoin = true;

        // ✅🪙 아이템 확인 코드 - JSON에서 여러 대사와 표정 정보 가져오기
        var data = MessageManager.Instance.GetItemMessage(itemname);
        if (data != null && data.lines != null && data.lines.Count > 0)
        {
            ItemAlertUI.Instance.ShowDialogue(data.lines); // 여러 줄 출력
        }
    }


    Vector3 dir;
    float throwF;

    public void HasCoin()
    {
        if (hasCoin)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Vector3 targetPoint;
            Vector3 ChTrans = character.transform.position;
            float newY = 0.5f;
            ChTrans.y = newY;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.origin + ray.direction * maxThrowDistance;
            }

            targetPoint.y = ChTrans.y;

            Vector3 throwDirection = (targetPoint - ChTrans).normalized;
            float distance = Vector3.Distance(ChTrans, targetPoint);

            if (distance > maxThrowDistance)
            {
                targetPoint = ChTrans + throwDirection * maxThrowDistance;
                distance = maxThrowDistance;
            }

            float throwForce = Mathf.Lerp(2f, maxThrowForce, distance / maxThrowDistance);

            DrawThrowPreview(throwDirection, throwForce);

            throwF = throwForce;
            dir = throwDirection;
        }
    }

    void DrawThrowPreview(Vector3 throwDirection, float throwForce)
    {
        lineRenderer.positionCount = 0;
        Vector3 ChTrans = character.transform.position;
        float newY = 1.5f;
        ChTrans.y = newY;
        Vector3 startPos = ChTrans;
        Vector3 velocity = throwDirection * throwForce;
        velocity.y = throwForce * 0.2f;

        int numSteps = 20;
        float timeStep = 0.05f;
        List<Vector3> positions = new List<Vector3>();

        Vector3 lastPosition = startPos;

        for (int i = 0; i < numSteps; i++)
        {
            float time = i * timeStep;
            Vector3 position = startPos + velocity * time;
            position.y += gravity * time * time / 2f;

            if (Physics.Raycast(lastPosition, position - lastPosition, out RaycastHit hit, (position - lastPosition).magnitude, LayerMask.GetMask("Ground")))
            {
                positions.Add(position);
                break;
            }

            positions.Add(position);
        }

        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    void ThrowCoin(Vector3 throwDirection, float throwForce)
    {
        SetHandActive(true);
        shoot = true;

        Vector3 transpo = character.transform.position;
        transpo.y = character.transform.position.y + 1;
        transform.position = transpo + throwDirection;

        Rigidbody rb = transform.GetComponent<Rigidbody>();
        filter.mesh = Base;

        if (rb != null)
        {
            Vector3 force = throwDirection * throwForce;
            force.y = throwForce * 0.2f;
            rb.AddForce(force, ForceMode.Impulse);
            character.GetInterActControll().ResetInteraction();
            ShootCoin();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6 && shoot)
        {
            Debug.Log("?");
            Rigidbody rigid = GetComponent<Rigidbody>();
            rigid.velocity = Vector3.zero;
            UseCoin();

            if (character != null)
            {
                character.MakeNoise(gameObject, Player.CoinNoise, 12);
            }
        }
    }

    public override void UseItem()
    {
        ThrowCoin(dir, throwF);
            InventoryManager.Instance.GetSlot().ClearItem();

    }

    public override void UpdateTime(float time)
    {
        base.UpdateTime(time);
        if (lineRenderer != null)
        {
            HasCoin();
        }
    }
    
    public override void inititem()
    {
        lineRenderer.positionCount = 0;
        HandAnything = character.HandCoin;
        SetHandActive(true);
    }
}
