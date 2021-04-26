using System;
using System.Collections.Generic;
using System.Text;

namespace NatureOptimizationAlgorithms.Models
{
    public class Particle
    {
        public Particle()
        {
            position = new List<double>();
            velocity = new List<double>();


            personalBestPosition = new List<double>();
            personalBestVelocity = new List<double>();
        }

        public Particle(List<double> position, List<double> velocity)
        {
            this.position = position;
            this.velocity = velocity;

            personalBestPosition = new List<double>();
            personalBestVelocity = new List<double>();
        }

        public Particle(List<double> position, List<double> velocity, List<double> personalBestPosition, List<double> personalBestVelocity)
        {
            this.position = position;
            this.velocity = velocity;
            this.personalBestPosition = personalBestPosition;
            this.personalBestVelocity = personalBestVelocity;
        }

        public List<double> position { get; set; }
        public List<double> velocity { get; set; }
        public double score { get; set; }

        public List<double> personalBestPosition { get; set; }
        public List<double> personalBestVelocity { get; set; }
        public double personalBestScore { get; set; }
    }
}
