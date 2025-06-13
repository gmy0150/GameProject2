﻿using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Player : Character
{
    public CursorUI cursorUI = new CursorUI();
    IController controller;

    public LineRenderer lineRenderer;

    [Header("소리 거리")]
    public float RunSound, CoinSound;
    public static float RunNoise, CoinNoise;

    [Header("던지는 거리")]
    public float maxThrowDistance = 40;
    public float maxThrowForce = 40;

    InteractController interactController;
    public float interactionDistance = 5.0f;
    public bool ViewPoint;
    public bool InteractPoint;
    public Mesh BoxMesh;
    public Material BoxMaterial;
    public Animator animator;
    public LayerMask Layer;
    public LayerMask Enemy;
    IController KeyboardControll;
    public GameObject Hammer;
    public GameObject Camera;
    public GameObject HandCoin;
    AudioSource sfxAudioSource;
    public AudioClip[] AudioClips;
    public void SetAudio(string name)
    {
        AudioMixerGroup[] sfxGroups = VolumeManager.Instance.audioMixer.FindMatchingGroups("SFXVolume");
        if (sfxGroups.Length > 0) sfxAudioSource.outputAudioMixerGroup = sfxGroups[0];
        if (sfxAudioSource.isPlaying && sfxAudioSource.clip.name == name)
        {
            return;
        }
        AudioClip clip = System.Array.Find(AudioClips, x => x.name == name);
        if (clip != null)
        {
            sfxAudioSource.clip = clip;
            sfxAudioSource.Play();
            StartCoroutine(ClearClipAfterPlay(clip.length));
        }
        else
        {
            Debug.LogWarning("AudioClip not found: " + name);
        }
    }
        private IEnumerator ClearClipAfterPlay(float duration)
    {
        yield return new WaitForSeconds(duration);
        sfxAudioSource.clip = null;
    }
    void LateUpdate()
    {
        if (controller != null)
        {
            controller.LateTick(Time.deltaTime);
        }
    }
    void Awake()
    {
        RunNoise = RunSound;
        CoinNoise = CoinSound;

        KeyboardControll = new KeyboardController();
        KeyboardControll.OnPosessed(this);
        this.controller = KeyboardControll;

        interactController = new InteractController();
        interactController.OnPosessed(this);
        animator = GetComponent<Animator>();
        GameManager.Instance.SavePos();

    }
    void Start()
    {
        sfxAudioSource = gameObject.AddComponent<AudioSource>();
        sfxAudioSource.loop = false;
        sfxAudioSource.playOnAwake = false;
        cursorUI.Start();

    }
    public void Move(Vector3 vector3, bool maingame)
    {
        KeyboardControll?.MovePlayer(vector3, maingame);
    }
    void OnDisable()
    {
        cursorUI?.OnDisable();
    }
    void Update()
    {
        
        interactController?.TIck(Time.deltaTime);
        controller?.Tick(Time.deltaTime);
        cursorUI.update();
    }
    void FixedUpdate()
    {
        controller?.FixedTick(Time.deltaTime);
    }
    public Image Lights;
    public Image PicChildImage;
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
    public void Die()
    {
        animator.SetBool("Die", true);
        controller = null;

    }

    public void restart()
    {
        controller = KeyboardControll;
        controller.RunningCancel();
        animator.SetBool("Die", false);
    }
    public void Hide(bool x)
    {
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = x;
        GetComponentInChildren<Collider>().enabled = x;
        GetComponent<Rigidbody>().useGravity = x;
    }
    public void AilionMeet()
    {
        AnimatorOverrideController overridecontrol = new AnimatorOverrideController(animator.runtimeAnimatorController);
        overridecontrol["Running"] = newRun;
        animator.runtimeAnimatorController = overridecontrol;
        Debug.Log("Ailion Meet");
    }
    public AnimationClip newRun;
}
