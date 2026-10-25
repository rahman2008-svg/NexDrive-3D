using UnityEngine;

public class CarController : MonoBehaviour
{
    public float acceleration=22f, maxSpeed=32f, steering=75f, brakePower=30f;
    private Rigidbody rb;
    public float SpeedKmh => rb == null ? 0 : rb.velocity.magnitude*3.6f;
    void Awake(){rb=GetComponent<Rigidbody>();}
    void FixedUpdate()
    {
        float vertical=Input.GetAxisRaw("Vertical"); float horizontal=Input.GetAxisRaw("Horizontal");
        if(TouchHold.Held.Contains("accelerate")) vertical=1; if(TouchHold.Held.Contains("brake")) vertical=-1; if(TouchHold.Held.Contains("left")) horizontal=-1; if(TouchHold.Held.Contains("right")) horizontal=1;
        if(vertical>0 && rb.velocity.magnitude<maxSpeed) rb.AddForce(transform.forward*vertical*acceleration,ForceMode.Acceleration);
        if(vertical<0) rb.AddForce(transform.forward*vertical*(Input.GetKey(KeyCode.Space)?brakePower:acceleration*.7f),ForceMode.Acceleration);
        if(Input.GetKey(KeyCode.Space) || TouchHold.Held.Contains("brake")) rb.velocity=Vector3.MoveTowards(rb.velocity,Vector3.zero,brakePower*Time.fixedDeltaTime);
        float speedFactor=Mathf.Clamp01(rb.velocity.magnitude/3f); if(rb.velocity.sqrMagnitude>.2f) transform.Rotate(0,horizontal*steering*speedFactor*Time.fixedDeltaTime,0);
        if(transform.position.y< -5) {transform.position=new Vector3(0,1,0);rb.velocity=Vector3.zero;}
    }
}
