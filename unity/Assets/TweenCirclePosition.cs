//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the object's position on circle.
/// </summary>

[AddComponentMenu("NGUI/Tween/CirclePosition")]
public class TweenCirclePosition : UITweener
{
    public Vector3 from;
    public Vector3 to;
    [Range(0, 360)]
    public float angleMax = 180;
    Transform mTrans;

    public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; } }
    public Vector3 position { get { return cachedTransform.localPosition; } set { cachedTransform.localPosition = value; } }

    private Vector3 cachedFrom;
    private Vector3 cachedTo;
    private Vector3 cachedCenter;
    private Vector3 cachedDirection;
    private readonly Vector3 Z = new Vector3(0, 0, 1);

    override protected void OnUpdate(float factor, bool isFinished)
    {
        if (cachedTo != to || cachedFrom != from)
        {
            cachedDirection = (to - from) * 0.5F;
            cachedCenter = from + cachedDirection; //center between from and to
            cachedFrom = from;
            cachedTo = to;
            cachedDirection *= 1;  //inverse direction for start begin at From

        }

        cachedTransform.localPosition = (Quaternion.AngleAxis(((1F - factor) * angleMax), Z) * cachedDirection) + cachedCenter;
    }

    public void setByRadius(Vector3 begin, Vector3 direction, float radius)
    {
        to = direction * 2 * radius + begin;
        from = begin;
    }

    public void setByPosition(Vector3 from, Vector3 to)
    {
        this.from = from;
        this.to = to;
    }

    public void setByCenter(Vector3 center, float radius)
    {
        from = center;
        to = center;
        from.y += radius;
        to.y -= radius;
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenCirclePosition Begin(GameObject go, float duration, Vector3 pos)
    {
        TweenCirclePosition comp = UITweener.Begin<TweenCirclePosition>(go, duration);
        comp.from = comp.position;
        comp.to = pos;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }
}