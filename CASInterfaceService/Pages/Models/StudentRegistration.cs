using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CASInterfaceService.Pages.Models
{
    public class StudentRegistration
    {
        List<Student> studentList;
        static StudentRegistration stdregd = null;
        private StudentRegistration()
        {
            studentList = new List<Student>();
        }
        public static StudentRegistration getInstance()
        {
            if (stdregd == null)
            {
                stdregd = new StudentRegistration();
                return stdregd;
            }
            else
            {
                return stdregd;
            }
        }
        public void Add(Student student)
        {
            studentList.Add(student);
        }
        public String Remove(String registrationNumber)
        {
            for (int i = 0; i < studentList.Count; i++)
            {
                Student stdn = studentList.ElementAt(i);
                if (stdn.RegistrationNumber.Equals(registrationNumber))
                {
                    studentList.RemoveAt(i);//update the new record
                    return "Delete successful";
                }
            }
            return "Delete un-successful";
        }
        public List<Student> getAllStudent()
        {
            return studentList;
        }
        public String UpdateStudent(Student std)
        {
            for (int i = 0; i < studentList.Count; i++)
            {
                Student stdn = studentList.ElementAt(i);
                if (stdn.RegistrationNumber.Equals(std.RegistrationNumber))
                {
                    studentList[i] = std;//update the new record
                    return "Update successful";
                }
            }
            return "Update un-successful";
        }
    }
}
