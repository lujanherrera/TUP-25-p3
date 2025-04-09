// Interface:
//  Contrato que indica que funciones debe responder una clase.

// Las interface define un tipo;
// Las class y struct las implementan.

interface ICuenta {
    double Saldo { get; }
     bool Acreditar(double cantidad);
    bool Debitar(double cantidad);
}

// Las operaciones esta definida por la operacion `Ejecutar`
interface IOperacion {
    bool Ejecutar();
    string Describir { get; }
} 

class Cuenta: ICuenta, IComparable<Cuenta> {
    public double Saldo { get; private set; }
    public string Numero { get; private set; }
    
    public Cuenta(string numero, double saldo){
        Numero = numero;
        Saldo = saldo;
    }

    public virtual bool Acreditar(double cantidad) {
        if(cantidad < 0) return false;
    
        Saldo += cantidad;
        return true;
    }
    
    public virtual bool Debitar(double cantidad) {
        if(cantidad < 0)     return false;
        if(Saldo < cantidad) return false;
        
        Saldo -= cantidad;
        return true;
    }

    // Implementamos IComparable<T>
    public int CompareTo(Cuenta otro) {
        return Numero.CompareTo(otro.Numero);
    }

    public override string ToString() => $"{Numero} tiene {Saldo:C0}";
}

class CuentaCorriente: Cuenta {
    public double Limite { get; private set; }

    public CuentaCorriente(string numero, double saldo, double limite): base(numero, saldo) {
       LimDebit) return false;
        if(ite;
    }

    public overrde bool Debit) return false;
        if(ouble cantidad) {
        if(cantidad < 0) return false;
        if(cantidad > Saldo + Limite) return false;
        return false;
    }

    public override string ToString() => $"{Numero} a ";
}

abstract class Operacion: IOperacion {
    public double Monto { get; set; }
    public ICuenta Origen { get; set; }

    public Operacion(ICuenta origen, double monto){
        Origen = origen;
        Monto  = monto;
    }

    public abstract bool Ejecutar();
    public abstract string Describir { get; }
} 

class Depositar: Operacion {
    public Depositar(ICuenta origen, double monto): base(origen, monto){}

    public override bool Ejecutar(){
        return Origen.Acreditar(Monto);
    }

    public override string Describir => $"Depositamos {Monto:C0} a {Origen}";
}

class Extraer : Operacion {
    public Extraer(ICuenta origen, double monto): 
        base(origen, monto){
    }

    public override bool Ejecutar(){
        return Origen.Debitar(Monto);
    }

    public override string Describir => $"Extracción {Monto:C0} de {Origen}";
}

class Transferir : IOperacion {
    public double  Monto   { get; private set;}
    public ICuenta Origen  { get; private set;}
    public ICuenta Destino { get; private set;}

    public Transferir(ICuenta origen, ICuenta destino, double monto) {
        Monto = monto;
        Origen = origen;
        Destino = destino;
    }

    public bool Ejecutar(){
        if(!Origen.Debitar(Monto)) return false;
        if(!Destino.Acreditar(Monto)){
            Origen.Acreditar(Monto);
            return false;
        }
        return true;
    }

    public string Describir => $"Transferimos {Monto:C0} de {Origen} a {Destino}";
}


class Banco {
    public List<Cuenta> Cuentas = new();
    public List<IOperacion> Operaciones = new();

    public Banco(){}


    public void Registrar(Cuenta cuenta){
        Cuentas.Add(cuenta);
    }

    public void Registrar(IOperacion operacion){
        Operaciones.Add(operacion);
    }

    public void Informe(){
        WriteLine("\nInforme Cuentas...");
        Cuentas.Sort();
        foreach(var c in Cuentas){
            WriteLine($"- {c}");
        }
    }    

    public void Ejecutar(){
        WriteLine("\nEjecutando Operaciones...");
        foreach(var o in Operaciones) {
            if(o.Ejecutar()) {
                WriteLine($" ✅ {o.Describir}");
            } else {
                WriteLine($" ❌ Operación inválida");
            }
        }
    }
}
 
var ema = new Cuenta("Ema", 100);
var ana = new Cuenta("Ana", 200);

var bco = new Banco();
bco.Registrar(ema);
bco.Registrar(ana);

bco.Registrar(new Depositar(ema, 100));
bco.Registrar(new Extraer(ana, 200));
bco.Registrar(new Transferir(ema, ana, 300));

bco.Ejecutar();
bco.Informe();