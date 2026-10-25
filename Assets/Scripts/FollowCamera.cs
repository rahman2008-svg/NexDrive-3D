using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; void LateUpdate(){if(!target)return; Vector3 desired=target.position-target.forward*7.5f+Vector3.up*4f; transform.position=Vector3.Lerp(transform.position,desired,Time.deltaTime*5f); transform.LookAt(target.position+Vector3.up*1.1f);}
}
