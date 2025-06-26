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
        void ISpaceship.MoveForward() { }
        void ISpaceship.Rotate(int angle) { }
        void ISpaceship.Fire() { }
        public int Speed { get; }
        public int FirePower { get; }

        public Cruiser()
        {
            Speed = 50;
            FirePower = 100;
        }
    }
    public class Fighter : ISpaceship
    {
        void ISpaceship.MoveForward() { }
        void ISpaceship.Rotate(int angle) { }
        void ISpaceship.Fire() { }
        public int Speed { get; }
        public int FirePower { get; }

        public Fighter()
        {
            Speed = 100;
            FirePower = 50;
        }
    }
}
