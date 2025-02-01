using System;



namespace Eri
{
    class Personaje
    {
        public string Nombre { get; set; }
        public string Character { get; set; }
        public string Habilidad { get; set; }
        public int TurnosHabilidad { get; set; } = 0;
    
    }


    class Laberinto
    {

        private static int alto = 21;   //Alto del laberinto.
        private static int ancho = 21;  //Ancho del laberinto.
        private static char[,] laberinto = new char[alto, ancho]; 


        private static Random random = new Random();    //Crea una variable random.

        
        private static List<Personaje> personajes = new List<Personaje> 
        {
            new Personaje { Nombre = "Sirius Black", Character = "🐺", Habilidad = "Cada 5 turnos obtiene el doble de movimientos." },
            new Personaje { Nombre = "Bellatrix Lestrange", Character = "🖤", Habilidad = "Inmune a la trampa de Magia Oscura." }, 
            new Personaje { Nombre = "Rodolphus Lestrange", Character = "🕷️", Habilidad = "Cada 10 turnos sana 10 de vida" }, 
            new Personaje { Nombre = "Barty Crouch Jr.", Character = "🎭", Habilidad = "Puede cambiar de posicion con el otro jugador cada 15 si quiere" }, 
            new Personaje { Nombre = "Dolores Umbridge", Character = "📜", Habilidad = "Hace que el otro jugador pierda una varita...pero solo puede utilizar esta habilidad dos veces en toda la partida" }
        };
    
    
        private static int player1Y;    //Posicion del jugador 1 en las filas
        private static int player1X;    //Posicion del jugador 1 en las columnas
        private static Personaje player1Char;


        private static int player2Y;    //Posicion del jugador 2 en las filas
        private static int player2X;    //Posicion del jugador 2 en las columnas
        private static Personaje player2Char;


        private static int varitasRecolectadasJugadorUno = 0;   //Cantidad de llaves que posee el jugador
        private static int varitasRecolectadasJugadorDos = 0;   //Cantidad de llaves que posee el jugador
        private static int totalVaritas = 3;   //Cantidad necesaria de varitas para abrir la puerta de salida
        private static int total = 9;   //Cantidad total de llaves en el mapa
        
        
        private static int trampas;
        private static int totalTrampas = 10;   //Cantidad total de trampas que se generan en el laberinto 
        private static char trampa_Petrificacion = 'P';     //Caracter que representa la trampa de petrificacion
        private static char trampa_Veneno = 'M';    //Caracter que representa la trampa de veneno
        private static char trampa_Magia_Oscura = 'O';      //Caracter que representa la trampa de magia oscura
        
        
        private static int vidaJugadorUno = 100;
        private static int vidaJugadorDos = 100;
        
        
        private static bool jugador1Afectado = false;       
        private static bool jugador2Afectado = false;

        private static int opcion;
        private static int habilidadUsada = 0;      //VAriable creada para contar las veces que utiliza la habilidad
        

        private static int movimientosplayer1 = 0;
        private static int movimientosplayer2 = 0;
        

        static void Main()
        {
            Introduccion();     //Llama al metodo para que aparezca una vreve introduccion

            Console.WriteLine("Jugador 1 elige primero");
            player1Char = MostrarPersonajes(personajes);        //El jugador 1 elige un personaje
            
            personajes.Remove(personajes[opcion]);      //Elimina el personaje elegido por el jugador 1

            Console.WriteLine("Elige ahora el jugador 2");
            player2Char = MostrarPersonajes(personajes);        //Elige ahora el jugador 2

            GenerarLaberinto();     //Llama al metodo para generar el laberinto.
            
            ColocarVaritasYPuerta();    //Llama al metodo para generar en el laberinto las llaves y la puerta. 
            
            ColocarTrampas();      //Llama al metodo para generar en el laberinto las trampas.
            
            EncontrarPosicionInicial();    //Llama al metodo para encontrar la casilla desde donde sale el jugador.
            
            bool turnoJugadorUno = true;
            int movimientosJugadorUno = 0;
            int movimientosJugadorDos = 0;

            while(true)
            {
            
                MostrarLaberinto();    //Llama al metodo para imprimir el laberinto actualizado, luego de limpiar la version anterior
                
                Console.WriteLine($"Varitas recolectadas jugador1: {varitasRecolectadasJugadorUno} / {totalVaritas}");     //Imprime la cantidad de varitas que ha coleccionado el jugador1 y las que necesita
                Console.WriteLine($"Vida jugador1: {vidaJugadorUno}");     // Muestra la vida del jugador1 
                
                
                Console.WriteLine($"Varitas recolectadas jugador2: {varitasRecolectadasJugadorDos} / {totalVaritas}");     //Imprime la cantidad de varitas que ha coleccionado el jugador2 y las que necesita
                Console.WriteLine($"Vida jugador2: {vidaJugadorDos}");     //Muestra la vida del jugador2 
                
                if(turnoJugadorUno)
                {
                    
                    
                    Console.WriteLine("Turno del jugador uno");
                    Console.WriteLine("Para moverte utilice W (arriba), A (izquierda), S (abajo), D (derecha)");
                    Console.WriteLine("Movimientos restantes: " + (3 - movimientosJugadorUno));

                    string movimiento = Convert.ToString(Console.ReadLine()!.ToUpper());

                    if (movimiento == "W") Mover(0, -1, true); // Jugador uno
                    else if (movimiento == "A") Mover(-1, 0, true); // Jugador uno
                    else if (movimiento == "S") Mover(0, 1, true); // Jugador uno
                    else if (movimiento == "D") Mover(1, 0, true); // Jugador uno
                    else {Console.WriteLine("Movimiento no valido. \ntoca alguna tecla para continuar"); Console.ReadKey();}

                    movimientosJugadorUno++;

                    if(movimientosJugadorUno == 3)
                    {
                        
                        turnoJugadorUno = false;
                        movimientosJugadorUno = 0;

                        Console.WriteLine("Presina cualquier tecla Espacio para cambiar al turno del Jugador 2...");
                        Thread.Sleep(3000);

                        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar)
                        {

                        }
                    
                    }

                    if (laberinto[player1Y, player1X] == 'V' && varitasRecolectadasJugadorUno == totalVaritas)
                    {
                        
                        Console.WriteLine("El jugador 1 ha ganado! Felicitaciones!");
                        Thread.Sleep(10000);

                        laberinto[player1Y, player1X] = ' '; // Limpia la puerta después de ganar
                        break;

                    }
                }
                
                else
                {
                    
                    
                    Console.WriteLine("Turno del jugador 2");
                    Console.WriteLine("Para moverte utilice I (arriba), J (izquierda), K (abajo), L (derecha)");
                    Console.WriteLine("Movimientos restantes: " + (3 - movimientosJugadorDos));

                    string movimiento = Convert.ToString(Console.ReadLine()!.ToUpper());
                    
                    if (movimiento == "I") Mover(0, -1, false);     //Jugador 2
                    else if (movimiento == "J") Mover(-1, 0, false);        //Jugador 2
                    else if (movimiento == "K") Mover(0, 1, false);     //Jugador 2
                    else if (movimiento == "L") Mover(1, 0, false);     //Jugador 2
                    else { Console.WriteLine("Movimiento no valido. \ntoca alguna tecla para continuar"); Console.ReadKey(); }

                    movimientosJugadorDos++;

                    if (movimientosJugadorDos == 3)
                    {
                        
                        turnoJugadorUno = true;
                        movimientosJugadorDos = 0;

                        Console.WriteLine("Presione la tecla ESPACIO para cambiar al turno del Jugador 1...");
                        Thread.Sleep(3000);

                        while(Console.ReadKey(true).Key != ConsoleKey.Spacebar)
                        {

                        }
                    
                    }
                    
                    if (laberinto[player2Y, player2X] == 'V' && varitasRecolectadasJugadorDos == totalVaritas)
                    {
                        
                        Console.WriteLine("El jugador 2 ha ganado! Felicitaciones!");
                        Thread.Sleep(10000);

                        laberinto[player2Y, player2X] = ' '; // Limpia la puerta después de ganar
                        break;
                    
                    }
                
                }
            
            }
        
        }


        static void Introduccion()
        {
            
            Console.ForegroundColor = ConsoleColor.DarkGray; // Cambiar el color del texto a un tono oscuro
            EscribirLentamente("====================================================");
            EscribirLentamente("¡Bienvenidos a la Fuga de Azkaban!");
            EscribirLentamente("====================================================");
            EscribirLentamente("\nEn las profundidades más oscuras y gélidas de Azkaban,");
            EscribirLentamente("donde el aire es tan frío que quema los pulmones y el silencio");
            EscribirLentamente("es roto solo por los susurros de las almas perdidas, dos prisioneros");
            EscribirLentamente("han sido condenados a una eternidad de desesperación.");
            EscribirLentamente("\nLos dementores, criaturas sin alma que se alimentan de la felicidad,");
            EscribirLentamente("rondan cada rincón de este laberinto infernal. Su aliento helado");
            EscribirLentamente("te persigue, y su presencia te deja vacío, sin esperanza, sin recuerdos.");
            EscribirLentamente("\nPara escapar, deberás:");
            EscribirLentamente("- Encontrar las varitas mágicas escondidas en este laberinto maldito.");
            EscribirLentamente("- Abrir la puerta de salida antes de que los dementores te encuentren.");
            EscribirLentamente("- Evitar las trampas mortales que acechan en cada esquina.");
            EscribirLentamente("\nCada paso que des podría ser tu último. Cada susurro que escuches");
            EscribirLentamente("podría ser el preludio de tu perdición. ¿Tienes el valor para enfrentar");
            EscribirLentamente("el horror de Azkaban y escapar con tu alma intacta?");
            EscribirLentamente("====================================================");
            EscribirLentamente("\nPresiona cualquier tecla para comenzar... si te atreves.");
            Console.ReadKey();
            Console.Clear();
            Console.ResetColor(); // Restablecer el color del texto
        
        }


        static void EscribirLentamente(string texto, int delay = 50)
        {
            
            foreach (char c in texto)
            {

                Console.Write(c);
                Thread.Sleep(delay);
            
            }
            Console.WriteLine();
        
        }


        static Personaje MostrarPersonajes(List<Personaje> personajes)
        {
            
            Console.WriteLine("Seleccione un personaje:");
            
            for(int i = 0; i < personajes.Count; i++)
            {
                
                Console.WriteLine($"{i + 1}. {personajes[i].Nombre} - {personajes[i].Habilidad}");
        
            }

            while(true)
            {
                Console.Write("Ingrese el número del personaje: ");
                
                if(int.TryParse(Console.ReadLine(), out int opcion) && opcion > 0 && opcion <= personajes.Count)
                {
                    break;
                }
            }

            opcion = Convert.ToInt32(Console.ReadLine()) - 1;
            return personajes[opcion];
        
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


        private static void ColocarTrampas()
        {
            
            for(int j = 0; j < totalTrampas; j++)
            {
                int y, x;

                do
                {
                    y = random.Next(1, alto - 1);
                    x = random.Next(1, ancho - 1);
                } while(laberinto[y, x] == ' ');

                int tipoTrampa = random.Next(3);
                
                switch(tipoTrampa)
                {
                    case 0: laberinto[y,x] = 'P'; break;
                    case 1: laberinto[y,x] = 'M'; break;
                    case 2: laberinto[y,x] = 'O'; break; 
                }
                 
            }
        
        }


        static void EncontrarPosicionInicial()
        {
            
            player1Y = 1;
            player1X = 1;

            player2Y = 19;
            player2X = 19;
 
            while(laberinto[ player1X, player1Y ] == 'K' || laberinto[ player1X, player1Y ] == 'V')
            {
                player1Y = random.Next(1, alto - 1);
                player1X = random.Next(1, ancho - 1);
                break;
            }

            while(laberinto[ player2X, player2Y ] == 'K' || laberinto[ player2X, player2Y ] == 'V')
            {

                player2Y = random.Next( 1, alto - 1);
                player2Y = random.Next( 1, ancho - 1);
                break;

            }
            
        
        }


        static void MostrarLaberinto()
        { 
            
            Console.Clear();

           for(int y = 0; y < alto; y++)
            {
                for(int x = 0; x < ancho; x++) 
                {
                    
                    if(x == player1X && y == player1Y) Console.Write(player1Char.Character);   //Mostrar el primer jugador
                    else if (x == player2X && y == player2Y) Console.Write(player2Char.Character); //Mostrar el segundo jugador
                    else if(laberinto[y,x] == 'K') Console.Write("✨");    //Mostrar las varitas
                    else if(laberinto[y,x] == 'V') Console.Write("🚪");    //Mostar las puerta
                    else if(laberinto[y,x] == 'P') Console.Write("🗿️");    //Mostrar la trampa de petrificacion
                    else if(laberinto[y,x] == 'M') Console.Write("💀");    //Mostrar la trampa de veneno en rojo
                    else if(laberinto[y,x] == 'O') Console.Write("🔮");    //Mostrar la trampa de magia oscura
                    else Console.Write(laberinto[y,x] == '#' ? "📦" : "  ");    //Mostrar las paredes y caminos del laberinto
                }
                
                Console.WriteLine();
            
            }
        
        }
        
        static void Mover(int deltaX, int deltaY, bool esJugadorUno)
        {
            int nuevaX = esJugadorUno ? player1X + deltaX: player2X + deltaX;
            int nuevaY = esJugadorUno ? player1Y + deltaY: player2Y + deltaY;


            //Verificar límites y muros
            if (nuevaX >= 0 && nuevaX < ancho && nuevaY >= 0 && nuevaY < alto && laberinto[nuevaY, nuevaX] != '#') 
            {   
                
                if(esJugadorUno && player1Char.Nombre == "Sirius Black")
                {
                    
                    player1Char.TurnosHabilidad++;

                    if(player1Char.TurnosHabilidad >= 5)
                    {
                        
                        Console.WriteLine("Sirius Black ha activado su habilidad");
                        Thread.Sleep(3000);

                        player1Char.TurnosHabilidad = 0;        //Reinicia el contador
                        Mover(deltaX, deltaY, esJugadorUno);        //Permite un turno adicional al llamar al metodo
                    
                    }

                    else if(!esJugadorUno && player2Char.Nombre == "Sirius Black")
                    {

                        player2Char.TurnosHabilidad++;

                        if(player2Char.TurnosHabilidad >= 5)
                        {

                            Console.WriteLine("Sirius Black ha activado su habilidad");
                            Thread.Sleep(3000);

                            player2Char.TurnosHabilidad = 0;        //Reinicia el contador

                            Mover(deltaX, deltaY, esJugadorUno);       //Permite un turno adicional al llamar al metodo
                        }
                    }
                
                }
                
                
                if(laberinto[nuevaY, nuevaX] == 'P')
                {
                    if(esJugadorUno)
                    {
                        
                        Console.WriteLine("El jugador1 ha caido en una trampa de petrificacion...esta petrificado y no puede moverse");
                        Thread.Sleep(5000);           
                    }
                    
                    else
                    {
                        
                        Console.WriteLine("El jugador2 ha caido en una trampa de petrificacion...esta petrificado y no puede moverse");
                        Thread.Sleep(5000);
                    }
                    
                    return;     //Evita que el jugador se mueva
                
                }
                
                
                else if(laberinto[nuevaY, nuevaX] == 'M')
                {
                    if(esJugadorUno)
                    {
                        
                        Console.WriteLine("Haz caido en una trampa de veneno...pierdes 20 de vida");
                        Thread.Sleep(3000);
                        vidaJugadorUno -= 20;
                        
                        if(vidaJugadorUno <= 0)
                        {
                            
                            vidaJugadorUno = 100; // Vuelve a la vida al máximo
                            player1X = 1; // Devuelve al jugador a la posición inicial en el eje X
                            player1Y = 1; // Devuelve al jugador a la posición inicial en el eje Y
                            if(varitasRecolectadasJugadorUno > 0) varitasRecolectadasJugadorUno--; // Le quita una varita al jugador1
                            Console.WriteLine("Ha muerto el jugador1. Vuelves a la posición inicial con la vida al máximo y pierdes una llave.");
                            Thread.Sleep(5000);
                        
                        }
                    
                    }
                    
                    else
                    {
                        
                        Console.WriteLine("El jugador2 ha caido en la trampa de veneno...pierde 20 de vida");
                        Thread.Sleep(3000);
                        vidaJugadorDos -= 20;
                        
                        if(vidaJugadorDos <= 0)
                        {
                            vidaJugadorDos = 100; // Vuelve a la vida al máximo
                            player2X = 19;   //Devuelve al jugador a la posición inicial en el eje X
                            player2Y = 19;   //Devuelve al jugador a la posición inicial en el
                            if(varitasRecolectadasJugadorDos > 0) varitasRecolectadasJugadorDos--;  //Le quita una varita al jugador2
                            Console.WriteLine("Ha muerto el jugador2 . Vuelve a la posición inicial con la vida al máximo y pierde una llave.");
                            Thread.Sleep(5000);
                        
                        }
                    
                    }
                
                }
                
                
                else if(laberinto[nuevaY, nuevaX] == 'O')
                {
                    
                    if(esJugadorUno)
                    {

                        if(player1Char.Nombre == "Bellatrix Lestrange")
                        {

                            Console.WriteLine("Bellatrix Lestrange es inmune a la trampa de Magia Oscura");
                            Thread.Sleep(3000);
                        }

                        else
                        {
                
                            Console.WriteLine("El jugador1 ha caido en una trampa de magia oscura");
                            Thread.Sleep(3000);
                            jugador1Afectado = true;

                        }
                    
                    }
                    
                    if(player2Char.Nombre == "Bellatrix Lestrange")
                    {

                        Console.WriteLine("Bellatrix Lestrange es inmune a la trampa de Magia Oscura");
                        Thread.Sleep(3000);

                    }
                    
                    else
                    {
                        
                        Console.WriteLine("El jugador2 ha caido en una trampa de magia oscura");
                        Thread.Sleep(3000);
                        jugador2Afectado = true;
                    
                    }
                
                }


                if (laberinto[nuevaY, nuevaX] == 'K' && (esJugadorUno ? varitasRecolectadasJugadorUno: varitasRecolectadasJugadorDos) < totalVaritas) 
                {
                    
                    if(esJugadorUno && jugador1Afectado)
                    {
                        
                        Console.WriteLine("No puedes recolectar varitas debido a que estas bajo el efecto de Magia Oscura");
                        Thread.Sleep(3000);
                    
                    }
                    
                    else if(esJugadorUno )
                    { 
                        
                        varitasRecolectadasJugadorUno++;
                        Console.WriteLine("El jugador1 ha recolectado una varita...cada vez esta mas cerca de poder escapar");
                        Thread.Sleep(3000);
                    
                    }
                    
                    else if(jugador2Afectado)
                    {
                        
                        Console.WriteLine("No puedes recolectar varitas debido a que estas bajo el efecto de Magia Oscura");
                        Thread.Sleep(3000);
                    
                    }
                    
                    else
                    {
                        
                        varitasRecolectadasJugadorDos++;
                        Console.WriteLine("El jugador2 ha recolectado una varita...escapar es una opcion cada vez mas real");
                        Thread.Sleep(3000);

                    } 
                
                }
                
                
                else if(laberinto[nuevaY, nuevaX] == 'V')
                {
                    
                    if (esJugadorUno)
                    {
                        
                        if (varitasRecolectadasJugadorUno < totalVaritas)
                        {
                            
                            Console.WriteLine("El jugador uno no tiene suficientes varitas para abrir la puerta...");
                            Thread.Sleep(3000);
                        
                        }
                    
                    }
                    
                    else
                    {
                        
                        if (varitasRecolectadasJugadorDos < totalVaritas)
                        {
                            
                            Console.WriteLine("El jugador dos no tiene suficientes varitas para abrir la puerta...");
                            Thread.Sleep(3000);          
                        
                        }
                    
                    }
                
                }


                if(esJugadorUno)
                {
                    
                    if (jugador1Afectado)
                    {
                        
                        jugador1Afectado = false;
                    
                    }
                
                }
                
                else
                {
                    
                    if (jugador2Afectado)
                    {
                        
                        jugador2Afectado = false;
                    
                    }
                
                }

                if(esJugadorUno)
                {
                    
                    player1X = nuevaX; 
                    player1Y = nuevaY;

                    if(player1Char.Nombre == "Rodolphus Lestrange")
                    {

                        player1Char.TurnosHabilidad++;

                        if(player1Char.TurnosHabilidad >= 10)
                        {

                            Console.WriteLine("Rodolphus Lestrange ha activado su habilidad: ¡Sana 10 de vida!");
                            vidaJugadorUno += 10;
                            if(vidaJugadorUno >= 100) vidaJugadorUno = 100;     //Limita el maximo de vida a 100
                            player1Char.TurnosHabilidad = 0;        //Reinicia el contador
                            Thread.Sleep(3000);
                        }
                    }

                    else if(player1Char.Nombre == "Barty Crouch Jr.")
                    {

                        player1Char.TurnosHabilidad++;

                        if(player1Char.TurnosHabilidad >= 15)
                        {

                            Console.WriteLine("Barty Coruch Jr. puede cambiar de posicion con el otro jugador. ¿Desea hacerlo? S(si) N(no)");
                            Thread.Sleep(3000);

                            string respuesta = Console.ReadLine().ToUpper();

                            while(respuesta != "S" && respuesta != "N")
                            {
                                Console.WriteLine("Respuesta no válida. Por favor, ingrese S para sí o N para no.");
                                respuesta = Console.ReadLine().ToUpper();
                            }

                            if(respuesta == "S")
                            {

                                int tempX = player1X;
                                int tempY = player1Y;
                                player1X = player2X;
                                player1Y = player2Y;
                                player2X = tempX;
                                player2Y = tempY;

                                Console.WriteLine("Posiciones intercambiadas");
                        
                            }

                            player1Char.TurnosHabilidad = 0;        //Reiniciaar el contador
                            Thread.Sleep(3000);

                        }
                    }
                
                    else if(player1Char.Nombre == "Dolores Umbridge")
                    {

                        if(habilidadUsada < 2 && varitasRecolectadasJugadorDos >= 1)
                        {

                            Console.WriteLine("Dolores Umbridge puede hacer que el otro jugador pierda una varita. Desea hacerlo? S(si) N(no)");
                            string respuesta = Console.ReadLine().ToUpper();

                            while(respuesta != "S" && respuesta != "N")
                            {
                                Console.WriteLine("Respuesta no válida. Por favor, ingrese S para sí o N para no.");
                                respuesta = Console.ReadLine().ToUpper();
                            }

                            if(respuesta == "S")
                            {

                                varitasRecolectadasJugadorDos -= 1;     //Hace que pierda una varita al jugador 2
                                Console.WriteLine("El jugador 2 ha perdido una varita.");
                                Thread.Sleep(3000);
                                habilidadUsada++;       //Aumenta el contador

                            }
                        }

                        else
                        {
                            Console.WriteLine("Dolores no puede usar la habilidad en este momento");
                            Thread.Sleep(2000);
                        }
                    }
                }
                
                else
                {
                    
                    player2X = nuevaX;
                    player2Y = nuevaY;

                    if(player2Char.Nombre == "Rodolphus Lestrange")
                    {
                        
                        player2Char.TurnosHabilidad++;

                        if(player2Char.TurnosHabilidad >= 10)
                        {
                            
                            Console.WriteLine("Rodolphus Lestrange ha activado su habilidad: ¡Sana 10 puntos de vida!");
                            vidaJugadorDos += 10;
                            if(vidaJugadorDos > 100) vidaJugadorDos = 100; // Limitar la vida máxima a 100
                            player2Char.TurnosHabilidad = 0; // Reiniciar el contador
                            Thread.Sleep(3000);
        
                        }
                    
                    }

                    else if(player2Char.Nombre == "Barty Crouch Jr.")
                    {

                        player2Char.TurnosHabilidad++;

                        if(player2Char.TurnosHabilidad >= 15)
                        {

                            Console.WriteLine("Barty Crouch Jr. puede cambiar de posición con el otro jugador. ¿Desea hacerlo? (S/N)");
                            string respuesta = Console.ReadLine().ToUpper();

                             while(respuesta != "S" && respuesta != "N")
                            {
                                Console.WriteLine("Respuesta no válida. Por favor, ingrese S para sí o N para no.");
                                respuesta = Console.ReadLine().ToUpper();
                            }

                            if(respuesta == "S")
                            {
                                
                                int tempX = player2X;
                                int tempY = player2Y;
                                player2X = player1X;
                                player2Y = player1Y;
                                player1X = tempX;
                                player1Y = tempY;

                                Console.WriteLine("Posiciones intercambiadas.");
                            }

                            player2Char.TurnosHabilidad = 0; // Reiniciar el contador
                            Thread.Sleep(3000);                            
                        
                        }
                    
                    }

                    else if(player2Char.Nombre == "Dolores Umbridge")
                    {
                        
                        if(habilidadUsada < 2 && varitasRecolectadasJugadorUno >= 1)
                        {
                            Console.WriteLine("Dolores Umbridge puede hacer que el otro jugador pierda una varita. ¿Desea hacerlo?");
                            string respuesta = Console.ReadLine().ToUpper();

                            while(respuesta != "S" && respuesta != "N")
                            {
                                Console.WriteLine("Respuesta no válida. Por favor, ingrese S para sí o N para no.");
                                respuesta = Console.ReadLine().ToUpper();
                            }

                            if(respuesta == "S")
                            {
                                // Hacer que el otro jugador pierda una varita
                                varitasRecolectadasJugadorUno -= 1;
                                Console.WriteLine("El jugador 1 ha perdido una varita.");
                                habilidadUsada++; // Incrementar el contador de veces que se ha utilizado la habilidad
                            }
                        }
                        else
                        {
                            Console.WriteLine("Dolores Umbridge no puede utilizar su habilidad en este momento.");
                            Thread.Sleep(2000);
                        }
                    }
                }
            
            }
            
            else
            {
                
                Console.WriteLine("Movimiento no valido, hay una pared o estas fuera de los limites");
                Thread.Sleep(3000);
            
            }
        
        }
    
    }

}

