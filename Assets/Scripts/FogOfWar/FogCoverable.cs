using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogCoverable : MonoBehaviour
{

    Renderer[] renderers;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }

        FieldOfView.OnTargetsVisibilityChange += FieldOfViewOnTargetsVisibilityChange;
    }

    void OnDestroy()
    {

        FieldOfView.OnTargetsVisibilityChange -= FieldOfViewOnTargetsVisibilityChange;
    }

    void FieldOfViewOnTargetsVisibilityChange(List<Transform> newTargets)
    {
        foreach (Renderer renderer in renderers)
        {
            if (!renderer.enabled)
                renderer.enabled = newTargets.Contains(transform);
        }
    }
}

/*private void OnTriggerStay(Collider other) {

        if (other.gameObject.tag == "Vision") {

            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {

        if (other.gameObject.tag == "Vision") {

            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = false;
            }
        }
    }*/