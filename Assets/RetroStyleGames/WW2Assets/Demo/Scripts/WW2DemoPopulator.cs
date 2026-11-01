using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RetroStyleGames.WW2Assets
{
    /// <summary>
    /// Fills the WW2 Assets demo scene with every RetroStyleGames WW2 airplane
    /// (P_RSG_Plane_*) and pilot (P_RSG_*_Pilot) prefab found in the project:
    /// one row per aircraft with its skins side by side, smallest aircraft in
    /// front, pilots in a row before them. Only installed packs show up.
    ///
    /// Prefabs are found through the asset database and never referenced by the
    /// scene, so no pack drags the others into its export. Edit-mode instances
    /// carry DontSave flags and are rebuilt on load; the saved scene stays empty.
    /// Editor only: in a player build nothing is spawned.
    /// </summary>
    [ExecuteAlways]
    public class WW2DemoPopulator : MonoBehaviour
    {
        [Tooltip("Gap between aircraft in a row, in metres.")]
        public float gap = 5f;
        [Tooltip("Gap between aircraft rows, in metres.")]
        public float rowGap = 8f;
        [Tooltip("Gap between pilots, in metres.")]
        public float pilotGap = 2f;

        private const string RootName = "[WW2 Demo] Spawned Content";
        private static readonly Regex SkinSuffix = new Regex(@"_Skin\d+$");
        private Transform root;

        private void OnEnable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                // Instantiating during OnEnable is not allowed while a scene loads.
                EditorApplication.delayCall += () => { if (this && isActiveAndEnabled) Populate(); };
                return;
            }
#endif
            Populate();
        }

        private void OnDisable() => Clear();

        [ContextMenu("Populate")]
        public void Populate()
        {
            Clear();
            List<GameObject> prefabs = FindPrefabs();
            if (prefabs.Count == 0) return;

            root = new GameObject(RootName).transform;
            root.SetParent(transform, false);

            // Aircraft rows extend toward -Z; pilots stand in front, toward +Z.
            var rows = prefabs.Where(p => !IsPilot(p.name))
                .GroupBy(p => SkinSuffix.Replace(p.name, ""))
                .Select(g => g.OrderBy(p => p.name).ToList())
                .OrderBy(row => LocalBounds(row[0]).size.magnitude);
            float frontZ = 0f;
            foreach (List<GameObject> row in rows)
                frontZ -= PlaceRow(row, frontZ, gap) + rowGap;

            var pilots = prefabs.Where(p => IsPilot(p.name)).OrderBy(p => p.name).ToList();
            if (pilots.Count > 0)
                PlaceRow(pilots, rowGap + pilots.Max(p => LocalBounds(p).size.z), pilotGap);

            // Centre the arrangement on this transform.
            Renderer[] renderers = root.GetComponentsInChildren<Renderer>();
            if (renderers.Length > 0)
            {
                Bounds all = renderers[0].bounds;
                foreach (Renderer r in renderers) all.Encapsulate(r.bounds);
                root.position -= new Vector3(all.center.x - transform.position.x, 0f, all.center.z - transform.position.z);
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
                foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
                {
                    t.gameObject.hideFlags = HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild | HideFlags.NotEditable;
                    foreach (Component c in t.GetComponents<Component>()) c.hideFlags = t.gameObject.hideFlags;
                }
#endif
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                GameObject child = transform.GetChild(i).gameObject;
                if (child.name != RootName) continue;
                if (Application.isPlaying) Destroy(child); else DestroyImmediate(child);
            }
            root = null;
        }

        /// <summary>Spawns the prefabs side by side along X, centred on X = 0, with their front edge at <paramref name="frontZ"/>. Returns the row depth.</summary>
        private float PlaceRow(List<GameObject> row, float frontZ, float spacing)
        {
            List<Bounds> bounds = row.Select(LocalBounds).ToList();
            float x = -(bounds.Sum(b => b.size.x) + spacing * (row.Count - 1)) * 0.5f;
            for (int i = 0; i < row.Count; i++)
            {
                Bounds b = bounds[i];
                GameObject go = Instantiate(row[i], root);
                go.name = row[i].name;
                go.transform.localPosition = new Vector3(x + b.size.x * 0.5f - b.center.x, 0f, frontZ - b.max.z);
                x += b.size.x + spacing;
            }
            return bounds.Max(b => b.size.z);
        }

        private static bool IsAirplane(string name) => name.StartsWith("P_RSG_Plane_");
        private static bool IsPilot(string name) => name.StartsWith("P_RSG_") && name.EndsWith("_Pilot");

        private static Bounds LocalBounds(GameObject prefab)
        {
            // Prefab assets sit at the identity transform, so renderer bounds are local bounds.
            Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>(true);
            Bounds b = renderers.Length > 0 ? renderers[0].bounds : new Bounds(Vector3.up * 0.5f, Vector3.one);
            foreach (Renderer r in renderers) b.Encapsulate(r.bounds);
            return b;
        }

        private static List<GameObject> FindPrefabs()
        {
            var result = new List<GameObject>();
#if UNITY_EDITOR
            foreach (string guid in AssetDatabase.FindAssets("t:Prefab P_RSG_"))
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
                if (prefab && (IsAirplane(prefab.name) || IsPilot(prefab.name))) result.Add(prefab);
            }
#endif
            return result;
        }
    }
}
