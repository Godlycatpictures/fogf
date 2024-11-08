using UnityEngine;
using Pathfinding;

namespace com.spacepuppy.Pathfinding
{
    public static class AGAStarUtils
    {
        public static GraphUpdateObject GetGUO(this GraphUpdateScene gus)
        {
            GraphUpdateObject guo = null;
            GetGUO(gus, ref guo);
            return guo;
        }

        public static void GetGUO(this GraphUpdateScene gus, ref GraphUpdateObject guo)
        {
            if (gus.points == null || gus.points.Length == 0)
            {
                var polygonCollider = gus.GetComponent<PolygonCollider2D>();
                if (polygonCollider != null)
                {
                    Vector3[] pts = new Vector3[polygonCollider.points.Length];
                    for (int i = 0; i < pts.Length; i++)
                    {
                        var p = polygonCollider.points[i] + polygonCollider.offset;
                        pts[i] = new Vector3(p.x, 0, p.y);
                    }

                    var mat = gus.transform.localToWorldMatrix * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90, 0, 0), Vector3.one);
                    var shape = new GraphUpdateShape(gus.points, gus.convex, mat, gus.minBoundsHeight);
                    guo = guo ?? new GraphUpdateObject();
                    guo.bounds = shape.GetBounds();
                    guo.shape = shape;
                }
                else
                {
                    var bounds = gus.GetBounds();
                    guo = guo ?? new GraphUpdateObject(bounds);
                    guo.shape = null;
                }
            }
            else
            {
                Vector3[] worldPoints = new Vector3[gus.points.Length];
                for (int i = 0; i < gus.points.Length; i++) worldPoints[i] = gus.transform.TransformPoint(gus.points[i]);
                var shape = new GraphUpdateShape(worldPoints, gus.convex, Matrix4x4.identity, gus.minBoundsHeight);

                guo = guo ?? new GraphUpdateObject();
                guo.bounds = shape.GetBounds();
                guo.shape = shape;
            }

            guo.nnConstraint = NNConstraint.None;
            guo.modifyWalkability = gus.modifyWalkability;
            guo.setWalkability = gus.setWalkability;
            guo.addPenalty = gus.penaltyDelta;
            guo.updatePhysics = gus.updatePhysics;
            guo.updateErosion = gus.updateErosion;
            guo.resetPenaltyOnPhysics = gus.resetPenaltyOnPhysics;

            guo.modifyTag = gus.modifyTag;
            guo.setTag = gus.setTag;
        }
    }
}
