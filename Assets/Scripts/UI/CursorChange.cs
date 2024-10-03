using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorChange : MonoBehaviour
{
    public Texture2D normalCursor;
    public Texture2D attackCursor;
    public GameObject clickFX;

    private SelectionController SC;
    private UnitsController UC;

    private void Awake()
    {
        ChangeToNormalCursor();
        SC = FindObjectOfType<SelectionController>();
        UC = FindObjectOfType<UnitsController>();
    }

    private void Start()
    {
        StartCoroutine(CursorChanger());
    }

    public void ChangeToNormalCursor()
    {
        Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
    }

    public void ChangeToAttackCursor()
    {
        Cursor.SetCursor(attackCursor, Vector2.zero, CursorMode.Auto);
    }

    IEnumerator CursorChanger()
    {
        while(true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                if (raycastHit.transform.tag == "Enemy" && SC.GetSelectedUnits().Count > 0)
                {
                    ChangeToAttackCursor();
                } 
                else if (UC != null && !UC.agresiveMove)
                {
                    ChangeToNormalCursor();
                }
            }
            yield return null;
        }
    }

    public void InstantiateClickFX(Vector3 pos ,Color color)
    {
        clickFX.GetComponent<SpriteRenderer>().color = color;
        Instantiate(clickFX, pos, Quaternion.Euler(90, 0, 0));
    } 
}