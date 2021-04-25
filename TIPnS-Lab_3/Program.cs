using System;

namespace TIPnS_Lab_3
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] G_matrix = new int[4, 11] {{1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1},
                                              {0, 1, 0, 0, 0, 1, 0, 1, 1, 1, 0},
                                              {0, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0},
                                              {0, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1}};

            int[,] H_matrix = new int[7, 11] {{1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0},
                                              {1, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0},
                                              {1, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0}, 
                                              {1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0}, 
                                              {0, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0}, 
                                              {0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0}, 
                                              {1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1}};

            int[] m = new int[4];

            int[] U = new int[11];

            int[,] errors = new int[11, 11] {{1,0,0,0,0,0,0,0,0,0,0},
                                             {0,1,0,0,0,0,0,0,0,0,0},
                                             {0,0,1,0,0,0,0,0,0,0,0},
                                             {0,0,0,1,0,0,0,0,0,0,0},
                                             {0,0,0,0,1,0,0,0,0,0,0},
                                             {0,0,0,0,0,1,0,0,0,0,0},
                                             {0,0,0,0,0,0,1,0,0,0,0},
                                             {0,0,0,0,0,0,0,1,0,0,0},
                                             {0,0,0,0,0,0,0,0,1,0,0},
                                             {0,0,0,0,0,0,0,0,0,1,0},
                                             {0,0,0,0,0,0,0,0,0,0,1}};

            int[,] code_U = new int[11, 11];

            int[,] syndromes = new int[7, 11];

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

            int[,] code_Matrix = new int[10, 11];

            Console.WriteLine("Введите m (через пробел):");
            string[] str = Console.ReadLine().Split(" ");

            for (int i = 0; i < str.Length; i++)
            {
                m[i] = Int32.Parse(str[i]);
            }

            int sum;
            Console.WriteLine("\nU:");
            for (int i = 0; i < 11; i++)
            {
                sum = 0;
                for (int j = 0; j < 4; j++)
                {
                    sum += (G_matrix[j, i] * m[j]);
                }
                U[i] = sum % 2;
            }

            for (int i = 0; i < 11; i++)
            {
                Console.Write(U[i] + " ");
            }

            Console.WriteLine("\n\nU':");

            for (int i = 0; i < 11; i++)
            {
                sum = 0;
                for (int j = 0; j < 11; j++)
                {
                   code_U[j,i] = (U[j] + errors[i, j]) % 2;
                }
            }

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    Console.Write(code_U[j,i] + " "); 
                }
                Console.WriteLine();
            }

            Console.WriteLine("\nСиндром:");

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    sum = 0;
                    for (int k = 0; k < 11; k++)
                    {
                        sum += (H_matrix[i, k] * code_U[k, j]) % 2;
                    }
                    syndromes[i,j] = sum % 2;
                }                
            }

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    Console.Write(syndromes[i,j] + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("\nКодирование");

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    sum = 0;
                    for (int k = 0; k < 4; k++)
                    {
                        sum += (bincodes[i, k] * G_matrix[k, j]) % 2;
                    }
                    code_Matrix[i, j] = sum % 2;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    Console.Write(code_Matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
