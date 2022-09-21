using System;
using System.Collections.Generic;
//Requerimiento 1.- Eliminar las dobles comillas del printf e interpretar las secuencias de escape
//                  dentro de la cadena LISTO
//Requerimiento 2.- Marcar los errores sintacticos cuando la variable no exista LISTO
//Requerimiento 3.- Modificar el valor de la variable en la asignacion (linea 51, aqui mismo) LISTO
//Requerimiento 4.- Obtener el valor de la variable cuando se requiera y programar el metodo getValor() LISTO
//Requerimiento 5.- Modificar el valor de la variable en el scanf LISTO
namespace semantica
{
    public class Lenguaje : Sintaxis
    {
        List <Variable> variables = new List<Variable>();
        Stack<float> stack = new Stack<float>();

        Variable.TipoDato dominante;

        public Lenguaje()
        {

        }
        public Lenguaje(string nombre) : base(nombre)
        {

        }

        private void addVariable(String nombre,Variable.TipoDato tipo)
        {
            variables.Add(new Variable(nombre, tipo));
        }

        private void displayVariables()
        {
            log.WriteLine();
            log.WriteLine("variables: ");
            foreach (Variable v in variables)
            {
                log.WriteLine(v.getNombre()+" "+v.getTipo()+" "+v.getValor());
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
                if (nombre == v.getNombre()){
                    v.setValor(nuevoValor);
                }
            }
        }

        private float getValor(string nombreVariable)
        {
            foreach (Variable v in variables) //Cambio que vamos a realizar
            {
                if (nombreVariable == v.getNombre()){
                    return v.getValor();
                }
            }
            return 0;
        }

        private Variable.TipoDato getTipo(string nombreVariable)
        {
            foreach (Variable v in variables) //Cambio que vamos a realizar
            {
                if (nombreVariable == v.getNombre()){
                    return v.getTipo();
                }
            }
            return Variable.TipoDato.Char;
        }

        //Programa  -> Librerias? Variables? Main
        public void Programa()
        {
            Libreria();
            Variables();
            Main();
            displayVariables();
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
                    throw new Error("Error de sintaxis, variable duplicada <" +getContenido()+"> en linea: "+linea, log);
                }
            }
            match(Tipos.Identificador);
            if (getContenido() == ",")
            {
                match(",");
                Lista_identificadores(tipo);
            }
        }
        //Bloque de instrucciones -> {listaIntrucciones?}
        private void BloqueInstrucciones()
        {
            match("{");
            if (getContenido() != "}")
            {
                ListaInstrucciones();
            }    
            match("}"); 
        }

        //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones()
        {
            Instruccion();
            if (getContenido() != "}")
            {
                ListaInstrucciones();
            }
        }

        //ListaInstruccionesCase -> Instruccion ListaInstruccionesCase?
        private void ListaInstruccionesCase()
        {
            Instruccion();
            if (getContenido() != "case" && getContenido() !=  "break" && getContenido() != "default" && getContenido() != "}")
            {
                ListaInstruccionesCase();
            }
        }

        //Instruccion -> Printf | Scanf | If | While | do while | For | Switch | Asignacion
        private void Instruccion()
        {
            if (getContenido() == "printf")
            {
                Printf();
            }
            else if (getContenido() == "scanf")
            {
                Scanf();
            }
            else if (getContenido() == "if")
            {
                If();
            }
            else if (getContenido() == "while")
            {
                While();
            }
            else if(getContenido() == "do")
            {
                Do();
            }
            else if(getContenido() == "for")
            {
                For();
            }
            else if(getContenido() == "switch")
            {
                Switch();
            }
            else
            {
                Asignacion();
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
                if (resultado <= 65535){
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
        private void Asignacion()
        {
            //Requerimiento 2.- Si no existe la variable levanta la excepcion
            string nuevaVariable = getContenido();
            if (existeVariable(nuevaVariable) != true){ //Utilizamos la funcion de ExisteVariable, pues regresa true o false
                throw new Error("\nLa variable " + nuevaVariable + " no se ha declarado en la cabecera\n", log);
            }
            log.WriteLine();
            log.Write(getContenido()+" = ");
            string nombre = getContenido();
            match(Tipos.Identificador);
            match(Tipos.Asignacion);
            dominante = Variable.TipoDato.Char;
            Expresion();
            match(";");
            float resultado = stack.Pop();
            log.Write("= " + resultado);
            log.WriteLine();
            if (dominante < evaluaNumero(resultado))
            {
                dominante = evaluaNumero(resultado);
            }
            if ( dominante <= getTipo(nombre))
            {
                modVariable(nombre, resultado);
            } 
            else
            {
                throw new Error("Error de semantica: no podemos asignar un: <" + dominante + "> a un <" + getTipo(nombre) +  "> en linea  " + linea, log);
            }
        }

        //While -> while(Condicion) bloque de instrucciones | instruccion
        private void While()
        {
            match("while");
            match("(");
            Condicion();
            match(")");
            if (getContenido() == "{") 
            {
                BloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }
        }

        //Do -> do bloque de instrucciones | intruccion while(Condicion)
        private void Do()
        {
            match("do");
            if (getContenido() == "{")
            {
                BloqueInstrucciones();
            }
            else
            {
                Instruccion();
            } 
            match("while");
            match("(");
            Condicion();
            match(")");
            match(";");
        }
        //For -> for(Asignacion Condicion; Incremento) BloqueInstruccones | Intruccion 
        private void For()
        {
            match("for");
            match("(");
            Asignacion();
            Condicion();
            match(";");
            Incremento();
            match(")");
            if (getContenido() == "{")
            {
                BloqueInstrucciones();  
            }
            else
            {
                Instruccion();
            }
        }

        //Incremento -> Identificador ++ | --
        private void Incremento()
        {
            string variable = getContenido();
            //Requerimiento 2.- Si no existe la variable levanta la excepcion
            if (existeVariable(variable) != true){ //Utilizamos la funcion de ExisteVariable, pues regresa true o false
                throw new Error("\nLa variable " + variable + " no se ha declarado en la cabecera\n", log);
            }
            match(Tipos.Identificador);
            if(getContenido() == "++")
            {
                modVariable(variable, getValor(variable)+1);
                match("++");
            }
            else
            {
                modVariable(variable, getValor(variable)-1);
                match("--");
            }
        }

        //Switch -> switch (Expresion) {Lista de casos} | (default: )
        private void Switch()
        {
            match("switch");
            match("(");
            Expresion();
            stack.Pop();
            match(")");
            match("{");
            ListaDeCasos();
            if(getContenido() == "default")
            {
                match("default");
                match(":");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones();  
                }
                else
                {
                    Instruccion();
                }
            }
            match("}");
        }

        //ListaDeCasos -> case Expresion: listaInstruccionesCase (break;)? (ListaDeCasos)?
        private void ListaDeCasos()
        {
            match("case");
            Expresion();
            stack.Pop();
            match(":");
            ListaInstruccionesCase();
            if(getContenido() == "break")
            {
                match("break");
                match(";");
            }
            if(getContenido() == "case")
            {
                ListaDeCasos();
            }
        }

        //Condicion -> Expresion operador relacional Expresion
        private void Condicion()
        {
            Expresion();
            stack.Pop();
            match(Tipos.OperadorRelacional);
            Expresion();
            stack.Pop();
        }

        //If -> if(Condicion) bloque de instrucciones (else bloque de instrucciones)?
        private void If()
        {
            match("if");
            match("(");
            Condicion();
            match(")");
            if (getContenido() == "{")
            {
                BloqueInstrucciones();  
            }
            else
            {
                Instruccion();
            }
            if (getContenido() == "else")
            {
                match("else");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones();
                }
                else
                {
                    Instruccion();
                }
            }
        }

        //Printf -> printf(cadena o expresion);
        private void Printf()
        {
            match("printf");
            match("(");
            if (getClasificacion() == Tipos.Cadena)
            {
                //Requerimiento 1.- Aqui se eliminan las comillas del resultado y las secuencias de escape
                //Se validan para cada una de los casos requeridos, se utiliza una \ para los metacaracteres especificados
                setContenido(getContenido().Replace("\"", ""));
                setContenido(getContenido().Replace("\\n", "\n"));
                setContenido(getContenido().Replace("\\t", "     "));
                Console.Write(getContenido());
                match(Tipos.Cadena);
            }
            else
            {
                Expresion();
                Console.Write(stack.Pop());
            }
            match(")");
            match(";");
        }

        //Scanf -> scanf(cadena, & Identificador);
        private void Scanf()    
        {
            match("scanf");
            match("(");
            match(Tipos.Cadena);
            match(",");
            match("&");
            //Requerimiento 2.- Si no existe la variable levanta la excepcion
            string variable = getContenido();
             if (existeVariable(variable) != true){ //Utilizamos la funcion de ExisteVariable, pues regresa true o false
                throw new Error("\nLa variable " + variable + " no se ha declarado en la cabecera\n", log);
            }
            string val = "" + Console.ReadLine();
            //Requerimiento 5.- Modificar el valor de la variable en el Scanf
            //Hacemos el parseo de val, de string a float, para poder utilizarlo en el metodo modVariable
            float nuevaVal = float.Parse(val);
            modVariable(variable, nuevaVal);
            match(Tipos.Identificador);
            match(")");
            match(";");
        }

        //Main      -> void main() Bloque de instrucciones
        private void Main()
        {
            match("void");
            match("main");
            match("(");
            match(")");
            BloqueInstrucciones();
        }

        //Expresion -> Termino MasTermino
        private void Expresion()
        {
            Termino();
            MasTermino();
        }
        //MasTermino -> (OperadorTermino Termino)?
        private void MasTermino()
        {
            if (getClasificacion() == Tipos.OperadorTermino)
            {
                string operador = getContenido();
                match(Tipos.OperadorTermino);
                Termino();
                log.Write(operador + " ");
                float n1 = stack.Pop();
                float n2 = stack.Pop();
                switch (operador)
                {
                    case "+":
                        stack.Push(n2 + n1);
                        break;
                    case "-":
                        stack.Push(n2 - n1);
                        break;
                }
            }
        }
        //Termino -> Factor PorFactor
        private void Termino()
        {
            Factor();
            PorFactor();
        }
        //PorFactor -> (OperadorFactor Factor)? 
        private void PorFactor()
        {
            if (getClasificacion() == Tipos.OperadorFactor)
            {
                string operador = getContenido();
                match(Tipos.OperadorFactor);
                Factor();
                log.Write(operador + " ");
                float n1 = stack.Pop();
                float n2 = stack.Pop();
                switch (operador)
                {
                    case "*":
                        stack.Push(n2 * n1);
                        break;
                    case "/":
                        stack.Push(n2 / n1);
                        break;
                }
            }
        }
        //Factor -> numero | identificador | (Expresion)
        private void Factor()
        {
            if (getClasificacion() == Tipos.Numero)
            {
                log.Write(getContenido() + " " );
                if (dominante < evaluaNumero(float.Parse(getContenido())))
                {
                    dominante = evaluaNumero(float.Parse(getContenido()));
                }
                stack.Push(float.Parse(getContenido()));
                match(Tipos.Numero);
            }
            else if (getClasificacion() == Tipos.Identificador)
            {
                //Requerimiento 2.- Si no existe la variable levanta la excepcion
                string variable = getContenido();
                if (existeVariable(variable) != true){ //Utilizamos la funcion de ExisteVariable, pues regresa true o false
                    throw new Error("\nLa variable " + variable + " no se ha declarado en la cabecera\n", log);
                }
                log.Write(getContenido() + " " );
                stack.Push(getValor(getContenido()));
                match(Tipos.Identificador);
            }
            else
            {
                match("(");
                Expresion();
                match(")");
            }
        }
    }
}