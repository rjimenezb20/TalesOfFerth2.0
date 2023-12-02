using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float destroyTime;
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
        else if (gameObject.tag == "Building")
        {
            Building building = gameObject.GetComponent<Building>();
            MaxHP = building.data.healthPoints1;
            HP = building.data.healthPoints1;
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
        GetComponent<Animator>().Play("Death");
    }

    private void UpdateHealBarSprite()
    {
        hpBarSprite.transform.localScale = new Vector3(initialScale.x * HP / MaxHP, hpBarSprite.transform.localScale.y, hpBarSprite.transform.localScale.z);
        hpBarSprite.transform.localPosition = new Vector3(-.85f + hpBarSprite.transform.localScale.x * 0.85f, hpBarSprite.transform.localPosition.y, hpBarSprite.transform.localPosition.z);
    }

    public void Destroy()
    {
        Destroy(this.gameObject, destroyTime);
    }

    IEnumerator a ()
    {
        yield return new WaitForSeconds(2);
        ReceiveDamage(100);
        StartCoroutine(a());
    }
}