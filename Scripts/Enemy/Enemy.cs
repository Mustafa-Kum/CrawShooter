using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, IBehaviorTreeInterface, ITeamInterface, ISpawnInterface
{
    [Header("Components")]
    [Space]
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private Animator animator;
    [SerializeField] private PerceptionComponent perceptionComponent;
    [SerializeField] private BehaviorTree behaviorTree;
    [SerializeField] private MovementComponent movementComponent;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private NavMeshAgent navMeshAgent;

    [Header("Team")]
    [Space]
    [SerializeField] private int teamID = 2;

    [Header("Reward")]
    [Space]
    [SerializeField] private Reward killReward;

    private bool isDead = false;
    private Vector3 previousPosition;

    public Animator Animator { get { return animator; } private set { animator = value; } }

    public int GetTeamID()
    {
        return teamID;
    }

    private void Awake()
    {
        perceptionComponent.onPerceptionTargetChanged += TargetChanged;
    }

    protected virtual void Start()
    {
        if (healthComponent != null)
        {
            healthComponent.onDead += StartDeath;
            healthComponent.onTakeDamage += TakenDamage;
        }

        previousPosition = transform.position;
    }
    protected virtual void Update()
    {
        WalkingAnimation();
    }

    private void WalkingAnimation()
    {
        if (movementComponent == null)
            return;

        Vector3 posDelta = transform.position - previousPosition;
        float speed = posDelta.magnitude / Time.deltaTime;
        animator.SetFloat("Speed", speed);
        previousPosition = transform.position;
    }

    private void TargetChanged(GameObject target, bool sensed)
    {
        if (sensed)
        {
            behaviorTree.BlackBoard.SetOrAddData("Target", target);
        }
        else
        {
            behaviorTree.BlackBoard.SetOrAddData("LastSeenLocation", target.transform.position);
            behaviorTree.BlackBoard.RemoveBlackboardData("Target");
        }
    }

    private void TakenDamage(float health, float delta, float maxHealth, GameObject Instigator)
    {
    }

    private void StartDeath(GameObject killer)
    {
        TriggerDeathAnimation();
        IRewardListener[] rewardListeners = killer.GetComponents<IRewardListener>();

        foreach (IRewardListener listener in rewardListeners)
        {
            listener.Reward(killReward);
        }
    }

    private void TriggerDeathAnimation()
    {
        isDead = true;

        capsuleCollider.enabled = false;
        navMeshAgent.speed = 0;

        Dead();

        if (animator != null)
        {
            animator.SetBool("isDead", true);
            animator.SetTrigger("Dead");
        }
    }

    public void OnDeathAnimationFinished()
    {
        Destroy(gameObject);
    }


    private void OnDrawGizmos()
    {
        if (behaviorTree && behaviorTree.BlackBoard.GetBlackboardData("Target", out GameObject target))
        {
            Vector3 drawTargetPos = target.transform.position + Vector3.up;

            Gizmos.DrawWireSphere(drawTargetPos, 0.7f);
            Gizmos.DrawLine(transform.position + Vector3.up, drawTargetPos);
        }
    }

    public void RotateTowards(GameObject target, bool verticalAim = false)
    {
        if (isDead == false)
        {
            Vector3 aimDir = target.transform.position - transform.position;
            aimDir.y = verticalAim ? aimDir.y : 0;
            aimDir = aimDir.normalized;
            movementComponent.RotatingAim(aimDir);
        }
    }

    public virtual void AttackTarget(GameObject target)
    {

    }

    public void SpawnedBy(GameObject spawnerGameObject)
    {
        BehaviorTree spawnerBehaviorTree = spawnerGameObject.GetComponent<BehaviorTree>();

        if (spawnerBehaviorTree != null && spawnerBehaviorTree.BlackBoard.GetBlackboardData<GameObject>("Target", out GameObject spawnerTarget))
        {
            Perception targetPerception = spawnerTarget.GetComponent<Perception>();

            if (perceptionComponent && targetPerception)
            {
                perceptionComponent.AssignPercievedPerception(targetPerception);
            }
        }
    }

    protected virtual void Dead()
    {

    }
}
