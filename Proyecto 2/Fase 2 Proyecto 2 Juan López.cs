using System;
using System.Collections.Generic;
//Juan Fernando López Cobo 1302925

namespace SistemaEstacionamiento
{
    // Clase que representa un espacio de estacionamiento.
    class Espacio
    {
        public string Codigo { get; set; }      // Ej: "A1", "B2", etc.
        public string Tipo { get; set; }        // "Moto", "SUV" o "Sedán"
        public bool Ocupado { get; set; }
        public Vehiculo Vehiculo { get; set; }  // Vehículo que ocupa el espacio (si lo hay)

        public Espacio(string codigo, string tipo)
        {
            Codigo = codigo;
            Tipo = tipo;
            Ocupado = false;
            Vehiculo = null;
        }
    }

    // Clase que representa un vehículo.
    class Vehiculo
    {
        public string Marca { get; set; }
        public string Color { get; set; }
        public string Placa { get; set; }          // Cadena ingresada por el usuario, Validando si usa nomenclatura de placas guatemalteca.
        public string Tipo { get; set; }           // "Moto", "Sedán" o "SUV"
        public int HoraEntrada { get; set; }       // Generada automáticamente (entre 6 y 20)
        public string CodigoEstacionamiento { get; set; } // Código del espacio asignado

        public Vehiculo(string marca, string color, string placa, string tipo, int horaEntrada, string codigoEstacionamiento)
        {
            Marca = marca;
            Color = color;
            Placa = placa;
            Tipo = tipo;
            HoraEntrada = horaEntrada;
            CodigoEstacionamiento = codigoEstacionamiento;
        }
    }

    // Clase que gestiona la lógica del estacionamiento.
    class Estacionamiento
    {
        //Instancia de la Matriz para guardar los vehiculos ingresados
        public Espacio[,] Espacios;
        public int Pisos;
        public int EspaciosPorPiso;

        // Se utiliza un diccionario para almacenar los vehículos; la clave es la placa y se usa
        // StringComparer.OrdinalIgnoreCase para que la búsqueda no distinga entre mayúsculas y minúsculas.
        public Dictionary<string, Vehiculo> VehiculosRegistrados;
        public int MotoCount, SUVCount, SedanCount;

        public Estacionamiento(int pisos, int espaciosPorPiso, int motoCount, int suvCount)
        {
            Pisos = pisos;
            EspaciosPorPiso = espaciosPorPiso;
            MotoCount = motoCount;
            SUVCount = suvCount;
            int total = Pisos * EspaciosPorPiso;
            SedanCount = total - (motoCount + suvCount);
            // Se inicializa el diccionario de vehículos con comparador insensible a mayúsculas.
            VehiculosRegistrados = new Dictionary<string, Vehiculo>(StringComparer.OrdinalIgnoreCase);
            Espacios = new Espacio[Pisos, EspaciosPorPiso];
            InicializarEspacios();
        }

        private void InicializarEspacios()
        {
            int contMoto = 0, contSUV = 0, contSedan = 0;
            for (int i = 0; i < Pisos; i++)
            {
                for (int j = 0; j < EspaciosPorPiso; j++)
                {
                    string codigo = $"{(char)('A' + i)}{j + 1}";
                    string tipo;
                    if (contMoto < MotoCount)
                    {
                        tipo = "Moto";
                        contMoto++;
                    }
                    else if (contSUV < SUVCount)
                    {
                        tipo = "SUV";
                        contSUV++;
                    }
                    else
                    {
                        tipo = "Sedán";
                        contSedan++;
                    }
                    Espacios[i, j] = new Espacio(codigo, tipo);
                }
            }
        }
        //este mapa mostrara los espacios disponibles para el tipo de vehiculo ingresado. 
        public void MostrarMapaParaTipo(string tipoVehiculo)
        {
            Console.WriteLine($"\nMapa de espacios disponibles para {tipoVehiculo}:");
            for (int i = 0; i < Pisos; i++)
            {
                for (int j = 0; j < EspaciosPorPiso; j++)
                {
                    Espacio esp = Espacios[i, j];
                    if (esp.Tipo.Equals(tipoVehiculo, StringComparison.OrdinalIgnoreCase) && !esp.Ocupado) //compara si esta desocupado, coloca el código
                        Console.Write(esp.Codigo + "\t");
                    else
                        Console.Write("X\t"); //Colocara una x en los que no son del tipo de vehiculo ingresado y en los que esten ocupados.
                }
                Console.WriteLine();
            }
        }
        //mapa completo que se mostrara, este solo mostrará con X los espacios ocupados, se usa en la opción 2
        public void MostrarMapaCompleto()
        {
            Console.WriteLine("\nMapa completo de estacionamiento:");
            for (int i = 0; i < Pisos; i++)
            {
                for (int j = 0; j < EspaciosPorPiso; j++)
                {
                    Espacio esp = Espacios[i, j];
                    if (esp.Ocupado)
                        Console.Write("X\t");
                    else
                        Console.Write(esp.Codigo + "\t");
                }
                Console.WriteLine();
            }
        }

        public (int, int) ConvertirCodigoAPosicion(string codigo)
        {
            if (codigo.Length < 2) //si el código es menor que 2, regresara (-1,-1) y esto sera tomado por error
                return (-1, -1);
            char letra = codigo[0];
            int fila = letra - 'A';
            if (!int.TryParse(codigo.Substring(1), out int num)) //si el resto del codigo no es un numero valido, dara error.
                return (-1, -1);
            int col = num - 1;
            if (fila < 0 || fila >= Pisos || col < 0 || col >= EspaciosPorPiso) //verificamos si los números se encuntran dentro del rango solicitado
                return (-1, -1);
            return (fila, col); //devuelve las coordenadas para la matriz
        }

        public bool IngresarVehiculo(Vehiculo vehiculo, string codigo)
        {
            var (fila, col) = ConvertirCodigoAPosicion(codigo); //convertir el código a coordenada
            if (fila == -1)
                return false;
            Espacio esp = Espacios[fila, col];
            if (!esp.Ocupado && esp.Tipo.Equals(vehiculo.Tipo, StringComparison.OrdinalIgnoreCase)) //verifica si esta ocupado y si es del mismo tipo de vehículo
            {
                esp.Ocupado = true;
                esp.Vehiculo = vehiculo;
                vehiculo.CodigoEstacionamiento = codigo;
                VehiculosRegistrados[vehiculo.Placa] = vehiculo;   //de lo anterior cumplirse, procedera a estacionar el vehiculo.
                return true;
            }
            return false;
        }

        // Al buscar no se hace conversión explícita, ya que el diccionario ya es insensible a mayúsculas.
        public void BuscarVehiculoPorPlaca(string placa)
        {
            if (VehiculosRegistrados.ContainsKey(placa)) //verifica si tiene la placa
            {
                Vehiculo v = VehiculosRegistrados[placa]; //de contener la placa procedera a mostrar los datos del vehiculo.
                Console.WriteLine("\nVehículo encontrado:");
                Console.WriteLine($"Placa: {v.Placa}");
                Console.WriteLine($"Marca: {v.Marca}");
                Console.WriteLine($"Color: {v.Color}");
                Console.WriteLine($"Tipo: {v.Tipo}");
                Console.WriteLine($"Hora de entrada: {v.HoraEntrada}");
                Console.WriteLine($"Código de estacionamiento: {v.CodigoEstacionamiento}");
            }
            else
            {
                Console.WriteLine("⚠️ Vehículo no encontrado."); //De no encontrar la placa mostrara este mensaje
            }
        }

        public bool RetirarVehiculo(string codigo, out Vehiculo vehiculo, out int duracionHoras) //utilizamos Out para enviar los datos como referencia
        {
            var (fila, col) = ConvertirCodigoAPosicion(codigo);
            vehiculo = null;
            duracionHoras = 0;
            if (fila == -1)
                return false;
            Espacio esp = Espacios[fila, col];
            if (esp.Ocupado)
            {
                vehiculo = esp.Vehiculo;
                Random rnd = new Random();
                int maxHoras = 24 - vehiculo.HoraEntrada;
                if (maxHoras < 0)
                    maxHoras = 0;
                duracionHoras = rnd.Next(0, maxHoras + 1); //calculo de horas que estuvo en el estacionamiento 
                esp.Ocupado = false;
                esp.Vehiculo = null; //cambia los valores, ya que se retiro el vehiculo
                VehiculosRegistrados.Remove(vehiculo.Placa);
                return true; //devuelve true si fue exitosa la salida
            }
            return false; //devuelve false si fue erronea la salida
        }

        public void IngresarLoteVehiculos(Random rnd)
        {
            int numVehiculos = rnd.Next(2, 7); // Entre 2 y 6 vehículos.
            string[] marcas = { "Honda", "Mazda", "Hyundai", "Toyota", "Suzuki" };
            string[] colores = { "Rojo", "Azul", "Negro", "Gris", "Blanco" };
            string[] tipos = { "Moto", "Sedán", "SUV" };

            Console.WriteLine($"\nIngresando automáticamente {numVehiculos} vehículos:");
            for (int i = 0; i < numVehiculos; i++)
            {
                string marca = marcas[rnd.Next(marcas.Length)];
                string color = colores[rnd.Next(colores.Length)];
                string tipo = tipos[rnd.Next(tipos.Length)];
                int horaEntrada = rnd.Next(6, 21); // Genera hora entre 6 y 20.

                string placa = GenerarPlacaAleatoria(rnd, tipo);
                string codigoDisponible = BuscarEspacioDisponible(tipo);
                if (codigoDisponible != null)
                {
                    Vehiculo v = new Vehiculo(marca, color, placa, tipo, horaEntrada, codigoDisponible);
                    IngresarVehiculo(v, codigoDisponible);
                    Console.WriteLine($"Vehículo con placa {v.Placa} (Tipo: {v.Tipo}) ubicado en {codigoDisponible}");
                }
                else
                {
                    Console.WriteLine($"No hay espacio disponible para un vehículo de tipo {tipo} (Placa generada: {placa}).");
                }
            }
            MostrarMapaCompleto();
        }

        public string BuscarEspacioDisponible(string tipo)
        {
            for (int i = 0; i < Pisos; i++)
            {
                for (int j = 0; j < EspaciosPorPiso; j++)
                {
                    Espacio esp = Espacios[i, j];
                    if (!esp.Ocupado && esp.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase))
                        return esp.Codigo;
                }
            }
            return null;
        }

        // Genera una placa aleatoria de 6 caracteres según el tipo (para ingreso automático).
        // Para Moto: "M" + 3 dígitos + 2 letras.
        // Para Sedán/SUV: "P" o "A" (aleatoriamente) + 3 dígitos + 2 letras.
        public string GenerarPlacaAleatoria(Random rnd, string tipo)
        {
            string prefix;
            if (tipo.Equals("Moto", StringComparison.OrdinalIgnoreCase))
            {
                prefix = "M";
            }
            else if (tipo.Equals("Sedán", StringComparison.OrdinalIgnoreCase) || tipo.Equals("SUV", StringComparison.OrdinalIgnoreCase))
            {
                prefix = rnd.Next(0, 2) == 0 ? "P" : "A";
            }
            else
            {
                prefix = "X";
            }

            string digits = rnd.Next(0, 1000).ToString("D3"); // 3 dígitos
            string letters = "";
            for (int i = 0; i < 2; i++)
            {
                letters += (char)('A' + rnd.Next(0, 26));
            }
            return prefix + digits + letters;
        }
    }

    public static class Pago
    {
        public static int CalcularTarifa(int duracion)
        {
            if (duracion <= 1)
                return 0;
            else if (duracion >= 2 && duracion <= 4)
                return 15;
            else if (duracion >= 5 && duracion <= 7)
                return 45;
            else if (duracion >= 8 && duracion <= 12)
                return 60;
            else
                return 150;
        }

        public static void RealizarPago(int tarifa)
        {
            Console.WriteLine($"\nMonto a pagar: Q{tarifa}");
            if (tarifa == 0)
            {
                Console.WriteLine("Cortesía: no se debe pagar nada.");
                return;
            }
            Console.WriteLine("Seleccione el método de pago:");
            Console.WriteLine("1. Tarjeta");
            Console.WriteLine("2. Sticker recargable");
            Console.WriteLine("3. Efectivo");
            int metodo = int.Parse(Console.ReadLine());
            if (metodo == 3)
            {
                Console.Write("Ingrese el monto con el que pagará: Q");
                int monto = int.Parse(Console.ReadLine());
                if (monto < tarifa)
                {
                    Console.WriteLine("Monto insuficiente para realizar el pago.");
                }
                else
                {
                    int vuelto = monto - tarifa;
                    Console.WriteLine($"Pago aceptado. Vuelto: Q{vuelto}");
                    int[] billetes = { 100, 50, 20, 10, 5 };
                    Console.WriteLine("Distribución del vuelto:");
                    foreach (int valor in billetes)
                    {
                        int cantidad = vuelto / valor;
                        if (cantidad > 0)
                        {
                            Console.WriteLine($"Billetes de Q{valor}: {cantidad}");
                        }
                        vuelto %= valor;
                    }
                }
            }
            else
            {
                Console.WriteLine("Pago procesado con tarjeta o sticker recargable.");
            }
        }
    }

    class Program
    {
        // Función para validar la placa ingresada por el usuario según los lineamientos.
        // Para Moto: 6 caracteres, iniciar con 'M', luego 3 dígitos y terminar con 2 letras.
        // Para Sedán/SUV: 6 caracteres, iniciar con 'P' o 'A', luego 3 dígitos y terminar con 2 letras.
        public static bool ValidarPlaca(string placa, string tipo)
        {
            if (placa.Length != 6)
                return false;

            if (tipo.Equals("Moto", StringComparison.OrdinalIgnoreCase))
            {
                if (placa[0] != 'M')
                    return false;
                for (int i = 1; i <= 3; i++)
                {
                    if (!char.IsDigit(placa[i]))
                        return false;
                }
                for (int i = 4; i < 6; i++)
                {
                    if (!char.IsLetter(placa[i]))
                        return false;
                }
                return true;
            }
            else if (tipo.Equals("Sedán", StringComparison.OrdinalIgnoreCase) || tipo.Equals("SUV", StringComparison.OrdinalIgnoreCase))
            {
                char first = placa[0];
                if (first != 'P' && first != 'A')
                    return false;
                for (int i = 1; i <= 3; i++)
                {
                    if (!char.IsDigit(placa[i]))
                        return false;
                }
                for (int i = 4; i < 6; i++)
                {
                    if (!char.IsLetter(placa[i]))
                        return false;
                }
                return true;
            }
            return false;
        }

        static void Main()
        {
            Random rnd = new Random();

            Console.Write("Ingrese la cantidad de pisos: ");
            int pisos = int.Parse(Console.ReadLine());
            Console.Write("Ingrese la cantidad de espacios por piso: ");
            int espaciosPorPiso = int.Parse(Console.ReadLine());
            Console.Write("Ingrese la cantidad de estacionamientos para Moto: ");
            int moto = int.Parse(Console.ReadLine());
            Console.Write("Ingrese la cantidad de estacionamientos para SUV: ");
            int suv = int.Parse(Console.ReadLine());
            Estacionamiento estacionamiento = new Estacionamiento(pisos, espaciosPorPiso, moto, suv);

            bool salir = false;
            while (!salir)
            {
                Console.WriteLine("\n----- Menú de Opciones -----");
                Console.WriteLine("1. Ingresar un vehículo manualmente");
                Console.WriteLine("2. Ingresar lote de vehículos (automático)");
                Console.WriteLine("3. Encontrar un vehículo");
                Console.WriteLine("4. Retirar un vehículo");
                Console.WriteLine("5. Salir");
                Console.Write("Seleccione una opción: ");
                int opcion = int.Parse(Console.ReadLine());

                switch (opcion)
                {
                    case 1:
                        Console.Write("Ingrese la marca del vehículo: ");
                        string marca = Console.ReadLine();
                        Console.Write("Ingrese el color del vehículo: ");
                        string color = Console.ReadLine();

                        // Se solicita y valida el tipo.
                        string tipo;
                        while (true)
                        {
                            Console.Write("Ingrese el tipo de vehículo (Moto/Sedán/SUV): ");
                            tipo = Console.ReadLine();
                            if (tipo.Equals("Moto", StringComparison.OrdinalIgnoreCase) ||
                                tipo.Equals("Sedán", StringComparison.OrdinalIgnoreCase) ||
                                tipo.Equals("SUV", StringComparison.OrdinalIgnoreCase))
                                break;
                            else
                                Console.WriteLine("Tipo de vehículo no válido. Intente nuevamente.");
                        }

                        int horaEntrada = rnd.Next(6, 21);
                        Console.WriteLine($"Hora de entrada generada: {horaEntrada}");

                        // Solicita ingresar la placa y se valida.
                        string placa;
                        while (true)
                        {
                            Console.Write("Ingrese la placa del vehículo (según lineamientos): ");
                            placa = Console.ReadLine().ToUpper();
                            if (ValidarPlaca(placa, tipo))
                                break;
                            else
                                Console.WriteLine("La placa ingresada no cumple con los lineamientos. Intente nuevamente.");
                        }
                        Console.WriteLine($"Placa ingresada: {placa}");

                        estacionamiento.MostrarMapaParaTipo(tipo);
                        Console.Write("Ingrese el código de estacionamiento en el que desea ubicar el vehículo: ");
                        string codigo = Console.ReadLine();

                        Vehiculo v = new Vehiculo(marca, color, placa, tipo, horaEntrada, codigo);
                        if (estacionamiento.IngresarVehiculo(v, codigo))
                        {
                            Console.WriteLine($"Vehículo con placa {placa} ingresado en el espacio {codigo}.");
                        }
                        else
                        {
                            Console.WriteLine("No se pudo ingresar el vehículo. Verifique el espacio y que corresponda al tipo.");
                        }
                        break;

                    case 2:
                        estacionamiento.IngresarLoteVehiculos(rnd);
                        break;

                    case 3:
                        Console.Write("Ingrese la placa del vehículo a buscar: ");
                        string placaBusqueda = Console.ReadLine();
                        estacionamiento.BuscarVehiculoPorPlaca(placaBusqueda);
                        break;

                    case 4:
                        Console.Write("Ingrese el código de estacionamiento donde se encuentra su vehículo: ");
                        string codigoRetiro = Console.ReadLine();
                        if (estacionamiento.RetirarVehiculo(codigoRetiro, out Vehiculo vehRetirado, out int duracion))
                        {
                            Console.WriteLine($"El vehículo con placa {vehRetirado.Placa} ha sido retirado.");
                            Console.WriteLine($"Tiempo de estadía: {duracion} horas.");
                            int tarifa = Pago.CalcularTarifa(duracion);
                            Pago.RealizarPago(tarifa);
                        }
                        else
                        {
                            Console.WriteLine("No se encontró vehículo en el código indicado o ya se liberó el espacio.");
                        }
                        break;

                    case 5:
                        Console.WriteLine("Saliendo del sistema...");
                        salir = true;
                        break;

                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }
            }
        }
    }
}
