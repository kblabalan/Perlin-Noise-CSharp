/*
	Kenneth Perlin's 2002 Improved Noise
	https://mrl.nyu.edu/~perlin/noise/
	
	C# Port by Kim Justin B. Labalan
*/

using System;

namespace Noise
{
    public class Perlin
    {
        private Random random;
        private int[] p;
        public Perlin()
        {
            random = new Random();
            p = new int[512];
            Calculate();
        }
        public Perlin(int seed)
        {
            SetSeed(seed);
            p = new int[512];
            Calculate();
        }

        public void SetSeed(int seed)
        {
            random = new Random(seed);
        }

        public double Noise(double x, double y, double z)
        {
            int X = (int)Math.Floor(x) & 255,
                Y = (int)Math.Floor(y) & 255,
                Z = (int)Math.Floor(z) & 255;
            x -= Math.Floor(x);
            y -= Math.Floor(y);
            z -= Math.Floor(z);
            double u = Fade(x),
                   v = Fade(y),
                   w = Fade(z);
            int A =  p[X] + Y,
                AA = p[A] + Z,
                AB = p[A + 1] + Z,
                B =  p[X + 1] + Y,
                BA = p[B] + Z,
                BB = p[B + 1] + Z;
            return Lerp(w, Lerp(v, Lerp(u, Gradient(p[AA], x, y, z),
                                           Gradient(p[BA], x - 1, y, z)),
                                   Lerp(u, Gradient(p[AB], x, y - 1, z),
                                           Gradient(p[BB], x - 1, y - 1, z))),
                           Lerp(v, Lerp(u, Gradient(p[AA + 1], x , y, z - 1),
                                           Gradient(p[BA + 1], x - 1, y, z - 1)),
                                   Lerp(u, Gradient(p[AB + 1], x , y - 1, z - 1),
                                           Gradient(p[BB + 1], x - 1, y - 1, z - 1))));
        }

        private double Fade(double t)
        {
            return t * t * t * (t * (t * 6d - 15d) + 10d);
        }

        private double Lerp(double t, double a, double b)
        {
            return a + t * (b - a);
        }

        private double Gradient(int hash, double x, double y, double z)
        {
            int h = hash & 15;
            double u = h < 8 ? x : y,
                   v = h < 4 ? y : h == 12 || h == 14 ? x : z;
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }

        private void Calculate()
        {
            for (int count = 0; count < 256; count++)
            {
                p[count] = p[count + 256] = random.Next(0, 256);
            }
        }
    }
}
