using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
public static class TransformExtensions
{
    #region Reset

    public static void ResetAll(this Transform transform)
    {
        transform.ResetPosition();
        transform.ResetRotation();
        transform.ResetLocalScale();
    }
    public static void ResetLocal(this Transform transform)
    {
        transform.ResetLocalPosition();
        transform.ResetLocalRotation();
        transform.ResetLocalScale();
    }
    public static void ResetPosition(this Transform transform)
    {
        transform.position = Vector3.zero;
    }
    public static void ResetLocalPosition(this Transform transform)
    {
        transform.localPosition = Vector3.zero;
    }
    public static void ResetRotation(this Transform transform)
    {
        transform.rotation = Quaternion.identity;
    }
    public static void ResetLocalRotation(this Transform transform)
    {
        transform.localRotation = Quaternion.identity;
    }
    public static void ResetLocalScale(this Transform transform)
    {
        transform.localScale = Vector3.one;
    }

    #endregion

    #region Copy

    public static void Copy(this Transform target, Transform source, 
        bool global, 
        bool copyPosition, 
        bool copyRotation, 
        bool copyScale)
    {
        if (global)
        {
            Vector3 position = copyPosition ? source.transform.position : target.transform.position;
            Quaternion rotation = copyRotation ? source.transform.rotation : target.transform.rotation;
            Vector3 scale = copyScale ? source.transform.localScale : target.transform.localScale;

            target.transform.SetTransformGlobal(position, rotation, scale);
        }
        else
        {
            Vector3 position = copyPosition ? source.transform.localPosition : target.transform.localPosition;
            Quaternion rotation = copyRotation ? source.transform.localRotation : target.transform.localRotation;
            Vector3 scale = copyScale ? source.transform.localScale : target.transform.localScale;

            target.transform.SetTransformLocal(position, rotation, scale);
        }

    }
    public static void CopyAllGlobal(this Transform target, Transform source)
    {
        target.position = source.position;
        target.rotation = source.rotation;
        target.localScale = source.localScale;
    }
    public static void CopyAllLocal(this Transform target, Transform source)
    {
        target.localPosition = source.localPosition;
        target.localRotation = source.localRotation;
        target.localScale = source.localScale;
    }
    public static void CopyRotationAndPositionGlobal(this Transform target, Transform source)
    {
        target.position = source.position;
        target.rotation = source.rotation;
    }
    public static void CopyRotationAndPositionLocal(this Transform target, Transform source)
    {
        target.localPosition = source.localPosition;
        target.localRotation = source.localRotation;
    }

    #endregion

    public static void SetTransformGlobal(this Transform transform, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = scale;
    }
    public static void SetTransformLocal(this Transform transform, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        transform.localPosition = position;
        transform.localRotation = rotation;
        transform.localScale = scale;
    }

    public static Vector3 GetGlobalScale(this Transform transform)
    {
        return transform.lossyScale;
    }
    public static Transform FindNearest(this Transform transform, Transform[] others, bool includeInactive = false)
    {
        return others.OrderBy(t => (t.position - transform.position).sqrMagnitude)
                              .Where(t => includeInactive ? true : t.gameObject.activeSelf).First();
    }
    public static Transform FindLocalNearest(this Transform transform, Transform[] others, bool includeInactive = false)
    {
        return others.OrderBy(t => (t.localPosition - transform.localPosition).sqrMagnitude)
                              .Where(t => includeInactive ? true : t.gameObject.activeSelf).First();
    }

    public static void DoDeep(this Transform transform, Action<Transform> function)
    {
        foreach (Transform child in transform.DeepChildren())
            function(child);
    }
    public static int DeepChildrenCount(this Transform transform)
    {
        return transform.DeepChildren().Count;
    }
    public static List<Transform> DeepChildren(this Transform transform)
    {
        List<Transform> children = new List<Transform>();

        void inner(Transform transform)
        {
            foreach (Transform child in transform)
            {
                children.Add(child);

                if (child.childCount == 0) continue;

                inner(child);
            }
        }

        inner(transform);

        return children;
    }

    public static void AddScaleForwardRelative(this Transform transform, Vector3 direction)
    {
        transform.localScale += direction;
        transform.Translate(direction / 2f);
    }
    public static void SetScaleForwardRelative(this Transform transform, Vector3 direction)
    {
        transform.Translate((direction - transform.localScale) / 2f);
        transform.localScale = direction;
    }
    public static void RectTransform_SetScaleForwardRelative(this RectTransform transform, Vector3 direction)
    {
        transform.Translate((direction - transform.localScale) / 2f);
        transform.localScale = direction;
    }   
}