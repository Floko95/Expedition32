using UnityEngine;
using UnityEditor;

namespace InputIcons
{
    // Create a new script file with this class
    public class InputIconsAssetPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            // Check imported (including duplicated) assets
            foreach (string assetPath in importedAssets)
            {
                // Check if it's one of our ScriptableObject types
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

                // Handle InputIconsManagerSO
                if (asset is InputIconsManagerSO manager)
                {
                    SerializedObject serializedManager = new SerializedObject(manager);
                    var isActualManager = serializedManager.FindProperty("isActualManager");

                    if (isActualManager.boolValue)
                    {
                        // Disable isActualManager on all other instances
                        string[] guids = AssetDatabase.FindAssets("t:InputIconsManagerSO");
                        foreach (string guid in guids)
                        {
                            string path = AssetDatabase.GUIDToAssetPath(guid);
                            InputIconsManagerSO otherManager = AssetDatabase.LoadAssetAtPath<InputIconsManagerSO>(path);

                            if (otherManager != manager) // Skip the current asset
                            {
                                SerializedObject otherSerializedObject = new SerializedObject(otherManager);
                                var otherIsActualManager = otherSerializedObject.FindProperty("isActualManager");
                                if (otherIsActualManager.boolValue)
                                {
                                    otherIsActualManager.boolValue = false;
                                    otherSerializedObject.ApplyModifiedProperties();
                                }
                            }
                        }
                    }
                }

                // Handle InputIconSetConfiguratorSO
                if (asset is InputIconSetConfiguratorSO configurator)
                {
                    SerializedObject serializedConfigurator = new SerializedObject(configurator);
                    var isActualManager = serializedConfigurator.FindProperty("isActualManager");

                    if (isActualManager.boolValue)
                    {
                        // Disable isActualManager on all other instances
                        string[] guids = AssetDatabase.FindAssets("t:InputIconSetConfiguratorSO");
                        foreach (string guid in guids)
                        {
                            string path = AssetDatabase.GUIDToAssetPath(guid);
                            InputIconSetConfiguratorSO otherConfigurator = AssetDatabase.LoadAssetAtPath<InputIconSetConfiguratorSO>(path);

                            if (otherConfigurator != configurator) // Skip the current asset
                            {
                                SerializedObject otherSerializedObject = new SerializedObject(otherConfigurator);
                                var otherIsActualManager = otherSerializedObject.FindProperty("isActualManager");
                                if (otherIsActualManager.boolValue)
                                {
                                    otherIsActualManager.boolValue = false;
                                    otherSerializedObject.ApplyModifiedProperties();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}