using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum AIBehavior { Roaming, Standing, RoomRoaming }
public enum AIType { Human, Ghost }
public enum AIStatus { Alive, Dead, Exorcised }

public class AIScript : MonoBehaviour
{
    [SerializeField] private AIBehavior behavior;
    [SerializeField] private AIType enemyType;
    [SerializeField] private AIStatus aiStatus = AIStatus.Alive;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject ghostPrefab;

    [Header("Material Configs")]
    [SerializeField] private Renderer objRenderer; 
    [SerializeField] private Material[] matStack;

    [Header("Movement Configs")]
    [SerializeField] private float roamRadius = 10f;
    [SerializeField] private Vector3 roomCenter;
    [SerializeField] private float roomSize = 5f;

    [Header("Exorcism Configs")]
    [SerializeField] private float exorcismDuration = 2f;

    private bool isAlive = true; 

    private void Start()
    {
        int enemyTypeCount = System.Enum.GetValues(typeof(AIType)).Length;
        enemyType = (AIType)Random.Range(0, enemyTypeCount);

        int randomIndex = Random.Range(0, matStack.Length);
        objRenderer.material = matStack[randomIndex];

        StartCoroutine(BehaviorRoutine());
    }

    private void FixedUpdate()
    {
        float detectionDistance = 1f;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, detectionDistance))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                Vector3 newDirection = Vector3.Reflect(transform.forward, hit.normal);
                agent.SetDestination(transform.position + newDirection * roamRadius);
            }
        }
    }

    public void KillThisAI()
    {
        anim.SetBool("isDead", true);
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.SetDestination(transform.position);
        StopAllCoroutines();
        aiStatus = AIStatus.Dead; 
        DeathSequence();

        agent.enabled = false;
        if (TryGetComponent(out Rigidbody rb)) 
            rb.isKinematic = true;
        if (TryGetComponent(out Collider col)) 
            col.enabled = false;
    }

    public AIStatus GetAIStatus() => aiStatus; 

    private void DeathSequence()
    {
        if (enemyType == AIType.Human)
        {

        }
        if (enemyType == AIType.Ghost)
        {
            aiStatus = AIStatus.Exorcised;
            ExorciseGhost();
        }
    }

    private void ExorciseGhost()
    {
        if (isAlive)
        {
            isAlive = false;
            GameObject ghostInstance = Instantiate(ghostPrefab, transform.position + Vector3.up, ghostPrefab.transform.rotation);
            Destroy(gameObject, exorcismDuration);
        }
    }


    private IEnumerator BehaviorRoutine()
    {
        float stuckTime = 2f;
        Vector3 lastPosition = transform.position;
        float lastMovedTime = Time.time;

        while (true)
        {
            switch (behavior)
            {
                case AIBehavior.Roaming:
                    Roam();
                    break;
                case AIBehavior.Standing:
                    StandStill();
                    break;
                case AIBehavior.RoomRoaming:
                    RoomRoam();
                    break;
            }

            yield return new WaitForSeconds(Random.Range(5, 10));

            if (Vector3.Distance(transform.position, lastPosition) < 0.1f)
            {
                if (Time.time - lastMovedTime > stuckTime)
                {
                    Roam();
                }
            }
            else
            {
                lastMovedTime = Time.time;
                lastPosition = transform.position;
            }
        }
    }

    private void Roam()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, roamRadius, 1);
        Vector3 finalPosition = hit.position;

        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(finalPosition, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetDestination(finalPosition);
        }
    }

    private void StandStill()
    {
        agent.isStopped = true;
    }

    private void RoomRoam()
    {
        Vector3 randomPoint = roomCenter + Random.insideUnitSphere * roomSize;
        agent.SetDestination(randomPoint);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, roamRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, roomSize);
    }
}
