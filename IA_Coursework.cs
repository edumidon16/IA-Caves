using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace IA_CW
{
    class Program
    {
        public static List<int> numbers = new List<int>();
        public static List<int> cordinates = new List<int>();
        public static List<int> conectivity = new List<int>();
        public static List<int> visitCaves = new List<int>();
        public static List<double> visitCavesDistance = new List<double>();
        public static List<int> LastCave = new List<int>();
        public static List<int> possibleCaves = new List<int>();
        public static List<double> possibleCavesDistance = new List<double>();
        public static List<double> finalCave = new List<double>();
        public static  double[,] route;
        public static double[,] matrix ;
        public static int numberOfCaverns;

        static void Main(string[] args)
        {
            ReadCsv();
            DivideList();
            StartRouteMatrix();
            FindDistancesRoute();
            StartRoute();
            NextRoute();
            double lowestDistance = BestRoute();
            Console.WriteLine("" + lowestDistance);
            
        }
        public static void ReadCsv()
        {
            string location = "input1.cav";
            System.IO.StreamReader file = new System.IO.StreamReader(location);
            string line;
            file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                numbers.Add(int.Parse(line));
            } 
        }
        public static void DivideList()
        {
            int b = 0;
            int numberOfCordinates = 0;
            foreach(int n in numbers)
            {
                if(b==0)
                {
                    numberOfCaverns = n;
                    matrix = new double[n, n];
                    numberOfCordinates = numberOfCaverns * 2;
                }
                if(b <= numberOfCordinates)
                {
                    cordinates.Add(n);
                }
                if(b > (numberOfCordinates + 1))
                {
                    conectivity.Add(n);
                }
                b++;
            }
        }
        public static void StartRouteMatrix()
        {
            route = new double[numberOfCaverns, 3];
            for(int i = 0; i < numberOfCaverns; i++)
            {
                //0 - Visit
                //1 - Distance
                //2 - lastCave
                route[i, 0] = 0; 
                route[i, 1]= 0;
                route[i, 2]= 0;
            }
        }
        public static void FindDistancesRoute()
        {
            for(int i = 0; i < conectivity.Count; i++)
            {
                double a= (double)(conectivity[i] / numberOfCaverns);
                int caveY  = (int)(conectivity[i] / numberOfCaverns);
                int caveX = (int)(conectivity[i] - (caveY  * numberOfCaverns));
                if(caveX == 0)
                {
                    caveX = numberOfCaverns;
                }
                if(a % 1 != 0)
                {
                    a++;
                }
                if(conectivity[i] == 1)
                {
                    int cordX = caveX * 2 - 2;
                    int cordY = caveY * 2 - 2;
                    double distanceXY = CalculateDistance(cordinates[cordX], cordinates[cordX+1], cordinates[cordY], cordinates[cordY+1]);
                    matrix[caveX, caveY] = distanceXY; 
                }
                else if(conectivity[i] == 0)
                {
                    matrix[caveX, caveY] = -1;
                }
            }
        }
        public static double CalculateDistance(int xLat, int xLon, int yLat, int yLon)
        {
            double distance = Math.Sqrt((Math.Pow(xLat - xLon, 2) + Math.Pow(yLat - yLon, 2)));
            return distance;
        }
        public static void StartRoute()
        {
            for(int j = 0; j < numberOfCaverns; j++)
            {
                double startPoint = matrix[0, j];
                if(startPoint != -1)
                {
                    int caveVisit = j;
                    possibleCaves.Add(caveVisit);
                    possibleCavesDistance.Add(startPoint);
                    route[(caveVisit - 1), 0] = 1;
                    route[(caveVisit - 1), 1] = startPoint;
                    route[(caveVisit - 1), 2] = 1;
                }
            }   
        }
        public static void NextRoute()
        {
            double shortestDistance = possibleCavesDistance[0];
            int index = 0;
            for(int i = 1; i < possibleCaves.Count; i++)
            {
                if(possibleCavesDistance[i] < shortestDistance)
                {
                    shortestDistance = possibleCavesDistance[i];
                    index = i;
                }
            }
            visitCaves.Add(possibleCaves[index]);
            visitCavesDistance.Add(shortestDistance);
            NextCave(index);
           
        }
        public static void NextCave(int caveIndex)
        {
            int cave = possibleCaves[caveIndex];
            for(int j = 0; j <= numberOfCaverns; j++)
            {
                double distanceBC = matrix[cave, j];
                if(distanceBC != -1)
                {
                    int caveVisit = j;
                    if((caveVisit + 1) == numberOfCaverns)
                    {
                        double newDistance = visitCavesDistance[caveIndex] + distanceBC;
                        finalCave.Add(cave);
                        finalCave.Add(newDistance);
                    }
                    else
                    {
                        visitCaves.Add(caveVisit);
                        double newDistance = visitCavesDistance[caveIndex] + distanceBC;
                        visitCavesDistance.Add(newDistance);
                        route[caveVisit, 0] = 1;
                        route[caveVisit, 1] = distanceBC;
                        route[caveVisit, 2] = cave + 1;
                    }
                }
            }
            NextRoute();
        }
        public static double BestRoute()
        {
            double lowerDistance = finalCave[1];
            for(int i = 3; i <= finalCave.Count; i+=2)
            {
                if(lowerDistance < finalCave[i])
                {
                    lowerDistance = finalCave[i];
                }
            }
            return lowerDistance;
        }

    }
}