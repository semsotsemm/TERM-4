using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

namespace Laboratory_02
{
    public class XmlService
    {
        public static void SaveOrUpdateXml(Student newStudent)
        {
            string filename = "data.xml";
            List<Student> studentsList;
            XmlSerializer formatter = new XmlSerializer(typeof(List<Student>));

            if (File.Exists(filename))
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    try
                    {
                        studentsList = (List<Student>)formatter.Deserialize(fs);
                    }
                    catch
                    {
                        studentsList = new List<Student>();
                    }
                }
            }
            else
            {
                studentsList = new List<Student>();
            }
            studentsList.Add(newStudent);
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                formatter.Serialize(fs, studentsList);
            }
        }

        public static void SaveJson(List<Student> students)
        {
            string json = JsonSerializer.Serialize(students);
            File.WriteAllText("filtered_data.json", json);
        }
        public static List<Student> GetStudentList()
        {
            string filename = "data.xml";
            List<Student> student_list;
            XmlSerializer formater = new XmlSerializer(typeof(List<Student>));
            if (File.Exists(filename))
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    try
                    {
                        student_list = (List<Student>)formater.Deserialize(fs);
                    }
                    catch
                    {
                        student_list = [];
                    }
                }
            }
            else
            {
                student_list = [];
            }
            return student_list;
        }

    }
}
