using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject hpBar;
    public SpriteRenderer hpBarSprite;
    [HideInInspector] public int HP;
    [HideInInspector] public int MaxHP;
    private Camera mainCamera;
    private Vector3 initialScale;

    void Start()
    {
        //StartCoroutine(a());

        if (gameObject.tag == "Enemy") 
        {
            Enemy enemy = gameObject.GetComponent<Enemy>();
            MaxHP = enemy.data.healthPoints;
            HP = enemy.data.healthPoints;
        }
        else if (gameObject.tag == "Unit")
        {
            Unit unit = gameObject.GetComponent<Unit>();
            MaxHP = unit.data.healthPoints;
            HP = unit.data.healthPoints;
        }
        else if (gameObject.tag == "Building" || gameObject.tag == "TownHall" || gameObject.tag == "WallPole")
        {
            Building building = gameObject.GetComponent<Building>();
            MaxHP = building.dataLvl1.healthPoints;
            HP = building.dataLvl1.healthPoints;
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
        if (gameObject.tag == "Enemy")
        {
            Enemy enemy = gameObject.GetComponent<Enemy>();
            enemy.Death();
        }
        else if (gameObject.tag == "Unit")
        {
            Unit unit = gameObject.GetComponent<Unit>();
            unit.Death();
        }
        else if (gameObject.tag == "Building" || gameObject.tag == "TownHall" || gameObject.tag == "WallPole")
        {
            Building building = gameObject.GetComponent<Building>();
            building.Destroy();
        }
    }

    private void UpdateHealBarSprite()
    {
        if (hpBarSprite != null)
        {
            hpBarSprite.transform.localScale = new Vector3(initialScale.x * HP / MaxHP, hpBarSprite.transform.localScale.y, hpBarSprite.transform.localScale.z);
            hpBarSprite.transform.localPosition = new Vector3(-.85f + hpBarSprite.transform.localScale.x * 0.85f, hpBarSprite.transform.localPosition.y, hpBarSprite.transform.localPosition.z);
        }
    }

    IEnumerator a ()
    {
        yield return new WaitForSeconds(2);
        ReceiveDamage(200);
        StartCoroutine(a());
    }
}