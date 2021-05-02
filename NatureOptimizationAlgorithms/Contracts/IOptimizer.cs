using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace NatureOptimizationAlgorithms.Contracts
{
    public interface IOptimizer : IDynamicMetaObjectProvider
    {
        void Solve();
        void Initialize();
        void Initialize(int maxIterations, int numberOfPopulation, int numberOfDimensions, List<double> upperBoundaries, List<double> lowerBoundaries);
    }
}
