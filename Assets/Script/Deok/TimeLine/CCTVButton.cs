using UnityEngine;
using UnityEngine.Playables;

public class CCTVButton : MonoBehaviour
{
    public PlayableDirector timelineDirector;
    private bool isPlayerNear = false;

    private void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            timelineDirector?.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}
