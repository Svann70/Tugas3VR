using UnityEngine;

using UnityEngine.Animations.Rigging;

public class AutoIKSetup : MonoBehaviour
{
    public Transform rightHandTarget;
    public Transform leftHandTarget;

    public TwoBoneIKConstraint rightHandIK;
    public TwoBoneIKConstraint leftHandIK;

    void Start()
    {
        // Aktifkan IK
        rightHandIK.weight = 1f;
        leftHandIK.weight = 1f;

        // Set target otomatis
        rightHandIK.data.target = rightHandTarget;
        leftHandIK.data.target = leftHandTarget;
    }
}