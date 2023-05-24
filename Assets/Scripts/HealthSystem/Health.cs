using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float destroyTime;
    public GameObject hpBar;
    public SpriteRenderer hpBarSprite;

    private Camera mainCamera;
    private int MaxHP;
    public int HP;
    private Vector3 initialScale;

    void Start()
    {
        if (GetComponent<Enemy>()) 
        {
            MaxHP = GetComponent<Enemy>().data.healthPoints;
            HP = GetComponent<Enemy>().data.healthPoints;
        }
        else if (GetComponent<Unit>())
        {
            MaxHP = GetComponent<Unit>().data.healthPoints;
            HP = GetComponent<Unit>().data.healthPoints;
        }
        else if (GetComponent<Building>())
        {
            MaxHP = GetComponent<Building>().data.healthPoints;
            HP = GetComponent<Building>().data.healthPoints;
        }
        
        mainCamera = Camera.main;
        initialScale = hpBarSprite.transform.localScale;
    }
    private void Update()
    {
        if(hpBar.activeSelf)
            hpBar.transform.LookAt(hpBar.transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }

    public void ReceiveDamage(int damage)
    {
        HP -= damage;

        if (HP <= 0)
            Die();

        UpdateHealBarSprite();
    }

    public void ReceiveHeal(int heal)
    {
        HP += heal;

        if (HP > MaxHP)
            HP = MaxHP;

        UpdateHealBarSprite();
    }

    private void Die()
    {
        Destroy(this.gameObject, destroyTime);
    }

    private void UpdateHealBarSprite()
    {
        hpBarSprite.transform.localScale = new Vector3(initialScale.x * HP / MaxHP, hpBarSprite.transform.localScale.y, hpBarSprite.transform.localScale.z);
        hpBarSprite.transform.localPosition = new Vector3(-.85f + hpBarSprite.transform.localScale.x * 0.85f, hpBarSprite.transform.localPosition.y, hpBarSprite.transform.localPosition.z);
    }
}