using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogCoverable : MonoBehaviour {

    Renderer renderer;

    void Start() {

        renderer = GetComponent<Renderer>();
        renderer.enabled = false;
        //FieldOfView.OnTargetsVisibilityChange += FieldOfViewOnTargetsVisibilityChange;
    }

    /*void OnDestroy() {

        FieldOfView.OnTargetsVisibilityChange -= FieldOfViewOnTargetsVisibilityChange;
    }

    void FieldOfViewOnTargetsVisibilityChange(List<Transform> newTargets) {

        renderer.enabled = newTargets.Contains(transform);
    }*/

    private void OnTriggerStay(Collider other) {

        if (other.gameObject.tag == "Vision") {

            renderer.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other) {

        if (other.gameObject.tag == "Vision") {

            renderer.enabled = false;
        }
    }

}