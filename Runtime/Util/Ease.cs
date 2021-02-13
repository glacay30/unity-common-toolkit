using System;
using UnityEngine;

/*
 * This class was made with the help of the GDC talk
 * 
 * Math for Game Programmers: Fast and Funky 1D Nonlinear Transformations
 * by Squirrel Eiserloh
 */

[Serializable]
public static class Ease
{
    /// <summary>
    /// Compact description of data to use with easing function.
    /// </summary>
    [Serializable]
    public class EaseData
    {
        /// <summary>
        /// The type of easing function.
        /// </summary>
        [Serializable]
        public enum Function
        {
            /// <summary>
            /// Linear interpolation.
            /// </summary>
            Linear,

            /// <summary>
            /// Smoothly start the motion.
            /// </summary>
            EaseIn,

            /// <summary>
            /// Smoothly end the motion.
            /// </summary>
            EaseOut,

            /// <summary>
            /// Smoothly start and end the motion.
            /// </summary>
            EaseInEaseOut,
        }

        /// <summary>
        /// Input between [InStart, InEnd] to use as t.
        /// </summary>
        [NonSerialized]
        public float input = 0.0f;

        /// <summary>
        /// Output between [OutStart, OutEnd] to use as p(t).
        /// </summary>
        [NonSerialized]
        public float output = 0.0f;

        /// <summary>
        /// Start of the input domain.
        /// </summary>
        public float InStart = 0.0f;

        /// <summary>
        /// End of the input domain.
        /// </summary>
        public float InEnd = 1.0f;

        /// <summary>
        /// Start of the output range.
        /// </summary>
        public float OutStart = 0.0f;

        /// <summary>
        /// End of the output range.
        /// </summary>
        public float OutEnd = 1.0f;

        /// <summary>
        /// The type of easing function to use.
        /// </summary>
        public Function Type = Function.Linear;

        /// <summary>
        /// The polynomial degree of the function.
        /// </summary>
        public float Degree = 1.0f;
    };

    /// <summary>
    /// Calculate an eased output with the given data.
    /// </summary>
    /// <param name="data">Easing data to use.</param>
    public static void Easing(in EaseData data)
    {
        float output = data.input - data.InStart; // [0, inEnd - inStart]
        output /= data.InEnd - data.InStart; // [0, 1]

        // apply some easing function
        {
            switch (data.Type) {
                case EaseData.Function.Linear:
                    // t
                    // (so no need to do anything)
                    break;

                case EaseData.Function.EaseIn:
                    // t^d
                    output = Mathf.Pow(output, data.Degree);
                    break;

                case EaseData.Function.EaseOut:
                    // 1 - (1 - t)^d
                    output = 1 - Mathf.Pow((1 - output), data.Degree);
                    break;

                case EaseData.Function.EaseInEaseOut:
                    var easeIn = Mathf.Pow(output, data.Degree);
                    var easeOut = 1 - Mathf.Pow((1 - output), data.Degree);
                    output = easeIn + output * (easeOut - easeIn);
                    break;
            }
        }

        output *= (data.OutEnd - data.OutStart); // [0, outEnd - outStart]
        data.output = output + data.OutStart; // [outStart, outEnd]
    }
}
