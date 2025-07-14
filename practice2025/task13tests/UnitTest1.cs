using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using task13;

public class DateTimeConverter : JsonConverter<DateTime>
{
    private string _dateFormat = "yyyy-MM-dd";
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.ParseExact(reader.GetString(), _dateFormat, null);
    }
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_dateFormat));
    }
}


namespace task13tests
{
    public class UnitTest1
    {
        public static bool ValidateStudent(Student student)
        {
            if (student == null) return false;
            if (string.IsNullOrEmpty(student.FirstName)) return false;
            if (string.IsNullOrEmpty(student.LastName)) return false;
            if (student.BirthDate > DateTime.Now) return false;
            if (student.Grades.Any(g => g.Grade > 100 || g.Grade < 0)) return false;
            return true;
        }

        private JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new DateTimeConverter() }
        };

        [Fact]
        public void json_SerializesCorrectly()
        {
            Student student = new Student()
            {
                FirstName = "Joe",
                LastName = "Biden",
                BirthDate = new DateTime(1987, 1, 1),
                Grades = new List<Subject>
                {
                    new Subject { Name = "Math", Grade = 33 },
                    new Subject { Name = "English", Grade = 100 }
                }
            };

            string json = JsonSerializer.Serialize(student, _options);
            Assert.Contains("Joe", json);
            Assert.Contains("Biden", json);
        }

        [Fact]
        public void json_DateSerializesCorrectly()
        {
            Student student = new Student()
            {
                FirstName = "Joe",
                LastName = "Biden",
                BirthDate = new DateTime(1987, 1, 1),
                Grades = new List<Subject>
                {
                    new Subject { Name = "Math", Grade = 33 },
                    new Subject { Name = "English", Grade = 100 }
                }
            };

            string json = JsonSerializer.Serialize(student, _options);

            Assert.Contains("1987-01-01", json);
        }

        [Fact]
        public void json_IgnoresNullFields()
        {
            Student student = new Student()
            {
                FirstName = "Joe Banana",
                LastName = null,
                BirthDate = new DateTime(1987, 1, 1),
                Grades = new List<Subject>
                {
                    new Subject { Name = "Math", Grade = 33 },
                    new Subject { Name = "English", Grade = 100 }
                }
            };

            string json = JsonSerializer.Serialize(student, _options);

            Assert.DoesNotContain("LastName", json);
        }

        [Fact]
        public void json_SerializationPassesValidation()
        {
            Student student = new Student()
            {
                FirstName = "Joe",
                LastName = "Biden",
                BirthDate = new DateTime(1987, 1, 1),
                Grades = new List<Subject>
                {
                    new Subject { Name = "Math", Grade = 33 },
                    new Subject { Name = "English", Grade = 100 }
                }
            };

            string json = JsonSerializer.Serialize(student, _options);

            Student student1 = JsonSerializer.Deserialize<Student>(json, _options);

            Assert.NotNull(student1);
            Assert.True(ValidateStudent(student1));
        }

        [Fact]
        public void json_DoesNotPassValidation_Birthday()
        {
            Student student = new Student()
            {
                FirstName = "Joe",
                LastName = "Biden",
                BirthDate = new DateTime(3000, 1, 1),
                Grades = new List<Subject>
                {
                    new Subject { Name = "Math", Grade = 33 },
                    new Subject { Name = "English", Grade = 100 }
                }
            };

            string json = JsonSerializer.Serialize(student, _options);

            Student student1 = JsonSerializer.Deserialize<Student>(json, _options);

            Assert.NotNull(student1);
            Assert.False(ValidateStudent(student1));
        }

        [Fact]
        public void json_DoesNotPassValidation_Grades()
        {
            Student student = new Student()
            {
                FirstName = "Joe",
                LastName = "Biden",
                BirthDate = new DateTime(1987, 1, 1),
                Grades = new List<Subject>
                {
                    new Subject { Name = "Math", Grade = 101 },
                    new Subject { Name = "English", Grade = -1 }
                }
            };

            string json = JsonSerializer.Serialize(student, _options);

            Student student1 = JsonSerializer.Deserialize<Student>(json, _options);

            Assert.NotNull(student1);
            Assert.False(ValidateStudent(student1));
        }

        [Fact]
        public void json_DoesNotPassValidation_FirstName()
        {
            Student student = new Student()
            {
                FirstName = null,
                LastName = "Biden",
                BirthDate = new DateTime(1987, 1, 1),
                Grades = new List<Subject>
                {
                    new Subject { Name = "Math", Grade = 101 },
                    new Subject { Name = "English", Grade = -1 }
                }
            };

            string json = JsonSerializer.Serialize(student, _options);

            Student student1 = JsonSerializer.Deserialize<Student>(json, _options);

            Assert.NotNull(student1);
            Assert.False(ValidateStudent(student1));
        }

        [Fact]
        public void json_DoesNotPassValidation_LastName()
        {
            Student student = new Student()
            {
                FirstName = "Joe",
                LastName = null,
                BirthDate = new DateTime(1987, 1, 1),
                Grades = new List<Subject>
                {
                    new Subject { Name = "Math", Grade = 101 },
                    new Subject { Name = "English", Grade = -1 }
                }
            };

            string json = JsonSerializer.Serialize(student, _options);

            Student student1 = JsonSerializer.Deserialize<Student>(json, _options);

            Assert.NotNull(student1);
            Assert.False(ValidateStudent(student1));
        }
    }
}