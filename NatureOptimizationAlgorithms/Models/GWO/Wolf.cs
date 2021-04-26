using System;
using System.Collections.Generic;
using System.Text;

namespace NatureOptimizationAlgorithms.Tools
{
    /// <summary>
    /// Wolf structure
    /// </summary>
    public class Wolf
    {
        public string name { get; set; }
        public double score { get; set; }
        public List<double> position { get; set; }

        public Wolf()
        {

        }

        public Wolf(string name)
        {
            this.name = name;
        }

        public Wolf(double score, List<double> position)
        {
            this.score = score;
            this.position = position;
        }

        public Wolf(string name, double score, List<double> position)
        {
            this.name = name;
            this.score = score;
            this.position = position;
        }
      
    }
}
