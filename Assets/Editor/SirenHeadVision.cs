using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (sirenHeadAi))]

public class SirenHeadVision : Editor
{
    private void OnSceneGUI()
    {
        sirenHeadAi fow = (sirenHeadAi)target;
        Handles.color = Color.magenta;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.chaseRange);
        Vector3 viewAngleA = fow.DirectionFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirectionFromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.chaseRange);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.chaseRange);

        Handles.color = Color.cyan;
        foreach(Transform visibleTarget in fow.visibleTargets)
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.position);
        }
    }
}
