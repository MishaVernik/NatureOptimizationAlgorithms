using NatureOptimizationAlgorithms.Algorithms;
using NatureOptimizationAlgorithms.Contracts;
using System;
using System.Collections.Generic;

namespace NatureOptimizationAlgorithms
{
    class Program
    {
        static void Main(string[] args)
        {
            int maxIterations, numberOfPopulation, numberOfDimensions;
            List<double> upperBoundaries, lowerBoundaries;
            Initialize(out maxIterations, out numberOfPopulation, out numberOfDimensions, out upperBoundaries, out lowerBoundaries);

            List<IOptimizer> optimizers = new List<IOptimizer>();
            optimizers.Add(new ParticleSwarmOptimization());
            optimizers.Add(new GrayWolfOptimizer());
            optimizers.Add(new WhaleOptimizationAlgorithm());


            for (int optimizerIndex = 0; optimizerIndex < optimizers.Count; optimizerIndex++)
            {
                optimizers[optimizerIndex].Initialize(maxIterations, numberOfPopulation, numberOfDimensions, upperBoundaries, lowerBoundaries);
                optimizers[optimizerIndex].Solve();
            } 
        }

        private static void Initialize(out int maxIterations, out int numberOfPopulation, out int numberOfDimensions, out List<double> upperBoundaries, out List<double> lowerBoundaries)
        {
            maxIterations = 1000;
            numberOfPopulation = 30;
            numberOfDimensions = 3;
            upperBoundaries = new List<double>();
            lowerBoundaries = new List<double>();
            for (int i = 0; i < numberOfDimensions; i++)
            {
                upperBoundaries.Add(100);
                lowerBoundaries.Add(-100);
            }
        }
    }
}
