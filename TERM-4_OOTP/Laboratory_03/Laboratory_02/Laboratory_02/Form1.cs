using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.ComponentModel.DataAnnotations;

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
            if (comboBox2.Items.Count > 0)
            {
                comboBox2.SelectedIndex = 0;
            }
            textBox12.CausesValidation = false;
            monthCalendar1.ShowToday = false;
            DateTime date_now = DateTime.Now;
            toolStripStatusLabel1.Text = $"Последнее действие: -";
            toolStripStatusLabel2.Text = "Количество объектов: 0";
            toolStripStatusLabel3.Text = $"Текущая дата: {date_now}";
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AllowUserToOrderColumns = false;
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
        private void DisplayValidationErrors(IEnumerable<ValidationResult> results)
        {
            errorProvider1.Clear();

            var controlMap = new Dictionary<string, Control>
    {
        { nameof(Student.Age), trackBar1 },
        { nameof(Student.FirstName), textBox1 },
        { nameof(Student.LastName), textBox2 },
        { nameof(Student.Surname), textBox3 },
        { nameof(Student.AverageGrade), textBox4 },
        { nameof(Address.Index), textBox5 },
        { nameof(Address.City), textBox8 },
        { nameof(Address.Street), textBox9 },
        { nameof(Address.House), textBox10 },
        { nameof(Work.Company), textBox6 },
        { nameof(Work.Position), textBox7 },
        { nameof(Address.Apartment), textBox11 }
    };

            foreach (var error in results)
            {
                foreach (var memberName in error.MemberNames)
                {
                    if (controlMap.ContainsKey(memberName))
                    {
                        errorProvider1.SetError(controlMap[memberName], error.ErrorMessage);
                    }
                }
            }
        }

        private void DisplayValidationFilterErrors(IEnumerable<ValidationResult> results)
        {
            errorProvider2.Clear();

            var controlMap = new Dictionary<string, Control>
    {
                { nameof(Filters.filter), textBox12 }
    };

            foreach (var error in results)
            {
                foreach (var memberName in error.MemberNames)
                {
                    if (controlMap.ContainsKey(memberName))
                    {
                        errorProvider2.SetError(controlMap[memberName], error.ErrorMessage);
                    }
                }
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
            Student st = new Student();
            st.FirstName = toTitleCase(textBox1.Text);
            st.LastName = toTitleCase(textBox2.Text);
            st.Surname = toTitleCase(textBox3.Text);
            st.Age = trackBar1.Value;
            st.Speciality = comboBox1.SelectedItem?.ToString() ?? "Не указана";
            st.BirthDate = monthCalendar1.SelectionStart;
            st.Course = (int)numericUpDown1.Value;
            st.Group = (int)numericUpDown2.Value;
            double.TryParse(textBox4.Text.Replace('.', ','), out double grade);
            st.AverageGrade = grade;
            st.Gender = radioButton1.Checked ? true : false;

            st.HomeAddress.Index = textBox5.Text;
            st.HomeAddress.City = textBox8.Text;
            st.HomeAddress.Street = textBox9.Text;
            st.HomeAddress.House = textBox10.Text;
            st.HomeAddress.Apartment = textBox11.Text;

            st.CurrentJob.Company = textBox6.Text;
            st.CurrentJob.Position = textBox7.Text;
            st.CurrentJob.Experience = (int)numericUpDown3.Value;
            var context = new ValidationContext(st);
            var result = new List<ValidationResult>();
            bool isStudentValid = Validator.TryValidateObject(st, context, result, true);

            var workContext = new ValidationContext(st.CurrentJob);
            bool isWorkValid = Validator.TryValidateObject(st.CurrentJob, workContext, result, true);

            var addressContext = new ValidationContext(st.HomeAddress);
            bool isAddressValid = Validator.TryValidateObject(st.HomeAddress, addressContext, result, true);
            if (!isStudentValid || !isWorkValid || !isAddressValid)
            {
                string errorMessage = "";
                foreach (var error in result)
                {
                    errorMessage += error.ErrorMessage + "\n";
                }
                DisplayValidationErrors(result);
                MessageBox.Show(errorMessage, "Ошибка валидации");
            }
            else
            {
                XmlService.SaveOrUpdateXml(st);
                MessageBox.Show("Успешно, все проверы пройдены. Студент записан в бд.");
            }
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
            label22.Text = "Оценочный бюджет: " + total_budget;
        }

        private void label23_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Версия: 2.3\nРазработчик: Антипов А.Р. 2-9", "О программе");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            List<Filters> all_filters = dataGridView2.DataSource as List<Filters> ?? new List<Filters>();
            Filters new_filter = new Filters();
            new_filter.filter = textBox12.Text;
            new_filter.criterion = comboBox2.SelectedItem?.ToString() ?? "Не указана";
            new_filter.isRegex = checkBox1.Checked;

            var context = new ValidationContext(new_filter);
            var result = new List<ValidationResult>();
            if (Validator.TryValidateObject(new_filter, context, result, true))
            {
                all_filters.Add(new_filter);
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = all_filters;
            }
            else
            {

                string errorMessage = "";
                foreach (var error in result)
                {
                    errorMessage += error.ErrorMessage + "\n";
                }
                DisplayValidationErrors(result);
                MessageBox.Show(errorMessage, "Ошибка валидации");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            List<Student> students = XmlService.GetStudentList();

            List<Filters> activeFilters = dataGridView2.DataSource as List<Filters> ?? new List<Filters>();

            foreach (Filters filter in activeFilters)
            {
                students = students.Where(student =>
                {
                    string valueToCompare = GetStudentValueByCriterion(student, filter.criterion);

                    if (filter.isRegex)
                    {
                        try
                        {
                            return System.Text.RegularExpressions.Regex.IsMatch(valueToCompare, filter.filter, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return valueToCompare.IndexOf(filter.filter, StringComparison.OrdinalIgnoreCase) >= 0;
                    }
                }).ToList();
            }

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = students;

            toolStripStatusLabel2.Text = $"Количество объектов: {dataGridView1.Rows.Count}";

            calculateUniversityBudget();
        }

        private string GetStudentValueByCriterion(Student student, string criterion)
        {
            switch (criterion)
            {
                case "Имя": return student.FirstName;
                case "Фамилия": return student.LastName;
                case "Отчество": return student.Surname;
                case "Возраст": return student.Age.ToString();
                case "Специальность": return student.Speciality;
                case "Дата рождения": return student.BirthDate.ToShortDateString();
                case "Курс": return student.Course.ToString();
                case "Группа": return student.Group.ToString();
                case "Средний балл": return student.AverageGrade.ToString();
                case "Пол": return student.Gender ? "Мужской" : "Женский";
                case "Компания": return student.CurrentJob.Company;
                case "Должность": return student.CurrentJob.Position;
                case "Город": return student.HomeAddress.City;
                case "Индекс": return student.HomeAddress.Index;
                case "Улица": return student.HomeAddress.Street;
                case "Дом": return student.HomeAddress.House;
                case "Квартира": return student.HomeAddress.Apartment;
                default: return "";
            }
        }
        private void DataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (dataGridView2.CurrentRow != null)
                {
                    List<Filters> filters = dataGridView2.DataSource as List<Filters>;

                    if (filters != null)
                    {
                        filters.RemoveAt(dataGridView2.CurrentRow.Index);
                        dataGridView2.DataSource = null;
                        dataGridView2.DataSource = filters;
                    }
                }
            }
        }

        bool isMenuExpanded = false;
        const int maxWidth = 280;
        const int minWidth = 10;
        private void menuTimer_Tick(object sender, EventArgs e)
        {
            if (isMenuExpanded)
            {
                panel1.Width -= 10;
                if (panel1.Width <= minWidth)
                {
                    isMenuExpanded = false;
                    timer1.Stop();
                }
            }
            else
            {
                panel1.Width += 10;
                if (panel1.Width >= maxWidth)
                {
                    isMenuExpanded = true;
                    timer1.Stop();
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Student> students = dataGridView1.DataSource as List<Student>;
            if (students == null || students.Count == 0)
            {
                return;
            }
            string selected = comboBox3.SelectedItem?.ToString();
            switch (selected)
            {
                case "Имя":
                    students = students.OrderBy(s => s.FirstName).ToList();
                    break;
                case "Фамилия":
                    students = students.OrderBy(s => s.LastName).ToList();
                    break;
                case "Отчество":
                    students = students.OrderBy(s => s.Surname).ToList();
                    break;
                case "Возраст":
                    students = students.OrderBy(s => s.Age).ToList();
                    break;
                case "Специальность":
                    students = students.OrderBy(s => s.Speciality).ToList();
                    break;
                case "Курс":
                    students = students.OrderBy(s => s.Course).ToList();
                    break;
                case "Группа":
                    students = students.OrderBy(s => s.Group).ToList();
                    break;
                case "Средний балл":
                    students = students.OrderByDescending(s => s.AverageGrade).ToList();
                    break;
            }
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = students;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            List<Student> students = dataGridView1.DataSource as List<Student> ?? new List<Student>();
            MessageBox.Show("Данные успешно сохранены в filtered_data.json");
            XmlService.SaveJson(students);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            timer1.Start();
            toolStripStatusLabel1.Text = "Последнее действие: search";
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            timer1.Start();
            toolStripStatusLabel1.Text = "Последнее действие: sort";
        }
       
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            toolStripStatusLabel1.Text = "Последнее действие: clear";
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                List<Student> students = dataGridView1.DataSource as List<Student> ?? new List<Student>();

                if (students != null)
                {
                    students.RemoveAt(dataGridView1.CurrentRow.Index);
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = students;
                }
            }
            toolStripStatusLabel1.Text = "Последнее действие: del";
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.Index + 1 != dataGridView1.Rows.Count)
            {
                int nextRowIndex = dataGridView1.CurrentRow.Index + 1;
                dataGridView1.CurrentCell = dataGridView1.Rows[nextRowIndex].Cells[0];
            }

            toolStripStatusLabel1.Text = "Последнее действие: next";
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.Index - 1 >= 0)
            {
                int nextRowIndex = dataGridView1.CurrentRow.Index - 1;
                dataGridView1.CurrentCell = dataGridView1.Rows[nextRowIndex].Cells[0];
            }

            toolStripStatusLabel1.Text = "Последнее действие: prev";
        }
    }
}
