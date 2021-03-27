using System;
using System.IO;
using System.Collections.Generic;

namespace TIPnS_Lab_2
{
    class Letters
    {
        public char c;
        public int[] bincode = new int[4];
        public double prob;
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<Letters> letters = new List<Letters>();

            double[] p = { 0.58, 0.42, 0.65, 0.35 };
            double[,] kmi = new double[10, 12];
            double[,] kmo = new double[10, 12];
            double[] PVj = new double[11];

            int height = 10;
            int width = 12;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width-1; j++)
                {
                    kmi[i, j] = 1;
                }
            }

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
                {1, 0, 0, 0 },
                {1, 0, 0, 1 }
            };
            double[] probs = { 0.24415, 0.22843, 0.08849, 0.21624, 0.02974, 0.12506, 0.02401, 0.02572, 0.01304, 0.00512 };

            for (int i = 0; i < 10; i++)
            {
                Letters letter = new Letters();
                letter.c = c[i];
                letter.prob = probs[i];
                for (int j = 0; j < 4; j++)
                {
                    letter.bincode[j] = bincodes[i, j];
                }
                letters.Add(letter);
            }
            double sum = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (j != 10)
                    {
                        if (j != 11)
                        {
                            for (int k = 0; k < 4; k++)
                            {
                                if ((letters[i].bincode[k] == 0) && (letters[j].bincode[k] == 0))
                                {
                                    kmi[i, j] *= p[0];
                                }
                                if ((letters[i].bincode[k] == 1) && (letters[j].bincode[k] == 1))
                                {
                                    kmi[i, j] *= p[2];
                                }
                                if ((letters[i].bincode[k] == 0) && (letters[j].bincode[k] == 1))
                                {
                                    kmi[i, j] *= p[1];
                                }
                                if ((letters[i].bincode[k] == 1) && (letters[j].bincode[k] == 0))
                                {
                                    kmi[i, j] *= p[3];
                                }
                            }
                            sum += kmi[i, j];
                            kmi[i, 11] += kmi[i, j] * Math.Log2(kmi[i, j]);
                        }
                    }
                    else
                    {
                        kmi[i, j] = 1 - sum;
                        sum = 0;
                        kmi[i, 11] += kmi[i, j] * Math.Log2(kmi[i, j]);
                    }
                }
            }
            Console.WriteLine("КМИ:");
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    kmi[i, j] = Math.Round(kmi[i, j], 5);
                    kmi[i, j] = Math.Abs(kmi[i, j]);
                    Console.Write(kmi[i,j] + "\t");
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n\nКМО:");
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (j != 11)
                    {
                        kmo[i, j] = kmi[i, j] * letters[i].prob;
                        kmo[i, 11] += kmo[i, j] * Math.Log2(kmo[i, j]);                                    
                    }
                    kmo[i, j] = Math.Round(kmo[i, j], 5);
                    kmo[i, j] = Math.Abs(kmo[i, j]);
                    Console.Write(kmo[i, j] + "\t");
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n\nP(Vj):");
            for (int i = 0; i < width-1; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    PVj[i] += kmo[j, i];
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
                entropyNoise += probs[i] * kmi[i,11];
            }
            entropyNoise = Math.Round(entropyNoise, 5);
            entropyNoise = Math.Abs(entropyNoise);
            Console.WriteLine("Энтропия шума: H(V|U) = " + entropyNoise);

            double entropyLeak = entropySrc - entropyRcv + entropyNoise;
            Console.WriteLine("Энтропия утечки: H(V|U) = " + entropyLeak);

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
