//Ortega Espinosa Angel Ismael
using System;
using System.Collections.Generic;
//Requerimiento 1.- Actualizacion:
//                                  a) Agregar el residuo de la division en el factor LISTO
//                                  b) Agregar en instruccion los incrementos de termino y de factor LISTO
//                                     a++, a--, a+=1, a-=1, a*=1, a/=1, a%=1 (hay que matchear un numero)
//                                     en donde el 1 puede ser cualquier numero entero o una expresion
//                                  c) Programar el destructor para ejecutar el metodo cerrarArchivo 
//                                     Existe una libreria especial para esto, trabajar en Lexico?? LISTO
//Requerimiento 2.- Actualizacion, parte 2
//                                  a) Marcar errores semanticos cuando los incrementos de termino o incrementos de factor
//                                     Superen el rango de la variable LISTO
//                                  b) Considerar el inciso b) y c) para el for LISTO
//                                  c) Hacer que funcione el do() y el while() LISTO
//Requerimiento 3.-
//                                  a) Considerar las variables y los casteos de las expresiones matematicas en ensamblador LISTO
//                                  b) Considerar el residuo de la division en el ensamblador LISTO
//                                  c) Programar el Printf y el Scanf en ensamblador LISTO
//                                     Verificar el caso de los printf y printn
//Requerimiento 4.-                 
//                                  a) Programar el else en ensamblador LISTO
//                                  b) Programar el for en ensamblador
//Requerimiento 5.-                 
//                                  a) Programar el while en ensamblador
//                                  b) Programar el do() while en ensamblador
namespace semantica
{
    public class Lenguaje : Sintaxis, IDisposable
    {
        List<Variable> variables = new List<Variable>();
        Stack<float> stack = new Stack<float>();

        Variable.TipoDato dominante;
        int cIf, cFor, cWhile, cDoWhile;
        string IncASM;
        public Lenguaje()
        {
            cIf = cFor = cWhile = cDoWhile = 0;
        }
        public Lenguaje(string nombre) : base(nombre)
        {
            cIf = cFor = cWhile = cDoWhile = 0;
        }
        //Requerimiento 1 a) Creacion de Destructor
        public void Dispose()
        {
            Console.WriteLine("Destructor");
            cerrar();
        }

        private void addVariable(String nombre, Variable.TipoDato tipo)
        {
            variables.Add(new Variable(nombre, tipo));
        }

        private void displayVariables()
        {
            log.WriteLine();
            log.WriteLine("variables: ");
            foreach (Variable v in variables)
            {
                log.WriteLine(v.getNombre() + " " + v.getTipo() + " " + v.getValor());
            }
        }

        private void variableASM()
        {
            //log.WriteLine();
            asm.WriteLine(";Variables: ");
            foreach (Variable v in variables)
            {
                asm.WriteLine("\t" + v.getNombre() + "  DW ?");
            }
        }

        private bool existeVariable(string nombre)
        {
            foreach (Variable v in variables)
            {
                if (v.getNombre().Equals(nombre))
                {
                    return true;
                }
            }
            return false;
        }
        private void modVariable(string nombre, float nuevoValor)
        {
            foreach (Variable v in variables) //Cambio que vamos a realizar
            {
                if (nombre == v.getNombre())
                {
                    v.setValor(nuevoValor);
                }
            }
        }

        private float getValor(string nombreVariable)
        {
            foreach (Variable v in variables) //Cambio que vamos a realizar
            {
                if (nombreVariable == v.getNombre())
                {
                    return v.getValor();
                }
            }
            return 0;
        }

        private Variable.TipoDato getTipo(string nombreVariable)
        {
            foreach (Variable v in variables) //Cambio que vamos a realizar
            {
                if (nombreVariable == v.getNombre())
                {
                    return v.getTipo();
                }
            }
            return Variable.TipoDato.Char;
        }

        //Programa  -> Librerias? Variables? Main
        public void Programa()
        {
            asm.WriteLine("#make_COM#");
            asm.WriteLine("include 'emu8086.inc'");
            asm.WriteLine("ORG 100H");
            Libreria();
            Variables();
            variableASM();
            Main();
            displayVariables();
            asm.WriteLine("RET");
            asm.WriteLine("DEFINE_PRINT_NUM");
            asm.WriteLine("DEFINE_PRINT_NUM_UNS");
            asm.WriteLine("DEFINE_SCAN_NUM");
            asm.WriteLine("END");
        }

        //Librerias -> #include<identificador(.h)?> Librerias?
        private void Libreria()
        {
            if (getContenido() == "#")
            {
                match("#");
                match("include");
                match("<");
                match(Tipos.Identificador);
                if (getContenido() == ".")
                {
                    match(".");
                    match("h");
                }
                match(">");
                Libreria();
            }
        }

        //Variables -> tipo_dato Lista_identificadores; Variables?
        private void Variables()
        {
            if (getClasificacion() == Tipos.TipoDato)
            {
                Variable.TipoDato tipo = Variable.TipoDato.Char;
                switch (getContenido())
                {
                    case "int": tipo = Variable.TipoDato.Int; break;
                    case "float": tipo = Variable.TipoDato.Float; break;
                }
                match(Tipos.TipoDato);
                Lista_identificadores(tipo);
                match(Tipos.FinSentencia);
                Variables();
            }
        }

        //Lista_identificadores -> identificador (,Lista_identificadores)?
        private void Lista_identificadores(Variable.TipoDato tipo)
        {
            if (getClasificacion() == Tipos.Identificador)
            {
                if (!existeVariable(getContenido()))
                {
                    addVariable(getContenido(), tipo);
                }
                else
                {
                    throw new Error("Error de sintaxis, variable duplicada <" + getContenido() + "> en linea: " + linea, log);
                }
            }
            match(Tipos.Identificador);
            if (getContenido() == ",")
            {
                match(",");
                Lista_identificadores(tipo);
            }
        }

        //Main      -> void main() Bloque de instrucciones
        private void Main()
        {
            match("void");
            match("main");
            match("(");
            match(")");
            BloqueInstrucciones(true, true);
        }

        //Bloque de instrucciones -> {listaIntrucciones?}
        private void BloqueInstrucciones(bool evaluacion, bool verifica)
        {
            match("{");
            if (getContenido() != "}")
            {
                ListaInstrucciones(evaluacion, verifica);
            }
            match("}");
        }

        //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones(bool evaluacion, bool verifica)
        {
            Instruccion(evaluacion, verifica);
            if (getContenido() != "}")
            {
                ListaInstrucciones(evaluacion, verifica);
            }
        }

        //ListaInstruccionesCase -> Instruccion ListaInstruccionesCase?
        private void ListaInstruccionesCase(bool evaluacion, bool verifica)
        {
            Instruccion(evaluacion, verifica);
            if (getContenido() != "case" && getContenido() != "break" && getContenido() != "default" && getContenido() != "}")
            {
                ListaInstruccionesCase(evaluacion, verifica);
            }
        }

        //Instruccion -> Printf | Scanf | If | While | do while | For | Switch | Asignacion
        private void Instruccion(bool evaluacion, bool verifica)
        {
            if (getContenido() == "printf")
            {
                Printf(evaluacion, verifica);
                //wasm.WriteLine("\tCALL PRINTF");
            }
            else if (getContenido() == "scanf")
            {
                Scanf(evaluacion, verifica);
            }
            else if (getContenido() == "if")
            {
                If(evaluacion, verifica);
            }
            else if (getContenido() == "while")
            {
                While(evaluacion, verifica);
            }
            else if (getContenido() == "do")
            {
                Do(evaluacion, verifica);
            }
            else if (getContenido() == "for")
            {
                For(evaluacion, verifica);
            }
            else if (getContenido() == "switch")
            {
                Switch(evaluacion, verifica);
            }
            else
            {
                Asignacion(evaluacion, verifica);
            }
        }

        private Variable.TipoDato evaluaNumero(float resultado)
        {
            if (resultado % 1 != 0)
            {
                return Variable.TipoDato.Float;
            }
            if (resultado <= 255)
            {
                return Variable.TipoDato.Char;
            }
            else
            {
                if (resultado <= 65535)
                {
                    return Variable.TipoDato.Int;
                }
            }
            return Variable.TipoDato.Float;
        }

        private bool evaluaSemantica(string variable, float resultado)
        {
            Variable.TipoDato tipoDato = getTipo(variable);
            return false;
        }

        //Asignacion -> identificador = cadena | Expresion;

        private void Asignacion(bool evaluacion, bool verifica)
        {
            string nuevaVariable = getContenido();
            if (existeVariable(nuevaVariable) != true)
            { //Utilizamos la funcion de ExisteVariable, pues regresa true o false
                throw new Error("\nLa variable " + nuevaVariable + " no se ha declarado en la cabecera\n", log);
            }
            log.WriteLine();
            log.Write(getContenido() + " = ");
            string nombre = getContenido();
            match(Tipos.Identificador);
            dominante = Variable.TipoDato.Char;
            if (getClasificacion() == Tipos.IncrementoTermino || getClasificacion() == Tipos.IncrementoFactor)
            {
                //Requerimiento 1 b)
                modVariable(nombre, Incremento(evaluacion, nombre, verifica));
                match(";");
                //Requerimiento 1 c)
                //El destructor se programo en la parte de arriba
            }
            else
            {
                match(Tipos.Asignacion);
                Expresion(verifica);
                match(";");
                float resultado = stack.Pop();
                if (verifica)
                {
                    asm.WriteLine("POP AX");
                }
                log.Write("= " + resultado);
                log.WriteLine();
                if (dominante < evaluaNumero(resultado))
                {
                    dominante = evaluaNumero(resultado);
                }
                if (dominante <= getTipo(nombre))
                {
                    if (evaluacion)
                    {
                        modVariable(nombre, resultado);
                    }
                }
                else
                {
                    throw new Error("Error de semantica: no podemos asignar un: <" + dominante + "> a un <" + getTipo(nombre) + "> en linea  " + linea, log);
                }
                //Requerimiento 3 a) Verificamos que el dato obtenido sea un char para el casteo
                if (getTipo(nuevaVariable) == Variable.TipoDato.Char)
                {
                    if (verifica)
                    {
                        asm.WriteLine("MOV AH, 0");
                    }
                }
                if (verifica)
                {
                    asm.WriteLine("MOV " + nombre + ", AX");
                }
            }
        }

        //While -> while(Condicion) bloque de instrucciones | instruccion
        private void While(bool evaluacion, bool verifica)
        {
            //Requerimiento 2 c) Hacemos lo mismo que el For, guardamos posiciones para regresar en el documento
            //float resIncremento = 0;
            match("while");
            match("(");
            bool ValidarWhile = true;
            int posicionAux = posicion;
            int lineaAux = linea;
            int tamañoAux = getContenido().Length;
            string contenidoAux = getContenido();
            do
            {
                ValidarWhile = Condicion("", verifica);
                if (evaluacion == false)
                {
                    ValidarWhile = false;
                }
                match(")");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(ValidarWhile, verifica);
                }
                else
                {
                    Instruccion(ValidarWhile, verifica);
                }
                if (ValidarWhile == true)
                {
                    //modVariable(contenidoAux, resIncremento);
                    posicion = posicionAux - tamañoAux;
                    linea = lineaAux;
                    setPosicion(posicion);
                    NextToken();
                }
                verifica = false;
            } while (ValidarWhile == true);
        }

        //Do -> do bloque de instrucciones | intruccion while(Condicion)
        private void Do(bool evaluacion, bool verifica)
        {
            //Requerimiento 2 c) Hacemos lo mismo que el For, guardamos posiciones para regresar en el documento
            float resIncremento = 0;
            int posicionAux = posicion;
            int lineaAux = linea;
            int tamañoAux = getContenido().Length;
            string contenidoAux = getContenido();
            bool validarDo = true;
            if (evaluacion == false)
            {
                validarDo = false;
            }
            match("do");
            if (getContenido() == "{")
            {
                BloqueInstrucciones(validarDo, verifica);
            }
            else
            {
                Instruccion(validarDo, verifica);
            }
            match("while");
            match("(");
            validarDo = Condicion("", verifica);
            match(")");
            match(";");
            if (validarDo == true)
            {
                modVariable(contenidoAux, resIncremento);
                posicion = posicionAux - tamañoAux;
                linea = lineaAux;
                setPosicion(posicion);
                NextToken();
            }
        }
        //For -> for(Asignacion Condicion; Incremento) BloqueInstruccones | Intruccion 
        private void For(bool evaluacion, bool verifica)
        {
            verifica = true;
            float resIncremento = 0;
            if (verifica)
            {
                ++cFor;
            }
            string etiquetaInicioFor = "inicioFor" + cFor;
            string etiquetaFinFor = "finFor" + cFor;
            match("for");
            match("(");
            Asignacion(evaluacion, verifica);
            int posicionAux = posicion;
            int lineaAux = linea;
            int tamañoAux = getContenido().Length;
            string contenidoAux = getContenido();
            string contenidoGlob = "";
            bool validarFor;
            do
            {
                if (verifica)
                {
                    asm.WriteLine(etiquetaInicioFor + ":");
                }
                validarFor = Condicion(etiquetaFinFor, verifica);
                if (evaluacion == false)
                {
                    validarFor = false;
                }
                match(";");
                //Requerimiento 2 a)
                //Realizar un switch case para validar si supera el rango permitido de int, float, char
                match(Tipos.Identificador);
                resIncremento = Incremento(validarFor, contenidoAux, verifica);
                contenidoGlob = IncASM;
                match(")");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(validarFor, verifica);
                }
                else
                {
                    Instruccion(validarFor, verifica);
                }
                if (validarFor == true)
                {
                    modVariable(contenidoAux, resIncremento);
                    posicion = posicionAux - tamañoAux;
                    linea = lineaAux;
                    setPosicion(posicion);
                    NextToken();
                }
                if (verifica)
                {
                    asm.WriteLine(contenidoGlob);
                    asm.WriteLine("JMP " + etiquetaInicioFor);
                    asm.WriteLine(etiquetaFinFor + ":");
                }
                verifica = false;
            } while (validarFor);
        }

        private void setPosicion(int posicion)
        {
            archivo.DiscardBufferedData();
            archivo.BaseStream.Seek(posicion, SeekOrigin.Begin);
        }

        //Incremento -> Identificador ++ | --
        private float Incremento(bool evaluacion, string variable, bool verifica)
        {
            float varMod = getValor(variable);
            float resultado = 0;
            if (existeVariable(variable) != true)
            {
                throw new Error("\nLa variable " + variable + " no se ha declarado en la cabecera\n", log);
            }
            //Requerimiento 1 b) Programar casos de ++, --, +=, -=, *=, /=
            switch (getContenido())
            {
                case "++":
                    if (verifica)
                    {
                        IncASM = "INC " + variable;
                    }
                    if (evaluacion)
                    {
                        varMod++;

                    }
                    match("++");
                    break;
                case "--":
                    if (verifica)
                    {
                        IncASM = "DEC " + variable;
                    }
                    if (evaluacion)
                    {
                        varMod--;

                    }
                    match("--");
                    break;
                case "+=":  //Revisar salida de datos, para que se imprima el resultado sobre si mismo
                    match("+=");
                    Expresion(verifica);
                    resultado = stack.Pop();
                    if (verifica)
                    {
                        IncASM = "POP AX ";
                        IncASM += "\nADD " + variable + ", AX";
                    }
                    if (evaluacion)
                    {
                        varMod += resultado;

                    }
                    break;
                case "-=":
                    match("-=");
                    Expresion(verifica);
                    resultado = stack.Pop();
                    if (verifica)
                    {
                        IncASM = "POP AX ";
                        IncASM += "\nSUB " + variable + ", AX";
                    }
                    if (evaluacion)
                    {
                        varMod -= resultado;
                    }
                    break;
                case "*=":
                    match("*=");
                    Expresion(verifica);
                    resultado = stack.Pop();
                    if (verifica)
                    {
                        IncASM = "POP AX";
                        IncASM += "\nMUL " + variable;
                        IncASM += "\nMOV " + variable + ", AX";
                    }
                    if (evaluacion)
                    {
                        varMod *= resultado;
                    }
                    break;
                case "/=":
                    match("/=");
                    Expresion(verifica);
                    resultado = stack.Pop();
                    if (verifica)
                    {
                        IncASM = "POP BX";
                        IncASM += "\nMOV AX, " + variable;
                        IncASM += "\nDIV BX";
                        IncASM += "\nMOV " + variable + ", AX";
                    }
                    if (evaluacion)
                    {
                        varMod /= resultado;
                    }
                    break;
                case "%=":
                    match("%=");
                    Expresion(verifica);
                    resultado = stack.Pop();
                    if (verifica)
                    {
                        IncASM = "POP BX";
                        IncASM += "\nMOV AX, " + variable;
                        IncASM += "\nDIV BX";
                        IncASM += "\nMOV " + variable + ", DX";
                    }
                    if (evaluacion)
                    {
                        varMod %= resultado;
                    }
                    break;
                default:
                    throw new Error("Error de sintaxis: Se espera un incremento en linea " + linea, log);
            }
            if (getTipo(variable) < dominante)
            {
                throw new Error("Error de sintaxis: No se puede asignar un " + dominante + " a un " + getTipo(variable) + " en linea " + linea, log);
            }
            if (getTipo(variable) == Variable.TipoDato.Char && varMod > 255)
            {
                throw new Error("Error de semantica: el valor sobrepasa el limite en linea  " + linea, log);
            }
            else if (getTipo(variable) == Variable.TipoDato.Int && varMod > 65535)
            {
                throw new Error("Error de semantica: el valor sobrepasa el limite en linea  " + linea, log);
            }
            return varMod;
        }

        //Switch -> switch (Expresion) {Lista de casos} | (default: )
        private void Switch(bool evaluacion, bool verifica)
        {
            match("switch");
            match("(");
            Expresion(verifica);
            stack.Pop();
            if (verifica)
            {
                asm.WriteLine("POP AX");
            }
            match(")");
            match("{");
            ListaDeCasos(evaluacion, verifica);
            if (getContenido() == "default")
            {
                match("default");
                match(":");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(evaluacion, verifica);
                }
                else
                {
                    Instruccion(evaluacion, verifica);
                }
            }
            match("}");
        }

        //ListaDeCasos -> case Expresion: listaInstruccionesCase (break;)? (ListaDeCasos)?
        private void ListaDeCasos(bool evaluacion, bool verifica)
        {
            match("case");
            Expresion(verifica);
            stack.Pop();
            if (verifica)
            {
                asm.WriteLine("POP AX");
            }
            match(":");
            ListaInstruccionesCase(evaluacion, verifica);
            if (getContenido() == "break")
            {
                match("break");
                match(";");
            }
            if (getContenido() == "case")
            {
                ListaDeCasos(evaluacion, verifica);
            }
        }

        //Condicion -> Expresion operador relacional Expresion
        private bool Condicion(string etiqueta, bool verifica)
        {
            Expresion(verifica);
            string operador = getContenido();
            match(Tipos.OperadorRelacional);
            Expresion(verifica);
            float e2 = stack.Pop();
            if (verifica)
            {
                asm.WriteLine("POP BX");
            }
            float e1 = stack.Pop();
            if (verifica)
            {
                asm.WriteLine("POP AX");
            }
            if (verifica)
            {
                asm.WriteLine("CMP AX, BX");
            }
            switch (operador)
            {
                case "==":
                    if (verifica)
                    {
                        asm.WriteLine("JNE " + etiqueta);
                    }
                    return e1 == e2;
                case ">":
                    if (verifica)
                    {
                        asm.WriteLine("JLE " + etiqueta);
                    }
                    return e1 > e2;
                case ">=":
                    if (verifica)
                    {
                        asm.WriteLine("JL " + etiqueta);
                    }
                    return e1 >= e2;
                case "<":
                    if (verifica)
                    {
                        asm.WriteLine("JGE " + etiqueta);
                    }
                    return e1 < e2;
                case "<=":
                    if (verifica)
                    {
                        asm.WriteLine("JG " + etiqueta);
                    }
                    return e1 <= e2;
                default:
                    if (verifica)
                    {
                        asm.WriteLine("JE " + etiqueta);
                    }
                    return e1 != e2;
            }
        }

        //If -> if(Condicion) bloque de instrucciones (else bloque de instrucciones)?
        private void If(bool evaluacion, bool verifica)
        {
            if (verifica)
            {
                cIf++;
            }
            string etiquetaIf = "if" + cIf;
            string finIf = "else" + cIf;
            match("if");
            match("(");
            bool validarIf = Condicion(etiquetaIf, verifica);
            if (evaluacion == false)
            {
                validarIf = false;
            }
            match(")");
            if (getContenido() == "{")
            {
                BloqueInstrucciones(validarIf, verifica);
            }
            else
            {
                Instruccion(validarIf, verifica);
            }
            if (verifica)
            {
                asm.WriteLine("JMP " + finIf);
                asm.WriteLine(etiquetaIf + ":");
            }

            if (getContenido() == "else")
            {
                match("else");
                if (getContenido() == "{")
                {
                    if (evaluacion == true)
                    {
                        BloqueInstrucciones(!validarIf, verifica);
                    }
                    else
                    {
                        BloqueInstrucciones(false, verifica);
                    }
                }
                else
                {
                    if (evaluacion == true)
                    {
                        Instruccion(!validarIf, verifica);
                    }
                    else
                    {
                        Instruccion(false, verifica);
                    }
                }
            }
            if (verifica)
            {
                asm.WriteLine(finIf + ":");
            }
        }

        //Printf -> printf(cadena o expresion);
        private void Printf(bool evaluacion, bool verifica)
        {
            match("printf");
            match("(");
            if (getClasificacion() == Tipos.Cadena)
            {
                setContenido(getContenido().Replace("\"", ""));
                setContenido(getContenido().Replace("\\n", "\n"));
                setContenido(getContenido().Replace("\\t", "     "));
                if (evaluacion)
                {
                    Console.Write(getContenido());
                }
                if (verifica)
                {
                    asm.WriteLine("PRINTN \"" + getContenido() + "\"");
                }
                match(Tipos.Cadena);
            }
            else
            {
                Expresion(verifica);
                float resultado = stack.Pop();
                //Requerimiento 3 c)
                if (verifica)
                {
                    asm.WriteLine("POP AX");
                    asm.WriteLine("CALL PRINT_NUM");
                }
                if (evaluacion)
                {
                    Console.Write(resultado);
                }
            }
            match(")");
            match(";");
        }

        //Scanf -> scanf(cadena, & Identificador);
        private void Scanf(bool evaluacion, bool verifica)
        {
            match("scanf");
            match("(");
            match(Tipos.Cadena);
            match(",");
            match("&");
            string variable = getContenido();
            if (existeVariable(variable) != true) //Utilizamos la funcion de ExisteVariable, pues regresa true o false
            {
                throw new Error("\nLa variable " + variable + " no se ha declarado en la cabecera\n", log);
            }
            if (evaluacion)
            {
                string val = "" + Console.ReadLine();
                if (float.TryParse(val, out float valor))
                {
                    modVariable(variable, valor);
                }
                else
                {
                    throw new Error("\nEl valor ingresado no es un numero\n", log);
                }
                if (verifica)
                {
                    asm.WriteLine("CALL SCAN_NUM");
                    asm.WriteLine("MOV " + variable + ", CX");
                }
            }
            match(Tipos.Identificador);
            match(")");
            match(";");
        }

        //Expresion -> Termino MasTermino
        private void Expresion(bool verifica)
        {
            Termino(verifica);
            MasTermino(verifica);
        }
        //MasTermino -> (OperadorTermino Termino)?
        private void MasTermino(bool verifica)
        {
            if (getClasificacion() == Tipos.OperadorTermino)
            {
                string operador = getContenido();
                match(Tipos.OperadorTermino);
                Termino(verifica);
                log.Write(operador + " ");
                float n1 = stack.Pop();
                if (verifica)
                {
                    asm.WriteLine("POP BX");
                }
                float n2 = stack.Pop();
                if (verifica)
                {
                    asm.WriteLine("POP AX");
                }
                switch (operador)
                {
                    case "+":
                        stack.Push(n2 + n1);
                        if (verifica)
                        {
                            asm.WriteLine("ADD AX, BX");
                            asm.WriteLine("PUSH AX");
                        }
                        break;
                    case "-":
                        stack.Push(n2 - n1);
                        if (verifica)
                        {
                            asm.WriteLine("SUB AX, BX");
                            asm.WriteLine("PUSH AX");
                        }
                        break;
                }
            }
        }
        //Termino -> Factor PorFactor
        private void Termino(bool verifica)
        {
            Factor(verifica);
            PorFactor(verifica);
        }
        //PorFactor -> (OperadorFactor Factor)? 
        private void PorFactor(bool verifica)
        {
            if (getClasificacion() == Tipos.OperadorFactor)
            {
                string operador = getContenido();
                match(Tipos.OperadorFactor);
                Factor(verifica);
                log.Write(operador + " ");
                float n1 = stack.Pop();
                if (verifica)
                {
                    asm.WriteLine("POP BX");
                }
                float n2 = stack.Pop();
                if (verifica)
                {
                    asm.WriteLine("POP AX");
                }
                //Requerimiento 1 a) LISTO
                switch (operador)
                {
                    case "*":
                        stack.Push(n2 * n1);
                        if (verifica)
                        {
                            asm.WriteLine("MUL BX");
                            asm.WriteLine("PUSH AX");
                        }
                        break;
                    case "/":
                        stack.Push(n2 / n1);
                        if (verifica)
                        {
                            asm.WriteLine("DIV BX");
                            asm.WriteLine("PUSH AX");
                        }
                        break;
                    //Requerimiento 1 a) Programar caso de residuo de factor
                    case "%":
                        stack.Push(n2 % n1);
                        if (verifica)
                        {
                            asm.WriteLine("DIV BX");
                            asm.WriteLine("PUSH DX");
                        }
                        break;
                }
            }
        }

        private float Convertir(float valor, Variable.TipoDato dominante)
        {
            if (dominante == Variable.TipoDato.Char)
            {
                valor = (char)valor % 256;
                return valor;
            }
            else if (dominante == Variable.TipoDato.Int)
            {
                valor = (int)valor % 65536;
                return valor;
            }
            else
            {
                return valor;
            }
        }

        //Factor -> numero | identificador | (Expresion)
        private void Factor(bool verifica)
        {
            if (getClasificacion() == Tipos.Numero)
            {
                log.Write(getContenido() + " ");
                if (dominante < evaluaNumero(float.Parse(getContenido())))
                {
                    dominante = evaluaNumero(float.Parse(getContenido()));
                }
                stack.Push(float.Parse(getContenido()));
                if (verifica)
                {
                    asm.WriteLine("MOV AX, " + getContenido());
                    asm.WriteLine("PUSH AX");
                }
                match(Tipos.Numero);
            }
            else if (getClasificacion() == Tipos.Identificador)
            {
                string variable = getContenido();
                if (existeVariable(variable) != true)
                { //Utilizamos la funcion de ExisteVariable, pues regresa true o false
                    throw new Error("\nLa variable " + variable + " no se ha declarado en la cabecera\n", log);
                }
                log.Write(getContenido() + " ");
                if (dominante < getTipo(getContenido()))
                {
                    dominante = getTipo(getContenido());
                }
                stack.Push(getValor(getContenido()));
                //Requerimiento 3 a)
                if (verifica)
                {
                    asm.WriteLine("MOV AX, " + getContenido());
                    asm.WriteLine("PUSH AX");
                }
                match(Tipos.Identificador);
            }
            else
            {
                bool huboCasteo = false;
                Variable.TipoDato casteo = Variable.TipoDato.Char;
                match("(");
                if (getClasificacion() == Tipos.TipoDato)
                {
                    huboCasteo = true;
                    switch (getContenido())
                    {
                        case "char":
                            casteo = Variable.TipoDato.Char;
                            break;
                        case "int":
                            casteo = Variable.TipoDato.Int;
                            break;
                        case "float":
                            casteo = Variable.TipoDato.Float;
                            break;
                    }
                    match(Tipos.TipoDato);
                    match(")");
                    match("(");
                }
                Expresion(verifica);
                match(")");
                if (huboCasteo)
                {
                    dominante = casteo;
                    float valorGuardado = stack.Pop();
                    valorGuardado = Convertir(valorGuardado, dominante);
                    stack.Push(valorGuardado);
                }
            }
        }
    }
}