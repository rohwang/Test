using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    public PlayerAttack playerReference;

    public Slider hpBar;
    public Slider expBar;


    public void UpdateHpUI()
    {
        float maxHp = playerReference.GetMaxHp(); 
        float curHp = playerReference.GetCurHp();

        hpBar.value = curHp / maxHp;
    }
}
