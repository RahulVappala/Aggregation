using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregate
{
    static class Program
    {


        static void Main(string[] args)
        {
            Console.WriteLine("Greetings!");

            int numberOfRows;
            int numberOfColumns;

            try
            {

                Console.Write("Enter the number of rows: ");
                numberOfRows = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter the number of columns: ");
                numberOfColumns = Convert.ToInt32(Console.ReadLine());

                int[,] inputArray = new int[numberOfRows, numberOfColumns];
                string inputString = "";
                string[] splitInputString;

                Console.WriteLine();
                Console.WriteLine("To construct the rectangular array, please enter the input row by row.");
                Console.WriteLine("Separate the column values by ','. \nFor e.g. Enter Row 1: 1,2,3,4,5");
                Console.WriteLine();

                //Input Rows and Columns.
                for (int i = 0; i < numberOfRows; i++)
                {
                    inputString = "";
                    Console.Write("Enter Row " + (i + 1) + " : ");
                    inputString = Console.ReadLine();
                    splitInputString = inputString.Split(',');

                    //check if number of columns entered match the number of columns mentioned earlier.
                    if (splitInputString.Length > numberOfColumns)
                    {
                        Console.WriteLine("More than {0} columns were entered. Trailing values will be truncated.", numberOfColumns);
                    }
                    else if (splitInputString.Length < numberOfColumns)
                    {
                        Console.WriteLine("Not enough columns entered. Remaining rows will be skipped.");
                        break;
                    }

                    //Append the current row to the input rectangular array object.
                    for (int j = 0; j < numberOfColumns; j++)
                    {
                        inputArray[i, j] = int.Parse(splitInputString[j]);
                    }
                }

                //Input the list of aggregation functions to apply.
                List<string> aggregateFunctions = new List<string> { };
                string inputAggregateFunctions = "";

                Console.WriteLine();
                Console.Write("Enter the aggregation functions to apply. (E.g. 'Sum,Count,Average'): ");
                inputAggregateFunctions = Console.ReadLine();
                aggregateFunctions = inputAggregateFunctions.ToLower().Split(',').ToList();

                Console.WriteLine();
                Console.WriteLine("Input Array");
                Console.WriteLine("-----------");

                // Display the input rectangular array.
                for (int i = 0; i < numberOfRows; i++)
                {
                    Console.Write("\t\t");
                    for (int j = 0; j < numberOfColumns; j++)
                    {
                        Console.Write(inputArray[i, j] + "\t");
                    }
                    Console.WriteLine();
                }

                // Processing the input
                Console.WriteLine("Output");
                Console.WriteLine("-------");
                aggregateFunctions.ForEach(x =>
                {
                    double[] aggregateResult = ColumnAggregate(inputArray, x);

                    if (aggregateResult != null)
                    {
                        if (x == "count") { Console.Write("COUNT DISTINCT" + "\t"); }
                        else { Console.Write(x.ToUpper() + "\t\t"); }

                        for (int i = 0; i < numberOfColumns; i++)
                        {
                            Console.Write(Math.Round(aggregateResult[i], 2) + "\t");
                        }
                        Console.Write("\n");
                    }
                    else
                    {
                        Console.WriteLine("Could not run aggregation. Please re-run the application with valid inputs.");
                    }
                }
                );
                Console.WriteLine();
                Console.Write("Press any key to exit: ");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Oops! An exception has occurred. Please re-run the application with valid inputs.");
                Console.Write("Press any key to exit: ");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Extension method to compute the aggregation function on columns of a rectangular array, both input by the user.
        /// </summary>
        /// <param name="inputRectangularArray"></param>
        /// <param name="aggregation"></param>
        /// <returns>a double array with the aggregated result for each column. </returns>
        public static double[] ColumnAggregate(this int[,] inputRectangularArray, string aggregation)
        {
            try
            {

                int rows = inputRectangularArray.GetLength(0);
                int cols = inputRectangularArray.GetLength(1);

                switch (aggregation)
                {
                    case "sum":
                        return Enumerable.Range(0, cols)
                            .Select(col => Convert.ToDouble(Enumerable
                                    .Range(0, rows)
                                    .Select(row => inputRectangularArray[row,col])
                                    .Sum()))
                                    //.Aggregate(0.0, (sum, row) => sum + inputRectangularArray[row, col]))
                            .ToArray();
                    case "count":
                        return Enumerable.Range(0, cols)
                            .Select(col => Convert.ToDouble(Enumerable
                                    .Range(0,rows)
                                    .Select(row => inputRectangularArray[row,col])
                                    .Distinct()
                                    .Count()))
                            .ToArray();
                    case "average":
                        return Enumerable.Range(0, cols)
                            .Select(col => Convert.ToDouble(Enumerable
                                    .Range(0, rows)
                                    .Select(row => inputRectangularArray[row, col])
                                    .Average()))
                                    //.Aggregate(0.0, (avg, row) => avg + ((double)inputRectangularArray[row,col] / rows)))
                            .ToArray();
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
