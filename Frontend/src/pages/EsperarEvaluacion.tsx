import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { getEvaluacion, Evaluacion } from '../service/EvaluacionService';
import { Box, Typography } from '@mui/material';
import { updateUsuarioById } from '../service/UsuarioService';
import { executeTask, getNextTaskId } from '../service/bonitaService';
import Button from '../components/Button';

const EsperarEvaluacion: React.FC = () => {
  const [evaluacion, setEvaluacion] = useState<Evaluacion | null>(null);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchEvaluacion = async () => {
      try {
        const caseId = localStorage.getItem('caseId');
        if (caseId) {
          const evaluacionData = await getEvaluacion(caseId);
          if (evaluacionData.state != 'ENV') {
            setEvaluacion(evaluacionData);
          }
        }
      } catch (error) {
        console.error('Error fetching evaluacion:', error);
        setError('Error fetching evaluacion');
      }
    };

    fetchEvaluacion();
  }, []);

  const handleLeidoEvaluacion = async () => {
    try {
      const caseId = localStorage.getItem('caseId');
      if (caseId) {
        const nextTaskId = await getNextTaskId(caseId);
        await executeTask(nextTaskId);

        const userId = localStorage.getItem('idUser');
        if (userId) {
          await updateUsuarioById(Number(userId), 0, false, 'R', false, 0);
        }

        navigate('/comenzar-recoleccion');
      }
    } catch (error) {
      console.error('Error processing evaluation read confirmation:', error);
      setError('Error processing evaluation read confirmation');
    }
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-gray-100 p-6">
      
      {error && <p className="text-red-500">{error}</p>}
      {evaluacion ? (
        
          <div className="max-w-lg mx-auto p-6 border rounded-lg shadow bg-white">
      <h2 className="text-2xl font-bold text-gray-800 mb-4">Evaluación de las últimas dos semanas</h2>
      <br />
      <div className="space-y-4">
        <div className="flex justify-between items-center border-b pb-2">
          <strong className="text-gray-700">Cantidad de Órdenes Recolectadas:</strong>
          <span className="text-gray-900">{evaluacion.cantOrdenes}</span>
        </div>
        <div className="flex justify-between items-center border-b pb-2">
          <strong className="text-gray-700">Cantidad de Órdenes mal pesadas:</strong>
          <span className="text-red-500">{evaluacion.cantOrdenesMal}</span>
        </div>
        <div className="flex justify-between items-center border-b pb-2">
          <strong className="text-gray-700">Cantidad de Órdenes bien pesadas:</strong>
          <span className="text-green-500">{evaluacion.cantOrdenesOk}</span>
        </div>
        <div>
          <strong className="text-gray-700 block mb-2">Observaciones:</strong>
          <p className="bg-gray-100 text-gray-800 p-3 rounded whitespace-pre-wrap break-words">
            {evaluacion.observaciones || "Sin observaciones"}
          </p>
        </div>
      </div>
      <br />
          <Button onClick={handleLeidoEvaluacion} color="blue">
            He leído mi evaluación
          </Button>
    </div>
      ) : (
        <div className="bg-white p-8 rounded-md shadow-md w-full max-w-md text-center">
        <Typography variant="body1" gutterBottom component="div" style={{ fontWeight: 'bold' }}>
          Espere a recibir la evaluación de su rendimiento.
        </Typography>
        <Typography variant="body1" gutterBottom component="div" style={{ fontWeight: 'bold' }}>
          Esto puede tardar unos días.
        </Typography>
      </div>
    )}
    </div>
  );
};

export default EsperarEvaluacion;