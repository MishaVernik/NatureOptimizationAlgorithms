using System;
using System.Collections.Generic;
using System.Text;

namespace NatureOptimizationAlgorithms.Models
{
    public class Swarm
    {
        public Int32 numberOfParticles { get => particles.Count; }
        public List<Particle> particles { get; set; }

        public List<double> globalBestPosition { get; set; }
        public List<double> globalBestVelocity { get; set; }
        public double globalBestScore { get; set; }
        public Swarm()
        {
            particles = new List<Particle>();

            globalBestPosition = new List<double>();
            globalBestVelocity = new List<double>();
        }
    }
}
