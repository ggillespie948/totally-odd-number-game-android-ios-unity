using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LayoutStyle
{
    Linear,
    Grid,
    Euclidean,
    Radial
}

public enum LayoutAxis3D
{
    X,
    Y,
    Z
}

public enum LayoutAxis2D
{
    X,
    Y
}


public class LayoutGroup3D : MonoBehaviour
{
    [HideInInspector]
    public List<Transform> LayoutElements;
    [HideInInspector]
    public Vector3 ElementDimensions = Vector3.one;

    [HideInInspector]
    public float Spacing;
    [HideInInspector]
    public LayoutStyle Style;

    [HideInInspector]
    public int GridConstraintCount;
    [HideInInspector]
    public int SecondaryConstraintCount;

    [HideInInspector]
    public float MaxArcAngle;
    [HideInInspector]
    public float Radius;
    [HideInInspector]
    public float StartAngleOffset;
    [HideInInspector]
    public bool AlignToRadius;
    [HideInInspector]
    public LayoutAxis3D LayoutAxis;
    [HideInInspector]
    public LayoutAxis3D SecondaryLayoutAxis;
    [HideInInspector]
    public LayoutAxis2D GridLayoutAxis;
    [HideInInspector]
    public List<Quaternion> ElementRotations;
    [HideInInspector]
    public Vector3 StartPositionOffset;
    [HideInInspector]
    public float SpiralFactor;

    public void RebuildLayout()
    {
        PopulateElementsFromChildren();

        switch(Style)
        {
            case LayoutStyle.Linear:
                LinearLayout();
                break;

            case LayoutStyle.Grid:
                GridLayout();
                break;

            case LayoutStyle.Euclidean:
                EuclideanLayout();
                break;

            case LayoutStyle.Radial:
                RadialLayout();
                break;
        }
    }

    public void LinearLayout()
    {
        Vector3 pos = Vector3.zero;

        for (int i = 0; i < LayoutElements.Count; i++)
        {
            Vector3 dimensions = ElementDimensions;

            switch (LayoutAxis)
            {
                case LayoutAxis3D.X:
                    pos.x = (float)i * (dimensions.x + Spacing);
                    break;
                case LayoutAxis3D.Y:
                    pos.y = (float)i * (dimensions.y + Spacing);
                    break;
                case LayoutAxis3D.Z:
                    pos.z = (float)i * (dimensions.z + Spacing);
                    break;
            }
            LayoutElements[i].localPosition = pos + StartPositionOffset;
        }
    }

    public void GridLayout()
    {
        Vector3 pos = Vector3.zero;

        int primaryCount = GridConstraintCount;
        int secondaryCount = Mathf.CeilToInt((float)LayoutElements.Count / primaryCount);

        float pDim = GridLayoutAxis == LayoutAxis2D.X ? ElementDimensions.x : ElementDimensions.y;
        float sDim = GridLayoutAxis == LayoutAxis2D.X ? ElementDimensions.y : ElementDimensions.x;

        int i = 0;

        for(int s = 0; s < secondaryCount; s++)
        {
            for(int p = 0; p < primaryCount; p++)
            {
                if(i < LayoutElements.Count)
                {
                    float pOffset = (float)p * (pDim + Spacing);
                    float sOffset = (float)s * (sDim + Spacing);

                    pos.x = GridLayoutAxis == LayoutAxis2D.X ? pOffset : sOffset;
                    pos.y = GridLayoutAxis == LayoutAxis2D.X ? sOffset : pOffset;

                    LayoutElements[i++].localPosition = pos + StartPositionOffset;
                }
            }
        }
    }

    public void EuclideanLayout()
    {
        Vector3 pos = Vector3.zero;

        int i = 0;

        int primaryCount = GridConstraintCount;
        int secondaryCount = SecondaryConstraintCount;
        int tertiaryCount = Mathf.CeilToInt((float)LayoutElements.Count / (primaryCount * secondaryCount));

        // Bit mask to determine final driven axis (001 = X, 010 = Y, 100 = Z)
        int tertiaryAxisMask = 7;

        #region Determine Element Dimensions in Each Axis
        float pDim = 0f, sDim = 0f, tDim = 0f;

        switch (LayoutAxis)
        {
            case LayoutAxis3D.X:
                pDim = ElementDimensions.x;
                tertiaryAxisMask ^= 1;
                break;
            case LayoutAxis3D.Y:
                pDim = ElementDimensions.y;
                tertiaryAxisMask ^= 2;
                break;
            case LayoutAxis3D.Z:
                pDim = ElementDimensions.z;
                tertiaryAxisMask ^= 4;
                break;
        }

        switch (SecondaryLayoutAxis)
        {
            case LayoutAxis3D.X:
                sDim = ElementDimensions.x;
                tertiaryAxisMask ^= 1;
                break;
            case LayoutAxis3D.Y:
                sDim = ElementDimensions.y;
                tertiaryAxisMask ^= 2;
                break;
            case LayoutAxis3D.Z:
                sDim = ElementDimensions.z;
                tertiaryAxisMask ^= 4;
                break;
        }

        switch (tertiaryAxisMask)
        {
            case 1:
                tDim = ElementDimensions.x;
                break;
            case 2:
                tDim = ElementDimensions.y;
                break;
            case 4:
                tDim = ElementDimensions.z;
                break;
        }
        #endregion

        for (int t = 0; t < tertiaryCount; t++)
        {
            for (int s = 0; s < secondaryCount; s++)
            {
                for(int p = 0; p < primaryCount; p++)
                {
                    if (i < LayoutElements.Count)
                    {
                        float pOffset = (float)p * (pDim + Spacing);
                        float sOffset = (float)s * (sDim + Spacing);
                        float tOffset = (float)t * (tDim + Spacing);

                        switch(LayoutAxis)
                        {
                            case LayoutAxis3D.X:
                                pos.x = pOffset;
                                break;
                            case LayoutAxis3D.Y:
                                pos.y = pOffset;
                                break;
                            case LayoutAxis3D.Z:
                                pos.z = pOffset;
                                break;
                        }

                        switch (SecondaryLayoutAxis)
                        {
                            case LayoutAxis3D.X:
                                pos.x = sOffset;
                                break;
                            case LayoutAxis3D.Y:
                                pos.y = sOffset;
                                break;
                            case LayoutAxis3D.Z:
                                pos.z = sOffset;
                                break;
                        }

                        switch(tertiaryAxisMask)
                        {
                            case 1:
                                pos.x = tOffset;
                                break;
                            case 2:
                                pos.y = tOffset;
                                break;
                            case 4:
                                pos.z = tOffset;
                                break;
                        }

                        LayoutElements[i++].localPosition = pos + StartPositionOffset;
                    }
                }
            }
        }
    }

    public void RadialLayout()
    {
        Vector3 pos = Vector3.zero;
        float spiralSum = 0f;
        float spiralIncrement = SpiralFactor / LayoutElements.Count;

        for(int i = 0; i < LayoutElements.Count; i++)
        {
            float angle = (float)i / (LayoutElements.Count - 1) * MaxArcAngle * Mathf.Deg2Rad;
            pos.x = Mathf.Cos(angle + Mathf.Deg2Rad * StartAngleOffset) * (Radius + spiralSum);
            pos.y = Mathf.Sin(angle + Mathf.Deg2Rad * StartAngleOffset) * (Radius + spiralSum);

            if(AlignToRadius)
            {
                Vector3 dir = transform.TransformDirection(pos);
                LayoutElements[i].up = dir;
            }

            LayoutElements[i].localPosition = pos + StartPositionOffset;
            spiralSum += spiralIncrement;
        }
    }

    public void PopulateElementsFromChildren()
    {
        if(LayoutElements == null)
        {
            LayoutElements = new List<Transform>();
        }

        if(ElementRotations == null)
        {
            ElementRotations = new List<Quaternion>();
        }

        LayoutElements.Clear();
        foreach(Transform child in transform)
        {
            LayoutElements.Add(child);
        }
    }

    public void RecordRotations()
    {
        if (LayoutElements == null)
        {
            return;
        }

        if(ElementRotations == null)
        {
            ElementRotations = new List<Quaternion>();
        }

        if (HasChildCountChanged())
        {
            PopulateElementsFromChildren();
        }

        ElementRotations.Clear();

        for (int i = 0; i < LayoutElements.Count; i++)
        {
            ElementRotations.Add(LayoutElements[i].localRotation);
        }
    }

    public void RestoreRotations()
    {
        if(LayoutElements == null || ElementRotations == null || LayoutElements.Count != ElementRotations.Count)
        {
            return;
        }

        for(int i = 0; i < LayoutElements.Count; i++)
        {
            LayoutElements[i].localRotation = ElementRotations[i];
        }
    }

    public bool HasChildCountChanged()
    {
        if (LayoutElements != null)
        {
            return transform.childCount != LayoutElements.Count;
        }

        return false;
    }
}
