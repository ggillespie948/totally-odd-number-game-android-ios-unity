using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LayoutGroup3D))]
public class LayoutGroup3DEditor : Editor
{
    private LayoutGroup3D LayoutGroup;

    private LayoutStyle Style;
    private float Spacing;
    private Vector3 ElementDimensions;

    private int GridConstraintCount;
    private int SecondaryConstraintCount;

    private float MaxArcAngle;
    private float Radius;
    private float StartAngleOffset;
    private bool AlignToRadius;
    private float SpiralFactor;
    private LayoutAxis3D LayoutAxis;
    private LayoutAxis3D SecondaryLayoutAxis;
    private LayoutAxis2D GridLayoutAxis;
    private Vector3 StartPositionOffset;

    public override void OnInspectorGUI()
    {
        LayoutGroup = target as LayoutGroup3D;

        DrawDefaultInspector();

        bool shouldRebuild = false;

        // Record rotations of all children if not forcing alignment in radial mode
        if (!(LayoutGroup.Style == LayoutStyle.Radial && LayoutGroup.AlignToRadius))
        {
            LayoutGroup.RecordRotations();
        }

        // Element Dimensions
        EditorGUI.BeginChangeCheck();

        ElementDimensions = EditorGUILayout.Vector3Field("Element Dimensions", LayoutGroup.ElementDimensions);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(LayoutGroup, "Change Element Dimensions");
            LayoutGroup.ElementDimensions = ElementDimensions;
            shouldRebuild = true;
        }

        // Start Offset
        EditorGUI.BeginChangeCheck();

        StartPositionOffset = EditorGUILayout.Vector3Field("Start Position Offset", LayoutGroup.StartPositionOffset);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(LayoutGroup, "Change Position Offset");
            LayoutGroup.StartPositionOffset = StartPositionOffset;
            shouldRebuild = true;
        }

        EditorGUI.BeginChangeCheck();

        Style = (LayoutStyle)EditorGUILayout.EnumPopup("Layout Style", LayoutGroup.Style);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(LayoutGroup, "Change Layout Style");
            LayoutGroup.Style = Style;
            shouldRebuild = true;
        }

        EditorGUI.BeginChangeCheck();

        if (Style == LayoutStyle.Linear)
        {
            LayoutAxis = (LayoutAxis3D)EditorGUILayout.EnumPopup("Layout Axis", LayoutGroup.LayoutAxis);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(LayoutGroup, "Change Layout Axis");
                LayoutGroup.LayoutAxis = LayoutAxis;
                shouldRebuild = true;
            }
        }
        else if (Style == LayoutStyle.Grid)
        {
            GridLayoutAxis = (LayoutAxis2D)EditorGUILayout.EnumPopup("Primary Layout Axis", LayoutGroup.GridLayoutAxis);
            GridConstraintCount = EditorGUILayout.IntField("Constraint Count", LayoutGroup.GridConstraintCount);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(LayoutGroup, "Change Grid Layout Options");
                LayoutGroup.GridConstraintCount = GridConstraintCount;
                LayoutGroup.GridLayoutAxis = GridLayoutAxis;
                shouldRebuild = true;
            }
        }
        else if (Style == LayoutStyle.Euclidean)
        {
            LayoutAxis = (LayoutAxis3D)EditorGUILayout.EnumPopup("Primary Layout Axis", LayoutGroup.LayoutAxis);
            SecondaryLayoutAxis = (LayoutAxis3D)EditorGUILayout.EnumPopup("Secondary Layout Axis", LayoutGroup.SecondaryLayoutAxis);

            GridConstraintCount = EditorGUILayout.IntField("Primary Constraint Count", LayoutGroup.GridConstraintCount);
            SecondaryConstraintCount = EditorGUILayout.IntField("Secondary Constraint Count", LayoutGroup.SecondaryConstraintCount);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(LayoutGroup, "Change Euclidean Layout Options");
                LayoutGroup.GridConstraintCount = GridConstraintCount;
                LayoutGroup.SecondaryConstraintCount = SecondaryConstraintCount;
                LayoutGroup.LayoutAxis = LayoutAxis;
                LayoutGroup.SecondaryLayoutAxis = SecondaryLayoutAxis;
                shouldRebuild = true;
            }
        }
        else if (Style == LayoutStyle.Radial)
        {
            MaxArcAngle = EditorGUILayout.FloatField("Max Arc Angle", LayoutGroup.MaxArcAngle);
            Radius = EditorGUILayout.FloatField("Radius", LayoutGroup.Radius);
            StartAngleOffset = EditorGUILayout.FloatField("Start Angle Offset", LayoutGroup.StartAngleOffset);
            SpiralFactor = EditorGUILayout.FloatField("Spiral Factor", LayoutGroup.SpiralFactor);
            AlignToRadius = EditorGUILayout.Toggle("Align To Radius", LayoutGroup.AlignToRadius);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(LayoutGroup, "Change Radial Layout Options");
                LayoutGroup.MaxArcAngle = MaxArcAngle;
                LayoutGroup.Radius = Radius;
                LayoutGroup.StartAngleOffset = StartAngleOffset;
                LayoutGroup.SpiralFactor = SpiralFactor;
                LayoutGroup.AlignToRadius = AlignToRadius;
                shouldRebuild = true;
            }
        }

        if (LayoutGroup.Style != LayoutStyle.Radial)
        {
            EditorGUI.BeginChangeCheck();
            Spacing = EditorGUILayout.FloatField("Spacing", LayoutGroup.Spacing);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(LayoutGroup, "Change spacing");
                LayoutGroup.Spacing = Spacing;
                shouldRebuild = true;
            }
        }

        if (!(LayoutGroup.Style == LayoutStyle.Radial && LayoutGroup.AlignToRadius))
        {
            LayoutGroup.RestoreRotations();
        }

        if (LayoutGroup.HasChildCountChanged() || shouldRebuild)
        {
            LayoutGroup.RebuildLayout();
        }


    }

}
