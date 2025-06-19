using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{

    private EnemyMove enemyMove;
    public int maxHealth = 50;
    
    [SerializeField]
    private int currentHealth;

    [SerializeField]
    private Animator anim;

    private void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        enemyMove = GetComponent<EnemyMove>();
    }


    public void TakeDamage(int dmg)
    {
        enemyMove.canMove = false;
        currentHealth -= dmg;
        anim.SetBool("Run", false);
        anim.SetTrigger("Hit");
        Invoke("CanMoveTrue", 1.5f);
        anim.SetBool("Run", true);


        if (currentHealth <= 0)
        {
            DieAnimation();
        }
    }

    public void Die()
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
        anim.SetTrigger("Die");
        Invoke("Die", 1f);
    }
}

