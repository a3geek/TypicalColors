using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TypicalColors.Examples.Classes
{
    using Clustering;
    using System.Threading.Tasks;
    using Random = UnityEngine.Random;

    [Serializable]
    public class PickUpRunner
    {
        public enum RunnerState
        {
            Wait = 1, Runnning, Finish
        }

        public RunnerState State { get; private set; } = RunnerState.Wait;
        public int ClusterCount => this.clusterCount;
        public int Iteration => this.iteration;
        public float ScaleUp => this.scaleUp;

        [SerializeField]
        private int clusterCount = 7;
        [SerializeField]
        private int iteration = 10;
        [SerializeField]
        private float scaleUp = 5f;

        private Color[] typicalColors = new Color[0];


        public void AsyncRun(RenderTexture texture)
            => this.AsyncRun(TypicalColorsPicker.GetColors(texture));

        public async void AsyncRun(Color[] colors)
        {
            if (this.State != RunnerState.Wait)
            {
                return;
            }
            this.State = RunnerState.Runnning;

            var clusterCount = this.clusterCount;
            var iteration = this.iteration;
            var scaleUp = this.scaleUp;
            var result = await Task.Run(() =>
                TypicalColorsPicker.PickUp(colors, clusterCount, iteration, scaleUp).Select(e => e.Color).ToArray()
            );

            this.typicalColors = result;
            this.State = RunnerState.Finish;
        }

        public Color[] GetTypicalColors()
        {
            if (this.State != RunnerState.Finish)
            {
                return new Color[0];
            }

            this.State = RunnerState.Wait;
            return this.typicalColors;
        }
    }
}
