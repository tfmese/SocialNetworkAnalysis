using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectYazLab
{
    public class Node
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //koordinatlar
        public float X { get; set; }
        public float Y { get; set; }
        public double Activity { get; set; }
        public double Interaction { get; set; }
        public double ConnectionCount { get; set; } // bağlantı sayısı

        // Constructor 
        public Node(int id, string name, float x, float y)
        {
            Id = id;
            Name = name;
            X = x;
            Y = y;
        }
    }
}
