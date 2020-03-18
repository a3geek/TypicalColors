using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TypicalColors.Examples
{
    using Clustering;
    using System.Threading.Tasks;
    using Classes;
    using Random = UnityEngine.Random;

    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public class TypicalColorsExample : MonoBehaviour
    {
        private const string PropTypicalColors = "_TypicalColors";
        private const string PropLength = "_Length";
        private const string PropThresholdX = "_ThresholdX";
        private const string PropThresholdBolid = "_ThresholdBolid";

        [SerializeField]
        private PickUpRunner pickUpRunner = new PickUpRunner();
        [SerializeField]
        private Color[] colors = new Color[]
        {
            Color.red, Color.blue, Color.green, Color.cyan, Color.yellow, Color.grey
        };
        [SerializeField]
        private Material material = null;
        [SerializeField, Range(0f, 1f)]
        private float thresholdX = 0.1f;
        [SerializeField, Range(0f, 0.1f)]
        private float thresholdBolid = 0.0025f;

        private ComputeBuffer buffer = null;


        private void Awake()
        {
            foreach (var e in this.GetComponentsInChildren<Renderer>().Select((render, idx) => new { render, idx }))
            {
                e.render.material.color = this.colors[e.idx % this.colors.Length];
            }

            this.buffer = new ComputeBuffer(
                this.pickUpRunner.ClusterCount, Marshal.SizeOf(typeof(Color)), ComputeBufferType.Default
            );
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
            if (this.material == null)
            {
                Graphics.Blit(src, dst);
                return;
            }

            if (this.pickUpRunner.State == PickUpRunner.RunnerState.Wait)
            {
                this.pickUpRunner.AsyncRun(src);
            }
            else if(this.pickUpRunner.State == PickUpRunner.RunnerState.Finish)
            {
                this.buffer.SetData(this.pickUpRunner.GetTypicalColors());
                this.material.SetInt(PropLength, this.buffer.count);
                this.material.SetBuffer(PropTypicalColors, this.buffer);
            }

            this.material.SetFloat(PropThresholdX, this.thresholdX);
            this.material.SetFloat(PropThresholdBolid, this.thresholdBolid);
            Graphics.Blit(src, dst, this.material);
        }

        private void OnDestroy()
        {
            this.buffer?.Dispose();
        }
    }
}
