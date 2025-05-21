
/* ejemplo arreglos
Console.WriteLine("Ejemplo de arreglos");

String[] nombres = ["Juan", "Pedro", "Luisa", "Adriana", "Sofia"];
string[] apellidos = new string[5];

string nombre = nombres[3];
for (int i = 0; i < nombres.Length; i++)
{
    Console.WriteLine($"nombre no, {i}: " + nombres[i]);
    apellidos[i] = Console.ReadLine();
}

for (int i = 0; i < apellidos.Length; i++)
{
    Console.WriteLine($"apellido no. {i}: " + apellidos[i]);
}
*/

//Juan Fernando López Cobo
string[] estudiantes = ["Juan", "Pedro", "Luisa", "Adriana", "Sofia"];
int[] notas = [88, 75, 96, 77, 59];
int promedio = 0;


for (int i = 0; i < estudiantes.Length; i++)
{
    Console.WriteLine(estudiantes[i] + " - " + notas[i]);
    promedio += notas[i];
}

int promedioFinal = promedio/notas.Length;
Console.WriteLine($"El promedio de los estudiantes es de {promedioFinal}");