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
    public partial class Form2 : Form
    {
        int r1, c1, //лишние переменные, нужны лишь для отлова исключений
            r2, c2;
        public Form2()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                r1 = Convert.ToInt32(in_rows1.Text);
                c1 = Convert.ToInt32(in_cols1.Text);
                r2 = Convert.ToInt32(in_rows2.Text);
                c2 = Convert.ToInt32(in_cols2.Text);
                if (r1 <= 0 || c1 <= 0 || r2 <= 0 || c2 <= 0)
                    throw new ArgumentNullException("Размер матрицы");
                this.DialogResult = DialogResult.OK; //изменения приняты
            }
            catch (Exception ex)
            {
                MessageBox.Show("Warning " + ex.GetType().Name + " " + ex.Message);
            }
        }

        //функция, передающая измененныее параметры на основную форму
        public void return_data(out int _r1, out int _c1, out int _r2, out int _c2)
        {
            _r1 = _r2 = _c1 = _c2 = 0;
            _r1 = Convert.ToInt32(in_rows1.Text);
            _c1 = Convert.ToInt32(in_cols1.Text);
            _r2 = Convert.ToInt32(in_rows2.Text);
            _c2 = Convert.ToInt32(in_cols2.Text);
        }
    }
}
