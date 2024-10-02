// src/pages/CargarRecoleccion.tsx
import React, { useState } from 'react';
import { motion } from 'framer-motion'; // Librería para animaciones
import PlanForm from '../components/PlanForm';
import { iniciarSesionBonita } from '../service/bonitaService'; // Servicio simulado

const CargarRecoleccion: React.FC = () => {
  const [mostrarFormulario, setMostrarFormulario] = useState(false); // Controla si se muestra el formulario
  const [loading, setLoading] = useState(false); // Controla el estado de carga

  const handleComenzarRecoleccion = async () => {
    setLoading(true); // Inicia el estado de carga mientras se simula la llamada a la API
    try {
      // Simulamos el inicio de sesión en Bonita
      await iniciarSesionBonita();
      // Mostrar el formulario después de la llamada exitosa
      setMostrarFormulario(true);
    } catch (error) {
      console.error('Error simulando autenticación en Bonita:', error);
    } finally {
      setLoading(false); // Detiene el estado de carga después de la simulación
    }
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-green-50 p-6">
      {!mostrarFormulario ? (
        <>
          <h1 className="text-4xl font-bold text-green-800 mb-6">Comenzar proceso de recolección</h1>
          <button
            onClick={handleComenzarRecoleccion}
            className="bg-green-600 text-white p-4 rounded-md hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500"
            disabled={loading} // Desactiva el botón mientras simula la autenticación
          >
            {loading ? 'Autenticando...' : 'Comenzar'} {/* Muestra un estado de carga */}
          </button>
        </>
      ) : (
        <motion.div
          initial={{ opacity: 0, y: -50 }} // Configuración inicial de la animación
          animate={{ opacity: 1, y: 0 }} // La animación hace que el formulario se muestre suavemente
          transition={{ duration: 0.5 }} // Tiempo que tarda en mostrarse el formulario
        >
          <PlanForm />
        </motion.div>
      )}
    </div>
  );
};

export default CargarRecoleccion;
