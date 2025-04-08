using UnityEngine;
using UnityEngine.AI;
public class ZombieChaseState : StateMachineBehaviour
{

    private Transform player;
    private NavMeshAgent agent;

    public float chaseSpeed = 6f;

    public float stopChasingDistance = 21;
    public float attackingDistance = 2.5f;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Initialization
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        
        agent.speed = chaseSpeed;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (SoundManager.Instance.zombieChannel.isPlaying == false)
        {
            SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieChase);
        }
        
        agent.SetDestination(player.position);
        animator.transform.LookAt(player);
        
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        
        //Check if agent should stop chasing
        if (distanceFromPlayer > stopChasingDistance)
        {
            animator.SetBool("isChasing", false);
        }
        
        //Check if agent should attack
        if (distanceFromPlayer < attackingDistance)
        {
            animator.SetBool("isAttacking", true);
        }
    }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
        
        SoundManager.Instance.zombieChannel.Stop();
    }
}
