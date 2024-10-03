// src/pages/CargarRecoleccion.tsx
import React, { useState } from 'react';
import { motion } from 'framer-motion'; // Librería para animaciones
import PlanForm from '../components/PlanForm';
import { loginBonita } from '../service/bonitaService'; // El servicio para el login de Bonita

const CargarRecoleccion: React.FC = () => {
  const [mostrarFormulario, setMostrarFormulario] = useState(false);

  const handleComenzarRecoleccion = async () => {
    try {
      // Llamada al backend para iniciar sesión en Bonita
      await loginBonita();
      // Mostrar el formulario después de la llamada
      setMostrarFormulario(true);
    } catch (error) {
      console.error('Error iniciando sesión en Bonita:', error);
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
          >
            Comenzar
          </button>
        </>
      ) : (
        <motion.div
          initial={{ opacity: 0, y: -50 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.5 }}
        >
          <PlanForm />
        </motion.div>
      )}
    </div>
  );
};

export default CargarRecoleccion;
