using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

public static class ParkPhysicsSceneBuilder
{
    private const string BaseFolder = "Assets/ParkPhysicsEnvironment";
    private const string MatFolder = BaseFolder + "/Materials";
    private const string PhysMatFolder = BaseFolder + "/PhysicsMaterials";
    private const string ScenesFolder = BaseFolder + "/Scenes";

    [MenuItem("Tools/Build Park Physics Environment")]
    public static void BuildScene()
    {
        EnsureFoldersExist();

        // 1. Create or Find Physics Material with high bounce
        PhysicsMaterial bouncyPhysMat = CreateBouncyPhysicsMaterial();

        // 2. Create Render Materials
        Material grassMat = CreateMaterial("Mat_ParkGrass", new Color(0.24f, 0.52f, 0.20f), 0.1f);
        Material pathMat = CreateMaterial("Mat_StonePath", new Color(0.72f, 0.70f, 0.65f), 0.2f);
        Material woodMat = CreateMaterial("Mat_RusticWood", new Color(0.55f, 0.28f, 0.12f), 0.3f);
        Material metalMat = CreateMaterial("Mat_DarkMetal", new Color(0.20f, 0.22f, 0.25f), 0.7f);
        Material barkMat = CreateMaterial("Mat_TreeBark", new Color(0.38f, 0.22f, 0.10f), 0.15f);
        Material leavesMat = CreateMaterial("Mat_TreeLeaves", new Color(0.18f, 0.45f, 0.18f), 0.1f);
        Material bouncyBallMat = CreateMaterial("Mat_BouncyBall_Red", new Color(0.95f, 0.20f, 0.12f), 0.85f);
        Material bowlingBallMat = CreateMaterial("Mat_HeavyBall_Blue", new Color(0.12f, 0.22f, 0.55f), 0.9f);
        Material crateMat = CreateMaterial("Mat_WoodenCrate", new Color(0.82f, 0.64f, 0.40f), 0.2f);
        Material lampMat = CreateMaterial("Mat_LampDark", new Color(0.15f, 0.15f, 0.15f), 0.8f);
        Material lampGlowMat = CreateMaterial("Mat_LampGlow", new Color(1.0f, 0.95f, 0.70f), 0.1f);

        // 3. Create a new empty scene
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Root Organizers
        GameObject envRoot = new GameObject("--- PARK ENVIRONMENT ---");
        GameObject physicsRoot = new GameObject("--- DYNAMIC PHYSICS OBJECTS ---");
        GameObject sysRoot = new GameObject("--- LIGHTS & CAMERA ---");

        // 4. Create Ground Plane
        GameObject groundPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        groundPlane.name = "Ground_Plane";
        groundPlane.transform.SetParent(envRoot.transform);
        groundPlane.transform.position = Vector3.zero;
        groundPlane.transform.rotation = Quaternion.identity;
        groundPlane.transform.localScale = new Vector3(3f, 1f, 3f); // 30m x 30m park
        groundPlane.GetComponent<MeshRenderer>().sharedMaterial = grassMat;

        // Ensure sturdy collider on ground (MeshCollider + thick BoxCollider to prevent tunneling)
        BoxCollider groundBoxCol = groundPlane.AddComponent<BoxCollider>();
        groundBoxCol.size = new Vector3(10f, 0.6f, 10f);
        groundBoxCol.center = new Vector3(0f, -0.3f, 0f);

        // 5. Pathway
        GameObject pathway = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pathway.name = "Park_Pathway";
        pathway.transform.SetParent(envRoot.transform);
        pathway.transform.position = new Vector3(0f, 0.02f, 0f);
        pathway.transform.localScale = new Vector3(2.4f, 0.05f, 18.0f);
        pathway.GetComponent<MeshRenderer>().sharedMaterial = pathMat;

        // 6. Park Bench (Hierarchy with Seat, Backrest, Legs)
        GameObject benchGroup = new GameObject("Park_Bench");
        benchGroup.transform.SetParent(envRoot.transform);
        benchGroup.transform.position = new Vector3(-2.8f, 0f, 1.5f);

        // Bench Seat (Cube 1)
        GameObject benchSeat = GameObject.CreatePrimitive(PrimitiveType.Cube);
        benchSeat.name = "Bench_Seat_Cube";
        benchSeat.transform.SetParent(benchGroup.transform);
        benchSeat.transform.localPosition = new Vector3(0f, 0.55f, 0f);
        benchSeat.transform.localScale = new Vector3(2.6f, 0.14f, 0.9f);
        benchSeat.GetComponent<MeshRenderer>().sharedMaterial = woodMat;

        // Bench Backrest (Cube 2)
        GameObject benchBack = GameObject.CreatePrimitive(PrimitiveType.Cube);
        benchBack.name = "Bench_Backrest_Cube";
        benchBack.transform.SetParent(benchGroup.transform);
        benchBack.transform.localPosition = new Vector3(0f, 1.05f, 0.4f);
        benchBack.transform.localRotation = Quaternion.Euler(-12f, 0f, 0f);
        benchBack.transform.localScale = new Vector3(2.6f, 0.75f, 0.12f);
        benchBack.GetComponent<MeshRenderer>().sharedMaterial = woodMat;

        // Bench Left Leg
        GameObject benchLegL = GameObject.CreatePrimitive(PrimitiveType.Cube);
        benchLegL.name = "Bench_Leg_Left";
        benchLegL.transform.SetParent(benchGroup.transform);
        benchLegL.transform.localPosition = new Vector3(-1.1f, 0.25f, 0f);
        benchLegL.transform.localScale = new Vector3(0.15f, 0.5f, 0.75f);
        benchLegL.GetComponent<MeshRenderer>().sharedMaterial = metalMat;

        // Bench Right Leg
        GameObject benchLegR = GameObject.CreatePrimitive(PrimitiveType.Cube);
        benchLegR.name = "Bench_Leg_Right";
        benchLegR.transform.SetParent(benchGroup.transform);
        benchLegR.transform.localPosition = new Vector3(1.1f, 0.25f, 0f);
        benchLegR.transform.localScale = new Vector3(0.15f, 0.5f, 0.75f);
        benchLegR.GetComponent<MeshRenderer>().sharedMaterial = metalMat;

        // 7. Park Trash Can (Cube 3)
        GameObject trashBin = GameObject.CreatePrimitive(PrimitiveType.Cube);
        trashBin.name = "Park_TrashCan_Cube";
        trashBin.transform.SetParent(envRoot.transform);
        trashBin.transform.position = new Vector3(-4.6f, 0.65f, 1.5f);
        trashBin.transform.localScale = new Vector3(0.85f, 1.3f, 0.85f);
        trashBin.GetComponent<MeshRenderer>().sharedMaterial = metalMat;

        // 8. Park Planter Box (Cube 4)
        GameObject planter = GameObject.CreatePrimitive(PrimitiveType.Cube);
        planter.name = "Park_PlanterBox_Cube";
        planter.transform.SetParent(envRoot.transform);
        planter.transform.position = new Vector3(2.8f, 0.4f, 1.5f);
        planter.transform.localScale = new Vector3(1.6f, 0.8f, 1.6f);
        planter.GetComponent<MeshRenderer>().sharedMaterial = woodMat;

        // 9. Tree with Cylinder Trunk and Foliage Sphere
        GameObject treeGroup = new GameObject("Park_Tree");
        treeGroup.transform.SetParent(envRoot.transform);
        treeGroup.transform.position = new Vector3(4.5f, 0f, -2.5f);

        // Cylinder Trunk
        GameObject trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        trunk.name = "Tree_Trunk_Cylinder";
        trunk.transform.SetParent(treeGroup.transform);
        trunk.transform.localPosition = new Vector3(0f, 1.75f, 0f);
        trunk.transform.localScale = new Vector3(0.7f, 1.75f, 0.7f);
        trunk.GetComponent<MeshRenderer>().sharedMaterial = barkMat;

        // Foliage Sphere
        GameObject foliage = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        foliage.name = "Tree_Foliage_Sphere";
        foliage.transform.SetParent(treeGroup.transform);
        foliage.transform.localPosition = new Vector3(0f, 4.0f, 0f);
        foliage.transform.localScale = new Vector3(3.0f, 2.8f, 3.0f);
        foliage.GetComponent<MeshRenderer>().sharedMaterial = leavesMat;

        // 10. Park Lamp Post (Cylinder)
        GameObject lampGroup = new GameObject("Park_LampPost");
        lampGroup.transform.SetParent(envRoot.transform);
        lampGroup.transform.position = new Vector3(-2.2f, 0f, -2.2f);

        GameObject lampPole = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        lampPole.name = "Lamp_Pole_Cylinder";
        lampPole.transform.SetParent(lampGroup.transform);
        lampPole.transform.localPosition = new Vector3(0f, 2.0f, 0f);
        lampPole.transform.localScale = new Vector3(0.18f, 2.0f, 0.18f);
        lampPole.GetComponent<MeshRenderer>().sharedMaterial = lampMat;

        GameObject lampHead = GameObject.CreatePrimitive(PrimitiveType.Cube);
        lampHead.name = "Lamp_LightFixture_Cube";
        lampHead.transform.SetParent(lampGroup.transform);
        lampHead.transform.localPosition = new Vector3(0f, 4.15f, 0f);
        lampHead.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
        lampHead.GetComponent<MeshRenderer>().sharedMaterial = lampGlowMat;

        // --- DYNAMIC PHYSICS OBJECTS ---

        // 11. Sphere 1: Bouncing Rubber Ball (Required bouncing effect)
        GameObject bouncySphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bouncySphere.name = "Bouncing_RubberSphere";
        bouncySphere.transform.SetParent(physicsRoot.transform);
        bouncySphere.transform.position = new Vector3(0f, 8.0f, -0.6f);
        bouncySphere.transform.localScale = Vector3.one * 1.0f;
        bouncySphere.GetComponent<MeshRenderer>().sharedMaterial = bouncyBallMat;

        SphereCollider sphereCol = bouncySphere.GetComponent<SphereCollider>();
        sphereCol.sharedMaterial = bouncyPhysMat; // High bounciness physics material!

        Rigidbody sphereRb = bouncySphere.AddComponent<Rigidbody>();
        sphereRb.mass = 1.0f;
        sphereRb.useGravity = true;
        sphereRb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // 12. Sphere 2: Heavy Metal / Bowling Ball
        GameObject heavySphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        heavySphere.name = "Heavy_SteelSphere";
        heavySphere.transform.SetParent(physicsRoot.transform);
        heavySphere.transform.position = new Vector3(-2.0f, 6.5f, -0.6f);
        heavySphere.transform.localScale = Vector3.one * 0.9f;
        heavySphere.GetComponent<MeshRenderer>().sharedMaterial = bowlingBallMat;

        Rigidbody heavyRb = heavySphere.AddComponent<Rigidbody>();
        heavyRb.mass = 8.0f; // Distinct heavier mass
        heavyRb.useGravity = true;

        // 13. Cube Physics Object: Wooden Crate
        GameObject physicsCrate = GameObject.CreatePrimitive(PrimitiveType.Cube);
        physicsCrate.name = "Physics_WoodenCrate";
        physicsCrate.transform.SetParent(physicsRoot.transform);
        physicsCrate.transform.position = new Vector3(2.8f, 3.8f, 1.5f);
        physicsCrate.transform.rotation = Quaternion.Euler(15f, 25f, 10f);
        physicsCrate.transform.localScale = Vector3.one * 0.95f;
        physicsCrate.GetComponent<MeshRenderer>().sharedMaterial = crateMat;

        Rigidbody crateRb = physicsCrate.AddComponent<Rigidbody>();
        crateRb.mass = 3.5f;
        crateRb.useGravity = true;

        // --- LIGHTS & CAMERA ---

        // 14. Directional Light
        GameObject sunLight = new GameObject("Directional Light (Sun)");
        sunLight.transform.SetParent(sysRoot.transform);
        Light lightComp = sunLight.AddComponent<Light>();
        lightComp.type = LightType.Directional;
        lightComp.color = new Color(1.0f, 0.96f, 0.88f);
        lightComp.intensity = 1.25f;
        lightComp.shadows = LightShadows.Soft;
        sunLight.transform.position = new Vector3(0f, 10f, 0f);
        sunLight.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        // 15. Main Camera
        GameObject mainCamera = new GameObject("Main Camera");
        mainCamera.transform.SetParent(sysRoot.transform);
        Camera camComp = mainCamera.AddComponent<Camera>();
        camComp.clearFlags = CameraClearFlags.Skybox;
        camComp.fieldOfView = 60f;
        mainCamera.AddComponent<AudioListener>();
        mainCamera.tag = "MainCamera";
        mainCamera.transform.position = new Vector3(0f, 5.8f, -11.5f);
        mainCamera.transform.rotation = Quaternion.Euler(18f, 0f, 0f);

        // 16. Demo Manager (Telemetry HUD & Reset Controller)
        GameObject demoMgrObj = new GameObject("Physics_Demo_Manager");
        demoMgrObj.transform.SetParent(sysRoot.transform);
        PhysicsBouncingDemo demo = demoMgrObj.AddComponent<PhysicsBouncingDemo>();
        demo.bouncingSphereRb = sphereRb;
        demo.heavySphereRb = heavyRb;
        demo.woodenCrateRb = crateRb;

        // 17. Save Scene
        string scenePath1 = ScenesFolder + "/ParkPhysicsEnvironment.unity";
        string scenePath2 = "Assets/ParkPhysicsEnvironment.unity";

        EditorSceneManager.SaveScene(newScene, scenePath1);
        AssetDatabase.CopyAsset(scenePath1, scenePath2);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"<color=green>SUCCESS: ParkPhysicsEnvironment scene built and saved at {scenePath1} and {scenePath2}!</color>");
    }

    private static void EnsureFoldersExist()
    {
        if (!AssetDatabase.IsValidFolder(BaseFolder))
            AssetDatabase.CreateFolder("Assets", "ParkPhysicsEnvironment");
        if (!AssetDatabase.IsValidFolder(MatFolder))
            AssetDatabase.CreateFolder(BaseFolder, "Materials");
        if (!AssetDatabase.IsValidFolder(PhysMatFolder))
            AssetDatabase.CreateFolder(BaseFolder, "PhysicsMaterials");
        if (!AssetDatabase.IsValidFolder(ScenesFolder))
            AssetDatabase.CreateFolder(BaseFolder, "Scenes");
    }

    private static PhysicsMaterial CreateBouncyPhysicsMaterial()
    {
        string path = PhysMatFolder + "/Bouncy_ParkPhysMat.physicMaterial";
        PhysicsMaterial existing = AssetDatabase.LoadAssetAtPath<PhysicsMaterial>(path);
        if (existing != null) return existing;

        PhysicsMaterial physMat = new PhysicsMaterial("Bouncy_ParkPhysMat");
        physMat.bounciness = 0.92f;
        physMat.bounceCombine = PhysicsMaterialCombine.Maximum;
        physMat.dynamicFriction = 0.25f;
        physMat.staticFriction = 0.25f;
        physMat.frictionCombine = PhysicsMaterialCombine.Average;

        AssetDatabase.CreateAsset(physMat, path);
        return physMat;
    }

    private static Material CreateMaterial(string name, Color color, float smoothness)
    {
        string path = MatFolder + "/" + name + ".mat";
        Material existing = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (existing != null)
        {
            SetMaterialProperties(existing, color, smoothness);
            return existing;
        }

        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        if (shader == null) shader = Shader.Find("Hidden/Universal Render Pipeline/FallbackError");

        Material mat = new Material(shader);
        mat.name = name;
        SetMaterialProperties(mat, color, smoothness);

        AssetDatabase.CreateAsset(mat, path);
        return mat;
    }

    private static void SetMaterialProperties(Material mat, Color color, float smoothness)
    {
        if (mat.HasProperty("_BaseColor"))
            mat.SetColor("_BaseColor", color);
        if (mat.HasProperty("_Color"))
            mat.SetColor("_Color", color);

        if (mat.HasProperty("_Smoothness"))
            mat.SetFloat("_Smoothness", smoothness);
        else if (mat.HasProperty("_Glossiness"))
            mat.SetFloat("_Glossiness", smoothness);

        EditorUtility.SetDirty(mat);
    }
}
