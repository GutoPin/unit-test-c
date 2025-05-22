using System;
using System.Collections.Generic;
using System.Linq;
namespace Gimnasio;

public class Gimnasio
{
    private string nombre;
    private string direccion;
    private List<Sala> salas;
    private List<Instructor> instructores;
    private PoliticaCancelacion politicaCancelacion;
    private List<Socio> socios;

    public Gimnasio(string nombre, string direccion)
    {
        this.nombre = nombre;
        this.direccion = direccion;
        this.salas = new List<Sala>();
        this.instructores = new List<Instructor>();
        this.socios = new List<Socio>();
        this.politicaCancelacion = new PoliticaCancelacion(12, 3);
    }

    public List<Sala> Salas => salas;
    public List<Socio> Socios => socios;

    public void AgregarSala(Sala sala)
    {
        if (!salas.Contains(sala))
        {
            salas.Add(sala);
            sala.Gimnasio = this;
        }
    }

    public void AgregarInstructor(Instructor instructor)
    {
        if (!instructores.Contains(instructor))
        {
            instructores.Add(instructor);
            instructor.Gimnasio = this;
        }
    }

    public void RegistrarSocio(Socio socio)
    {
        if (!socios.Contains(socio))
        {
            socios.Add(socio);
        }
    }

    public bool PuedeImpartirClase(Instructor instructor, string tipoClase)
    {
        return instructores.Contains(instructor) && 
               instructor.PuedeImpartir(tipoClase);
    }

    // Propiedades y métodos adicionales
}

public class Sala
{
    public string Nombre { get; set; }
    public int Capacidad { get; set; }
    public List<Equipamiento> Equipamientos { get; set; }
    public Gimnasio Gimnasio { get; set; }
    public List<ClaseGrupal> Clases { get; set; }

    public Sala(string nombre, int capacidad)
    {
        Nombre = nombre;
        Capacidad = capacidad;
        Equipamientos = new List<Equipamiento>();
        Clases = new List<ClaseGrupal>();
    }

    public void AgregarEquipamiento(Equipamiento equipamiento)
    {
        if (!Equipamientos.Contains(equipamiento))
        {
            Equipamientos.Add(equipamiento);
            equipamiento.Sala = this;
        }
    }

    public void AgregarClase(ClaseGrupal clase)
    {
        if (!Clases.Contains(clase))
        {
            Clases.Add(clase);
            clase.Sala = this;
        }
    }

    public bool TieneEquipamientoNecesario(string tipoEquipamiento)
    {
        return Equipamientos.Any(e => e.Nombre.Equals(tipoEquipamiento, StringComparison.OrdinalIgnoreCase) && 
                                    !e.NecesitaMantenimiento());
    }
}

public class Equipamiento
{
    public string Nombre { get; set; }
    public string Tipo { get; set; }
    public DateTime? UltimoMantenimiento { get; set; }
    public Sala Sala { get; set; }

    public Equipamiento(string nombre, string tipo)
    {
        Nombre = nombre;
        Tipo = tipo;
    }

    public bool NecesitaMantenimiento()
    {
        return UltimoMantenimiento == null || 
               UltimoMantenimiento < DateTime.Now.AddMonths(-3);
    }

    public void RealizarMantenimiento()
    {
        UltimoMantenimiento = DateTime.Now;
    }
}

public class Instructor
{
    public string Nombre { get; set; }
    public string Especialidad { get; set; }
    public List<Certificacion> Certificaciones { get; set; }
    public Gimnasio Gimnasio { get; set; }

    public Instructor(string nombre, string especialidad)
    {
        Nombre = nombre;
        Especialidad = especialidad;
        Certificaciones = new List<Certificacion>();
    }

    public bool PuedeImpartir(string tipoClase)
    {
        return Certificaciones.Any(c => c.TipoClase.Equals(tipoClase, StringComparison.OrdinalIgnoreCase) && 
                                      c.EsVigente());
    }

    public void AgregarCertificacion(Certificacion certificacion)
    {
        if (!Certificaciones.Contains(certificacion))
        {
            Certificaciones.Add(certificacion);
            certificacion.Instructor = this;
        }
    }

    public bool EstaDisponible(DateTime fechaHora)
    {
        // Implementar lógica de disponibilidad
        return true;
    }
}

public class Certificacion
{
    public string TipoClase { get; set; }
    public DateTime FechaExpedicion { get; set; }
    public DateTime FechaExpiracion { get; set; }
    public Instructor Instructor { get; set; }

    public Certificacion(string tipoClase, int añosValidez)
    {
        TipoClase = tipoClase;
        FechaExpedicion = DateTime.Now;
        FechaExpiracion = FechaExpedicion.AddYears(añosValidez);
    }

    public bool EsVigente()
    {
        return DateTime.Now < FechaExpiracion;
    }
}

public class Socio
{
    public string Id { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Telefono { get; set; }
    public DateTime FechaInscripcion { get; set; }
    public string TipoMembresia { get; set; }
    public int Nivel { get; set; } // 1-5 según experiencia
    public int ContadorPenalizaciones { get; set; }
    public List<Reserva> Reservas { get; set; }
    public Gimnasio Gimnasio { get; set; }

    public Socio(string id, string nombre, string apellido, string telefono, string tipoMembresia, int nivel)
    {
        Id = id;
        Nombre = nombre;
        Apellido = apellido;
        Telefono = telefono;
        TipoMembresia = tipoMembresia;
        Nivel = nivel;
        FechaInscripcion = DateTime.Now;
        Reservas = new List<Reserva>();
        ContadorPenalizaciones = 0;
    }

    public bool PuedeReservar()
    {
        return GetReservasActivas().Count < GetLimiteReservas() && 
               ContadorPenalizaciones < 3;
    }

    public bool PuedeTomarClase(string dificultad)
    {
        switch (dificultad.ToUpper())
        {
            case "PRINCIPIANTE": return true;
            case "INTERMEDIO": return Nivel >= 2;
            case "AVANZADO": return Nivel >= 4;
            default: return false;
        }
    }

    public List<Reserva> GetReservasActivas()
    {
        return Reservas.Where(r => r.Estado == "ACTIVA").ToList();
    }

    private int GetLimiteReservas()
    {
        switch (TipoMembresia.ToUpper())
        {
            case "BASIC": return 3;
            case "PREMIUM": return 5;
            case "VIP": return 10;
            default: return 0;
        }
    }

    public int IncrementarPenalizacion()
    {
        return ContadorPenalizaciones++;
    }

    public void AgregarReserva(Reserva reserva)
    {
        if (!Reservas.Contains(reserva))
        {
            Reservas.Add(reserva);
            Console.WriteLine($"Reserva agregada al socio: {Nombre}");
        }
    }
}

public class ClaseGrupal
{
    public string Id { get; set; }
    public string Nombre { get; set; }
    public Instructor Instructor { get; set; }
    public int Capacidad { get; set; }
    public string Dificultad { get; set; }
    public DateTime FechaHora { get; set; }
    public int Duracion { get; set; } // en minutos
    public Sala Sala { get; set; }
    public List<Reserva> Reservas { get; set; }

    public ClaseGrupal(string id, string nombre, Instructor instructor, int capacidad, 
                      string dificultad, DateTime fechaHora, int duracion)
    {
        Id = id;
        Nombre = nombre;
        Instructor = instructor;
        Capacidad = capacidad;
        Dificultad = dificultad;
        FechaHora = fechaHora;
        Duracion = duracion;
        Reservas = new List<Reserva>();
    }

    public bool HayCupoDisponible()
    {
        return GetReservasActivas().Count < Capacidad;
    }

    public bool CumpleRequisitos(Socio socio)
    {
        return socio.PuedeTomarClase(Dificultad) && 
               Instructor.EstaDisponible(FechaHora) &&
               (Sala != null && Sala.TieneEquipamientoNecesario(GetTipoEquipamientoRequerido()));
    }

    private string GetTipoEquipamientoRequerido()
    {
        switch (Nombre.ToUpper())
        {
            case "SPINNING": return "Bicicleta Spinning";
            case "YOGA": return "Mat";
            case "CROSSFIT": return "Pesas";
            case "CROSSFIT INTENSO": return "Pesas";
            default: return "General";
        }
    }

    public void AgregarReserva(Reserva reserva)
    {
        if (!Reservas.Contains(reserva))
        {
            Reservas.Add(reserva);
            Console.WriteLine($"Reserva agregada a la clase: {Nombre}");
        }
    }

    public List<Reserva> GetReservasActivas()
    {
        return Reservas.Where(r => r.Estado == "ACTIVA").ToList();
    }
}

public class Reserva
{
    public string Id { get; set; }
    public Socio Socio { get; set; }
    public ClaseGrupal Clase { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaCancelacion { get; set; }
    public string Estado { get; set; }
    public bool Asistio { get; set; }

    public Reserva(Socio socio, ClaseGrupal clase)
    {
        if (!socio.PuedeReservar())
        {
            throw new InvalidOperationException("El socio no puede realizar más reservas");
        }
        if (!clase.HayCupoDisponible())
        {
            throw new InvalidOperationException("No hay cupo disponible");
        }
        if (!clase.CumpleRequisitos(socio))
        {
            throw new InvalidOperationException("El socio no cumple los requisitos para esta clase");
        }

        Id = GenerarId();
        Socio = socio;
        Clase = clase;
        FechaCreacion = DateTime.Now;
        Estado = "ACTIVA";
        Asistio = false;
        
        socio.AgregarReserva(this);
        clase.AgregarReserva(this);
    }

    public void Cancelar()
    {
        if (Estado == "ACTIVA")
        {
            TimeSpan diferencia = Clase.FechaHora - DateTime.Now;
            double horasRestantes = diferencia.TotalHours;
            
            if (horasRestantes >= 12)
            {
                Estado = "CANCELADA";
                FechaCancelacion = DateTime.Now;
            }
            else
            {
                Socio.IncrementarPenalizacion();
                throw new InvalidOperationException("Cancelación tardía - penalización aplicada");
            }
        }
    }

    private string GenerarId()
    {
        return "RES-" + DateTime.Now.Ticks;
    }
}

public class PoliticaCancelacion
{
    public int HorasMinimas { get; set; }
    public int MaxPenalizaciones { get; set; }
    public double TarifaCancelacionTardia { get; set; }

    public PoliticaCancelacion(int horasMinimas, int maxPenalizaciones)
    {
        HorasMinimas = horasMinimas;
        MaxPenalizaciones = maxPenalizaciones;
        TarifaCancelacionTardia = 10.0; // USD
    }

    public bool PuedeCancelar(Socio socio)
    {
        return socio.ContadorPenalizaciones < MaxPenalizaciones;
    }

    public double CalcularTarifaCancelacion(DateTime fechaReserva)
    {
        TimeSpan diferencia = fechaReserva - DateTime.Now;
        double horas = diferencia.TotalHours;
        return horas < HorasMinimas ? TarifaCancelacionTardia : 0.0;
    }
}
