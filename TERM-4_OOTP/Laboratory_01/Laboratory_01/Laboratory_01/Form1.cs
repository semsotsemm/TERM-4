using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Laboratory_01
{
    public partial class Form1 : Form
    {
        private readonly Calculator _calculator;

        private readonly Dictionary<string, string[]> _units = new Dictionary<string, string[]>
        {
            { "Длина", new string[] { "Метры", "Сантиметры", "Футы", "Дюймы" } },
            { "Вес", new string[] { "Килограммы", "Граммы", "Фунты", "Унции" } },
            { "Объем", new string[] { "Литры", "Миллилитры", "Галлоны" } }
        };

        public Form1()
        {
            InitializeComponent();
            _calculator = new Calculator();
            _calculator.OnCalculated += ShowResult;

            SetupForm();
        }

        private void SetupForm()
        {
            comboBox2.Items.Clear();
            foreach (var category in _units.Keys)
            {
                comboBox2.Items.Add(category);
            }

            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBox2.SelectedIndex = 0;

            textBox2.ReadOnly = true;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCategory = comboBox2.SelectedItem.ToString();

            if (_units.TryGetValue(selectedCategory, out string[] unitsList))
            {
                comboBox1.Items.Clear();
                comboBox3.Items.Clear();

                comboBox1.Items.AddRange(unitsList);
                comboBox3.Items.AddRange(unitsList);

                comboBox1.SelectedIndex = 0;
                comboBox3.SelectedIndex = 1;
            }

            DoCalculation();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DoCalculation();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            DoCalculation();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DoCalculation();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void DoCalculation()
        {
            if (comboBox1.SelectedItem == null || comboBox3.SelectedItem == null) return;
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox2.Text = "";
                return;
            }

            if (double.TryParse(textBox1.Text.Replace(".", ","), out double value))
            {
                string fromUnit = comboBox1.SelectedItem.ToString();
                string toUnit = comboBox3.SelectedItem.ToString();

                _calculator.Convert(value, fromUnit, toUnit);
            }
            else
            {
                textBox2.Text = "Ошибка ввода";
            }
        }

        private void ShowResult(double result)
        {
            textBox2.Text = result.ToString("0.####");
        }

        private void textBox2_TextChanged(object sender, EventArgs e) { }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}