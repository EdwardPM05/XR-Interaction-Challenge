using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

// Menu: EC XR > Construir escena. Genera la escena EC_XR_PittmanEdward.
public static class BuildXRScene
{
    const string SceneName = "EC_XR_PittmanEdward";
    const string ScenePath = "Assets/Scenes/" + SceneName + ".unity";
    const string MaterialsDir = "Assets/Materials";

    [MenuItem("EC XR/Construir escena")]
    static void Build()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        Directory.CreateDirectory(MaterialsDir);
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // La camara la aporta el XR Origin.
        foreach (var cam in Object.FindObjectsOfType<Camera>())
            Object.DestroyImmediate(cam.gameObject);

        var root = new GameObject("Escenario").transform;

        // Piso
        var floor = Primitive(PrimitiveType.Cube, "Piso", new Vector3(0, -0.05f, 0), new Vector3(10, 0.1f, 10), Mat("Piso", new Color(0.55f, 0.38f, 0.22f)), root);

        // Limites visuales: una casita (paredes, puerta, ventanas, tejado a dos aguas)
        BuildHouse(root);

        // Mesa
        Primitive(PrimitiveType.Cube, "Mesa", new Vector3(0, 0.4f, 2), new Vector3(1.6f, 0.8f, 0.8f), Mat("Mesa", new Color(0.45f, 0.3f, 0.18f)), root);

        // Objetos manipulables (Rigidbody + XR Grab Interactable)
        Grabbable(PrimitiveType.Cube, "Cubo Agarrable", new Vector3(-0.4f, 0.95f, 2), Vector3.one * 0.2f, Mat("Cubo", new Color(0.9f, 0.25f, 0.25f)), root);
        Grabbable(PrimitiveType.Sphere, "Esfera Agarrable", new Vector3(0.4f, 0.95f, 2), Vector3.one * 0.2f, Mat("Esfera", new Color(0.25f, 0.8f, 0.35f)), root);

        // Decoracion: sofa dentro; arbol, camino y pasto fuera
        var sofaMat = Mat("Sofa", new Color(0.7f, 0.25f, 0.3f));
        Primitive(PrimitiveType.Cube, "Sofa Asiento", new Vector3(-3f, 0.25f, -3.5f), new Vector3(2f, 0.5f, 0.9f), sofaMat, root);
        Primitive(PrimitiveType.Cube, "Sofa Respaldo", new Vector3(-3f, 0.7f, -3.85f), new Vector3(2f, 0.6f, 0.2f), sofaMat, root);
        Primitive(PrimitiveType.Cube, "Pasto", new Vector3(0, -0.11f, 0), new Vector3(40f, 0.1f, 40f), Mat("Pasto", new Color(0.3f, 0.6f, 0.25f)), root);
        Primitive(PrimitiveType.Cube, "Camino", new Vector3(0, -0.04f, -7.5f), new Vector3(1.6f, 0.08f, 5f), Mat("Camino", new Color(0.75f, 0.7f, 0.6f)), root);
        Primitive(PrimitiveType.Cylinder, "Tronco", new Vector3(8f, 1f, -3f), new Vector3(0.6f, 1f, 0.6f), Mat("Tronco", new Color(0.4f, 0.26f, 0.13f)), root);
        Primitive(PrimitiveType.Sphere, "Copa del Arbol", new Vector3(8f, 3.2f, -3f), Vector3.one * 3.2f, Mat("Copa", new Color(0.15f, 0.5f, 0.2f)), root);

        // Lampara: interaccion a distancia con el rayo
        BuildLamp(root);

        // Zona de entrega: reto libre (contador)
        BuildDeliveryZone(root);

        // XR Origin (Starter Assets) + simulador de dispositivos
        InstantiateSample("XR Interaction Setup");
        InstantiateSample("XR Device Simulator");

        var light = Object.FindObjectOfType<Light>();
        if (light != null) light.transform.rotation = Quaternion.Euler(50, -30, 0);

        Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(scene, ScenePath);
        AddToBuildSettings();
        Debug.Log("Escena creada: " + ScenePath);
    }

    static void BuildHouse(Transform root)
    {
        const float h = 2.6f;
        var wall = Mat("Pared", new Color(0.96f, 0.9f, 0.75f));
        var roofMat = Mat("Tejado", new Color(0.7f, 0.25f, 0.15f));
        var frame = Mat("Marco", new Color(0.35f, 0.22f, 0.12f));
        var glass = Mat("Vidrio", new Color(0.6f, 0.85f, 1f));

        Primitive(PrimitiveType.Cube, "Pared Norte", new Vector3(0, h / 2, 5), new Vector3(10, h, 0.2f), wall, root);
        Primitive(PrimitiveType.Cube, "Pared Este", new Vector3(5, h / 2, 0), new Vector3(0.2f, h, 10), wall, root);
        Primitive(PrimitiveType.Cube, "Pared Oeste", new Vector3(-5, h / 2, 0), new Vector3(0.2f, h, 10), wall, root);

        // Pared sur con puerta de 1.6 x 2.2 m
        Primitive(PrimitiveType.Cube, "Pared Sur Izq", new Vector3(-2.9f, h / 2, -5), new Vector3(4.2f, h, 0.2f), wall, root);
        Primitive(PrimitiveType.Cube, "Pared Sur Der", new Vector3(2.9f, h / 2, -5), new Vector3(4.2f, h, 0.2f), wall, root);
        Primitive(PrimitiveType.Cube, "Dintel Puerta", new Vector3(0, 2.4f, -5), new Vector3(1.6f, 0.4f, 0.2f), wall, root);
        Primitive(PrimitiveType.Cube, "Marco Puerta Izq", new Vector3(-0.85f, 1.1f, -5.02f), new Vector3(0.1f, 2.2f, 0.25f), frame, root);
        Primitive(PrimitiveType.Cube, "Marco Puerta Der", new Vector3(0.85f, 1.1f, -5.02f), new Vector3(0.1f, 2.2f, 0.25f), frame, root);

        // Ventanas en las paredes este y oeste
        foreach (var side in new[] { -1f, 1f })
        {
            Primitive(PrimitiveType.Cube, "Vidrio " + side, new Vector3(side * 4.88f, 1.5f, 0), new Vector3(0.05f, 1f, 1.6f), glass, root);
            Primitive(PrimitiveType.Cube, "Marco Ventana " + side, new Vector3(side * 4.9f, 1.5f, 0), new Vector3(0.03f, 1.15f, 1.75f), frame, root);
        }

        // Tejado a dos aguas: prisma solido (sin huecos). No proyecta sombra
        // para no oscurecer el interior; su base sirve de techo.
        var roofGo = new GameObject("Tejado");
        roofGo.transform.SetParent(root, false);
        roofGo.transform.position = new Vector3(0, h, 0);
        roofGo.AddComponent<MeshFilter>().sharedMesh = RoofMesh(5.6f, 5.6f, 2.2f);
        var mr = roofGo.AddComponent<MeshRenderer>();
        mr.sharedMaterial = roofMat;
        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        // Chimenea
        Primitive(PrimitiveType.Cube, "Chimenea", new Vector3(3f, 4.2f, 2.5f), new Vector3(0.7f, 1.4f, 0.7f), Mat("Ladrillo", new Color(0.6f, 0.25f, 0.2f)), root);
    }

    // Prisma triangular cerrado: base de 2*hx por 2*hz, cumbrera a lo largo de X a altura rise.
    static Mesh RoofMesh(float hx, float hz, float rise)
    {
        var a = new Vector3(-hx, 0, -hz);
        var b = new Vector3(hx, 0, -hz);
        var c = new Vector3(hx, 0, hz);
        var d = new Vector3(-hx, 0, hz);
        var r1 = new Vector3(-hx, rise, 0);
        var r2 = new Vector3(hx, rise, 0);
        var center = new Vector3(0, rise / 3f, 0);

        var verts = new System.Collections.Generic.List<Vector3>();
        var tris = new System.Collections.Generic.List<int>();
        void Tri(Vector3 p, Vector3 q, Vector3 w)
        {
            // Orienta la cara hacia afuera respecto al centro del prisma.
            var n = Vector3.Cross(q - p, w - p);
            var faceCenter = (p + q + w) / 3f;
            if (Vector3.Dot(n, faceCenter - center) < 0) { var t = q; q = w; w = t; }
            int i = verts.Count;
            verts.Add(p); verts.Add(q); verts.Add(w);
            tris.Add(i); tris.Add(i + 1); tris.Add(i + 2);
        }
        Tri(a, b, c); Tri(a, c, d);        // base (techo interior)
        Tri(a, b, r2); Tri(a, r2, r1);     // agua sur
        Tri(d, c, r2); Tri(d, r2, r1);     // agua norte
        Tri(a, d, r1);                     // frontón oeste
        Tri(b, c, r2);                     // frontón este

        var mesh = new Mesh { name = "TejadoMesh" };
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        var path = MaterialsDir + "/TejadoMesh.asset";
        AssetDatabase.DeleteAsset(path);
        AssetDatabase.CreateAsset(mesh, path);
        return mesh;
    }

    static void BuildLamp(Transform root)
    {
        var pos = new Vector3(3f, 0f, 2f);
        Primitive(PrimitiveType.Cylinder, "Poste Lampara", pos + new Vector3(0, 0.75f, 0), new Vector3(0.15f, 0.75f, 0.15f), Mat("Poste", new Color(0.15f, 0.15f, 0.15f)), root);

        var bulb = Primitive(PrimitiveType.Sphere, "Lampara (Rayo)", pos + new Vector3(0, 1.7f, 0), Vector3.one * 0.4f, Mat("Bombilla", Color.yellow), root);
        var lightGo = new GameObject("Luz Lampara");
        lightGo.transform.SetParent(bulb.transform, false);
        var l = lightGo.AddComponent<Light>();
        l.type = LightType.Point;
        l.range = 6f;
        l.intensity = 3f;
        l.color = new Color(1f, 0.92f, 0.6f);

        bulb.AddComponent<XRSimpleInteractable>();
        var toggle = bulb.AddComponent<LightToggle>();
        var so = new SerializedObject(toggle);
        so.FindProperty("targetLight").objectReferenceValue = l;
        so.FindProperty("lampRenderer").objectReferenceValue = bulb.GetComponent<Renderer>();
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    static void BuildDeliveryZone(Transform root)
    {
        var zone = Primitive(PrimitiveType.Cube, "Zona de Entrega", new Vector3(-3f, 0.05f, 1f), new Vector3(1.5f, 0.1f, 1.5f), Mat("Zona", new Color(0.2f, 0.9f, 0.4f)), root);
        // La losa mantiene su collider solido: los objetos se apoyan encima y no se hunden.
        // El conteo lo hace un volumen trigger aparte, apoyado sobre la losa.
        var trigger = new GameObject("Trigger Zona");
        trigger.transform.SetParent(root, false);
        trigger.transform.position = new Vector3(-3f, 1.6f, 1f);
        var box = trigger.AddComponent<BoxCollider>();
        box.isTrigger = true;
        box.size = new Vector3(1.3f, 3f, 1.3f);

        var textGo = new GameObject("Contador");
        // Fuera de la zona (que esta aplastada) para que el texto no se deforme.
        textGo.transform.SetParent(root, false);
        textGo.transform.position = new Vector3(-3f, 1.5f, 1f);
        var tm = textGo.AddComponent<TextMesh>();
        tm.fontSize = 48;
        tm.characterSize = 0.02f;
        tm.anchor = TextAnchor.MiddleCenter;
        tm.alignment = TextAlignment.Center;
        tm.color = Color.white;

        var dz = trigger.AddComponent<DeliveryZone>();
        var so = new SerializedObject(dz);
        so.FindProperty("counterText").objectReferenceValue = tm;
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    static GameObject Grabbable(PrimitiveType type, string name, Vector3 pos, Vector3 scale, Material mat, Transform parent)
    {
        var go = Primitive(type, name, pos, scale, mat, parent);
        var rb = go.AddComponent<Rigidbody>();
        rb.mass = 0.5f;
        go.AddComponent<XRGrabInteractable>();
        return go;
    }

    static GameObject Primitive(PrimitiveType type, string name, Vector3 pos, Vector3 scale, Material mat, Transform parent)
    {
        var go = GameObject.CreatePrimitive(type);
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.position = pos;
        go.transform.localScale = scale;
        go.GetComponent<Renderer>().sharedMaterial = mat;
        return go;
    }

    static Material Mat(string name, Color color)
    {
        var path = MaterialsDir + "/" + name + ".mat";
        var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (mat == null)
        {
            mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            AssetDatabase.CreateAsset(mat, path);
        }
        mat.SetColor("_BaseColor", color);
        mat.color = color;
        EditorUtility.SetDirty(mat);
        return mat;
    }

    static void InstantiateSample(string prefabName)
    {
        var guids = AssetDatabase.FindAssets(prefabName + " t:Prefab");
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (Path.GetFileNameWithoutExtension(path) != prefabName) continue;
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            PrefabUtility.InstantiatePrefab(prefab);
            return;
        }
        Debug.LogWarning("No se encontro el prefab '" + prefabName + "'. Importa los Samples del XR Interaction Toolkit.");
    }

    static void AddToBuildSettings()
    {
        var scenes = EditorBuildSettings.scenes;
        foreach (var s in scenes) if (s.path == ScenePath) return;
        var list = new System.Collections.Generic.List<EditorBuildSettingsScene>(scenes) { new EditorBuildSettingsScene(ScenePath, true) };
        EditorBuildSettings.scenes = list.ToArray();
    }
}
