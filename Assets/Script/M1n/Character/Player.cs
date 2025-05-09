﻿﻿using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Player : Character
{
    IController controller;
    public GameObject Prefab;

    public LineRenderer lineRenderer;
    
    [Header("소리 거리")]
    public float RunSound , CoinSound;
    public static float RunNoise , CoinNoise;
    
    [Header("던지는 거리")]
    public float maxThrowDistance = 40;
    public float maxThrowForce = 40;
    
    InteractController interactController;
    public float interactionDistance = 5.0f;
    public bool ViewPoint;
    public bool InteractPoint;
    public Mesh BaseMesh;
    public Mesh BoxMesh;

    public LayerMask Layer;
    public LayerMask Enemy;
    IController KeyboardControll;
    void LateUpdate()
    {
        if (controller != null)
        {
            controller.LateTick(Time.deltaTime);
        }
    }
    void Start()
    {
        RunNoise = RunSound;
        CoinNoise = CoinSound;

        KeyboardControll = new KeyboardController();
        KeyboardControll.OnPosessed(this);
        this.controller = KeyboardControll;

        interactController = new InteractController();
        interactController.OnPosessed(this);
    }
    void OnAnimatorIK(int layerIndex)
    {
        
    }

    void Update()
    {
        if (interactController != null)
        {
            interactController.TIck(Time.deltaTime);
        }
        if (controller != null)
        {
            controller.Tick(Time.deltaTime);
        }
    }
    public Image Lights;
    public InteractController GetInterActControll()
    {
        return interactController;
    }



    public LayerMask Picture;


    public void ControllerDisable()
    {
        controller = null;
    }

    public void ControllerEnable()
    {
        controller = KeyboardControll;
    }

    public IController GetKey()
    {
        return KeyboardControll;
    }

    // public LayerMask detectionMask;
    // public LayerMask wallLayer;

    // [Header("시야")]
    // public float angleLimit = 60;
    // public float detectionRange = 20;
    // public float CircleRange = 8;
    public GameObject go;

    private void OnDrawGizmos()
    {
        // if (ViewPoint)
        // {
        //     Gizmos.color = Color.yellow;
        //     Gizmos.DrawWireSphere(transform.position, maxThrowDistance);

        //     Vector3 forward = transform.forward;
        //     Gizmos.color = Color.green;
        //     Gizmos.DrawLine(transform.position, transform.position + forward * detectionRange);

        //     Vector3 leftBound = Quaternion.Euler(0, -angleLimit, 0) * forward * detectionRange;
        //     Vector3 rightBound = Quaternion.Euler(0, angleLimit, 0) * forward * detectionRange;

        //     Gizmos.color = Color.blue;
        //     Gizmos.DrawLine(transform.position, transform.position + leftBound);
        //     Gizmos.DrawLine(transform.position, transform.position + rightBound);

        //     Gizmos.color = new Color(0, 1, 1, 0.1f);
        //     Gizmos.DrawLine(transform.position, transform.position + leftBound);
        //     Gizmos.DrawLine(transform.position, transform.position + rightBound);
        // }

        if (InteractPoint)
        {
            Gizmos.color = Color.red;

            Vector3 position = transform.position;
            Vector3 forward2 = transform.forward;
            float angle = 30f;

            Quaternion leftRotation = Quaternion.Euler(0, -angle, 0);
            Quaternion rightRotation = Quaternion.Euler(0, angle, 0);
            Vector3 leftDirection = leftRotation * forward2 * interactionDistance;
            Vector3 rightDirection = rightRotation * forward2 * interactionDistance;

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(position, interactionDistance);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(position, position + forward2 * interactionDistance);
            Gizmos.DrawLine(position, position + leftDirection);
            Gizmos.DrawLine(position, position + rightDirection);
        }
    }

    public void ListenSound(GuardAI enemy)
    {
        enemy.ShowOutline();
    }

    public override void MakeNoise(GameObject obj, float radius, float stepsize)
    {
        Vector3 origin = obj.transform.position;
        origin.y = 1f;

        for (float anglestep = 0; anglestep < 360f; anglestep += stepsize)
        {
            float currentAngle = anglestep * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(currentAngle), 0, Mathf.Sin(currentAngle));
            RaycastHit[] hits = Physics.RaycastAll(origin, direction, radius);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.GetComponent<Enemy>())
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    enemy.ProbArea(origin);
                }
            }
        }
    }
}
