using System;
using System.Collections.Generic;
using System.Text;

namespace NatureOptimizationAlgorithms.Tests
{
    public class Tests
    {
        public static Double ObjectiveFunction(List<double> X)
        {
            return Function1(X);
        }
        private static Double Function2(List<double> X)
        {
            double result = 0;

            for (int i = 0; i < X.Count - 1; i++)
            {
                result = result + (X[i + 1] - X[i]) * (X[i + 1] - X[i]) * 100 + (X[i] - 1) * (X[i] - 1);
            }
            return result;
        }
        private static Double Function1(List<double> X)
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
    }
}
