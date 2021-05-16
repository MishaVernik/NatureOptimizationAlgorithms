using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Text;

namespace NatureOptimizationAlgorithms.Contracts
{
    public abstract class Optimizer : IDynamicMetaObjectProvider
    {
        public delegate Double ObjectiveFunctiomDelegate(List<double> X);
        public virtual void SetTestFunction(ref ObjectiveFunctiomDelegate objectiveFunctiomDelegate) 
        {

        }

        public virtual Double ObjectiveFunction(List<double> X) { return 0; }
        public virtual Double Solve()
        {
            return 0;
        }
        public virtual void Initialize() { }
        public virtual  void Initialize(int maxIterations, int numberOfPopulation, int numberOfDimensions, List<double> upperBoundaries, List<double> lowerBoundaries) { }

        public virtual DynamicMetaObject GetMetaObject(Expression parameter)
        {
            throw new NotImplementedException();
        }
    }
}
