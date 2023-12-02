using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LookAt : MonoBehaviour
{
   
    private void OnEnable()
    {
        Vector3 directionToTarget = Camera.main.transform.position - transform.position;
        directionToTarget.y = 0; 

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
    }
}
