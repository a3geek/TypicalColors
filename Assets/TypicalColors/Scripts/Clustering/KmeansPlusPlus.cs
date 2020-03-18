using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TypicalColors.Clustering
{
    using Random = UnityEngine.Random;

    public static partial class KmeansPlusPlus
    {
        public static List<IReadOnlyCluster> Calculate(List<Vector3> points, int clusterCount, int iteration, out List<IReadOnlyCluster> centroids)
        {
            var cents = new List<Cluster>(clusterCount){
                new Cluster(points[Rand(0, points.Count)], 0)
            };
            var clusters = points.ConvertAll(p => new Cluster(p));

            InitialClustering(clusters, cents);
            while(cents.Count < clusterCount)
            {
                AddCentroid(clusters, cents);
                InitialClustering(clusters, cents);
            }
            
            for(var i = 0; i < iteration; i++)
            {
                CalculateAverage(clusters, cents);
                NeighborClustering(clusters, cents);
            }

            centroids = new List<IReadOnlyCluster>(cents);
            return new List<IReadOnlyCluster>(clusters);
        }

        private static void InitialClustering(List<Cluster> clusters, List<Cluster> centroids)
        {
            for(var i = 0; i < clusters.Count; i++)
            {
                var c = clusters[i];

                for(var j = 0; j < centroids.Count; j++)
                {
                    var d = (centroids[j].Point - c.Point).sqrMagnitude;
                    if(c.ClusterId < 0 || d < c.Distance)
                    {
                        c.Distance = d;
                        c.ClusterId = j;
                    }
                }
            }
        }

        private static void AddCentroid(List<Cluster> clusters, List<Cluster> centroids)
        {
            var sum = clusters.Sum(c => c.Distance);

            var rnd = Rand01();
            var rate = 0d;
            for(var i = 0; i < clusters.Count; i++)
            {
                var c = clusters[i];
                rate += c.Distance / sum;

                if(rate >= rnd)
                {
                    centroids.Add(new Cluster(c.Point, centroids.Count));
                    break;
                }
            }
        }

        private static void CalculateAverage(List<Cluster> clusters, List<Cluster> centroids)
        {
            var average = new List<Cluster>(
                centroids.Select(c => new Cluster(Vector3.zero, 0))
            );

            for(var i = 0; i < clusters.Count; i++)
            {
                var c = clusters[i];

                average[c.ClusterId].Point += c.Point;
                average[c.ClusterId].ClusterId++;
            }

            for(var i = 0; i < average.Count; i++)
            {
                centroids[i].Point = average[i].Point / average[i].ClusterId;
            }
        }

        private static void NeighborClustering(List<Cluster> clusters, List<Cluster> centroids)
        {
            for(var i = 0; i < clusters.Count; i++)
            {
                var c = clusters[i];
                var dis1 = (centroids[c.ClusterId].Point - c.Point).sqrMagnitude;

                for(var j = 0; j < centroids.Count; j++)
                {
                    var dis2 = (centroids[j].Point - c.Point).sqrMagnitude;
                    if(dis1 > dis2)
                    {
                        c.ClusterId = j;
                    }
                }
            }
        }
    }
}
