using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Laboratory_02
{
    public partial class Университет : Form
    {
        public Университет()
        {
            InitializeComponent();
            this.AutoValidate = AutoValidate.Disable;

            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
            monthCalendar1.ShowToday = false;
            radioButton1.Checked = true;
            calculateUniversityBudget();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label4.Text = $"Возраст: {trackBar1.Value}";
        }

        private void monthCalendar1_Validating(object sender, CancelEventArgs e)
        {
            DateTime selectedDate = monthCalendar1.SelectionStart;
            string userAge = new string((label4.Text).Where(char.IsDigit).ToArray());

            if (!string.IsNullOrEmpty(userAge))
            {
                int age = int.Parse(userAge);

                if ((DateTime.Today.Year - selectedDate.Year) == age)
                {
                    e.Cancel = false;
                    errorProvider4.SetError(monthCalendar1, "");
                }
                else
                {
                    e.Cancel = true;
                    errorProvider4.SetError(monthCalendar1, "Ошибка, возраст и дата рождения не согласованы");
                }
            }
            else
            {
                e.Cancel = true;
                errorProvider4.SetError(monthCalendar1, "Проверьте дату");
            }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                errorProvider1.SetError(textBox1, "Это поле должно быть заполнено");
                e.Cancel = true;
            }
            else if (textBox1.Text.Length < 3)
            {
                errorProvider1.SetError(textBox1, "Имя не может быть короче 3 символов");
                e.Cancel = true;
            }
            else if (!textBox1.Text.All(c => char.IsLetter(c)) || textBox1.Text.All(c => char.IsWhiteSpace(c)))
            {
                errorProvider1.SetError(textBox1, "Имя может содержать только буквы");
                e.Cancel = true;
            }
            else if (textBox1.Text.Length > 20)
            {
                errorProvider1.SetError(textBox1, "Имя не может быть длиннее 20 символов");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(textBox1, "");
                e.Cancel = false;
            }
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (textBox2.Text.Length == 0)
            {
                errorProvider2.SetError(textBox2, "Это поле должно быть заполнено");
                e.Cancel = true;
            }
            else if (textBox2.Text.Length < 3)
            {
                errorProvider2.SetError(textBox2, "Фамилия не может быть короче 3 символов");
                e.Cancel = true;
            }
            else if (!textBox2.Text.All(c => char.IsLetter(c)) || textBox2.Text.All(c => char.IsWhiteSpace(c)))
            {
                errorProvider2.SetError(textBox2, "Фамилия может содержать только буквы");
                e.Cancel = true;
            }
            else if (textBox2.Text.Length > 20)
            {
                errorProvider2.SetError(textBox2, "Фамилия не может быть длиннее 20 символов");
                e.Cancel = true;
            }
            else
            {
                errorProvider2.SetError(textBox2, "");
                e.Cancel = false;
            }
        }

        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            if (textBox3.Text.Length == 0)
            {
                errorProvider3.SetError(textBox3, "Это поле должно быть заполнено");
                e.Cancel = true;
            }
            else if (textBox3.Text.Length < 3)
            {
                errorProvider3.SetError(textBox3, "Отчество не может быть короче 3 символов");
                e.Cancel = true;
            }
            else if (!textBox3.Text.All(c => char.IsLetter(c)) || textBox3.Text.All(c => char.IsWhiteSpace(c)))
            {
                errorProvider3.SetError(textBox3, "Отчество может содержать только буквы");
                e.Cancel = true;
            }
            else if (textBox3.Text.Length > 20)
            {
                errorProvider3.SetError(textBox3, "Отчество не может быть длиннее 20 символов");
                e.Cancel = true;
            }
            else
            {
                errorProvider3.SetError(textBox3, "");
                e.Cancel = false;
            }
        }

        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            if (textBox4.Text.Length == 0)
            {
                errorProvider5.SetError(textBox4, "Это поле должно быть заполнено");
                e.Cancel = true;
                return;
            }
            if (double.TryParse(textBox4.Text, out double result))
            {
                if (result > 0 && result <= 10)
                {
                    errorProvider5.SetError(textBox4, "");
                    e.Cancel = false;
                }
                else
                {
                    errorProvider5.SetError(textBox4, "Значение за пределами допустимых");
                    e.Cancel = true;
                }
            }
            else
            {
                errorProvider5.SetError(textBox4, "Неверный формат");
                e.Cancel = true;
            }
        }

        private void textBox6_Validating(object sender, CancelEventArgs e)
        {
            if (textBox6.Text.Length == 0)
            {
                errorProvider6.SetError(textBox6, "Это поле должно быть заполнено");
                e.Cancel = true;
            }
            else if (textBox6.Text.Length < 3 || textBox6.Text.Length > 20)
            {
                errorProvider6.SetError(textBox6, "Длина компании должна быть от 3 до 20 символов");
                e.Cancel = true;
            }
            else
            {
                errorProvider6.SetError(textBox6, "");
                e.Cancel = false;
            }
        }

        private void textBox7_Validating(object sender, CancelEventArgs e)
        {
            if (textBox7.Text.Length == 0)
            {
                errorProvider7.SetError(textBox7, "Это поле должно быть заполнено");
                e.Cancel = true;
            }
            else if (textBox7.Text.Length < 5 || textBox7.Text.Length > 40)
            {
                errorProvider7.SetError(textBox7, "Длина должности должна быть от 5 до 40 символов");
                e.Cancel = true;
            }
            else if (!textBox7.Text.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                errorProvider7.SetError(textBox7, "Должность может содержать только буквы");
                e.Cancel = true;
            }
            else
            {
                errorProvider7.SetError(textBox7, "");
                e.Cancel = false;
            }
        }

        private void textBox8_Validating(object sender, CancelEventArgs e)
        {
            if (textBox8.Text.Length == 0)
            {
                errorProvider8.SetError(textBox8, "Это поле должно быть заполнено");
                e.Cancel = true;
            }
            else if (!textBox8.Text.All(c => char.IsLetter(c) || char.IsWhiteSpace(c) || c == '-'))
            {
                errorProvider8.SetError(textBox8, "Название может содержать только буквы");
                e.Cancel = true;
            }
            else
            {
                errorProvider8.SetError(textBox8, "");
                e.Cancel = false;
            }
        }

        private void textBox5_Validating(object sender, CancelEventArgs e)
        {
            if (textBox5.Text.Length == 0)
            {
                errorProvider9.SetError(textBox5, "Это поле должно быть заполнено");
                e.Cancel = true;
            }
            else if (!textBox5.Text.All(char.IsDigit) || textBox5.Text.Length != 6)
            {
                errorProvider9.SetError(textBox5, "Индекс должен состоять из 6 цифр");
                e.Cancel = true;
            }
            else
            {
                errorProvider9.SetError(textBox5, "");
                e.Cancel = false;
            }
        }

        private void textBox9_Validating(object sender, CancelEventArgs e)
        {
            if (textBox9.Text.Length == 0)
            {
                errorProvider10.SetError(textBox9, "Это поле должно быть заполнено");
                e.Cancel = true;
            }
            else
            {
                errorProvider10.SetError(textBox9, "");
                e.Cancel = false;
            }
        }

        private void textBox10_Validating(object sender, CancelEventArgs e)
        {
            if (textBox10.Text.Length == 0)
            {
                errorProvider11.SetError(textBox10, "Это поле должно быть заполнено");
                e.Cancel = true;
            }
            else if (!textBox10.Text.All(char.IsDigit))
            {
                errorProvider11.SetError(textBox10, "Номер квартиры должен быть числом");
                e.Cancel = true;
            }
            else
            {
                errorProvider11.SetError(textBox10, "");
                e.Cancel = false;
            }
        }

        private string toTitleCase(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }
            input = input.Trim();
            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren())
            {
                Student st = new Student();
                st.FirstName = toTitleCase(textBox1.Text);
                st.LastName = toTitleCase(textBox2.Text);
                st.Surname = toTitleCase(textBox3.Text);
                st.Age = trackBar1.Value;
                st.Speciality = comboBox1.SelectedItem?.ToString() ?? "Не указана";
                st.BirthDate = monthCalendar1.SelectionStart;
                st.Course = (int)numericUpDown1.Value;
                st.Group = (int)numericUpDown2.Value;
                st.AverageGrade = double.Parse(textBox4.Text.Replace('.', ','));
                st.Gender = radioButton1.Checked ? true : false;

                st.HomeAddress.Index = textBox5.Text;
                st.HomeAddress.City = textBox8.Text;
                st.HomeAddress.Street = textBox9.Text;
                st.HomeAddress.House = textBox10.Text;
                st.HomeAddress.Apartment = textBox11.Text;

                st.CurrentJob.Company = textBox6.Text;
                st.CurrentJob.Position = textBox7.Text;
                st.CurrentJob.Experience = (int)numericUpDown3.Value;

                XmlService.SaveOrUpdateXml(st);
                MessageBox.Show("Успешно, все проверы пройдены. Студент записан в бд.");
            }
            else
            {
                MessageBox.Show("Проверьте информацию.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<Student> students = XmlService.GetStudentList();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = students;
            calculateUniversityBudget();
        }
        private void calculateUniversityBudget()
        {
            List<Student> students = XmlService.GetStudentList();
            double total_budget = 0;

            int base_price = 4700;

            foreach (Student student in students) 
            {
                double studentIncome = base_price / student.Course;

                if (student.AverageGrade >= 9.0)
                    studentIncome *= 0.8;
                else if (student.AverageGrade >= 6.0)
                    studentIncome *= 0.9;
                
                total_budget += studentIncome;
            }
            label22.Text = "Оценочный бюджет университета: " + total_budget;
        }
    }
}
