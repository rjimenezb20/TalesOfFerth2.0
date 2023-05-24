using UnityEngine;
using UnityEngine.EventSystems;

public class ToogleHealthBar : MonoBehaviour
{
    private GameObject objectOnRay;
    private RaycastHit hitInfo;

    private void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity))
        {
            if (objectOnRay != null && objectOnRay != hitInfo.collider.gameObject)
            {
                objectOnRay.GetComponent<Health>().hpBar.SetActive(false);
            } 

            if (hitInfo.collider.GetComponent<Health>())
            {
                objectOnRay = hitInfo.collider.gameObject;
                hitInfo.collider.GetComponent<Health>().hpBar.SetActive(true);
            }
        } 
    }
}