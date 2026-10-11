using UnityEngine;

/// <summary>
/// Physics Demonstration Helper for ParkPhysicsEnvironment
/// Provides an instant drop reset with the 'R' key.
/// </summary>
public class PhysicsBouncingDemo : MonoBehaviour
{
    [Header("UI Display")]
    [Tooltip("Check this box in the Inspector if you want to show the on-screen telemetry box.")]
    public bool showTelemetryUI = false; // Set to false by default so the screen stays clean!

    [Header("Observed Objects")]
    public Rigidbody bouncingSphereRb;
    public Rigidbody heavySphereRb;
    public Rigidbody woodenCrateRb;

    private Vector3 sphereInitialPos;
    private Vector3 heavyInitialPos;
    private Vector3 crateInitialPos;
    private Quaternion crateInitialRot;

    private void Start()
    {
        // Auto-detect references if not manually hooked up
        if (bouncingSphereRb == null)
        {
            GameObject ball = GameObject.Find("Bouncing_RubberSphere");
            if (ball != null) bouncingSphereRb = ball.GetComponent<Rigidbody>();
        }

        if (heavySphereRb == null)
        {
            GameObject heavy = GameObject.Find("Heavy_SteelSphere");
            if (heavy != null) heavySphereRb = heavy.GetComponent<Rigidbody>();
        }

        if (woodenCrateRb == null)
        {
            GameObject crate = GameObject.Find("Physics_WoodenCrate");
            if (crate != null) woodenCrateRb = crate.GetComponent<Rigidbody>();
        }

        if (bouncingSphereRb != null) sphereInitialPos = bouncingSphereRb.transform.position;
        if (heavySphereRb != null) heavyInitialPos = heavySphereRb.transform.position;
        if (woodenCrateRb != null)
        {
            crateInitialPos = woodenCrateRb.transform.position;
            crateInitialRot = woodenCrateRb.transform.rotation;
        }
    }

    private void Update()
    {
        // Press 'R' to reset the physics drop
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetSimulation();
        }

        // Optional: Press 'H' to toggle the HUD details on/off
        if (Input.GetKeyDown(KeyCode.H))
        {
            showTelemetryUI = !showTelemetryUI;
        }
    }

    public void ResetSimulation()
    {
        if (bouncingSphereRb != null)
        {
            bouncingSphereRb.linearVelocity = Vector3.zero;
            bouncingSphereRb.angularVelocity = Vector3.zero;
            bouncingSphereRb.transform.position = sphereInitialPos;
        }

        if (heavySphereRb != null)
        {
            heavySphereRb.linearVelocity = Vector3.zero;
            heavySphereRb.angularVelocity = Vector3.zero;
            heavySphereRb.transform.position = heavyInitialPos;
        }

        if (woodenCrateRb != null)
        {
            woodenCrateRb.linearVelocity = Vector3.zero;
            woodenCrateRb.angularVelocity = Vector3.zero;
            woodenCrateRb.transform.position = crateInitialPos;
            woodenCrateRb.transform.rotation = crateInitialRot;
        }
    }

    private void OnGUI()
    {
        if (!showTelemetryUI) return;

        GUI.Box(new Rect(15, 15, 340, 230), "<b>Park Physics Demonstration (Unity 6)</b>");

        GUILayout.BeginArea(new Rect(25, 45, 320, 195));

        GUILayout.Label("<b>--- Bouncing Sphere Telemetry ---</b>");
        if (bouncingSphereRb != null)
        {
            float height = bouncingSphereRb.transform.position.y;
            float speed = bouncingSphereRb.linearVelocity.magnitude;
            GUILayout.Label($"Height (Y): {height:F2} m");
            GUILayout.Label($"Velocity: {speed:F2} m/s");
            GUILayout.Label("Mass: 1.0 kg | Gravity: ON | Collision: Continuous");
            GUILayout.Label("Physics Material: Bounciness = 0.92 | Combine: Maximum");
        }

        GUILayout.Space(6);
        GUILayout.Label("<b>--- Comparison Objects ---</b>");
        if (heavySphereRb != null)
        {
            GUILayout.Label($"Heavy Steel Ball: Mass = 8.0 kg (Low Bounce)");
        }
        if (woodenCrateRb != null)
        {
            GUILayout.Label($"Wooden Crate: Mass = 3.5 kg (Box Collider)");
        }

        GUILayout.Space(6);
        if (GUILayout.Button("Reset Simulation Drop (Press R)"))
        {
            ResetSimulation();
        }

        GUILayout.EndArea();
    }
}
