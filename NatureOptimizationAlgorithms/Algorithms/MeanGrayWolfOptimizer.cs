﻿using Force.DeepCloner;
using KingAOP;
using NatureOptimizationAlgorithms.Attributes;
using NatureOptimizationAlgorithms.Contracts;
using NatureOptimizationAlgorithms.Tools;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NatureOptimizationAlgorithms.Algorithms
{
    public class MeanGrayWolfOptimizer : Optimizer
    {
        private Optimizer.ObjectiveFunctiomDelegate objectiveFunctiomDelegate;
        
        public override void SetTestFunction(ref Optimizer.ObjectiveFunctiomDelegate objectiveFunctiomDelegate)
        {
            this.objectiveFunctiomDelegate = objectiveFunctiomDelegate;
        }
        public int maxIterations { get; set; }

        public int numberOfWolves { get; set; }
        public int numberOfDimensions { get; set; }
        public List<double> upperBoundaries { get; set; }
        public List<double> lowerBoundaries { get; set; }
        public List<List<double>> wolfPositions { get; set; }
        public List<Wolf> wolves { get; set; }

        private double a { get; set; }

        private int currentIteration { get; set; }
        public MeanGrayWolfOptimizer()
        {

        }

        public MeanGrayWolfOptimizer(int maxIterations, int numberOfWolves, int numberOfDimensions)
        {
            this.maxIterations = maxIterations;
            this.numberOfWolves = numberOfWolves;
            this.numberOfDimensions = numberOfDimensions;
        }

        public MeanGrayWolfOptimizer(int maxIterations, int numberOfWolves, int numberOfDimensions, List<double> upperBoundaries, List<double> lowerBoundaries)
        {
            this.maxIterations = maxIterations;
            this.numberOfWolves = numberOfWolves;
            this.numberOfDimensions = numberOfDimensions;
            this.upperBoundaries = upperBoundaries;
            this.lowerBoundaries = lowerBoundaries;
        }

        public override Double ObjectiveFunction(List<double> X)
        {
            return objectiveFunctiomDelegate(X);
            return Tests.Tests.ObjectiveFunction(X);
        }

        [LoggingAspect]
        public override void Initialize(int maxIterations, int numberOfWolves, int numberOfDimensions, List<double> upperBoundaries, List<double> lowerBoundaries)
        {
            this.maxIterations = maxIterations;
            this.numberOfWolves = numberOfWolves;
            this.numberOfDimensions = numberOfDimensions;
            this.upperBoundaries = upperBoundaries;
            this.lowerBoundaries = lowerBoundaries;

            Initialize();
        }

        public override void Initialize()
        {
            GenerateWolves();
            GeneratePositions();
        }

        private void GenerateWolves()
        {
            wolves = new List<Wolf>();
            wolves.Add(new Wolf("alpha", Double.MaxValue, new List<double>(new double[numberOfDimensions])));
            wolves.Add(new Wolf("beta", Double.MaxValue, new List<double>(new double[numberOfDimensions])));
            wolves.Add(new Wolf("delta", Double.MaxValue, new List<double>(new double[numberOfDimensions])));
        }
        private void GeneratePositions()
        {
            if (upperBoundaries.Count == 0 || lowerBoundaries.Count == 0)
            {
                throw new Exception("You don't have constraints! Add `em!");
            }

            Random random = new Random();
            wolfPositions = new List<List<double>>();
            for (int wolfIndex = 0; wolfIndex < numberOfWolves; wolfIndex++)
            {
                wolfPositions.Add(new List<double>());
                for (int dimension = 0; dimension < numberOfDimensions; dimension++)
                {
                    double position = random.NextDouble() * (upperBoundaries[dimension] - lowerBoundaries[dimension]) + lowerBoundaries[dimension];

                    wolfPositions[wolfIndex].Add(position);
                }
            }
        }

        public override Double Solve()
        {
            Random random = new Random();
            currentIteration = 0;
            while (currentIteration < maxIterations)
            {
                // Step 1
                // Calculate Objective Function and Update Alpha,Beta, Delta
                for (int wolfIndex = 0; wolfIndex < numberOfWolves; wolfIndex++)
                {
                    double fitness = ObjectiveFunction(wolfPositions[wolfIndex]);

                    for (int mainWolfIndex = 0; mainWolfIndex < wolves.Count; mainWolfIndex++)
                    {
                        if (mainWolfIndex == 0)
                        {
                            if (fitness < wolves[mainWolfIndex].score)
                            {
                                wolves[mainWolfIndex].position = wolfPositions[wolfIndex].DeepClone();
                                wolves[mainWolfIndex].score = fitness;
                            }

                        }
                        else
                        {
                            bool canApplynewScore = true;
                            for (int i = 0; i < mainWolfIndex; i++)
                            {
                                if (fitness <= wolves[i].score)
                                {
                                    canApplynewScore = false;
                                    break;
                                }
                            }
                            if (canApplynewScore)
                            {
                                if (fitness < wolves[mainWolfIndex].score)
                                {
                                    wolves[mainWolfIndex].position = wolfPositions[wolfIndex].DeepClone();
                                    wolves[mainWolfIndex].score = fitness;
                                }
                            }
                        }


                    }
                }

                // Step 2
                // Update th posistion of wolves
                a = 2 - currentIteration * (2 / (double)maxIterations);
                for (int wolfIndex = 0; wolfIndex < numberOfWolves; wolfIndex++)
                {
                    for (int dimension = 0; dimension < numberOfDimensions; dimension++)
                    {
                        double x1 = GenerateNewPosition(random, wolfIndex, dimension, "alpha");
                        double x2 = GenerateNewPosition(random, wolfIndex, dimension, "beta");
                        double x3 = GenerateNewPosition(random, wolfIndex, dimension, "delta");

                        wolfPositions[wolfIndex][dimension] = (x1 + x2 + x3) / 3;
                    }
                }


                // Step 3 
                // Check boundaries
                for (int wolfIndex = 0; wolfIndex < numberOfWolves; wolfIndex++)
                {
                    for (int dimension = 0; dimension < numberOfDimensions; dimension++)
                    {
                        if (wolfPositions[wolfIndex][dimension] > upperBoundaries[dimension])
                        {
                            wolfPositions[wolfIndex][dimension] = upperBoundaries[dimension];
                        }
                        else
                        if (wolfPositions[wolfIndex][dimension] < lowerBoundaries[dimension])
                        {
                            wolfPositions[wolfIndex][dimension] = lowerBoundaries[dimension];
                        }
                    }
                }
                currentIteration = currentIteration + 1;
                //Console.WriteLine($"Iteration: {currentIteration}");
                //Console.WriteLine($"Alpha Score: {wolves.Find(wolf => wolf.name.Equals("alpha")).score}");
                //Console.WriteLine($"Alpha Positions: {String.Join(", ", wolves.Find(wolf => wolf.name.Equals("alpha")).position.ToArray())}");
            }
         //   Console.WriteLine($"Mean Alpha Score: {wolves.Find(wolf => wolf.name.Equals("alpha")).score}");
         //   Console.WriteLine($"Mean Alpha Positions: {String.Join(", ", wolves.Find(wolf => wolf.name.Equals("alpha")).position.ToArray())}");

            return wolves.Find(wolf => wolf.name.Equals("alpha")).score;
        }

        private double GenerateNewPosition(Random random, int wolfIndex, int dimension, string wolfname)
        {
            Random L = new Random();
            double r1 = random.NextDouble();
            double r2 = random.NextDouble();

            double l = (2 * L.NextDouble() - 1);
            double A = 2 * a*  r1 - a;
            double C = 2 * r2;
            double alpha_pos = wolves.Find(wolf => wolf.name.Equals(wolfname)).position[dimension];
            double D_alpha = Math.Abs(C * alpha_pos - wolfPositions[wolfIndex].Average());
            double X = alpha_pos - A * D_alpha;

            return X;
        }
        public override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new AspectWeaver(parameter, this);
        }

      
    }
}
