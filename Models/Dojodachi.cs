namespace dojodachi {

    public class Dojodachi{
        public string Name;
        public int Happy;
        public int Full;
        public int Energy;
        public int Meals;

        public Dojodachi(string Nombre){
            Name = Nombre;
            Happy = 20;
            Full = 20;
            Energy = 20;
            Meals = 3;
        }
        public Dojodachi(string Nombre, int Hap, int Ful, int Ene, int Mea){
            Name = Nombre;
            Happy = Hap;
            Full = Ful;
            Energy = Ene;
            Meals = Mea;
        }
              
    }

}