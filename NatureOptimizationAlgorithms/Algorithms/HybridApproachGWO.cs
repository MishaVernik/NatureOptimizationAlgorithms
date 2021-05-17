using Force.DeepCloner;
using KingAOP;
using MoreLinq;
using NatureOptimizationAlgorithms.Attributes;
using NatureOptimizationAlgorithms.Contracts;
using NatureOptimizationAlgorithms.Models.HAGWO;
using NatureOptimizationAlgorithms.Tools;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NatureOptimizationAlgorithms.Algorithms
{
    /// <summary>
    /// Hybrid Approach Gray Wolf Optimizer - HAGWO
    /// Hybrid WOA-MGWO merges the best strengths of both:
    ///     MGWO - exploitation
    ///     WOA  - exploration 
    /// </summary>
    public class HybridApproachGWO : Optimizer
    {
        private Optimizer.ObjectiveFunctiomDelegate objectiveFunctiomDelegate;

        public override void SetTestFunction(ref Optimizer.ObjectiveFunctiomDelegate objectiveFunctiomDelegate)
        {
            this.objectiveFunctiomDelegate = objectiveFunctiomDelegate;
        }

        public int numberOfHybridWolfWhales { get; set; }
        public int numberOfDimensions { get; set; }
        public int maxIterations { get; private set; }
        public List<double> upperBoundaries { get; set; }
        public List<double> lowerBoundaries { get; set; }

        private List<HybridWolfWhale> hybridApproachGWOs { get; set; }

        public List<Wolf> wolves { get; set; }
        public HybridApproachGWO()
        {

        }

        public HybridApproachGWO(int numberOfHybridWolfWhales, int numberOfDimensions, int maxIterations, List<double> upperBoundaries, List<double> lowerBoundaries)
        {
            this.numberOfHybridWolfWhales = numberOfHybridWolfWhales;
            this.numberOfDimensions = numberOfDimensions;
            this.maxIterations = maxIterations;
            this.upperBoundaries = upperBoundaries;
            this.lowerBoundaries = lowerBoundaries;
        }
        private void GenerateWolves()
        {
            wolves = new List<Wolf>();
            wolves.Add(new Wolf("alpha", Double.MaxValue, new List<double>(new double[numberOfDimensions])));
            wolves.Add(new Wolf("beta", Double.MaxValue, new List<double>(new double[numberOfDimensions])));
            wolves.Add(new Wolf("delta", Double.MaxValue, new List<double>(new double[numberOfDimensions])));
        }
        public override Double ObjectiveFunction(List<double> X)
        {
            return objectiveFunctiomDelegate(X);
            return Tests.Tests.ObjectiveFunction(X);
        }
        public override void Initialize()
        {
            Random random = new Random();
            hybridApproachGWOs = new List<HybridWolfWhale>();
            for (int hybridWolfWhalesIndex = 0; hybridWolfWhalesIndex < numberOfHybridWolfWhales; hybridWolfWhalesIndex++)
            {
                List<double> currentWolfWhalesPosition = new List<double>();
                for (int dimensionIndex = 0; dimensionIndex < numberOfDimensions; dimensionIndex++)
                {
                    double position = random.NextDouble() * (upperBoundaries[dimensionIndex] - lowerBoundaries[dimensionIndex]) + lowerBoundaries[dimensionIndex];
                    currentWolfWhalesPosition.Add(position);
                }

                double objectiveFunctionScore = ObjectiveFunction(currentWolfWhalesPosition);

                hybridApproachGWOs.Add(new HybridWolfWhale(currentWolfWhalesPosition, objectiveFunctionScore));
            }
            GenerateWolves();
        }

        [LoggingAspect]
        public override void Initialize(int maxIterations, int numberOfPopulation, int numberOfDimensions, List<double> upperBoundaries, List<double> lowerBoundaries)
        {
            this.numberOfHybridWolfWhales = numberOfPopulation;
            this.numberOfDimensions = numberOfDimensions;
            this.maxIterations = maxIterations;
            this.upperBoundaries = upperBoundaries;
            this.lowerBoundaries = lowerBoundaries;

            Initialize();
        }
        private double GenerateNewPosition(Random random, double a, double l, int wolfWhaleIndex, int dimension, string wolfname, double M)
        {
            double r1 = random.NextDouble();
            double r2 = random.NextDouble();

            double A = 2 * l * r1;
            double C = 2 * r2;
            double alpha_pos = wolves.Find(wolf => wolf.name.Equals(wolfname)).position[dimension];
            double D_alpha = Math.Abs(C * alpha_pos - M);
            double X = alpha_pos - A * D_alpha;

            return X;
        }
        public override Double Solve()
        {
            Random random = new Random();

            Random r1 = new Random();
            Random r2 = new Random();
            Random L = new Random();
            Random P = new Random();

            double b = random.NextDouble() * random.Next(1, hybridApproachGWOs.Count) + 7; // logarithmic spiral
            double a = 2;

            int currentIteration = 0;

            HybridWolfWhale bestSearchAgent = hybridApproachGWOs.MaxBy(item => item.score).FirstOrDefault().DeepClone();

            while (currentIteration < maxIterations)
            {
                // Step 1
                // Calculate Objective Function and Update Alpha,Beta, Delta
                for (int wolfWhaleIndex = 0; wolfWhaleIndex < numberOfHybridWolfWhales; wolfWhaleIndex++)
                {
                    double fitness = ObjectiveFunction(hybridApproachGWOs[wolfWhaleIndex].position);

                    for (int mainWolfIndex = 0; mainWolfIndex < wolves.Count; mainWolfIndex++)
                    {
                        if (mainWolfIndex == 0)
                        {
                            if (fitness > wolves[mainWolfIndex].score)
                            {
                                wolves[mainWolfIndex].position = hybridApproachGWOs[wolfWhaleIndex].position.DeepClone();
                                wolves[mainWolfIndex].score = fitness;
                            }

                        }
                        else
                        {
                            bool canApplynewScore = true;
                            for (int i = 0; i < mainWolfIndex; i++)
                            {
                                if (fitness >= wolves[i].score)
                                {
                                    canApplynewScore = false;
                                    break;
                                }
                            }
                            if (canApplynewScore)
                            {
                                if (fitness > wolves[mainWolfIndex].score)
                                {
                                    wolves[mainWolfIndex].position = hybridApproachGWOs[wolfWhaleIndex].position.DeepClone();
                                    wolves[mainWolfIndex].score = fitness;
                                }
                            }
                        }


                    }
                }

                // Step 2
                // Main Loop
                for (int wolfWhaleIndex = 0; wolfWhaleIndex < hybridApproachGWOs.Count; wolfWhaleIndex++)
                {
                    HybridWolfWhale currentWolfWhale = hybridApproachGWOs[wolfWhaleIndex];
                    a = 2 - currentIteration * ((double)2 / maxIterations);
                    double A = 2 * r1.NextDouble() * a - a;
                    double C = 2 * r2.NextDouble();

                    double l = 2 * L.NextDouble() - 1; // [-1; 1]
                    double p = P.NextDouble();

                    if (p < 0.5)
                    {
                        if (Math.Abs(A) < 1)
                        {
                            List<double> D = new List<double>();
                            for (int dimensionIndex = 0; dimensionIndex < numberOfDimensions; dimensionIndex++)
                            {
                                double d_i = Math.Sqrt(Math.Pow((C * bestSearchAgent.position[dimensionIndex] - currentWolfWhale.position[dimensionIndex]), 2));

                                D.Add(d_i);
                            }

                            for (int dimensionIndex = 0; dimensionIndex < numberOfDimensions; dimensionIndex++)
                            {
                                // Updates current whale position
                                currentWolfWhale.position[dimensionIndex] = bestSearchAgent.position[dimensionIndex] - A * D[dimensionIndex];
                            }
                            // Eq. 2.1
                        }
                        else if (Math.Abs(A) >= 1)
                        {
                            int randomXSearchAgent = random.Next(0, this.hybridApproachGWOs.Count);

                            List<double> D = new List<double>();
                            for (int dimensionIndex = 0; dimensionIndex < numberOfDimensions; dimensionIndex++)
                            {
                                double d_i = Math.Sqrt(Math.Pow((C * this.hybridApproachGWOs[randomXSearchAgent].position[dimensionIndex] - currentWolfWhale.position[dimensionIndex]), 2));

                                D.Add(d_i);
                            }

                            for (int dimensionIndex = 0; dimensionIndex < numberOfDimensions; dimensionIndex++)
                            {
                                // Updates current whale position
                                currentWolfWhale.position[dimensionIndex] = this.hybridApproachGWOs[randomXSearchAgent].position[dimensionIndex] - A * D[dimensionIndex];
                            }
                            // Select a random search Agent 
                            // Eq 2.8
                        }
                    }
                    else if (p >= 0.5)
                    {
                        List<double> D = new List<double>();
                        for (int dimensionIndex = 0; dimensionIndex < numberOfDimensions; dimensionIndex++)
                        {
                            double d_i = Math.Sqrt(Math.Pow((bestSearchAgent.position[dimensionIndex] - currentWolfWhale.position[dimensionIndex]), 2));

                            D.Add(d_i);
                        }
                        List<double> M = new List<double>();
                        for (int dimensionIndex = 0; dimensionIndex < numberOfDimensions; dimensionIndex++)
                        {
                            // Updates current whale position
                            double m_i = D[dimensionIndex] * Math.Pow(Math.E, b * l) * Math.Cos(2 * Math.PI * l) + bestSearchAgent.position[dimensionIndex];

                            M.Add(m_i);
                        }

                        for (int dimensionIndex = 0; dimensionIndex < numberOfDimensions; dimensionIndex++)
                        {
                            double averageXPosition = currentWolfWhale.position.Average();

                            double x1 = GenerateNewPosition(random, a, l, wolfWhaleIndex, dimensionIndex, "alpha", M[dimensionIndex]);
                            double x2 = GenerateNewPosition(random, a, l, wolfWhaleIndex, dimensionIndex, "beta", M[dimensionIndex]);
                            double x3 = GenerateNewPosition(random, a, l, wolfWhaleIndex, dimensionIndex, "delta", M[dimensionIndex]);


                            currentWolfWhale.position[dimensionIndex] = (x1 + x2 + x3) / 3;
                        }
                        // E.q. 2.5
                    }

                }

                // Check boundaries
                foreach (var currentWolfWhale in this.hybridApproachGWOs)
                {
                    for (int dimensionIndex = 0; dimensionIndex < numberOfDimensions; dimensionIndex++)
                    {
                        if (currentWolfWhale.position[dimensionIndex] > upperBoundaries[dimensionIndex])
                        {
                            currentWolfWhale.position[dimensionIndex] = upperBoundaries[dimensionIndex];
                        }
                        else if (currentWolfWhale.position[dimensionIndex] < lowerBoundaries[dimensionIndex])
                        {
                            currentWolfWhale.position[dimensionIndex] = lowerBoundaries[dimensionIndex];
                        }
                    }
                }

                // Calculate fintess function for each agent
                foreach (var currentWolfWhale in this.hybridApproachGWOs)
                {
                    currentWolfWhale.score = ObjectiveFunction(currentWolfWhale.position);
                }

                var newBestSearchAgent = hybridApproachGWOs.MaxBy(item => item.score).FirstOrDefault().DeepClone();
                if (newBestSearchAgent.score > bestSearchAgent.score)
                {
                    bestSearchAgent = newBestSearchAgent.DeepClone();
                }

                currentIteration++;
                //Console.WriteLine($"Iteration: {currentIteration}");
                //Console.WriteLine($"Whale Score: {bestSearchAgent.score}");
                //Console.WriteLine($"Whale Positions: {String.Join(", ", bestSearchAgent.position.ToArray())}");
            }

         //   Console.WriteLine($"WolfWhale Score: {bestSearchAgent.score}");
         //   Console.WriteLine($"WolfWhale Positions: {String.Join(", ", bestSearchAgent.position.ToArray())}");

            return bestSearchAgent.score;
        }

        public override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new AspectWeaver(parameter, this);
        }
    }
}
