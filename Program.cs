using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobii.Interaction;

namespace eyetest1
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new Host();
            var gazePointDataStream = host.Streams.CreateGazePointDataStream();
            gazePointDataStream.GazePoint((x, y, time) => Console.WriteLine("X:{0} Y:{1}", x, y));
            Console.ReadKey();
        }
    }
}
