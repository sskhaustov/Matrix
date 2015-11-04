using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Copyright_LOLOLOL
{
    public partial class Form1 : Form
    {
        Matrix m1, //разреженные матрицы
               m2,
               m3;
        int[,] mat1 = null, //матрицы
               mat2 = null;
        int r1, c1, //число строк и столбцов для каждого массива
            r2, c2;
        Random rnd;
        DataTable dt1 = null, //данные для таблицы
                  dt2 = null,
                  dt3 = null;
        public Form1()
        {
            InitializeComponent();
            m1 = new Matrix();
            m2 = new Matrix();
            rnd = new Random();
            dt1 = new DataTable();
            dt2 = new DataTable();
            dt3 = new DataTable();
            matr1.DataSource = dt1;
            matr2.DataSource = dt2;
            matr3.DataSource = dt3;
            r1 = r2 = c1 = c2 = 0;
        }
        //изменение размеров матриц
        private void SetMatrixSize(DataTable _dt, DataGridView _dgv, int _rows, int _cols)
        {
            // добавляем столбцы
            int i, j;
            if (_cols > _dt.Columns.Count)       // добавляем недостающие столбцы
                for (i = _dt.Columns.Count; i < _cols; i++)
                {
                    // добавляем в DataTable столбец и задаём формат данных в нём
                    _dt.Columns.Add(new DataColumn(/*"Column 1"*/(i + 1).ToString(), typeof(int)));
                    _dgv.Columns[i].Width = 35;  // задание ширины для отображения (поэтому DataGridView)
                    // DataTable только хранит данные, а ширина столбца - на уровне отображения

                    // Также здесь нужно проверить случай добавления столбцов при неизменном
                    // количестве строк, чтобы новые столбцы заполнить нулями
                    for (j = 0; j < _dt.Rows.Count; j++)
                        _dt.Rows[j][i] = 0;
                }
            else        // удаляем лишние столбцы
                for (i = _dt.Columns.Count - 1; i >= _cols; i--) // обход с конца
                    // т.к. при удалении столбца следующий сместится на его место, Columns - список
                    _dt.Columns.RemoveAt(i);

            // добавляем строки
            if (_rows > _dt.Rows.Count)       // добавляем недостающие строки
                for (i = _dt.Rows.Count; i < _rows; i++)
                {
                    //dt.Rows.Add(new DataRow());   // неизвестен формат, в котором должна быть строка
                    _dt.Rows.Add(_dt.NewRow());     // строка в формате имеющейся таблицы (dt)
                    for (j = 0; j < _cols; j++)
                        _dt.Rows[i][j] = 0;
                }
            else
                for (i = _dt.Rows.Count - 1; i >= _rows; i--) // обход с конца
                    // т.к. при удалении строки следующая сместится на её место, Rows - список
                    _dt.Rows.RemoveAt(i);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (r1 == 0)
            {
                MessageBox.Show("Сначала задайте параметры матрицы!");
            }
            else
            {
                //задаем исходные матрицы
                SetMatrixSize(dt1, matr1, r1, c1);
                SetMatrixSize(dt2, matr2, r2, c2);
                set_random_matrix(dt1, out mat1, r1, c1);
                set_random_matrix(dt2, out mat2, r2, c2);
                m1.compress_matrix(mat1, r1, c1);
                m2.compress_matrix(mat2, r2, c2);
                print_matrix();
            }
        }
        //формирование исходной матрицы и ее отображение на форме
        private void set_random_matrix(DataTable _dt, out int[,] _matr, int _r, int _c)
        {
            _matr = new int[_r,_c];
            for (int i = 0; i < _r; i++)
                for (int j = 0; j < _c; j++)
                {
                    _matr[i, j] = rnd.Next(100)+1; //случайные значения от 1 до 100
                    _dt.Rows[i][j] = _matr[i, j];
                }
            //принудительное добавление нулей в матрицу
            for (int n = 0; n < (_r * _c) / 2; n++) //число нулей - не более половины числа элементов матрицы
            {
                //случайным образом выбираем элемент, и если он не равен 0, то обнуляем его
                int i = rnd.Next(_r);
                int j = rnd.Next(_c);
                if (_matr[i, j] == 0) continue;
                else
                {
                    _matr[i, j] = 0;
                    _dt.Rows[i][j] = _matr[i, j];
                }
            }
        }

        private void print_matrix()
        {
            compr1.Clear();
            compr2.Clear();
            for (int i = 0; i < m1.get_values().Length; i++)
                compr1.AppendText(Convert.ToString(m1.get_values()[i] + " "));
            compr1.AppendText("\n");
            for (int i = 0; i < m1.get_columns().Length; i++)
                compr1.AppendText(Convert.ToString(m1.get_columns()[i] + " "));
            compr1.AppendText("\n");
            for (int i = 0; i < m1.get_pointers().Length; i++)
                compr1.AppendText(Convert.ToString(m1.get_pointers()[i] + " "));
            for (int i = 0; i < m2.get_values().Length; i++)
                compr2.AppendText(Convert.ToString(m2.get_values()[i] + " "));
            compr2.AppendText("\n");
            for (int i = 0; i < m2.get_columns().Length; i++)
                compr2.AppendText(Convert.ToString(m2.get_columns()[i] + " "));
            compr2.AppendText("\n");
            for (int i = 0; i < m2.get_pointers().Length; i++)
                compr2.AppendText(Convert.ToString(m2.get_pointers()[i] + " "));
        }

        private void print_matrix3()
        {
            compr3.Clear();
            for (int i = 0; i < m3.get_len(); i++)
                compr3.AppendText(Convert.ToString(m3.get_values()[i] + " "));
            compr3.AppendText("\n");
            for (int i = 0; i < m3.get_len(); i++)
                compr3.AppendText(Convert.ToString(m3.get_columns()[i] + " "));
            compr3.AppendText("\n");
            for (int i = 0; i < m3.get_point_len(); i++)
                compr3.AppendText(Convert.ToString(m3.get_pointers()[i] + " "));
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Parent.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 w = new Form2();
            w.ShowDialog();
            if (w.DialogResult == DialogResult.OK) //если нажата кнопка ОК
            {
                w.return_data(out r1, out c1, out r2, out c2); //данные о матрицах передаются со специальной формы
                SetMatrixSize(dt1, matr1, r1, c1);
                SetMatrixSize(dt2, matr2, r2, c2);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int _newr, _newc; //число строк и столбцов в новой матрице
            int [,] _ans; //новая матрица
            //в рез. матрице числа строк и столбцов будут наибольшими из чисел слагаемых матриц
            _newr = Math.Max(dt1.Rows.Count, dt2.Rows.Count);
            _newc = Math.Max(dt1.Columns.Count, dt2.Columns.Count);
            m3 = new Matrix();
            m3 = m1 + m2;
            SetMatrixSize(dt3, matr3, _newr, _newc);
            m3.decompress_matrix(_newr, _newc, out _ans);
            for (int i = 0; i < _newr; i++)
                for (int j = 0; j < _newc; j++)
                    dt3.Rows[i][j] = _ans[i, j];
            print_matrix3();
        }

    }
}
