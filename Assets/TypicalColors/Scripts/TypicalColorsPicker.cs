using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TypicalColors
{
    using Random = UnityEngine.Random;
    using Clustering;

    public static class TypicalColorsPicker
    {
        public static Color[] PickUp(Texture2D texture, int clusterCount, int iteration = 10, float scaleUp = 1f)
            => PickUp(texture.GetPixels(), clusterCount, iteration, scaleUp);

        public static Color[] PickUp(RenderTexture texture, int clusterCount, int iteration = 10, float scaleUp = 1f)
            => PickUp(GetColors(texture), clusterCount, iteration, scaleUp);

        public static Color[] PickUp(Color[] colors, int clusterCount, int iteration = 10, float scaleUp = 1f)
        {
            var hsv = colors.Select(c =>
            {
                float h, s, v;
                Color.RGBToHSV(c, out h, out s, out v);
                return new Vector3(h, s, v) * scaleUp;
            });

            List<IReadOnlyCluster> centroids;
            KmeansPlusPlus.Calculate(hsv.ToList(), clusterCount, iteration, out centroids);

            var rate = 1f / scaleUp;
            return centroids.Select(c => Color.HSVToRGB(
                c.Point.x * rate, c.Point.y * rate, c.Point.z * rate)
            ).ToArray();
        }

        public static Color[] GetColors(RenderTexture texture)
        {
            var tmp = RenderTexture.active;
            RenderTexture.active = texture;

            var tex = new Texture2D(texture.width, texture.height);
            tex.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
            tex.Apply();

            var colors = tex.GetPixels();
            RenderTexture.active = tmp;

            UnityEngine.Object.Destroy(tex);

            return colors;
        }
    }
}
