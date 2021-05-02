using System;
using System.Collections.Generic;
using System.Text;

namespace NatureOptimizationAlgorithms.Models.HAGWO
{
    public class HybridWolfWhale
    {
        public HybridWolfWhale()
        {

        }

        public HybridWolfWhale(List<double> position, double score)
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
