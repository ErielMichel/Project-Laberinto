using System;


namespace Eri
{
    class Laberinto
    {

        private static int alto = 21;   //Alto del laberinto.
        private static int ancho = 21;  //Ancho del laberinto.
        private static char[,] laberinto = new char[alto, ancho];   
        private static Random random = new Random();    //Crea una variable random.
        private static int playerY;    //Posicion del jugador en las filas.
        private static int playerX;    //Posicion del jugador en las columnas.
        private static int varitasRecolectadas = 0;   //Cantidad de llaves que posee el jugador.
        private static int totalVaritas = 3;   //Cantidad necesaria de varitas para abrir la puerta de salida.
        private static int total = 5;   //Cantidad total de llaves en el mapa.
        
        

        static void Main()
        {
            GenerarLaberinto();     //Llama al metodo para generar el laberinto.
            ColocarVaritasYPuerta();       //Llama al metodo para generar en el laberinto las llaves y la puerta. 
            EncontrarPosicionInicial();    //Llama al metodo para encontrar la casilla desde donde sale el jugador.


            while(true)
            {
            
                MostrarLaberinto();    //Llama al metodo para imprimir el laberinto actualizado, luego de limpiar la version anterior.
                Console.WriteLine($"Varitas recolectadas: {varitasRecolectadas} / {totalVaritas}");     //Imprime la cantidad de varitas que haz coleccionado y las que necesitas.
                Console.WriteLine("Para moverte utilice W (arriba), A (izquierda), S (abajo), D (derecha)");       //Imprime las teclas que debes utilizar para jugar.
                
                string movimiento = Convert.ToString(Console.ReadLine()!.ToUpper());

                if(movimiento == "W") Mover(0,-1);
                else if(movimiento == "A") Mover(-1,0);
                else if(movimiento == "S") Mover(0,1);
                else if(movimiento == "D") Mover(1,0);      
                else {Console.WriteLine("Movimiento no valido. \ntoca alguna tecla para continuar"); Console.ReadKey();}
            
                if(laberinto[playerY, playerX] == 'V' && varitasRecolectadas == totalVaritas)
                {
                    Console.WriteLine("Duro en tu peso, haz conseguido vencer...he notado que te mantienes firme ante las dificultades, si aplicas este método de buena manera en la vida lograrás cosas maravillosas, sólo no te rindas nunca");
                    break;
                }
            }
        }


        static void GenerarLaberinto()
        {
            InicializarLaberinto();
            Cavar(1,1);
        }


        private static void Mezclar(int[] array)
        {
            for (int i = array.Length - 1; i > 0; i--) 
            { 
                int j = random.Next(i + 1); 
                int temp = array[i]; 
                array[i] = array[j]; 
                array[j] = temp;
            }
        }


        private static void InicializarLaberinto()
        {
            for(int y = 0; y < ancho; y++)
            {
                for(int x = 0; x < alto; x++)
                {
                    laberinto[y, x] = '#';
                }
            }
        }


        private static void Cavar(int y, int x)
        {
            laberinto[y,x] = ' '; //Marca la celda como visitada

            int[] direcciones = {0, 1, 2, 3};
            Mezclar(direcciones);

            foreach(int direccion in direcciones)
            {
                int ny = y, nx = x;

                switch(direccion)
                {
                    case 0 : nx = x + 2; break;     //Derecha
                    case 1 : ny = y + 2; break;     //Abajo
                    case 2 : nx = x - 2; break;     //Izquierda
                    case 3 : ny = y - 2; break;     //Arriba
                }

                if (ny >= 0 && ny < alto && nx >= 0 && nx < ancho && laberinto[ny, nx] == '#') 
                { 
                    laberinto[ny - (ny - y) / 2, nx - (nx - x) / 2] = ' ';
                    laberinto[ny, nx] = ' ';
                    Cavar(ny,nx);
                }
            }
        }


        private static void ColocarVaritasYPuerta() 
        { 
            for(int i = 0; i < total; i++) 
            { 
                int y, x; 
                do 
                { 
                    y = random.Next(1, alto - 1); 
                    x = random.Next(1, ancho - 1); 
                
                } while(laberinto[y, x] != ' '); 
                laberinto[y, x] = 'K';  // Colocar una varita 
            } 
            
            int puertaY, puertaX; 
            do 
            { 
                puertaY = random.Next(1, alto - 1); 
                puertaX = random.Next(1, ancho - 1); 
            
            } while (laberinto[puertaY, puertaX] != ' '); 
            laberinto[puertaY, puertaX] = 'V';  // Colocar la puerta de salida
        }


        static void EncontrarPosicionInicial()
        {
            playerY = 1;
            playerX = 1;
 
            while(laberinto[playerX, playerY] == 'K' || laberinto[playerX, playerY] == 'V')
            {
                playerY = random.Next(1, alto - 1);
                playerX = random.Next(1, ancho - 1);
            }
            
        }


        static void MostrarLaberinto()
        { 
            Console.Clear();

           for(int y = 0; y < alto; y++)
            {
                for(int x = 0; x < ancho; x++) 
                {
                    if(x == playerX && y == playerY) Console.Write("🐺");   //Mostrar el jugador
                    else if(laberinto[y, x] == 'K') Console.Write("✨");    //Mostrar las varitas
                    else if(laberinto[y, x] == 'V') Console.Write("🚪");
                    else Console.Write(laberinto[y,x] == '#' ? "📦" : "  ");
                }
                Console.WriteLine();
            }
        }
        
        static void Mover(int deltaX, int deltaY)
        {
            int nuevaX = playerX + deltaX;
            int nuevaY = playerY + deltaY;


            //Verificar límites y muros
            if (nuevaX >= 0 && nuevaX < ancho && nuevaY >= 0 && nuevaY < alto && laberinto[nuevaY, nuevaX] != '#') 
            { 
                if (laberinto[nuevaY, nuevaX] == 'K' && varitasRecolectadas < totalVaritas) 
                { 
                    varitasRecolectadas++;
                    Console.WriteLine("Haz recolectado una varita...cada vez estas mas cerca de poder escapar"); 
                }
                playerX = nuevaX; 
                playerY = nuevaY;
            }

            else
            {
                Console.WriteLine("Movimiento no valido, hay una pared o estas fuera de los limites");
            }
        }
    }
}
