using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMove))]
public class PlayerAttack : MonoBehaviour
{
    [Header("공격 설정")]
    public KeyCode attackKey = KeyCode.J;   //  공격 키
    public float attackRate = 2f;
    public int attackDamage = 20;

    [Header("공격 방향")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    private float _nextAttackTime = 0f;

    [SerializeField]
    private Animator anim;


    [Header("체력 설정")]
    public float MaxHp = 100;
    public float CurHp = 100;

    public UnityEvent OnHpChanged;

    public float GetMaxHp()
    {
        return MaxHp;
    }

    public float GetCurHp()
    {
        return CurHp;
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (CurHp <= 0)
        {
            Destroy(gameObject);
        }

        // 공격 쿨다운 체크 및 입력 처리
        if (Time.time >= _nextAttackTime && Input.GetKeyDown(attackKey))
        {
            Attack();
            _nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void Attack()
    {
        // 1) 애니메이션 트리거
        anim.SetTrigger("Attack");

        // 2) 공격 판정: attackPoint 위치 기준 원형 영역 내의 적 찾기
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            // 적 스크립트에서 TakeDamage 메서드를 구현해야 합니다.
            // use tryGetComponent to avoid null reference exception
            if (enemyCollider.TryGetComponent<Enemy>(out Enemy enemy) && enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
        }


        // 가정
        OnHpChanged.Invoke();
    }

    // 씬 뷰에서 공격 판정 범위 시각화
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
