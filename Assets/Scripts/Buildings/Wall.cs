using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public GameObject wallPrefab;
    
    private float rayLength = 1.5f;

    public void BuildWall()
    {
        CheckClosePoles(CastRay(transform.forward), new Vector3(1.05f, 1.4f, 0), new Vector3(-90, 90, 0));
        CheckClosePoles(CastRay(transform.right), new Vector3(0, 1.4f, 1.05f), new Vector3(-90, 0, 0));
        CheckClosePoles(CastRay(-transform.forward), new Vector3(1.05f, 1.4f, 0), new Vector3(-90, 90, 0));
        CheckClosePoles(CastRay(-transform.right), new Vector3(0, 1.4f, 1.05f), new Vector3(-90, 0, 0));
    }

    public void CheckClosePoles(RaycastHit hit, Vector3 wallPos, Vector3 wallRot)
    {
        if (hit.transform != null)
            if (hit.transform.tag == "WallPole" && hit.transform.gameObject != gameObject)
            {
                Instantiate(wallPrefab, Vector3.Lerp(transform.position, hit.transform.position, 0.5f) + wallPos, Quaternion.Euler(wallRot), transform);
            }
    }

    public void CheckCloseWallsToDestroy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.0f);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Wall"))
            {
                Destroy(collider.gameObject);
            }
        }
    }

    private RaycastHit CastRay(Vector3 direction)
    {
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, rayLength))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.blue);
            return hitInfo;
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);
            return hitInfo;
        }
    }

    private void OnDestroy()
    {
        if (GetComponent<Building>().Placed)
            CheckCloseWallsToDestroy();
    }
}