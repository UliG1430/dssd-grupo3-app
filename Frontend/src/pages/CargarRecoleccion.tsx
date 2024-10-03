import React, { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import ZonaSelector from '../components/ZonaSelector';
import PlanForm from '../components/PlanForm';
import { loginBonita, getProcessId, startProcessById } from '../service/bonitaService';

const CargarRecoleccion: React.FC = () => {
  const [tokenGuardado, setTokenGuardado] = useState(false);
  const [token, setToken] = useState<string | null>(null);
  const [zonaSeleccionada, setZonaSeleccionada] = useState<string | null>(null);
  const [processId, setProcessId] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const tokenGuardado = localStorage.getItem('bonitaToken');
    if (tokenGuardado) {
      setToken(tokenGuardado);
      setTokenGuardado(true);
    }
  }, []);

  const handleComenzarRecoleccion = async () => {
    try {
      setError(null);
      console.log('Iniciando el proceso de login en Bonita...');
      const data = await loginBonita();
      if (data && data.token) {
        localStorage.setItem('bonitaToken', data.token);
        setToken(data.token);
        setTokenGuardado(true);

        // Obtener el processId
        console.log('Obteniendo el processId...');
        const processIdData = await getProcessId('Proceso de recolección', data.token);
        if (processIdData && processIdData.processId) {
          setProcessId(processIdData.processId);  // Guardar processId
          console.log('Process ID recibido:', processIdData.processId);
        } else {
          setError('No se pudo obtener el ID del proceso.');
        }
      }
    } catch (error) {
      console.error('Error iniciando sesión o obteniendo el processId:', error);
      setError('Error iniciando sesión o obteniendo el ID del proceso.');
    }
  };

  const handleProcessIdReceived = async (processId: string) => {
    try {
      if (!token) {
        throw new Error('Token no encontrado');
      }
      const processInstance = await startProcessById(processId, token);
      console.log('Proceso iniciado:', processInstance);
    } catch (error) {
      console.error('Error al iniciar el proceso:', error);
      setError('Error al iniciar el proceso.');
    }
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-green-50 p-6">
      {error && <p className="text-red-500">{error}</p>}
      {!tokenGuardado ? (
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
        processId ? ( // Solo mostramos ZonaSelector si ya tenemos el processId
          <ZonaSelector
            onZonaSeleccionada={setZonaSeleccionada}
            onProcessIdReceived={handleProcessIdReceived}
            processId={processId}
            token={token!}
          />
        ) : (
          <p className="text-gray-500">Cargando proceso de recolección...</p>
        )
      ) : (
        <motion.div
          initial={{ opacity: 0, y: -50 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.5 }}
        >
          <PlanForm zona={zonaSeleccionada!} processId={processId!} />
        </motion.div>
      )}
    </div>
  );
};

export default CargarRecoleccion;
