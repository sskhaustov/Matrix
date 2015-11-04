//<-------------------------------------------МАТРИЦА В РАЗРЕЖЕННОМ СТРОЧНОМ ФОРМАТЕ----------------------------------------->
//
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Copyright_LOLOLOL
{
    class Matrix
    {
        private int[] values; //<------ ненулевые значения
        private int[] columns; //<----- столбцы ненулевых значений
        private int[] pointers; //<---- указатели на строки
        private int nvc; //<----- число ненулевых элементов и столбцов
        private int np; //<----- число указателей
        public Matrix()
        {
            nvc = 0;
            np = 0;
        }
        private int num_of_elements(int[,] _matr, int _nrow, int _ncol) //определяем число ненулевых элементов
        {
            int count = 0;
            for (int i = 0; i < _nrow; i++)
                for (int j = 0; j < _ncol; j++)
                    if (_matr[i, j] != 0) count++;
            return count;
        }

        //сложение матриц
        public static Matrix operator +(Matrix _m1, Matrix _m2)
        {
            Matrix _res = new Matrix(); //результат
            _res.pointers = new int[Math.Max(_m1.pointers.Length, _m2.pointers.Length)];
            _res.values = new int[_m1.values.Length + _m2.values.Length];
            _res.columns = new int[_m1.values.Length + _m2.values.Length];
            int _max_len = Math.Max(_m1.pointers.Length, _m2.pointers.Length); //максимально возможное число строк в итог. матрице

            int len_point = 0, //длина массива указателей
                len_res = 0; //длина массива значений/столбцов
            int dif1 = 0, //выделение строк в массивах указателей 1 и 2
                dif2 = 0;
            int n1 = 0, //начало строки в операциях записи/сложения/сортировки
                n2 = 0,
                n3 = 0;
            _res.pointers[len_point++] = 0; //первый указатель - ноль
            //всего итераций - число строк
            for (int i = 0; i < _max_len - 1; i++)
            {
                n1 += dif1; //определяем начало строки в массивах значений и столбцов
                n2 += dif2;
                dif1 = (i < _m1.pointers.Length - 1) ? _m1.pointers[i + 1] - _m1.pointers[i] : 0; //определяем конец строки
                dif2 = (i < _m2.pointers.Length - 1) ? _m2.pointers[i + 1] - _m2.pointers[i] : 0;
                //СУТЬ: сначала просто записываем в результат все значения из строки обоих массивов, далее среди них ищутся
                //с одинаковыми столбцами и складываются, а лишиние удаляются путем сдвига. Затем происходит сортировка по
                //возрастанию столбцов. Делается все это построчно
                for (int j = n1; j < n1 + dif1; j++) //<--- запись
                {
                    _res.values[len_res] = _m1.values[j];
                    _res.columns[len_res] = _m1.columns[j];
                    len_res++;
                }
                for (int j = n2; j < n2 + dif2; j++)
                {
                    _res.values[len_res] = _m2.values[j];
                    _res.columns[len_res] = _m2.columns[j];
                    len_res++;
                }

                //<--- сложение
                for (int j = n3; j < len_res - 1; j++)
                    for (int k = j + 1; k < len_res; k++)
                        if (_res.columns[j] == _res.columns[k])
                        {
                            _res.values[j] += _res.values[k];
                            if (k == len_res - 1)
                            {
                                _res.values[k] = 0;
                                _res.columns[k] = 0;
                            }
                            else
                                for (int l = k; l < len_res; l++) //удаление сдвигом (лишнее слагаемое)
                                {
                                    _res.columns[l] = _res.columns[l + 1];
                                    _res.values[l] = _res.values[l + 1];
                                }
                            len_res--;
                            if (_res.values[j] == 0)
                            {
                                for (int l = j; l < len_res; l++) //удаление сдвигом (если рез. сложения - 0)
                                {
                                    _res.columns[l] = _res.columns[l + 1];
                                    _res.values[l] = _res.values[l + 1];
                                }
                                len_res--;
                            }
                        }
                _res.pointers[len_point++] = len_res;
                n3 = len_res; //начало следующей строки - длина текущего массива
            }

            //<--- сортировка
                n1 = 0;
                int swap;
                for (int j = 0; j < len_point - 1; j++)
                {
                    n1 = (j == 0) ? 0 : n1 + dif1;
                    dif1 = _res.pointers[j + 1] - _res.pointers[j];
                    for (int k = n1; k < n1 + dif1; k++)
                        for (int l = n1; l < n1 + dif1 - 1; l++)
                            if (_res.columns[l] > _res.columns[l + 1])
                            {
                                swap = _res.columns[l];
                                _res.columns[l] = _res.columns[l + 1];
                                _res.columns[l + 1] = swap;
                                swap = _res.values[l];
                                _res.values[l] = _res.values[l + 1];
                                _res.values[l + 1] = swap;
                            }
                }
            _res.nvc = len_res;
            _res.np = len_point;
            return _res;
        }
        //"сжатие матрицы" На входе - матрица и ее размеры, на выходе - массивы ненулевых значений, столбцов и указателей
        public void compress_matrix(int[,] _matr, int _nrow, int _ncol)
        {
            nvc = 0;
            np = 0;
            values = new int[num_of_elements(_matr, _nrow, _ncol)];
            columns = new int[num_of_elements(_matr, _nrow, _ncol)];
            pointers = new int[_nrow + 1];
            pointers[np] = 0; //первый указатель всегда ноль
            np++; //число указателей равно числу строк в матрице + 1
            for (int i = 0; i < _nrow; i++)
            {
                int _next_pointer = 0;
                for (int j = 0; j < _ncol; j++)
                    //если элемент ненулевой, то запоминаем его и номер его столбца
                    if (_matr[i,j] != 0)
                    {
                        values[nvc] = _matr[i,j];
                        columns[nvc] = j;
                        nvc++;
                        _next_pointer++;
                    }
                //каждый последующий указатель на начало следующей строки в массиве значений 
                //и столбцов отличен от предыдущего на число ненулевых элементов в строке матрицы
                if (np == 0)
                    pointers[np] += _next_pointer;
                else
                    pointers[np] = pointers[np - 1] + _next_pointer;
                np++;
            }
        }
        //распаковка матрицы
        public void decompress_matrix(int _rows, int _cols, out int[,] _matr)
        {
            _matr = new int[_rows, _cols];
            //сначала заполняем нулями
            for (int i = 0; i < _rows; i++)
                for (int j = 0; j < _cols; j++)
                    _matr[i, j] = 0;
            //теперь заполняем значениями
            int mi = 0; //строка в развернутой матрице
            for (int i = 0; i < np - 1; i++)
            {
                for (int j = pointers[i]; j < pointers[i + 1]; j++)
                    _matr[mi, columns[j]] = values[j];
                mi++;
            }

        }
        //селекторы
        public int[] get_values()
        {
            return values;
        }
        public int[] get_columns()
        {
            return columns;
        }
        public int[] get_pointers()
        {
            return pointers;
        }
        public int get_len()
        {
            return nvc;
        }
        public int get_point_len()
        {
            return np;
        }
    }
}
