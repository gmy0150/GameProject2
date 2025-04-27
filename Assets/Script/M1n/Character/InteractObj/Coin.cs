using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : UseageInteract
{
    float maxThrowDistance;
    float maxThrowForce;
    LineRenderer lineRenderer;
    float gravity = -9.81f;
    GameObject CoinPrefab;
    Mesh Base;
    MeshFilter filter;

    public string itemName = "ToyHammer"; // ì•„ì´í…œ í™•ì¸ ì½”ë“œ - ë©”ì‹œì§€ JSONì—ì„œ ì°¾ì„ ì´ë¦„

    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
        filter = GetComponent<MeshFilter>();
        Base = filter.mesh;
        filter.mesh = null;

        GetCoin(); // ì•„ì´í…œ í™•ì¸ ì½”ë“œ - ì•„ì´í…œ íšë“ ì²˜ë¦¬

        maxThrowDistance = character.maxThrowDistance;
        maxThrowForce = character.maxThrowForce;
        lineRenderer = character.lineRenderer;
        CoinPrefab = character.Prefab;
    }
<<<<<<< Updated upstream
    private void Update()
=======

    public override bool CanInteract()
>>>>>>> Stashed changes
    {
        if (lineRenderer != null)
        {
            HasCoin();
        }
        
    }


    public override void InteractAgain()
    {
        if (!shoot)
        {
            filter.mesh = Base;
            //GameObject throwobj = Instantiate(CoinPrefab, transform.position, Quaternion.identity);

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

        // âœ… ì•„ì´í…œ í™•ì¸ ì½”ë“œ - JSON ë©”ì‹œì§€ ê°€ì ¸ì™€ì„œ ì¶œë ¥
        string msg = MessageManager.Instance.GetMessage(itemName);
        if (!string.IsNullOrEmpty(msg))
        {
            ItemAlertUI.Instance.ShowUIText(msg); // ë©”ì‹œì§€ UIì— ì¶œë ¥
        }
    }
<<<<<<< Updated upstream
    public void HasCoin()
    {
        if (hasCoin) // ÄÚÀÎÀ» ¾ò¾úÀ» ¶§¸¸ Æ÷¹°¼± Ç¥½Ã
=======

    Vector3 dir;
    float throwF;

    public void HasCoin()
    {
        if (hasCoin)
>>>>>>> Stashed changes
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Vector3 targetPoint;
            Vector3 ChTrans = character.transform.position;
            float newY = 0.5f;
            ChTrans.y = newY;

            // ¸¶¿ì½º À§Ä¡°¡ ¶¥°ú Ãæµ¹ÇÏ´Â ÁöÁ¡À» ¸ñÇ¥ ÁöÁ¡À¸·Î ¼³Á¤
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.origin + ray.direction * maxThrowDistance;
            }

<<<<<<< Updated upstream
            targetPoint.y = ChTrans.y; // ¼öÆò¸é¿¡¼­¸¸ ¸ñÇ¥ ¼³Á¤
=======
            targetPoint.y = ChTrans.y;
>>>>>>> Stashed changes

            // ´øÁö´Â ¹æÇâ °è»ê
            Vector3 throwDirection = (targetPoint - ChTrans).normalized;
            float distance = Vector3.Distance(ChTrans, targetPoint);

            if (distance > maxThrowDistance)
            {
                targetPoint = ChTrans + throwDirection * maxThrowDistance;
<<<<<<< Updated upstream
                distance = maxThrowDistance; // ÃÖ´ë °Å¸®·Î Á¦ÇÑ
=======
                distance = maxThrowDistance;
>>>>>>> Stashed changes
            }

            // ¸ñÇ¥ ÁöÁ¡±îÁöÀÇ °Å¸®·Î ´øÁö´Â Èû °è»ê
            float throwForce = Mathf.Lerp(2f, maxThrowForce, distance / maxThrowDistance);

            // Æ÷¹°¼± °æ·Î¸¦ ½Ç½Ã°£À¸·Î ±×¸®±â
            DrawThrowPreview(throwDirection, throwForce);

            // ÁÂÅ¬¸¯ÇÏ¸é ÄÚÀÎ ´øÁö±â
            if (Input.GetMouseButtonDown(0))
            {
                ThrowCoin(throwDirection, throwForce);
            }
        }
        else
        {
            lineRenderer.positionCount = 0; // ÄÚÀÎÀÌ ¾øÀ¸¸é Æ÷¹°¼± ¼û±è

        }
    }


    // Æ÷¹°¼± °æ·Î¸¦ ±×¸®´Â ÇÔ¼ö
    void DrawThrowPreview(Vector3 throwDirection, float throwForce)
    {
        lineRenderer.positionCount = 0;
        Vector3 ChTrans = character.transform.position;
        float newY = 1.5f;
        ChTrans.y = newY;
        Vector3 startPos = ChTrans;
        Vector3 velocity = throwDirection * throwForce;
<<<<<<< Updated upstream
        velocity.y = throwForce * 0.2f; //  ±âÁ¸ ¹æ½Ä°ú ÀÏ°üµÇµµ·Ï YÃà ÀÌµ¿·® Á¶Á¤
=======
        velocity.y = throwForce * 0.2f;
>>>>>>> Stashed changes

        int numSteps = 20;
        float timeStep = 0.05f;
        List<Vector3> positions = new List<Vector3>();

        Vector3 lastPosition = startPos;
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes

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
        shoot = true;
        Vector3 transpo = character.transform.position;
        transpo.y = character.transform.position.y + 1;
        transform.position = transpo + throwDirection;

        Rigidbody rb = transform.GetComponent<Rigidbody>();
        filter.mesh = Base;

        if (rb != null)
        {
            Vector3 force = throwDirection * throwForce;
<<<<<<< Updated upstream
            force.y = throwForce * 0.2f; //  YÃà ÀÌµ¿ Àû¿ë
=======
            force.y = throwForce * 0.2f;
>>>>>>> Stashed changes
            rb.AddForce(force, ForceMode.Impulse);
            character.GetInterActControll().ResetInteraction();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" && shoot)
        {
            Rigidbody rigid = GetComponent<Rigidbody>();
            rigid.velocity = Vector3.zero;
            UseCoin();

            if (character != null)
            {
                character.MakeNoise(gameObject, Player.CoinNoise, 12);
            }
        }
    }
<<<<<<< Updated upstream
=======

    public override void UseItem()
    {
        ThrowCoin(dir, throwF);
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
    }
>>>>>>> Stashed changes
}
