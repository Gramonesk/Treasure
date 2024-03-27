using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(PlayerMovement))]
public class PlayerMovementCI : Editor
{
    public override void OnInspectorGUI()
    {
        PlayerMovement playerMovement = target as PlayerMovement;
        base.OnInspectorGUI();
        if (GUILayout.Button("Dash"))
        {
            playerMovement.ApplyForce(playerMovement.dashMagnitude.value);
        }
    }
}
