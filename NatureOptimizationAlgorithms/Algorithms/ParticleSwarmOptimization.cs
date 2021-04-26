using Force.DeepCloner;
using NatureOptimizationAlgorithms.Contracts;
using NatureOptimizationAlgorithms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NatureOptimizationAlgorithms.Algorithms
{
    public class ParticleSwarmOptimization : IOptimizer
    {
        public int maxIterations { get; set; }

        public int numberOfParticles { get; set; }
        public int numberOfDimensions { get; set; }
        public List<double> upperBoundaries { get; set; }
        public List<double> lowerBoundaries { get; set; }
        public Swarm swarm { get; set; }


        public Double ObjectiveFunction(List<double> X)
        {
            return Tests.Tests.ObjectiveFunction(X);
        }
        public ParticleSwarmOptimization()
        {
            
        }

        public ParticleSwarmOptimization(int maxIterations, int numberOfParticles, int numberOfDimensions, List<double> upperBoundaries, List<double> lowerBoundaries)
        {
            this.maxIterations = maxIterations;
            this.numberOfParticles = numberOfParticles;
            this.numberOfDimensions = numberOfDimensions;
            this.upperBoundaries = upperBoundaries;
            this.lowerBoundaries = lowerBoundaries;

            swarm = new Swarm();
        }

        public void Solve()
        {
            Random random = new Random();
            double wMax = 0.9;
            double wMin = 0.2;
            double w = 0;

            double c1 = 2;
            double c2 = 2;

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                for (int particleIndex = 0; particleIndex < swarm.numberOfParticles; particleIndex++)
                {
                    Particle particle = swarm.particles[particleIndex];
                    List<double> currentParticlePosition = particle.position;
                    Double currentParticleScore = ObjectiveFunction(currentParticlePosition);

                    if (currentParticleScore < particle.personalBestScore)
                    {
                        particle.personalBestPosition = currentParticlePosition.DeepClone();
                        particle.personalBestScore = currentParticleScore;

                        if (currentParticleScore < swarm.globalBestScore)
                        {
                            swarm.globalBestPosition = currentParticlePosition.DeepClone();
                            swarm.globalBestScore = currentParticleScore;
                        }
                    }
                }

                w = wMax - iteration * ((double)(wMax - wMin) / maxIterations);
                for (int particleIndex = 0; particleIndex < swarm.numberOfParticles; particleIndex++)
                {
                    for (int dimensionIndex = 0; dimensionIndex < numberOfDimensions; dimensionIndex++)
                    {
                        Particle particle = swarm.particles[particleIndex];
                        particle.velocity[dimensionIndex] = w * particle.velocity[dimensionIndex] + c1 * random.NextDouble() * (particle.personalBestPosition[dimensionIndex] - particle.position[dimensionIndex])
                            + c2 * random.NextDouble() * (swarm.globalBestPosition[dimensionIndex] - particle.position[dimensionIndex]);

                        particle.position[dimensionIndex] = particle.position[dimensionIndex] + particle.velocity[dimensionIndex];
                    }
                }

                for (int particleIndex = 0; particleIndex < swarm.numberOfParticles; particleIndex++)
                {
                    for (int dimensionIndex = 0; dimensionIndex < numberOfDimensions; dimensionIndex++)
                    {
                        if (swarm.particles[particleIndex].position[dimensionIndex] > upperBoundaries[dimensionIndex])
                        {
                            swarm.particles[particleIndex].position[dimensionIndex] = upperBoundaries[dimensionIndex];
                        }
                        else
                        if (swarm.particles[particleIndex].position[dimensionIndex] < lowerBoundaries[dimensionIndex])
                        {
                            swarm.particles[particleIndex].position[dimensionIndex] = lowerBoundaries[dimensionIndex];
                        }
                    }
                }
            }

            Console.WriteLine($"PSO Score: {ObjectiveFunction(swarm.globalBestPosition)}");
            Console.WriteLine($"PSO Positions: {String.Join(", ", swarm.globalBestPosition.ToArray())}");
        }
        public void Initialize(int maxIterations, int numberOfParticles, int numberOfDimensions, List<double> upperBoundaries, List<double> lowerBoundaries)
        {
            this.maxIterations = maxIterations;
            this.numberOfParticles = numberOfParticles;
            this.numberOfDimensions = numberOfDimensions;
            this.upperBoundaries = upperBoundaries;
            this.lowerBoundaries = lowerBoundaries;

            swarm = new Swarm();
            Initialize();
        }
        public void Initialize()
        {
            Random random = new Random();
           
            for (int particleIndex = 0; particleIndex < numberOfParticles; particleIndex++)
            {
                Particle particle = new Particle();
                List<double> currentParticlePosition = new List<double>();
                for (int dimensionIndex = 0; dimensionIndex < numberOfDimensions; dimensionIndex++)
                {
                    double position = random.NextDouble() * (upperBoundaries[dimensionIndex] - lowerBoundaries[dimensionIndex]) + lowerBoundaries[dimensionIndex];
                    currentParticlePosition.Add(position);
                }
                particle.position = currentParticlePosition;
                particle.velocity = new List<double>(new double[numberOfDimensions]);

                particle.personalBestPosition = new List<double>(new double[numberOfDimensions]);
                particle.personalBestVelocity = new List<double>(new double[numberOfDimensions]);
      
                particle.score = Globals.Globals.INFINITY;
                particle.personalBestScore = Globals.Globals.INFINITY;

                swarm.particles.Add(particle);
            }
            swarm.globalBestPosition = new List<double>(new double[numberOfDimensions]);
            swarm.globalBestVelocity = new List<double>(new double[numberOfDimensions]);
            swarm.globalBestScore = Globals.Globals.INFINITY;
        }
    }
}
