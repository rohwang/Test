using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{

    [Header("공격 설정")]
    public float attackRate = 2f;
    public int attackDamage = 20;
    private int attackType = 0;

    [Header("공격 방향")]
    public Transform attackPoint;
    public float attackRange = 0.3f;
    public LayerMask playerLayers;

    private EnemyMove enemyMove;
    public int maxHealth = 50;
    
    [SerializeField]
    private int currentHealth;

    [SerializeField]
    private Animator anim;

    private float _nextAttackTime = 2f;

    public PlayerAttack playeratk;

    private bool IsPlayerCloseEnough = false;

    private void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        enemyMove = GetComponent<EnemyMove>();
    }

    public void DealDamage() { // 체력 감소 공식

        // 1) 공격 판정: attackPoint 위치 기준 원형 영역 내의 플레이어 찾기
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);
        foreach (Collider2D playerCollider in hitPlayer)
        {
            // use tryGetComponent to avoid null reference exception
            if (playerCollider.TryGetComponent<PlayerAttack>(out PlayerAttack player) && player != null)
            {
                // 플레이어 피격 시
                playeratk.CurHp = playeratk.CurHp - attackDamage;
            }
        }
    }

    public void FirstAttack()
    {
        Debug.Log("1차 공격 발동");
        anim.SetTrigger(name = "Attack1");
        DealDamage();
    }

    public void SecondAttack()
    {
        Debug.Log("2차 공격 발동");
        anim.SetTrigger(name = "Attack2");
        DealDamage();
    }

    public void SingleAtk()
    {
        Debug.Log("싱글 공격 발동");
        FirstAttack();
    }

    public IEnumerator ComboCoroutine()
    {
        Debug.Log("콤보 공격 코루틴 발동");
        FirstAttack();
        yield return new WaitForSeconds(1f);
        SecondAttack();
    }

    public void ComboAtk()
    {
        StartCoroutine(ComboCoroutine());
    }

    private void Update()
    {

        if (currentHealth <= 0) //  몹 사망 시
        {
            Debug.Log("머쉬룸 사망");
            enemyMove.canMove = false;
        }

        CheckPlayerTransform();

        if (Time.time >= _nextAttackTime && IsPlayerCloseEnough)
        {
            Debug.Log("공격 조건 만족");

            enemyMove.canMove = false;

            attackType = Random.Range(0, 2);
            if (attackType == 0)
            {
                Debug.Log("어택 타입 0");
                SingleAtk();
            }
            else if (attackType == 1)
            {
                Debug.Log("어택 타입 1");
                ComboAtk();
            }
            _nextAttackTime = Time.time + 1f / attackRate;

            enemyMove.canMove = true;
        }
    }

    public void CheckPlayerTransform()
    {

        // 1) 공격 판정: attackPoint 위치 기준 원형 영역 내의 플레이어 찾기
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);
        foreach (Collider2D playerCollider in hitPlayer)
        {
            // use tryGetComponent to avoid null reference exception
            if (playerCollider.TryGetComponent<PlayerAttack>(out PlayerAttack player) && player != null)
            {
                IsPlayerCloseEnough = true;
            }
            else
            {
                IsPlayerCloseEnough = false;
            }
        }
    }
    public void TakeDamage(int dmg)
    {
        enemyMove.canMove = false;
        currentHealth -= dmg;
        anim.SetBool(name="Run", false);

        if (currentHealth <= 0)
        {
            DieAnimation();
        }
        else
        {
            anim.SetTrigger(name = "Hit");
            Invoke(name = "CanMoveTrue", 1f);
        }

    }

    public void Die()   //  사망 처리
    {
        Destroy(gameObject);
    }

    private void CanMoveTrue()
    {
        enemyMove.canMove = true;
    }

    private void DieAnimation()
    {
        // 사망 처리 (애니메이션, 오브젝트 제거 등)
        enemyMove.canMove = false;
        anim.SetTrigger(name = "Die");
        Invoke(name="Die", 1f);
    }

    // 씬 뷰에서 공격 판정 범위 시각화
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

