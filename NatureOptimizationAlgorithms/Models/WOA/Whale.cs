using System;
using System.Collections.Generic;
using System.Text;

namespace NatureOptimizationAlgorithms.Tools
{
    public class Whale
    {
        public Whale()
        {

        }

        public Whale(List<double> position, double score)
        {
            this.position = position;
            this.score = score;
        }

        public List<double> position { get; set; }
        /// <summary>
        /// Fintess function
        /// </summary>
        public double score;
    }
}
