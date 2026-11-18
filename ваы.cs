using System;
using System.Collections.Generic;
using System.Linq;

namespace CollegeSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("========================================");
            Console.WriteLine(" ДЕМОНСТРАЦИЯ ЗАДАНИЙ ПО ООП И ДОСТУПУ");
            Console.WriteLine("========================================");

            // Блок 1
            Task1_8.Run();

            // Блок 2
            Task9_15.Run();

            // Блок 3
            Task16_20.Run();

            // Блок 4
            Task21_25.Run();

            // Блок 5 (Финальная сборка — эмуляция веб-приложения / системы)
            Task26_30.Run();
        }
    }

    // ==========================================
    // БЛОК 1. Создание структуры и применение public/private (1-8)
    // ==========================================
    namespace Task1_8
    {
        public class Student
        {
            // 2, 4. Закрытые поля
            private string name;
            private string surname;
            private int age;
            private string group;
            private readonly string studentId; // 8. Только для чтения после создания

            // 5. Конструктор
            public Student(string name, string surname, int age, string group)
            {
                ValidateStudentData(name, surname, age, group); // 6. Вызов валидации

                this.name = name;
                this.surname = surname;
                SetAge(age); // 4. Безопасная установка возраста
                this.group = group;
                this.studentId = "ST-" + Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
            }

            // 6. Приватный метод валидации
            private void ValidateStudentData(string n, string s, int a, string g)
            {
                if (string.IsNullOrWhiteSpace(n) || string.IsNullOrWhiteSpace(s) || string.IsNullOrWhiteSpace(g))
                    throw new ArgumentException("Имя, фамилия и группа не могут быть пустыми.");
            }

            // 3, 4. Свойства и метод SetAge с проверкой (15-70 лет)
            public string Name
            {
                get => name;
                set { if (!string.IsNullOrWhiteSpace(value)) name = value; }
            }

            public string Surname
            {
                get => surname;
                set { if (!string.IsNullOrWhiteSpace(value)) surname = value; }
            }

            public int Age
            {
                get => age;
                set => SetAge(value);
            }

            public string Group
            {
                get => group;
                set => group = value;
            }

            public string StudentId => studentId; // 8. Read-only свойство

            public void SetAge(int newAge)
            {
                if (newAge < 15 || newAge > 70)
                    throw new ArgumentOutOfRangeException("Возраст студента должен быть от 15 до 70 лет.");
                age = newAge;
            }

            // 7. Метод отображения информации
            public string GetStudentInfo()
            {
                return $"ID: {studentId} | Студент: {name} {surname}, Возраст: {age}, Группа: {group}";
            }
        }

        public static void Run()
        {
            Console.WriteLine("\n--- БЛОК 1: Студенты, инкапсуляция, валидация ---");
            Student student = new Student("Иван", "Иванов", 20, "ПИ-21");
            Console.WriteLine(student.GetStudentInfo());
            
            // Проверка защиты возраста
            try
            {
                student.Age = 10; // Вызовет исключение
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка установки возраста (перехвачено): {ex.Message}");
            }
        }
    }

    // ==========================================
    // БЛОК 2. Управление преподавателями и группами (9-15)
    // ==========================================
    namespace Task9_15
    {
        public class Teacher
        {
            private string name;
            private string surname;
            private string subject;
            private int experience;

            public Teacher(string name, string surname, string subject, int experience)
            {
                this.name = name;
                this.surname = surname;
                this.subject = subject;
                UpdateExperience(experience);
            }

            public string Name => name;
            public string Surname => surname;
            public string Subject => subject;
            public int Experience => experience;

            // 10. Изменение стажа через специальный метод
            public void UpdateExperience(int years)
            {
                if (years < 0) throw new ArgumentException("Стаж не может быть отрицательным.");
                experience = years;
            }

            public void PrintInfo() => Console.WriteLine($"Преподаватель: {name} {surname}, Предмет: {subject}, Стаж: {experience} лет");
        }

        public class Group
        {
            public string Name { get; set; }
            public int Course { get; set; }
            private List<Task1_8.Student> students = new List<Task1_8.Student>(); // 11. Закрытый список

            public Group(string name, int course)
            {
                Name = name;
                Course = course;
            }

            // 12. Добавление студента с проверкой уникальности
            public bool AddStudent(Task1_8.Student student)
            {
                if (students.Any(s => s.StudentId == student.StudentId)) return false;
                students.Add(student);
                return true;
            }

            // 13. Безопасное удаление студента
            public bool RemoveStudent(string studentId)
            {
                var student = students.FirstOrDefault(s => s.StudentId == studentId);
                if (student != null)
                {
                    students.Remove(student);
                    return true;
                }
                return false;
            }

            public IReadOnlyCollection<Task1_8.Student> Students => students.AsReadOnly(); // Безопасный доступ для чтения извне
        }

        public class Subject
        {
            private string title;
            private int hours;
            private Teacher teacher; // 14. Закрытые поля

            public Subject(string title, int hours)
            {
                this.title = title;
                this.hours = hours;
            }

            // 15. Назначение преподавателя с проверкой на null
            public void AssignTeacher(Teacher newTeacher)
            {
                teacher = newTeacher ?? throw new ArgumentNullException(nameof(newTeacher), "Преподаватель не может быть null.");
            }

            public void PrintSubjectInfo()
            {
                string tName = teacher != null ? $"{teacher.Name} {teacher.Surname}" : "Не назначен";
                Console.WriteLine($"Предмет: {title}, Часы: {hours}, Преподаватель: {tName}");
            }
        }

        public static void Run()
        {
            Console.WriteLine("\n--- БЛОК 2: Преподаватели, Группы, Предметы ---");
            Teacher teacher = new Teacher("Анна", "Смирнова", "Математика", 5);
            teacher.UpdateExperience(6);
            teacher.PrintInfo();

            Subject math = new Subject("Высшая математика", 72);
            math.AssignTeacher(teacher);
            math.PrintSubjectInfo();
        }
    }

    // ==========================================
    // БЛОК 3. Наследование и protected (16-20)
    // ==========================================
    namespace Task16_20
    {
        public class Person
        {
            // 16. Использование protected для полей базового класса
            protected string name;
            protected string surname;
            protected int age;

            public Person(string name, string surname, int age)
            {
                this.name = name;
                this.surname = surname;
                this.age = age;
            }

            // 19. Защищенный метод отображения базовой информации
            protected void DisplayBasicInfo()
            {
                Console.Write($"Имя: {name} {surname}, Возраст: {age} ");
            }
        }

        // 17. Наследование класса Student от Person
        public class Student : Person
        {
            public string Group { get; set; }

            public Student(string name, string surname, int age, string group) : base(name, surname, age)
            {
                Group = group;
            }

            public void PrintInfo()
            {
                DisplayBasicInfo(); // Доступно благодаря protected
                Console.WriteLine($"| Роль: Студент, Группа: {Group}");
            }
        }

        // 18. Наследование класса Teacher от Person
        public class Teacher : Person
        {
            public string Subject { get; set; }

            public Teacher(string name, string surname, int age, string subject) : base(name, surname, age)
            {
                Subject = subject;
            }

            public void PrintTeacherInfo()
            {
                DisplayBasicInfo(); // Доступно благодаря protected
                Console.WriteLine($"| Роль: Преподаватель, Предмет: {Subject}");
            }
        }

        public static void Run()
        {
            Console.WriteLine("\n--- БЛОК 3: Наследование и protected ---");
            Student s = new Student("Петр", "Петров", 19, "ИС-22");
            s.PrintInfo();

            Teacher t = new Teacher("Ольга", "Васильева", 42, "Физика");
            t.PrintTeacherInfo();
        }
    }

    // ==========================================
    // БЛОК 4. Модификаторы internal и protected internal (21-25)
    // ==========================================
    namespace Task21_25
    {
        // 21. Класс с модификатором internal
        internal class CollegeSettings
        {
            public string CollegeName { get; set; } = "Колледж Информационных Технологий";
            public int AcademicYear { get; set; } = 2026;

            // 22. Внутренние методы изменения настроек
            internal void UpdateSettings(string name, int year)
            {
                CollegeName = name;
                AcademicYear = year;
            }
        }

        public class PersonBase
        {
            protected internal string GetFullName(string first, string last) // 23. protected internal
            {
                return $"{first} {last}";
            }

            private protected virtual void CalculateStatistics() // 25. private protected
            {
                Console.WriteLine("Расчет базовой статистики...");
            }
        }

        public class ReportManager : PersonBase
        {
            protected internal void GenerateReport() // 24.
            {
                Console.WriteLine("Генерация отчета по успеваемости колледжа...");
            }
        }

        public static void Run()
        {
            Console.WriteLine("\n--- БЛОК 4: internal и protected internal ---");
            CollegeSettings settings = new CollegeSettings();
            Console.WriteLine($"Настройки: {settings.CollegeName}, Учебный год: {settings.AcademicYear}");

            ReportManager rm = new ReportManager();
            rm.GenerateReport();
            Console.WriteLine($"Полное имя через protected internal: {rm.GetFullName("Максим", "Иванов")}");
        }
    }

    // ==========================================
    // БЛОК 5. Реализация функциональности сайта / Системы (26-30)
    // ==========================================
    namespace Task26_30
    {
        // 26. Система авторизации
        public class UserAccount
        {
            private string username;
            private string passwordHash;

            public UserAccount(string username, string password)
            {
                this.username = username;
                this.passwordHash = password; // В реальной жизни здесь хэширование
            }

            public string Username => username;

            public bool Login(string inputPassword)
            {
                return this.passwordHash == inputPassword;
            }

            public void Logout()
            {
                Console.WriteLine($"Пользователь {username} вышел из системы.");
            }
        }

        // 27. Роли пользователей
        public abstract class User
        {
            protected string fullName;
            protected string email;
            private string internalNotes; // Конфиденциальные данные

            protected User(string fullName, string email)
            {
                this.fullName = fullName;
                this.email = email;
            }

            public abstract void ShowDashboard();
        }

        public class Admin : User
        {
            public Admin(string name, string email) : base(name, email) { }
            public override void ShowDashboard() => Console.WriteLine($"[Админ-панель] Управление пользователями и настройками. Администратор: {fullName}");
        }

        public class TeacherUser : User
        {
            public TeacherUser(string name, string email) : base(name, email) { }
            public override void ShowDashboard() => Console.WriteLine($"[Кабинет преподавателя] Проставление оценок и посещаемости. Преподаватель: {fullName}");
        }

        public class StudentUser : User
        {
            public StudentUser(string name, string email) : base(name, email) { }
            public override void ShowDashboard() => Console.WriteLine($"[Личный кабинет студента] Просмотр оценок и расписания. Студент: {fullName}");
        }

        // 28. Журнал оценок
        public class GradeBook
        {
            private Dictionary<string, List<int>> studentGrades = new Dictionary<string, List<int>>();

            public void AddGrade(string studentName, int grade)
            {
                if (grade < 2 || grade > 5)
                    throw new ArgumentOutOfRangeException("Оценка должна быть от 2 до 5.");

                if (!studentGrades.ContainsKey(studentName))
                    studentGrades[studentName] = new List<int>();

                studentGrades[studentName].Add(grade);
            }

            public void PrintJournal()
            {
                Console.WriteLine("--- Электронный журнал оценок ---");
                foreach (var pair in studentGrades)
                {
                    Console.WriteLine($"{pair.Key}: {string.Join(", ", pair.Value)}");
                }
            }
        }

        // 29. Система посещаемости
        public class AttendanceManager
        {
            private Dictionary<string, bool> attendanceLog = new Dictionary<string, bool>();

            public void MarkAttendance(string studentName, bool isPresent)
            {
                attendanceLog[studentName] = isPresent;
            }

            public void GetStatistics()
            {
                int total = attendanceLog.Count;
                if (total == 0) { Console.WriteLine("Нет данных о посещаемости."); return; }
                int presentCount = attendanceLog.Values.Count(v => v);
                double percent = (double)presentCount / total * 100;
                Console.WriteLine($"Посещаемость: присутствовало {presentCount} из {total} ({percent:F1}%)");
            }
        }

        // 30. Финальная сборка системы (эмуляция веб-приложения колледжа)
        public static void Run()
        {
            Console.WriteLine("\n--- БЛОК 5: Финальная сборка проекта (Веб-портал Колледжа) ---");

            // 1. Авторизация
            UserAccount account = new UserAccount("admin", "secret123");
            bool isAuth = account.Login("secret123");
            Console.WriteLine($"Страница авторизации: Вход выполнен ({isAuth})");

            // 2. Главная страница колледжа и роли
            Console.WriteLine("\n[Главная страница колледжа]");
            User admin = new Admin("Иван Главный", "admin@college.ru");
            User teacher = new TeacherUser("Елена Петровна", "teacher@college.ru");
            User student = new StudentUser("Алексей Студент", "student@college.ru");

            admin.ShowDashboard();
            teacher.ShowDashboard();
            student.ShowDashboard();

            // 3. Электронный журнал оценок
            Console.WriteLine();
            GradeBook gb = new GradeBook();
            gb.AddGrade("Алексей Студент", 5);
            gb.AddGrade("Алексей Студент", 4);
            gb.PrintJournal();

            // 4. Посещаемость
            Console.WriteLine();
            AttendanceManager am = new AttendanceManager();
            am.MarkAttendance("Алексей Студент", true);
            am.MarkAttendance("Мария Иванова", false);
            am.GetStatistics();

            Console.WriteLine("\nВсе модули проекта успешно интегрированы и функционируют!");
        }
    }
}