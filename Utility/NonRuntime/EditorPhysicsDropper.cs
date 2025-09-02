using UnityEditor;
using UnityEngine;

public static class EditorPhysicsDropper
{
    private static Vector3 defaultGravity;

    [MenuItem("Tools/Drop With Physics %#d")] // Ctrl+Shift+D
    public static void DropWithPhysics()
    {
        foreach (var go in Selection.gameObjects)
        {
            if (go.GetComponent<EditorPhysicsDrop>() == null)
                go.AddComponent<EditorPhysicsDrop>();
        }

        // Save current gravity so we can restore later
        defaultGravity = Physics.gravity;

        // Use gentler gravity only while simulating in editor

        Physics.simulationMode = SimulationMode.Script;
        EditorApplication.update += Simulate;
    }

    [MenuItem("Tools/Freeze Dropped Objects %#f")] // Ctrl+Shift+F
    public static void FreezeDroppedObjects()
    {
        foreach (var drop in Object.FindObjectsByType<EditorPhysicsDrop>(FindObjectsSortMode.None))
        {
            drop.Freeze();
        }

        EditorApplication.update -= Simulate;


        Physics.simulationMode = SimulationMode.FixedUpdate;
    }

    private static void Simulate()
    {
        if (!Application.isPlaying)
        {
            Physics.Simulate(Time.fixedDeltaTime);
            SceneView.RepaintAll();
        }
    }
}
