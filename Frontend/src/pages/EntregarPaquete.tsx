import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Button from '../components/Button';
import { updatePaquete } from '../service/paquetesService';
import { executeTask, getNextTaskId, assignTask, getUsuarioIdByUsername } from '../service/bonitaService';

const EntregarPaquete: React.FC = () => {
  const [isSubmitting, setIsSubmitting] = useState(false);
  const navigate = useNavigate();

  const handleEntregaRealizada = async () => {
    setIsSubmitting(true);
    try {
      const paqueteId = localStorage.getItem('paqueteId');
      if (paqueteId) {
        await updatePaquete(Number(paqueteId), 'ENV');
        console.log(`Paquete ${paqueteId} actualizado a 'ENV'`);
      }

      const nextTaskId = localStorage.getItem('nextTaskId');
      if (nextTaskId) {
        await executeTask(nextTaskId);
      }

      const caseId = localStorage.getItem('caseId');
      if (caseId) {
        await new Promise(resolve => setTimeout(resolve, 2000));
        const nextTask = await getNextTaskId(caseId);
        console.log('Next task:', nextTask);

        const bonitaUserAdmin = await getUsuarioIdByUsername('william.jobs');
        if (bonitaUserAdmin) {
          await assignTask(nextTask, bonitaUserAdmin.toString());
          localStorage.setItem('nextTaskId', nextTask);
        }
      }

      navigate('/esperar-cobro');
    } catch (error) {
      console.error('Error al procesar la entrega:', error);
      alert('Hubo un problema al procesar la entrega');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-green-50 p-6">
      <div className="bg-white p-8 rounded-md shadow-md w-full max-w-md text-center">
        <h2 className="text-2xl font-bold mb-6">Entrega de Paquete</h2>
        <p className="mb-6">No te olvides indicar que has entregado los paquetes.</p>
        <Button onClick={handleEntregaRealizada} disabled={isSubmitting} color="green">
          {isSubmitting ? 'Procesando...' : 'Entrega realizada'}
        </Button>
      </div>
    </div>
  );
};

export default EntregarPaquete;