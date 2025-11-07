using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    CinemachineCamera Cam;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    Vector3 camPos;
    [SerializeField]
    private float Zdistance;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
       Vector3 pos =  player.transform.position;

        camPos.x = pos.x;
        camPos.y = pos.y;
        camPos.z = pos.z - Zdistance; //계속 연산이 이루어짐

        this.transform.position = new Vector3(camPos.x,camPos.y,camPos.z);
    }
}
