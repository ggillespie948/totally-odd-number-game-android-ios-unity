using System;
using UnityEditor;
using UnityEngine;

namespace Borodar.FarlandSkies.Core.Editor
{
    public class RateMeDialog
    {
        private static readonly Color BG_COLOR_PRO = new Color(0.17f, 0.17f, 0.17f);
        private static readonly Color BG_COLOR_FREE = new Color(0.6f, 0.6f, 0.6f);

        private const string PREF_DIALOG_SUFFIX = ".showRateDialog";
        private const string PREF_TIME_SUFFIX = ".nextReviewDate";

        private const string MESSAGE = "Hey friend! If you enjoy using \"{0}\", would you mind taking a moment to rate it and leave a comment " +
                                       "on the Asset Store? It won't take more than minute.\n\nThanks in advance and have a great day!";
        private const string BUTTON_RATE = "Rate now";
        private const string BUTTON_HIDE = "No, thanks";
        private const string BUTTON_LATER = "Remind later";

        private const double DELAY_DAYS = 1;

        private const string GA_CAMPAIGN_PARAMS = "?utm_source=rate_me_dialog&utm_campaign=unity_editor";

        //---------------------------------------------------------------------
        // Public
        //---------------------------------------------------------------------

        public static void DrawRateDialog(string assetName, string assetStoreID)
        {
            // Check dialog flag
            var ratePrefKey = assetStoreID + PREF_DIALOG_SUFFIX;
            if (!EditorPrefs.GetBool(ratePrefKey, true)) return;

            // Check review date
            var reviewDateKey = assetStoreID + PREF_TIME_SUFFIX;
            if (!EditorPrefs.HasKey(reviewDateKey))
            {
                UpdateReviewDate(reviewDateKey);
                return;
            }

            var reviewDateString = EditorPrefs.GetString(reviewDateKey);
            var reviewDate = DateTime.FromBinary(Convert.ToInt64(reviewDateString));
            if (DateTime.Now < reviewDate) return;

            // Show dialog
            EditorGUILayout.Space();

            var defaultColor = GUI.backgroundColor;
            GUI.color = (EditorGUIUtility.isProSkin) ? BG_COLOR_PRO : BG_COLOR_FREE;

            EditorGUILayout.BeginVertical(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).GetStyle("sv_iconselector_labelselection"), GUILayout.MinHeight(10f));
            {
                GUI.color = defaultColor;
                EditorGUILayout.HelpBox(string.Format(MESSAGE, assetName), MessageType.Info);
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button(BUTTON_LATER, GUILayout.Width(100f), GUILayout.MinWidth(75f)))
                    {
                        UpdateReviewDate(reviewDateKey);
                    }

                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button(BUTTON_HIDE, GUILayout.Width(100f), GUILayout.MinWidth(75f)))
                    {
                        EditorPrefs.SetBool(ratePrefKey, false);
                    }

                    if (GUILayout.Button(BUTTON_RATE, GUILayout.Width(100f), GUILayout.MinWidth(75f)))
                    {
                        Application.OpenURL("com.unity3d.kharma:content/" + assetStoreID + GA_CAMPAIGN_PARAMS);
                        EditorPrefs.SetBool(ratePrefKey, false);
                    }
                }
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(3f);
            }
            EditorGUILayout.EndVertical();
        }

        //---------------------------------------------------------------------
        // Helpers
        //---------------------------------------------------------------------

        private static void UpdateReviewDate(string reviewDateKey)
        {
            var nextReviewDate = DateTime.Now.AddDays(DELAY_DAYS).ToBinary();
            EditorPrefs.SetString(reviewDateKey, nextReviewDate.ToString());
        }
    }
}