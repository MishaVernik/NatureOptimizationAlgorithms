using NatureOptimizationAlgorithms.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace NatureOptimizationAlgorithms.Algorithms
{
    /// <summary>
    /// Hybrid Approach Gray Wolf Optimizer - HAGWO
    /// Hybrid WOA-MGWO merges the best strengths of both:
    ///     MGWO - exploitation
    ///     WOA  - exploration 
    /// </summary>
    public class HybridApproachGWO : IOptimizer
    {
        public HybridApproachGWO()
        {

        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Initialize(int maxIterations, int numberOfPopulation, int numberOfDimensions, List<double> upperBoundaries, List<double> lowerBoundaries)
        {
            throw new NotImplementedException();
        }

        public void Solve()
        {
            throw new NotImplementedException();
        }
    }
}
