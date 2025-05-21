
/* Ejemplo funciones y procedimiento:
using System.Dynamic;
using System.Runtime.ExceptionServices;

Console.WriteLine("Hola por favor ingrese su nombre");
string nombre = Console.ReadLine();
saludar();

Console.WriteLine("Ahora ingrese un número para calcular su facctorial");
int numero = Convert.ToInt32(Console.ReadLine());

int factorial = CalcularFactorial(numero); 

Console.WriteLine($"El factorial del número ingresado es {factorial}");
void saludar()
{
    Console.WriteLine($"Buenos dias {nombre}");
}

int CalcularFactorial(int num)
{
    int resultado = 1;
    for (int i = num; i > 0; i--)
    {
        resultado = resultado * i;
    }
    return resultado; 
*/


using System.Collections;
using System.Globalization;
using System.Linq.Expressions;

Console.WriteLine("Seleccione la opción deseada: \n1. Pasar de Celsius a Fahrenheit \n2. Pasar de Fahrenheit a Celsius \n3. Información del programador \n4. Salir");
Console.WriteLine("Ingrese el número de la selección deseada");
int seleccion = Convert.ToInt32(Console.ReadLine());
double resultado = 0.0;

switch (seleccion)
{
   case 1:
   
        Console.WriteLine("Ingrese los grados celcius a convertir en fahrenheit");
        double grados = Convert.ToDouble(Console.ReadLine());
        double celcius = Celcius(grados);
        Console.WriteLine($"Los grados celcius ingresados equivale a {resultado} en fahrenheit");
        
   break;
   
   case 2:
        Console.WriteLine("Ingrese los grados Fahrenheit a convertir en celcius");
        double grados2 = Convert.ToDouble(Console.ReadLine());
        double fahrenheit = Fahrenheit(grados2);
        Console.WriteLine($"Los grados fahrenheit ingresados equivale a {resultado} en celcius");

    break;

    case 3: 
        Console.WriteLine("A continuación se presenta la información del programador");
        infoProgramador();
    break;
    case 4:
        salir();
    break;
    
    default:
    Console.WriteLine("Opción inválida");
    
    break;
}

double Celcius(double num)
{
    resultado = (num * 1.8)+32; 
    return resultado;
}

double Fahrenheit(double num)
{
    resultado = (num - 32)/1.8;
    return resultado;
}

void infoProgramador()
{ 
    Console.WriteLine("\nNombre:Juan Fernando López Cobo \nEdad: 20 años \nCarné: 1302925 \nEstudiante de Ingenieria en sistemas");
}

void salir()
{
    Console.WriteLine("Muchas gracias por usar el programa, Adiós ;)");
}