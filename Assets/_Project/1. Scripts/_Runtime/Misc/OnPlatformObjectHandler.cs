using System.Collections.Generic;
using UnityEngine;

public class OnPlatformObjectHandler : MonoBehaviour {
    Transform platformTransform;
    Vector3 recordedPosition;
    Vector3 platformPositionChangeValue;
    List<Collider2D> onPlatformsObjectCollider;

    private void Awake() {
        platformTransform = transform.parent;
        recordedPosition = platformTransform.position;
        onPlatformsObjectCollider = new List<Collider2D>();
    }

    private void FixedUpdate() {
        platformPositionChangeValue = platformTransform.position - recordedPosition;
        recordedPosition = platformTransform.position;
        for (int i = 0; i < onPlatformsObjectCollider.Count; i++) {
            onPlatformsObjectCollider[i].transform.position += new Vector3(
                platformPositionChangeValue.x,
                Mathf.Max(0f, platformPositionChangeValue.y),
                0f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (!onPlatformsObjectCollider.Contains(collision)) {
            onPlatformsObjectCollider.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        onPlatformsObjectCollider.Remove(collision);
    }
}

