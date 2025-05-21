//Diego Lopez 1187525, Fernando Lopez 1302925
Console.WriteLine("Bienvenido Jugador");

//Declaramos variables que toman valor al iniciar el juego
//Las variables de Ajuste son para que no se muestren valores negativos en las stats
int energia, comida, agua, probabilidad, comidaAjuste, botellas = 1, energiaAjuste;

//usamos la clase Random de C# para asignar un numero aleatorio a las variables
Random aleatorio = new Random();
energia = aleatorio.Next(60, 75);
comida = aleatorio.Next(25, 30);
agua = aleatorio.Next(20, 30);



//Mostramos los valores iniciales de las variables
Console.WriteLine("\nSu Energia incial es: " + energia);
Console.WriteLine("Su Comida inicial es: " + comida); 
Console.WriteLine("Su Agua inicial es: " + agua);

Console.WriteLine("\nIntente sobrevivir los 10 dias, buena suerte!");

//Ciclo for que simula los 10 dias del juego
//Para que el codigo sea mas compacto empleamos un do while para que el jugador pueda seguir jugando hasta que pierda o gane
for(int i =1; i <= 10; i++){
    do
    {
        //Mostramos el dia en el que se encuentra el jugador
        Console.WriteLine("\nDia " + i);
        Console.WriteLine("¿Que desea hacer hoy?");

        //Variable para seleccionar que hacer en el dia
        int opcion = 0;
        //Asumimos un valor, en este caso 10, para tener los porcentajes de probabilidad de la comida
        probabilidad = aleatorio.Next(1, 10);

        Console.WriteLine("\n1. Buscar comida \n2. Buscar agua \n3. Descansar \n4. Explorar la isla");
        opcion = Convert.ToInt32(Console.ReadLine());
        //Leemos la opcion seleccionada por el jugador
        switch (opcion){
            case 1:
                energia -= aleatorio.Next(5, 15);
                //Math.Max es una funcion que nos permite obtener el valor maximo entre dos numeros
                energiaAjuste = Math.Max(energia,0);
                Console.WriteLine("\nBuscaste comida, tu energia es: " + energiaAjuste);
                
                //Asumimos un valor, en este caso 10 como el 100%, para tener los porcentajes de probabilidad de la comida
                if(probabilidad <= 3){
                    Console.WriteLine("Encontraste peces! +30 unidades de comida");
                    comida += 30;
                    Console.WriteLine("\nTu comida es: " + comida);
                }else if(probabilidad >= 4 && probabilidad <= 8){
                    Console.WriteLine("Encontraste frutas! +25 unidades de comida");
                    comida += 25;
                    Console.WriteLine("\nTu comida es: " + comida);
                }else{
                    Console.WriteLine("Encontraste semillas! +10 unidades de comida");
                    comida += 10;
                    Console.WriteLine("\nTu comida es: " + comida);
                }
                break;

                case 2:
                energia -= aleatorio.Next(10, 20);
                energiaAjuste = Math.Max(energia,0);
                Console.WriteLine("\nBuscaste agua, tu energia es: " + energiaAjuste);

                if(probabilidad <= 8){
                    Console.WriteLine("Encontraste agua potable! +20 puntos de agua");
                    agua += 20;
                    Console.WriteLine("\nTu agua es: " + agua);
                }else{
                    Console.WriteLine("Oh no! Es agua contaminada! -10 unidades de energia");
                    energia -= 10;
                    Console.WriteLine("\nTu energia es:" + energia);
                    Console.WriteLine("Tu agua es: " + agua);
                }
                break;

                case 3:
                    energia += 20;
                    Console.WriteLine("\nDescansaste, +20 puntos de energia! Tu energia es: " + energia);
                    probabilidad +=1 ;
                    Console.WriteLine("Aumentan las probabilidades de animales salvajes, ten cuidado!");    
                    Console.WriteLine("No aumentaron las demas stats. Energia: " + energia + " Tu comida: " + comida + "Tu agua:  " + agua);
                break;

                case 4:
                    Console.WriteLine("\nValiente explorador de la isla!");
                    if(probabilidad <= 3){
                        Console.WriteLine("Animales salvajes! -10 unidades de energia");
                        energia -= 10;
                        energiaAjuste = Math.Max(energia,0);
                        Console.WriteLine("\nTu energia es: " + energiaAjuste);
                    }else if(probabilidad >= 4 && probabilidad <= 5){
                        Console.WriteLine("Terrenos peligrosos! -20 unidades de energia");
                        energia -= 20;
                        energiaAjuste = Math.Max(energia,0);
                        Console.WriteLine("\nTu energia es: " + energiaAjuste);
                    }else{
                        botellas ++;
                        Console.WriteLine("Encontraste una botella, tienes " + botellas + " botellas");
                    }
                    break;

                    default:
                        Console.WriteLine("Opcion no valida");
                        break;
                }

        //Math.Min es una funcion que nos permite obtener el valor minimo entre dos numeros
        Console.WriteLine("\nConsumes 20 puntos de comida y 15 de agua por dia");
        int comidaRestada = Math.Min(comida, 20);
        int aguaRestada = Math.Min(agua, 15);
        energia -= (20 - comidaRestada) + (15 - aguaRestada);

        comida -= comidaRestada;
        agua -= aguaRestada;

        Console.WriteLine("\nSe acabó el dia, tus stats son: \nComida: " +comida + "\nAgua: " + agua + "\nEnergia: " + energia);
        

        if (aleatorio.Next(1, 10) == 1){
            switch (aleatorio.Next(1, 4)){
                case 1:
                    Console.WriteLine("\nLLuvia! +10 unidades de agua");
                    agua += 10 * botellas;
                    Console.WriteLine("Tus stats son: \nComida: " + comida + "\nAgua: " + agua + "\nEnergia: " + energia);
                    break;
                case 2:
                    Console.WriteLine("\nAnimales salvajes! -10 puntos de comida");
                    comida -= 10;
                    comidaAjuste = Math.Max(comida - 10,0);
                    Console.WriteLine("Tus stats son: \nComida: " + comidaAjuste + "\nAgua: " + agua + "\nEnergia: " + energia);
                    break;
                case 3:
                    Console.WriteLine("\nClima frio! -10 puntos de energia");
                    energia -= 10;
                    Console.WriteLine("Tus stats son: \nComida: " + comida + "\nAgua: " + agua + "\nEnergia: " + energia);
                    break;
            }
        }i++;
    //La condicion del do while es que siempre que la energia sea mayor a 0, seguira jugando
    } while (energia > 0);
    Console.WriteLine("\nSe acabó el dia " + i);
    //Si ya no tiene energia es game over
    if (energia <= 0){
        Console.WriteLine("\nGAME OVER: ya no tienes energia");
        Console.ReadKey();
    //Si el jugador llega al dia 10 y sigue con energia, ha ganado
    }else if (i == 10){
        Console.WriteLine("\nHAS GANADO!");
        Console.ReadKey();
    }
    break;
}