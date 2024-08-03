using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rotateSpeedMovement = 0.05f;
    [SerializeField] private Animator anim;
    [SerializeField] private float motionSmoothTime = 0.1f;
    [SerializeField] private NavMeshAgent agent;
    private float rotateVelocity;

    private void Update()
    {
        Animate(); 
        Move(); 
    }

    public void Move()
    {
        if (Input.GetMouseButton(1))
        {
            RaycastHit hit; 
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                if (hit.collider.GetComponent<Ground>())
                {
                    agent.SetDestination(hit.point);
                    agent.stoppingDistance = 0;

                    Quaternion rotationToLookAt = Quaternion.LookRotation(hit.point - transform.position);
                    float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeedMovement * (Time.deltaTime * 5));
                    transform.eulerAngles = new Vector3(0, rotationY, 0); 
                }
            }
        }
    }

    private void Animate()
    {
        float speed = agent.velocity.magnitude / agent.speed;
        anim.SetFloat("Speed", speed, motionSmoothTime, Time.deltaTime); 
    }
}
