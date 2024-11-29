import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getNextTaskId, executeTask, assignTask } from '../service/bonitaService';
import { updatePaquete, getPaqueteByCaseId } from '../service/paquetesService';
import { getUsuarioByUsername } from '../service/UsuarioService';
import Button from '../components/Button';
import { addNotificacionPago } from '../service/notificacionPagoService';

interface NotificacionPago {
    CaseId: number;
    Cantidad: number;
  }

const RegistrarResultado: React.FC = () => {
  const { caseId } = useParams<{ caseId: string }>();
  const [pago, setPago] = useState<number | null>(null);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchNextTask = async () => {
      try {
        if (caseId) {
          const nextTask = await getNextTaskId(caseId);
          localStorage.setItem('nextTaskId', nextTask);
          console.log('Next task:', nextTask);
        }
      } catch (error) {
        console.error('Error fetching next task:', error);
        setError('Error fetching next task');
      }
    };

    fetchNextTask();
  }, [caseId]);

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    try {
      console.log('Case ID:', caseId, "Pago:", pago);
      const data = { "CaseId": Number(caseId), "Cantidad": pago! };
      await addNotificacionPago(data);      
    
      const nextTaskId = localStorage.getItem('nextTaskId');
      if (nextTaskId) {
        await executeTask(nextTaskId);
      }

      if (caseId) {
        const paquete = await getPaqueteByCaseId(caseId);
        await updatePaquete(paquete.id, 'FIN');
      }

      const usuario = await getUsuarioByUsername('walter.bates');
      await new Promise(resolve => setTimeout(resolve, 2000));
      const nextTask = await getNextTaskId(usuario.caseId.toString());
      await assignTask(nextTask, usuario.id.toString());

      navigate('/paquetes'); // Replace with the actual route you want to navigate to
    } catch (error) {
      console.error('Error processing payment notification:', error);
      setError('Error processing payment notification');
    }
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-gray-100 p-6">
      <h2 className="text-2xl font-bold mb-6">Registrar Resultado</h2>
      {error && <p className="text-red-500">{error}</p>}
      <form onSubmit={handleSubmit} className="bg-white p-8 rounded-md shadow-md w-full max-w-md">
        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700">Ingrese el pago correspondiente al usuario</label>
          <input
            type="number"
            value={pago || ''}
            onChange={(e) => setPago(Number(e.target.value))}
            className="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-green-500 focus:border-green-500 p-2"
            required
          />
        </div>
        <Button type="submit" color="blue">
          Enviar notificación de pago
        </Button>
      </form>
    </div>
  );
};

export default RegistrarResultado;