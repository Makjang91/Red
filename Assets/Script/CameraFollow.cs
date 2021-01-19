using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float timeOffset;
    public Vector3 posOffset;

    public Vector3 velocity;
    
    void Start()
    {
        player = GameObject.Find("Red");
    }

    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + posOffset, ref velocity, timeOffset);
    }

    public void StartRewind()
    {
        GetComponent<Animator>().SetBool("rewind", true);
    }
    public void StopRewind()
    {
        GetComponent<Animator>().SetBool("rewind", false);
    }
}
