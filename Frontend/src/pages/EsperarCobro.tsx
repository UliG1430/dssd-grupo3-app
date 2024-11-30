import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Button from '../components/Button';
import { Box, Typography } from '@mui/material';
import { getNotificacionPagoByCaseId } from '../service/notificacionPagoService';
import { updateUsuarioById } from '../service/UsuarioService';
import { getNextTaskId, executeTask, setCaseVariable, assignTask, getUsuarioIdByUsername } from '../service/bonitaService';
import { parseISO, subDays, isBefore } from 'date-fns';
import { updateUltimaNotificacion, getUltimaNotificacion } from '../service/UltimaNotificacionService';
import { addEvaluacion } from '../service/EvaluacionService';

const EsperarCobro: React.FC = () => {
  const [notificacionPago, setNotificacionPago] = useState<{ cantidad: number } | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [evaluationDone, setEvaluationDone] = useState<boolean>(true);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchNotificacionPago = async () => {
      try {
        const caseId = localStorage.getItem('caseId');
        if (caseId) {
          const notificacion = await getNotificacionPagoByCaseId(caseId);
          if (notificacion) {
            setNotificacionPago(notificacion);
            const ultimaNotificacion = await getUltimaNotificacion();
            if (ultimaNotificacion) {
              //console.log('Ultima notificacion:', ultimaNotificacion);
              const ultimaEvaluacionDate = parseISO(ultimaNotificacion.fecha);
              const twoWeeksAgo = subDays(new Date(), 14);

              if (isBefore(ultimaEvaluacionDate, twoWeeksAgo)) {
                setEvaluationDone(false);
              }
            }
          }
        }
      } catch (error) {
        console.error('Error fetching notificacion pago:', error);
        setError('Error fetching notificacion pago');
      }
    };

    fetchNotificacionPago();
  }, []);

  const handleConfirmarNotificacion = async () => {
    try {
      const caseId = localStorage.getItem('caseId');
      if (caseId) {
        const nextTaskId = await getNextTaskId(caseId);
        if (evaluationDone) {
            await setCaseVariable(caseId, 'evaluationDone', true);
        } else {
            await setCaseVariable(caseId, 'evaluationDone', false);
        }
        
        await executeTask(nextTaskId);
        await new Promise(resolve => setTimeout(resolve, 2000));
        const nextTask = await getNextTaskId(caseId);
        const userId = localStorage.getItem('idUser');
        if (userId) {
          if (evaluationDone) {
            await updateUsuarioById(Number(userId), 0, false, 'R', false, 0);
          } else {
            const bonitaUserAdmin = await getUsuarioIdByUsername('william.jobs');
            await addEvaluacion(caseId);
            if (bonitaUserAdmin) {
                await assignTask(nextTask, bonitaUserAdmin.toString());
                localStorage.setItem('nextTaskId', nextTask);
            }
          }
        }

        if (evaluationDone) {
            navigate('/comenzar-recoleccion');
        } else {
            navigate('/esperar-evaluacion');
        }
      }
    } catch (error) {
      console.error('Error confirming payment notification:', error);
      setError('Error confirming payment notification');
    }
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-gray-100 p-6">
      
      {error && <p className="text-red-500">{error}</p>}
      {notificacionPago ? (
        <div className="bg-white p-8 rounded-md shadow-md w-full max-w-md text-center">
          <p className="mb-4">Cantidad: {notificacionPago.cantidad}</p>
          <Button onClick={handleConfirmarNotificacion} color="blue">
            Confirmo haber recibido la notificación del pago
          </Button>
        </div>
      ) : (
        <Box
            display="flex"
            flexDirection="column"
            justifyContent="center"
            alignItems="center"
            height="100vh"
            textAlign="center"
        >
            <Typography variant="h4" gutterBottom>
                Espere a recibir la notificación de cobro
            </Typography>
            <Typography variant="subtitle1" color="textSecondary">
                Esto puede tardar algunos días
            </Typography>
        </Box>
      )}
    </div>
  );
};

export default EsperarCobro;