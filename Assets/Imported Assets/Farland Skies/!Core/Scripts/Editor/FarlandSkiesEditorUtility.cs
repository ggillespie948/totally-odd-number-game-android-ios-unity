using System.IO;
using UnityEditor;
using UnityEngine;

namespace Borodar.FarlandSkies.Core.Editor
{
    public static class FarlandSkiesEditorUtility
    {
        private const string LOAD_ASSET_ERROR_MSG = "Could not load {0}\n" +
                                                    "Did you move the \"Farland Skies\" folder around in your project? " +
                                                    "Go to \"Preferences -> Farland Skies\" and update the location of the asset.";

        private const string ICONS_FOLDER = "!Core/Icons/";

        //---------------------------------------------------------------------
        // Assets
        //---------------------------------------------------------------------

        public static T LoadFromAsset<T>(string relativePath) where T : UnityEngine.Object
        {
            var assetPath = Path.Combine(FarlandSkiesPreferences.HomeFolder, relativePath);
            var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);            
            if (!asset) Debug.LogError(string.Format(LOAD_ASSET_ERROR_MSG, assetPath));
            return asset;
        }

        public static Texture2D LoadTexture(string relativePath)
        {
            return LoadFromAsset<Texture2D>(relativePath);            
        }

        public static Texture2D LoadEditorIcon(string relativePath)
        {
            return LoadTexture(ICONS_FOLDER + relativePath);
        }
    }
}