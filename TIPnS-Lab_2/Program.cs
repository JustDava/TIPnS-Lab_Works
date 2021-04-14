using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace TIPnS_Lab_2
{

    class Program
    {
        public static int[] generate_binarycode()
        {
            int bite;
            int[] biteCode = new int[4];
            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                bite = random.Next(0, 2);
                biteCode[i] = bite;
            }
            return biteCode;
        }

        public static int GetIndex(int[,] ar1, int[] ar2)
        { 
            for (int i = 0; i < ar1.GetLength(0); i++)
            {
                int[] ar = new int[4];
                for (int j = 0; j < ar1.GetLength(1); j++)
                {
                    ar[j] = ar1[i, j];
                    if (ar.SequenceEqual(ar2))
                    {
                        return i;
                    }
                    
                }
            }
            return 10;                 
        }

        static void Main(string[] args)
        {
            Random random = new Random();

            double[] p = { 0.58, 0.42, 0.65, 0.35 };

            double[,] apost_kmi = new double[10, 12];
            double[,] apost_kmo = new double[10, 12];
            double[] PVj = new double[11];

            int height = 10;
            int width = 12;

            char[] c = { 'e', 'a', 'd', 'i', 'g', 'c', 'f', 'b', 'h', 'j' };
            int[,] bincodes = {
                {1, 0, 1, 0 },
                {0, 0, 0, 1 },
                {0, 0, 1, 0 },
                {0, 0, 1, 1 },
                {0, 1, 0, 0 },
                {0, 1, 0, 1 },
                {0, 1, 1, 0 },
                {0, 1, 1, 1 },
                {1, 1, 0, 0 },
                {1, 0, 0, 1 }
            };
            double[] probs = { 0.24415, 0.22843, 0.08849, 0.21624, 0.02974, 0.12506, 0.02401, 0.02572, 0.01304, 0.00512 };    

            int[] biteCode = new int[4];

            Console.WriteLine("Апостериорная канальная матрица источника:\n");

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 500; j++)
                {
                    biteCode = generate_binarycode();
                    apost_kmi[i, GetIndex(bincodes, biteCode)] += (double)1 / 500;
                }
            }

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (j != 11)
                    {
                        apost_kmi[i, 11] += apost_kmi[i, j] * Math.Log2(apost_kmi[i, j]);
                    }
                    apost_kmi[i, j] = Math.Round(apost_kmi[i, j], 5);
                    apost_kmi[i, j] = Math.Abs(apost_kmi[i, j]);
                    Console.Write(apost_kmi[i, j] + "\t");
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n\nАпостериорная канальная матрица объединения:\n");

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (j != 11)
                    {
                        apost_kmo[i, j] = apost_kmi[i, j] * probs[i];
                        apost_kmo[i, 11] += apost_kmo[i, j] * Math.Log2(apost_kmo[i, j]);
                    }
                    apost_kmo[i, j] = Math.Round(apost_kmo[i, j], 5);
                    apost_kmo[i, j] = Math.Abs(apost_kmo[i, j]);
                    Console.Write(apost_kmo[i, j] + "\t");
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n\nP(Vj):");
            for (int i = 0; i < width - 1; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    PVj[i] += apost_kmo[j, i];
                    PVj[i] = Math.Round(PVj[i], 5);
                }
                Console.Write(PVj[i] + "\t");
            }

            double entropySrc = 0;
            for (int i = 0; i < probs.Length; i++)
            {
                entropySrc += probs[i] * Math.Log2(probs[i]);
            }
            entropySrc = Math.Round(entropySrc, 5);
            entropySrc = Math.Abs(entropySrc);
            Console.WriteLine("\n\nЭнтропия источника: H(U) = " + entropySrc);

            double entropyRcv = 0;
            for (int i = 0; i < PVj.Length; i++)
            {
                entropyRcv += PVj[i] * Math.Log2(PVj[i]);
            }
            entropyRcv = Math.Round(entropyRcv, 5);
            entropyRcv = Math.Abs(entropyRcv);
            Console.WriteLine("Энтропия приемника: H(V) = " + entropyRcv);

            double entropyNoise = 0;
            for (int i = 0; i < probs.Length; i++)
            {
                entropyNoise += probs[i] * apost_kmi[i, 11];
            }
            entropyNoise = Math.Round(entropyNoise, 5);
            entropyNoise = Math.Abs(entropyNoise);
            Console.WriteLine("Энтропия шума: H(V|U) = " + entropyNoise);

            double entropyLeak = entropySrc - entropyRcv + entropyNoise;
            Console.WriteLine("Энтропия утечки: H(V|U) = " + Math.Round(entropyLeak, 5));

            double countofUsefulInfo = entropyRcv - entropyNoise;
            countofUsefulInfo = Math.Round(countofUsefulInfo, 5);
            Console.WriteLine("Количество полезной информации: I(U,V) = " + countofUsefulInfo);

            double sendingSpeed = countofUsefulInfo / 0.0001 / 4;
            sendingSpeed = Math.Round(sendingSpeed, 0);
            Console.WriteLine("Скорость передачи информации: J = " + sendingSpeed);

            Console.ReadLine();
        }
    }
}
