using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using task13;
public class DateTimeConverter: JsonConverter<DateTime>
{
    private string _dateFormat = "yyyy-MM-dd";
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.ParseExact(reader.GetString(),_dateFormat,null);
    }
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_dateFormat));
    }
}

namespace Serialization
{
    internal class Program
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
        static void Main(string[] args)
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = { new DateTimeConverter() }
            };

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

            string json = JsonSerializer.Serialize(student, options);

            Console.WriteLine("Сериализованный студент:");
            Console.WriteLine(json);

            string tempDirectory = Path.Combine(Path.GetTempPath(), "TempFolder");
            Directory.CreateDirectory(tempDirectory);
            string filePath = Path.Combine(tempDirectory, "student.json");

            File.WriteAllText(filePath, json);

            string readJson = File.ReadAllText(filePath);

            Student deserializedStudent = JsonSerializer.Deserialize<Student>(readJson, options);
            bool valid = ValidateStudent(deserializedStudent);
            Console.WriteLine($"Десериализованный класс прошел проверку: {valid}");

            Directory.Delete(tempDirectory, true);
        }
    }
}
