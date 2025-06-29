using System;
using System.Linq;

namespace GaussAlgorithm
{
    public class Solver
    {
        public double[] Solve(double[][] matrix, double[] freeMembers) =>
            new LinearEquationSystem(matrix, freeMembers).Solve();
    }

    public class LinearEquationSystem
    {
        private const double Epsilon = 1e-6;
        private readonly double[][] matrix;
        private readonly double[] constants;
        private readonly bool[][] pivotColumns;
        private int processedColumns;
        private int processedRows;

        public int EquationCount => matrix.Length;
        public int VariableCount => matrix.Length > 0 ? matrix[0].Length : 0;

        public LinearEquationSystem(double[][] matrix, double[] constants)
        {
            this.matrix = matrix.Select(row => row.ToArray()).ToArray();
            this.constants = constants.ToArray();
            pivotColumns = matrix.Select(x => new bool[VariableCount]).ToArray();
        }

        public double[] Solve()
        {
            PerformGaussianElimination();
            ValidateSystemConsistency();
            return CalculateSolution();
        }

        private void PerformGaussianElimination()
        {
            while (processedColumns < VariableCount && processedRows < EquationCount)
            {
                var pivotRow = FindPivotRow(processedRows, processedColumns);
                if (pivotRow == -1)
                {
                    processedColumns++;
                    continue;
                }

                SwapRows(processedRows, pivotRow);
                ProcessPivotRow();
                processedRows++;
                processedColumns++;
            }
        }

        private int FindPivotRow(int startRow, int column)
        {
            for (var row = startRow; row < EquationCount; row++)
                if (Math.Abs(matrix[row][column]) > Epsilon)
                    return row;
            return -1;
        }

        private void ProcessPivotRow()
        {
            var currentRow = processedRows;
            var pivot = matrix[currentRow][processedColumns];

            ScaleRow(currentRow, 1.0 / pivot);

            for (var row = 0; row < EquationCount; row++)
            {
                if (row != currentRow && Math.Abs(matrix[row][processedColumns]) > Epsilon)
                    CombineRows(row, currentRow, -matrix[row][processedColumns]);
            }

            pivotColumns[currentRow][processedColumns] = true;
        }

        private void ValidateSystemConsistency()
        {
            foreach (var row in Enumerable.Range(0, EquationCount))
            {
                if (IsZeroRow(row) && !IsZeroConstant(row))
                    throw new NoSolutionException("System has no solution");
            }
        }

        private double[] CalculateSolution()
        {
            var solution = new double[VariableCount];
            var definedVars = new bool[VariableCount];

            foreach (var row in Enumerable.Range(0, EquationCount).Reverse())
            {
                if (IsZeroRow(row)) continue;

                var pivotCol = Array.FindLastIndex(pivotColumns[row], c => c);
                if (pivotCol == -1) continue;

                solution[pivotCol] = CalculateVariableValue(row, pivotCol, solution);
                definedVars[pivotCol] = true;
            }

            return solution;
        }

        private double CalculateVariableValue(int row, int pivotCol, double[] solution) =>
            (constants[row] - DotProduct(matrix[row], solution, pivotCol + 1)) / matrix[row][pivotCol];

        private static double DotProduct(double[] coefficients, double[] solution, int start) =>
            coefficients.Zip(solution, (c, s) => c * s)
                        .Skip(start)
                        .Sum();
        
        private void ScaleRow(int row, double factor)
        {
            matrix[row] = matrix[row].Select(x => x * factor).ToArray();
            constants[row] *= factor;
        }

        private void CombineRows(int targetRow, int sourceRow, double factor)
        {
            for (var col = 0; col < VariableCount; col++)
                matrix[targetRow][col] += factor * matrix[sourceRow][col];

            constants[targetRow] += factor * constants[sourceRow];
        }

        private void SwapRows(int row1, int row2)
        {
            (matrix[row1], matrix[row2]) = (matrix[row2], matrix[row1]);
            (constants[row1], constants[row2]) = (constants[row2], constants[row1]);
            (pivotColumns[row1], pivotColumns[row2]) = (pivotColumns[row2], pivotColumns[row1]);
        }
       
        private bool IsZeroRow(int row) => matrix[row].All(x => Math.Abs(x) < Epsilon);
        private bool IsZeroConstant(int row) => Math.Abs(constants[row]) < Epsilon;
    }
}