using Force.DeepCloner;
using NatureOptimizationAlgorithms.Contracts;
using NatureOptimizationAlgorithms.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace NatureOptimizationAlgorithms.Algorithms
{
    /// <summary>
    /// Whale Optimization Algorithm - WOA
    /// </summary>
    public class WhaleOptimizationAlgorithm : IOptimizer
    {

        public int numberOfWhales { get; set; }
        public int numberOfDimensions { get; set; }
        public int maxIterations { get; private set; }
        public List<double> upperBoundaries { get; set; }
        public List<double> lowerBoundaries { get; set; }

        private List<Whale> whales { get; set; }
        public WhaleOptimizationAlgorithm()
        {

        }

        public WhaleOptimizationAlgorithm(int maxIterations, int numberOfWhales, int numberOfDimensions, List<double> upperBoundaries, List<double> lowerBoundaries)
        {
            this.numberOfWhales = numberOfWhales;
            this.numberOfDimensions = numberOfDimensions;
            this.maxIterations = maxIterations;
            this.upperBoundaries = upperBoundaries;
            this.lowerBoundaries = lowerBoundaries;
        }
        public Double ObjectiveFunction(List<double> X)
        {
            return Tests.Tests.ObjectiveFunction(X);
        }
        public void Initialize(int maxIterations, int numberOfWhales, int numberOfDimensions, List<double> upperBoundaries, List<double> lowerBoundaries)
        {
            this.numberOfWhales = numberOfWhales;
            this.numberOfDimensions = numberOfDimensions;
            this.maxIterations = maxIterations;
            this.upperBoundaries = upperBoundaries;
            this.lowerBoundaries = lowerBoundaries;

            Initialize();
        }
        public void Initialize()
        {
            Random random = new Random();
            whales = new List<Whale>();
            for (int whaleIndex = 0; whaleIndex < numberOfWhales; whaleIndex++)
            {
                List<double> currentWhalePosition = new List<double>();
                for (int dimensionIndex = 0; dimensionIndex < numberOfDimensions; dimensionIndex++)
                {
                    double position = random.NextDouble() * (upperBoundaries[dimensionIndex] - lowerBoundaries[dimensionIndex]) + lowerBoundaries[dimensionIndex];
                    currentWhalePosition.Add(position);
                }

                double objectiveFunctionScore = ObjectiveFunction(currentWhalePosition);

                whales.Add(new Whale(currentWhalePosition, objectiveFunctionScore));
            }
        }

        public void Solve()
        {
            Random random = new Random();

            Random r1 = new Random();
            Random r2 = new Random();
            Random L = new Random();
            Random P = new Random();

            double b = random.NextDouble() * random.Next(1, whales.Count); // logarithmic spiral
            double a = 2;

            int currentIteration = 0;

            Whale bestSearchAgent = whales.MinBy(item => item.score).DeepClone();

            while (currentIteration < maxIterations)
            {
                foreach (var currentWhale in whales)
                {
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
                                double d_i = Math.Sqrt(Math.Pow((C * bestSearchAgent.position[dimensionIndex] - currentWhale.position[dimensionIndex]), 2));

                                D.Add(d_i);
                            }

                            for (int dimensionIndex = 0; dimensionIndex < numberOfDimensions; dimensionIndex++)
                            {
                                // Updates current whale position
                                currentWhale.position[dimensionIndex] = bestSearchAgent.position[dimensionIndex] - A * D[dimensionIndex];
                            }
                            // Eq. 2.1
                        }
                        else if (Math.Abs(A) >= 1)
                        {
                            int randomXSearchAgent = random.Next(0, this.whales.Count);

                            List<double> D = new List<double>();
                            for (int dimensionIndex = 0; dimensionIndex < numberOfDimensions; dimensionIndex++)
                            {
                                double d_i = Math.Sqrt(Math.Pow((C * this.whales[randomXSearchAgent].position[dimensionIndex] - currentWhale.position[dimensionIndex]), 2));

                                D.Add(d_i);
                            }

                            for (int dimensionIndex = 0; dimensionIndex < numberOfDimensions; dimensionIndex++)
                            {
                                // Updates current whale position
                                currentWhale.position[dimensionIndex] = this.whales[randomXSearchAgent].position[dimensionIndex] - A * D[dimensionIndex];
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
                            double d_i = Math.Sqrt(Math.Pow((bestSearchAgent.position[dimensionIndex] - currentWhale.position[dimensionIndex]), 2));

                            D.Add(d_i);
                        }

                        for (int dimensionIndex = 0; dimensionIndex < numberOfDimensions; dimensionIndex++)
                        {
                            // Updates current whale position

                            currentWhale.position[dimensionIndex] = D[dimensionIndex] * Math.Pow(Math.E, b * l) * Math.Cos(2 * Math.PI * l) + bestSearchAgent.position[dimensionIndex];
                        }
                        // E.q. 2.5
                    }

                }

                // Check boundaries
                foreach (var currentWhale in this.whales)
                {
                    for (int dimensionIndex = 0; dimensionIndex < numberOfDimensions; dimensionIndex++)
                    {
                        if (currentWhale.position[dimensionIndex] > upperBoundaries[dimensionIndex])
                        {
                            currentWhale.position[dimensionIndex] = upperBoundaries[dimensionIndex];
                        }
                        else if (currentWhale.position[dimensionIndex] < lowerBoundaries[dimensionIndex])
                        {
                            currentWhale.position[dimensionIndex] = lowerBoundaries[dimensionIndex];
                        }
                    }
                }

                // Calculate fintess function for each agent
                foreach (var currentWhale in this.whales)
                {
                    currentWhale.score = ObjectiveFunction(currentWhale.position);
                }

                var newBestSearchAgent = whales.MinBy(item => item.score).DeepClone();
                if (newBestSearchAgent.score < bestSearchAgent.score)
                {
                    bestSearchAgent = newBestSearchAgent.DeepClone();
                }

                currentIteration++;
                //Console.WriteLine($"Iteration: {currentIteration}");
                //Console.WriteLine($"Whale Score: {bestSearchAgent.score}");
                //Console.WriteLine($"Whale Positions: {String.Join(", ", bestSearchAgent.position.ToArray())}");
            }

            Console.WriteLine($"Whale Score: {bestSearchAgent.score}");
            Console.WriteLine($"Whale Positions: {String.Join(", ", bestSearchAgent.position.ToArray())}");
        }
    }
}
