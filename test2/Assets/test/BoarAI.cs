using UnityEngine;

public class BoarAI : MonoBehaviour
{
    public Animator animator;
    public Transform player;
    public float detectRange = 8f;
    public float attackRange = 1.5f;
    public float moveSpeed = 2f;
    public float respawnTime = 10f;
    public float HP = 5f;
    public GameObject attackTrigger, BoarHP;
    public float maxLiveDistance = 20f;
    public float respawnOffsetRadius = 10f;


    private bool isDead = false;
    private float maxHP;
    private Vector3 spawnPosition;
    private Rigidbody rb;
    private Boar_HP BoarHP_Instance;

    void Start()
    {
        spawnPosition = transform.position;
        rb = GetComponent<Rigidbody>();

        // 防止Y轴浮动和旋转异常
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        maxHP = HP;

        // 实例化血条
        GameObject bar = Instantiate(BoarHP);
        BoarHP_Instance = bar.GetComponent<Boar_HP>();
        BoarHP_Instance.target = transform;
    }

    void Update()
    {
        if (isDead) return;

        if (player.GetComponent<PlayerController>().isDead)
        {
            SetState(false, false, animator.GetBool("isDie"));
            rb.isKinematic = true;
            return;
        }
        else if (rb.isKinematic)
        {
            rb.isKinematic = false;
        }

        Vector3 boarPosXZ = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 playerPosXZ = new Vector3(player.position.x, 0, player.position.z);
        float distance = Vector3.Distance(boarPosXZ, playerPosXZ);

        if (distance <= attackRange)
        {
            SetState(false, true, false);
            FaceTarget();
        }
        else if (distance <= detectRange)
        {
            SetState(true, false, false);
            ChasePlayer();
        }
        else
        {
            SetState(false, false, false);
            StopMovement();
        }
    }

    void SetState(bool isMove, bool isAttack, bool isDie)
    {
        animator.SetBool("isRun", isMove);
        animator.SetBool("canAttack", isAttack);
        animator.SetBool("isDie", isDie);
    }

    void ChasePlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        direction.Normalize();

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion yOnlyRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            Quaternion modelOffset = Quaternion.Euler(-90f, 0f, 0f); // 模型朝向 X 轴补偿
            transform.rotation = Quaternion.Slerp(transform.rotation, yOnlyRotation * modelOffset, Time.deltaTime * 5f);
        }

        Vector3 nextPosition = rb.position + direction * moveSpeed * Time.deltaTime;
        rb.MovePosition(nextPosition);
    }

    void StopMovement()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void FaceTarget()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion yOnlyRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            Quaternion modelOffset = Quaternion.Euler(-90f, 0f, 0f);
            transform.rotation = yOnlyRotation * modelOffset;
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        SetState(false, false, true);
        gameObject.tag = "Untagged";
        StopMovement();
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;

        Invoke(nameof(Respawn), respawnTime);
    }

    void Respawn()
    {
        isDead = false;
        gameObject.tag = "Enemy";
        transform.position = spawnPosition;
        rb.isKinematic = false;
        GetComponent<Collider>().enabled = true;
        SetState(false, false, false);
        animator.Play("idle");
        HP = 5;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        Debug.Log("野猪受到伤害：" + amount);
        HP -= amount;

        if (HP <= 0)
            Die();
    }

    public void EnableAttackCollider()
    {
        if (attackTrigger != null)
            attackTrigger.SetActive(true);
    }

    public void DisableAttackCollider()
    {
        if (attackTrigger != null)
            attackTrigger.SetActive(false);
    }
}
