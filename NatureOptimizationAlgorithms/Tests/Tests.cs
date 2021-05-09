using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace NatureOptimizationAlgorithms.Tests
{
    public class Tests
    {
        public static Double ObjectiveFunction(List<double> X)
        {
            return F2(X);
        }




        /// <summary>
        /// Dimensions: 2  
        /// This function is a simple two-dimensional example used to illustrate methods of modeling computer-experiment output. 
        /// </summary>
        /// <param name="X"></param>
        /// [-2, 6]
        /// <returns></returns>
        private static Double GrammacyFunction(List<Double> X)
        {
            if (X.Count > 2) return 0;
            double result = 0;

            result = X[0] * Math.Exp(-X[0] * X[0] - X[1] * X[1]);

            return result;
        }
        /// <summary>
        /// Dimensions:d
        ///
        /// The Ackley function is widely used to test optimization algorithms.
        /// In its two-dimensional form, as shown in the preceding plot, it is characterized by a nearly flat outer region, and a large hole at the center.
        /// The function poses a risk for optimization algorithms, particularly hill-climbing algorithms, to be trapped in one of its many local minima. 
        ///        
        /// Recommended variable values are a = 20, b = 0.2, and c = 2π.
        /// The function is usually evaluated on the xi ∈ [-32.768, 32.768] hypercube, for all i = 1, …, d, although it may also be restricted to a smaller domain. 
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        private static Double AckleyFunction(List<double> X)
        {
            double result = 0;
            double a = 20;
            double b = 0.2;
            double c = 2 * Math.PI;

            result = -a * Math.Exp(-b * Math.Sqrt((1.0 / X.Count) * X.Sum(x => x * x))) - Math.Exp((1.0 / X.Count) * X.Sum(x => Math.Cos(x * c))) + a + Math.E;

            return result;
        }


        /// <summary>
        /// F2
        /// Boundaries: [-100, 100]
        /// Dim: 30
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        private static Double F1(List<double> X)
        {
            double result = 0;

            result = X.Sum(x => x) + X.Aggregate(1.0, (x, y) => x * y);

            return result;
        }
        /// <summary>
        /// F2
        /// Boundaries: [-100, 100]
        /// Dim: 30
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        private static Double F2(List<double> X)
        {
            double result = 0;

            result = X.Sum(x => Math.Abs(x)) + X.Aggregate(1.0, (x, y) => Math.Abs(x * y));

            return result;
        }

        private static Double F3(List<double> X)
        {
            double result = 0;
            for (int i = 0; i < X.Count; i++)
            {
                double val = 0;
                for (int j = 0; j < i; j++)
                {
                    val = val + X[j] * X[j];
                }
                val = val * val;
                result = result + val;
            }

            return result;
        }

        private static Double F4(List<double> X)
        {
            double result = Int32.MinValue;
            for (int i = 0; i < X.Count; i++)
            {
                if (Math.Abs(X[i]) > result)
                {
                    result = Math.Abs(X[i]);
                }
            }

            return result;
        }

        private static Double F5(List<double> X)
        {
            double result = 0;

            for (int i = 0; i < X.Count - 1; i++)
            {
                result = result + (X[i + 1] - X[i]) * (X[i + 1] - X[i]) * 100 + (X[i] - 1) * (X[i] - 1);
            }
            return result;
        }

        private static Double F6(List<double> X)
        {
            double result = 0;
            result = X.Sum(x => Math.Abs((x + 0.5) * (x + 0.5)));
            return result;
        }

        /// <summary>
        /// [-500, 500]
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        private static Double F8(List<double> X)
        {
            double result = 0;

            result = X.Sum(x => -x * Math.Sin(Math.Sqrt(Math.Abs(x))));
            return result;
        }

        private static Double F9(List<double> X)
        {
            // [-5.12, 5.12]
            double result = 0;

            result = X.Sum(x => x * x - 10 * Math.Sin(2 * Math.PI * x) + 10);
            return result;
        }
        private static Double F11(List<double> X)
        {
            // [-600, 600]
            double result = 0;
            Complex complex = new Complex(0, 1);
            double aggregated = 1;
            foreach (var item in X)
            {
                aggregated *= Math.Cos(item / 0.707106781);
            }
            result = X.Sum(x => x * x - X.Aggregate(1.0, (x,y) => x*y) + 1) / 4000;
            return result;
        }
    }
}
