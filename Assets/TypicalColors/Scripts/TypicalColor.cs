using System;
using UnityEngine;

namespace TypicalColors
{
    [Serializable]
    public struct TypicalColor
    {
        public Color Color => this.color;
        public float Percentage => this.percentage;

        [SerializeField]
        private Color color;
        [SerializeField]
        private float percentage;


        public TypicalColor(Color color, float percentage)
        {
            this.color = color;
            this.percentage = percentage;
        }
    }
}
