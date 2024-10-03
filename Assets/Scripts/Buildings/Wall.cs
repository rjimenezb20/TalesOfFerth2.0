using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public GameObject WallPrefab;

    private int layer;
    private readonly float rayLength = 2f;

    public void Awake()
    {
        layer = LayerMask.GetMask("Building");
    }

    public IEnumerator BuildWall()
    {
        yield return new WaitForSeconds(0.05f);

        CheckClosePoles(transform.forward);
        CheckClosePoles(-transform.forward);
        CheckClosePoles(transform.right);
        CheckClosePoles(-transform.right);
    }

    public void CheckClosePoles(Vector3 direction)
    {
        RaycastHit hit = CastRay(direction);
        if (hit.transform == null || hit.transform == transform) return;

        if (hit.transform.CompareTag("WallPole"))
        {
            Vector3 wallPos = new Vector3(0, 1.4f, 0);
            Vector3 wallRot = new Vector3(-90, transform.rotation.eulerAngles.y, direction == transform.right || direction == -transform.right ? 0 : 90);
            Instantiate(WallPrefab, Vector3.Lerp(transform.position, hit.transform.position, 0.5f) + wallPos, Quaternion.Euler(wallRot), transform);
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

        if (Physics.Raycast(ray, out var hitInfo, rayLength, layer))
        {
            return hitInfo;
        }
        return hitInfo;
    }

    private void OnDestroy()
    {
        if (GetComponent<Building>().Placed)
            CheckCloseWallsToDestroy();
    }
}