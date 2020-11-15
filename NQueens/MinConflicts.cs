using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NQueens
{
    public class MinConflicts
    {
        private int[] queensAtCols;
        private int[] diagLeftConflicts;
        private int[] diagRightConflicts;
        private int[] rowConflicts;
        private int queensCount;
        private Random rand;

        public MinConflicts(int queensCount)
        {
            this.queensCount = queensCount;
            this.queensAtCols = new int[queensCount];
            this.rowConflicts = new int[queensCount];
            var diagCnt = 2 * queensCount - 1;
            this.diagLeftConflicts = new int[diagCnt];
            this.diagRightConflicts = new int[diagCnt];
            this.rand = new Random();
        }

        public void InitQueens()
        {
            var rowOfFirstQueen = rand.Next(0, queensCount);
            this.MoveQueen(0, rowOfFirstQueen, false);

            for (int i = 1; i < this.queensCount; i++)
            {
                var rows = new List<int>();
                var curMin = int.MaxValue;
                for (int j = 0; j < this.queensCount; j++)
                {
                    var conflictsAtRow = this.GetTotalConflicts(i, j);
                    if (conflictsAtRow < curMin)
                    {
                        curMin = conflictsAtRow;
                        rows = new List<int>() { j };
                    }
                    else if (conflictsAtRow == curMin)
                    {
                        rows.Add(j);
                    }
                }

                var chosenRow = this.rand.Next(0, rows.Count);
                this.MoveQueen(i, rows[chosenRow], false);
            }
        }

        public bool Solve(int stepsCoef)
        {
            for (int i = 0; i < stepsCoef * this.queensCount; i++)
            {
                var maxConflictQueenCol = this.GetMaxConflictsQueenCol();

                if (maxConflictQueenCol == -1)
                {
                    return true;
                }

                var minConflictRow = this.GetMinConflictRowToMove(maxConflictQueenCol);
                this.MoveQueen(maxConflictQueenCol, minConflictRow, true);
            }
            return this.GetMaxConflictsQueenCol() == -1;
        }

        public void PrintState()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < this.queensCount; i++)
            {
                for (int j = 0; j < this.queensCount; j++)
                {
                    if (i == this.queensAtCols[j])
                    {
                        sb.Append("* ");
                    }
                    else
                    {
                        sb.Append("- ");
                    }
                }
                sb.Append(Environment.NewLine);
            }
            Console.WriteLine(sb.ToString());
        }

        private int GetMaxConflictsQueenCol()
        {
            var cols = new List<int>();
            var curMax = 0;
            for (int i = 0; i < this.queensCount; i++)
            {
                var curQueenConflicts = this.GetTotalConflicts(i, this.queensAtCols[i]) - 3;
                if (curQueenConflicts > curMax)
                {
                    curMax = curQueenConflicts;
                    cols = new List<int>() { i };
                }
                else if (curQueenConflicts == curMax)
                {
                    cols.Add(i);
                }
            }

            if (curMax == 0)
            {
                return -1;
            }

            var colIdx = rand.Next(0, cols.Count);
            return cols[colIdx];
        }

        private int GetMinConflictRowToMove(int col)
        {
            var rows = new List<int>();
            var curMin = int.MaxValue;
            for (int i = 0; i < this.queensCount; i++)
            {
                var curRowConflicts = this.GetTotalConflicts(col, i);
                if (curRowConflicts < curMin)
                {
                    curMin = curRowConflicts;
                    rows = new List<int>() { i };
                }
                else if (curRowConflicts == curMin)
                {
                    rows.Add(i);
                }
            }

            var rowIdx = rand.Next(0, rows.Count);
            return rows[rowIdx];
        }

        private void MoveQueen(int atCol, int toRow, bool hasPrev)
        {
            if (hasPrev)
            {
                var oldRow = this.queensAtCols[atCol];
                this.rowConflicts[oldRow]--;
                UpdateDiagConflicts(oldRow, atCol, true);
            }

            this.queensAtCols[atCol] = toRow;
            this.rowConflicts[toRow]++;
            UpdateDiagConflicts(toRow, atCol);
        }

        private int GetTotalConflicts(int col, int row)
        {
            return this.rowConflicts[row] +
                this.diagLeftConflicts[row + col] +
                this.diagRightConflicts[col - row + this.queensCount - 1];
        }

        private void UpdateDiagConflicts(int row, int col, bool decrease = false)
        {
            if (decrease)
            {
                this.diagLeftConflicts[row + col]--;
                this.diagRightConflicts[col - row + this.queensCount - 1]--;
                return;
            }

            this.diagLeftConflicts[row + col]++;
            this.diagRightConflicts[col - row + this.queensCount - 1]++;
        }
    }
}
