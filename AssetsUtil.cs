using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace SuperhotRandomizerAssets
{
    public static class AssetsUtil
    {
        /// <summary>
        /// Finds the material and returns it from the games resources.
        /// </summary>
        /// <param name="MaterialName"> Name of the material to be found.</param>
        /// <returns> Returns the material or null if not found. </returns>
        public static Material FindMaterial(string MaterialName)
        {
            // Search through available fonts
            Material[] mats = Resources.FindObjectsOfTypeAll<Material>();
            foreach (Material mat in mats)
            {
                // MelonLoader.MelonLogger.Msg($"Mat: {mat.name}");
                if (mat.name == MaterialName)
                {
                    return mat;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the prefab and returns it from the games resources.
        /// </summary>
        /// <param name="PrefabName"> Name of the prefab to be found.</param>
        /// <returns> Returns the prefab or null if not found. </returns>
        public static GameObject FindPrefab(string PrefabName)
        {
            // Search through available fonts
            GameObject[] prefabs = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject prefab in prefabs)
            {
                MelonLoader.MelonLogger.Msg($"GameObject Found: {prefab.name}");
                if (prefab.name == PrefabName)
                {
                    return prefab;
                }
            }
            return null;
        }

        /// <summary>
        /// Prints the names of all assets in the Resources folder.
        /// </summary>
        public static UnityEngine.Object PrintAllResources(string resourceName)
        {
            // Load all assets of type Object in the Resources folder
            UnityEngine.Object[] allResources = Resources.LoadAll("");

            // Iterate through each asset and print its name
            foreach (UnityEngine.Object resource in allResources)
            {
                // Debug.Log($"Resource Name: {resource.name}, Type: {resource.GetType()}");

                if (resource.name == resourceName)
                {

                    return resource;
                }
            }

            return null;
        }
    }
}
