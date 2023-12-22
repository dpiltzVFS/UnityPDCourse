using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpeedCalculator : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed;
    public float UpdateDelay;
    public float speedScaler = 1;
    public Animator animator;

    private void OnEnable()
    {
        StartCoroutine(SpeedReckoner());
    }
    private IEnumerator SpeedReckoner()
    {

        YieldInstruction timedWait = new WaitForSeconds(UpdateDelay);
        Vector3 lastPosition = transform.position;
        float lastTimestamp = Time.time;

        while (enabled)
        {
            yield return timedWait;

            var deltaPosition = (transform.position - lastPosition).magnitude;
            var deltaTime = Time.time - lastTimestamp;

            if (Mathf.Approximately(deltaPosition, 0f)) // Clean up "near-zero" displacement
                deltaPosition = 0f;

            Speed = deltaPosition / deltaTime;
            Speed = Mathf.Clamp(Speed, 1f, 2f);
            animator.speed = Speed;
            animator.SetFloat("speed_f", Speed);
            
       


            lastPosition = transform.position;
            lastTimestamp = Time.time;
        }
    }
}



