using Force.DeepCloner;
using KingAOP;
using KingAOP.Aspects;
using NatureOptimizationAlgorithms.Algorithms;
using NatureOptimizationAlgorithms.Attributes;
using NatureOptimizationAlgorithms.Contracts;
using NatureOptimizationAlgorithms.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using static NatureOptimizationAlgorithms.Contracts.Optimizer;

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

            DataTable dataTable = new DataTable();
            //input data
            var columns = new[] { "PSO", "MGWO", "GWO", "WOA", "HAGWO", "SOLUTION" };
            var rows = new List<object[]>();



            List<ObjectiveFunctiomDelegate> objectiveFunctiomDelegates = new List<ObjectiveFunctiomDelegate>();
            objectiveFunctiomDelegates.Add(new ObjectiveFunctiomDelegate(Tests.Tests.F1));
            objectiveFunctiomDelegates.Add(new ObjectiveFunctiomDelegate(Tests.Tests.F2));
            objectiveFunctiomDelegates.Add(new ObjectiveFunctiomDelegate(Tests.Tests.F3));
            objectiveFunctiomDelegates.Add(new ObjectiveFunctiomDelegate(Tests.Tests.F4));
            objectiveFunctiomDelegates.Add(new ObjectiveFunctiomDelegate(Tests.Tests.F5));
            objectiveFunctiomDelegates.Add(new ObjectiveFunctiomDelegate(Tests.Tests.F6));
            //objectiveFunctiomDelegates.Add(new ObjectiveFunctiomDelegate(Tests.Tests.F7));
            objectiveFunctiomDelegates.Add(new ObjectiveFunctiomDelegate(Tests.Tests.F8));
            objectiveFunctiomDelegates.Add(new ObjectiveFunctiomDelegate(Tests.Tests.F9));
            objectiveFunctiomDelegates.Add(new ObjectiveFunctiomDelegate(Tests.Tests.F10));
            objectiveFunctiomDelegates.Add(new ObjectiveFunctiomDelegate(Tests.Tests.F11));


            int maxIterations, numberOfPopulation, numberOfDimensions;
            List<double> upperBoundaries, lowerBoundaries;
            Initialize(out maxIterations, out numberOfPopulation, out numberOfDimensions, out upperBoundaries, out lowerBoundaries);
            List<List<List<double>>> solutinos = new List<List<List<double>>>();
            for (int iteration = 0; iteration < 10; iteration++)
            {
                for (int objectiveFunction = 0; objectiveFunction < objectiveFunctiomDelegates.Count; objectiveFunction++)
                {
                    List<Optimizer> optimizers = new List<Optimizer>();
                    optimizers.Add(new ParticleSwarmOptimization());
                    optimizers.Add(new MeanGrayWolfOptimizer());
                    optimizers.Add(new GrayWolfOptimizer());
                    optimizers.Add(new WhaleOptimizationAlgorithm());
                    optimizers.Add(new HybridApproachGWO());


                    for (int optimizerIndex = 0; optimizerIndex < optimizers.Count; optimizerIndex++)
                    {
                        var f = objectiveFunctiomDelegates[objectiveFunction];
                        optimizers[optimizerIndex].SetTestFunction(ref f);
                        optimizers[optimizerIndex].Initialize(maxIterations, numberOfPopulation, numberOfDimensions, upperBoundaries, lowerBoundaries);
                        var sol = optimizers[optimizerIndex].Solve();

                        if (solutinos.Count <= objectiveFunction)
                        {
                            solutinos.Add(new List<List<double>>() { new List<double>() { sol } });
                        }
                        else
                        {
                            if (solutinos[objectiveFunction].Count <= optimizerIndex)
                            {
                                solutinos[objectiveFunction].Add(new List<double>() { sol });
                            }
                            else
                            {
                                solutinos[objectiveFunction][optimizerIndex].Add(sol);
                            }
                        }
                    }
                }
            }
            IEnumerable<IEnumerable<double>> enumerable = solutinos.Select(x => x.Select(d => d.Average()));
            //   rows.Add(enumerable.DeepClone());
            List<object[]> lst = new List<object[]>();

            foreach (var item in enumerable)
            {
                double[] v = item.ToArray();
                object[] ob = new object[item.Count()];
                v.CopyTo(ob, 0);
                lst.Add(ob);
            }

            Print.WriteToExcel(columns, lst);
        }

        private static void Initialize(out int maxIterations, out int numberOfPopulation, out int numberOfDimensions, out List<double> upperBoundaries, out List<double> lowerBoundaries)
        {
            maxIterations = 1000;
            numberOfPopulation = 50;
            numberOfDimensions = 30;
            upperBoundaries = new List<double>();
            lowerBoundaries = new List<double>();
            for (int i = 0; i < numberOfDimensions; i++)
            {
                upperBoundaries.Add(32);
                lowerBoundaries.Add(-32);
            }
        }
    }
}
