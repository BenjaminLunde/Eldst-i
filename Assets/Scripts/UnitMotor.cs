using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitMotor : MonoBehaviour
{
    Transform target;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(target != null)
        {
            agent.SetDestination(target.position);
            FaceTarget();
        }
        
    }

    public void MoveToPoint(Vector3 point)
    {
        agent.SetDestination(point);
    }

    public void FollowTarget(Interacteble newTarget)
    {
        agent.stoppingDistance = newTarget.radius * 0.8f;

        target = newTarget.interactionTransform;

    }

    public void StopFollowingTarget()
    {
        agent.stoppingDistance = 0f;
        target = null;
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRoation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRoation, Time.deltaTime * 5f);
    }
}
