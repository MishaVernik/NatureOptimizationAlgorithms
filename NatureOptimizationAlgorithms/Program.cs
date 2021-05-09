using KingAOP;
using KingAOP.Aspects;
using NatureOptimizationAlgorithms.Algorithms;
using NatureOptimizationAlgorithms.Attributes;
using NatureOptimizationAlgorithms.Contracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;

namespace NatureOptimizationAlgorithms
{
    class HelloWorldAspect : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            Console.WriteLine("OnEntry: Hello KingAOP");
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            Console.WriteLine("OnExit: Hello KingAOP");
        }
    }
    class HelloWorld : IDynamicMetaObjectProvider
    {
        [LoggingAspect]
        //[HelloWorldAspect]
        public void HelloWorldCall()
        {
            Console.WriteLine("Hello World");
        }

        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new AspectWeaver(parameter, this);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            int maxIterations, numberOfPopulation, numberOfDimensions;
            List<double> upperBoundaries, lowerBoundaries;
            Initialize(out maxIterations, out numberOfPopulation, out numberOfDimensions, out upperBoundaries, out lowerBoundaries);

            dynamic optimizers = new List<IOptimizer>();
            optimizers.Add(new ParticleSwarmOptimization());
          //  optimizers.Add(new MeanGrayWolfOptimizer());
            optimizers.Add(new GrayWolfOptimizer());
            optimizers.Add(new WhaleOptimizationAlgorithm());
            optimizers.Add(new HybridApproachGWO());


            for (int optimizerIndex = 0; optimizerIndex < optimizers.Count; optimizerIndex++)
            {
                optimizers[optimizerIndex].Initialize(maxIterations, numberOfPopulation, numberOfDimensions, upperBoundaries, lowerBoundaries);
                optimizers[optimizerIndex].Solve();
            }
        }

        private static void Initialize(out int maxIterations, out int numberOfPopulation, out int numberOfDimensions, out List<double> upperBoundaries, out List<double> lowerBoundaries)
        {
            maxIterations = 3000;
            numberOfPopulation = 100;
            numberOfDimensions = 30;
            upperBoundaries = new List<double>();
            lowerBoundaries = new List<double>();
            for (int i = 0; i < numberOfDimensions; i++)
            {
                upperBoundaries.Add(10);
                lowerBoundaries.Add(-10);
            }
        }
    }
}
