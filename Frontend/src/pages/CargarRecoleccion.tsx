import React, { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import ZonaSelector from '../components/ZonaSelector';
import PlanForm from '../components/PlanForm';
import { loginBonita } from '../service/bonitaService';

const CargarRecoleccion: React.FC = () => {
  const [mostrarZonaSelector, setMostrarZonaSelector] = useState(false); // Mostrar selector de zona
  const [mostrarFormulario, setMostrarFormulario] = useState(false);    // Mostrar formulario
  const [zonaSeleccionada, setZonaSeleccionada] = useState<string | null>(null); // Zona seleccionada

  // Verificar si ya hay un token en sessionStorage al cargar la página
  useEffect(() => {
    const token = sessionStorage.getItem('bonitaToken');
    if (token) {
      setMostrarZonaSelector(true); // Si hay token, mostrar el selector de zonas
    }
  }, []);

  const handleComenzarRecoleccion = async () => {
    try {
      console.log('Iniciando el proceso de login en Bonita...');
      const data = await loginBonita();
      if (data && data.token) {
        sessionStorage.setItem('bonitaToken', data.token); // Guardar en sessionStorage
        setMostrarZonaSelector(true); // Cambiar a la siguiente etapa
      }
    } catch (error) {
      console.error('Error iniciando sesión en Bonita:', error);
    }
  };

  const handleZonaSeleccionada = (zona: string) => {
    setZonaSeleccionada(zona);
    setMostrarFormulario(true); // Al seleccionar la zona, mostrar el formulario
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-green-50 p-6">
      {!mostrarZonaSelector ? (
        <>
          <h1 className="text-4xl font-bold text-green-800 mb-6">Comenzar proceso de recolección</h1>
          <button
            onClick={handleComenzarRecoleccion}
            className="bg-green-600 text-white p-4 rounded-md hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500"
          >
            Comenzar
          </button>
        </>
      ) : !mostrarFormulario ? (
        <ZonaSelector onZonaSeleccionada={handleZonaSeleccionada} /> // Mostrar selector de zonas
      ) : (
        zonaSeleccionada && (
          <motion.div
            initial={{ opacity: 0, y: -50 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5 }}
          >
            <PlanForm zona={zonaSeleccionada} />  {/* Muestra el formulario */}
          </motion.div>
        )
      )}
    </div>
  );
};

export default CargarRecoleccion;
