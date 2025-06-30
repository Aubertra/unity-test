using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public string enemyTag = "Enemy";
    public GameObject attackTrigger;
    public float HP = 10;

    private Transform currentTarget;
    [HideInInspector]
    public bool isDead = false;

    void Start()
    {
        if (RockerController.Instance == null)
            Debug.LogError("摇杆控制器未初始化！");
    }


    void Update()
    {
        if (isDead) return;

        HandleMovement();
        HandleTargeting();
        HandleAttackInput();
    }

    void HandleMovement()
    {
        // 默认使用键盘输入
        Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // 如果连接了摇杆控制器，并有方向输入，则覆盖键盘输入
        if (RockerController.Instance != null && RockerController.Instance.outPos != Vector2.zero)
        {
            Debug.Log("轮盘输入");
            Vector2 rockerDir = RockerController.Instance.outPos.normalized;
            inputDir = new Vector3(rockerDir.x, 0, rockerDir.y);
        }

        if (RockerController.Instance != null)
            Debug.Log("摇杆方向：" + RockerController.Instance.outPos);


        bool isMoving = inputDir.magnitude > 0.1f;

        animator.SetBool("isRun", isMoving);

        if (isMoving)
        {
            Quaternion lookRotation = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            //transform.Translate(inputDir.normalized * moveSpeed * Time.deltaTime, Space.World);
            Vector3 move = new Vector3(inputDir.x, 0, inputDir.z).normalized * moveSpeed * Time.deltaTime;
            transform.position += move;
        }
    }

    void HandleTargeting()
    {
        currentTarget = FindClosestEnemy();

        if (currentTarget != null)
        {
            float distance = Vector3.Distance(transform.position, currentTarget.position);
            bool inRange = distance <= attackRange;
            animator.SetBool("canAttack", inRange);
        }
        else
        {
            animator.SetBool("canAttack", false);
        }
    }

    void HandleAttackInput()
    {
        if (currentTarget != null)
        {
            float distance = Vector3.Distance(transform.position, currentTarget.position);
            if (distance <= attackRange)
            {
                // 朝向敌人
                Vector3 dirToEnemy = (currentTarget.position - transform.position).normalized;
                dirToEnemy.y = 0;
                if (dirToEnemy != Vector3.zero)
                    transform.forward = dirToEnemy;

                // 播放攻击动画
                animator.SetBool("canAttack", true);
            }
        }
    }

    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = enemy.transform;
            }
        }

        return closest;
    }

    public void Die()
    {
        isDead = true;
        animator.SetBool("isDie", true);
        animator.SetBool("isRun", false);
        animator.SetBool("canAttack", false);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        Debug.Log("玩家受到伤害：" + amount);

        HP -= amount;

        if (HP <= 0)
            Die();
    }

    public void EnableAttackCollider()
    {
        attackTrigger.SetActive(true);
    }

    public void DisableAttackCollider()
    {
        attackTrigger.SetActive(false);
    }

}
