using UnityEngine;

namespace TypicalColors.Clustering
{
    public interface IReadOnlyCluster
    {
        Vector3 Point { get; }
        int ClusterId { get; }
    }

    public static partial class KmeansPlusPlus
    {
        private class Cluster : IReadOnlyCluster
        {
            public Vector3 Point { get; set; } = Vector3.zero;
            public int ClusterId { get; set; } = -1;
            public float Distance { get; set; } = 0f;


            public Cluster(Vector3 point)
            {
                this.Point = point;
            }

            public Cluster(Vector3 point, int clusterid) : this(point)
            {
                this.ClusterId = clusterid;
            }
        }
    }
}
