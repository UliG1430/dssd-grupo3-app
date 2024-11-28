import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { getProcessId, startProcessById, getNextTaskId, assignTask, executeTask, setCaseVariable } from '../service/bonitaService';
import { ClipLoader } from 'react-spinners';
import { addPaquete, getPaqueteByCaseId } from '../service/paquetesService';
import { updateUsuarioById } from '../service/UsuarioService';

const StartRecoleccion: React.FC = () => {
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const handleStartRecoleccion = async () => {
    setIsLoading(true);
    try {
      const processId = await getProcessId('recoleccion');
      const caseId = await startProcessById(processId);
      console.log('Case ID:', caseId);
      localStorage.setItem('caseId', caseId.toString());
      await updateUsuarioById(Number(localStorage.getItem('idUser')!), caseId, true, 'R', false, 1);
      await addPaquete(caseId, 'CRE');
      const paquete = await getPaqueteByCaseId(caseId.toString());
      localStorage.setItem('paqueteId', paquete.id.toString());
      // Add a delay before calling getNextTask
      await new Promise(resolve => setTimeout(resolve, 2000));

      const nextTask = await getNextTaskId(caseId);
      console.log('Next task:', nextTask);
      const bonitaUserId = localStorage.getItem('idUserBonita');
      await assignTask(nextTask, bonitaUserId!);
      navigate('/visitar-punto');
    } catch (error) {
      console.error('Error al iniciar la recolección:', error);
      alert('Hubo un problema al iniciar la recolección');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-green-50">
      <button
        onClick={handleStartRecoleccion}
        className="bg-green-600 text-white p-4 rounded-md hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500"
        disabled={isLoading}
        style={{ width: '200px' }} // Set a fixed width for the button
      >
        <div className="flex items-center justify-center">
          {isLoading ? <ClipLoader color="#ffffff" size={24} /> : 'Comenzar recolección'}
        </div>
      </button>
    </div>
  );
};

export default StartRecoleccion;