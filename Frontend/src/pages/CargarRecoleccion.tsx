import React, { useState, useEffect } from 'react';
import { motion } from 'framer-motion'; // Librería para animaciones
import PlanForm from '../components/PlanForm';
import ZonaSelector from '../components/ZonaSelector';
import { loginBonita, getProcessId } from '../service/bonitaService'; // Servicios de Bonita

const CargarRecoleccion: React.FC = () => {
  const [mostrarFormulario, setMostrarFormulario] = useState(false);
  const [zonaSeleccionada, setZonaSeleccionada] = useState<string | null>(null);
  const [processId, setProcessId] = useState<string | null>(null);

  useEffect(() => {
    console.log('Revisando token en localStorage en la carga inicial...');
    const token = localStorage.getItem('bonitaToken');
    if (token) {
      console.log('Token encontrado en localStorage:', token);
      setMostrarFormulario(false); // Esto cambia según el estado de zona
    }
  }, []);

  const handleComenzarRecoleccion = async () => {
    try {
      console.log('Iniciando el proceso de login en Bonita...');
      const data = await loginBonita();
      console.log('Login exitoso, token recibido:', data.token);
      
      if (data && data.token) {
        // Guardar el token en localStorage
        localStorage.setItem('bonitaToken', data.token);
        console.log('Token almacenado en localStorage:', data.token);
        // Cambiar el estado para avanzar al selector de zonas
        setMostrarFormulario(true);  // Cambiamos aquí a true para mostrar el selector de zona
      }
    } catch (error) {
      console.error('Error iniciando sesión en Bonita:', error);
    }
  };

  const handleZonaSeleccionada = (zona: string) => {
    setZonaSeleccionada(zona);
  };

  const handleProcessIdReceived = async (processName: string) => {
    try {
      const id = await getProcessId(processName);
      setProcessId(id);
      setMostrarFormulario(true);
    } catch (error) {
      console.error('Error obteniendo el Process ID:', error);
    }
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-green-50 p-6">
      {!mostrarFormulario && !zonaSeleccionada ? (
        <>
          <h1 className="text-4xl font-bold text-green-800 mb-6">Comenzar proceso de recolección</h1>
          <button
            onClick={handleComenzarRecoleccion}
            className="bg-green-600 text-white p-4 rounded-md hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500"
          >
            Comenzar
          </button>
        </>
      ) : !zonaSeleccionada ? (
        <ZonaSelector
          onZonaSeleccionada={handleZonaSeleccionada}
          onProcessIdReceived={handleProcessIdReceived}
        />
      ) : (
        <motion.div
          initial={{ opacity: 0, y: -50 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.5 }}
        >
          {processId && <PlanForm zona={zonaSeleccionada} processId={processId} />}
        </motion.div>
      )}
    </div>
  );
};

export default CargarRecoleccion;
