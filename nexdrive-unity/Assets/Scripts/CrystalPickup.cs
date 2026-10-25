using UnityEngine;

public class CrystalPickup : MonoBehaviour
{
    void Update(){transform.Rotate(0,100*Time.deltaTime,0); transform.position+=Vector3.up*Mathf.Sin(Time.time*2f)*Time.deltaTime*.15f;}
    void OnTriggerEnter(Collider other){if(other.GetComponent<CarController>()!=null && GameBootstrap.Instance!=null) GameBootstrap.Instance.Collect(gameObject);}
}
