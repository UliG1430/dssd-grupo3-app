import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import {
  getProcessId,
  startProcessById,
  getNextTaskId,
  assignTask,
} from '../service/bonitaService';

const HomeRedGlobal: React.FC = () => {
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem('redGlobalToken');
    if (!token) {
      navigate('/login-red-global');
    }
  }, [navigate]);

  const userRole = localStorage.getItem('userRol'); // Obtiene el rol del usuario desde localStorage

  const handleConsult = async () => {
    try {
      const username = localStorage.getItem('redGlobalUsername');
      const bonitaToken = localStorage.getItem('bonitaToken');

      // Verifica que los datos requeridos existan en localStorage
      if (!bonitaToken || !username) {
        toast.error('No estás autenticado.');
        return;
      }

      const processName = 'Proceso API'

      // Llama a los servicios de Bonita
      const processId = await getProcessId(processName);
      const caseId = await startProcessById(processId);
      await new Promise(resolve => setTimeout(resolve, 2000));
      localStorage.setItem('caseId', caseId);
      const nextTaskId = await getNextTaskId(caseId);
      localStorage.setItem('nextTaskId', nextTaskId);
      // Asigna la tarea al usuario actual
      await assignTask(nextTaskId, localStorage.getItem('idUserBonita')!);

      // Muestra un mensaje de éxito
      toast.success('Tarea asignada correctamente.');

      // Redirige al usuario según su rol
      navigate(userRole === 'A' ? '/necesidades' : '/ordenes-distribucion');
    } catch (error) {
      // Muestra un mensaje de error si algo falla
      toast.error('Hubo un problema durante la consulta.');
      console.error('Error en handleConsult:', error);
    }
  };

  return (
    <div className="min-h-screen bg-blue-100">
      <ToastContainer />
      <div className="container mx-auto p-8">
        <h1 className="text-3xl font-bold text-center text-blue-800 mb-6">
          Bienvenido a la Red Global de Recicladores
        </h1>
        <div className="mt-6 text-center">
          <button
            onClick={handleConsult}
            className="bg-green-600 text-white px-4 py-2 rounded-md hover:bg-green-700 transition"
          >
            {userRole === 'A'
              ? 'Consultar Necesidades'
              : 'Consultar Órdenes de Distribución'}
          </button>
        </div>
      </div>
    </div>
  );
};

export default HomeRedGlobal; 