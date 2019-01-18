using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgNum_Projekt3
{
    public class SparseMatrix
    {
        private int[] _rowIndexes;
        private double[] _values;
        private int[] _columnIndexes;

        public int RowCount => _rowIndexes.Length;

        public double[] Values
        {
            get => _values;
            set => _values = value;
        }

        public int[] RowIndexes
        {
            get => _rowIndexes;
            set => _rowIndexes = value;
        }

        public int[] ColumnIndexes
        {
            get => _columnIndexes;
            set => _columnIndexes = value;
        }

        public SparseMatrix(double[] values, int[] rowIndexes, int[] columnIndexes)
        {
            _values = values;
            _rowIndexes = rowIndexes;
            _columnIndexes = columnIndexes;
        }  
    }
}
