using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using BehaviorTree;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;
using EPOOutline;
using UnityEngine.Audio;


public class Enemy : Character
{
    protected bool DetectPlayer;
    protected bool AttractPlayer;
    protected AIPath aIPath;
    [Header("�þ߹���, �Ÿ�")]
    [Range(0, 360)]
    public float RadiusAngle = 90f;  // ��ä�� ����
    public float Distance = 5f;   // ��ä�� ������

    public Node node;
    Node NewNode;
    float PlayerY;
    public Animator anim;
    Collider scollider;
    public Vector3 startPos;

    public Slider CheckPlayerSlider;
    [SerializeField] float DetectTimer = 2.5f;
    public Player player { get; private set; }
    float maxTimer;
    protected virtual void Start()
    {
        sfxAudioSource = gameObject.AddComponent<AudioSource>();
        sfxAudioSource.loop = false;
        sfxAudioSource.playOnAwake = false;
        if (CheckPlayerSlider != null)
        {
            CheckPlayerSlider.value = 1;
        }
        maxTimer = DetectTimer;
        anim = GetComponent<Animator>();
        if (GetComponent<AIPath>() != null)
        {
            aIPath = GetComponent<AIPath>();
            applyspeed = MoveSpeed;

        }
        startPos = transform.position;
        startRot = transform.rotation;
        if (node != null)
        {
            NewNode = node.Clone();

            NewNode.SetRunner(this);
        }
        ShowShape();
        player = GameObject.FindAnyObjectByType<Player>();
        if (player != null)
        {
            PlayerY = player.transform.position.y;
        }
        scollider = GetComponentInChildren<Collider>();
    }
    public void InitNode()
    {
        Node.InitTree(NewNode);
    }
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
        isSound = false;
    }
    protected virtual void Update()
    {
        if (UIImage)
            UIImage.transform.position = new Vector3(transform.position.x, UIImage.transform.position.y, transform.position.z);
        if (CheckPlayerSlider)
            CheckPlayerSlider.transform.position = new Vector3(transform.position.x, CheckPlayerSlider.transform.position.y, transform.position.z);
        if (GameManager.Instance.isGameOver) return;
        if (aIPath != null)
        {
            aIPath.maxSpeed = applyspeed;
        }
        if (NewNode != null)
        {
            NewNode.Evaluate();
        }

        if (CheckPlayerSlider != null)
        {
            if (DetectPlayer)
            {
                DetectTimer -= Time.deltaTime;
                CheckPlayerSlider.value = DetectTimer / maxTimer;
                if (DetectTimer <= 0)
                {
                    AttractPlayer = true;
                    CheckPlayerSlider.gameObject.SetActive(false);
                }
            }
            else
            {
                DetectTimer = maxTimer;
                CheckPlayerSlider.gameObject.SetActive(true);
                CheckPlayerSlider.value = DetectTimer / maxTimer;
            }

        }
        
        GetProb();

    }
    public void SliderActive(bool active)
    {
        if (CheckPlayerSlider != null)
        {
            CheckPlayerSlider.gameObject.SetActive(active);
        }
    }
    public virtual bool GetPlayer() => AttractPlayer;
    public virtual bool GetDetectPlayer() => DetectPlayer;

    public virtual void missPlayer()
    {
        DetectPlayer = false;
        AttractPlayer = false;
        ShowShape();
    }
    bool stun;
    public void HitEnemy()
    {
        StopMove();
        missPlayer();
        stun = true;
        Time.timeScale = 0;
        //ui창 띄우기 애니매이션 재생
        Time.timeScale = 1;
    }
    float releaseTimer;
    public void releaseStun(float time)
    {
        releaseTimer += Time.deltaTime;
        if (releaseTimer >= time)
        {
            Debug.Log(time + "초 후에 스턴 해제");
            Debug.Log(releaseTimer + "초 경과");
            stun = false;
        }
    }
    public bool GetStun()
    {
        return stun;
    }
    public virtual void StopMove()
    {
        if (aIPath != null)
        {
            UseAnim("Idle");
            aIPath.enabled = false;
        }
        if (chase)
        {
            chase = false;
        }
        //aIPath.isStopped = true;
    }
    string Idle = "Idle";
    protected string Move = "Move";
    string ChasePlayer = "ChasePlayer";
    string CheckNoise = "CheckNoise";
    string Stun = "Stun";
    string StandUp = "StandUp";
    public void UseAnim(string exclude)
    {
        if (anim == null) return;

        string[] triggers = { Idle, Move, ChasePlayer, CheckNoise, Stun, StandUp };
        foreach (string trigger in triggers)
        {
            if (trigger != exclude)
            {
                anim.SetBool(trigger, false);
            }
            else
            {
                anim.SetBool(trigger, true);
            }
        }
    }
    [Header("UI Image")]
    [SerializeField] private Image UIImage;
    string UIPAth = "UI/In_Game/";
    bool isSound = false;
    string saveUIName;
    public void AboveUI(string exclude = "", bool active = true)
    {

        UIImage.gameObject.SetActive(active);
        if (!active)
        {
            return;
        }
        string[] triggers = { "ChasePlayer", "Stun", "CheckNoise" };
        for (int i = 0; triggers.Count() > i; i++)
        {
            if (triggers[i] == exclude)
            {
                UIImage.sprite = Resources.Load<Sprite>(UIPAth + exclude);
                if (saveUIName != exclude && !isSound)
                {
                    saveUIName = exclude;
                    isSound = true;
                    SetAudio(exclude);
                }
            }
            if (triggers[i] != exclude)
            {
            }
        }

    }



    public virtual void ShowOutline() { }
    public virtual void HideOutline() { }
    public virtual void ShowShape()
    {
        if (GetComponentInChildren<TestOne>())
        {
            TestOne t1 = GetComponentInChildren<TestOne>();
            t1.ShowMesh();
        }
    }

    public virtual void HideShape()
    {

        if (GetComponentInChildren<TestOne>())
        {
            TestOne t1 = GetComponentInChildren<TestOne>();
            t1.InvMeshRen();
        }
    }
    bool chase;
    public virtual void StartChase(Player player)
    {
        if (stun) return;
        applyspeed = RunSpeed;
        UseAnim(ChasePlayer);
        aIPath.enabled = true;
        Vector3 newvec = player.transform.position;
        newvec.y = transform.position.y;
        aIPath.destination = newvec;
        aIPath.isStopped = false;
        if (GetComponentInChildren<TestOne>())
        {
            TestOne t1 = GetComponentInChildren<TestOne>();
            t1.InvMeshRen();
            chase = true;
        }
    }
    void GoHome(Vector3 newTarget)
    {
        aIPath.enabled = true;
        aIPath.destination = newTarget;

        aIPath.isStopped = false;

        //aIPath.SearchPath();
    }


    protected virtual void MoveToTarget(Vector3 newTarget, string anim, float speed)
    {
        if (stun) return;
        applyspeed = speed;
        UseAnim(anim);
        aIPath.enabled = true;
        newTarget.y = transform.position.y;
        aIPath.destination = newTarget;
        aIPath.isStopped = false;
    }

    protected Vector3 noise;

    public virtual void ProbArea(Vector3 pos)
    {
        noise = pos;
        noise.y = transform.position.y;
    }
    public virtual Vector3 GetNoiseVec()
    {
        return noise;
    }

    public virtual void InitNoise()
    {
        noise = Vector3.zero;
    }

    public virtual void InitProb(){}
    protected bool EndProb = false;

    public virtual void EndProbarea()
    {
        EndProb = true;
    }

    public virtual void UnEndProbArea()
    {
        EndProb = false;
    }
    public virtual bool isEndProb()
    {
        return EndProb;
    }

    protected bool probSuccess = false;
    public void MoveProb(Vector3 vec)
    {
        if (stun) return;

        MoveToTarget(vec, ChasePlayer, RunSpeed);
        Vector3 newVec = vec;
        newVec.y = transform.position.y;
        float distanceToTarget = Vector3.Distance(transform.position, newVec);
        if (distanceToTarget < 1.5f)
        {
            probSuccess = true;
        }
    }

    public virtual void ProbEnd() { }
    public virtual void StartProb() { }
    public virtual bool isProb() { return false; }
    public bool GetProb() { return probSuccess; }
    public virtual void Patrols() { }
    public virtual void StopPatrol() { }
    public virtual void RestartPatrol() { }
    public virtual bool GetPatrol() { return false; }
    public struct VisibilityResult
    {
        public List<Vector3> visiblePoints;
        public List<Vector3> blockedPoints;
    }
    public GameObject RayShoot;
    public Quaternion startRot;

    public VisibilityResult CheckVisibility(int rayCount)
    {
        VisibilityResult result = new VisibilityResult();
        result.visiblePoints = new List<Vector3>();
        result.blockedPoints = new List<Vector3>();
        if (stun) return result;

        Vector3 NewVector = transform.position;
        if (scollider != null)
        {

            NewVector.y = scollider.bounds.center.y;

            Transform enemyTransform = transform;
            Vector3 rayStartPos = transform.position;
            rayStartPos.y = NewVector.y;
            bool foundPlayer = false;
            // ��ä�� ������ Raycast
            for (int i = 0; i <= rayCount; i++)
            {
                if (AttractPlayer == true)
                    return result;

                float currentAngle = -RadiusAngle / 2 + RadiusAngle * (i / (float)rayCount);
                Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
                Vector3 rayDirection = rotation * enemyTransform.forward; // ����

                // 2D ��鿡�� y���� �����ϰ� rayDirection�� y���� 0���� ����
                rayDirection.y = 0f;
                // Raycast ����
                RaycastHit hit;
                if (Physics.Raycast(rayStartPos, rayDirection, out hit, Distance))
                {
                    // Player�� �����ϸ� visiblePoints�� �߰�
                    if (hit.collider.GetComponentInParent<Player>())
                    {
                        DetectPlayer = true;
                        foundPlayer = true;
                        // Debug.Log(hit.collider.GetComponent<Player>().GetInterActControll().GetHide());
                        if (hit.collider.GetComponentInParent<Player>().GetInterActControll().GetHide())
                        {
                            DetectPlayer = false;
                        }
                        Debug.Log(hit.collider.GetComponentInParent<Player>().GetInterActControll().GetHide());
                    }


                    result.visiblePoints.Add(hit.point);

                }
                else // Raycast�� �ƹ��Ϳ��� ���� ���� ��� (��ä�� ����)
                {
                    result.visiblePoints.Add(enemyTransform.position + rayDirection * Distance);
                }
                Debug.DrawRay(NewVector, rayDirection * hit.distance, Color.red, 0.1f);
            }
            if (!foundPlayer)
            {
                DetectPlayer = false;
            }
        }
        return result;
    }
    public VisibilityResult CheckVisibility(int rayCount, float newy)
    {
        VisibilityResult result = new VisibilityResult();
        result.visiblePoints = new List<Vector3>();
        result.blockedPoints = new List<Vector3>();
        if (stun) return result;

        Transform enemyTransform = transform;

        Vector3 rayStartPos = enemyTransform.position;
        rayStartPos.y = newy;
        // ��ä�� ������ Raycast
        for (int i = 0; i <= rayCount; i++)
        {
            float currentAngle = -RadiusAngle / 2 + RadiusAngle * (i / (float)rayCount);
            Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
            Vector3 rayDirection = rotation * enemyTransform.forward; // ����
            // 2D ��鿡�� y���� �����ϰ� rayDirection�� y���� 0���� ����
            rayDirection.y = 0;
            // Raycast ����
            RaycastHit hit;
            if (Physics.Raycast(rayStartPos, rayDirection, out hit, Distance))
            {
                // Player�� �����ϸ� visiblePoints�� �߰�
                if (hit.collider.GetComponentInParent<Player>())
                {
                    DetectPlayer = true;
                    if (hit.collider.GetComponentInParent<Player>().GetInterActControll().GetHide())
                    {
                        DetectPlayer = false;
                    }
                }
                else
                {
                    DetectPlayer = false;
                }

                result.visiblePoints.Add(hit.point);
                Debug.DrawRay(rayStartPos, rayDirection * hit.distance, Color.red, 0.1f);

            }
            else // Raycast�� �ƹ��Ϳ��� ���� ���� ��� (��ä�� ����)
            {
                Vector3 endPoint = rayStartPos + rayDirection * Distance;
                result.visiblePoints.Add(rayStartPos + rayDirection * Distance);
                Debug.DrawRay(rayStartPos, rayDirection * Distance, Color.green, 0.1f);

            }
        }
        return result;
    }


    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            StartCoroutine(TempBlockENemy(collision.transform.position));
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                if (stun)
                    return;
                player.Die();
                GameManager.Instance.GameOver();
            }
        }
    }

    public override void MakeNoise(GameObject obj, float radius, float stepsize)
    {
        throw new System.NotImplementedException();
    }
    IEnumerator TempBlockENemy(Vector3 enemyPos)
    {
        Bounds bounds = new Bounds(enemyPos, Vector3.one * 0.5f);
        GraphUpdateObject guo = new GraphUpdateObject(bounds)
        {
            modifyWalkability = true,
            setWalkability = false
        };
        AstarPath.active.UpdateGraphs(guo);
        yield return new WaitForSeconds(0.3f);
        GraphUpdateObject guorestore = new GraphUpdateObject(bounds)
        {
            modifyWalkability = true,
            setWalkability = true
        };
        AstarPath.active.UpdateGraphs(guorestore);
    }


}
