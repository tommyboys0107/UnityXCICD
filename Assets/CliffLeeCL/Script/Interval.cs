using System;

namespace CliffLeeCL
{
    /// <summary>
    /// Base generic class for storing interval.
    /// </summary>
    /// <typeparam name="T">Type defines minimun and maximun.</typeparam>
    [Serializable]
    public class Interval<T>
    {
        /// <summary>
        /// The variable store minimun value.
        /// </summary>
        public T minimun;
        /// <summary>
        /// The variable store maximun value.
        /// </summary>
        public T maximun;

        /// <summary>
        /// Constructor that sets both minimum and maximun.
        /// </summary>
        /// <param name="min">Minimun value.</param>
        /// <param name="max">Maximun value.</param>
        /// <seealso cref="minimun"/>
        /// <seealso cref="maximun"/>
        public Interval(T min, T max)
        {
            minimun = min;
            maximun = max;
        }
    }

    /// <summary>
    /// The class act as Interval<int>, but the propose is to make variables can be shown in editor.
    /// </summary>
    [Serializable]
    public class IntervalInt : Interval<int>
    {
        /// <summary>
        /// Consturctor that sets both minimum and maximun.
        /// </summary>
        /// <param name="min">Minimun value.</param>
        /// <param name="max">Maximun value.</param>
        /// <seealso cref="minimun"/>
        /// <seealso cref="maximun"/>
        public IntervalInt(int min, int max) : base(min, max) {}
    }

    /// <summary>
    /// The class act as Interval<float>, but the propose is to make variables can be shown in editor.
    /// </summary>
    [Serializable]
    public class IntervalFloat : Interval<float>
    {
        /// <summary>
        /// Consturctor that sets both minimum and maximun.
        /// </summary>
        /// <param name="min">Minimun value.</param>
        /// <param name="max">Maximun value.</param>
        /// <seealso cref="minimun"/>
        /// <seealso cref="maximun"/>
        public IntervalFloat(float min, float max) : base(min, max) {}
    }
}
