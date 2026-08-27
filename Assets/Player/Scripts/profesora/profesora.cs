using UnityEngine;

public class ProfessoraExplicando : MonoBehaviour
{
    [Header("Objetivo de Mirada")]
    public Transform objetivoMirada; // Arrastra aquí a tu 'player' si quieres que lo mire

    [Header("Configuración de Gestos")]
    public Transform manoDerecha;
    public Transform manoIzquierda;
    public Transform cabeza;

    public float velocidadGesticulacion = 3.0f;
    public float intensidadMovimiento = 15.0f;

    private Quaternion rotInicialManoDer;
    private Quaternion rotInicialManoIzamb;
    private Quaternion rotInicialCabeza;

    void Start()
    {
        if (manoDerecha) rotInicialManoDer = manoDerecha.localRotation;
        if (manoIzquierda) rotInicialManoIzamb = manoIzquierda.localRotation;
        if (cabeza) rotInicialCabeza = cabeza.localRotation;
    }

    void Update()
    {
        Gesticular();

        if (objetivoMirada != null && cabeza != null)
        {
            MirarJugador();
        }
    }

    void Gesticular()
    {
        float tiempo = Time.time * velocidadGesticulacion;

        // Movimiento sutil de la mano derecha (explicando)
        if (manoDerecha != null)
        {
            float anguloX = Mathf.Sin(tiempo) * intensidadMovimiento;
            float anguloY = Mathf.Cos(tiempo * 0.8f) * (intensidadMovimiento * 0.5f);
            manoDerecha.localRotation = rotInicialManoDer * Quaternion.Euler(anguloX, anguloY, 0);
        }

        // Movimiento complementario de la mano izquierda
        if (manoIzquierda != null)
        {
            float anguloX = Mathf.Cos(tiempo * 0.7f) * (intensidadMovimiento * 0.4f);
            manoIzquierda.localRotation = rotInicialManoIzamb * Quaternion.Euler(anguloX, 0, 0);
        }
    }

    void MirarJugador()
    {
        // Dirección hacia el jugador
        Vector3 direccion = objetivoMirada.position - cabeza.position;
        if (direccion != Vector3.zero)
        {
            Quaternion rotBuscada = Quaternion.LookRotation(direccion);
            cabeza.rotation = Quaternion.Slerp(cabeza.rotation, rotBuscada, Time.deltaTime * 2.0f);
        }
    }
}