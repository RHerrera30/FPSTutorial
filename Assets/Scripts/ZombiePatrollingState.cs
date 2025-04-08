using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ZombiePatrollingState : StateMachineBehaviour
{
    private float timer;
    public float patrollingTime = 10f;

    private Transform player;
    private NavMeshAgent agent;

    public float detectionArea = 18f;
    public float patrolSpeed = 2f;
    
    List<Transform> waypointsList = new List<Transform>();
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Initializing
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        
        agent.speed = patrolSpeed;
        timer = 0;
        
        //Move to 1st waypoint
        GameObject wayPointCluster = GameObject.FindGameObjectWithTag("Waypoints");
        foreach (Transform t in wayPointCluster.transform)
        {
            waypointsList.Add(t);
        }
        
        Vector3 nextPosition = waypointsList[Random.Range(0, waypointsList.Count)].position;
        agent.SetDestination(nextPosition);
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (SoundManager.Instance.zombieChannel.isPlaying == false)
        {
            SoundManager.Instance.zombieChannel.clip = SoundManager.Instance.zombieWalking;
            SoundManager.Instance.zombieChannel.PlayDelayed(1f);
        }
        
        //Check if agent arrived at waypoint and move to next waypoint
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(waypointsList[Random.Range(0, waypointsList.Count)].position);
        }
        
        timer += Time.deltaTime;
        if (timer > patrollingTime)
        {
            animator.SetBool("isPatrolling", false);
        }
        
        //Transition to chase state
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionArea)
        {
            animator.SetBool("isChasing", true);
        }
    }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Stop the agent
        agent.SetDestination(agent.transform.position);
        
        SoundManager.Instance.zombieChannel.Stop();
    }
}
