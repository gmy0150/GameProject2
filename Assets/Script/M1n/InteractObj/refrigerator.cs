using DG.Tweening;
using UnityEngine;

public class refrigerator : UseageInteract
{
    bool isopen;
    public GameObject GBox;
    public GameObject SpawnObject;
bool isSpawn = false;
    [SerializeField] float openy;
    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
        if (isopen) CloseDoor();
        else OpenDoor();
    }
    public override bool IsOneTimeInteraction()
    {
        return true;
    }
    void OpenDoor()
    {
        GBox.transform.DORotate(new Vector3(GBox.transform.eulerAngles.x, GBox.transform.eulerAngles.y,  -openy), 0.5f).onComplete = () =>
        {
            if (!isSpawn)
            {
                SpawnObject.SetActive(true);
                isSpawn = true;
            }
        };
        isopen = true;
    }
    void CloseDoor()
    {
        GBox.transform.DORotate(new Vector3(GBox.transform.eulerAngles.x, GBox.transform.eulerAngles.y, openy), 0.5f);
        isopen = false;
    }
    Quaternion SpawnDoor;
    void Start()
    {
        base.Start();
        if (GBox == null)
            GBox = gameObject;
    }
    public override void UpdateTime(float time)
    {
        throw new System.NotImplementedException();
    }
}
