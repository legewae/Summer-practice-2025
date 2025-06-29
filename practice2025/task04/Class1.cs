namespace task04
{
    public interface ISpaceship
    {
        void MoveForward();      // Движение вперед
        void Rotate(int angle);  // Поворот на угол (градусы)
        void Fire();             // Выстрел ракетой
        int Speed { get; }       // Скорость корабля
        int FirePower { get; }   // Мощность выстрела
    }
    public class Cruiser: ISpaceship
    {
        void ISpaceship.MoveForward() { Console.WriteLine($"Крейсер двигается со скоростью {Speed}"); }
        void ISpaceship.Rotate(int angle) { Console.WriteLine($"Крейсер повернулся на {angle} градусов");
            Angle += angle;
            Angle = Angle % 360;
        }
        void ISpaceship.Fire() { Console.WriteLine($"Крейсер выстрелил с силой {FirePower}"); }
        public int Speed { get; }
        public int FirePower { get; }
        public int Angle { get; set; }

        public Cruiser()
        {
            Speed = 50;
            FirePower = 100;
        }
    }
    public class Fighter : ISpaceship
    {
        void ISpaceship.MoveForward() { Console.WriteLine($"Истребитель двигается со скоростью {Speed}"); }
        void ISpaceship.Rotate(int angle)
        {
            Console.WriteLine($"Истребитель повернулся на {angle} градусов");
            Angle += angle;
            Angle = Angle % 360;
        }
        void ISpaceship.Fire() { Console.WriteLine($"Истребитель выстрелил с силой {FirePower}"); }
        public int Speed { get; }
        public int FirePower { get; }
        public int Angle { get; set; }

        public Fighter()
        {
            Speed = 100;
            FirePower = 50;
        }
    }
}
